using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using JetBrains.Annotations;

namespace NecroMacro.Core.StateMachine
{
    [UsedImplicitly]
    public class StateMachine
    {
        private IState currentState;
        private IState previousState;
        private readonly Dictionary<Type, IState> states = new();
        private readonly Queue<Transition> pendingTransitions = new();
        private CancellationTokenSource cancellationTokenSource;

        public void Run()
        {
            cancellationTokenSource = new CancellationTokenSource();
            Update().Forget();
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        public void RegisterState(IState state)
        {
            state.StateMachine = this;

            states.Add(state.GetType(), state);
        }

        public void RequestTransition(Type stateType)
        {
            pendingTransitions.Enqueue(new Transition(stateType, null));
        }

        public void RequestTransition<T>(Type stateType, T options) where T : StateOptions
        {
            pendingTransitions.Enqueue(new Transition(stateType, options));
        }

        private async UniTask ChangeTo<T>(Type stateType, T options) where T : StateOptions
        {
            if (currentState != null)
            {
                previousState = currentState;
                await previousState.OnExit();
                previousState.Dispose();
                currentState = null;
            }

            if (states.TryGetValue(stateType, out IState nextState))
            {
                nextState.SetOptions(options);
                currentState = nextState;
                await nextState.OnEnter();
            }
            else
            {
                throw new Exception($"State: {stateType.Name} is not registered to state machine.");
            }
        }

        private async UniTaskVoid Update()
        {
            await foreach (var _ in UniTaskAsyncEnumerable
                               .EveryUpdate()
                               .WithCancellation(cancellationTokenSource.Token))
            {
                while (pendingTransitions.Count > 0)
                {
                    var transition = pendingTransitions.Dequeue();
                    await ChangeTo(transition.Type, transition.StateOptions);
                }

                currentState?.OnUpdate();
            }
        }
    }
}