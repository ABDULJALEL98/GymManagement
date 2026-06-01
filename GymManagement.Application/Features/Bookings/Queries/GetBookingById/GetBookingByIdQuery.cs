using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.Bookings.Queries.GetBookingById;

public class GetBookingByIdQuery : IRequest<Result<BookingDto>>
{
    public Guid Id { get; set; }

    public GetBookingByIdQuery(Guid id)
    {
        Id = id;
    }
}