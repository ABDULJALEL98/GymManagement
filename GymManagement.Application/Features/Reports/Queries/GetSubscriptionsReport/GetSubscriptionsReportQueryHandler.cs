using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Reports.Queries.GetSubscriptionsReport;

public class GetSubscriptionsReportQueryHandler
    : IRequestHandler<GetSubscriptionsReportQuery, Result<List<SubscriptionsReportDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSubscriptionsReportQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<SubscriptionsReportDto>>> Handle(
        GetSubscriptionsReportQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Subscriptions
            .Query()
            .AsNoTracking()
            .Include(x => x.Member)
            .Include(x => x.SubscriptionPlan)
            .AsQueryable();

        if (request.FromDate.HasValue)
        {
            query = query.Where(x => x.StartDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(x => x.StartDate <= request.ToDate.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        var result = await query
            .OrderByDescending(x => x.StartDate)
            .Select(x => new SubscriptionsReportDto
            {
                SubscriptionId = x.Id,
                MemberName = x.Member.FullName,
                PlanName = x.SubscriptionPlan.Name,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);

        return Result<List<SubscriptionsReportDto>>.Success(result);
    }
}