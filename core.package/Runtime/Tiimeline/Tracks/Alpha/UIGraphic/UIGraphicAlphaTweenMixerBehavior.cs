using UnityEngine;
using UnityEngine.UI;

namespace NecroMacro.Core.Timeline
{
	public class UIGraphicAlphaTweenMixerBehavior : AbstractMixerBehaviorForFloat<AlphaBehavior, Graphic>
	{
		protected override float GetInitialValue(Graphic trackBinding)
		{
			return trackBinding.color.a;
		}

		protected override void SetValue(Graphic trackBinding, float alpha)
		{
			var color = trackBinding.color;
			color = new Color(color.r, color.g, color.b, alpha);
			trackBinding.color = color;
		}
	}
}