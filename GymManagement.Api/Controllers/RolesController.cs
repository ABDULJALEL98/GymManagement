using GymManagement.Application.DTOs;
using GymManagement.Domain.Constants;
using GymManagement.Infrastructure.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet]
    [HasPermission(Permissions.Roles.View)]
    public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
    {
        var roles = await _roleManager.Roles
            .Select(role => new RoleDto
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty
            })
            .ToListAsync(cancellationToken);

        return Ok(roles);
    }
}