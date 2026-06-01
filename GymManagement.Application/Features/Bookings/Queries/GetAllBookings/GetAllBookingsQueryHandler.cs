using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Bookings.Queries.GetAllBookings;

public class GetAllBookingsQueryHandler
    : IRequestHandler<GetAllBookingsQuery, Result<List<BookingDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllBookingsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<BookingDto>>> Handle(
        GetAllBookingsQuery request,
        CancellationToken cancellationToken)
    {
        var bookings = await _unitOfWork.Bookings
            .Query()
            .AsNoTracking()
            .Include(x => x.Member)
            .Include(x => x.GymClass)
            .OrderByDescending(x => x.BookingDate)
            .Select(x => new BookingDto
            {
                Id = x.Id,
                MemberId = x.MemberId,
                MemberName = x.Member.FullName,
                GymClassId = x.GymClassId,
                GymClassName = x.GymClass.Name,
                BookingDate = x.BookingDate,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);

        return Result<List<BookingDto>>.Success(bookings);
    }
}