using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types.InlineQueryResults;

namespace CurrencyHandler.Models.WorkerClasses
{
    public static class InlineAnswerBuilder
    {
        public static async Task<IEnumerable<InlineQueryResultBase>> ArticleToQueryResultAsync(string id, string title, string text = "")
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
