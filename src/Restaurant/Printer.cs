using System.Threading;

namespace Restaurant
{
    public class Printer : IHandle<OrderPaid>
    {
        private readonly IHorn _horn;

        public Printer(IHorn horn)
        {
            _horn = horn;
        }

        public void Handle(OrderPaid message)
        {
            _horn.Say("[printer]: printing");
            Thread.Sleep(750);
            _horn.Say("[printer]: " + message.Order.MutableContainer);
        }
    }
}
