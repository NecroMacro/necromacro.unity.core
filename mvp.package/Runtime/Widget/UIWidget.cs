using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NecroMacro.Core;
using NecroMacro.Core.Extensions;
using NecroMacro.Core.Lifetime;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NecroMacro.UI
{
	public abstract class UIWidget<TProps> : MonoBehaviour, IWidget where TProps : class, IUIProps
	{
		[Inject] protected readonly SignalBus signalBus;
		
		[SerializeField]
		[HideInInspector]
		[CanBeNull]
		protected Canvas Canvas;

		[SerializeField]
		[HideInInspector]
		[CanBeNull]
		protected GraphicRaycaster GraphicRaycaster;

		[SerializeField]
		[HideInInspector]
		[CanBeNull]
		protected ExtendedCanvasScaler CanvasScaler;
		
		[SerializeField]
		protected UIAppearController AppearController;
		
		[SerializeField]
		protected Button CloseButton;
		
		public VisibilityState VisualState => AppearController.VisualState;
		public WidgetStatus Status { get; private set; }

		public TProps Props { get; private set; }
		public IUIProps RawProps => Props;
		protected ILifeCycleDisposer lifetime;
		public IDisposer LifeTime => lifetime;
		public virtual Vector3 Position => transform.position;
		public bool IsStable => !AppearController || !AppearController.VisualState.IsAnimated();
		public GameObject GameObject => this ? gameObject : null;

		public void SetLifetime(ILifeCycleDisposer lifetime)
		{
			if (this.lifetime != null) return;
			this.lifetime = lifetime;
		}
		
		public void Construct()
		{
			Status = WidgetStatus.Initializing;
			OnPreInit();

			TryInitComponents();

			try
			{
				OnConstructed();
			} catch (Exception e)
			{
				Debug.LogException(e);
			}

			SetObjectVisibility(false);
			TrySubscribeCloseButton();
		}

		private void TrySubscribeCloseButton()
		{
			if(CloseButton == null) return;
			CloseButton.onClick.AddListener(() =>
				{
					Close().Forget();
				}
			);
		}

		public void ApplyProps(IUIProps props)
		{
			Props = props as TProps;
			
			if (Props == null)
			{
				Debug.LogError($"Try to init widget({name}) with incompatible props!");
				return;
			}
			
			Status = WidgetStatus.Initialized;
			
			try
			{
				OnPropsApplied();
			} catch (Exception e)
			{
				Debug.LogException(e);
			}
			
			signalBus.Fire(new WidgetOnPropsAppliedSignal { Props = props });
		}

		public async UniTask Show(bool animated = true)
		{
			await SetVisibilityState(animated ? VisibilityState.Appearing : VisibilityState.Visible);
		}
		
		public async UniTask Hide(bool animated = true)
		{
			await SetVisibilityState(animated ? VisibilityState.Hiding : VisibilityState.Invisible);
		}

		public virtual UniTask Prepare()
		{
			return UniTask.CompletedTask;
		}

		public async UniTask SetVisibilityState(VisibilityState state)
		{
			if (!VisibilityStateCanBeChanged(state, out string warning))
			{
				if (!string.IsNullOrEmpty(warning)) Debug.LogWarning(warning);
				return;
			}

			switch (state)
			{
				case VisibilityState.Unknown:
					return;
				case VisibilityState.Appearing or VisibilityState.Visible:
					PrepareToShow();
					break;
				case VisibilityState.Hiding or VisibilityState.Invisible:
					PrepareToHide();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}

			await ProceedAnimation(state);
		}

		public virtual async UniTask Close()
		{
			await CloseInternal(true);
		}

		public virtual void CloseImmediate()
		{
			CloseInternal(false).Forget();
		}

		public void SetParent(Transform parent)
		{
			transform.SetParent(parent);
		}

		private async UniTask ProceedAnimation(VisibilityState state)
		{
			if (AppearController != null)
			{
				await AppearController.SetVisibilityState(state);
				OnComplete();
				await UniTask.WaitUntil(() => IsStable);
			} else
			{
				OnComplete();
			}
			return;

			void OnComplete()
			{
				switch (state)
				{
					case VisibilityState.Appearing or VisibilityState.Visible:
						OnShown();
						break;
					case VisibilityState.Hiding or VisibilityState.Invisible:
						OnHidden();
						break;
				}
			}
		}

		private void TryInitComponents()
		{
			if (Canvas.IsNull()) Canvas = GetComponent<Canvas>();
			if (AppearController == null)
				AppearController = GetComponent<UIAppearController>() ?? gameObject.AddComponent<UIAppearController>();
			if (GraphicRaycaster.IsNull())
				GraphicRaycaster = Canvas != null ? Canvas.GetComponent<GraphicRaycaster>() : null;
			if (CanvasScaler.IsNull()) CanvasScaler = gameObject.GetComponent<ExtendedCanvasScaler>();
		}

		private bool VisibilityStateCanBeChanged(VisibilityState visibilityState, out string warning)
		{
			warning = string.Empty;
			switch (visibilityState)
			{
				case VisibilityState.Appearing or VisibilityState.Visible when !CanBeShown(out warning):
				case VisibilityState.Hiding or VisibilityState.Invisible when !CanBeHidden(out warning):
					return false;
				default:
					return true;
			}
		}

		private bool CanBeShown(out string warning)
		{
			switch (Status)
			{
				case WidgetStatus.Shown:
					warning = "Trying to show already shown widget!";
					return false;
				case WidgetStatus.Closing or WidgetStatus.Closed:
					warning = "Trying to show a closed widget!";
					return false;
			}
			warning = string.Empty;
			return true;
		}

		private bool CanBeHidden(out string warning)
		{
			switch (Status)
			{
				case WidgetStatus.Hidden:
					warning = "Trying to hide a hiding widget!";
					return false;
				case WidgetStatus.Closed:
					warning = "Trying to hide a closed widget!";
					return false;
			}
			warning = string.Empty;
			return true;
		}

		private void PrepareToShow()
		{
			GraphicRaycaster!.enabled = false;
			
			SetObjectVisibility(true);
			TryRefreshCanvasScaler();

			try
			{
				OnShowingBegin();
			} catch (Exception e)
			{
				Debug.LogException(e);
			}

			signalBus.Fire(new WidgetOnBeforeShowSignal { Props = Props });
			Status = WidgetStatus.Shown;
		}

		private void TryRefreshCanvasScaler()
		{
			if (CanvasScaler.IsNotNull()) CanvasScaler.Refresh();
		}

		private void PrepareToHide()
		{
			GraphicRaycaster!.enabled = false;
			
			BeforeHide();

			Status = WidgetStatus.Hidden;
			signalBus.Fire(new WidgetOnBeforeHideSignal { Props = Props });
		}

		private async UniTask CloseInternal(bool animated)
		{
			if (!GameObject) return; 
			switch (Status)
			{
				case WidgetStatus.Closed or WidgetStatus.Closing/* or WidgetStatus.Hidden*/:
					//Log.Warning("Trying to close already closed widget!");
					return;
			}

			Status = WidgetStatus.Closing;

			var visibleState = GameObject.activeSelf && animated ? VisibilityState.Hiding : VisibilityState.Invisible;
			await SetVisibilityState(visibleState);
			CompleteClose();
		}

		private void OnShown()
		{
			GraphicRaycaster!.enabled = true;
			signalBus.Fire(new WidgetOnAfterShowSignal(Props));
			OnShowingEnd();
		}

		private void OnHidden()
		{
			SetObjectVisibility(false);
			AfterHide();

			signalBus.Fire(new WidgetOnAfterHideSignal(Props) { Props = Props, });
		}

		private void CompleteClose()
		{
			BeforeClose();
			signalBus.Fire(new WidgetOnCloseSignal(Props, this));
			Status = WidgetStatus.Closed;
			AfterClose();
		}

		private void SetObjectVisibility(bool value)
		{
			if (Canvas != null) Canvas.enabled = value;
			if (GameObject) GameObject.SetActive(value);
		}

		protected virtual void OnPreInit() { }
		protected virtual void OnConstructed() { }
		protected virtual void OnPropsApplied() { }
		protected virtual void OnShowingBegin() { }
		protected virtual void OnShowingEnd() { }
		protected virtual void BeforeHide() { }
		protected virtual void AfterHide() { }
		protected virtual void BeforeClose() { }
		protected virtual void AfterClose() { }
		protected virtual void OnDestroy() { }

		private string SummaryInfo
		{
			get
			{
				string type = Props != null ? Props.WidgetType.ToString() : "Widget";
				string goName = GameObject ? GameObject.name : "Null-GameObject";
				return $"<color=white>{type}:</color><color=green> {goName}</color>";
			}
		}

		private void OnValidate()
		{
			TryInitComponents();
		}
	}
}