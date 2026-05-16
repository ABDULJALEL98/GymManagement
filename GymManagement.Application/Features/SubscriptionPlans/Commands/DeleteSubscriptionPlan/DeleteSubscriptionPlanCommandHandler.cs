using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.SubscriptionPlans.Commands.DeleteSubscriptionPlan;

public class DeleteSubscriptionPlanCommandHandler
    : IRequestHandler<DeleteSubscriptionPlanCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSubscriptionPlanCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteSubscriptionPlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _unitOfWork.SubscriptionPlans.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (plan is null)
        {
            return Result.Failure("Subscription plan not found");
        }

        _unitOfWork.SubscriptionPlans.Delete(plan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Subscription plan deleted successfully");
    }
}