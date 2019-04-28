using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CurrencyHandler.Models.DataCaching;
using CurrencyHandler.Models.WorkerClasses;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace CurrencyHandler.Models.QueryHandling
{
    public static class InlineQueryHandler
    {
        private static readonly string[] Currencies =
        {
            "UAH", "RUB", "EUR", "USD"
        };

        public static async Task Handle(InlineQuery q)
        {
            if (String.IsNullOrEmpty(q.Query))
                return;

            var bot = await Bot.Get();

            var data = await CurrenciesDataCaching.GetValCurs();
            var input = q.Query;

            string currency = null;

            foreach (var i in Currencies)
                if (input.Contains(i))
                {
                    currency = i;
                    break;
                }

            var argsPart = currency == null ? "" : $" {currency}";

            string pattern = $@"[0-9][0-9]*([\.,][0-9][0-9]*)?{argsPart}";

            if (Regex.IsMatch(input, pattern))
            {
                var valueToParse = String.IsNullOrEmpty(argsPart) ? input : input.Replace(argsPart, "");

                var isValid = decimal.TryParse(valueToParse, out var value);

                if (isValid)
                {
                    if (currency == null)
                        currency = "UAH";

                    var values =
                        (await ValuesCalculator.GetValuesInRubAndOtherCurrencies(value, currency, data))
                        .ToDictionary(t => t.Key, t => t.Value);

                    var answer1 = await AnswerBuilder.BuildStringFromValuesAsync(values, 100);

                    await bot.AnswerInlineQueryAsync(q.Id, await GetResult("Result", answer1, answer1));
                }
            }
        }

        public static async Task<IEnumerable<InlineQueryResultBase>> GetResult(string id, string title, string text = "")
        {
            return await Task.Run(() =>
            {
                if (text == null)
                    text = String.Empty;

                var content = new InputTextMessageContent(text);

                InlineQueryResultBase[] results =
                {
                    new InlineQueryResultArticle(id, title, content)
                };

                return results;
            });
        }
    }
}
