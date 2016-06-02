using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Restaurant
{
    public interface IStartable
    {
        void Start();
    }

    public interface ITrackable
    {
        string QueueName { get; }
        int QueueDepth { get; }
    }

    public class QueuedHandler<TMessage> : IHandle<TMessage>, IStartable, ITrackable
    {
        public string QueueName { get; }
        public int QueueDepth => _concurrentQueue.Count;

        private readonly IHandle<TMessage> _handler;
        private readonly ConcurrentQueue<TMessage> _concurrentQueue;
        private bool _started;

        public QueuedHandler(string queueName, IHandle<TMessage> handler)
        {
            _handler = handler;
            QueueName = queueName;
            _concurrentQueue = new ConcurrentQueue<TMessage>();
        }

        public void Handle(TMessage message)
        {
            if (!_started)
            {
                throw new InvalidOperationException();
            }
            _concurrentQueue.Enqueue(message);
        }

        public void Start()
        {
            if (_started)
            {
                throw new InvalidOperationException();
            }
            _started = true;
            var thread = new Thread(ThreadRunner) {IsBackground = true};
            thread.Start();
        }

        private void ThreadRunner()
        {
            while (true)
            {
                TMessage message;
                if (_concurrentQueue.TryDequeue(out message))
                {
                    _handler.Handle(message);
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}
