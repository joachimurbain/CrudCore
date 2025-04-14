using System.Reflection;
using System.Runtime.CompilerServices;

namespace CrudCore.Patching;

public static class PatchEntityBuilder
{
	public static PatchModel<TEntity> BuildPartial<TEntity, TDto>(TDto dto)
	where TEntity : class
	where TDto : class
	{
		// Create uninitialized instance of the entity (bypasses required field constructor checks)

		TEntity entity = (TEntity)RuntimeHelpers.GetUninitializedObject(typeof(TEntity));
		HashSet<string> updatedFields = new HashSet<string>();


		PropertyInfo[] dtoProps = typeof(TDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
		PropertyInfo[] entityProps = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

		foreach (var dtoProp in dtoProps)
		{
			var value = dtoProp.GetValue(dto);
			if (value is null) continue;

			PropertyInfo? entityProp = entityProps.FirstOrDefault(p =>
				string.Equals(p.Name, dtoProp.Name, StringComparison.OrdinalIgnoreCase) &&
				p.CanWrite
			);

			if (entityProp == null)
			{
				continue;
			}

			try
			{
				Type targetType = entityProp.PropertyType;

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
