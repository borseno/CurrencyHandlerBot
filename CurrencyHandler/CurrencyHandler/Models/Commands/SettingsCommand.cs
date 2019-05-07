using System;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    public class SettingsCommand : Command
    {
        public static SettingsCommand Instance { get; } = new SettingsCommand();

        public override string Name => "Settings";

        private SettingsCommand()
        {

        }

        public override async Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo)
        {
            var messageId = message.MessageId;
            var chatId = message.Chat.Id;
            var nl = Environment.NewLine; // just a shortcut

            var percentsTask = repo.GetPercentsAsync(chatId);
            var currencyTask = repo.GetCurrencyAsync(chatId);

            // execute them in parallel
            await Task.WhenAll(percentsTask, currencyTask); 

            var text = $"Settings for this chat: {nl}" +
                       $"Percents: {percentsTask.Result}{nl}" +
                       $"Currency: {currencyTask.Result}";

            await client.SendTextMessageAsync(chatId, text, replyToMessageId: messageId);
        }
    }
}
