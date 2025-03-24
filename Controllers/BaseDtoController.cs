using Microsoft.AspNetCore.Mvc;
using Wattsup.BLL.Services.Base.Interfaces;

namespace Wattsup.Api.Controllers.Base;
[Route("api/[controller]")]
[ApiController]
public abstract class BaseDtoController<TEntity, TCreateDto, TUpdateDto, TDetailsDto, TListDto> : ControllerBase
	where TEntity : class
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

		TEntity entity = ToUpdatedEntity(dto);

		((dynamic)entity).Id = id;

		entity = await _service.UpdateAsync(entity);
		return Ok(ToDetailsDto(entity));
	}

	[HttpDelete("{id}")]
	public virtual async Task<IActionResult> Delete(int id)
	{
		await _service.RemoveAsync(id);
		return NoContent();
	}


	protected abstract TEntity ToEntity(TCreateDto createDto);
	protected abstract TEntity ToUpdatedEntity(TUpdateDto dto);
	protected abstract TDetailsDto ToDetailsDto(TEntity entity);
	protected abstract TListDto ToListDto(TEntity entity);


}
