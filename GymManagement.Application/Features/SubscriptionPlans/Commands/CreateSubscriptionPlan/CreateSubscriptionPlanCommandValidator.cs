using FluentValidation;

namespace GymManagement.Application.Features.SubscriptionPlans.Commands.CreateSubscriptionPlan;

public class CreateSubscriptionPlanCommandValidator
    : AbstractValidator<CreateSubscriptionPlanCommand>
{
    public CreateSubscriptionPlanCommandValidator()
    {
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