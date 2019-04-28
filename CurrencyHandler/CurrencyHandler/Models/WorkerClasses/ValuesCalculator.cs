using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MainBankOfRussia.XmlModels;

namespace CurrencyHandler.Models.WorkerClasses
{
    public static class ValuesCalculator
    {
        private static readonly NumberFormatInfo DecimalFormatInfo = new NumberFormatInfo() { NumberDecimalSeparator = "," };

        public static IEnumerable<KeyValuePair<string, decimal>> GetValuesInOtherCurrencies(decimal rubles, ValCurs data)
        {
            string[] neededCurrencies = { "UAH", "USD", "EUR" };

            return data?.Valute
                .Where(
                    i => neededCurrencies
                        .Contains(i.CharCode)
                ).Select(
                    i =>
                    {
                        var formatInfo = new NumberFormatInfo { NumberDecimalSeparator = "," };

                        return new KeyValuePair<string, decimal>(
                            i.CharCode,
                            rubles / (decimal.Parse(i.Value, formatInfo) / i.Nominal)
                            );
                    }
                );
        }

        public static async Task<IDictionary<string, decimal>> GetValuesInRubAndOtherCurrencies(decimal value, string currency, ValCurs data)
        {
            return await Task.Run(() =>
            {
                decimal rub = ConvertToRub(value, currency, data);             

                string[] neededCurrencies = { "UAH", "USD", "EUR" };

                var result = data?.Valute
                    .Where(i => neededCurrencies.Contains(i.CharCode)
                    ).Select(i =>
                        {
                            var formatInfo = new NumberFormatInfo { NumberDecimalSeparator = "," };

                            return new KeyValuePair<string, decimal>(
                                i.CharCode,
                                rub / (decimal.Parse(i.Value, formatInfo) / i.Nominal)
                            );
                        }
                    ).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                result?.Add("RUB", rub);

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
