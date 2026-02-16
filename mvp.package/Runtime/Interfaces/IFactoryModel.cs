using System;

namespace NecroMacro.MVP
{
    public interface IFactoryModel<TData> : IDisposable
    {
        public TData Data { get; set; }
        public void Initialize(TData data);
    }
}