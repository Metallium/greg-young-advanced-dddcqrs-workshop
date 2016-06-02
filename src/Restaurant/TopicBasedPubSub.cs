using System;
using System.Collections.Generic;

namespace Restaurant
{
    public interface IPublisher
    {
        void Publish<TMessage>(TMessage message);
    }

    public class TopicBasedPubSub : IPublisher
    {
        private readonly Dictionary<Type, object> _subs;

        public TopicBasedPubSub()
        {
            _subs = new Dictionary<Type, object>();
        }

        public void Publish<TMessage>(TMessage message)
        {
            var handler = _subs[typeof(TMessage)];
            ((IHandle<TMessage>)handler).Handle(message);
        }

        public void Subscribe<TMessage>(IHandle<TMessage> handler)
        {
            _subs.Add(typeof(TMessage), handler);
        }
    }
}
