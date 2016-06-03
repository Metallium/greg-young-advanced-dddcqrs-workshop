using System.Collections.Generic;

namespace Restaurant
{
    public class WireUpResult
    {
        public IList<IStartable> Startables { get; set; }
        public Waiter Waiter { get; set; }
        public IList<ITrackable> Trackables { get; set; }
        public IHandle<OrderPlaced> MidgetHouse { get; set; }
    }
}
