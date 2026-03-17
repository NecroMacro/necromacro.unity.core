using Cysharp.Threading.Tasks;

namespace NecroMacro.UI
{
	public interface IVisibility
	{
		VisibilityState VisualState { get; }
		UniTask SetVisibilityState(VisibilityState state);
	}
}