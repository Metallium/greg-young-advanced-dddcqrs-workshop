using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using Trader;

namespace Tests
{
    public class TraderProcessManagerTests
    {
        [Test]
        public void EmitsStopLossPricesUpdatedTest()
        {
            var bus = new TestBus();
            var processManager = new ProcessManager(bus);

            AssertEmits(
                bus,
                () => processManager.Handle(new PositionAcquired {Amount = 1, PricePerItem = 100}),
                new[]
                {
                    new StopLossPriceUpdated {NewStopLossPrice = 90}
                });
        }

        [Test]
        public void EmitsRemindersOnPriceUpdatedTest()
        {
            var bus = new TestBus();
            var processManager = new ProcessManager(bus);
            processManager.Handle(new PositionAcquired {Amount = 1, PricePerItem = 100});

            AssertEmits(bus, () => processManager.Handle(new PriceUpdated {NewPricePerItem = 110}), new[]
            {
                new SendBackIn(TimeSpan.FromSeconds(10), new TenSecondsPassed {Price = 110}),
                new SendBackIn(TimeSpan.FromSeconds(13), new ThirteenSecondsPassed {Price = 110})
            });
        }

        [Test]
        public void UpdateStopLossPriceTest()
        {
            var bus = new TestBus();
            var processManager = new ProcessManager(bus);
            processManager.Handle(new PositionAcquired {Amount = 1, PricePerItem = 100});
            processManager.Handle(new PriceUpdated {NewPricePerItem = 110});

            AssertEmits(bus, () => { processManager.Handle(new TenSecondsPassed {Price = 110}); }, new[]
            {
                new StopLossPriceUpdated {NewStopLossPrice = (int) Math.Floor(ProcessManager.StopLossRatio*110)}
            });
        }

        [Test]
        public void UpdateStopLossPriceToStableMinimumTest()
        {
            var bus = new TestBus();
            var processManager = new ProcessManager(bus);
            processManager.Handle(new PositionAcquired {Amount = 1, PricePerItem = 100});
            processManager.Handle(new PriceUpdated {NewPricePerItem = 200});
            processManager.Handle(new PriceUpdated {NewPricePerItem = 150});

            AssertEmits(bus, () =>
            {
                processManager.Handle(new TenSecondsPassed {Price = 200});
                processManager.Handle(new TenSecondsPassed {Price = 150});
            }, new[]
            {
                new StopLossPriceUpdated {NewStopLossPrice = (int) Math.Floor(ProcessManager.StopLossRatio*150)}
            });
        }

        [Test]
        public void DoesNotUpdateStopLossPriceIfSomeBelowCurrentTest()
        {
            var bus = new TestBus();
            var processManager = new ProcessManager(bus);
            processManager.Handle(new PositionAcquired {Amount = 1, PricePerItem = 100});
            processManager.Handle(new PriceUpdated {NewPricePerItem = 110});
            processManager.Handle(new PriceUpdated {NewPricePerItem = 90});

            AssertEmitsNothing(bus, () =>
            {
                processManager.Handle(new TenSecondsPassed {Price = 110});
                processManager.Handle(new TenSecondsPassed {Price = 90});
            });
        }

        [Test]
        public void EmitStopLossHitTest()
        {
            var bus = new TestBus();
            var processManager = new ProcessManager(bus);
            processManager.Handle(new PositionAcquired {Amount = 1, PricePerItem = 100});
            processManager.Handle(new PriceUpdated {NewPricePerItem = 89});

            AssertEmits(bus, () => processManager.Handle(new ThirteenSecondsPassed {Price = 89}), new[]
            {
                new StopLossHitEvent(89)
            });
        }

        [Test]
        public void DoesNotEmitStopLossHitTest()
        {
            var bus = new TestBus();
            var processManager = new ProcessManager(bus);
            processManager.Handle(new PositionAcquired {Amount = 1, PricePerItem = 100});
            processManager.Handle(new PriceUpdated {NewPricePerItem = 89});
            processManager.Handle(new PriceUpdated {NewPricePerItem = 90});

            AssertEmitsNothing(bus, () =>
            {
                processManager.Handle(new ThirteenSecondsPassed {Price = 89});
                processManager.Handle(new ThirteenSecondsPassed {Price = 90});
            });
        }

        [Test]
        public void DoesNotEmitStopLossHitOnSamePriceTest()
        {
            var bus = new TestBus();
            var processManager = new ProcessManager(bus);
            processManager.Handle(new PositionAcquired {Amount = 1, PricePerItem = 100});
            processManager.Handle(new PriceUpdated {NewPricePerItem = 100});

            AssertEmitsNothing(bus, () => { processManager.Handle(new ThirteenSecondsPassed {Price = 100}); });
        }

        private static void AssertEmits(TestBus bus, Action action, IEnumerable<IMessage> expectedMessages)
        {
            bus.FiredMessages.Clear();
            action();
            var actual = SerializeForComparison(bus.FiredMessages);
            var expected = SerializeForComparison(expectedMessages);
            Assert.That(actual, Is.EqualTo(expected));
        }

        private static void AssertEmitsNothing(TestBus bus, Action action)
        {
            AssertEmits(bus, action, Enumerable.Empty<IMessage>());
        }

        private static string SerializeForComparison(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
        }
    }
}
