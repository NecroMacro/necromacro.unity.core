using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NecroMacro.AddressablesTools;
using NecroMacro.Core;
using NecroMacro.Core.Extensions;
using NecroMacro.Core.Lifetime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace NecroMacro.UI
{
	public class WidgetFactory
	{
		private readonly IObjectResolver resolver;
		
		private readonly WidgetCache activeWidgets;
		private readonly WidgetCache inactiveWidgets;
		private IWidget currentTopWidget;
		private readonly HashSet<IUIProps> loadingWidgets = new();
		
		public WidgetFactory(SignalBus signalBus, IObjectResolver resolver)
		{
			this.resolver = resolver;
			
			activeWidgets = new WidgetCache();
			inactiveWidgets = new WidgetCache();

			signalBus.Subscribe<WidgetOnCloseSignal>(WidgetCloseSignalHandler);
		}

		public void CreateWidget(IUIProps props, Action<IWidget> onCreated)
		{
			var task = CreateWidget(props);
			if (onCreated != null) task.ContinueWith(onCreated.Invoke);
			task.HandleException(HandleCreatingException);
		}

		public async UniTask<IWidget> CreateWidget(IUIProps props)
		{
			if (props == null) return null;
			
			if (props.SingleInstanceInQueue)
			{
				await UniTask.WaitWhile(() => loadingWidgets.Contains(props));
				
				var another = activeWidgets.Get(props);
				if (another != null) 
					return another;
			}
			
			var widget = inactiveWidgets.GetAndRemove(props);
			if (widget != null)
			{
				widget.ApplyProps(props);
				if (props.Parent) widget.SetParent(props.Parent);
				CheckOverlapped(props, widget);
				activeWidgets.Add(props, widget);
				return widget;
			}

			loadingWidgets.Add(props);
			
			widget = await ValidateAndCreate(props);
			//resolver.Inject(widget);
			resolver.InjectGameObject(widget.GameObject);
			widget.Construct();
			widget.ApplyProps(props);
			CheckOverlapped(props, widget);
			activeWidgets.Add(props, widget);
			await widget.Prepare();
			
			loadingWidgets.Remove(props);
			return widget;
		}

		public IWidget GetActiveWidget(AssetReference reference)
		{
			return activeWidgets.Get(reference);
		}

		private async UniTask<IWidget> ValidateAndCreate(IUIProps props)
		{
			props.Validate();
			var parent = props.Parent ? props.Parent.transform : null;

			var widgetLifeTime = resolver.Resolve<ILifeCycleDisposer>();
			var widget = await props.Asset.Instantiate<IWidget>(CancellationToken.None, parent, widgetLifeTime);
			widget.SetLifetime(widgetLifeTime);
			return widget;
		}
		
		private void CheckOverlapped(IUIProps props, IWidget widget)
		{
			if (props.CanBeHiddenByOtherElement) return;
			
			if (currentTopWidget != null && currentTopWidget.RawProps != widget.RawProps)
			{
				Debug.LogWarning($"Another widget on top layer: {currentTopWidget.RawProps.GetType()}");
				return;
			}
			
			widget.GameObject.transform.SetAsLastSibling();
			currentTopWidget = widget;
		}
		
		private void HandleCreatingException(Exception exception)
		{
			Debug.LogError(exception);
		}

		private void WidgetCloseSignalHandler(WidgetOnCloseSignal signal)
		{
			var props = signal.Props;
			var widget = signal.Widget;
			if (signal.Widget == null) 
				return;

			if (currentTopWidget == widget)
				currentTopWidget = null;
			
			if (signal.Props.KeepInCacheAfterClose)
			{
				inactiveWidgets.Add(props, widget);
				activeWidgets.Remove(props, widget);
				return;
			}

			activeWidgets.Remove(props, widget);
			widget.LifeTime.Dispose();
		}
	}
}
