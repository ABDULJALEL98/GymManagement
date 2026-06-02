using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.Subscriptions.Commands.DeleteSubscription;

public class DeleteSubscriptionCommand : IRequest<Result>
{
    public Guid Id { get; set; }

    public DeleteSubscriptionCommand(Guid id)
    {
        Id = id;
    }
}