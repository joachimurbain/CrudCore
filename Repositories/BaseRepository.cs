using CrudCore.Enums;
using CrudCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrudCore.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntity
{
    protected readonly DbContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id, TrackingBehavior tracking = TrackingBehavior.NoTracking)
    {
        var query = _dbSet.AsQueryable();

        query = AddIncludes(query);

        if (tracking == TrackingBehavior.NoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(TrackingBehavior tracking = TrackingBehavior.NoTracking)
    {
        var query = _dbSet.AsQueryable();

        query = AddIncludes(query);

        if (tracking == TrackingBehavior.NoTracking)
            query = query.AsNoTracking();

        return await query.ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public virtual async Task RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Override this in derived repos to include navigation properties.
    /// </summary>
    protected virtual IQueryable<T> AddIncludes(IQueryable<T> query)
    {
        return query;
    }


}
