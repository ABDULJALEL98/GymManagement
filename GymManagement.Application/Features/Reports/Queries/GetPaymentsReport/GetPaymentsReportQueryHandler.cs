using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Reports.Queries.GetPaymentsReport;

public class GetPaymentsReportQueryHandler
    : IRequestHandler<GetPaymentsReportQuery, Result<List<PaymentsReportDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPaymentsReportQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<PaymentsReportDto>>> Handle(
        GetPaymentsReportQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Payments
            .Query()
            .AsNoTracking()
            .Include(x => x.Member)
            .AsQueryable();

        if (request.FromDate.HasValue)
        {
            query = query.Where(x => x.PaymentDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(x => x.PaymentDate <= request.ToDate.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        var result = await query
            .OrderByDescending(x => x.PaymentDate)
            .Select(x => new PaymentsReportDto
            {
                PaymentId = x.Id,
                MemberName = x.Member.FullName,
                Amount = x.Amount,
                PaymentMethod = x.PaymentMethod,
                Status = x.Status,
                PaymentDate = x.PaymentDate,
                TransactionReference = x.TransactionReference
            })
            .ToListAsync(cancellationToken);

        return Result<List<PaymentsReportDto>>.Success(result);
    }
}