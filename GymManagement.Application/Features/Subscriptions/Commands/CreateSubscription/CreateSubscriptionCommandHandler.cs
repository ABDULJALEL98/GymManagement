using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler
    : IRequestHandler<CreateSubscriptionCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateSubscriptionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(
            request.MemberId,
            cancellationToken);

        if (member is null)
        {
            return Result<Guid>.Failure("Member not found");
        }

        if (!member.IsActive)
        {
            return Result<Guid>.Failure("Member is not active");
        }

        var plan = await _unitOfWork.SubscriptionPlans.GetByIdAsync(
            request.SubscriptionPlanId,
            cancellationToken);

        if (plan is null)
        {
            return Result<Guid>.Failure("Subscription plan not found");
        }

        if (!plan.IsActive)
        {
            return Result<Guid>.Failure("Subscription plan is not active");
        }

        var hasActiveSubscription = await _unitOfWork.Subscriptions.AnyAsync(
            x => x.MemberId == request.MemberId &&
                 x.Status == SubscriptionStatus.Active &&
                 x.EndDate >= DateTime.UtcNow,
            cancellationToken);

        if (hasActiveSubscription)
        {
            return Result<Guid>.Failure("Member already has an active subscription");
        }

        var startDate = request.StartDate;
        var endDate = startDate.AddDays(plan.DurationInDays);

        var subscription = new Subscription
        {
            MemberId = request.MemberId,
            SubscriptionPlanId = request.SubscriptionPlanId,
            StartDate = startDate,
            EndDate = endDate,
            Status = SubscriptionStatus.Active
        };

        await _unitOfWork.Subscriptions.AddAsync(subscription, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(subscription.Id, "Subscription created successfully");
    }
}