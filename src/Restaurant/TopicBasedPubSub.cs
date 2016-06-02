using System.Collections.Generic;

namespace Restaurant
{
    public interface IPublisher
    {
        void Publish(string topicName, Order order);
    }

    public class TopicBasedPubSub : IPublisher
    {
        private readonly Dictionary<string, IHandleOrder> _subs;

        public TopicBasedPubSub()
        {
            _subs = new Dictionary<string, IHandleOrder>();
        }

        public void Publish(string topicName, Order order)
        {
            _subs[topicName].Handle(order);
        }

        public void Subscribe(string topicName, IHandleOrder orderHandler)
        {
            _subs.Add(topicName, orderHandler);
        }
    }
}
