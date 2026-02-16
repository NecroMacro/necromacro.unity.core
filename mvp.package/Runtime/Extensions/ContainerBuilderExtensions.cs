using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace NecroMacro.MVP
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterAddressableViewFactory<T>(
            this IContainerBuilder builder,
            AssetReferenceGameObject reference,
            Lifetime lifetime)
            where T : Component
        {
            builder.RegisterFactory<Vector3, UniTask<T>>(resolver =>
            {
                return async position =>
                {
                    var handle = reference.InstantiateAsync(position, Quaternion.identity);
                    var obj = await handle.Task;

                    return obj.GetComponent<T>();
                };
            }, lifetime);
        }
        
        public static void RegisterMVPFactory<TModel, TData, TView, TPresenter>(this IContainerBuilder builder, Lifetime lifetime)
            where TModel : IFactoryModel<TData>, new()
            where TPresenter : IFactoryPresenter<TModel, TView>, new()
        {
            builder.RegisterFactory<TData, TModel>(resolver =>
            {
                return data =>
                {
                    var model = new TModel();
                    resolver.Inject(model);
                    model.Initialize(data);
                    return model;
                };
            }, lifetime);
    
            builder.RegisterFactory<TModel, TView, TPresenter>(resolver =>
            {
                return (model, view) =>
                {
                    var presenter = new TPresenter();
                    resolver.Inject(presenter);
                    presenter.Initialize(model, view);
                    return presenter;
                };
            }, lifetime);
        }
    }
}
