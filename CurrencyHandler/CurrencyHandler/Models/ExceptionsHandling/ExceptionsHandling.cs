using CurrencyHandler.Models.QueryHandling;
using CurrencyHandler.Models.WorkerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CurrencyHandler.Models.ExceptionsHandling
{
    public static class ExceptionsHandling
    {
        private static readonly string nl = Environment.NewLine;

        public static async Task HandleExceptionAsync(Exception e, Update update, UpdateType type)
        {
            var bot = await Bot.Get();

            var exceptionMsg = 
                $"Exception type: {e.GetType().Name}{nl}" +
                $"Exception message: {e.Message}{nl}" +
                $"InnerException: {nl}{e.InnerException}"; 

            if (type == UpdateType.CallbackQuery)
            {
                await bot.AnswerCallbackQueryAsync(update.CallbackQuery.Id, exceptionMsg);
                await bot.SendTextMessageAsync(update.CallbackQuery.From.Id, exceptionMsg);
            }
           
            if (type == UpdateType.InlineQuery)
            {
                var answer = await InlineAnswerBuilder.ArticleToQueryResultAsync("Error", exceptionMsg, exceptionMsg);

                await bot.AnswerInlineQueryAsync(update.InlineQuery.Id, answer);
            }

            if (type == UpdateType.Message)
            {
                await bot.SendTextMessageAsync(update.Message.Chat.Id, exceptionMsg);
            }
        }
    }
}
