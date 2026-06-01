using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Bookings.Queries.GetBookingById;

public class GetBookingByIdQueryHandler
    : IRequestHandler<GetBookingByIdQuery, Result<BookingDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBookingByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BookingDto>> Handle(
        GetBookingByIdQuery request,
        CancellationToken cancellationToken)
    {
        var booking = await _unitOfWork.Bookings
            .Query()
            .AsNoTracking()
            .Include(x => x.Member)
            .Include(x => x.GymClass)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (booking is null)
        {
            return Result<BookingDto>.Failure("Booking not found");
        }

        var dto = new BookingDto
        {
            Id = booking.Id,
            MemberId = booking.MemberId,
            MemberName = booking.Member.FullName,
            GymClassId = booking.GymClassId,
            GymClassName = booking.GymClass.Name,
            BookingDate = booking.BookingDate,
            Status = booking.Status
        };

        return Result<BookingDto>.Success(dto);
    }
}