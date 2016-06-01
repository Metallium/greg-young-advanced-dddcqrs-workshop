using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class ProcessManager
    {
        public const double StopLossRatio = 0.9;
        private readonly IBus _bus;

        public ProcessManager(IBus bus)
        {
            _bus = bus;
        }

        private bool IsInitialized { get; set; }
        private int CurrentPricePerItem { get; set; }
        private int CurrentStopLossPrice { get; set; }

        private IList<int> LastTenSecondsPrices { get; } = new List<int>();
        private IList<int> LastThirteenSecondsPrices { get; } = new List<int>();

        public void Handle(PositionAcquiredEvent positionAcquiredEvent)
        {
            if (IsInitialized)
            {
                throw new NotImplementedException();
            }
            CurrentPricePerItem = positionAcquiredEvent.PricePerItem;

            UpdateStopLossPrice(StopLossRatio*positionAcquiredEvent.PricePerItem);

            IsInitialized = true;
        }

        private void UpdateStopLossPrice(double newPrice)
        {
            var stopLossPriceUpdatedEvent = new StopLossPriceUpdatedEvent
            {
                NewStopLossPrice = (int) Math.Floor(newPrice)
            };
            CurrentStopLossPrice = stopLossPriceUpdatedEvent.NewStopLossPrice;
            _bus.Fire(stopLossPriceUpdatedEvent);
        }

        public void Handle(PriceUpdatedEvent priceUpdatedEvent)
        {
            CurrentPricePerItem = priceUpdatedEvent.NewPricePerItem;

            var price = priceUpdatedEvent.NewPricePerItem;
            LastTenSecondsPrices.Add(price);
            LastThirteenSecondsPrices.Add(price);

            _bus.Fire(new SendBackIn(TimeSpan.FromSeconds(10), new TenSecondsPassed {Price = price}));
            _bus.Fire(new SendBackIn(TimeSpan.FromSeconds(13), new ThirteenSecondsPassed {Price = price}));
        }

        public void Handle(TenSecondsPassed tenSecondsPassed)
        {
            var wasRemoved = LastTenSecondsPrices.Remove(tenSecondsPassed.Price);
            if (!wasRemoved)
            {
                throw new InvalidOperationException();
            }

            var minPrice = new[] {tenSecondsPassed.Price}.Concat(LastTenSecondsPrices).Min();
            if (StopLossRatio*minPrice > CurrentStopLossPrice)
            {
                UpdateStopLossPrice(StopLossRatio*CurrentPricePerItem);
            }
        }

        public void Handle(ThirteenSecondsPassed thirteenSecondsPassed)
        {
            var wasRemoved = LastThirteenSecondsPrices.Remove(thirteenSecondsPassed.Price);
            if (!wasRemoved)
            {
                throw new InvalidOperationException();
            }

            var maxPrice = new[] {thirteenSecondsPassed.Price}.Concat(LastThirteenSecondsPrices).Max();
            if (maxPrice < CurrentStopLossPrice)
            {
                _bus.Fire(new StopLossHitEvent(maxPrice));
            }
        }
    }
}