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

	public virtual async Task<T> AddAsync(T entity)
	{
		await _dbSet.AddAsync(entity);
		await _dbContext.SaveChangesAsync();
		return entity;
	}

	public virtual async Task<IEnumerable<T>> GetAllAsync()
	{
		return await _dbSet
			.AsNoTracking()
			.ToListAsync();
	}

	public virtual async Task<T?> GetByIdAsync(int id)
	{
		return await _dbSet
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Id == id);
	}

	public virtual async Task RemoveAsync(T entity)
	{
		_dbSet.Remove(entity);
		await _dbContext.SaveChangesAsync();
	}

	public virtual async Task<T> UpdateAsync(T entity)
	{
		_dbContext.Update(entity);
		await _dbContext.SaveChangesAsync();
		return entity;
	}

}
