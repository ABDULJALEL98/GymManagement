using FluentValidation;

namespace GymManagement.Application.Features.GymClasses.Commands.CreateGymClass;

public class CreateGymClassCommandValidator : AbstractValidator<CreateGymClassCommand>
{
    public CreateGymClassCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Class name is required")
            .MaximumLength(100)
            .WithMessage("Class name must not exceed 100 characters");

        RuleFor(x => x.TrainerId)
            .NotEmpty()
            .WithMessage("Trainer id is required");

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Start time must be in the future");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be after start time");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than zero");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters");
    }
}