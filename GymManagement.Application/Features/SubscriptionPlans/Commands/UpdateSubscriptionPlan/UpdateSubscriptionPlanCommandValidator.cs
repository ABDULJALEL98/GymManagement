using FluentValidation;

namespace GymManagement.Application.Features.SubscriptionPlans.Commands.UpdateSubscriptionPlan;

public class UpdateSubscriptionPlanCommandValidator
    : AbstractValidator<UpdateSubscriptionPlanCommand>
{
    public UpdateSubscriptionPlanCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Subscription plan id is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Subscription plan name is required")
            .MaximumLength(100)
            .WithMessage("Subscription plan name must not exceed 100 characters");

        RuleFor(x => x.DurationInDays)
            .GreaterThan(0)
            .WithMessage("Duration must be greater than zero");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be greater than or equal to zero");
    }
}