using System.Linq;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Models;
using CurrencyHandler.Models.Database.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

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
            var bot = Bot.GetAsync().GetAwaiter().GetResult();
            var buttonText = GetTextFromCallbackData(callbackQuery);
            var currencyEmoji = Repository.GetCurrencyEmojiFromEmoji(buttonText);

            string answer;
            if (currencyEmoji != null)
            {
                HandleCurrencyChange(callbackQuery.Message.Chat.Id, currencyEmoji);

                answer = $"You've successfully set your currency to {currencyEmoji.Emoji}!";
            }
            else
            {
                answer = $"Could not set currency to {buttonText} :(";
            }

            var callbackAnswerTask = bot.AnswerCallbackQueryAsync(callbackQuery.Id, answer);

            var textAnswerTask = bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, answer);

            Task.WhenAll(callbackAnswerTask, textAnswerTask).GetAwaiter().GetResult();
        }

        public override async Task HandleCallBackAsync(CallbackQuery callbackQuery)
        {
            var bot = await Bot.GetAsync();
            var buttonText = GetTextFromCallbackData(callbackQuery);
            var currencyEmoji = await Repository.GetCurrencyEmojiFromEmojiAsync(buttonText);

            string answer;
            if (currencyEmoji != null)
            {
                await HandleCurrencyChangeAsync(callbackQuery.Message.Chat.Id, currencyEmoji);

                answer = $"You've successfully set your currency to {currencyEmoji.Emoji}!";
            }
            else
            {
                answer = $"Could not set currency to {buttonText} :(";
            }

            var callbackAnswerTask = bot.AnswerCallbackQueryAsync(callbackQuery.Id, answer);

            var textAnswerTask = bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, answer);

            await Task.WhenAll(callbackAnswerTask, textAnswerTask);
        }

        public override void SendKeyboard(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;

            var currencies = Repository.GetAllCurrencies().ToArray();

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
