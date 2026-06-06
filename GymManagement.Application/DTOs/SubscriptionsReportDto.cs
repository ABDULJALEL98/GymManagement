using GymManagement.Domain.Enums;

namespace GymManagement.Application.DTOs;

public class SubscriptionsReportDto
{
    public Guid SubscriptionId { get; set; }

    public string MemberName { get; set; } = string.Empty;

    public string PlanName { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public SubscriptionStatus Status { get; set; }
}