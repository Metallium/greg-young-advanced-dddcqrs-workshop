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
        private readonly Dictionary<string, IHandle<IMessage>> _subs;

        public TopicBasedPubSub()
        {
            _subs = new Dictionary<string, IHandle<IMessage>>();
        }

        public void Publish<TMessage>(TMessage message)
            where TMessage : class, IMessage
        {
            var handler = _subs[typeof(TMessage).FullName];
            handler.WidenFrom<TMessage, IMessage>().Handle(message);
        }

        public void SubscribeByType<TMessage>(IHandle<TMessage> handler)
            where TMessage : class, IMessage
        {
            _subs.Add(typeof(TMessage).FullName, handler.NarrowTo<IMessage, TMessage>());
        }
    }
}
