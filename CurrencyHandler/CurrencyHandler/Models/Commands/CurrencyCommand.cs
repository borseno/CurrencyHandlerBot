using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.InlineKeyboardHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq;
using System;

namespace CurrencyHandler.Models.Commands
{
    class CurrencyCommand : Command
    {
        public static CurrencyCommand Instance { get; } = new CurrencyCommand();

        public override string Name => "ValueCurrency";

        private CurrencyCommand()
        {

        }

        public override async Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo)
        {
            var keyboard = Keyboards.Get(repo).FirstOrDefault(kb => kb.Name.ToLower() == Name.ToLower());

            if (keyboard != null)
            {
                await keyboard.SendKeyboardAsync(message, client);
            }
            else
                throw new InvalidOperationException("could not find an appropriate keyboard");
        }
    }
}
