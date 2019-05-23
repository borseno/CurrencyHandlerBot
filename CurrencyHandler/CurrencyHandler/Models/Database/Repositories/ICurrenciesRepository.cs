using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Models;

namespace CurrencyHandler.Models.Database.Repositories
{
    public partial interface ICurrenciesRepository /* async methods */: IDisposable
    {
        Task AddRangeDisplayCurrenciesAsync(IEnumerable<string> displayCurrencies, long chatId);
        Task<string[]> GetAllCurrenciesAsync();
        Task<string[]> GetAllEmojisAsync();
        Task<string> GetCurrencyAsync(long chatId);
        Task<CurrencyEmoji> GetCurrencyEmojiAsync(long chatId);
        Task<CurrencyEmoji> GetCurrencyEmojiFromCurrencyAsync(string currency);
        Task<CurrencyEmoji> GetCurrencyEmojiFromEmojiAsync(string emoji);
        Task<string> GetCurrencyFromEmojiAsync(string emoji);
        Task<IReadOnlyList<string>> GetDisplayCurrenciesAsync(long chatId);
        Task<IReadOnlyList<CurrencyEmoji>> GetDisplayCurrenciesEmojisAsync(long chatId);
        Task<string> GetEmojiFromCurrencyAsync(string currency);
        Task<decimal> GetPercentsAsync(long chatId);
        Task SetCurrencyAsync(string value, long chatId);
        Task SetDisplayCurrenciesAsync(IEnumerable<string> displayCurrencies, long chatId);
        Task SetPercentsAsync(decimal value, long chatId);
        Task<string[]> GetDisplayEmojisAsync(long chatId);
        Task AddDisplayEmojisAsync(string emoji, long chatID);
        Task RemoveDisplayEmojisAsync(string emoji, long chatID);
    }
    
    public partial interface ICurrenciesRepository /* sync methods */
    {
        IEnumerable<string> GetAllCurrencies();
        IEnumerable<string> GetAllEmojis();
        string GetCurrency(long chatId);
        CurrencyEmoji GetCurrencyEmoji(long chatId);
        CurrencyEmoji GetCurrencyEmojiFromCurrency(string currency);
        CurrencyEmoji GetCurrencyEmojiFromEmoji(string emoji);
        string GetCurrencyFromEmoji(string emoji);
        string GetEmojiFromCurrency(string currency);
        decimal GetPercents(long chatId);
        void SetCurrency(string value, long chatId);
        void SetPercents(decimal value, long chatId);
    }
}