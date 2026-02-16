using System;

namespace NecroMacro.Core.StateMachine
{
    public class Transition : Transition<StateOptions>
    {
        public Transition(Type type, StateOptions stateOptions) : base(type, stateOptions)
        {
        }
    }

    public abstract class Transition<T> where T : StateOptions
    {
        public Type Type { get; }
        public T StateOptions { get; }

        protected Transition(Type type, T stateOptions)
        {
            Type = type;
            StateOptions = stateOptions;
        }
    }
}