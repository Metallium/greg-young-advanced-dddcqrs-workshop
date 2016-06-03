using System;

namespace Restaurant
{
    public class LithuanianMidget : IMidget
    {
        private readonly IPublisher _publisher;
        private bool _cooked;

        public LithuanianMidget(IPublisher publisher)
        {
            _publisher = publisher;
            _cooked = false;
        }

        public void Handle(OrderPlaced message)
        {
            _publisher.Publish(new CookFood(message, message.Order));
            _publisher.Publish(new SendMeIn(message, DateTime.UtcNow.AddSeconds(10),
                x => new CookingTimedOut(x, message.Order)));
        }

        public void Handle(OrderCooked message)
        {
            _cooked = true;
            _publisher.Publish(new PriceOrder(message, message.Order));
        }

        public void Handle(CookingTimedOut message)
        {
            if (_cooked)
            {
                return;
            }
            _publisher.Publish(new CookFood(message, message.Order));
            _publisher.Publish(new SendMeIn(message, DateTime.UtcNow.AddSeconds(10),
                x => new CookingTimedOut(x, message.Order)));
        }


        public void Handle(OrderPriced message)
        {
            _publisher.Publish(new TakePayment(message, message.Order));
        }

        public void Handle(OrderPaid message)
        {
            _publisher.Publish(new PrintReceipt(message, message.Order));
            _publisher.Publish(new OrderFinalized(message, message.Order));
        }
    }
}
