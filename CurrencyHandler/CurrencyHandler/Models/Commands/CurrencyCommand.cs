﻿using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CurrencyHandler.Models.Commands
{
    class CurrencyCommand : Command
    {
        public static CurrencyCommand Instance { get; } = new CurrencyCommand();

        public override string Name => "Currency";

        private CurrencyCommand()
        {

        }

        public override async Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo)
        {
            var chatId = message.Chat.Id;

            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new [] // first row
                {
                    InlineKeyboardButton.WithCallbackData("🇺🇦"),
                    InlineKeyboardButton.WithCallbackData("🇪🇺"),
                },
                new [] // second row
                {
                    InlineKeyboardButton.WithCallbackData("🇷🇺"),
                    InlineKeyboardButton.WithCallbackData("🇺🇸"),
                }
            });

            await client.SendTextMessageAsync(chatId, "Choose currency", replyMarkup: inlineKeyboard);
        }
    }
}