using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Reports.Queries.GetSubscriptionsReport;

public class GetSubscriptionsReportQuery : IRequest<Result<List<SubscriptionsReportDto>>>
{
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public SubscriptionStatus? Status { get; set; }
}