using GymManagement.Domain.Common;
using GymManagement.Domain.Enums;

namespace GymManagement.Domain.Entities;

public class Payment : AuditableEntity
{
    public Guid MemberId { get; set; }

    public Member Member { get; set; } = null!;

    public Guid? SubscriptionId { get; set; }

    public Subscription? Subscription { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    public string PaymentMethod { get; set; } = string.Empty;

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public string? TransactionReference { get; set; }
}