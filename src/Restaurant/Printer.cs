using System;

namespace Restaurant
{
    public class Printer : IHandleOrder
    {
        public void Handle(Order order)
        {
            Console.WriteLine("[printer]: " + order.MutableContainer);
        }
    }
}
