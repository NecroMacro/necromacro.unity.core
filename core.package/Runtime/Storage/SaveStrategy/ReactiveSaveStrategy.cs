using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace NecroMacro.Core.Storage
{
	public class ReactiveSaveStrategy : ISaveStrategy
	{
		private readonly CancellationTokenSource cancellationTokenSource = new();

		private Func<UniTask> saveFunction;

		private bool isSaveScheduled;

		public void Initialize(Func<UniTask> saveFunction)
		{
			this.saveFunction = saveFunction;
		}

		public void RequestSave()
		{
			if (isSaveScheduled)
				return;

			isSaveScheduled = true;
			WaitEndOfFrameAndSave().SuppressCancellationThrow().Forget();
		}

		public UniTask SaveForce()
		{
			isSaveScheduled = false;
			return SaveSafe();
		}

		private async UniTask WaitEndOfFrameAndSave()
		{
			await UniTask.WaitForEndOfFrame(cancellationToken: cancellationTokenSource.Token);

			if (!isSaveScheduled)
				return;

			isSaveScheduled = false;
			await SaveSafe();
		}

		private UniTask SaveSafe()
		{
			return saveFunction?.Invoke() ?? UniTask.CompletedTask;
		}

		public void Dispose()
		{
			cancellationTokenSource.Cancel();
			cancellationTokenSource.Dispose();
		}
	}
}