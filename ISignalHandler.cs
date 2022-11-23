using System;

namespace Signals
{
    public interface ISignalHandler : IDisposable
    {
        void Execute();
    }
}