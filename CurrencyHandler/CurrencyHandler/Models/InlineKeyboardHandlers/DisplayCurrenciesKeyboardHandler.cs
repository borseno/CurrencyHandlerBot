﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CurrencyHandler.Models.InlineKeyboardHandlers
{
    public class DisplayCurrenciesKeyboardHandler : InlineKeyboardHandler
    {
        private const string UnchosenText = "     ";
        private const string ChosenText = " ✅";

        public override string Name => "DisplayCurrencies";

        public DisplayCurrenciesKeyboardHandler(CurrenciesRepository repo) : base(repo)
        {
        }

        public override void HandleCallBack(CallbackQuery callbackQuery)
        {
            throw new NotImplementedException();
        }

        public override async Task HandleCallBackAsync(CallbackQuery callbackQuery)
        {
            var bot = await Bot.Get();

            var chatID = callbackQuery.Message.Chat.Id;
            var msgID = callbackQuery.Message.MessageId;

            var buttonContent = GetTextFromCallbackData(callbackQuery);
            var chatId = callbackQuery.Message.Chat.Id;
            var buttonCurrencyEmoji = buttonContent.TrimEnd(ChosenText.ToCharArray());

            string answer = null;
            if (buttonContent.Contains(ChosenText))
            {
                await ProcessRemoveAsync(buttonCurrencyEmoji, chatId);
                answer = $"You've successfully removed {buttonCurrencyEmoji} from displaying!";
            }
            else
            {
                await ProcessAddAsync(buttonCurrencyEmoji, chatId);
                answer = $"You've successfully added {buttonCurrencyEmoji} to display!";
            }  

            await UpdateKeyboardAsync(chatID, msgID);

            await bot.AnswerCallbackQueryAsync(callbackQuery.Id, answer);
        }

        private async Task ProcessAddAsync(string emoji, long chatID)
        {
            await Repository.AddDisplayEmojiesAsync(emoji, chatID);
        }

        private async Task ProcessRemoveAsync(string emoji, long chatID)
        {
            await Repository.RemoveDisplayEmojiesAsync(emoji, chatID);
        }

        public override void SendKeyboard(Message message, TelegramBotClient client)
        {
            throw new NotImplementedException();
        }

        public override async Task SendKeyboardAsync(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;

            var keyboard = await GetHighlightedKeyboardAsync(chatId);

            await client.SendTextMessageAsync(chatId, "Choose currencies to display", replyMarkup: keyboard);
        }

        private async Task<InlineKeyboardMarkup> GetHighlightedKeyboardAsync(long chatId)
        {
            var emojisTask = Repository.GetAllEmojisAsync();
            var chosenEmojisTask = Repository.GetDisplayEmojisAsync(chatId);

            await Task.WhenAll(emojisTask, chosenEmojisTask);

            var keyboard = StringArrayToKeyboard(emojisTask.Result);
            var buttons = keyboard.InlineKeyboard;
            var chosenEmojis = chosenEmojisTask.Result;

            foreach (var row in buttons)
            {
                var length = row.Count();

                for (int i = 0; i < length; i++)
                {
                    var current = row.ElementAt(i);

                    if (chosenEmojis.FirstOrDefault(emoji => current.Text.Contains(emoji)) != null)
                    {
                        current.CallbackData += ChosenText;
                        current.Text += ChosenText;
                    }
                    else
                    {
                        current.Text += UnchosenText;
                    }
                }
            }

            return keyboard;
        }

        private async Task UpdateKeyboardAsync(long chatID, int messageID)
        {
            var botTask = Bot.Get();
            var keyboardTask = GetHighlightedKeyboardAsync(chatID);

            await Task.WhenAll(botTask, keyboardTask);

            await botTask.Result.EditMessageReplyMarkupAsync(chatID, messageID, keyboardTask.Result);
        }
    }
}