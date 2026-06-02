using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.Subscriptions.Queries.GetAllSubscriptions;

public class GetAllSubscriptionsQuery : IRequest<Result<List<SubscriptionDto>>>
{
}