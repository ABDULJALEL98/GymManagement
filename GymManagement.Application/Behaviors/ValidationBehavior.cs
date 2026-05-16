using FluentValidation;
using FluentValidation.Results;
using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(validator =>
                validator.ValidateAsync(context, cancellationToken)));

        var errors = validationResults
            .SelectMany(result => result.Errors)
            .Where(error => error is not null)
            .Select(error => error.ErrorMessage)
            .Distinct()
            .ToList();

        if (errors.Count == 0)
        {
            return await next();
        }

        if (typeof(TResponse) == typeof(Result))
        {
            object response = Result.Failure("Validation failed", errors);
            return (TResponse)response;
        }

        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var dataType = typeof(TResponse).GetGenericArguments()[0];

            var failureMethod = typeof(Result<>)
                .MakeGenericType(dataType)
                .GetMethod(
                    nameof(Result<object>.Failure),
                    new[] { typeof(string), typeof(List<string>) });

            if (failureMethod is not null)
            {
                object? response = failureMethod.Invoke(
                    null,
                    new object[] { "Validation failed", errors });

                if (response is not null)
                {
                    return (TResponse)response;
                }
            }
        }

        throw new ValidationException(
    errors.Select(error => new ValidationFailure(string.Empty, error))
);
    }
}