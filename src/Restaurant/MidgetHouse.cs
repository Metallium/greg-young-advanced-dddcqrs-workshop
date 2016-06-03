using System;
using System.Collections.Generic;

namespace Restaurant
{
    public class MidgetFactory
    {
        public static LithuanianMidget Create(TopicBasedPubSub topicBasedPubSub)
        {
            return new LithuanianMidget(topicBasedPubSub);
        }
    }
    public class MidgetHouse : IHandle<OrderPlaced>
    {
        private readonly TopicBasedPubSub _topicBasedPubSub;
        private readonly Dictionary<Guid, LithuanianMidget> _midgets;

        public MidgetHouse(TopicBasedPubSub topicBasedPubSub)
        {
            _topicBasedPubSub = topicBasedPubSub;
            _midgets = new Dictionary<Guid, LithuanianMidget>();
        }

        public void Handle(OrderPlaced message)
        {
            var midget = MidgetFactory.Create(_topicBasedPubSub);
            _midgets[message.CorellationId] = midget;
            _topicBasedPubSub.SubscribeByCorellationId<OrderPlaced>(message.CorellationId, midget);
            _topicBasedPubSub.SubscribeByCorellationId<OrderCooked>(message.CorellationId, midget);
            _topicBasedPubSub.SubscribeByCorellationId<OrderPriced>(message.CorellationId, midget);
            _topicBasedPubSub.SubscribeByCorellationId<OrderPaid>(message.CorellationId, midget);

            midget.Handle(message);
        }
    }
}
