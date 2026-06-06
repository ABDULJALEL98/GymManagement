using GymManagement.Application.Interfaces;
using GymManagement.Domain.Enums;
using GymManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.BackgroundJobs;

public class SubscriptionExpirationService : ISubscriptionExpirationService
{
    private readonly ApplicationDbContext _context;

    public SubscriptionExpirationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> ExpireSubscriptionsAsync(
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var expiredSubscriptions = await _context.Subscriptions
            .Where(subscription =>
                subscription.Status == SubscriptionStatus.Active &&
                subscription.EndDate < now)
            .ToListAsync(cancellationToken);

        if (!expiredSubscriptions.Any())
        {
            return 0;
        }

        foreach (var subscription in expiredSubscriptions)
        {
            subscription.Status = SubscriptionStatus.Expired;
            subscription.UpdatedAtUtc = now;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return expiredSubscriptions.Count;
    }
}