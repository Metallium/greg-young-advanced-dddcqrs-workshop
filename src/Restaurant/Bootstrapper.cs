using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace Restaurant
{
    public static class Bootstrapper
    {
        const bool PrintDetails = false;

        public static void Main()
        {
            var horn = new ConsoleHorn();
            var wireUpResult = WireUp();
            wireUpResult.Startables.ForEach(x => x.Start());
            StartPrintingQueueStats(horn, wireUpResult.QueuedHandlers);
            var ordersCount = 100;
            for (var i = 0; i < ordersCount; ++i)
            {
                var orderId = wireUpResult.Waiter
                    .PlaceNewOrder(new Dictionary<string, int>
                    {
                        {"meat", 2}
                    });
                horn.Say($"[outer user]: got order handle {orderId}.");
            }
            horn.Say("[outer user]: placed all orders.");
            Console.ReadKey(false);
        }

        private static void StartPrintingQueueStats(IHorn horn, IList<QueuedHandler> queuedHandlers)
        {
            var thread = new Thread(() =>
            {
                while (true)
                {
                    var stats = queuedHandlers.ToDictionary(x => x.QueueName, x => x.QueueDepth);
                    horn.Note("[stats]: " + JsonConvert.SerializeObject(stats, Formatting.Indented));
                    Thread.Sleep(1000);
                }
            }) {IsBackground = true};
            thread.Start();
        }

        private static WireUpResult WireUp()
        {
            var horn = PrintDetails ? (IHorn)new ConsoleHorn() : new SilentHorn();

            var cookNames = new[]
            {
                "Joe",
                "Greg",
                "Bro"
            };

            var printer = AsQueueable(nameof(Printer), new Printer(horn));
            var cashier = AsQueueable(nameof(Cashier), new Cashier(horn, printer));
            var assistantManager = AsQueueable(nameof(AssistantManager), new AssistantManager(horn, cashier));
            var random = new Random();
            var queuedHandlers = cookNames
                .Select(
                    cookName =>
                        AsQueueable($"{nameof(Cook)}-{cookName}",
                            new Cook(horn, cookName, random.Next(0, 10000), assistantManager)))
                .Concat(new[]
                {
                    printer,
                    cashier,
                    assistantManager
                })
                .ToList();
            var waiter = new Waiter(horn, new RoundRobinDispatcher(queuedHandlers));
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
