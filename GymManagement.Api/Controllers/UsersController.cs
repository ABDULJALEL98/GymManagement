using GymManagement.Application.DTOs;
using GymManagement.Domain.Constants;
using GymManagement.Domain.Identity;
using GymManagement.Infrastructure.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    [HasPermission(Permissions.Users.View)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .OrderByDescending(user => user.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        var result = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            result.Add(new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                IsActive = user.IsActive,
                Roles = roles.ToList()
            });
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.Users.View)]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return NotFound("User was not found");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var result = new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            IsActive = user.IsActive,
            Roles = roles.ToList()
        };

        return Ok(result);
    }

    [HttpPost]
    [HasPermission(Permissions.Users.Create)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var roleExists = await _roleManager.RoleExistsAsync(request.Role);

        if (!roleExists)
        {
            return BadRequest($"Role '{request.Role}' does not exist");
        }

        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            return BadRequest("Email already exists");
        }

        var user = new ApplicationUser
        {
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Email,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            var errors = createResult.Errors
                .Select(error => error.Description)
                .ToList();

            return BadRequest(errors);
        }

        var addRoleResult = await _userManager.AddToRoleAsync(user, request.Role);

        if (!addRoleResult.Succeeded)
        {
            var errors = addRoleResult.Errors
                .Select(error => error.Description)
                .ToList();

            return BadRequest(errors);
        }

        return Ok(new
        {
            Message = "User created successfully",
            UserId = user.Id
        });
    }

    [HttpPut("{id}/role")]
    [HasPermission(Permissions.Users.Update)]
    public async Task<IActionResult> UpdateRole(
        string id,
        [FromBody] UpdateUserRoleRequest request)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return NotFound("User was not found");
        }

        var roleExists = await _roleManager.RoleExistsAsync(request.Role);

        if (!roleExists)
        {
            return BadRequest($"Role '{request.Role}' does not exist");
        }

        var currentRoles = await _userManager.GetRolesAsync(user);

        if (currentRoles.Any())
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
            {
                var errors = removeResult.Errors
                    .Select(error => error.Description)
                    .ToList();

                return BadRequest(errors);
            }
        }

        var addResult = await _userManager.AddToRoleAsync(user, request.Role);

        if (!addResult.Succeeded)
        {
            var errors = addResult.Errors
                .Select(error => error.Description)
                .ToList();

            return BadRequest(errors);
        }

        return Ok(new
        {
            Message = "User role updated successfully"
        });
    }

    [HttpPut("{id}/status")]
    [HasPermission(Permissions.Users.Update)]
    public async Task<IActionResult> UpdateStatus(
        string id,
        [FromBody] UpdateUserStatusRequest request)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return NotFound("User was not found");
        }

        user.IsActive = request.IsActive;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(error => error.Description)
                .ToList();

            return BadRequest(errors);
        }

        return Ok(new
        {
            Message = request.IsActive
                ? "User activated successfully"
                : "User disabled successfully"
        });
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.Users.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return NotFound("User was not found");
        }

        user.IsActive = false;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(error => error.Description)
                .ToList();

            return BadRequest(errors);
        }

        return Ok(new
        {
            Message = "User disabled successfully"
        });
    }
}