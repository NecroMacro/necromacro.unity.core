using System;
using Cysharp.Threading.Tasks;

namespace NecroMacro.Core.Storage
{
	public class IntervalSaveStrategy : ISaveStrategy
	{
		private readonly TimeSpan delay;
		private Func<UniTask> saveFunction;
		private bool running;

		public IntervalSaveStrategy(TimeSpan delay)
		{
			this.delay = delay;
		}

		public void Initialize(Func<UniTask> saveFunction)
		{
			this.saveFunction = saveFunction;
			running = true;
			Loop().Forget();
		}

		private async UniTask Loop()
		{
			while (running)
			{
				await saveFunction();
				await UniTask.Delay(delay);
			}
		}

		public void MarkDirty() { }

		public void Dispose()
		{
			running = false;
		}
	}
}