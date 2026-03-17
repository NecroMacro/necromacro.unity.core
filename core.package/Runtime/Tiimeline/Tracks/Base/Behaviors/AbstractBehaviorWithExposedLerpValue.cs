namespace NecroMacro.Core.Timeline
{
	public class AbstractBehaviorWithExposedLerpValue<TExposedValue, TValue> : AbstractBehaviorWithLerpValue<TValue>
	{
		public TExposedValue ExposedStartValue;
		public TExposedValue ExposedEndValue;
	}
}