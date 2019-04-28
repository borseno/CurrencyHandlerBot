using System;

namespace DateTimeZonesHandling
{
    public static class DateTimeZones
    {
        public static DateTime NowMoscow
        {
            get
            {
                var utc = DateTime.UtcNow;
                var moscow = TimeZoneInfo
                    .ConvertTime(utc, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));

                return moscow;
            }
        }
    }
}
