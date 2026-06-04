using FluentValidation;

namespace GymManagement.Application.Features.Payments.Commands.CreatePayment;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty()
            .WithMessage("Member id is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .WithMessage("Payment method is required")
            .MaximumLength(50)
            .WithMessage("Payment method must not exceed 50 characters");

        RuleFor(x => x.TransactionReference)
            .MaximumLength(150)
            .WithMessage("Transaction reference must not exceed 150 characters");
    }
}