using System;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.InlineKeyboardHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    public abstract class Command : IDisposable
    {
        public Command(Keyboards keyboards, CurrenciesRepository repo)
        {
            Keyboards = keyboards;
            Repo = repo;
            Client = Bot.GetClient();
        }

        protected TelegramBotClient Client { get; }

        protected CurrenciesRepository Repo { get; }

        protected Keyboards Keyboards { get; }

        public abstract string Name { get; }

        public abstract Task Execute(Message message);

        /// <summary>
        /// Indicates whether the given command is this command or not.
        /// <para>The given command should contain both bot name and command name.</para>
        /// </summary>
        /// <param name="command"></param>
        /// <returns>value that indicates whether the command is valid or not</returns>
        public virtual bool Contains(string command)
        {
            if (command == null)
                return false;

            return command.ToLower().Contains(Name.ToLower());
        }

        public void Dispose()
        {
            ((IDisposable)Repo).Dispose();
            ((IDisposable)Keyboards).Dispose();
        }
    }
}
