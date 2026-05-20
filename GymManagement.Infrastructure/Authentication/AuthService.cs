using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using GymManagement.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace GymManagement.Infrastructure.Authentication;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<string>> RegisterAsync(
        string fullName,
        string email,
        string password,
        string role)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser is not null)
        {
            return Result<string>.Failure("Email already exists");
        }

        var roleExists = await _roleManager.RoleExistsAsync(role);

        if (!roleExists)
        {
            return Result<string>.Failure("Role does not exist");
        }

        var user = new ApplicationUser
        {
            FullName = fullName,
            Email = email,
            UserName = email,
            IsActive = true
        };

        var createResult = await _userManager.CreateAsync(user, password);

        if (!createResult.Succeeded)
        {
            var errors = createResult.Errors
                .Select(x => x.Description)
                .ToList();

            return Result<string>.Failure("User registration failed", errors);
        }

        var addToRoleResult = await _userManager.AddToRoleAsync(user, role);

        if (!addToRoleResult.Succeeded)
        {
            var errors = addToRoleResult.Errors
                .Select(x => x.Description)
                .ToList();

            return Result<string>.Failure("Adding user to role failed", errors);
        }

        var token = await _jwtTokenService.GenerateTokenAsync(user);

        return Result<string>.Success(token, "User registered successfully");
    }

    public async Task<Result<string>> LoginAsync(
        string email,
        string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return Result<string>.Failure("Invalid email or password");
        }

        if (!user.IsActive)
        {
            return Result<string>.Failure("User account is disabled");
        }

        var passwordValid = await _userManager.CheckPasswordAsync(user, password);

        if (!passwordValid)
        {
            return Result<string>.Failure("Invalid email or password");
        }

        var token = await _jwtTokenService.GenerateTokenAsync(user);

        return Result<string>.Success(token, "Login successful");
    }
}