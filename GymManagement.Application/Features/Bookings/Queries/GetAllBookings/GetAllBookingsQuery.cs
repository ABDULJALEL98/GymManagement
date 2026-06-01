using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.Bookings.Queries.GetAllBookings;

public class GetAllBookingsQuery : IRequest<Result<List<BookingDto>>>
{
}