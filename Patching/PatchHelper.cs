using System.Reflection;

namespace CrudCore.Patching;
public static class PatchHelper
{
	public static void ApplyToEntity<TEntity>(PatchModel<TEntity> patchModel, TEntity target)
		where TEntity : class
	{
		var partial = patchModel.PartialEntity;
		var updatedFields = patchModel.UpdatedFields;

		var entityType = typeof(TEntity);

		foreach (var field in updatedFields)
		{
			var prop = entityType.GetProperty(field, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
			if (prop is null || !prop.CanWrite) continue;

			var value = prop.GetValue(partial);
			if (value is null) continue;

			prop.SetValue(target, value);
		}
	}
}
