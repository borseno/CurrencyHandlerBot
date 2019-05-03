using CurrencyHandler.Models.Database.Contexts;
using CurrencyHandler.Models.Database.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<CurrencyEmoji[]> GetCurrencyEmojisAsync()
        {
            return await Context.CurrencyEmojis.ToArrayAsync();
        }

        public async Task<string[]> GetCurrenciesAsync()
        {
            return await Context.CurrencyEmojis.Select(ce => ce.Currency).ToArrayAsync();
        }

        public async Task<string[]> GetEmojiesAsync()
        {
            return await Context.CurrencyEmojis.Select(ce => ce.Emoji).ToArrayAsync();
        }
    }
}
