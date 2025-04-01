using CrudCore.Services.Helpers;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CrudCore.Controllers.Helpers;

public class PatchEntityBuilder
{
	public static PatchModel<TEntity> BuildPartial<TEntity, TDto>(TDto dto)
	where TEntity : class
	where TDto : class
	{
		// Create uninitialized instance of the entity (bypasses required field constructor checks)

		var entity = (TEntity)RuntimeHelpers.GetUninitializedObject(typeof(TEntity));
		var updatedFields = new HashSet<string>();


		var dtoProps = typeof(TDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
		var entityProps = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

		foreach (var dtoProp in dtoProps)
		{
			var value = dtoProp.GetValue(dto);
			if (value is null) continue;

			var entityProp = entityProps.FirstOrDefault(p =>
				string.Equals(p.Name, dtoProp.Name, StringComparison.OrdinalIgnoreCase) &&
				p.CanWrite
			);

			if (entityProp == null) continue;

			try
			{
				var targetType = entityProp.PropertyType;

				// Convert value to entity's property type (handling nullable)
				var converted = Convert.ChangeType(value, Nullable.GetUnderlyingType(targetType) ?? targetType);
				entityProp.SetValue(entity, converted);

				updatedFields.Add(entityProp.Name);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to patch property {entityProp.Name}: {ex.Message}");
			}
		}

		return new PatchModel<TEntity>
		{
			PartialEntity = entity,
			UpdatedFields = updatedFields
		};
	}
}
