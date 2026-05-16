using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.SubscriptionPlans.Commands.CreateSubscriptionPlan;

public class CreateSubscriptionPlanCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int DurationInDays { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;
}