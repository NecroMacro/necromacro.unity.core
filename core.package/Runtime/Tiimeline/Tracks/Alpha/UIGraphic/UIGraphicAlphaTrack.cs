using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace NecroMacro.Core.Timeline
{
	[TrackColor(0.855f, 0.8623f, 0.870f)]
	[TrackClipType(typeof(AlphaClip))]
	[TrackBindingType(typeof(Graphic))]
	public class UIGraphicAlphaTrack : AbstractTrack
	{
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			return ScriptPlayable<UIGraphicAlphaTweenMixerBehavior>.Create(graph, inputCount);
		}
	}
}