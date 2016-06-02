using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Restaurant;
using System;
using System.Linq;

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


        [Test]
        public void CreateOrderFromOrder_CopiesAllValues()
        {
            var originalOrder = new Order
            {
                Ingredients = "meat, more meat and some bread",
                IsPaid = false,
                IsShittyCustomer = false,
                OrderId = Guid.NewGuid().ToString("N"),
                LineItems = new[] {
                          new LineItemDto { Item = "burger", Price = 100, Quantity = 3 },
                          new LineItemDto { Item = "smth", Price = 120, Quantity = 2 }
                     },
                TableNumber = 5,
                Tax = 15,
                Totals = 100500
            };

            var orderCopy = new Order(originalOrder);
            Assert.That(originalOrder.MutableContainer.ToString(), Is.EqualTo(orderCopy.MutableContainer.ToString()));
        }
    }
}
