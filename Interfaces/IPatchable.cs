namespace Wattsup.Domain.Interfaces;
public interface IPatchable<TPatchModel>
{
	void ApplyPatch(TPatchModel patch);
}
