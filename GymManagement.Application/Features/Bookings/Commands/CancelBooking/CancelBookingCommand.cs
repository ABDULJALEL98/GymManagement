using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.Bookings.Commands.CancelBooking;

public class CancelBookingCommand : IRequest<Result>
{
    public Guid Id { get; set; }

    public CancelBookingCommand(Guid id)
    {
        Id = id;
    }
}