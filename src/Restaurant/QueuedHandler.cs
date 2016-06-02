using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Restaurant
{
    public interface IStartable
    {
        void Start();
    }

    public class QueuedHandler : IHandleOrder, IStartable
    {
        private readonly IHandleOrder _orderHandler;
        private readonly ConcurrentQueue<Order> _concurrentQueue;
        private bool _started;

        public QueuedHandler(IHandleOrder orderHandler)
        {
            _orderHandler = orderHandler;
            _concurrentQueue = new ConcurrentQueue<Order>();
        }

        public void Handle(Order order)
        {
            if (!_started)
            {
                throw new InvalidOperationException();
            }
            _concurrentQueue.Enqueue(order);
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
                Order order;
                if (_concurrentQueue.TryDequeue(out order))
                {
                    _orderHandler.Handle(order);
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}
