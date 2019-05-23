using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyHandler.Models.InlineKeyboardHandlers
{
    public class Keyboards : IDisposable
    {
        private readonly List<InlineKeyboardHandler> keyboards;

        public Keyboards(ValueCurrencyKeyboardHandler kb1, DisplayCurrenciesKeyboardHandler kb2)
        {
            keyboards = new List<InlineKeyboardHandler>
                {
                    kb1,
                    kb2
                };
        }

        public IReadOnlyList<InlineKeyboardHandler> Get()
        {
            return keyboards;
        }

        public InlineKeyboardHandler FirstOrDefault(string name)
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
