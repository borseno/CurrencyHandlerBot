using System.Collections.Generic;

namespace CurrencyHandler.Models.Extensions
{
    public static class EnumerableExtension
    {
        /// <summary>
        /// reverses Collection.Contains(element) into element.In(Collection) (like in sql)  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="el"></param>
        /// <param name="enumerable"></param>
        /// <returns></returns>
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
