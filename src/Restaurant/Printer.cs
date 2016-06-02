using System.Threading;

namespace Restaurant
{
    public class Printer : IHandleOrder
    {
        private readonly IHorn _horn;

        public Printer(IHorn horn)
        {
            _horn = horn;
        }

        public void Handle(Order order)
        {
            _horn.Say("[printer]: printing");
            Thread.Sleep(750);
            _horn.Say("[printer]: " + order.MutableContainer);
        }
    }
}
