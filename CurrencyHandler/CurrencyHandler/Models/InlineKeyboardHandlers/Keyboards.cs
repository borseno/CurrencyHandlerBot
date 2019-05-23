using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyHandler.Models.InlineKeyboardHandlers
{
    public class Keyboards : IKeyboards
    {
        private readonly List<IInlineKeyboardHandler> keyboards;

        public Keyboards(ValueCurrencyKeyboardHandler kb1, DisplayCurrenciesKeyboardHandler kb2)
        {
            keyboards = new List<IInlineKeyboardHandler>
                {
                    kb1,
                    kb2
                };
        }

        public IReadOnlyList<IInlineKeyboardHandler> Get()
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
