using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.HelperClasses;
using Telegram.Bot;
using Telegram.Bot.Types;
using static System.String;
using static CurrencyHandler.Models.DataCaching.CurrenciesDataCaching;

namespace CurrencyHandler.Models.QueryHandling
{
    public class InlineQueryHandler : IInlineQueryHandler
    {
        private readonly ICurrenciesEmojisRepository repo;
        private readonly ITelegramBotClient bot;

        public InlineQueryHandler(ICurrenciesEmojisRepository repo)
        {
            this.repo = repo;
            this.bot = Bot.GetClient();
        }

        public async Task HandleAsync(InlineQuery q)
        {
            if (IsNullOrEmpty(q.Query))
                return;

            var input = q.Query;

            var dataTask = GetValCursAsync();
            var currenciesEmojisTask = repo.GetCurrencyEmojisAsync();

            var data = dataTask.Result;
            var currenciesEmojis = currenciesEmojisTask.Result;

            string currency = null;
            foreach (var i in currenciesEmojis)
                if (input.Contains(i.Currency))
                {
                    currency = i.Currency;
                    break;
                }

            var space = " "; // format: {decimal}{space}{currency} e.g "53.2 UAH" or just {decimal}
            var argsPart = currency == null ? "" : $"{space}{currency}";

            var pattern = $@"[0-9][0-9]*([\.,][0-9][0-9]*)?{argsPart}";

            if (!Regex.IsMatch(input, pattern))
            {
                var text = "format: {decimal}{space}{currency} e.g \"53.2 UAH\" or just {decimal} e.g \"53.2\"";

                await bot.AnswerInlineQueryAsync(
                    q.Id,
                    await InlineAnswerBuilder.ArticleToQueryResultAsync(
                        "Result", text, text)
                );

                return;
            }

            var valueToParse = IsNullOrEmpty(argsPart) ? input : input.Replace(argsPart, "");
            var isValid = decimal.TryParse(valueToParse, out var value);

            if (isValid)
            {
                if (currency == null)
                    currency = DefaultValues.DefaultValueCurrency;

                var currencyEmoji = currenciesEmojis.First(ce => ce.Currency == currency);

                var values =
                    await ValuesCalculator.GetCurrenciesValuesAsync(value, currencyEmoji, data, currenciesEmojis);

                var answer1 = await AnswerBuilder.BuildStringFromValuesAsync(values, currencyEmoji);

                await bot.AnswerInlineQueryAsync(
                    q.Id,
                    await InlineAnswerBuilder.ArticleToQueryResultAsync(
                        "Result", answer1, answer1)
                );
            }
        }

        public void Dispose()
        {
            repo.Dispose();
        }
    }
}
