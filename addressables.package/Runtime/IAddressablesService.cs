using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NecroMacro.Core.Lifetime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer.Unity;

namespace NecroMacro.AddressablesTools
{
	public interface IAddressablesService : IAsyncStartable
	{
		UniTask<T> Load<T>(
			AssetReferenceT<T> reference,
			IDisposer disposer,
			CancellationToken token = default,
			IProgress<float> progress = null
		) where T : UnityEngine.Object;

		UniTask<GameObject> Instantiate(
			AssetReference reference,
			IDisposer disposer,
			CancellationToken token = default,
			IProgress<float> progress = null
		);

		UniTask<IList<T>> LoadByKey<T>(
			object key,
			IDisposer disposer,
			CancellationToken token = default,
			IProgress<float> progress = null
		);

		UniTask<long> GetDownloadSizeAsync(
			object resource,
			CancellationToken token = default
		);

		UniTask<AsyncOperationStatus> DownloadDependenciesTaskAsync(
			object resource,
			CancellationToken token = default,
			bool autoReleaseHandle = true,
			IProgress<float> progress = null
		);
	}
}