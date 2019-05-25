using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using System;
using CurrencyHandler.Models.Commands.Abstractions;
using CurrencyHandler.Models.InlineKeyboardHandlers.Abstractions;

namespace CurrencyHandler.Models.Commands
{
    public class ValueCurrencyCommand : Command, IValueCurrencyCommand
    {
        public ValueCurrencyCommand(IKeyboards keyboards, ICurrenciesRepository repo) : base(keyboards, repo)
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
