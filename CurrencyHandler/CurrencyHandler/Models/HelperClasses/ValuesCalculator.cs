using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Models;
using MainBankOfRussia.XmlModels;

namespace CurrencyHandler.Models.HelperClasses
{
    public static class ValuesCalculator
    {
        private static readonly NumberFormatInfo DecimalFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "," };

        public static async Task<Dictionary<CurrencyEmoji, decimal>> GetCurrenciesValuesAsync(decimal value, CurrencyEmoji valueCurrencyEmoji, ValCurs data, CurrencyEmoji[] neededCurrencies)
        {
            return await Task.Run(() =>
            {
                var currency = valueCurrencyEmoji.Currency;

                decimal rub = ConvertToRub(value, currency, data); // whatever currency the value is, it is processed as rub            

                Dictionary<ValCursValute, CurrencyEmoji> currencies = new Dictionary<ValCursValute, CurrencyEmoji>(neededCurrencies.Length);

                foreach (var i in data.Valute)
                {
                    foreach (var j in neededCurrencies)
                        if (i.CharCode == j.Currency)
                            currencies.Add(i, j);
                }

                var result = currencies.Select(
                    i =>
                        {
                            var valute = i.Key;
                            var currencyEmoji = i.Value;

                            return new KeyValuePair<CurrencyEmoji, decimal>(
                                currencyEmoji,
                                rub / (decimal.Parse(valute.Value, DecimalFormatInfo) / valute.Nominal)
                            );
                        }
                    ).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                var rubEmoji = neededCurrencies.FirstOrDefault(ce => ce.Currency == DefaultValues.APICurrency);

                if (rubEmoji != null)
                    result.Add(rubEmoji, rub);

                if (!result.TryGetValue(valueCurrencyEmoji, out _))
                    result.Add(valueCurrencyEmoji, value);

                return result;
            });
        }

        private static decimal ConvertToRub(decimal value, string currency, ValCurs data)
        {
            if (currency == DefaultValues.APICurrency)
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
