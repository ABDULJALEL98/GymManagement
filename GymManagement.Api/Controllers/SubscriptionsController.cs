using GymManagement.Application.Features.Subscriptions.Commands.ChangeSubscriptionStatus;
using GymManagement.Application.Features.Subscriptions.Commands.CreateSubscription;
using GymManagement.Application.Features.Subscriptions.Commands.DeleteSubscription;
using GymManagement.Application.Features.Subscriptions.Queries.GetAllSubscriptions;
using GymManagement.Application.Features.Subscriptions.Queries.GetSubscriptionById;
using GymManagement.Domain.Constants;
using GymManagement.Domain.Enums;
using GymManagement.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubscriptionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [HasPermission(Permissions.Subscriptions.View)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAllSubscriptionsQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [HasPermission(Permissions.Subscriptions.View)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetSubscriptionByIdQuery(id),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    [HasPermission(Permissions.Subscriptions.Create)]
    public async Task<IActionResult> Create(
        [FromBody] CreateSubscriptionCommand command,
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

    [HttpPut("{id:guid}/activate")]
    [HasPermission(Permissions.Subscriptions.Update)]
    public async Task<IActionResult> Activate(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ChangeSubscriptionStatusCommand(id, SubscriptionStatus.Active),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id:guid}/suspend")]
    [HasPermission(Permissions.Subscriptions.Update)]
    public async Task<IActionResult> Suspend(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ChangeSubscriptionStatusCommand(id, SubscriptionStatus.Suspended),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id:guid}/cancel")]
    [HasPermission(Permissions.Subscriptions.Update)]
    public async Task<IActionResult> Cancel(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ChangeSubscriptionStatusCommand(id, SubscriptionStatus.Cancelled),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id:guid}/expire")]
    [HasPermission(Permissions.Subscriptions.Update)]
    public async Task<IActionResult> Expire(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ChangeSubscriptionStatusCommand(id, SubscriptionStatus.Expired),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.Subscriptions.Delete)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new DeleteSubscriptionCommand(id),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}