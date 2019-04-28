using System;
using System.Linq;
using System.Threading.Tasks;
using CurrencyHandler.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.QueryHandling
{
    class CallBackMessageHandler
    {
        private readonly string[] _currencies = { "🇺🇦", "🇪🇺", "🇷🇺", "🇺🇸" };

        private DbContext _ctx;

        public CallBackMessageHandler(DbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task Handle(CallbackQuery callbackQuery)
        { 
            var bot = await Bot.Get();

            if (_currencies.Contains(callbackQuery.Data))
            {
                await HandleCurrencyChange(callbackQuery.Message.Chat.Id, callbackQuery.Data);

                await bot.AnswerCallbackQueryAsync(callbackQuery.Id,
                    $"You've successfully set your currency to {callbackQuery.Data}!");

                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id,
                    $"You've successfully set your currency to {callbackQuery.Data}!");
            }
        }

        private async Task HandleCurrencyChange(long chatId, string currency)
        {
            string value = null;

            switch (currency)
            {
                case "🇺🇦":
                    value = "UAH";
                    break;
                case "🇪🇺":
                    value = "EUR";
                    break;
                case "🇷🇺":
                    value = "RUB";
                    break;
                case "🇺🇸":
                    value = "USD";
                    break;
            }

            if (value != null)
            {
                await Settings.SetCurrencyForChat(chatId, value, (ChatSettingsContext)_ctx);
            }
            else
            {
                throw new InvalidOperationException("Attempt to save wrong currency value to db");
            }
        }
    }
}
