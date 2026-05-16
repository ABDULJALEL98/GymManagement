using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using GymManagement.Persistence.Contexts;

namespace GymManagement.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Members = new GenericRepository<Member>(_context);
        Trainers = new GenericRepository<Trainer>(_context);
        SubscriptionPlans = new GenericRepository<SubscriptionPlan>(_context);
        Subscriptions = new GenericRepository<Subscription>(_context);
        GymClasses = new GenericRepository<GymClass>(_context);
        Bookings = new GenericRepository<Booking>(_context);
        Payments = new GenericRepository<Payment>(_context);
    }

    public IGenericRepository<Member> Members { get; }

    public IGenericRepository<Trainer> Trainers { get; }

    public IGenericRepository<SubscriptionPlan> SubscriptionPlans { get; }

    public IGenericRepository<Subscription> Subscriptions { get; }

    public IGenericRepository<GymClass> GymClasses { get; }

    public IGenericRepository<Booking> Bookings { get; }

    public IGenericRepository<Payment> Payments { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}