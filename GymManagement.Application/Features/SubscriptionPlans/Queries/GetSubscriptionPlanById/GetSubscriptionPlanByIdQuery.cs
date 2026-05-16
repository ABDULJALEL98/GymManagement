using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.SubscriptionPlans.Queries.GetSubscriptionPlanById;

public class GetSubscriptionPlanByIdQuery : IRequest<Result<SubscriptionPlanDto>>
{
    public Guid Id { get; set; }

    public GetSubscriptionPlanByIdQuery(Guid id)
    {
        Id = id;
    }
}