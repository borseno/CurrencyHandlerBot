using System;
using System.Threading.Tasks;
using DateTimeZonesHandling;
using MainBankOfRussia.DataProcessors;
using MainBankOfRussia.XmlModels;

namespace CurrencyHandler.Models.DataCaching
{
    public static class CurrenciesDataCaching
    {
        // Main Bank of Russia's info is normally updated at 14:00 
        private static readonly TimeSpan UpdateTime = new TimeSpan(hours: 14, minutes: 00, seconds: 00);

        public static DateTime LastCachedDate { get; private set; }

        public static ValCurs Data { get; private set; }

        public static async Task<ValCurs> GetValCursAsync()
        {
            if (IsCacheNeeded(LastCachedDate, DateTimeZones.NowMoscow, Data))
                await CacheTodayAsync();

            return Data;
        }

        /// <summary>
        /// Indicates whether the given value "data" should be cached at the given dateTime dt, if last caching happened at lastCache
        /// </summary>
        /// <param name="lastCache">last caching date</param>
        /// <param name="dt">given date</param>
        /// <param name="data">data itself</param>
        /// <returns></returns>
        private static bool IsCacheNeeded(DateTime lastCache, DateTime dt, object data)
        {
            if (data == null)
                return true;

            if (lastCache.Date == dt.Date) // same day
            {
                return lastCache.TimeOfDay < UpdateTime && dt.TimeOfDay >= UpdateTime;
            }
            if (lastCache.Date == dt.Date.AddDays(value: -1)) // previous day
            {
                if (lastCache.TimeOfDay < UpdateTime)
                    return true;
                if (dt.TimeOfDay >= UpdateTime)
                    return true;
            }
            else if (lastCache.Date < dt.Date.AddDays(value: -1)) // earlier than previous day
            {
                return true;
            }

            return false;
        }

        private static async Task CacheTodayAsync()
        {
            Data = await CurrenciesProcessor.LoadValCursAsync();

            LastCachedDate = DateTimeZones.NowMoscow;
        }
    }
}
