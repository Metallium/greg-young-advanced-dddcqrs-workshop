using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace Restaurant
{
    public static class Bootstrapper
    {
        private const bool PrintDetails = false;

        public static void Main()
        {
            var horn = new ConsoleHorn();
            var topicBasedPubSub = new TopicBasedPubSub();

            var wireUpResult = WireUp(topicBasedPubSub);
            wireUpResult.Startables.ForEach(x => x.Start());
            StartPrintingQueueStats(horn, wireUpResult.Trackables, wireUpResult.StatsProjection);

            topicBasedPubSub.SubscribeByType(wireUpResult.MidgetHouse);

            var ordersCount = 10;
            for (var i = 0; i < ordersCount; ++i)
            {
                var isDrinker = i%2 == 0;
                var orderId = Guid.NewGuid();
                topicBasedPubSub.SubscribeByCorellationId(orderId, new MessageTracer(orderId.ToString("N")));
                wireUpResult.Waiter
                    .PlaceNewOrder(orderId, new Dictionary<string, int>
                    {
                        {isDrinker ? GoodsMenu.Drinkables.Vodka : GoodsMenu.Eatables.Meat, 2}
                    });

                horn.Say($"[outer user]: got order handle {orderId}.");
            }
            horn.Say("[outer user]: placed all orders.");
            Console.ReadKey(false);
        }

        private static void StartPrintingQueueStats(IHorn horn, IList<ITrackable> trackables, StatsProjection statsProjection)
        {
            var thread = new Thread(() =>
            {
                while (true)
                {
                    horn.Note("[stats]: " + JsonConvert.SerializeObject(new
                    {
                        queueLengths = trackables.ToDictionary(x => x.QueueName, x => x.QueueDepth),
                        statsProjection = statsProjection
                    }, Formatting.Indented));
                    Thread.Sleep(500);
                }
            }) {IsBackground = true};
            thread.Start();
        }

        private static WireUpResult WireUp(TopicBasedPubSub topicBasedPubSub)
        {
            var horn = PrintDetails ? (IHorn)new ConsoleHorn() : new SilentHorn();

            var cookNames = new[]
            {
                "Joe",
                "Greg",
                "Bro"
            };
            var printer = new Printer(horn);

            var cashier = AsQueueable(nameof(Cashier), new Cashier(topicBasedPubSub, horn));

            var assistantManager = AsQueueable(nameof(AssistantManager), new AssistantManager(topicBasedPubSub, horn));

            var random = new Random();
            var cooks = cookNames
                .Select(
                    cookName =>
                        AsQueueable($"{nameof(Cook)}-{cookName}",
                            new DroppingHandler<CookFood>(new Cook(topicBasedPubSub, horn, cookName, random.Next(0, 10000)))))
                .ToList();

            var megaCook = AsQueueable("CookDispatcher", new MoreFairDispatcher<CookFood>(cooks));

            var waiter = new Waiter(topicBasedPubSub, horn);

            topicBasedPubSub.SubscribeByType<PrintReceipt>(printer);
            topicBasedPubSub.SubscribeByType<TakePayment>(cashier);
            topicBasedPubSub.SubscribeByType<PriceOrder>(assistantManager);
            topicBasedPubSub.SubscribeByType<CookFood>(megaCook);

            var midgetHouse = AsQueueable(nameof(MidgetHouse), new MidgetHouse(topicBasedPubSub));

            var items = cooks.Concat(new object[]
            {
                megaCook,
                cashier,
                assistantManager,
                midgetHouse
            }).ToList();

            var statsProjection = new StatsProjection();
            topicBasedPubSub.SubscribeByType<OrderPlaced>(statsProjection);
            topicBasedPubSub.SubscribeByType<OrderFinalized>(statsProjection);

            return new WireUpResult
            {
                Waiter = waiter,
                Startables = items
                    .Cast<IStartable>()
                    .ToList(),
                Trackables = items
                    .Cast<ITrackable>()
                    .ToList(),
                MidgetHouse = midgetHouse,
                StatsProjection = statsProjection
            };
        }

        private static QueuedHandler<TMessage> AsQueueable<TMessage>(string queueName, IHandle<TMessage> handler)
        {
            return new QueuedHandler<TMessage>(queueName, handler);
        }
    }
}
