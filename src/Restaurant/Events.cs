namespace Restaurant
{
    public class OrderPlaced : OrderMessage
    {
        public OrderPlaced(bool isDodgyCustomer, Order order) : base(order)
        {
            IsDodgyCustomer = isDodgyCustomer;
        }

        public bool IsDodgyCustomer { get; }
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

    public class OrderFinalized : OrderMessage
    {
        public OrderFinalized(IMessage causationMessage, Order order) : base(causationMessage, order)
        {
        }
    }

    public class CookingTimedOut : OrderMessage
    {
        public CookingTimedOut(IMessage causationMessage, Order order) : base(causationMessage, order)
        {
        }
    }
}
