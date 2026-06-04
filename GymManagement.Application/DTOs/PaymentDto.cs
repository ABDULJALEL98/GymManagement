using GymManagement.Domain.Enums;

namespace GymManagement.Application.DTOs;

public class PaymentDto
{
    public Guid Id { get; set; }

    public Guid MemberId { get; set; }

    public string MemberName { get; set; } = string.Empty;

    public Guid? SubscriptionId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public PaymentStatus Status { get; set; }

    public string? TransactionReference { get; set; }
}