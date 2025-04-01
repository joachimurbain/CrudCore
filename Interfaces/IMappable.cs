namespace CrudCore.Interfaces;
public interface IMappable<TEntity, TCreateDto, TUpdateDto, TDetailsDto, TListDto>
{
    TEntity ToEntity(TCreateDto dto);
    TEntity ToUpdatedEntity(TUpdateDto dto, TEntity existing);
    TDetailsDto ToDetailsDto(TEntity entity);
    TListDto ToListDto(TEntity entity);
}
