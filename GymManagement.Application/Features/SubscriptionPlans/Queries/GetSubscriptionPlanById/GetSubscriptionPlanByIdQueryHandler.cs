using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.SubscriptionPlans.Queries.GetSubscriptionPlanById;

public class GetSubscriptionPlanByIdQueryHandler
    : IRequestHandler<GetSubscriptionPlanByIdQuery, Result<SubscriptionPlanDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSubscriptionPlanByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SubscriptionPlanDto>> Handle(
        GetSubscriptionPlanByIdQuery request,
        CancellationToken cancellationToken)
    {
        var plan = await _unitOfWork.SubscriptionPlans.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (plan is null)
        {
            return Result<SubscriptionPlanDto>.Failure("Subscription plan not found");
        }

        var dto = new SubscriptionPlanDto
        {
            Id = plan.Id,
            Name = plan.Name,
            Description = plan.Description,
            DurationInDays = plan.DurationInDays,
            Price = plan.Price,
            IsActive = plan.IsActive
        };

        return Result<SubscriptionPlanDto>.Success(dto);
    }
}