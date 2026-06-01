using GymManagement.Application.Common.Models;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Bookings.Commands.ChangeBookingStatus;

public class ChangeBookingStatusCommand : IRequest<Result>
{
    public Guid Id { get; set; }

    public BookingStatus Status { get; set; }

    public ChangeBookingStatusCommand(Guid id, BookingStatus status)
    {
        Id = id;
        Status = status;
    }
}