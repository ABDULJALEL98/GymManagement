using GymManagement.Application.Features.Bookings.Commands.CancelBooking;
using GymManagement.Application.Features.Bookings.Commands.ChangeBookingStatus;
using GymManagement.Application.Features.Bookings.Commands.CreateBooking;
using GymManagement.Application.Features.Bookings.Queries.GetAllBookings;
using GymManagement.Application.Features.Bookings.Queries.GetBookingById;
using GymManagement.Domain.Constants;
using GymManagement.Domain.Enums;
using GymManagement.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [HasPermission(Permissions.Bookings.View)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAllBookingsQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [HasPermission(Permissions.Bookings.View)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetBookingByIdQuery(id),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    [HasPermission(Permissions.Bookings.Create)]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookingCommand command,
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

    [HttpPut("{id:guid}/confirm")]
    [HasPermission(Permissions.Bookings.Update)]
    public async Task<IActionResult> Confirm(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ChangeBookingStatusCommand(id, BookingStatus.Confirmed),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id:guid}/complete")]
    [HasPermission(Permissions.Bookings.Update)]
    public async Task<IActionResult> Complete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ChangeBookingStatusCommand(id, BookingStatus.Completed),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id:guid}/cancel")]
    [HasPermission(Permissions.Bookings.Update)]
    public async Task<IActionResult> Cancel(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new CancelBookingCommand(id),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}