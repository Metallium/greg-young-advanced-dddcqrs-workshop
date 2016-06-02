using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant
{
    public static class Bootstrapper
    {
        public static void Main()
        {
            var ordersCount = 10;
            for (var i = 0; i < ordersCount; ++i)
            {
                var assistantManager = new AssistantManager(
                    new Cashier(new Printer())
                    );
                var orderId = new Waiter(
                    new Multiplexor(
                        Enumerable.Range(0, 3).Select(x => new Cook(assistantManager))
                        )
                    )
                    .PlaceNewOrder(new Dictionary<string, int>
                    {
                        {"meat", 2}
                    });
                Console.WriteLine($"Placed an order {orderId}.");

            }
            Console.WriteLine("Placed all orders.");
            Console.ReadKey(false);
        }

    }
}
