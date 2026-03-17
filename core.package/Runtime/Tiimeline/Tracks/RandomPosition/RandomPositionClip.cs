using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace NecroMacro.Core.Timeline {
    [Serializable]
    public class RandomPositionClip : AbstractClip<RandomPositionBehavior> {
        public override ClipCaps clipCaps => ClipCaps.None;

        public TranslationPointData RectPosition = new TranslationPointData(Vector3.zero);
        public ExposedReference<Transform> RectScaleDependence;
        public Vector2 RectSize;

        protected override void FillBehavior(RandomPositionBehavior behavior, IExposedPropertyTable resolver) {
            RectPosition.RuntimeResolver = resolver;
            behavior.RectSize = RectSize;
            behavior.RectPosition = RectPosition;
            behavior.RectScaleDependence = RectScaleDependence.Resolve(resolver);
        }

        private void Reset() {
            var defaultRectSize = Vector2.one * 20f;
            RectSize = defaultRectSize;
        }
    }
}