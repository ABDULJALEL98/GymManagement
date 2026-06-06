using GymManagement.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GymManagement.Infrastructure.BackgroundJobs;

public class SubscriptionExpirationBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<SubscriptionExpirationBackgroundService> _logger;

    public SubscriptionExpirationBackgroundService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<SubscriptionExpirationBackgroundService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Subscription expiration background service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var expirationService = scope.ServiceProvider
                    .GetRequiredService<ISubscriptionExpirationService>();

                var expiredCount = await expirationService
                    .ExpireSubscriptionsAsync(stoppingToken);

                if (expiredCount > 0)
                {
                    _logger.LogInformation(
                        "Expired {ExpiredCount} subscriptions",
                        expiredCount);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "An error occurred while expiring subscriptions");
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}