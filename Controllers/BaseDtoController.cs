using CrudCore.Interfaces;
using CrudCore.Mapping;
using CrudCore.Patching;
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
	protected readonly IMapper _mapper;

	protected BaseDtoController(IBaseService<TEntity> service, IMapper mapper)
	{
		_service = service;
		_mapper = mapper;
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
		PatchModel<TEntity> patch = PatchEntityBuilder.BuildPartial<TEntity, TUpdateDto>(dto);
		TEntity updated = await _service.UpdateAsync(id, patch);
		return Ok(ToDetailsDto(updated));
	}

	[HttpDelete("{id}")]
	public virtual async Task<IActionResult> Delete(int id)
	{
		await _service.RemoveAsync(id);
		return NoContent();
	}


	protected virtual TEntity ToEntity(TCreateDto createDto)
		=> _mapper.Map<TCreateDto, TEntity>(createDto);

	protected virtual TDetailsDto ToDetailsDto(TEntity entity)
		=> _mapper.Map<TEntity, TDetailsDto>(entity);

	protected virtual TListDto ToListDto(TEntity entity)
		=> _mapper.Map<TEntity, TListDto>(entity);

}
