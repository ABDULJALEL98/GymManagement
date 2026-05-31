using FluentValidation;

namespace GymManagement.Application.Features.Members.Commands.CreateMember;

public class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required")
            .MaximumLength(150)
            .WithMessage("Full name must not exceed 150 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .MaximumLength(30)
            .WithMessage("Phone number must not exceed 30 characters");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email is invalid")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.UtcNow)
            .WithMessage("Date of birth must be in the past");

        RuleFor(x => x.Address)
            .MaximumLength(300)
            .WithMessage("Address must not exceed 300 characters");
    }
}