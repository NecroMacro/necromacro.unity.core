using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NecroMacro.Core.Lifetime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NecroMacro.AddressablesTools
{
	[UsedImplicitly]
	public partial class AddressablesService : IAddressablesService
	{
		public async UniTask StartAsync(CancellationToken token)
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

		public async UniTask<T> Load<T>(
			AssetReferenceT<T> reference,
			IDisposer disposer,
			CancellationToken token,
			IProgress<float> progress = null
		) where T : UnityEngine.Object
		{
			var handle = reference.LoadAssetAsync<T>();

			var asset = await AwaitHandle(handle, token, progress);
			if (asset == null) return null;

			disposer?.Add(new AssetHandle<T>(handle));
			return asset;
		}

		public async UniTask<GameObject> Instantiate(
			AssetReference reference,
			IDisposer disposer,
			CancellationToken token,
			IProgress<float> progress = null
		)
		{
			var handle = reference.InstantiateAsync();

			var instance = await AwaitHandle(handle, token, progress);
			if (instance == null) return null;

			disposer?.Add(new InstanceHandle(handle));
			return instance;
		}

		public async UniTask<IList<T>> LoadByKey<T>(
			object key,
			IDisposer disposer,
			CancellationToken token,
			IProgress<float> progress = null
		)
		{
			var handle = Addressables.LoadAssetsAsync<T>(key, null);

			var assets = await AwaitHandle(handle, token, progress);
			if (assets == null) return null;

			disposer?.Add(new AssetHandle<IList<T>>(handle));
			return assets;
		}
	}
}