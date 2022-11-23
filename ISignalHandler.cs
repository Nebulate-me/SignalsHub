using System;

namespace _Scripts.Signals
{
    public interface ISignalHandler : IDisposable
    {
        void Execute();
    }
}