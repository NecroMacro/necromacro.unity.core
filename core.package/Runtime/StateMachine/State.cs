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

        public async UniTask OnEnterInner()
        {
	        StateLifetime.Reset();
	        await OnEnter();
        }
        
        public async UniTask OnExitInner()
        {
	        await OnExit();
	        StateLifetime.Dispose();
        }

        public abstract UniTask OnEnter();

        public abstract UniTask OnExit();

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