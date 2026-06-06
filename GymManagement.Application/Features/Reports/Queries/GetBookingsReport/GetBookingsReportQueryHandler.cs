using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Reports.Queries.GetBookingsReport;

public class GetBookingsReportQueryHandler
    : IRequestHandler<GetBookingsReportQuery, Result<List<BookingsReportDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBookingsReportQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<BookingsReportDto>>> Handle(
        GetBookingsReportQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Bookings
            .Query()
            .AsNoTracking()
            .Include(x => x.Member)
            .Include(x => x.GymClass)
            .ThenInclude(x => x.Trainer)
            .AsQueryable();

        if (request.FromDate.HasValue)
        {
            query = query.Where(x => x.BookingDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(x => x.BookingDate <= request.ToDate.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        var result = await query
            .OrderByDescending(x => x.BookingDate)
            .Select(x => new BookingsReportDto
            {
                BookingId = x.Id,
                MemberName = x.Member.FullName,
                GymClassName = x.GymClass.Name,
                TrainerName = x.GymClass.Trainer.FullName,
                BookingDate = x.BookingDate,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);

        return Result<List<BookingsReportDto>>.Success(result);
    }
}