using UnityEngine;

namespace Core.MobileInput
{
	public abstract class AbstractSinglePointGestureSignal
	{
		public Vector3 ScreenPoint { get; }

		protected AbstractSinglePointGestureSignal(float screenX, float screenY)
		{
			ScreenPoint = new Vector3(screenX, screenY, 0.0f);
		}
	}

	public class TapStartedSignal : AbstractSinglePointGestureSignal
	{
		public TapStartedSignal(float screenX, float screenY) : base(screenX, screenY) { }
	}

	public class TapEndedSignal : AbstractSinglePointGestureSignal
	{
		public bool IsStartOverUi;

		public TapEndedSignal(float screenX, float screenY, bool isStartOverUi) : base(screenX, screenY)
		{
			IsStartOverUi = isStartOverUi;
		}
	}

	public class Tap4FingersEndedSignal : AbstractSinglePointGestureSignal
	{
		public Tap4FingersEndedSignal(float screenX, float screenY) : base(screenX, screenY) { }
	}

	public class TapFailedSignal : AbstractSinglePointGestureSignal
	{
		public TapFailedSignal(float screenX, float screenY) : base(screenX, screenY) { }
	}

	public class TapWithMultipleFingersEndedSignal : AbstractSinglePointGestureSignal
	{
		public int FingersAmount { get; }

		public TapWithMultipleFingersEndedSignal(float screenX, float screenY, int fingersAmount) : base(
			screenX, screenY
		)
		{
			FingersAmount = fingersAmount;
		}
	}

	public class DoubleTapWithMultipleFingersEndedSignal : AbstractSinglePointGestureSignal
	{
		public int FingersAmount { get; }

		public DoubleTapWithMultipleFingersEndedSignal(float screenX, float screenY, int fingersAmount) : base(
			screenX, screenY
		)
		{
			FingersAmount = fingersAmount;
		}
	}

	public class DoubleTapEndedSignal : AbstractSinglePointGestureSignal
	{
		public DoubleTapEndedSignal(float screenX, float screenY) : base(screenX, screenY) { }
	}

	public class LongTapStartedSignal : AbstractSinglePointGestureSignal
	{
		public LongTapStartedSignal(float screenX, float screenY) : base(screenX, screenY) { }
	}

	public class LongTapMovedSignal : AbstractSinglePointGestureSignal
	{
		public Vector3 StartScreenPoint { get; }

		public LongTapMovedSignal(float screenX, float screenY, float startScreenX, float startScreenY) : base(
			screenX, screenY
		)
		{
			StartScreenPoint = new Vector3(startScreenX, startScreenY, 0.0f);
		}
	}

	public class LongTapEndedSignal : AbstractSinglePointGestureSignal
	{
		public LongTapEndedSignal(float screenX, float screenY) : base(screenX, screenY) { }
	}

	public class LongTapFailedSignal : AbstractSinglePointGestureSignal
	{
		public LongTapFailedSignal(float screenX, float screenY) : base(screenX, screenY) { }
	}
}
