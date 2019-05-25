using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types.InlineQueryResults;

namespace CurrencyHandler.Models.HelperClasses
{
    public static class InlineAnswerBuilder
    {
        /// <summary>
        /// combines Id of message, Title of message, Text of message into a Message (InlineQueryResult), also called an "Article".
        /// </summary>
        /// <param name="id">id of article</param>
        /// <param name="title">title - what user sees even before submitting to send that QueryResult</param>
        /// <param name="text">text - what will be send to a chat</param>
        /// <returns></returns>
        public static async Task<IEnumerable<InlineQueryResultBase>> ArticleToQueryResultAsync(string id, string title, string text = "")
        {
            return await Task.Run(() =>
            {
                if (text == null)
                    text = string.Empty;

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
