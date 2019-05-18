using System;
using System.Runtime.InteropServices;
using TimeZoneConverter;

namespace DateTimeZonesHandling
{
    public static class DateTimeZones
    { 
        private static TimeZoneInfo MoscowInfo { get; }

        public static DateTime NowMoscow
        {
            get
            {
                var utc = DateTime.UtcNow;
                var moscow = TimeZoneInfo
                    .ConvertTime(utc, MoscowInfo);

                return moscow;
            }
        }

        static DateTimeZones()
        {
            const string WindowsMoscowTime = "Russian Standard Time";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                MoscowInfo = TimeZoneInfo.FindSystemTimeZoneById(WindowsMoscowTime);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                string linux = TZConvert.WindowsToIana(WindowsMoscowTime);

                MoscowInfo = TimeZoneInfo.FindSystemTimeZoneById(linux);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                throw new NotImplementedException("Cannot get moscow time for OSX OS");
            }
        }
    }
}
