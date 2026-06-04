using GymManagement.Domain.Enums;

namespace GymManagement.Application.DTOs;

public class RecentPaymentDto
{
    public Guid Id { get; set; }

    public string MemberName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public DateTime PaymentDate { get; set; }

    public PaymentStatus Status { get; set; }
}