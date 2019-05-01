using CurrencyHandler.Models.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyHandler.Models.WorkerClasses
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

                StringBuilder builder = new StringBuilder(200);

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
    }
}
