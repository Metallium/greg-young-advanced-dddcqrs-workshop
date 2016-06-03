using System;
using System.Collections.Generic;

namespace Restaurant
{
    public class Midget :
        IHandle<OrderPlaced>,
        IHandle<OrderCooked>,
        IHandle<OrderPriced>,
        IHandle<OrderPaid>
    {
        private readonly IPublisher _publisher;

        public Midget(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public void Handle(OrderPlaced message)
        {
            _publisher.Publish(new CookFood(message, message.Order));
        }

        public void Handle(OrderCooked message)
        {
            _publisher.Publish(new PriceOrder(message, message.Order));
        }

        public void Handle(OrderPriced message)
        {
            _publisher.Publish(new TakePayment(message, message.Order));
        }

        public void Handle(OrderPaid message)
        {
            _publisher.Publish(new PrintReceipt(message, message.Order));
        }
    }
    public class MidgetFactory
    {
        public static Midget Create(TopicBasedPubSub topicBasedPubSub)
        {
            return new Midget(topicBasedPubSub);
        }
    }
    public class MidgetHouse : IHandle<OrderPlaced>
    {
        private readonly TopicBasedPubSub _topicBasedPubSub;
        private readonly Dictionary<Guid, Midget> _midgets;

        public MidgetHouse(TopicBasedPubSub topicBasedPubSub)
        {
            _topicBasedPubSub = topicBasedPubSub;
            _midgets = new Dictionary<Guid, Midget>();
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
