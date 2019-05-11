using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Models;
using MainBankOfRussia.XmlModels;

namespace CurrencyHandler.Models.HelperClasses
{
    // TODO: change return type to IEnumerable<KeyValuePair<>> everywhere
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public static class ValuesCalculator
    {
        private static readonly NumberFormatInfo DecimalFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "," };

        public static async Task<Dictionary<CurrencyEmoji, decimal>> GetCurrenciesValuesAsync(decimal value, CurrencyEmoji valueCurrencyEmoji, ValCurs data, IReadOnlyList<CurrencyEmoji> neededCurrencies)
        {
            return await Task.Run(() =>
            {
                var currency = valueCurrencyEmoji.Currency;

                var rub = ConvertToRub(value, currency, data); // whatever currency the value is, it is processed as rub            

                var currencies = new Dictionary<ValCursValute, CurrencyEmoji>(neededCurrencies.Count);

                foreach (var i in data.Valute)
                {
                    foreach (var j in neededCurrencies)
                        if (i.CharCode == j.Currency)
                            currencies.Add(i, j);
                }

                var result = currencies.Select(
                    i =>
                        {
                            var (valute, currencyEmoji) = i;

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

            // asked currency info
            var instance = data.Valute.First(v => v.CharCode == currency);
            
            // how many rubles one value of this currency contains (e.g one dollar - 20 rubles, oneInRub = 20)
            var oneInRub = Decimal.Parse(instance.Value, DecimalFormatInfo) / instance.Nominal;

            // 5 dollars, oneDollarInRub = 20, return 20 * 5 = 100 rubs
            return oneInRub * value;
        }
    }
}
