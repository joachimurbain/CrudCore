using CrudCore.Interfaces;
using CrudCore.Mapping;
using CrudCore.Services;

namespace CrudCore.Controllers;

public abstract class BaseEntityController<TEntity>
	: BaseDtoController<TEntity, TEntity, TEntity, TEntity, TEntity>
	where TEntity : class, IEntity
{
	protected BaseEntityController(IBaseService<TEntity> service, IMapper mapper)
		: base(service, mapper)
	{
	}
}