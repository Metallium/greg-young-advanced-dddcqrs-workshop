namespace Restaurant
{
    public interface IMidget :
        IHandle<OrderPlaced>,
        IHandle<OrderCooked>,
        IHandle<CookingTimedOut>,
        IHandle<OrderPriced>,
        IHandle<OrderPaid>
    {

    }
}
