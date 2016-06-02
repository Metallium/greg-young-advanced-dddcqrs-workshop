using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace Restaurant
{
    public static class Bootstrapper
    {
        public static void Main()
        {
            var wireUpResult = WireUp();
            wireUpResult.Startables.ForEach(x => x.Start());
            StartPrintingQueueStats(wireUpResult.QueuedHandlers);
            var ordersCount = 100;
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

        private static void StartPrintingQueueStats(IList<QueuedHandler> queuedHandlers)
        {
            var thread = new Thread(() =>
            {
                while (true)
                {
                    var stats = queuedHandlers.ToDictionary(x => x.QueueName, x => x.QueueDepth);
                    var previousColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[stats]: " + JsonConvert.SerializeObject(stats, Formatting.Indented));
                    Console.ForegroundColor = previousColor;

                    Thread.Sleep(1000);
                }
            }) {IsBackground = true};
            thread.Start();
        }

        private static WireUpResult WireUp()
        {
            var cookNames = new[]
            {
                "Joe",
                "Greg",
                "Bro"
            };
            var printer = AsQueueable(nameof(Printer), new Printer());
            var cashier = AsQueueable(nameof(Cashier), new Cashier(printer));
            var assistantManager = AsQueueable(nameof(AssistantManager), new AssistantManager(cashier));
            var random = new Random();
            var queuedHandlers = cookNames
                .Select(
                    cookName =>
                        AsQueueable($"{nameof(Cook)}-{cookName}",
                            new Cook(cookName, random.Next(0, 10000), assistantManager)))
                .Concat(new[]
                {
                    printer,
                    cashier,
                    assistantManager
                })
                .ToList();
            var waiter = new Waiter(new RoundRobinDispatcher(queuedHandlers));
            return new WireUpResult
            {
                Waiter = waiter,
                Startables = queuedHandlers
                    .Cast<IStartable>()
                    .ToList(),
                QueuedHandlers = queuedHandlers
            };
        }

        private static QueuedHandler AsQueueable(string queueName, IHandleOrder orderHandler)
        {
            return new QueuedHandler(queueName, orderHandler);
        }
    }
}
