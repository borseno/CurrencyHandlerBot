using CurrencyHandler.Models.Commands.Abstractions;
using System;
using System.Collections.Generic;

namespace CurrencyHandler.Models.Commands
{
    public class Commands : ICommands
    {
        private readonly List<ICommand> commands;

        public Commands(ICalcCommand c1, IDisplayCurrenciesCommand c2, IValueCurrencyCommand c3,
            IStartCommand c4, ISettingsCommand c5, IPercentsCommand c6, IInfoCommand c7)
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
