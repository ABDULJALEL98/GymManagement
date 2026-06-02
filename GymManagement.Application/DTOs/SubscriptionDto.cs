using GymManagement.Domain.Enums;

namespace GymManagement.Application.DTOs;

public class SubscriptionDto
{
    public Guid Id { get; set; }

    public Guid MemberId { get; set; }

    public string MemberName { get; set; } = string.Empty;

    public Guid SubscriptionPlanId { get; set; }

    public string SubscriptionPlanName { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public SubscriptionStatus Status { get; set; }
}