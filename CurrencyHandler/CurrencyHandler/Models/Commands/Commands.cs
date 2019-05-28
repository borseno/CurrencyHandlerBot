using CurrencyHandler.Models.Commands.Abstractions;
using System.Collections.Generic;

namespace CurrencyHandler.Models.Commands
{
    public class Commands : ICommands
    {
        private readonly IEnumerable<ICommand> commands;

        public Commands(IEnumerable<ICommand> commands)
        {
            this.commands = commands;  
        }

        public IEnumerable<ICommand> Get()
        {
            return commands;
        }

        public void Dispose()
        {
            foreach (var i in commands)
                i.Dispose();
        }
    }
}
