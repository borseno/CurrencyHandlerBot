using System;
using System.Linq;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CurrencyHandler.Models.InlineKeyboardHandlers
{
    public class DisplayCurrenciesKeyboardHandler : InlineKeyboardHandler
    {
        private const string ChosenText = " ✅";

        public DisplayCurrenciesKeyboardHandler(ICurrenciesRepository repo) : base(repo)
        {
        }

        public override string Name => "DisplayCurrencies";

        public override void HandleCallBack(CallbackQuery callbackQuery)
        {
            throw new NotImplementedException();
        }

        public override async Task HandleCallBackAsync(CallbackQuery callbackQuery)
        {
            var chatID = callbackQuery.Message.Chat.Id;
            var msgID = callbackQuery.Message.MessageId;

            var buttonContent = GetTextFromCallbackData(callbackQuery);
            var chatId = callbackQuery.Message.Chat.Id;
            var buttonCurrencyEmoji = buttonContent.TrimEnd(ChosenText.ToCharArray());

            string answer;
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

            await Bot.AnswerCallbackQueryAsync(callbackQuery.Id, answer);
        }

        private async Task ProcessAddAsync(string emoji, long chatID)
        {
            await Repository.AddDisplayEmojisAsync(emoji, chatID);
        }

        private async Task ProcessRemoveAsync(string emoji, long chatID)
        {
            await Repository.RemoveDisplayEmojisAsync(emoji, chatID);
        }

        public override void SendKeyboard(Message message)
        {
            throw new NotImplementedException();
        }

        public override async Task SendKeyboardAsync(Message message)
        {
            var chatId = message.Chat.Id;

            var keyboard = await GetHighlightedKeyboardAsync(chatId);

            await Bot.SendTextMessageAsync(chatId, "Choose currencies to display", replyMarkup: keyboard);
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
                foreach (var current in row)
                {
                    // if this emoji is not chosen, then don't highlight it
                    if (chosenEmojis.FirstOrDefault(emoji => current.Text.Contains(emoji)) == null)
                        continue;

                    // else - highlight it
                    current.CallbackData += ChosenText;
                    current.Text += ChosenText;
                }
            }

            return keyboard;
        }

        private async Task UpdateKeyboardAsync(long chatID, int messageID)
        {
            var keyboardTask = GetHighlightedKeyboardAsync(chatID);

            await Task.WhenAll(keyboardTask);

            await Bot.EditMessageReplyMarkupAsync(chatID, messageID, keyboardTask.Result);
        }
    }
}
