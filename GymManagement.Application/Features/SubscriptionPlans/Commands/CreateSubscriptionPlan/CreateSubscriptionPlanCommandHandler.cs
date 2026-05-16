using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using MediatR;

namespace GymManagement.Application.Features.SubscriptionPlans.Commands.CreateSubscriptionPlan;

public class CreateSubscriptionPlanCommandHandler
    : IRequestHandler<CreateSubscriptionPlanCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateSubscriptionPlanCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateSubscriptionPlanCommand request,
        CancellationToken cancellationToken)
    {
        var nameExists = await _unitOfWork.SubscriptionPlans.AnyAsync(
            x => x.Name == request.Name,
            cancellationToken);

        if (nameExists)
        {
            return Result<Guid>.Failure("Subscription plan name already exists");
        }

        var subscriptionPlan = new SubscriptionPlan
        {
            Name = request.Name,
            Description = request.Description,
            DurationInDays = request.DurationInDays,
            Price = request.Price,
            IsActive = request.IsActive
        };

        await _unitOfWork.SubscriptionPlans.AddAsync(subscriptionPlan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(subscriptionPlan.Id, "Subscription plan created successfully");
    }
}