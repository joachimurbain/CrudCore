using CrudCore.Interfaces;
using CrudCore.Services;

namespace CrudCore.Controllers;

public abstract class BaseEntityController<TEntity>
	: BaseDtoController<TEntity, TEntity, TEntity, TEntity, TEntity>
	where TEntity : class, IEntity
{
	protected BaseEntityController(IBaseService<TEntity> service) : base(service) { }

	protected override TEntity ToEntity(TEntity dto) => dto;
	protected override TEntity ToDetailsDto(TEntity entity) => entity;
	protected override TEntity ToListDto(TEntity entity) => entity;
}