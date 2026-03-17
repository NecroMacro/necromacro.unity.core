using System;
using Cysharp.Threading.Tasks;
using NecroMacro.Core.Timeline;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

namespace NecroMacro.UI
{
	public class UIAppearController : MonoBehaviour, IVisibility
	{
		[Button("Appear")]
		private void Appear() => SetVisibilityState(VisibilityState.Appearing).Forget();
		
		[Button("Hide")]
		private void Hide() => SetVisibilityState(VisibilityState.Hiding).Forget();

		[Button("Visible")]
		private void Visible() => SetVisibilityState(VisibilityState.Visible).Forget();

		[Button("Invisible")]
		private void Invisible() => SetVisibilityState(VisibilityState.Invisible).Forget();

		[SerializeField]
		private PlayableDirector appearDirector;
		[SerializeField] 
		private PlayableDirector hideDirector;
		[SerializeField] 
		private PlayableDirector visibleDirector;
		[SerializeField] 
		private PlayableDirector invisibleDirector;
		[SerializeField] 
		private PlayableDirector idleDirector;

		public VisibilityState VisualState { get; private set; } = VisibilityState.Unknown;

		private void Awake() 
		{
			Assert.IsNotNull(visibleDirector, $"{nameof(UIAppearController)}:: Visible director is null");
			Assert.IsNotNull(invisibleDirector, $"{nameof(UIAppearController)}:: Invisible director is null");

			StopAll();

			if (appearDirector)
			{
				appearDirector.playOnAwake = false;
				appearDirector.extrapolationMode = DirectorWrapMode.None;
			}
			if (hideDirector)
			{
				hideDirector.playOnAwake = false;
				hideDirector.extrapolationMode = DirectorWrapMode.None;
			}
			if (visibleDirector)
			{
				visibleDirector.playOnAwake = false;
				visibleDirector.extrapolationMode = DirectorWrapMode.Hold;
			}
			if (invisibleDirector)
			{
				invisibleDirector.playOnAwake = false;
				invisibleDirector.extrapolationMode = DirectorWrapMode.Hold;
			}
			if (idleDirector)
			{
				idleDirector.playOnAwake = false;
				idleDirector.extrapolationMode = DirectorWrapMode.Loop;
			}
		}

		private void StopAll() 
		{
			Stop(visibleDirector);
			Stop(invisibleDirector);
			Stop(appearDirector);
			Stop(hideDirector);
			Stop(idleDirector);
		}
		
		private static void Stop(PlayableDirector director) 
		{
			if (director)
				director.gameObject.SetActive(false);
		}

		public async UniTask SetVisibilityState(VisibilityState state)
		{
			switch (state) 
			{
				case VisibilityState.Unknown:
					break;
				case VisibilityState.Appearing:
					await SetVisibilityAnimated(true);
					break;
				case VisibilityState.Hiding:
					await SetVisibilityAnimated(false);
					break;
				case VisibilityState.Visible:
					SetVisibility(true);
					break;
				case VisibilityState.Invisible:
					SetVisibility(false);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}

		private void SetVisibility(bool isVisible) 
		{
			var director = isVisible ? visibleDirector : invisibleDirector;
			SetState(isVisible ? VisibilityState.Visible : VisibilityState.Invisible);
			Play(director).Forget();
		}

		private void SetState(VisibilityState state) 
		{
			VisualState = state;
			
			if (VisualState == VisibilityState.Visible) 
				Play(idleDirector).Forget();
		}

		private async UniTask SetVisibilityAnimated(bool isVisible) 
		{
			var director = isVisible ? appearDirector : hideDirector;
			
			if (!director) 
			{
				SetVisibility(isVisible);
				return;
			}

			SetState(isVisible ? VisibilityState.Appearing : VisibilityState.Hiding);
			
			await Play(director);
			
			SetState(isVisible ? VisibilityState.Visible : VisibilityState.Invisible);
		}

		private async UniTask Play(PlayableDirector director) 
		{
			if (!director || !director.playableAsset)
				return;

			StopAll();

			if (!director.gameObject.activeSelf) {
				director.gameObject.SetActive(true);
			}
			
			var tcs = new UniTaskCompletionSource();
			
			director.PlayExt(() =>
			{
				tcs.TrySetResult();
			});
			
			director.Evaluate();
			
			await tcs.Task;
		}
	}
}
