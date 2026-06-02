using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommand : IRequest<Result<Guid>>
{
    public Guid MemberId { get; set; }

    public Guid SubscriptionPlanId { get; set; }

    public DateTime StartDate { get; set; }
}