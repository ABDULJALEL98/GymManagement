using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.Subscriptions.Commands.DeleteSubscription;

public class DeleteSubscriptionCommandHandler
    : IRequestHandler<DeleteSubscriptionCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSubscriptionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (subscription is null)
        {
            return Result.Failure("Subscription not found");
        }

        _unitOfWork.Subscriptions.Delete(subscription);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Subscription deleted successfully");
    }
}