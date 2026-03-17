using UnityEngine;

namespace NecroMacro.Core.Timeline {
    public class ScaleBehavior : AbstractBehaviorWithLerpValue<Vector3> {
        public bool SaveStartSign;
        public bool IsRelative;
    }
}