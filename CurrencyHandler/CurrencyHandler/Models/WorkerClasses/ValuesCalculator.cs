using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CurrencyHandler.Models.Extensions;
using MainBankOfRussia.XmlModels;

namespace CurrencyHandler.Models.WorkerClasses
{
    public static class ValuesCalculator
    {
        private static readonly NumberFormatInfo DecimalFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "," };

        public static async Task<Dictionary<string, decimal>> GetCurrenciesValuesAsync(decimal value, string currency, ValCurs data, string[] neededCurrencies)
        {
            return await Task.Run(() =>
            {
                decimal rub = ConvertToRub(value, currency, data); // whatever currency the value is, it is processed as rub            

                var result = data?.Valute
                    .Where(i => i.CharCode.In(neededCurrencies)
                    ).Select(i =>
                        {
                            return new KeyValuePair<string, decimal>(
                                i.CharCode,
                                rub / (decimal.Parse(i.Value, DecimalFormatInfo) / i.Nominal)
                            );
                        }
                    ).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                if (neededCurrencies.Contains("RUB"))
                    result.Add("RUB", rub);

                return result;
            });
        }

        private static decimal ConvertToRub(decimal value, string currency, ValCurs data)
        {
            if (currency == "RUB")
                return value;

            // how many rubles one value of this currency contains (e.g one dollar - 20 rubles, oneInRub = 20)
            decimal oneInRub;

            // asked currency info
            var instance = data.Valute.First(v => v.CharCode == currency);

            oneInRub = Decimal.Parse(instance.Value, DecimalFormatInfo) / instance.Nominal;

            // 5 dollars, oneDollarInRub = 20, return 20 * 5 = 100 rubs
            return oneInRub * value;
        }
    }
}
