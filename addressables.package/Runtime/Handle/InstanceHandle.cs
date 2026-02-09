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
			Addressables.ReleaseInstance(handle);
		}
	}
}