using System;
using System.Threading.Tasks;
using CurrencyHandler.Models.HelperClasses;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CurrencyHandler.Models.ExceptionsHandling
{
    /// <summary>
    /// Handles Different types of Telegram.Bot Exceptions in a corresponding manner. 
    /// </summary>
    public static class ExceptionsHandling
    {
        private static readonly string NL = Environment.NewLine;

        public static async Task HandleExceptionAsync(Exception e, Update update, UpdateType type)
        {
            var bot = await Bot.Get();

            var exceptionMsg = 
                $"Exception type: {e.GetType().Name}{NL}" +
                $"Exception message: {e.Message}{NL}" +
                $"InnerException: {NL}{e.InnerException} +{NL}" +
                $"StackTrace: {NL}{e.StackTrace}"; 

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
