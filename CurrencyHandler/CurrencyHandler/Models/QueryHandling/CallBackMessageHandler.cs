using System;
using System.Linq;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Models;
using CurrencyHandler.Models.Database.Repositories;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.QueryHandling
{
    class CallBackMessageHandler
    {
        private readonly CurrenciesRepository _repo;

        public CallBackMessageHandler(CurrenciesRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(CallbackQuery callbackQuery)
        {
            var bot = await Bot.Get();

            var currencyEmoji = await _repo.GetCurrencyEmojiFromEmojiAsync(callbackQuery.Data);

            if (currencyEmoji != null)
            {
                await HandleCurrencyChange(callbackQuery.Message.Chat.Id, currencyEmoji);

                var msg = $"You've successfully set your currency to {currencyEmoji.Emoji}!";

                await bot.AnswerCallbackQueryAsync(callbackQuery.Id, msg);

                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, msg);
            }
        }

        private async Task HandleCurrencyChange(long chatId, CurrencyEmoji ce)
        {
            await _repo.SetCurrencyAsync(ce.Currency, chatId);
        }
    }
}
