using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Models;

namespace CurrencyHandler.Models.HelperClasses
{
    public static class AnswerBuilder
    {
        public static async Task<string> BuildStringFromValuesAsync(IEnumerable<KeyValuePair<CurrencyEmoji, decimal>> valuesArg,
            decimal percents = DefaultValues.DefaultPercents)
        {
            return await Task.Run(() =>
            {
                const int points = 2; // 2 points after . in decimal
                const int completeValue = 100; // 100%

                var values = valuesArg.Select(el =>
                {
                    var key = el.Key;
                    var value = el.Value;

                    value = Math.Round(value * percents / completeValue, points);

                    return new KeyValuePair<CurrencyEmoji, decimal>(key, value);
                });
               
                var builder = new StringBuilder(capacity: 200);

                builder.AppendLine($"Percents to be considered: {percents}");

                foreach (var (key, value) in values)
                {
                    var emoji = key.Emoji;
                    var currency = key.Currency;

                    builder.AppendLine($"{emoji} {currency}: {value}");
                }

                return builder.ToString();
            });
        }

        public static async Task<string> BuildStringFromValuesAsync(IEnumerable<KeyValuePair<CurrencyEmoji, decimal>> values, 
            CurrencyEmoji mainValue, decimal percents = DefaultValues.DefaultPercents)
        {
            var mainValuePair = values.First(t => t.Key.Currency == mainValue.Currency);

            // if percents are default (100), then don't also print the value currency, else - print it.
            var needed = 
                percents == DefaultValues.DefaultPercents ? values.Where(t => !t.Equals(mainValuePair))
                : values;

            var str = await BuildStringFromValuesAsync(needed, percents);

            return $"Given value: {mainValuePair.Key.Emoji} {mainValuePair.Value} {mainValuePair.Key.Currency}" +
                $"{Environment.NewLine}{Environment.NewLine}" + str;
        }
    }
}
