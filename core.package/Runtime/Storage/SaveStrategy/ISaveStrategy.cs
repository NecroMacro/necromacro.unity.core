using System;
using Cysharp.Threading.Tasks;

namespace NecroMacro.Core.Storage
{
	public interface ISaveStrategy : IDisposable
	{
		void Initialize(Func<UniTask> saveFunction);
		void MarkDirty();
	}
}