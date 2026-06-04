using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.Payments.Commands.CreatePayment;

public class CreatePaymentCommand : IRequest<Result<Guid>>
{
    public Guid MemberId { get; set; }

    public Guid? SubscriptionId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public string? TransactionReference { get; set; }
}