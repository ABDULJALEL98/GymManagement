using GymManagement.Application.Features.Reports.Queries.GetBookingsReport;
using GymManagement.Application.Features.Reports.Queries.GetNewMembersReport;
using GymManagement.Application.Features.Reports.Queries.GetPaymentsReport;
using GymManagement.Application.Features.Reports.Queries.GetSubscriptionsReport;
using GymManagement.Domain.Constants;
using GymManagement.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("payments")]
    [HasPermission(Permissions.Reports.View)]
    public async Task<IActionResult> GetPaymentsReport(
        [FromQuery] GetPaymentsReportQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("subscriptions")]
    [HasPermission(Permissions.Reports.View)]
    public async Task<IActionResult> GetSubscriptionsReport(
        [FromQuery] GetSubscriptionsReportQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("bookings")]
    [HasPermission(Permissions.Reports.View)]
    public async Task<IActionResult> GetBookingsReport(
        [FromQuery] GetBookingsReportQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("new-members")]
    [HasPermission(Permissions.Reports.View)]
    public async Task<IActionResult> GetNewMembersReport(
        [FromQuery] GetNewMembersReportQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}