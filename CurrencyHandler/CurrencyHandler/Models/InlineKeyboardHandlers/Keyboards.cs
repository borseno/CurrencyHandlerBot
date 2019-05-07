using CurrencyHandler.Models.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyHandler.Models.InlineKeyboardHandlers
{
    public static class Keyboards
    {
        public static IReadOnlyList<InlineKeyboardHandler> Get(CurrenciesRepository repo)
        {
            var keyboards = new List<InlineKeyboardHandler>
                {
                    new ValueCurrencyKeyboardHandler(repo)
                };

            return keyboards;
        }

        public static InlineKeyboardHandler FirstOrDefault(CurrenciesRepository repo, string name)
        {
            return Get(repo).FirstOrDefault(kb => String.Equals(kb.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
