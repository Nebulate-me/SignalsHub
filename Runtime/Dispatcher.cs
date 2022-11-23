using System.Collections.Generic;
using UnityEngine;

namespace Signals
{
    public class Dispatcher : MonoBehaviour
    {
        private static readonly Queue<ISignalHandler> NotProcessedSignals = new Queue<ISignalHandler>();

        public void Update()
        {
            while (NotProcessedSignals.Count > 0)
            {
                using var signalHandler = NotProcessedSignals.Dequeue();
                signalHandler.Execute();
            }
        }

        public static void DispatchAsync(ISignalHandler handleSignal)
        {
            NotProcessedSignals.Enqueue(handleSignal);
        }
    }
}