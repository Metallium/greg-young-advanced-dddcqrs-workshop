using System;
using System.Threading;

namespace Restaurant
{
    public class Printer : IHandleOrder
    {
        public void Handle(Order order)
        {
            Console.WriteLine("[printer]: printing");
            Thread.Sleep(750);
            Console.WriteLine("[printer]: " + order.MutableContainer);
        }
    }
}
