using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Restaurant;

namespace Tests
{
    public class IsolatedJsonExtensionPlayground
    {
        [Test]
        public void Test()
        {
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                IsShittyCustomer = true,
                TableNumber = 123,
                LineItems = new[]
                {
                    new LineItemDto {Item = "Item", Price = 100, Quantity = 1}
                },
                Ingredients = "stuff",
                OrderId = orderId.ToString("N"),
                Tax = 20,
                Totals = 100,
                IsPaid = true,
            };

            var json = order.MutableContainer.ToString(Formatting.Indented);

            var restoredOrder = new Order(JObject.Parse(json));
            Assert.That(restoredOrder.IsShittyCustomer, Is.True);
            Assert.That(restoredOrder.TableNumber, Is.EqualTo(123));
            Assert.That(restoredOrder.LineItems, Is.Not.Null);
            Assert.That(restoredOrder.LineItems.Count, Is.EqualTo(1));
            Assert.That(restoredOrder.Ingredients, Is.EqualTo("stuff"));
            Assert.That(restoredOrder.OrderId, Is.EqualTo(orderId.ToString("N")));
            Assert.That(restoredOrder.Tax, Is.EqualTo(20));
            Assert.That(restoredOrder.Totals, Is.EqualTo(100));
            Assert.That(restoredOrder.IsPaid, Is.EqualTo(true));

            var lineItemDto = restoredOrder.LineItems.Single();
            Assert.That(lineItemDto.Item, Is.EqualTo("Item"));
            Assert.That(lineItemDto.Price, Is.EqualTo(100));
            Assert.That(lineItemDto.Quantity, Is.EqualTo(1));
        }
    }
}
