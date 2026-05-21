using System.Security.Claims;
using GymManagement.Application.DTOs;
using GymManagement.Domain.Constants;
using GymManagement.Infrastructure.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public PermissionsController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet]
    [HasPermission(Permissions.Roles.View)]
    public IActionResult GetAllPermissions()
    {
        var permissions = Permissions.GetAllPermissions();

        return Ok(permissions);
    }

    [HttpGet("role/{roleName}")]
    [HasPermission(Permissions.Roles.View)]
    public async Task<IActionResult> GetRolePermissions(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);

        if (role is null)
        {
            return NotFound($"Role '{roleName}' was not found");
        }

        var roleClaims = await _roleManager.GetClaimsAsync(role);

        var rolePermissions = roleClaims
            .Where(claim => claim.Type == "Permission")
            .Select(claim => claim.Value)
            .ToList();

        var allPermissions = Permissions.GetAllPermissions();

        var result = allPermissions
            .Select(permission => new PermissionDto
            {
                Name = permission,
                IsSelected = rolePermissions.Contains(permission)
            })
            .ToList();

        return Ok(result);
    }

    [HttpPut("role/{roleName}")]
    [HasPermission(Permissions.Roles.Manage)]
    public async Task<IActionResult> UpdateRolePermissions(
        string roleName,
        [FromBody] UpdateRolePermissionsRequest request)
    {
        var role = await _roleManager.FindByNameAsync(roleName);

        if (role is null)
        {
            return NotFound($"Role '{roleName}' was not found");
        }

        var allPermissions = Permissions.GetAllPermissions();

        var invalidPermissions = request.Permissions
            .Where(permission => !allPermissions.Contains(permission))
            .ToList();

        if (invalidPermissions.Any())
        {
            return BadRequest(new
            {
                Message = "Invalid permissions found",
                InvalidPermissions = invalidPermissions
            });
        }

        var currentClaims = await _roleManager.GetClaimsAsync(role);

        var currentPermissionClaims = currentClaims
            .Where(claim => claim.Type == "Permission")
            .ToList();

        foreach (var claim in currentPermissionClaims)
        {
            await _roleManager.RemoveClaimAsync(role, claim);
        }

        foreach (var permission in request.Permissions.Distinct())
        {
            await _roleManager.AddClaimAsync(
                role,
                new Claim("Permission", permission));
        }

        return Ok(new
        {
            Message = $"Permissions for role '{roleName}' updated successfully"
        });
    }
}