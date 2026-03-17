using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace NecroMacro.TimeScale
{
	public class TimeScaleService
	{
		private readonly List<float> scaleSteps = new()
		{
			0.2f,
			0.5f,
			1f,
			2f,
			5f,
			10f
		};

		public IReadOnlyList<float> ScaleSteps => scaleSteps;
		private float Scale => scaleSteps[scaleStepIndex];
		private int scaleStepIndex;
		private float? overridenScale;

		public int ScaleStepIndex
		{
			get => scaleStepIndex;
			private set
			{
				int validatedIndex = ValidateIndex(value);
				if (scaleStepIndex == validatedIndex) return;
				scaleStepIndex = validatedIndex;
				//_signalBus.Fire(new TimeScaleChangedSignal());
				UpdateScale();
			}
		}
		
		public TimeScaleService()
		{
			int defaultIndex = scaleSteps.FindIndex(value => Math.Abs(value - 1f) < 0.01f);
			Trace.Assert(defaultIndex >= 0, "TimeScale must include 1f value");
			ScaleStepIndex = defaultIndex;
			UpdateScale();
		}

		public void IncScale()
		{
			ScaleStepIndex++;
		}

		public void DecScale()
		{
			ScaleStepIndex--;
		}

		public void SetScaleIndex(int value)
		{
			ScaleStepIndex = value;
		}

		public float GetScaleForIndex(int index)
		{
			index = ValidateIndex(index);
			return scaleSteps[index];
		}

		public void SetOverridenScale(float value)
		{
			SetOverridenScale((float?)value);
		}

		public void RemoveOverridenScale()
		{
			SetOverridenScale(null);
		}

		private void SetOverridenScale(float? value)
		{
			overridenScale = value;
			UpdateScale();
		}

		private void UpdateScale()
		{
			Time.timeScale = overridenScale ?? Scale;
		}

		private int ValidateIndex(int index)
		{
			return Math.Clamp(index, 0, scaleSteps.Count - 1);
		}
	}
}