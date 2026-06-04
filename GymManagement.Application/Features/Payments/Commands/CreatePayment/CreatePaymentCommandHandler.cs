using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Payments.Commands.CreatePayment;

public class CreatePaymentCommandHandler
    : IRequestHandler<CreatePaymentCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreatePaymentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(
            request.MemberId,
            cancellationToken);

        if (member is null)
        {
            return Result<Guid>.Failure("Member not found");
        }

        if (!member.IsActive)
        {
            return Result<Guid>.Failure("Member is not active");
        }

        if (request.SubscriptionId.HasValue)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(
                request.SubscriptionId.Value,
                cancellationToken);

            if (subscription is null)
            {
                return Result<Guid>.Failure("Subscription not found");
            }

            if (subscription.MemberId != request.MemberId)
            {
                return Result<Guid>.Failure("Subscription does not belong to this member");
            }
        }

        var payment = new Payment
        {
            MemberId = request.MemberId,
            SubscriptionId = request.SubscriptionId,
            Amount = request.Amount,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = request.PaymentMethod,
            TransactionReference = request.TransactionReference,
            Status = PaymentStatus.Pending
        };

        await _unitOfWork.Payments.AddAsync(payment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(payment.Id, "Payment created successfully");
    }
}