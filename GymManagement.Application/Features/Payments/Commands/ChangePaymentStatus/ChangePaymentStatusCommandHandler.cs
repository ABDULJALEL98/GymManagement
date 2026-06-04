using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Payments.Commands.ChangePaymentStatus;

public class ChangePaymentStatusCommandHandler
    : IRequestHandler<ChangePaymentStatusCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangePaymentStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ChangePaymentStatusCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (payment is null)
        {
            return Result.Failure("Payment not found");
        }

        if (payment.Status == PaymentStatus.Refunded)
        {
            return Result.Failure("Refunded payment cannot be changed");
        }

        payment.Status = request.Status;
        payment.UpdatedAtUtc = DateTime.UtcNow;

        _unitOfWork.Payments.Update(payment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Payment status updated successfully");
    }
}