using CurrencyHandler.Models.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
