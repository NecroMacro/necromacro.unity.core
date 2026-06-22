using System.IO;
using Cysharp.Threading.Tasks;

namespace NecroMacro.Core.Storage
{
	public class FileStorageDriver : IStorageDriver
	{
		private readonly string filePath;

		public FileStorageDriver(string folderPath, string fileName)
		{
			filePath = Path.Combine(folderPath, fileName);
		}

		public UniTask<bool> Exists()
		{
			return UniTask.FromResult(File.Exists(filePath));
		}

		public async UniTask<string> Read()
		{
			return await File.ReadAllTextAsync(filePath).AsUniTask();
		}

		public async UniTask Write(string data)
		{
			string tempPath = filePath + ".tmp";
			await File.WriteAllTextAsync(tempPath, data).AsUniTask();

			if (File.Exists(filePath))
				File.Replace(tempPath, filePath, null);
			else
				File.Move(tempPath, filePath);
		}
	}

}