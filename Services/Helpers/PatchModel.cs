namespace CrudCore.Services.Helpers;

public class PatchModel<T> where T : class
{
	public T PartialEntity { get; set; } = default!;
	public HashSet<string> UpdatedFields { get; set; } = new();
}