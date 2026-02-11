using Cysharp.Threading.Tasks;

namespace NecroMacro.Core.Storage
{
	public interface IDataStorageBackend
	{
		UniTask<bool> Exists();
		UniTask<string> Read();
		UniTask Write(string data);
	}
}