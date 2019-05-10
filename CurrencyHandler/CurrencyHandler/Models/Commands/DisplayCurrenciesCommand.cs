using System;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.InlineKeyboardHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    // TODO: Let user change currencies to display with checkbox
    // checkbox is implemented via inlinekeyboard and check symbol on clicking a button.

    public class DisplayCurrenciesCommand : Command
    {
        public static DisplayCurrenciesCommand Instance => new DisplayCurrenciesCommand();

        public override string Name => "DisplayCurrencies";

        public override async Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo)
        {
            var keyboard = Keyboards.FirstOrDefault(repo, Name);

            if (keyboard != null)
            {
                await keyboard.SendKeyboardAsync(message, client);
            }
            else
            {
                throw new InvalidOperationException("could not find an appropriate keyboard");
            }
        }
    }
}
