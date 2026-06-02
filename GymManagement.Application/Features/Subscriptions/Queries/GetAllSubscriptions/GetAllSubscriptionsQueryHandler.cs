using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Subscriptions.Queries.GetAllSubscriptions;

public class GetAllSubscriptionsQueryHandler
    : IRequestHandler<GetAllSubscriptionsQuery, Result<List<SubscriptionDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllSubscriptionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<SubscriptionDto>>> Handle(
        GetAllSubscriptionsQuery request,
        CancellationToken cancellationToken)
    {
        var subscriptions = await _unitOfWork.Subscriptions
            .Query()
            .AsNoTracking()
            .Include(x => x.Member)
            .Include(x => x.SubscriptionPlan)
            .OrderByDescending(x => x.StartDate)
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

        return Result<List<SubscriptionDto>>.Success(subscriptions);
    }
}