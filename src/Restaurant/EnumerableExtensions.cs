using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Tap<T>(this IEnumerable<T> items, Action<IEnumerable<T>> action)
        {
            var materializedItems = items.ToList();
            action(materializedItems);
            return materializedItems;
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }
    }
}
