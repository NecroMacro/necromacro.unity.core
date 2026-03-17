using UnityEngine;
using UnityEngine.Playables;

namespace NecroMacro.Core.Timeline {
    public class RandomPositionBehavior : AbstractBehavior {
        public Vector2 RectSize;
        public TranslationPointData RectPosition;
        public Transform RectScaleDependence;

        protected override bool ExecuteOnce => true;

        protected override void ProcessFrameInternal(Playable playable, FrameData info, object playerData) {
            var trackBinding = playerData as Transform;
            if (trackBinding == null || RectPosition == null) {
                return;
            }
            RectPosition.RuntimeResolver = playable.GetGraph().GetResolver();
            var scaleDependence = RectScaleDependence.localScale;
            var halfXSize = RectSize.x / 2 * scaleDependence.x;
            var halfYSize = RectSize.y / 2 * scaleDependence.y;
            var randomOffset = new Vector3(Random.Range(-halfXSize, halfXSize), Random.Range(-halfYSize, halfYSize));
            trackBinding.position = RectPosition.Position + randomOffset;
            base.ProcessFrameInternal(playable, info, playerData);
        }
    }
}