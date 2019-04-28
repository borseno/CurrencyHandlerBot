using System;
using System.Threading.Tasks;
using DateTimeZonesHandling;
using MainBankOfRussia.DataProcessors;
using MainBankOfRussia.XmlModels;

namespace CurrencyHandler.Models.DataCaching
{
    public static class CurrenciesDataCaching
    {
        private static readonly TimeSpan UpdateTime = new TimeSpan(hours: 14, minutes: 00, seconds: 00);

        public static DateTime LastCachedDate { get; private set; }

        public static ValCurs Data { get; private set; }

        public static async Task<ValCurs> GetValCurs()
        {
            if (IsCacheNeeded(LastCachedDate, DateTimeZones.NowMoscow, Data))
                await CacheTodayAsync();

            return Data;
        }

        public static bool IsCacheNeeded(DateTime lastCache, DateTime dt, object data)
        {
            if (data == null)
                return true;

            if (lastCache.Date == dt.Date) // same day
            {
                return lastCache.TimeOfDay < UpdateTime && dt.TimeOfDay >= UpdateTime;
            }
            if (lastCache.Date == dt.Date.AddDays(-1)) // previous day
            {
                if (lastCache.TimeOfDay < UpdateTime)
                    return true;
                if (dt.TimeOfDay >= UpdateTime)
                    return true;
            }
            else if (lastCache.Date < dt.Date.AddDays(-1)) // earlier than previous day
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
