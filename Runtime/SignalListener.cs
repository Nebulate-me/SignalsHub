using System;
using System.Collections.Generic;

namespace Signals
{
    public class SignalListener<T> : ISignalListener<T>
    {
        private readonly List<Subscriber> subscribers = new List<Subscriber>();
        private readonly List<Subscriber> addedSubscribers = new List<Subscriber>();
        private readonly List<Subscriber> removedSubscribers = new List<Subscriber>();

        private bool inProcess;

        public void AddListener(Action<T> listener, bool once)
        {
            var subscriber = new Subscriber
            {
                Action = listener,
                Once = once
            };

            if (inProcess)
            {
                addedSubscribers.Add(subscriber);
                return;
            }

            subscribers.Add(subscriber);
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
                if (HandleSubscriber(subscriber, data))
                    continue;
            }

            inProcess = false;

            foreach (var subscriber in addedSubscribers)
            {
                subscribers.Add(subscriber);

                if (HandleSubscriber(subscriber, data))
                    continue;
            }

            addedSubscribers.Clear();

            foreach (var removedSubscriber in removedSubscribers)
                subscribers.Remove(removedSubscriber);

            removedSubscribers.Clear();
        }

        private bool HandleSubscriber(Subscriber subscriber, T data)
        {
            if (removedSubscribers.Contains(subscriber))
                return true;

            subscriber.Action(data);
            if (subscriber.Once) 
                removedSubscribers.Add(subscriber);

            return false;            
        }

        private struct Subscriber
        {
            public Action<T> Action;
            public bool Once;
        }
    }
}