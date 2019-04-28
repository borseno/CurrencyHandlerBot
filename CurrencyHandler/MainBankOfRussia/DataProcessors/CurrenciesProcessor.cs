using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DateTimeZonesHandling;
using MainBankOfRussia.XmlModels;
using XmlSerialization.Services;

namespace MainBankOfRussia.DataProcessors
{
    public static class CurrenciesProcessor
    {
        private const string DateFormat = "dd/MM/yyyy";
        private const string UrlTemplate = "http://www.cbr.ru/scripts/XML_daily.asp?date_req=";

        // get windows-1251 (api encoding)
        static CurrenciesProcessor()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public static async Task<ValCurs> LoadValCursAsync(DateTime dateTime = default(DateTime))
        {
            if (dateTime == default(DateTime))
            {
                dateTime = DateTimeZones.NowMoscow;
            }

            var url = $"{UrlTemplate}{dateTime.ToString(DateFormat)}";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // throw an exception if possible

                var data = await response.Content.ReadAsStreamAsync();

                using (data)
                {
                    return await XmlSerializationService.Deserialize<ValCurs>(data);
                }
            }
        }
    }
}

