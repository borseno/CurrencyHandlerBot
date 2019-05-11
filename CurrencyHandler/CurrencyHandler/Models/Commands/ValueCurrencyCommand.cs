using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.InlineKeyboardHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;
using System;

namespace CurrencyHandler.Models.Commands
{
    class ValueCurrencyCommand : Command
    {
        public static ValueCurrencyCommand Instance { get; } = new ValueCurrencyCommand();

        public override string Name => "ValueCurrency";

        private ValueCurrencyCommand()
        {

        }

        public override async Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo)
        {
            var keyboard = Keyboards.FirstOrDefault(repo, Name);

            if (keyboard == null)
            {
                throw new InvalidOperationException("could not find an appropriate keyboard");
            }

            await keyboard.SendKeyboardAsync(message, client);
        }
    }
}
