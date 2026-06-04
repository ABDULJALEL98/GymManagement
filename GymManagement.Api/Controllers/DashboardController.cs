using GymManagement.Application.Features.Dashboard.Queries.GetDashboardStats;
using GymManagement.Domain.Constants;
using GymManagement.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("stats")]
    [HasPermission(Permissions.Dashboard.View)]
    public async Task<IActionResult> GetStats(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetDashboardStatsQuery(),
            cancellationToken);

        return Ok(result);
    }
}