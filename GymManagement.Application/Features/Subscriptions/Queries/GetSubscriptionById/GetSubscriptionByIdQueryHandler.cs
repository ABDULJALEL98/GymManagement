using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Subscriptions.Queries.GetSubscriptionById;

public class GetSubscriptionByIdQueryHandler
    : IRequestHandler<GetSubscriptionByIdQuery, Result<SubscriptionDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSubscriptionByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SubscriptionDto>> Handle(
        GetSubscriptionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var subscription = await _unitOfWork.Subscriptions
            .Query()
            .AsNoTracking()
            .Include(x => x.Member)
            .Include(x => x.SubscriptionPlan)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (subscription is null)
        {
            return Result<SubscriptionDto>.Failure("Subscription not found");
        }

        var dto = new SubscriptionDto
        {
            Id = subscription.Id,
            MemberId = subscription.MemberId,
            MemberName = subscription.Member.FullName,
            SubscriptionPlanId = subscription.SubscriptionPlanId,
            SubscriptionPlanName = subscription.SubscriptionPlan.Name,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            Status = subscription.Status
        };

        return Result<SubscriptionDto>.Success(dto);
    }
}