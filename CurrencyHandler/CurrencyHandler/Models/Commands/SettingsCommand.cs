using System;
using System.Threading.Tasks;
using CurrencyHandler.Models.Commands.Abstractions;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.InlineKeyboardHandlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    public class SettingsCommand : Command
    {
        public SettingsCommand(IKeyboards keyboards, ICurrenciesRepository repo) : base(keyboards, repo)
        {
        }

        public override string Name => "Settings";

        // TODO: 1. Add DisplayCurrencies to settings output
        // TODO: 2. Add emojis to currencies
        public override async Task Execute(Message message)
        {
            var messageId = message.MessageId;
            var chatId = message.Chat.Id;
            var nl = Environment.NewLine; // just a shortcut

            var percentsTask = Repo.GetPercentsAsync(chatId);
            var currencyTask = Repo.GetCurrencyAsync(chatId);

            // execute them in parallel
            await Task.WhenAll(percentsTask, currencyTask);

            var text = $"Settings for this chat: {nl}" +
                       $"Percents: {percentsTask.Result}{nl}" +
                       $"Currency: {currencyTask.Result}";

            await Client.SendTextMessageAsync(chatId, text, replyToMessageId: messageId);
        }
    }
}
