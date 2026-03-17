using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NecroMacro.Core.Timeline {
    [Serializable]
    public class AbstractClipWithExposedLerpValue<TBehavior, TExposedValue, TValue> : AbstractClipWithLerpValue<TBehavior, ExposedReference<TExposedValue>, TValue>
        where TBehavior : AbstractBehaviorWithExposedLerpValue<TExposedValue, TValue>, new()
        where TExposedValue : Object {
        protected override bool NeedToShowStartValue => false;
        protected override bool NeedToShowEndValue => false;
        
        protected virtual bool NeedToShowExposedStartValue => !UseInitialValueAsStart;
        protected virtual bool NeedToShowExposedEndValue => !UseInitialValueAsEnd;
        
        [ShowIf(nameof(NeedToShowExposedStartValue))]
        public ExposedReference<TExposedValue> ExposedStartValue;
        
        [ShowIf(nameof(NeedToShowExposedEndValue))]
        public ExposedReference<TExposedValue> ExposedEndValue;

        protected override TValue BehaviorStartValue => throw new Exception("Not used");
        protected override TValue BehaviorEndValue => throw new Exception("Not used");
        
        protected override void FillBehavior(TBehavior behavior, IExposedPropertyTable resolver) {
            behavior.Curve = Curve;
            behavior.UseInitialValueAsStart = UseInitialValueAsStart;
            behavior.UseInitialValueAsEnd = UseInitialValueAsEnd;
            if (!UseInitialValueAsStart) {
                behavior.ExposedStartValue = ExposedStartValue.Resolve(resolver);
            }
            if (!UseInitialValueAsEnd) {
                behavior.ExposedEndValue = ExposedEndValue.Resolve(resolver);
            }
            // !!! do not call base method
        }
    }
}