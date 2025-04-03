using CrudCore.Enums;

namespace CrudCore.Repositories;

public interface IBaseRepository<T> where T : class
{
	Task<T?> GetByIdAsync(int id, TrackingBehavior tracking = TrackingBehavior.NoTracking);
	Task<IEnumerable<T>> GetAllAsync(TrackingBehavior tracking = TrackingBehavior.NoTracking);
	Task<T> AddAsync(T entity);
	Task<T> UpdateAsync(T entity);
	Task RemoveAsync(T entity);

}
