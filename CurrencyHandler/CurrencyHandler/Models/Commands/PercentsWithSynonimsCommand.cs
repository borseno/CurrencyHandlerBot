using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyHandler.Models.Commands.Abstractions;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.InlineKeyboardHandlers.Abstractions;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    public class PercentsWithSynonymsCommand : Command
    {
        private readonly ICommand mainCommand;

        public PercentsWithSynonymsCommand(IKeyboards keyboards, ICurrenciesRepository repo) : base(keyboards, repo)
        {
            mainCommand = new PercentsCommand(keyboards, repo);
        }

        public override string Name => mainCommand.Name;

        public override Task Execute(Message message)
        {
            return mainCommand.Execute(message);
        }
            
        public override bool Contains(string command)
        {
            const char Space = ' ';
            const char CommandIdentifier = '/';

            var result = mainCommand.Contains(command);

            string pureCommand;

            try
            {
                pureCommand = command
                    .TrimStart(CommandIdentifier)
                    .Substring(0, command.TrimStart(CommandIdentifier).IndexOf(Space));
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            return result ? result : mainCommand
                .Name
                .StartsWith(pureCommand, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
