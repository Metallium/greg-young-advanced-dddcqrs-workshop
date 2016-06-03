using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant
{
    public interface IPublisher
    {
        void Publish<TMessage>(TMessage message)
            where TMessage : class, IMessage;
    }

    public class TopicBasedPubSub : IPublisher
    {
        private readonly ConcurrentDictionary<string, List<IHandle<IMessage>>> _subs;

        public TopicBasedPubSub()
        {
            _subs = new ConcurrentDictionary<string, List<IHandle<IMessage>>>();
        }

        public void Publish<TMessage>(TMessage message)
            where TMessage : class, IMessage
        {
            PublishInternal(TypeToTopicName(typeof(TMessage)), message);
            PublishInternal(CorellationIdToTopicName(message.CorellationId), message);
        }

        private void PublishInternal<TMessage>(string topicName, TMessage message)
            where TMessage : class, IMessage
        {
            List<IHandle<IMessage>> list;

            if (_subs.TryGetValue(topicName, out list))
            {
                list.ForEach(x => x.WidenFrom<TMessage, IMessage>().Handle(message));
            }
        }

        public void Subscribe(string topicName, IHandle<IMessage> handler)
        {
            _subs.AddOrUpdate(
                topicName,
                key => new List<IHandle<IMessage>> {handler},
                (key, currentList) => currentList.Concat(new [] {handler}).ToList());
        }

        public void SubscribeByCorellationId<TMessage>(Guid corellationId, IHandle<TMessage> handler)
            where TMessage : class, IMessage
        {
            Subscribe(CorellationIdToTopicName(corellationId), handler.NarrowToIfYouCan<IMessage, TMessage>());
        }

        public void SubscribeByCorellationId(Guid corellationId, IHandle<IMessage> handler)
        {
            Subscribe(CorellationIdToTopicName(corellationId), handler);
        }

        public void SubscribeByType<TMessage>(IHandle<TMessage> handler)
            where TMessage : class, IMessage
        {
            Subscribe(typeof(TMessage).FullName, handler.NarrowToIfYouCan<IMessage, TMessage>());
        }

        public void Unsubscribe(string topicName, IHandle<IMessage> handler)
        {
            _subs.AddOrUpdate(
                topicName,
                key => new List<IHandle<IMessage>>(),
                (key, currentList) => currentList.Where(x => !x.Equals(handler)).ToList());
        }

        public void UnsubscribeByType<TMessage>(IHandle<TMessage> handler)
            where TMessage : class, IMessage
        {
            Unsubscribe(TypeToTopicName(typeof(TMessage)), handler.NarrowToIfYouCan<IMessage, TMessage>());
        }

        private static string TypeToTopicName(Type type)
        {
            return type.FullName;
        }

        private static string CorellationIdToTopicName(Guid corellationId)
        {
            return corellationId.ToString("N");
        }
    }
}
