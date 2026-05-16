using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.SubscriptionPlans.Commands.UpdateSubscriptionPlan;

public class UpdateSubscriptionPlanCommand : IRequest<Result>
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int DurationInDays { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }
}