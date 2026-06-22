using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace NecroMacro.Core.Storage
{
	public class IntervalSaveStrategy : ISaveStrategy
	{
		private readonly TimeSpan delay;
		private CancellationTokenSource cancellationTokenSource = new();
		private Func<UniTask> saveFunction;

		public IntervalSaveStrategy(TimeSpan delay)
		{
			this.delay = delay;
		}

		public void Initialize(Func<UniTask> saveFunction)
		{
			this.saveFunction = saveFunction;
			Loop().SuppressCancellationThrow().Forget();
		}

		private async UniTask Loop()
		{
			while (!cancellationTokenSource.Token.IsCancellationRequested)
			{
				await saveFunction();
				await UniTask.Delay(delay, cancellationToken: cancellationTokenSource.Token);
			}
		}

		public void RequestSave()
		{
		}

		public async UniTask SaveForce()
		{
			cancellationTokenSource.Cancel();
			cancellationTokenSource.Dispose();

			await saveFunction();

			cancellationTokenSource = new CancellationTokenSource();
			Loop().SuppressCancellationThrow().Forget();
		}

		public void Dispose()
		{
			cancellationTokenSource.Cancel();
			cancellationTokenSource.Dispose();
		}
	}
}