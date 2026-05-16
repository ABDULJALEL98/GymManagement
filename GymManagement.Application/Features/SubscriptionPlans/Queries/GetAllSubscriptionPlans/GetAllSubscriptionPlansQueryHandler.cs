using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.SubscriptionPlans.Queries.GetAllSubscriptionPlans;

public class GetAllSubscriptionPlansQueryHandler
    : IRequestHandler<GetAllSubscriptionPlansQuery, Result<List<SubscriptionPlanDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllSubscriptionPlansQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<SubscriptionPlanDto>>> Handle(
        GetAllSubscriptionPlansQuery request,
        CancellationToken cancellationToken)
    {
        var plans = await _unitOfWork.SubscriptionPlans.GetAllAsync(cancellationToken);

        var result = plans
            .Select(x => new SubscriptionPlanDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                DurationInDays = x.DurationInDays,
                Price = x.Price,
                IsActive = x.IsActive
            })
            .ToList();

        return Result<List<SubscriptionPlanDto>>.Success(result);
    }
}