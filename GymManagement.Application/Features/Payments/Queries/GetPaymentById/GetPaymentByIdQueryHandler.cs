using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Payments.Queries.GetPaymentById;

public class GetPaymentByIdQueryHandler
    : IRequestHandler<GetPaymentByIdQuery, Result<PaymentDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPaymentByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaymentDto>> Handle(
        GetPaymentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var payment = await _unitOfWork.Payments
            .Query()
            .AsNoTracking()
            .Include(x => x.Member)
            .Include(x => x.Subscription)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (payment is null)
        {
            return Result<PaymentDto>.Failure("Payment not found");
        }

        var dto = new PaymentDto
        {
            Id = payment.Id,
            MemberId = payment.MemberId,
            MemberName = payment.Member.FullName,
            SubscriptionId = payment.SubscriptionId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMethod = payment.PaymentMethod,
            Status = payment.Status,
            TransactionReference = payment.TransactionReference
        };

        return Result<PaymentDto>.Success(dto);
    }
}