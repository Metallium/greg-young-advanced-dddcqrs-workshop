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
            var cookNames = new[]
            {
                "Joe",
                "Greg",
                "Bro"
            };
            var assistantManager = new AssistantManager(
                new Cashier(new Printer())
                );
            var waiter = new Waiter(
                new RoundRobinDispatcher(
                    cookNames.Select(cookName => new Cook(cookName, assistantManager))
                    )
                );

            for (var i = 0; i < ordersCount; ++i)
            {
                var orderId = waiter
                    .PlaceNewOrder(new Dictionary<string, int>
                    {
                        {"meat", 2}
                    });
                Console.WriteLine($"[outer user]: got order handle {orderId}.");
            }
            Console.WriteLine("[outer user]: placed all orders.");
            Console.ReadKey(false);
        }
    }
}
