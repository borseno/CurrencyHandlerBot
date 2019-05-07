using System;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    // TODO: Let user change currencies to display with checkbox
    // checkbox is implemented via inlinekeyboard and check symbol on clicking a button.

    public class DisplayCurrenciesCommand : Command
    {
        public static DisplayCurrenciesCommand Instance => throw new NotImplementedException();

        public override string Name => throw new NotImplementedException();

        public override Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo)
        {
            throw new NotImplementedException();
        }
    }
}
