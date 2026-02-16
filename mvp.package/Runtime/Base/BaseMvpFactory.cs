using System;
using Cysharp.Threading.Tasks;
using NecroMacro.Core.Lifetime;
using UnityEngine;
using VContainer;

namespace NecroMacro.MVP
{
	public interface IMvpFactory
	{
		UniTask SpawnAsync<TModel, TView, TPresenter, TData>(TData data, Transform transform, Vector3? position, IDisposer disposer)
				where TModel : IFactoryModel<TData>
				where TPresenter : IFactoryPresenter<TModel, TView>
				where TView : MonoBehaviour;
	}
	
	public class GenericMvpFactory : IMvpFactory
	{
		private readonly IObjectResolver resolver;

		public GenericMvpFactory(IObjectResolver resolver)
		{
			this.resolver = resolver;
		}

		public async UniTask SpawnAsync<TModel, TView, TPresenter, TData>(
			TData data,
			Transform transform,
			Vector3? position,
			IDisposer disposer)
			where TModel : IFactoryModel<TData>
			where TPresenter : IFactoryPresenter<TModel, TView>
			where TView : MonoBehaviour
		{
			var modelFactory = resolver.Resolve<Func<TData, TModel>>();
			var presenterFactory = resolver.Resolve<Func<TModel, TView, TPresenter>>();
			var viewFactory = resolver.Resolve<Func<Vector3, UniTask<TView>>>();

			Vector3 spawnPos = position ?? Vector3.zero;

			TView view = await viewFactory(spawnPos);
			view.transform.SetParent(transform);

			TModel model = modelFactory(data);
			TPresenter presenter = presenterFactory(model, view);

			disposer.Add(model);
			disposer.Add(presenter);
		}

	}
}