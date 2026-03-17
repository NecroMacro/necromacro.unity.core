using UnityEngine;
using UnityEngine.Timeline;

namespace NecroMacro.Core.Timeline {
    [TrackColor(0.855f, 0.8623f, 0.870f)]
    [TrackClipType(typeof(RandomPositionClip))]
    [TrackBindingType(typeof(Transform))]
    public class RandomPositionTrack : AbstractTrack {}
}