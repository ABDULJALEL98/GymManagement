using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.Payments.Commands.DeletePayment;

public class DeletePaymentCommandHandler
    : IRequestHandler<DeletePaymentCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePaymentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeletePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (payment is null)
        {
            return Result.Failure("Payment not found");
        }

        _unitOfWork.Payments.Delete(payment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Payment deleted successfully");
    }
}