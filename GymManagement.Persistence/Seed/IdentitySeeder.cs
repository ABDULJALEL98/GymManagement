using System.Security.Claims;
using GymManagement.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Persistence.Seed;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var role in AppRoles.All)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        await SeedAdminPermissionsAsync(roleManager);
    }

    private static async Task SeedAdminPermissionsAsync(RoleManager<IdentityRole> roleManager)
    {
        var adminRole = await roleManager.FindByNameAsync(AppRoles.Admin);

        if (adminRole is null)
        {
            return;
        }

        var existingClaims = await roleManager.GetClaimsAsync(adminRole);

        var allPermissions = Permissions.GetAllPermissions();

        foreach (var permission in allPermissions)
        {
            var hasPermission = existingClaims.Any(claim =>
                claim.Type == "Permission" &&
                claim.Value == permission);

            if (!hasPermission)
            {
                await roleManager.AddClaimAsync(
                    adminRole,
                    new Claim("Permission", permission));
            }
        }
    }
}