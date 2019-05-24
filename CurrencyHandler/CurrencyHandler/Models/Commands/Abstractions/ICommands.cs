using System;
using System.Collections.Generic;

namespace CurrencyHandler.Models.Commands.Abstractions
{
    public interface ICommands : IDisposable
    {
        IReadOnlyList<ICommand> Get();
    }
}