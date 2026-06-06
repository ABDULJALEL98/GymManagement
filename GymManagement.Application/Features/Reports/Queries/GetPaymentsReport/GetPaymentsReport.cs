using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Reports.Queries.GetPaymentsReport;

public class GetPaymentsReportQuery : IRequest<Result<List<PaymentsReportDto>>>
{
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public PaymentStatus? Status { get; set; }
}