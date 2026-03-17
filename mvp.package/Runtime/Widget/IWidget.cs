using Cysharp.Threading.Tasks;
using NecroMacro.Core.Lifetime;
using UnityEngine;

namespace NecroMacro.UI {
    public interface IWidget : IVisibility 
    {
        GameObject GameObject { get; }
        WidgetStatus Status { get; }
        IUIProps RawProps { get; }
        IDisposer LifeTime { get; }
        void Construct();
        void ApplyProps(IUIProps props);
        void SetParent(Transform parent);
        UniTask Close();
        void CloseImmediate();
        UniTask Show(bool animated = true);
        UniTask Hide(bool animated = true);
        UniTask Prepare();
        void SetLifetime(ILifeCycleDisposer lifetime);
    }
    
    public static class WidgetExtensions 
    {
        public static bool IsShown(this IWidget widget) 
        {
            return widget is { Status: WidgetStatus.Shown };
        }
        public static bool IsHidden(this IWidget widget) 
        {
            return widget is { Status: WidgetStatus.Hidden };
        }
        public static bool IsClosed(this IWidget widget) 
        {
            return widget is { Status: WidgetStatus.Closed };
        }
    }
}
