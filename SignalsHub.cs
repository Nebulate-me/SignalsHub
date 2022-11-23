using System;
using System.Collections.Generic;

namespace _Scripts.Signals
{
    public static class SignalsHub
    {
        private static readonly Dictionary<Type, ISignalListener> Listeners = new Dictionary<Type, ISignalListener>();

        public static void AddListener<TSignal>(Action<TSignal> listener)
        {
            On<TSignal>().AddListener(listener, false);
        }
        
        public static void AddListenerOnce<TSignal>(Action<TSignal> listener)
        {
            On<TSignal>().AddListener(listener, true);
        }

        public static void RemoveListener<TSignal>(Action<TSignal> listener)
        {
            On<TSignal>().RemoveListener(listener);
        }

        public static void DispatchAsync<TSignal>(TSignal data)
        {
            var listener = GetListener<TSignal>();
            if (listener == null)
                return;
            
            var signalHandler = SignalHandler<TSignal>.Pool.Spawn(data, listener);
            Dispatcher.DispatchAsync(signalHandler);
        }

        private static ISignalListener<TSignal> On<TSignal>()
        {
            var listener = GetListener<TSignal>();
            if (listener == null)
            {
                listener = new SignalListener<TSignal>();
                Listeners.Add(typeof(TSignal), listener);
            }

            return listener;
        }

        private static ISignalListener<TSignal> GetListener<TSignal>()
        {
            if (!Listeners.TryGetValue(typeof(TSignal), out var listener)) return default;
            return listener as ISignalListener<TSignal>;
        }
    }
}