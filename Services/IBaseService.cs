using CrudCore.Enums;
using CrudCore.Interfaces;
using CrudCore.Patching;

namespace CrudCore.Services;

public interface IBaseService<T> where T : class, IEntity
{
	Task<T> GetByIdAsync(int id, TrackingBehavior strategy = TrackingBehavior.NoTracking);
	Task<IEnumerable<T>> GetAllAsync(TrackingBehavior strategy = TrackingBehavior.NoTracking);
	Task<T> AddAsync(T entity);
	Task<T> UpdateAsync(int id, PatchModel<T> patchModel);
	Task RemoveAsync(int id);
}
