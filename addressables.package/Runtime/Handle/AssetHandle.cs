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
			Addressables.Release(handle);
		}
	}
}