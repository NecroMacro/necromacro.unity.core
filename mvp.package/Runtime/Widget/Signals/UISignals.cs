using NecroMacro.Core;

namespace NecroMacro.UI 
{
    public abstract class UISignal
    {
        public IUIProps Props;
    }

    public class WidgetOnCloseSignal : UISignal, ISignalIdentifier
    {
        public object SignalIdentifier { get; }
        public IWidget Widget { get; }
        
        public WidgetOnCloseSignal(IUIProps signalIdentifier, IWidget widget)
        {
            SignalIdentifier = signalIdentifier;
            Widget = widget;
            Props = signalIdentifier;
        }
    }
    
    public class WidgetOnPropsAppliedSignal : UISignal {}

    public class WidgetOnAfterHideSignal : UISignal, ISignalIdentifier
    {
        public object SignalIdentifier { get; }
        
        public WidgetOnAfterHideSignal(IUIProps signalIdentifier)
        {
            SignalIdentifier = signalIdentifier;
            Props = signalIdentifier;
        }
    }

    public class WidgetOnBeforeHideSignal : UISignal { }
    
    public class WidgetOnBeforeShowSignal : UISignal {}

    public class WidgetOnAfterShowSignal : UISignal, ISignalIdentifier
    {
        public object SignalIdentifier { get; }
        
        public WidgetOnAfterShowSignal(IUIProps signalIdentifier)
        {
            SignalIdentifier = signalIdentifier;
            Props = signalIdentifier;
        }
    }

}
