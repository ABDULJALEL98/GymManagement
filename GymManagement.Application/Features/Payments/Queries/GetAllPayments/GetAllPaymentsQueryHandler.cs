using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Payments.Queries.GetAllPayments;

public class GetAllPaymentsQueryHandler
    : IRequestHandler<GetAllPaymentsQuery, Result<PagedResult<PaymentDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllPaymentsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PagedResult<PaymentDto>>> Handle(
        GetAllPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Payments
            .Query()
            .AsNoTracking()
            .Include(x => x.Member)
            .Include(x => x.Subscription)
            .AsQueryable();

        if (request.MemberId.HasValue)
        {
            query = query.Where(x => x.MemberId == request.MemberId.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(x => x.PaymentDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(x => x.PaymentDate <= request.ToDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.Trim();

            query = query.Where(x =>
                x.Member.FullName.Contains(searchTerm) ||
                x.PaymentMethod.Contains(searchTerm) ||
                (x.TransactionReference != null &&
                 x.TransactionReference.Contains(searchTerm)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var payments = await query
            .OrderByDescending(x => x.PaymentDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
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

        var pagedResult = PagedResult<PaymentDto>.Create(
            payments,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PagedResult<PaymentDto>>.Success(pagedResult);
    }
}