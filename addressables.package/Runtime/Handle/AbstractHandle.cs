using System;

namespace NecroMacro.AddressablesTools
{
	public abstract class AbstractHandle<T> : IDisposable
	{
		private bool IsReleased { get; set; }
		protected abstract T GetValue();

		public void Dispose()
		{
			if (IsReleased)
				return;

			ReleaseInternal();
			IsReleased = true;
		}

		protected abstract void ReleaseInternal();
	}
}