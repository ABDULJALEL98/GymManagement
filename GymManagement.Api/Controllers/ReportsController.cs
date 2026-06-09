using GymManagement.Application.Features.Reports.Queries.GetBookingsReport;
using GymManagement.Application.Features.Reports.Queries.GetNewMembersReport;
using GymManagement.Application.Features.Reports.Queries.GetPaymentsReport;
using GymManagement.Application.Features.Reports.Queries.GetSubscriptionsReport;
using GymManagement.Domain.Constants;
using GymManagement.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using GymManagement.Application.Interfaces;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IExcelExportService _excelExportService;

    public ReportsController(
        IMediator mediator,
        IExcelExportService excelExportService)
    {
        _mediator = mediator;
        _excelExportService = excelExportService;
    }
    [HttpGet("payments/export")]
    [HasPermission(Permissions.Reports.View)]
    public async Task<IActionResult> ExportPaymentsReport(
    [FromQuery] GetPaymentsReportQuery query,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess || result.Data is null)
        {
            return BadRequest(result);
        }

        var fileBytes = _excelExportService.ExportToExcel(
            result.Data,
            "Payments Report");

        var fileName = $"payments-report-{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";

        return File(
            fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }
    [HttpGet("subscriptions/export")]
    [HasPermission(Permissions.Reports.View)]
    public async Task<IActionResult> ExportSubscriptionsReport(
    [FromQuery] GetSubscriptionsReportQuery query,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess || result.Data is null)
        {
            return BadRequest(result);
        }

        var fileBytes = _excelExportService.ExportToExcel(
            result.Data,
            "Subscriptions Report");

        var fileName = $"subscriptions-report-{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";

        return File(
            fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }
    [HttpGet("bookings/export")]
    [HasPermission(Permissions.Reports.View)]
    public async Task<IActionResult> ExportBookingsReport(
    [FromQuery] GetBookingsReportQuery query,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess || result.Data is null)
        {
            return BadRequest(result);
        }

        var fileBytes = _excelExportService.ExportToExcel(
            result.Data,
            "Bookings Report");

        var fileName = $"bookings-report-{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";

        return File(
            fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }
    [HttpGet("new-members/export")]
    [HasPermission(Permissions.Reports.View)]
    public async Task<IActionResult> ExportNewMembersReport(
    [FromQuery] GetNewMembersReportQuery query,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess || result.Data is null)
        {
            return BadRequest(result);
        }

        var fileBytes = _excelExportService.ExportToExcel(
            result.Data,
            "New Members Report");

        var fileName = $"new-members-report-{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";

        return File(
            fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
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