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
            var wireUpResult = WireUp();
            wireUpResult.Startables.ForEach(x => x.Start());

            for (var i = 0; i < ordersCount; ++i)
            {
                var orderId = wireUpResult.Waiter
                    .PlaceNewOrder(new Dictionary<string, int>
                    {
                        {"meat", 2}
                    });
                Console.WriteLine($"[outer user]: got order handle {orderId}.");
            }
            Console.WriteLine("[outer user]: placed all orders.");
            Console.ReadKey(false);
        }

        private static WireUpResult WireUp()
        {
            var cookNames = new[]
            {
                "Joe",
                "Greg",
                "Bro"
            };
            var assistantManager = new AssistantManager(
                new Cashier(new Printer())
                );

            var queuedHandlers = cookNames
                .Select(cookName => new Cook(cookName, assistantManager))
                .Select(x => new QueuedHandler(x))
                .ToList();
            var waiter = new Waiter(
                new RoundRobinDispatcher(
                    queuedHandlers)
                );
            return new WireUpResult
            {
                Waiter = waiter,
                Startables = queuedHandlers.Cast<IStartable>().ToList()
            };
        }
    }
}
