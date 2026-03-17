using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NecroMacro.AddressablesTools
{
	[UsedImplicitly]
	public partial class AddressablesService
	{
		public async UniTask StartAsync(CancellationToken token = default)
		{
			await Addressables.InitializeAsync(true).ToUniTask(cancellationToken: token);
			await UpdateCatalogs(token);
			await WaitWhileCachingReadyAsync(token);
		}

		public async UniTask<long> GetDownloadSizeAsync(object resource, CancellationToken token)
		{
			var locations = await Addressables.LoadResourceLocationsAsync(resource)
			                                  .ToUniTask(cancellationToken: token);

			if (locations == null || locations.Count == 0)
				return 0;

			var sizeHandle = Addressables.GetDownloadSizeAsync(locations);
			long size = await sizeHandle.ToUniTask(cancellationToken: token);

			Addressables.Release(sizeHandle);

			return size;
		}

		public async UniTask<AsyncOperationStatus> DownloadDependenciesTaskAsync(
			object resource,
			CancellationToken token,
			bool autoReleaseHandle = true,
			IProgress<float> progress = null
		)
		{
			var dependencies = Addressables.DownloadDependenciesAsync(resource, autoReleaseHandle);

			await dependencies.ToUniTask(progress, PlayerLoopTiming.Update, token);

			var status = dependencies.Status;

			if (status == AsyncOperationStatus.Succeeded && autoReleaseHandle)
				Addressables.Release(dependencies);

			return status;
		}
	}
}