namespace CrudCore.Enums;
public enum IncludeStrategy
{
	/// <summary>
	/// No navigation properties included. Use for pure scalar operations or update-safe loading.
	/// </summary>
	Flat,

	/// <summary>
	/// Includes only reference (single-entity) navigation properties.
	/// Safe for most update operations.
	/// </summary>
	ReferencesOnly,

	/// <summary>
	/// Includes both references and collections (e.g., nested lists).
	/// Use only for read-only views like dashboards or exports.
	/// </summary>
	WithCollections
}
