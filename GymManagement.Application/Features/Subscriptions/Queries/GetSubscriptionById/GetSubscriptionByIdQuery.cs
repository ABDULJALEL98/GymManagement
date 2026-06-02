using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.Subscriptions.Queries.GetSubscriptionById;

public class GetSubscriptionByIdQuery : IRequest<Result<SubscriptionDto>>
{
    public Guid Id { get; set; }

    public GetSubscriptionByIdQuery(Guid id)
    {
        Id = id;
    }
}