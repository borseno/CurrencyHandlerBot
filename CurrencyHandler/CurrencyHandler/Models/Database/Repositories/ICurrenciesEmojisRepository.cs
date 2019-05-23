using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Models;

namespace CurrencyHandler.Models.Database.Repositories
{
    public partial interface ICurrenciesEmojisRepository /* async */ : IDisposable
    {
        Task<string[]> GetCurrenciesAsync();
        Task<CurrencyEmoji> GetCurrencyEmojiFromCurrencyAsync(string currency);
        Task<CurrencyEmoji> GetCurrencyEmojiFromEmojiAsync(string emoji);
        Task<CurrencyEmoji[]> GetCurrencyEmojisAsync();
        Task<string> GetCurrencyFromEmojiAsync(string emoji);
        Task<string> GetEmojiFromCurrencyAsync(string currency);
        Task<string[]> GetEmojisAsync();
    }

    public partial interface ICurrenciesEmojisRepository /* sync */
    {
        IEnumerable<string> GetEmojis();
        IEnumerable<CurrencyEmoji> GetCurrencyEmojis();
        IEnumerable<string> GetCurrencies();
    }
}