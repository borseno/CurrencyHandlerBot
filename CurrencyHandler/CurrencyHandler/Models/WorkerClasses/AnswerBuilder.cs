using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyHandler.Models.WorkerClasses
{
    public static class AnswerBuilder
    {
        public static async Task<string> BuildStringFromValuesAsync(Dictionary<string, decimal> values,
            decimal percents)
        {
            return await Task.Run(() =>
            {
                const int points = 2;
                const int completeValue = 100;

                foreach (var key in values.Keys.ToList())
                    values[key] = Math.Round(values[key] * percents / completeValue, points);

                StringBuilder builder = new StringBuilder(128);

                builder.AppendLine($"Percents to be considered: {percents}");

                foreach (var (key, value) in values)
                {
                    string emoji;

                    switch (key) // select currency emoji
                    {
                        case "RUB":
                            emoji = "🇷🇺";
                            break;
                        case "UAH":
                            emoji = "🇺🇦";
                            break;
                        case "USD":
                            emoji = "🇺🇸";
                            break;
                        case "EUR":
                            emoji = "🇪🇺";
                            break;
                        default:
                            throw new Exception("Could not find an appropriate emoji for the currency");
                    }

                    builder.AppendLine($"{emoji} {key}: {value}");
                }

                return builder.ToString();
            });
        }
    }
}
