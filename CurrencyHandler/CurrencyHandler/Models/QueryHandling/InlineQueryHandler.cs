using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.DataCaching;
using CurrencyHandler.Models.WorkerClasses;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace CurrencyHandler.Models.QueryHandling
{
    public class InlineQueryHandler
    {
        private readonly CurrenciesRepository _repo;

        private readonly string[] Currencies =
        {
            "UAH", "RUB", "EUR", "USD"
        };

        public InlineQueryHandler(CurrenciesRepository repo)
        {
            _repo = repo;
        }

        public async Task HandleAsync(InlineQuery q)
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

            var space = " "; // format: {decimal}{space}{currency} e.g "53.2 UAH" or just {decimal}
            var argsPart = currency == null ? "" : $"{space}{currency}";

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
                        await ValuesCalculator.GetCurrenciesValuesAsync(value, currency, data, Currencies);

                    var answer1 = await AnswerBuilder.BuildStringFromValuesAsync(values, 100);

                    await bot.AnswerInlineQueryAsync(
                        q.Id, 
                        await InlineAnswerBuilder.ArticleToQueryResultAsync(
                            "Result", answer1, answer1)
                        );
                }
            }
        }
    }
}
