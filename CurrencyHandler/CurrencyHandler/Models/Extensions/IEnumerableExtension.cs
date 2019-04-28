using System.Collections.Generic;

namespace CurrencyHandler.Models.Extensions
{
    public static class IEnumerableExtension
    {
        public static bool In<T>(this T el, IEnumerable<T> enumerable)
        {
            foreach (var i in enumerable)
            {
                if (i.Equals(el))
                    return true;
            }

            return false;
        }
    }
}
