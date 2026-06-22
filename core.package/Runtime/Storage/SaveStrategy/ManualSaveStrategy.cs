using System;
using Cysharp.Threading.Tasks;

namespace NecroMacro.Core.Storage
{
	public class ManualSaveStrategy : ISaveStrategy
	{
		private Func<UniTask> saveFunction;

		public void Initialize(Func<UniTask> saveFunction)
		{
			this.saveFunction = saveFunction;
		}

		public void RequestSave()
		{
		}

		public UniTask SaveForce()
		{
			return saveFunction?.Invoke() ?? UniTask.CompletedTask;
		}

		public void Dispose()
		{
		}
	}
}