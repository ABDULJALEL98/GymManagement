using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Reports.Queries.GetNewMembersReport;

public class GetNewMembersReportQueryHandler
    : IRequestHandler<GetNewMembersReportQuery, Result<List<NewMembersReportDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetNewMembersReportQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<NewMembersReportDto>>> Handle(
        GetNewMembersReportQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Members
            .Query()
            .AsNoTracking();

        if (request.FromDate.HasValue)
        {
            query = query.Where(x => x.CreatedAtUtc >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(x => x.CreatedAtUtc <= request.ToDate.Value);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        var result = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new NewMembersReportDto
            {
                MemberId = x.Id,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                CreatedAtUtc = x.CreatedAtUtc,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);

        return Result<List<NewMembersReportDto>>.Success(result);
    }
}