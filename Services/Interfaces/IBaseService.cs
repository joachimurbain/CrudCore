namespace Wattsup.BLL.Services.Base.Interfaces;
public interface IBaseService<T> where T : class
{
	Task<T> GetByIdAsync(int id);
	Task<IEnumerable<T>> GetAllAsync();
	Task<T> AddAsync(T entity);
	Task<T> UpdateAsync(T entity);
	Task<T> UpdateAsync<TPatch>(int id, TPatch patchModel)
		where TPatch : class;
	Task RemoveAsync(int id);
}
