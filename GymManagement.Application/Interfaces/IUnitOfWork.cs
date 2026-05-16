using GymManagement.Domain.Entities;

namespace GymManagement.Application.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<Member> Members { get; }

    IGenericRepository<Trainer> Trainers { get; }

    IGenericRepository<SubscriptionPlan> SubscriptionPlans { get; }

    IGenericRepository<Subscription> Subscriptions { get; }

    IGenericRepository<GymClass> GymClasses { get; }

    IGenericRepository<Booking> Bookings { get; }

    IGenericRepository<Payment> Payments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}