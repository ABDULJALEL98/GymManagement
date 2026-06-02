using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Subscriptions.Commands.ChangeSubscriptionStatus;

public class ChangeSubscriptionStatusCommandHandler
    : IRequestHandler<ChangeSubscriptionStatusCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangeSubscriptionStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ChangeSubscriptionStatusCommand request,
        CancellationToken cancellationToken)
    {
        var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (subscription is null)
        {
            return Result.Failure("Subscription not found");
        }

        if (subscription.Status == SubscriptionStatus.Cancelled)
        {
            return Result.Failure("Cancelled subscription cannot be changed");
        }

        if (subscription.Status == SubscriptionStatus.Expired)
        {
            return Result.Failure("Expired subscription cannot be changed");
        }

        subscription.Status = request.Status;
        subscription.UpdatedAtUtc = DateTime.UtcNow;

        _unitOfWork.Subscriptions.Update(subscription);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Subscription status updated successfully");
    }
}