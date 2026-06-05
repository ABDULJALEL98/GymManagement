using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Subscriptions.Queries.GetAllSubscriptions;

public class GetAllSubscriptionsQueryHandler
    : IRequestHandler<GetAllSubscriptionsQuery, Result<PagedResult<SubscriptionDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllSubscriptionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PagedResult<SubscriptionDto>>> Handle(
        GetAllSubscriptionsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Subscriptions
            .Query()
            .AsNoTracking()
            .Include(x => x.Member)
            .Include(x => x.SubscriptionPlan)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.Trim();

            query = query.Where(x =>
                x.Member.FullName.Contains(searchTerm) ||
                x.SubscriptionPlan.Name.Contains(searchTerm));
        }

        if (request.MemberId.HasValue)
        {
            query = query.Where(x => x.MemberId == request.MemberId.Value);
        }

        if (request.SubscriptionPlanId.HasValue)
        {
            query = query.Where(x => x.SubscriptionPlanId == request.SubscriptionPlanId.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(x => x.StartDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(x => x.StartDate <= request.ToDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var subscriptions = await query
            .OrderByDescending(x => x.StartDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new SubscriptionDto
            {
                Id = x.Id,
                MemberId = x.MemberId,
                MemberName = x.Member.FullName,
                SubscriptionPlanId = x.SubscriptionPlanId,
                SubscriptionPlanName = x.SubscriptionPlan.Name,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);

        var pagedResult = PagedResult<SubscriptionDto>.Create(
            subscriptions,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PagedResult<SubscriptionDto>>.Success(pagedResult);
    }
}