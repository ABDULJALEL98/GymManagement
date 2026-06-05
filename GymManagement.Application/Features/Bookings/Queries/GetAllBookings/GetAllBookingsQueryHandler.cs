using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Bookings.Queries.GetAllBookings;

public class GetAllBookingsQueryHandler
    : IRequestHandler<GetAllBookingsQuery, Result<PagedResult<BookingDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllBookingsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PagedResult<BookingDto>>> Handle(
        GetAllBookingsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Bookings
            .Query()
            .AsNoTracking()
            .Include(x => x.Member)
            .Include(x => x.GymClass)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.Trim();

            query = query.Where(x =>
                x.Member.FullName.Contains(searchTerm) ||
                x.GymClass.Name.Contains(searchTerm));
        }

        if (request.MemberId.HasValue)
        {
            query = query.Where(x => x.MemberId == request.MemberId.Value);
        }

        if (request.GymClassId.HasValue)
        {
            query = query.Where(x => x.GymClassId == request.GymClassId.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(x => x.BookingDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(x => x.BookingDate <= request.ToDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var bookings = await query
            .OrderByDescending(x => x.BookingDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
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

        var pagedResult = PagedResult<BookingDto>.Create(
            bookings,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PagedResult<BookingDto>>.Success(pagedResult);
    }
}