using System;
using Cysharp.Threading.Tasks;

namespace NecroMacro.Core.Storage
{
	public class ImmediateSaveStrategy : ISaveStrategy
	{
		private Func<UniTask> saveFunction;
		
		private bool isSaveScheduled;
		
		public void Initialize(Func<UniTask> saveFunction)
		{
			this.saveFunction = saveFunction;
		}

		public void MarkDirty()
		{
			if (isSaveScheduled)
				return;

			isSaveScheduled = true;
			
			WaitEndOfFrameAndSave().Forget();
		}

		private async UniTaskVoid WaitEndOfFrameAndSave()
		{
			await UniTask.WaitForEndOfFrame();
			isSaveScheduled = false;
			saveFunction().Forget();
		}

		public void Dispose() { }
	}
}