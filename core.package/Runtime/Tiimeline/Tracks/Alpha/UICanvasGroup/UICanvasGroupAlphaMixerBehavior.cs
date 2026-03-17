using UnityEngine;

namespace NecroMacro.Core.Timeline
{
	public class UICanvasGroupAlphaMixerBehavior : AbstractMixerBehaviorForFloat<AlphaBehavior, CanvasGroup>
	{
		protected override float GetInitialValue(CanvasGroup trackBinding)
		{
			return trackBinding.alpha;
		}

		protected override void SetValue(CanvasGroup trackBinding, float alpha)
		{
			trackBinding.alpha = alpha;
		}
	}
}