using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NecroMacro.Core.Storage
{
	public class PlayerPrefsStorageBackend : IDataStorageBackend
	{
		private readonly string key;

		public PlayerPrefsStorageBackend(string key)
		{
			this.key = key;
		}

		public UniTask<bool> Exists()
		{
			return UniTask.FromResult(PlayerPrefs.HasKey(key));
		}

		public UniTask<string> Read()
		{
			return UniTask.FromResult(PlayerPrefs.GetString(key));
		}

		public UniTask Write(string data)
		{
			PlayerPrefs.SetString(key, data);
			PlayerPrefs.Save();
			return UniTask.CompletedTask;
		}
	}
}