using System;

namespace Restaurant
{
    public abstract class OrderMessage : IdentifiedMessage
    {
        protected OrderMessage(Order order)
            : base(Guid.Parse(order.OrderId), Guid.Parse(order.OrderId), Guid.NewGuid())
        {
            Order = order;
        }

        protected OrderMessage(IMessage causationMessage, Order order) : base(causationMessage)
        {
            Order = order;
        }

        public Order Order { get; }
    }

    public class CookFood : OrderMessage
    {
        public CookFood(Order order) : base(order)
        {
        }

        public CookFood(IMessage causationMessage, Order order) : base(causationMessage, order)
        {
        }
    }

    public class PriceOrder : OrderMessage
    {
        public PriceOrder(Order order) : base(order)
        {
        }

        public PriceOrder(IMessage causationMessage, Order order) : base(causationMessage, order)
        {
        }
    }

    public class TakePayment : OrderMessage
    {
        public TakePayment(Order order) : base(order)
        {
        }

        public TakePayment(IMessage causationMessage, Order order) : base(causationMessage, order)
        {
        }
    }

    public class PrintReceipt : OrderMessage
    {
        public PrintReceipt(Order order) : base(order)
        {
        }

        public PrintReceipt(IMessage causationMessage, Order order) : base(causationMessage, order)
        {
        }
    }
}
