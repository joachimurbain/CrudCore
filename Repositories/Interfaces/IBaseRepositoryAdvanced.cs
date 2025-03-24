using System.Linq.Expressions;

namespace Wattsup.DAL.Repositories.Base.Interfaces;
public interface IBaseRepositoryAdvanced<T>
{
	Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
	Task AddRangeAsync(IEnumerable<T> entities);
	void RemoveRange(IEnumerable<T> entities);
	Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}
