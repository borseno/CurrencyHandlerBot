using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Contexts;
using CurrencyHandler.Models.Database.Models;
using CurrencyHandler.Models.Extensions;
using CurrencyHandler.Models.HelperClasses;
using Microsoft.EntityFrameworkCore;

namespace CurrencyHandler.Models.Database.Repositories
{
    public class CurrenciesRepository : IDisposable
    {
        protected ChatSettingsContext Context { get; }

        protected CurrenciesEmojisRepository CurrenciesEmojisRepository { get; }

        public CurrenciesRepository(ChatSettingsContext ctx)
        {
            Context = ctx;
            CurrenciesEmojisRepository = new CurrenciesEmojisRepository(ctx);
        }

        protected ChatSettings AddChat(ChatSettings entity)
        {
            Context.ChatSettings.Add(entity);

            Context.SaveChanges();

            return entity;
        }

        protected ChatSettings InitChat(long chatId)
        {
            var entity = new ChatSettings
            {
                ChatId = chatId,
                Percents = DefaultValues.DefaultPercents,
                ValueCurrency = DefaultValues.DefaultValueCurrency,
                DisplayCurrencies = DefaultValues.DefaultDisplayCurrencies
            };

            Context.ChatSettings.Add(entity);

            Context.SaveChanges();

            return entity;
        }

        protected async Task<ChatSettings> AddChatAsync(ChatSettings entity)
        {
            await Context.ChatSettings.AddAsync(entity);

            await Context.SaveChangesAsync();

            return entity;
        }

        protected async Task<ChatSettings> InitChatAsync(long chatId)
        {
            var entity = new ChatSettings
            {
                ChatId = chatId,
                Percents = DefaultValues.DefaultPercents,
                ValueCurrency = DefaultValues.DefaultValueCurrency,
                DisplayCurrencies = DefaultValues.DefaultDisplayCurrencies
            };

            await Context.ChatSettings.AddAsync(entity);

            await Context.SaveChangesAsync();

            return entity;
        }

        // TODO: Optimize
        internal async Task RemoveDisplayEmojiesAsync(string emoji, long chatID)
        {
            var chat = Context.ChatSettings.FirstOrDefault(t => t.ChatId == chatID);

            if (chat == null)
            {
                chat = await InitChatAsync(chatID);
            }

            var currency = (await Context.CurrencyEmojis.FirstAsync(i => i.Emoji == emoji)).Currency;

            var isRemoved =  chat.DisplayCurrencies.Remove(currency);

            if (!isRemoved)
                throw new Exception(
                    $"Attempt to remove {currency} from displayCurrencies. " +
                    $"{currency} is already not in the list");

            Context.Entry(chat).State = EntityState.Modified;

            Context.SaveChanges();
        }

        // TODO: Optimize
        internal async Task AddDisplayEmojiesAsync(string emoji, long chatID)
        {
            var chat = Context.ChatSettings.FirstOrDefault(t => t.ChatId == chatID);

            if (chat == null)
            {
                chat = await InitChatAsync(chatID);
            }

            var currency = (await Context.CurrencyEmojis.FirstAsync(i => i.Emoji == emoji)).Currency;

            chat.DisplayCurrencies.Add(currency); // don't you EF see it yourself that it's changed?!>!>!>!>?!??

            Context.Entry(chat).State = EntityState.Modified; // not having this line costed me 2 hours

            Context.SaveChanges();
        }

        internal async Task<string[]> GetDisplayEmojisAsync(long chatId)
        {
            var chat = await Context.ChatSettings.FirstOrDefaultAsync(cs => cs.ChatId == chatId);

            if (chat == null)
            {
                chat = await InitChatAsync(chatId);
            }

            var displayCurrencies = chat.DisplayCurrencies;
            var currenciesEmojis = Context.CurrencyEmojis;

            return await currenciesEmojis.Where(i => i.Currency.In(displayCurrencies)).Select(ce => ce.Emoji).ToArrayAsync();
        }

        public string GetCurrency(long chatId)
        {
            var chat = Context.ChatSettings.FirstOrDefault(t => t.ChatId == chatId);

            if (chat == null)
                return InitChat(chatId).ValueCurrency;

            return chat.ValueCurrency;
        }

        public decimal GetPercents(long chatId)
        {
            var chat = Context.ChatSettings.FirstOrDefault(t => t.ChatId == chatId);

            if (chat == null)
                return InitChat(chatId).Percents;

            return chat.Percents;
        }

        public void SetCurrency(string value, long chatId)
        {
            var chat = Context.ChatSettings.FirstOrDefault(t => t.ChatId == chatId);

            if (chat == null)
            {
                chat = new ChatSettings
                {
                    ChatId = chatId,
                    ValueCurrency = value
                };

                AddChat(chat);
            }
            else
            {
                chat.ValueCurrency = value;

                Context.SaveChanges();
            }
        }

        public void SetPercents(decimal value, long chatId)
        {
            var chat = Context.ChatSettings.FirstOrDefault(t => t.ChatId == chatId);

            if (chat == null)
            {
                chat = new ChatSettings
                {
                    ChatId = chatId,
                    Percents = value
                };

                AddChat(chat);
            }
            else
            {
                chat.Percents = value;

                Context.SaveChanges();
            }
        }

        public async Task SetCurrencyAsync(string value, long chatId)
        {
            var chat = await Context.ChatSettings.FirstOrDefaultAsync(t => t.ChatId == chatId);

            if (chat == null)
            {
                chat = new ChatSettings
                {
                    ChatId = chatId,
                    ValueCurrency = value
                };

                await AddChatAsync(chat);
            }
            else
            {
                chat.ValueCurrency = value;

                await Context.SaveChangesAsync();
            }
        }

        public async Task SetPercentsAsync(decimal value, long chatId)
        {
            var chat = await Context.ChatSettings.FirstOrDefaultAsync(t => t.ChatId == chatId);

            if (chat == null)
            {
                chat = new ChatSettings
                {
                    ChatId = chatId,
                    Percents = value
                };

                await AddChatAsync(chat);
            }
            else
            {
                chat.Percents = value;

                await Context.SaveChangesAsync();
            }
        }

        public async Task<string> GetCurrencyAsync(long chatId)
        {
            var chat = await Context.ChatSettings.FirstOrDefaultAsync(t => t.ChatId == chatId);

            if (chat == null)
                return (await InitChatAsync(chatId)).ValueCurrency;

            return chat.ValueCurrency;
        }

        public async Task<decimal> GetPercentsAsync(long chatId)
        {
            var chat = await Context.ChatSettings.FirstOrDefaultAsync(t => t.ChatId == chatId);

            if (chat == null)
                return (await InitChatAsync(chatId)).Percents;

            return chat.Percents;
        }

        public string GetCurrencyFromEmoji(string emoji)
        {
            return GetCurrencyEmojiFromEmoji(emoji).Currency;
        }

        public string GetEmojiFromCurrency(string currency)
        {
            return GetCurrencyEmojiFromCurrency(currency).Emoji;
        }

        public CurrencyEmoji GetCurrencyEmojiFromCurrency(string currency)
        {
            return Context.CurrencyEmojis.FirstOrDefault(ce => ce.Currency == currency);
        }

        public CurrencyEmoji GetCurrencyEmojiFromEmoji(string emoji)
        {
            return Context.CurrencyEmojis.FirstOrDefault(ce => ce.Emoji == emoji);
        }

        public CurrencyEmoji GetCurrencyEmoji(long chatId)
        {
            return GetCurrencyEmojiFromCurrency(GetCurrency(chatId));
        }

        public async Task<CurrencyEmoji> GetCurrencyEmojiFromCurrencyAsync(string currency)
        {
            return await Context.CurrencyEmojis.FirstOrDefaultAsync(ce => ce.Currency == currency);
        }

        public async Task<CurrencyEmoji> GetCurrencyEmojiFromEmojiAsync(string emoji)
        {
            return await Context.CurrencyEmojis.FirstOrDefaultAsync(ce => ce.Emoji == emoji);
        }

        public async Task<string> GetCurrencyFromEmojiAsync(string emoji)
        {
            return (await GetCurrencyEmojiFromEmojiAsync(emoji)).Currency;
        }

        public async Task<string> GetEmojiFromCurrencyAsync(string currency)
        {
            return (await GetCurrencyEmojiFromCurrencyAsync(currency)).Emoji;
        }

        public async Task SetDisplayCurrenciesAsync(IEnumerable<string> displayCurrencies, long chatId)
        {
            var chat = await Context.ChatSettings.FirstAsync(cs => cs.ChatId == chatId);

            chat.DisplayCurrencies = displayCurrencies.ToList();

            await Context.SaveChangesAsync();
        }

        public async Task AddRangeDisplayCurrenciesAsync(IEnumerable<string> displayCurrencies, long chatId)
        {
            var chat = await Context.ChatSettings.FirstAsync(cs => cs.ChatId == chatId);

            chat.DisplayCurrencies.AddRange(displayCurrencies);

            await Context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<string>> GetDisplayCurrenciesAsync(long chatId)
        {
            return (await Context.ChatSettings.FirstAsync(cs => cs.ChatId == chatId)).DisplayCurrencies.AsReadOnly();
        }

        public async Task<IReadOnlyList<CurrencyEmoji>> GetDisplayCurrenciesEmojisAsync(long chatId)
        {
            var displayCurrencies = (await Context.ChatSettings.FirstAsync(cs => cs.ChatId == chatId)).DisplayCurrencies;
            var currenciesEmojis = Context.CurrencyEmojis;

            return await currenciesEmojis.Where(i => i.Currency.In(displayCurrencies)).ToArrayAsync();
        }

        public async Task<CurrencyEmoji> GetCurrencyEmojiAsync(long chatId)
        {
            return await GetCurrencyEmojiFromCurrencyAsync(await GetCurrencyAsync(chatId));
        }

        public IEnumerable<string> GetAllCurrencies()
        {
            return CurrenciesEmojisRepository.GetCurrencies();
        }

        public IEnumerable<string> GetAllEmojis()
        {
            return CurrenciesEmojisRepository.GetEmojies();
        }

        public async Task<string[]> GetAllCurrenciesAsync()
        {
            return await CurrenciesEmojisRepository.GetCurrenciesAsync();
        }

        public async Task<string[]> GetAllEmojisAsync()
        {
            return await CurrenciesEmojisRepository.GetEmojiesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
