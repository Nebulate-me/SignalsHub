using Zenject;

namespace _Scripts.Signals
{
    public class SignalHandler<T> : ISignalHandler
    {
        public static readonly StaticMemoryPool<T, ISignalListener<T>, SignalHandler<T>> Pool =
            new StaticMemoryPool<T, ISignalListener<T>, SignalHandler<T>>(OnSpawned, OnDespawned);

        private T data;
        private ISignalListener<T> listener;

        public void Execute()
        {
            listener.Handle(data);
        }

        public void Dispose()
        {
            Pool.Despawn(this);
        }

        private static void OnDespawned(SignalHandler<T> obj)
        {
            obj.data = default;
            obj.listener = default;
        }

        private static void OnSpawned(T data, ISignalListener<T> listener, SignalHandler<T> obj)
        {
            obj.data = data;
            obj.listener = listener;
        }
    }
}