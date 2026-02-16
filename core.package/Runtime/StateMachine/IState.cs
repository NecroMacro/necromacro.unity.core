using System;
using Cysharp.Threading.Tasks;

namespace NecroMacro.Core.StateMachine
{
    public interface IState : IDisposable
    {
        StateMachine StateMachine { get; set; }
        UniTask OnEnter();
        UniTask OnExit();
        void SetOptions(StateOptions stateOptions);
        void OnUpdate();
    }
}