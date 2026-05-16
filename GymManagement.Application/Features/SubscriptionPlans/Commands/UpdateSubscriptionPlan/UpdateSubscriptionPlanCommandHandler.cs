using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.SubscriptionPlans.Commands.UpdateSubscriptionPlan;

public class UpdateSubscriptionPlanCommandHandler
    : IRequestHandler<UpdateSubscriptionPlanCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSubscriptionPlanCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateSubscriptionPlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _unitOfWork.SubscriptionPlans.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (plan is null)
        {
            return Result.Failure("Subscription plan not found");
        }

        var nameExists = await _unitOfWork.SubscriptionPlans.AnyAsync(
            x => x.Name == request.Name && x.Id != request.Id,
            cancellationToken);

        if (nameExists)
        {
            return Result.Failure("Subscription plan name already exists");
        }

        plan.Name = request.Name;
        plan.Description = request.Description;
        plan.DurationInDays = request.DurationInDays;
        plan.Price = request.Price;
        plan.IsActive = request.IsActive;
        plan.UpdatedAtUtc = DateTime.UtcNow;

        _unitOfWork.SubscriptionPlans.Update(plan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Subscription plan updated successfully");
    }
}