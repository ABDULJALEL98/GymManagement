using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.SubscriptionPlans.Commands.DeleteSubscriptionPlan;

public class DeleteSubscriptionPlanCommand : IRequest<Result>
{
    public Guid Id { get; set; }

    public DeleteSubscriptionPlanCommand(Guid id)
    {
        Id = id;
    }
}