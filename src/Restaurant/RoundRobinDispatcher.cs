using System.Collections.Generic;

namespace Restaurant
{
    public class RoundRobinDispatcher<TMessage> : IHandle<TMessage>
    {
        private readonly Queue<IHandle<TMessage>> _queue;

        public RoundRobinDispatcher(IEnumerable<IHandle<TMessage>> handlers)
        {
            _queue = new Queue<IHandle<TMessage>>(handlers);
        }

        public void Handle(TMessage message)
        {
            _queue.Peek().Handle(message);
            _queue.Enqueue(_queue.Dequeue());
        }
    }
}
