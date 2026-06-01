using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommand : IRequest<Result<Guid>>
{
    public Guid MemberId { get; set; }

    public Guid GymClassId { get; set; }
}