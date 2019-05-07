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
        public static async Task<string> BuildStringFromValuesAsync(Dictionary<CurrencyEmoji, decimal> values,
            decimal percents = 100)
        {
            return await Task.Run(() =>
            {
                const int points = 2;
                const int completeValue = 100;

                foreach (var key in values.Keys.ToList())
                    values[key] = Math.Round(values[key] * percents / completeValue, points);

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

        public static async Task<string> BuildStringFromValuesAsync(Dictionary<CurrencyEmoji, decimal> values, CurrencyEmoji mainValue, 
            decimal percents = 100)
        {
            var mainValuePair = values.First(t => t.Key.Currency == mainValue.Currency);

            var needed = values
                .Where
                (t => !t.Equals(mainValuePair)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var str = await BuildStringFromValuesAsync(needed, percents);

            return $"Given value: {mainValuePair.Key.Emoji} {mainValuePair.Value} {mainValuePair.Key.Currency}" +
                $"{Environment.NewLine}{Environment.NewLine}" + str;
        }
    }
}
