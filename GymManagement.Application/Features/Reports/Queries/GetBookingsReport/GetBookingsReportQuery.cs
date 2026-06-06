using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Reports.Queries.GetBookingsReport;

public class GetBookingsReportQuery : IRequest<Result<List<BookingsReportDto>>>
{
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public BookingStatus? Status { get; set; }
}