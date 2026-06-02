using GymManagement.Application.Common.Models;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Subscriptions.Commands.ChangeSubscriptionStatus;

public class ChangeSubscriptionStatusCommand : IRequest<Result>
{
    public Guid Id { get; set; }

    public SubscriptionStatus Status { get; set; }

    public ChangeSubscriptionStatusCommand(Guid id, SubscriptionStatus status)
    {
        Id = id;
        Status = status;
    }
}