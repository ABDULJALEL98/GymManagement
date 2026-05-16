using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.SubscriptionPlans.Queries.GetAllSubscriptionPlans;

public class GetAllSubscriptionPlansQuery : IRequest<Result<List<SubscriptionPlanDto>>>
{
}