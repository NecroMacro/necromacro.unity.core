using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

#pragma warning disable CS4014

namespace NecroMacro.Core.Timeline
{
	public static class PlayableDirectorExtensions
	{
		private static readonly Dictionary<PlayableDirector, PlayableDirectorObserver> _observersMap = new();
		private static readonly Dictionary<PlayableDirector, Action> _completeHandlers = new();
		
		public static void PlayExt(this PlayableDirector director, Action onComplete = null)
		{
			if (!director.isActiveAndEnabled)
				return;

			var observer = GetObserver(director);
			if (_completeHandlers.ContainsKey(director))
			{
				observer.Completed -= _completeHandlers[director];
				_completeHandlers.Remove(director);
			}

			if (onComplete != null && director.extrapolationMode != DirectorWrapMode.Loop)
			{
				void onCompleteWrapper()
				{
					observer.Completed -= onCompleteWrapper;
					_completeHandlers.Remove(director);
					onComplete?.Invoke();
				}

				_completeHandlers[director] = onCompleteWrapper;
				observer.Completed += onCompleteWrapper;
			}

			if (director.extrapolationMode == DirectorWrapMode.Hold)
			{
				director.Stop(); // Если этого не сделать, то колбек не отработает
			}

			director.Play();

			if (!director.isActiveAndEnabled || !director.playableGraph.IsValid())
				return;

			var rootPlayable = director.playableGraph.GetRootPlayable(0);
			rootPlayable.SetTime(0f);
		}

		private static PlayableDirectorObserver GetObserver(PlayableDirector director)
		{
			if (!_observersMap.ContainsKey(director))
			{
				var gameObject = director.gameObject;
				var observer = gameObject.GetComponent<PlayableDirectorObserver>();
				if (!observer)
				{
					//AnimationsDependencies.Logger.Warning($"{gameObject.name} does not contain PlayableDirectorObserver component, it will be added during runtime, but recommended to add it from editor!");
					Debug.LogWarning(
						$"{gameObject.name} does not contain PlayableDirectorObserver component, it will be added during runtime, but recommended to add it from editor!"
					);
					observer = gameObject.AddComponent<PlayableDirectorObserver>();
				}
				_observersMap[director] = observer;
			}
			return _observersMap[director];
		}
	}
}