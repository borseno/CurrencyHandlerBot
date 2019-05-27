using System;
using System.Threading.Tasks;
using CurrencyHandler.Models.Commands.Abstractions;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.InlineKeyboardHandlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    public abstract class Command : ICommand
    {
        public Command(IKeyboards keyboards, ICurrenciesRepository repo)
        {
            Keyboards = keyboards;
            Repo = repo;
            Client = Bot.GetClient();
        }

        protected ITelegramBotClient Client { get; }

        protected ICurrenciesRepository Repo { get; }

        protected IKeyboards Keyboards { get; }

        // e.g PercentsCommand -> default name is Percents
        public virtual string Name => GetType().Name.Substring(0, 
            GetType().Name.LastIndexOf(nameof(Command)));

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
            Repo.Dispose();
            Keyboards.Dispose();
        }
    }
}
