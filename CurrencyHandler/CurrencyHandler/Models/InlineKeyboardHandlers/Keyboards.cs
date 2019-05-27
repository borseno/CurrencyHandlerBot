using CurrencyHandler.Models.InlineKeyboardHandlers.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyHandler.Models.InlineKeyboardHandlers
{
    public class Keyboards : IKeyboards
    {
        private readonly IEnumerable<IInlineKeyboardHandler> keyboards;

        public Keyboards(IEnumerable<IInlineKeyboardHandler> kb)
        {
            keyboards = kb;
        }

        public IEnumerable<IInlineKeyboardHandler> Get()
        {
            return keyboards;
        }

        public IInlineKeyboardHandler FirstOrDefault(string name)
        {
            return Get().FirstOrDefault(kb => String.Equals(kb.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        public void Dispose()
        {
            foreach (var i in keyboards)
                i.Dispose();
        }
    }
}
