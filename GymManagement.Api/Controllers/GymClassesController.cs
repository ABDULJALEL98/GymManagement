using GymManagement.Application.Features.GymClasses.Commands.CreateGymClass;
using GymManagement.Application.Features.GymClasses.Commands.DeleteGymClass;
using GymManagement.Application.Features.GymClasses.Commands.UpdateGymClass;
using GymManagement.Application.Features.GymClasses.Queries.GetAllGymClasses;
using GymManagement.Application.Features.GymClasses.Queries.GetGymClassById;
using GymManagement.Domain.Constants;
using GymManagement.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GymClassesController : ControllerBase
{
    private readonly IMediator _mediator;

    public GymClassesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [HasPermission(Permissions.GymClasses.View)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAllGymClassesQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [HasPermission(Permissions.GymClasses.View)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetGymClassByIdQuery(id),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    [HasPermission(Permissions.GymClasses.Create)]
    public async Task<IActionResult> Create(
        [FromBody] CreateGymClassCommand command,
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
    [HasPermission(Permissions.GymClasses.Update)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateGymClassCommand command,
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
    [HasPermission(Permissions.GymClasses.Delete)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new DeleteGymClassCommand(id),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}