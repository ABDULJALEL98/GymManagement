using FluentValidation;

namespace GymManagement.Application.Features.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
{
    public CreateSubscriptionCommandValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty()
            .WithMessage("Member id is required");

        RuleFor(x => x.SubscriptionPlanId)
            .NotEmpty()
            .WithMessage("Subscription plan id is required");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Start date must be today or in the future");
    }
}