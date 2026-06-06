using GymManagement.Application.Interfaces;
using GymManagement.Infrastructure.Authentication;
using GymManagement.Infrastructure.Authorization;
using GymManagement.Infrastructure.BackgroundJobs;
using GymManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddScoped<ISubscriptionExpirationService, SubscriptionExpirationService>();

        services.AddHostedService<SubscriptionExpirationBackgroundService>();

        return services;
    }
}