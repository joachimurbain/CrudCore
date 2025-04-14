using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CrudCore.Mapping;
public class ExpressionMapper : IMapper
{
	private readonly ConcurrentDictionary<(Type, Type), Delegate> _mapFuncs = new();
	public TTarget Map<TSource, TTarget>(TSource source)
		where TSource : class
		where TTarget : class
	{
		(Type, Type) key = (typeof(TSource), typeof(TTarget));

		Func<TSource, TTarget> func = (Func<TSource, TTarget>)_mapFuncs.GetOrAdd(key, _ =>
		{
			ParameterExpression sourceParam = Expression.Parameter(typeof(TSource), "src");


			Dictionary<string, PropertyInfo> sourceProps = typeof(TSource)
				.GetProperties()
				.Where(p => p.CanRead)
				.ToDictionary(p => p.Name);


			List<MemberBinding> bindings = typeof(TTarget)
				.GetProperties()
				.Where(t => t.CanWrite)
				.Select(targetProperty =>
				{

					sourceProps.TryGetValue(targetProperty.Name, out PropertyInfo? sourceProperty);

					bool isRequired = targetProperty
						.GetCustomAttributes(typeof(RequiredMemberAttribute), true)
						.Any();



					if (sourceProperty == null || sourceProperty.PropertyType != targetProperty.PropertyType)
					{
						if (isRequired)
						{
							throw new InvalidOperationException(
								$"Mapping from {typeof(TSource).Name} to {typeof(TTarget).Name} is missing required property: {targetProperty.Name}");
						}

						return null;
					}


					MemberExpression sourceVal = Expression.Property(sourceParam, sourceProperty);
					return Expression.Bind(targetProperty, sourceVal);
				})
				.OfType<MemberBinding>()
				.ToList()!;

			MemberInitExpression body = Expression.MemberInit(Expression.New(typeof(TTarget)), bindings);
			Expression<Func<TSource, TTarget>> lambda = Expression.Lambda<Func<TSource, TTarget>>(body, sourceParam);

			return lambda.Compile();
		});

		return func(source);

	}
}
