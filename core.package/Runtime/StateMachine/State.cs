using Cysharp.Threading.Tasks;
using NecroMacro.Core.Lifetime;
using VContainer;

namespace NecroMacro.Core.StateMachine
{
    public abstract class State : State<StateOptions>
    {
    }

    public abstract class State<T> : IState where T : StateOptions
    {
        [Inject]
        protected ILifeCycleDisposer StateLifetime;
        
        public StateMachine StateMachine { get; set; }
        
        protected T Options { get; private set; }

        public virtual async UniTask OnEnter()
        {
            await UniTask.Yield();
        }

        public virtual async UniTask OnExit()
        {
            await UniTask.Yield();
        }

        public void SetOptions(StateOptions stateOptions)
        {
            if (stateOptions is T typedOptions)
            {
                Options = typedOptions;
            }
        }

        public virtual void OnUpdate()
        {
        }

        public void Dispose()
        {
            StateLifetime.Dispose();
        }
    }
}