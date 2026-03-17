using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NecroMacro.AddressablesTools
{
	public sealed class InstanceHandle : AbstractHandle<GameObject>
	{
		private AsyncOperationHandle<GameObject> handle;

		public InstanceHandle(AsyncOperationHandle<GameObject> handle)
		{
			this.handle = handle;
		}

		protected override GameObject GetValue()
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
				Debug.Log($"<color=green>Release</color> instance: {value}");
			}
			#endif
			
			Addressables.ReleaseInstance(handle);
		}
	}
}