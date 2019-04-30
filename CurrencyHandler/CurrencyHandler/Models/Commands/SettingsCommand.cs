using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Contexts;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.DbModels;
using Telegram.Bot;
using Telegram.Bot.Types;
using IO = System.IO;

namespace CurrencyHandler.Models.Commands
{
    public class SettingsCommand : Command
    {
        private const string SettingsDescriptionTextPath = "Texts/SettingsDescription.txt";

        private SettingsCommand()
        {

        }

        public static SettingsCommand Instance { get; } = new SettingsCommand();

        public override string Name => "Settings";

        public override async Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo)
        {
            var messageId = message.MessageId;
            var chatId = message.Chat.Id;
            var nl = Environment.NewLine;

            var percents = await repo.GetPercentsAsync(chatId);
            var currency = await repo.GetCurrencyAsync(chatId);

            var text = $"Settings for this chat: {nl}" +
                       $"Percents: {percents}{nl}" +
                       $"Currency: {currency}";

            await client.SendTextMessageAsync(chatId, text, replyToMessageId: messageId);
        }
    }
}
