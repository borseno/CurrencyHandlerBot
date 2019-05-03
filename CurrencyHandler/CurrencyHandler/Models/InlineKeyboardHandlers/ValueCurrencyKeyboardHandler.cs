using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Models;
using CurrencyHandler.Models.Database.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CurrencyHandler.Models.InlineKeyboardHandlers
{
    public class ValueCurrencyKeyboardHandler : InlineKeyboardHandler
    {
        public override string Name => "ValueCurrency";

        public ValueCurrencyKeyboardHandler(CurrenciesRepository repo) : base(repo)
        {
        }

        public override void HandleCallBack(CallbackQuery callbackQuery)
        {
            var bot = Bot.Get().GetAwaiter().GetResult();

            var currencyEmoji = Repository.GetCurrencyEmojiFromEmoji(callbackQuery.Data);

            if (currencyEmoji != null)
            {
                HandleCurrencyChange(callbackQuery.Message.Chat.Id, currencyEmoji);

                var msg = $"You've successfully set your currency to {currencyEmoji.Emoji}!";

                bot.AnswerCallbackQueryAsync(callbackQuery.Id, msg).GetAwaiter().GetResult();

                bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, msg).GetAwaiter().GetResult();
            }
        }

        public override async Task HandleCallBackAsync(CallbackQuery callbackQuery)
        {
            var bot = await Bot.Get();
            var buttonText = this.GetTextFromCallbackData(callbackQuery);

            var currencyEmoji = await Repository.GetCurrencyEmojiFromEmojiAsync(buttonText);

            if (currencyEmoji != null)
            {
                await HandleCurrencyChangeAsync(callbackQuery.Message.Chat.Id, currencyEmoji);

                var msg = $"You've successfully set your currency to {currencyEmoji.Emoji}!";

                await bot.AnswerCallbackQueryAsync(callbackQuery.Id, msg);

                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, msg);
            }
            else
            {
                var msg = $"Could not set currency to {buttonText} :(";

                await bot.AnswerCallbackQueryAsync(callbackQuery.Id, msg);

                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, msg);
            }
        }

        public override void SendKeyboard(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;

            var currencies = Repository.GetAllCurrencies();

            var keyboard = StringArrayToKeyboard(currencies);

            client.SendTextMessageAsync(chatId, "Choose value currency", replyMarkup: keyboard).GetAwaiter().GetResult();
        }

        public override async Task SendKeyboardAsync(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;

            var currencies = await Repository.GetAllEmojisAsync();

            var keyboard = StringArrayToKeyboard(currencies);

            await client.SendTextMessageAsync(chatId, "Choose value currency", replyMarkup: keyboard);
        }

        private void HandleCurrencyChange(long chatId, CurrencyEmoji ce)
        {
            Repository.SetCurrency(ce.Currency, chatId);
        }

        private async Task HandleCurrencyChangeAsync(long chatId, CurrencyEmoji ce)
        {
            await Repository.SetCurrencyAsync(ce.Currency, chatId);
        }
    }
}
