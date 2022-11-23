using System;

namespace Signals
{
    public interface ISignalListener
    {
    }
    
    public interface ISignalListener<T>: ISignalListener
    {
        void AddListener(Action<T> listener, bool once);
        void RemoveListener(Action<T> listener);
        void Handle(T data);
    }
}