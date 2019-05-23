using System;
using System.Collections.Generic;

namespace CurrencyHandler.Models.Commands
{
    public interface ICommands : IDisposable
    {
        IReadOnlyList<ICommand> Get();
    }
}