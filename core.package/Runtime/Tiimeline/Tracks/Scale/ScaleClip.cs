using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NecroMacro.Core.Timeline
{
	[Serializable]
	public class ScaleClip : AbstractClipWithLerpValue<ScaleBehavior, Vector3>
	{
		protected override bool NeedToShowUseInitialValueAsStart => !IsRelative;
		protected override bool NeedToShowUseInitialValueAsEnd => !IsRelative;
		private bool NeedToShowSaveStartSign => !IsRelative;

		[ShowIf(nameof(NeedToShowSaveStartSign))]
		public bool SaveStartSign;

		public bool IsRelative;

		protected override void FillBehavior(ScaleBehavior behavior, IExposedPropertyTable resolver)
		{
			base.FillBehavior(behavior, resolver);
			behavior.SaveStartSign = SaveStartSign;
			behavior.IsRelative = IsRelative;
		}
	}
}