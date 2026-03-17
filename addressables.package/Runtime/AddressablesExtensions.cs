using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NecroMacro.Core.Lifetime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NecroMacro.AddressablesTools
{
	public static class AddressablesExtensions
	{
		public static async UniTask<T> Load<T>(
			this AssetReferenceT<T> reference,
			CancellationToken token = default,
			IDisposer lifetime = null,
			IProgress<float> progress = null
		) where T : UnityEngine.Object
		{
			var handle = Addressables.LoadAssetAsync<T>(reference.AssetGUID);

			var asset = await handle.AwaitHandle(token, progress);
			if (asset == null) return null;

			if (lifetime != null)
			{
				var releaseHandle = new AssetHandle<T>(handle);
				lifetime?.Add(releaseHandle);
			}
			
			return asset;
		}
		
		public static async UniTask<TComponent> Instantiate<TComponent>(
			this AssetReference reference,
			CancellationToken token = default,
			Transform parent = null,
			IDisposer lifetime = null,
			IProgress<float> progress = null
		) where TComponent : class
		{
			var handle = reference.InstantiateAsync(parent);

			var instance = await handle.AwaitHandle(token, progress);
			if (instance == null) return null;

			if (lifetime != null)
			{
				var releaseHandle = new InstanceHandle(handle);
				lifetime?.Add(releaseHandle);
			}
			
			if (typeof(TComponent) == typeof(GameObject)) 
				return instance as TComponent;
			
			instance.TryGetComponent<TComponent>(out var component);
			return component;
		}

		public static async UniTask<TComponent> Instantiate<TComponent>(
			this object key,
			CancellationToken token = default,
			Transform parent = null,
			IDisposer lifetime = null,
			IProgress<float> progress = null
		) where TComponent : UnityEngine.Object 
		{
			var handle = Addressables.InstantiateAsync(key, parent);

			var instance = await handle.AwaitHandle(token, progress);
			if (instance == null) return null;

			if (lifetime != null)
			{
				var releaseHandle = new InstanceHandle(handle);
				lifetime?.Add(releaseHandle);
			}
			
			if (typeof(TComponent) == typeof(GameObject)) 
				return instance as TComponent;
			
			instance.TryGetComponent<TComponent>(out var component);
			return component;
		}
		
		public static async UniTask<IList<T>> LoadByKey<T>(
			this object key,
			CancellationToken token = default,
			IDisposer lifetime = null,
			IProgress<float> progress = null
		)
		{
			var handle = Addressables.LoadAssetsAsync<T>(key, null);

			var assets = await handle.AwaitHandle(token, progress);
			if (assets == null) return null;
			
			if (lifetime != null)
			{
				var releaseHandle = new AssetHandle<IList<T>>(handle);
				lifetime?.Add(releaseHandle);
			}
			
			return assets;
		}

		private static async UniTask<T> AwaitHandle<T>(
			this AsyncOperationHandle<T> handle, CancellationToken token, IProgress<float> progress
		)
		{
			var result = await handle
			                   .ToUniTask(progress, PlayerLoopTiming.Update, token)
			                   .SuppressCancellationThrow();

			if (result.IsCanceled || result.Result == null)
			{
				Addressables.Release(handle);
				return default;
			}

			return result.Result;
		}
	}
}