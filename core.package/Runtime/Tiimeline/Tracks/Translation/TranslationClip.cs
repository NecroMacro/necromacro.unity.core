using System;
using UnityEngine;

namespace NecroMacro.Core.Timeline {
    [Serializable]
    public class TranslationClip : AbstractClipWithExposedLerpValue<TranslationBehavior, Transform, Vector3> {}
}