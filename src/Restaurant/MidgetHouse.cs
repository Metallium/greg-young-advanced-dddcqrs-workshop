using System;
using System.Collections.Generic;

namespace Restaurant
{
    public class MidgetFactory
    {
        public static IMidget Create(bool isDodgyCustomer, TopicBasedPubSub topicBasedPubSub)
        {
            return isDodgyCustomer
                ? (IMidget) new ZimbabweanMidget(topicBasedPubSub)
                : new LithuanianMidget(topicBasedPubSub);
        }
    }

    public class MidgetHouse : IHandle<OrderPlaced>
    {
        private readonly TopicBasedPubSub _topicBasedPubSub;
        private readonly Dictionary<Guid, IMidget> _midgets;

        public MidgetHouse(TopicBasedPubSub topicBasedPubSub)
        {
            _topicBasedPubSub = topicBasedPubSub;
            _midgets = new Dictionary<Guid, IMidget>();
        }

        public void Handle(OrderPlaced message)
        {
            var midget = MidgetFactory.Create(message.IsDodgyCustomer, _topicBasedPubSub);
            _midgets[message.CorellationId] = midget;
            _topicBasedPubSub.SubscribeByCorellationId<OrderPlaced>(message.CorellationId, midget);
            _topicBasedPubSub.SubscribeByCorellationId<OrderCooked>(message.CorellationId, midget);
            _topicBasedPubSub.SubscribeByCorellationId<CookingTimedOut>(message.CorellationId, midget);
            _topicBasedPubSub.SubscribeByCorellationId<OrderPriced>(message.CorellationId, midget);
            _topicBasedPubSub.SubscribeByCorellationId<OrderPaid>(message.CorellationId, midget);

            midget.Handle(message);
        }
    }
}
