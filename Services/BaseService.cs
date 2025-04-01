
using CrudCore.Interfaces;
using CrudCore.Repositories;
using CrudCore.Services.Helpers;


namespace CrudCore.Services;
public class BaseService<T> : IBaseService<T> where T : class, IEntity
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

	public async virtual Task<T> UpdateAsync(int id, PatchModel<T> patchModel)
	{
		T? entity = await _repository.GetByIdAsync(id);
		if (entity == null)
		{
			throw new Exception("Resource not found");
		}

		PatchHelper.ApplyToEntity(patchModel, entity);

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


