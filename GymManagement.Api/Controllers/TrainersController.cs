using GymManagement.Application.Features.Trainers.Commands.CreateTrainer;
using GymManagement.Application.Features.Trainers.Commands.DeleteTrainer;
using GymManagement.Application.Features.Trainers.Commands.UpdateTrainer;
using GymManagement.Application.Features.Trainers.Queries.GetAllTrainers;
using GymManagement.Application.Features.Trainers.Queries.GetTrainerById;
using GymManagement.Domain.Constants;
using GymManagement.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainersController : ControllerBase
{
    private readonly IMediator _mediator;

    public TrainersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    [HasPermission(Permissions.Trainers.View)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllTrainersQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [HasPermission(Permissions.Trainers.View)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetTrainerByIdQuery(id),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    [HasPermission(Permissions.Trainers.Create)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTrainerCommand command,
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

    [HttpPut("{id:guid}")]
    [HasPermission(Permissions.Trainers.Update)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTrainerCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Route id does not match request body id");
        }

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.Trainers.Delete)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new DeleteTrainerCommand(id),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}