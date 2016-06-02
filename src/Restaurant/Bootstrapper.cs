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
            var printer = AsQueueable(new Printer());
            var cashier = AsQueueable(new Cashier(printer));
            var assistantManager = AsQueueable(new AssistantManager(cashier));
            var random = new Random();
            var queuedHandlers = cookNames
                .Select(cookName => new Cook(cookName, random.Next(3000), assistantManager))
                .Select(AsQueueable)
                .ToList();
            var waiter = new Waiter(new RoundRobinDispatcher(queuedHandlers));
            return new WireUpResult
            {
                Waiter = waiter,
                Startables = queuedHandlers
                    .Concat(new[]
                    {
                        printer,
                        cashier,
                        assistantManager
                    })
                    .Cast<IStartable>()
                    .ToList()
            };
        }

        private static IHandleOrder AsQueueable(IHandleOrder orderHandler)
        {
            return new QueuedHandler(orderHandler);
        }
    }
}
