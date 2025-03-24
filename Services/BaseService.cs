using Wattsup.BLL.Services.Base.Interfaces;
using Wattsup.DAL.Repositories.Base.Interfaces;
using Wattsup.Domain.Interfaces;

namespace Wattsup.BLL.Services.Base;
public class BaseService<T> : IBaseService<T> where T : class
{
	protected readonly IBaseRepository<T> _repository;

	public BaseService(IBaseRepository<T> repository)
	{
		_repository = repository;
	}
	public virtual async Task<T> GetByIdAsync(int id)
	{
		T? result = await _repository.GetByIdAsync(id);
		if (result == null)
		{
			throw new Exception("Resource not found");
		}
		return result;
	}

	public virtual async Task<IEnumerable<T>> GetAllAsync()
	{
		return await _repository.GetAllAsync();
	}

	public virtual async Task<T> AddAsync(T entity)
	{
		if (entity is IValidatable validatable)
		{
			validatable.Validate();
		}
		return await _repository.AddAsync(entity);
	}

	public virtual async Task<T> UpdateAsync(T entity)
	{
		if (entity is IValidatable validatable)
		{
			validatable.Validate();
		}
		return await _repository.UpdateAsync(entity);
	}

	public async virtual Task<T> UpdateAsync<TPatch>(int id, TPatch patchModel) where TPatch : class
	{
		T? entity = await _repository.GetByIdAsync(id);
		if (entity == null)
		{
			throw new Exception("Resource not found");
		}

		if (entity is not IPatchable<TPatch> patchable)
		{
			throw new InvalidOperationException($"{typeof(T).Name} does not support patching with {typeof(TPatch).Name}.");
		}

		patchable.ApplyPatch(patchModel);

		if (entity is IValidatable validatable)
		{
			validatable.Validate();
		}
		return await _repository.UpdateAsync(entity);
	}

	public virtual async Task RemoveAsync(int id)
	{
		T? entity = await GetByIdAsync(id);
		if (entity is not null)
		{
			await _repository.RemoveAsync(entity);
		}

	}


}


