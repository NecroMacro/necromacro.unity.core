using UnityEngine;

namespace NecroMacro.Core.Timeline {
    public class AnchoredPositionMixerBehaviour : AbstractMixerBehaviorForVector2<AnchoredPositionBehavior, RectTransform> {
        protected override Vector2 GetInitialValue(RectTransform trackBinding) {
            return trackBinding.anchoredPosition;
        }
        
        protected override void SetValue(RectTransform trackBinding, Vector2 value) {
            trackBinding.anchoredPosition = value;
        }
    }
}