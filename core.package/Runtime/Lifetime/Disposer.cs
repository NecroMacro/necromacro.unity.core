using System;
using JetBrains.Annotations;
using R3;

namespace NecroMacro.Core.Lifetime
{
    public static class DisposerExtensions
    {
        public static T AddTo<T>(this T disposable, IDisposer disposer)
            where T : IDisposable
        {
            disposer.Add(disposable);
            return disposable;
        }
    }
    
    public interface IDisposer : IDisposable
    {
        void Add(IDisposable disposable);
        void Reset();
    }
    
    public abstract class Disposer : IDisposer
    {
        private readonly CompositeDisposable Disposable = new();

        public void Add(IDisposable disposable)
        {
            Disposable.Add(disposable);
        }

        public void Reset()
        {
            Disposable.Dispose();
            Disposable.Clear();
        }

        public void Dispose()
        {
            Disposable.Dispose();
        }
    }

    public interface ILifeCycleDisposer : IDisposer { }
    public interface ILifetimeScopeDisposer : IDisposer { }

    [UsedImplicitly] public class LifeCycleDisposer : Disposer, ILifeCycleDisposer { }
    [UsedImplicitly] public class LifetimeScopeDisposer : Disposer, ILifetimeScopeDisposer {}
}