using CrudCore.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace CrudCore.Extensions;
public static class CrudCoreServiceCollectionExtensions
{
	public static IServiceCollection AddCrudCore(this IServiceCollection services)
	{
		services.AddSingleton<IMapper, ExpressionMapper>();
		return services;
	}
}
