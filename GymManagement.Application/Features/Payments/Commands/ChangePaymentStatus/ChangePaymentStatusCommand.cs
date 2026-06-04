using GymManagement.Application.Common.Models;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Payments.Commands.ChangePaymentStatus;

public class ChangePaymentStatusCommand : IRequest<Result>
{
    public Guid Id { get; set; }

    public PaymentStatus Status { get; set; }

    public ChangePaymentStatusCommand(Guid id, PaymentStatus status)
    {
        Id = id;
        Status = status;
    }
}