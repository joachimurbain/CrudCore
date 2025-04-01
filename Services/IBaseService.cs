using CrudCore.Interfaces;
using CrudCore.Services.Helpers;

namespace CrudCore.Services;

public interface IBaseService<T> where T : class, IEntity
{
	Task<T> GetByIdAsync(int id);
	Task<IEnumerable<T>> GetAllAsync();
	Task<T> AddAsync(T entity);
	Task<T> UpdateAsync(int id, PatchModel<T> patchModel);
	Task RemoveAsync(int id);
}
