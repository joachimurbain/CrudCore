namespace CrudCore.Mapping;
public interface IMapper
{
	TTarget Map<TSource, TTarget>(TSource source)
		where TSource : class
		where TTarget : class;
}
