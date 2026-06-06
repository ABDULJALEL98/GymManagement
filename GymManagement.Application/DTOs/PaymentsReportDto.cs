using GymManagement.Domain.Enums;

namespace GymManagement.Application.DTOs;

public class PaymentsReportDto
{
    public Guid PaymentId { get; set; }

    public string MemberName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public PaymentStatus Status { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? TransactionReference { get; set; }
}