using System.Collections.Generic;

namespace Restaurant
{
    public interface IPublisher
    {
        void Publish<TMessage>(TMessage message)
            where TMessage : class, IMessage;

    }

    public class TopicBasedPubSub : IPublisher
    {
        private readonly Dictionary<string, object> _subs;

        public TopicBasedPubSub()
        {
            _subs = new Dictionary<string, object>();
        }

        public void Publish<TMessage>(TMessage message)
            where TMessage : class, IMessage
        {
            var handler = _subs[typeof(TMessage).FullName];
            ((IHandle<TMessage>)handler).Handle(message);
        }

        public void Subscribe<TMessage>(IHandle<TMessage> handler)
            where TMessage : class, IMessage
        {
            _subs.Add(typeof(TMessage).FullName, handler);
        }
    }
}
