using System;
using System.Collections.Generic;

namespace CurrencyHandler.Models.InlineKeyboardHandlers
{
    public interface IKeyboards : IDisposable
    {
        IInlineKeyboardHandler FirstOrDefault(string name);
        IReadOnlyList<IInlineKeyboardHandler> Get();
    }
}