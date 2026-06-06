using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.Reports.Queries.GetNewMembersReport;

public class GetNewMembersReportQuery : IRequest<Result<List<NewMembersReportDto>>>
{
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public bool? IsActive { get; set; }
}