namespace Restaurant
{
    public class LithuanianMidget :
        IHandle<OrderPlaced>,
        IHandle<OrderCooked>,
        IHandle<OrderPriced>,
        IHandle<OrderPaid>
    {
        private readonly IPublisher _publisher;

        public LithuanianMidget(IPublisher publisher)
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
}
