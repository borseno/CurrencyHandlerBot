using System;
using System.Threading.Tasks;
using CurrencyHandler.Models.Commands.Abstractions;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.InlineKeyboardHandlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    public class DisplayCurrenciesCommand : Command
    {
        public DisplayCurrenciesCommand(IKeyboards keyboards, ICurrenciesRepository repo) : base(keyboards, repo)
        {
        }

        public override string Name => "DisplayCurrencies";

        /// <summary>
        /// Sends DisplayCurrenciesKeyboard to the chat 
        /// </summary>
        /// <param name="message">the message a user sent</param>
        /// <param name="client">Bot instance, needed to answer on the message</param>
        /// <param name="repo">Repository for the whole db, allows this command handler to save/read data</param>
        /// <returns>Task to be awaited</returns>
        public override async Task Execute(Message message)
        {
            var keyboard = Keyboards.FirstOrDefault(Name);

            if (keyboard != null)
            {
                await keyboard.SendKeyboardAsync(message);
            }
            else
            {
                throw new InvalidOperationException("could not find an appropriate keyboard");
            }
        }
    }
}
