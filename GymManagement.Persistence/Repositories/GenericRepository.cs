using System.Linq.Expressions;
using GymManagement.Application.Interfaces;
using GymManagement.Domain.Common;
using GymManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : AuditableEntity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    public IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        entity.IsDeleted = true;
        _dbSet.Update(entity);
    }

   
}