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
            var bot = await Bot.GetAsync();

            var exceptionMsg =
                $"Exception type: {e.GetType().Name}{NL}" +
                $"Exception message: {e.Message}{NL}" +
                $"InnerException: {NL}{e.InnerException}";

            switch (type)
            {
                case UpdateType.CallbackQuery:
                {
                    var callbackTask = bot.AnswerCallbackQueryAsync(update.CallbackQuery.Id, exceptionMsg);
                    var textTask = bot.SendTextMessageAsync(update.CallbackQuery.From.Id, exceptionMsg);

                    await Task.WhenAll(callbackTask, textTask);
                    break;
                }
                case UpdateType.InlineQuery:
                {
                    var answer = await InlineAnswerBuilder.ArticleToQueryResultAsync("Error", exceptionMsg, exceptionMsg);

                    await bot.AnswerInlineQueryAsync(update.InlineQuery.Id, answer);
                    break;
                }
                case UpdateType.Message:
                    await bot.SendTextMessageAsync(update.Message.Chat.Id, exceptionMsg);
                    break;
            }
        }
    }
}
