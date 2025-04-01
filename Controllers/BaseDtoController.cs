using CrudCore.Controllers.Helpers;
using CrudCore.Interfaces;
using CrudCore.Services;
using Microsoft.AspNetCore.Mvc;


namespace CrudCore.Controllers;
[Route("api/[controller]")]
[ApiController]
public abstract class BaseDtoController<TEntity, TCreateDto, TUpdateDto, TDetailsDto, TListDto> : ControllerBase
	where TEntity : class, IEntity
	where TCreateDto : class
	where TUpdateDto : class
	where TDetailsDto : class
	where TListDto : class
{
	protected readonly IBaseService<TEntity> _service;

	protected BaseDtoController(IBaseService<TEntity> service)
	{
		_service = service;
	}

	[HttpGet]
	public virtual async Task<ActionResult<IEnumerable<TListDto>>> GetAll()
	{
		IEnumerable<TEntity> entities = await _service.GetAllAsync();
		return Ok(entities.Select(ToListDto));
	}

	[HttpGet("{id}")]
	public virtual async Task<ActionResult<TDetailsDto>> GetById(int id)
	{
		TEntity entity = await _service.GetByIdAsync(id);
		return Ok(ToDetailsDto(entity));
	}

	[HttpPost]
	public virtual async Task<ActionResult<TDetailsDto>> Create([FromBody] TCreateDto dto)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		TEntity entity = ToEntity(dto);
		entity = await _service.AddAsync(entity);

		return Ok(ToDetailsDto(entity));
	}

	[HttpPut("{id}")]
	public virtual async Task<ActionResult<TDetailsDto>> Update(int id, [FromBody] TUpdateDto dto)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}


		//var partialEntity = PatchEntityBuilder.BuildPartial<TEntity, TUpdateDto>(dto);

		var patch = PatchEntityBuilder.BuildPartial<TEntity, TUpdateDto>(dto);

		var updated = await _service.UpdateAsync(id, patch);
		return Ok(ToDetailsDto(updated));
	}

	[HttpDelete("{id}")]
	public virtual async Task<IActionResult> Delete(int id)
	{
		await _service.RemoveAsync(id);
		return NoContent();
	}


	protected abstract TEntity ToEntity(TCreateDto createDto);
	protected abstract TDetailsDto ToDetailsDto(TEntity entity);
	protected abstract TListDto ToListDto(TEntity entity);


}
