using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NecroMacro.AddressablesTools
{
	public sealed class AssetHandle<T> : AbstractHandle<T>
	{
		private AsyncOperationHandle<T> handle;

		public AssetHandle(AsyncOperationHandle<T> handle)
		{
			this.handle = handle;
		}

		protected override T GetValue()
		{
			return handle.Result;
		}

		protected override void ReleaseInternal()
		{
			#if UNITY_EDITOR
			var matches = Regex.Matches(handle.DebugName, @"\(([^()]*)\)");
				if (matches.Count > 0)
				{
					string value = matches[^1].Groups[1].Value;
					Debug.Log($"<color=green>Release</color> asset: {value}");
				}
			#endif
			
			Addressables.Release(handle);
		}
	}
}