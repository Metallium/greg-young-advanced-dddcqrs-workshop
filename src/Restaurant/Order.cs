using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Restaurant
{
    public class Order
    {
        public Order()
        {
            MutableContainer = new JObject();
        }

        public Order(JObject rootObject)
        {
            MutableContainer = rootObject.DeepClone();
        }

        public JToken MutableContainer { get; }

        public bool? IsShittyCustomer
        {
            get { return MutableContainer.Root[nameof(IsShittyCustomer)]?.Value<bool>(); }
            set { MutableContainer.Root[nameof(IsShittyCustomer)] = new JValue(value); }
        }

        public int? TableNumber
        {
            get { return MutableContainer.Root[nameof(TableNumber)]?.Value<int>(); }
            set { MutableContainer.Root[nameof(TableNumber)] = new JValue(value); }
        }

        public IReadOnlyList<LineItemDto> LineItems
        {
            get
            {
                var lineItems = (JArray) MutableContainer.Root[nameof(LineItems)];
                return lineItems?.Select(x => new LineItemDto
                {
                    Item = x[nameof(LineItemDto.Item)].Value<string>(),
                    Price = x[nameof(LineItemDto.Price)].Value<int>(),
                    Quantity = x[nameof(LineItemDto.Quantity)].Value<int>()
                }).ToList();
            }

            set
            {
                MutableContainer.Root[nameof(LineItems)] = new JArray(value.Select(x => new JObject
                {
                    {nameof(LineItemDto.Item), new JValue(x.Item)},
                    {nameof(LineItemDto.Price), new JValue(x.Price)},
                    {nameof(LineItemDto.Quantity), new JValue(x.Quantity)}
                }));
            }
        }

        public string OrderId
        {
            get { return MutableContainer.Root[nameof(OrderId)]?.Value<string>(); }
            set { MutableContainer.Root[nameof(OrderId)] = new JValue(value); }
        }

        public string Ingredients
        {
            get { return MutableContainer.Root[nameof(Ingredients)]?.Value<string>(); }
            set { MutableContainer.Root[nameof(Ingredients)] = new JValue(value); }
        }

        public int? Tax
        {
            get { return MutableContainer.Root[nameof(Tax)]?.Value<int>(); }
            set { MutableContainer.Root[nameof(Tax)] = new JValue(value); }
        }

        public int? Totals
        {
            get { return MutableContainer.Root[nameof(Totals)]?.Value<int>(); }
            set { MutableContainer.Root[nameof(Totals)] = new JValue(value); }
        }
        public bool? IsPaid
        {
            get { return MutableContainer.Root[nameof(IsPaid)]?.Value<bool>(); }
            set { MutableContainer.Root[nameof(IsPaid)] = new JValue(value); }
        }
    }
}
