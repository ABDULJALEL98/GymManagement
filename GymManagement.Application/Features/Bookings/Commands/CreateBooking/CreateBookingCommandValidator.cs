using FluentValidation;

namespace GymManagement.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty()
            .WithMessage("Member id is required");

        RuleFor(x => x.GymClassId)
            .NotEmpty()
            .WithMessage("Gym class id is required");
    }
}