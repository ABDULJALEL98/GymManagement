using FluentValidation;

namespace GymManagement.Application.Features.Trainers.Commands.UpdateTrainer;

public class UpdateTrainerCommandValidator : AbstractValidator<UpdateTrainerCommand>
{
    public UpdateTrainerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Trainer id is required");

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

        RuleFor(x => x.Specialization)
            .NotEmpty()
            .WithMessage("Specialization is required")
            .MaximumLength(100)
            .WithMessage("Specialization must not exceed 100 characters");

        RuleFor(x => x.YearsOfExperience)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Years of experience must be greater than or equal to zero");
    }
}