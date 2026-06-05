using GymManagement.Application.Features.Payments.Commands.ChangePaymentStatus;
using GymManagement.Application.Features.Payments.Commands.CreatePayment;
using GymManagement.Application.Features.Payments.Commands.DeletePayment;
using GymManagement.Application.Features.Payments.Queries.GetAllPayments;
using GymManagement.Application.Features.Payments.Queries.GetPaymentById;
using GymManagement.Domain.Constants;
using GymManagement.Domain.Enums;
using GymManagement.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [HasPermission(Permissions.Payments.View)]
    public async Task<IActionResult> GetAll(
     [FromQuery] GetAllPaymentsQuery query,
     CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [HasPermission(Permissions.Payments.View)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetPaymentByIdQuery(id),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    [HasPermission(Permissions.Payments.Create)]
    public async Task<IActionResult> Create(
        [FromBody] CreatePaymentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Data },
            result);
    }

    [HttpPut("{id:guid}/mark-paid")]
    [HasPermission(Permissions.Payments.Update)]
    public async Task<IActionResult> MarkAsPaid(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ChangePaymentStatusCommand(id, PaymentStatus.Paid),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id:guid}/mark-failed")]
    [HasPermission(Permissions.Payments.Update)]
    public async Task<IActionResult> MarkAsFailed(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ChangePaymentStatusCommand(id, PaymentStatus.Failed),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id:guid}/refund")]
    [HasPermission(Permissions.Payments.Update)]
    public async Task<IActionResult> Refund(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ChangePaymentStatusCommand(id, PaymentStatus.Refunded),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.Payments.Delete)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new DeletePaymentCommand(id),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}