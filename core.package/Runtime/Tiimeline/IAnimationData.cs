using UnityEngine;
using UnityEngine.Playables;

namespace NecroMacro.Core.Timeline
{
    public interface IAnimationData {
        PlayableAsset Asset { get; }
        Object GetGenericBinding(PlayableBinding trackAsset, int index);
    }
}
