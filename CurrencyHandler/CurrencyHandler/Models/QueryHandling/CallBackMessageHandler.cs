using System;
using System.Linq;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Contexts;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.QueryHandling
{
    class CallBackMessageHandler
    {
        private readonly CurrenciesRepository _repo;

        private readonly string[] _currencies = { "🇺🇦", "🇪🇺", "🇷🇺", "🇺🇸" };

        public CallBackMessageHandler(CurrenciesRepository repo)
        {
            _repo = repo;
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
                await _repo.SetCurrencyAsync(value, chatId);
            else
                throw new InvalidOperationException("Attempt to save wrong currency value to db");
            
        }
    }
}
