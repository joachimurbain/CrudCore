using CrudCore.Enums;

namespace CrudCore.Repositories;

public interface IBaseRepository<T> where T : class
{
	Task<T?> GetByIdAsync(int id, IncludeStrategy strategy = IncludeStrategy.WithCollections);
	Task<IEnumerable<T>> GetAllAsync(IncludeStrategy strategy = IncludeStrategy.WithCollections);
	Task<T> AddAsync(T entity);
	Task<T> UpdateAsync(T entity);
	Task RemoveAsync(T entity);
	IQueryable<T> WithIncludes(IncludeStrategy strategy);

}
