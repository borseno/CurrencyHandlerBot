using CurrencyHandler.Models.Database.Contexts;
using CurrencyHandler.Models.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyHandler.Models.Database.Repositories
{
    public class CurrenciesEmojisRepository
    {
        protected ChatSettingsContext Context { get; }

        public CurrenciesEmojisRepository(ChatSettingsContext ctx)
        {
            Context = ctx;
        }

        public CurrencyEmoji[] GetCurrencyEmojis()
        {
            return Context.CurrencyEmojis.ToArray();
        }

        public string[] GetCurrencies()
        {
            return Context.CurrencyEmojis.Select(ce => ce.Currency).ToArray();
        }

        public string[] GetEmojies()
        {
            return Context.CurrencyEmojis.Select(ce => ce.Emoji).ToArray();
        }
    }
}
