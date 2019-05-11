using System;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.InlineKeyboardHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    public class DisplayCurrenciesCommand : Command
    {
        public static DisplayCurrenciesCommand Instance => new DisplayCurrenciesCommand();

        public override string Name => "DisplayCurrencies";

        /// <summary>
        /// Sends DisplayCurrenciesKeyboard to the chat 
        /// </summary>
        /// <param name="message">the message a user sent</param>
        /// <param name="client">Bot instance, needed to answer on the message</param>
        /// <param name="repo">Repository for the whole db, allows this command handler to save/read data</param>
        /// <returns>Task to be awaited</returns>
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
