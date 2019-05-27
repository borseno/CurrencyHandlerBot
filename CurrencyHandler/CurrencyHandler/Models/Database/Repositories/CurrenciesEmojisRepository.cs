using System;
using CurrencyHandler.Models.Database.Contexts;
using CurrencyHandler.Models.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyHandler.Models.Database.Repositories
{
    public class CurrenciesEmojisRepository : ICurrenciesEmojisRepository
    {
        protected ChatSettingsContext Context { get; }

        public CurrenciesEmojisRepository(ChatSettingsContext ctx)
        {
            Context = ctx;
        }

        public IEnumerable<CurrencyEmoji> GetCurrencyEmojis()
        {
            return Context.CurrencyEmojis.AsNoTracking();
        }

        public IEnumerable<string> GetCurrencies()
        {
            return Context.CurrencyEmojis
                .Select(ce => ce.Currency)
                .AsNoTracking();
        }

        public IEnumerable<string> GetEmojis()
        {
            return Context.CurrencyEmojis
                .Select(ce => ce.Emoji)
                .AsNoTracking();
        }

        public async Task<CurrencyEmoji[]> GetCurrencyEmojisAsync()
        {
            return await Context.CurrencyEmojis
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<string[]> GetCurrenciesAsync()
        {
            return await Context.CurrencyEmojis
                .Select(ce => ce.Currency)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<string[]> GetEmojisAsync()
        {
            return await Context.CurrencyEmojis
                .Select(ce => ce.Emoji)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<string> GetCurrencyFromEmojiAsync(string emoji)
        {
            return (await Context.CurrencyEmojis
                .AsNoTracking()
                .FirstAsync(ce => ce.Emoji == emoji)
                ).Currency;
        }

        public async Task<string> GetEmojiFromCurrencyAsync(string currency)
        {
            return (await Context.CurrencyEmojis
                .AsNoTracking()
                .FirstAsync(ce => ce.Currency == currency)
                ).Emoji;
        }

        public async Task<CurrencyEmoji> GetCurrencyEmojiFromCurrencyAsync(string currency)
        {
            return await Context.CurrencyEmojis
                .AsNoTracking()
                .FirstAsync(ce => ce.Currency == currency);
        }

        public async Task<CurrencyEmoji> GetCurrencyEmojiFromEmojiAsync(string emoji)
        {
            return await Context.CurrencyEmojis
                .AsNoTracking()
                .FirstAsync(ce => ce.Emoji == emoji);
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
