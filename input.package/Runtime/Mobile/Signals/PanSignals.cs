using UnityEngine;

namespace Core.MobileInput
{
	public class PanStartedSignal : AbstractSinglePointGestureSignal
	{
		public bool IsStartOverUi;

		public PanStartedSignal(float screenX, float screenY, bool isStartOverUi) : base(screenX, screenY)
		{
			IsStartOverUi = isStartOverUi;
		}
	}

	public class PanMovedSignal : AbstractSinglePointGestureSignal
	{
		public Vector2 DeltaScreenPos { get; }
		public Vector2 ScreenPos { get; }
		public bool IsStartOverUi;
		
		public PanMovedSignal(float screenX, float screenY, float deltaX, float deltaY, bool isStartOverUi) : base(screenX, screenY)
		{
			ScreenPos = new Vector2(screenX, screenY);
			DeltaScreenPos = new Vector2(deltaX, deltaY);
			IsStartOverUi = isStartOverUi;
		}
	}

	public class PanEndedSignal : AbstractSinglePointGestureSignal
	{
		public Vector2 Velocity { get; }

		public PanEndedSignal(float screenX, float screenY, float velocityX, float velocityY) : base(screenX, screenY)
		{
			Velocity = new Vector2(velocityX, velocityY);
		}
	}
}