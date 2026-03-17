using System.Collections.Generic;
using System.Linq;
using DigitalRubyShared;
using NecroMacro.Core;
using NecroMacro.Core.Extensions;
using UnityEngine;
using VContainer;

namespace Core.MobileInput 
{
    [System.Flags]
    public enum GestureFlags
    {
        None = 0,
        Tap = 1 << 0,
        DoubleTap = 1 << 1,
        Tap4Fingers = 1 << 2,
        TapWithMultipleFingers = 1 << 3,
        DoubleTapWithMultipleFingers = 1 << 4,
        Pan = 1 << 5,
        Scale = 1 << 6,
        LongPress = 1 << 7,
        DragAndDrop = 1 << 8
    }
    
    public class GestureController : MonoBehaviour, IPausable
    {
	    [Inject] private readonly SignalBus signalBus;
	    [SerializeField] private FingersScript fingersScript;
        [SerializeField] private TapGestureRecognizerComponentScript tap;
        [SerializeField] private TapGestureRecognizerComponentScript doubleTap;
        [SerializeField] private PanGestureRecognizerComponentScript pan;
        [SerializeField] private ScaleGestureRecognizerComponentScript scale;
        
        private readonly LocksContainer pauseRequesters = new();
        private readonly HashSet<GameObject> gestureCatchers = new();
        public bool IsPaused => pauseRequesters.IsLocked;
        
        private void Awake()
        {
            tap.enabled = false;
            doubleTap.enabled = false;
            pan.enabled = false;
            scale.enabled = false;
        }
        
        private void Start() 
        {
	        fingersScript.CaptureGestureHandler = obj => {
		        if (obj.IsNull()) {
			        return false;
		        }
		        bool hasMatches = gestureCatchers.Any(gestureCatcher =>
			        gestureCatcher == obj || obj.transform.IsChildOf(gestureCatcher.transform));
		        
		        return hasMatches ? null : false;
	        };
        }
        
        public void EnableGestures(GestureFlags enabled)
        {
            if (enabled.HasFlag(GestureFlags.Tap))
                Configure(tap, true, TapGestureCallback);
            
            if (enabled.HasFlag(GestureFlags.DoubleTap))
                Configure(doubleTap, true, DoubleTapGestureCallback);
            
            if (enabled.HasFlag(GestureFlags.Pan))
                Configure(pan, true, PanGestureCallback);

            if (enabled.HasFlag(GestureFlags.Scale))
                Configure(scale, true, ScaleGestureCallback);
            
            // при необходимости добавить остальные:
            // Configure(tap4fingers, enabled, GestureFlags.Tap4Fingers, Tap4FingersGestureCallback);
            // Configure(tapWithMultipleFingers, enabled, GestureFlags.TapWithMultipleFingers, TapWithMultipleFingersGestureCallback);
            // Configure(doubleTapWithMultipleFingers, enabled, GestureFlags.DoubleTapWithMultipleFingers, DoubleTapWithMultipleFingersGestureCallback);
            // Configure(longPress, enabled, GestureFlags.LongPress, LongPressGestureCallback);
            // Configure(dragAndDrop, enabled, GestureFlags.DragAndDrop, DragAndDropGestureCallback);
        }

        public void DisableGestures()
        {
	        Configure(tap, false, TapGestureCallback);
	        Configure(doubleTap, false, DoubleTapGestureCallback);
	        Configure(pan, false, PanGestureCallback);
	        Configure(scale, false, ScaleGestureCallback);
	        // при необходимости добавить остальные:
	        // Configure(tap4fingers, enabled, GestureFlags.Tap4Fingers, Tap4FingersGestureCallback);
	        // Configure(tapWithMultipleFingers, enabled, GestureFlags.TapWithMultipleFingers, TapWithMultipleFingersGestureCallback);
	        // Configure(doubleTapWithMultipleFingers, enabled, GestureFlags.DoubleTapWithMultipleFingers, DoubleTapWithMultipleFingersGestureCallback);
	        // Configure(longPress, enabled, GestureFlags.LongPress, LongPressGestureCallback);
	        // Configure(dragAndDrop, enabled, GestureFlags.DragAndDrop, DragAndDropGestureCallback);
        }

        private static void Configure<T>(
            GestureRecognizerComponentScript<T> component, bool status,
            GestureRecognizerStateUpdatedDelegate handler
        )
            where T : GestureRecognizer, new()
        {
            if (!component)
                return;

            if (component.enabled == status)
                return;
            
            component.enabled = status;

            var gesture = component.Gesture;
            if (gesture == null || handler == null)
                return;

            if (status) gesture.StateUpdated += handler;
            else gesture.StateUpdated -= handler;
        }
        

        private void OnDestroy() 
        {
            tap.Gesture.StateUpdated -= TapGestureCallback;
            doubleTap.Gesture.StateUpdated -= DoubleTapGestureCallback;
            pan.Gesture.StateUpdated -= PanGestureCallback;
            scale.Gesture.StateUpdated -= ScaleGestureCallback;
            
            /*
			tap4fingers.Gesture.StateUpdated -= Tap4FingersGestureCallback;
            longPress.Gesture.StateUpdated -= LongPressGestureCallback;
            dragAndDrop.Gesture.StateUpdated -= DragAndDropGestureCallback;
            */
        }

        private void TapGestureCallback(GestureRecognizer gesture) 
        {
            if (gesture.State == GestureRecognizerState.Failed)
            {
                return;
            }
            
            if (gesture.CurrentTrackedTouches.IsEmpty())
                return;
            
            var touch = gesture.CurrentTrackedTouches.First();
            
            switch (gesture.State)
            {
                case GestureRecognizerState.Possible:
                    //gesture.IsStartOverUi = EventSystem.current?.IsPointerOverGameObject(touch.Id) ?? false;
                    break;
                case GestureRecognizerState.Began:
                    Publish(new TapStartedSignal(gesture.FocusX, gesture.FocusY));
                    break;
                case GestureRecognizerState.Ended:
                    Publish(new TapEndedSignal(gesture.FocusX, gesture.FocusY, false/*gesture.IsStartOverUi!.Value*/));
                    break;
                case GestureRecognizerState.Failed:
                    Publish(new TapFailedSignal(gesture.FocusX, gesture.FocusY));
                    break;
            }
        }
        
        private void Tap4FingersGestureCallback(GestureRecognizer gesture) {
            if (gesture.State == GestureRecognizerState.Ended) {
                Publish(new Tap4FingersEndedSignal(gesture.FocusX, gesture.FocusY));
            }
        }

        private void DoubleTapGestureCallback(GestureRecognizer gesture) {
            if (gesture.State == GestureRecognizerState.Ended) {
                Publish(new DoubleTapEndedSignal(gesture.FocusX, gesture.FocusY));
            }
        }
        
        private void PanGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Failed)
            {
                //gesture.IsStartOverUi = null;
                return;
            }
            
            if (gesture.CurrentTrackedTouches.IsEmpty()) return;
            
            var touch = gesture.CurrentTrackedTouches.First();

            // - 1 это ЛКМ
            int id = touch.PlatformSpecificTouch is int i ? -1 : touch.Id;
            
            switch (gesture.State) {
                case GestureRecognizerState.Possible:
                    //gesture.IsStartOverUi = EventSystem.current?.IsPointerOverGameObject(id) ?? false;
                    break;
                case GestureRecognizerState.Began:
                    Publish(new PanStartedSignal(gesture.StartFocusX, gesture.StartFocusY, /*gesture.IsStartOverUi!.Value*/false));
                    break;
                case GestureRecognizerState.Executing:
                    Publish(new PanMovedSignal(gesture.FocusX, gesture.FocusY, gesture.DeltaX, gesture.DeltaY, false/*gesture.IsStartOverUi!.Value*/));
                    break;
                case GestureRecognizerState.Ended:
                    Publish(new PanEndedSignal(gesture.FocusX, gesture.FocusY, gesture.VelocityX, gesture.VelocityY));
                    break;
            }
        }
        
        private void ScaleGestureCallback(GestureRecognizer gesture) {
            switch (gesture.State) {
                case GestureRecognizerState.Began:
                    break;
                case GestureRecognizerState.Executing:
                    Publish(new ScaleMovedSignal(scale.Gesture.ScaleMultiplier, scale.Gesture.ScaleMultiplierRange));
                    break;
                case GestureRecognizerState.Ended:
                    //Publish(new ScaleEndedSignal());
                    break;
            }
        }
        
        /*private void TapWithMultipleFingersGestureCallback(GestureRecognizer gesture) {
            if (gesture is not TapGestureRecognizer tapGestureRecognizer) {
                return;
            }
            
            if (gesture.State == GestureRecognizerState.Ended) {
                Publish(new TapWithMultipleFingersEndedSignal(gesture.FocusX, gesture.FocusY, tapGestureRecognizer.TapTouches.Count));
            }
        }

        private void DoubleTapWithMultipleFingersGestureCallback(GestureRecognizer gesture) {
            if (gesture is not TapGestureRecognizer tapGestureRecognizer) {
                return;
            }
            
            if (gesture.State == GestureRecognizerState.Ended) {
                Publish(new DoubleTapWithMultipleFingersEndedSignal(gesture.FocusX, gesture.FocusY, tapGestureRecognizer.TapTouches.Count));
            }
        }

        private void DoubleTapGestureCallback(GestureRecognizer gesture) {
            if (gesture.State == GestureRecognizerState.Ended) {
                Publish(new DoubleTapEndedSignal(gesture.FocusX, gesture.FocusY));
            }
        }

        

        

        private void LongPressGestureCallback(GestureRecognizer gesture) {
            switch (gesture.State) {
                case GestureRecognizerState.Began:
                    Publish(new LongTapStartedSignal(gesture.FocusX, gesture.FocusY));
                    break;
                case GestureRecognizerState.Executing:
                    Publish(new LongTapMovedSignal(
                        gesture.FocusX,
                        gesture.FocusY,
                        gesture.StartFocusX,
                        gesture.StartFocusY));
                    break;
                case GestureRecognizerState.Ended:
                    Publish(new LongTapEndedSignal(gesture.FocusX, gesture.FocusY));
                    break;
                case GestureRecognizerState.Failed:
                    Publish(new LongTapFailedSignal(gesture.FocusX, gesture.FocusY));
                    break;
            }
        }

        private void DragAndDropGestureCallback(GestureRecognizer gesture) {
            switch (gesture.State) {
                case GestureRecognizerState.Began:
                    Publish(new DragStartedSignal(gesture.FocusX, gesture.FocusY));
                    break;
                case GestureRecognizerState.Executing:
                    Publish(new DragMovedSignal(gesture.FocusX, gesture.FocusY));
                    break;
                case GestureRecognizerState.Ended:
                    Publish(new DragEndedSignal(gesture.FocusX, gesture.FocusY, gesture.VelocityX, gesture.VelocityY));
                    break;
            }
        }*/
       

        internal void RegisterGestureCatcher(GestureCatcher gestureCatcher) 
        {
	        gestureCatchers.Add(gestureCatcher.gameObject);
        }

        internal void UnregisterGestureCatcher(GestureCatcher gestureCatcher) 
        {
	        gestureCatchers.Remove(gestureCatcher.gameObject);
        }
        
        private void Publish<TSignal>(TSignal signal) 
        {
            if (IsPaused) return;
            
            signalBus.Fire(signal);
        }
        
        public void Pause(object requester)
        {
	        pauseRequesters.Lock(requester);
	        ResetGestures();
        }

        public void Resume(object requester)
        {
	        pauseRequesters.Unlock(requester); 
        }
        
        public void ResetGestures() 
        {
	        foreach (var gesture in fingersScript.Gestures) 
	        {
		        gesture.Reset();
	        }
        }
    }
}