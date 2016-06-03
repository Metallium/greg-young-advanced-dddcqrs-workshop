namespace Restaurant
{
    public class OrderPlaced : OrderMessage
    {
        public OrderPlaced(Order order) : base(order)
        {
        }
    }

    public class OrderCooked : OrderMessage
    {
        public OrderCooked(IMessage causationMessage, Order order) : base(causationMessage, order)
        {
        }
    }

    public class OrderPriced : OrderMessage
    {
        public OrderPriced(IMessage causationMessage, Order order) : base(causationMessage, order)
        {
        }
    }

    public class OrderPaid : OrderMessage
    {
        public OrderPaid(IMessage causationMessage, Order order) : base(causationMessage, order)
        {
        }
    }
}
