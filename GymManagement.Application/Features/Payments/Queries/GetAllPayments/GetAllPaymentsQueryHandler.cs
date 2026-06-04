using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Payments.Queries.GetAllPayments;

public class GetAllPaymentsQueryHandler
    : IRequestHandler<GetAllPaymentsQuery, Result<List<PaymentDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllPaymentsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<PaymentDto>>> Handle(
        GetAllPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        var payments = await _unitOfWork.Payments
            .Query()
            .AsNoTracking()
            .Include(x => x.Member)
            .Include(x => x.Subscription)
            .OrderByDescending(x => x.PaymentDate)
            .Select(x => new PaymentDto
            {
                Id = x.Id,
                MemberId = x.MemberId,
                MemberName = x.Member.FullName,
                SubscriptionId = x.SubscriptionId,
                Amount = x.Amount,
                PaymentDate = x.PaymentDate,
                PaymentMethod = x.PaymentMethod,
                Status = x.Status,
                TransactionReference = x.TransactionReference
            })
            .ToListAsync(cancellationToken);

        return Result<List<PaymentDto>>.Success(payments);
    }
}