using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public abstract Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo);

        /// <summary>
        /// Indicates whether the given command is this command or not.
        /// <para>The given command should contain both bot's name and command's name.</para>
        /// </summary>
        /// <param name="command"></param>
        /// <returns>value that indicates whether the command is valid or not</returns>
        public virtual bool Contains(string command)
        {
            if (command == null)
                return false;

            return command.ToLower().Contains(Name.ToLower());
        }
    }
}
