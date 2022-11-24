using System;
using System.Collections.Generic;

namespace Signals
{
    public class SignalListener<T> : ISignalListener<T>
    {
        private readonly List<Subscriber> subscribers = new List<Subscriber>();
        private readonly List<Subscriber> removedSubscribers = new List<Subscriber>();

        private bool inProcess;

        public void AddListener(Action<T> listener, bool once)
        {
            subscribers.Add(new Subscriber
            {
                Action = listener,
                Once = once
            });
        }

        public void RemoveListener(Action<T> listener)
        {
            var index = subscribers.FindIndex(x => x.Action == listener);
            if (index < 0)
                return;
            var subscriber = subscribers[index];

            if (inProcess)
                removedSubscribers.Add(subscriber);
            else
                subscribers.Remove(subscriber);
        }

        public void RemoveAllListeners()
        {
            if (inProcess)
                removedSubscribers.AddRange(subscribers);
            else
                subscribers.Clear();
        }

        public void Handle(T data)
        {
            inProcess = true;

            foreach (var subscriber in subscribers)
            {
                if (removedSubscribers.Contains(subscriber))
                    continue;
                subscriber.Action(data);
                if (subscriber.Once) 
                    removedSubscribers.Add(subscriber);
            }

            inProcess = false;
            foreach (var removedSubscriber in removedSubscribers)
                subscribers.Remove(removedSubscriber);
            removedSubscribers.Clear();
        }
        
        private struct Subscriber
        {
            public Action<T> Action;
            public bool Once;
        }
    }
}