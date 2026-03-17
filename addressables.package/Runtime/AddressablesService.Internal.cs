using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NecroMacro.AddressablesTools
{
	public partial class AddressablesService
	{
		private static async UniTask UpdateCatalogs(CancellationToken token)
		{
			try
			{
				var checkCatalogUpdatesTask = Addressables.CheckForCatalogUpdates(false);
				var catalogsForUpdate = await checkCatalogUpdatesTask.ToUniTask(cancellationToken: token);

				if (checkCatalogUpdatesTask.OperationException != null)
				{
					Debug.LogWarning(
						$"{nameof(AddressablesService)}.{nameof(UpdateCatalogs)} exception:" +
						$"\n{checkCatalogUpdatesTask.OperationException}"
					);

					Addressables.Release(checkCatalogUpdatesTask);
					return;
				}

				Addressables.Release(checkCatalogUpdatesTask);

				if (catalogsForUpdate.Count > 0)
				{
					var updateCatalogsTask = Addressables.UpdateCatalogs(catalogsForUpdate, false);
					await updateCatalogsTask.ToUniTask(cancellationToken: token);

					if (updateCatalogsTask.OperationException != null)
					{
						Debug.LogWarning(
							$"{nameof(AddressablesService)}.{nameof(UpdateCatalogs)} exception:" +
							$"\n{updateCatalogsTask.OperationException}"
						);

						Addressables.Release(updateCatalogsTask);
						return;
					}

					Addressables.Release(updateCatalogsTask);
				}
			} catch (Exception e)
			{
				Debug.LogWarning($"{nameof(AddressablesService)}.{nameof(UpdateCatalogs)} Unexpected exception:{e}");
			}
		}

		private static async UniTask WaitWhileCachingReadyAsync(CancellationToken cancellationToken = default)
		{
			#if !UNITY_WEBGL
			if (Caching.ready) return;

			while (!Caching.ready)
			{
				await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
			}
			#endif
		}

		
	}
}