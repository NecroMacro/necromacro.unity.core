using Cysharp.Threading.Tasks;

namespace NecroMacro.Core.Storage
{
	public interface IStorageDriver
	{
		UniTask<bool> Exists();
		UniTask<string> Read();
		UniTask Write(string data);
	}
}