using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Bookings.Queries.GetAllBookings;

public class GetAllBookingsQuery : PagedRequest, IRequest<Result<PagedResult<BookingDto>>>
{
    public Guid? MemberId { get; set; }

    public Guid? GymClassId { get; set; }

    public BookingStatus? Status { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}