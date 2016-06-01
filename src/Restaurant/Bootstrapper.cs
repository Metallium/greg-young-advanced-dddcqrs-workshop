using System;
using System.Collections.Generic;

namespace Restaurant
{
    public static class Bootstrapper
    {
        public static void Main()
        {
            var orderId = new Waiter(
                new Cook(
                    new AssistantManager(
                        new Cashier(new Printer())
                        )
                    )
                )
                .PlaceNewOrder(new Dictionary<string, int>
                {
                    {"meat", 2}
                });
            Console.WriteLine($"Placed an order {orderId}.");
            Console.ReadKey(false);
        }

    }
}
