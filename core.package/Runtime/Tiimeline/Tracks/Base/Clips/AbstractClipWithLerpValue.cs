using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Timeline;

namespace NecroMacro.Core.Timeline
{
	[Serializable]
	public abstract class
		AbstractClipWithLerpValue<TBehavior, TValue> : AbstractClipWithLerpValue<TBehavior, TValue, TValue>
		where TBehavior : AbstractBehaviorWithLerpValue<TValue>, new()
	{
		protected override TValue BehaviorStartValue => StartValue;
		protected override TValue BehaviorEndValue => EndValue;
	}

	[Serializable]
	public abstract class AbstractClipWithLerpValue<TBehavior, TClipValue, TBehaviorValue> : AbstractClip<TBehavior>
		where TBehavior : AbstractBehaviorWithLerpValue<TBehaviorValue>, new()
	{
		public override ClipCaps clipCaps => ClipCaps.Blending;

		public AnimationCurve Curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		protected virtual bool NeedToShowUseInitialValueAsStart => true;
		protected virtual bool NeedToShowUseInitialValueAsEnd => true;
		protected virtual bool NeedToShowStartValue => !UseInitialValueAsStart;
		protected virtual bool NeedToShowEndValue => !UseInitialValueAsEnd;

		[ShowIf(nameof(NeedToShowUseInitialValueAsStart))]
		public bool UseInitialValueAsStart;

		[ShowIf(nameof(NeedToShowUseInitialValueAsEnd))]
		public bool UseInitialValueAsEnd;

		[ShowIf(nameof(NeedToShowStartValue))]
		public TClipValue StartValue;

		[ShowIf(nameof(NeedToShowEndValue))]
		public TClipValue EndValue;

		protected abstract TBehaviorValue BehaviorStartValue { get; }
		protected abstract TBehaviorValue BehaviorEndValue { get; }

		protected override void FillBehavior(TBehavior behavior, IExposedPropertyTable resolver)
		{
			behavior.Curve = Curve;
			behavior.UseInitialValueAsStart = UseInitialValueAsStart;
			behavior.UseInitialValueAsEnd = UseInitialValueAsEnd;
			if (!UseInitialValueAsStart) behavior.StartValue = BehaviorStartValue;
			if (!UseInitialValueAsEnd) behavior.EndValue = BehaviorEndValue;
		}
	}
}
