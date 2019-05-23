using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.InlineKeyboardHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;
using System;

namespace CurrencyHandler.Models.Commands
{
    public class ValueCurrencyCommand : Command
    {
        public ValueCurrencyCommand(Keyboards keyboards, CurrenciesRepository repo) : base(keyboards, repo)
        {
        }

        public override string Name => "ValueCurrency";

        public override async Task Execute(Message message)
        {
            var keyboard = Keyboards.FirstOrDefault(Name);

            if (keyboard == null)
            {
                throw new InvalidOperationException("could not find an appropriate keyboard");
            }

            await keyboard.SendKeyboardAsync(message);
        }
    }
}
