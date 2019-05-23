using System;
using System.Collections.Generic;

namespace CurrencyHandler.Models.Commands
{
    public class Commands : ICommands
    {
        private readonly List<ICommand> commands;

        public Commands(CalcCommand c1, DisplayCurrenciesCommand c2, ValueCurrencyCommand c3,
            StartCommand c4, SettingsCommand c5, PercentsCommand c6, InfoCommand c7)
        {
            commands = new List<ICommand>
                {
                    c1,
                    c2,
                    c3,
                    c4,
                    c5,
                    c6,
                    c7
                };
        }

        public IReadOnlyList<ICommand> Get()
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
