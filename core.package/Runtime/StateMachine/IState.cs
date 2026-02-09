using Cysharp.Threading.Tasks;

namespace NecroMacro.Core.StateMachine
{
    public interface IState
    {
        StateMachine StateMachine { get; set; }
        UniTask OnEnter();
        UniTask OnExit();
        void SetOptions(Options options);
        void OnUpdate();
    }
}