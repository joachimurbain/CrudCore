namespace CrudCore.Interfaces;
public interface ITempMappingScaffold<TEntity, TCreateDto, TDetailsDto, TListDto>
{
    TEntity ToEntity(TCreateDto dto);
    TDetailsDto ToDetailsDto(TEntity entity);
    TListDto ToListDto(TEntity entity);
}
