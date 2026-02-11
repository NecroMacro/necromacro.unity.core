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

		public void MarkDirty()
		{ }

		public void Save()
		{
			saveFunction().Forget();
		}

		public void Dispose() { }
	}
}