using CurrencyHandler.Models.InlineKeyboardHandlers.Abstractions;
using System;
using System.Collections.Generic;

namespace CurrencyHandler.Models.InlineKeyboardHandlers.Abstractions
{
    public interface IKeyboards : IDisposable
    {
        IInlineKeyboardHandler FirstOrDefault(string name);
        IEnumerable<IInlineKeyboardHandler> Get();
    }
}