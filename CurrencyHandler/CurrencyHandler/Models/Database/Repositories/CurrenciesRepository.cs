using System;
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

        public async Task SetDisplayCurrenciesAsync(string[] displayCurrencies, long chatId)
        {
            var chat = await Context.ChatSettings.FirstAsync(cs => cs.ChatId == chatId);

            chat.DisplayCurrencies = displayCurrencies;

            await Context.SaveChangesAsync();
        }

        public async Task AddDisplayCurrenciesAsync(string[] displayCurrencies, long chatId)
        {
            var chat = await Context.ChatSettings.FirstAsync(cs => cs.ChatId == chatId);

            chat.DisplayCurrencies = chat.DisplayCurrencies.Concat(displayCurrencies).ToArray();

            await Context.SaveChangesAsync();
        }

        public async Task<string[]> GetDisplayCurrenciesAsync(long chatId)
        {
            return (await Context.ChatSettings.FirstAsync(cs => cs.ChatId == chatId)).DisplayCurrencies;
        }

        public async Task<CurrencyEmoji[]> GetDisplayCurrenciesEmojisAsync(long chatId)
        {
            var displayCurrencies = (await Context.ChatSettings.FirstAsync(cs => cs.ChatId == chatId)).DisplayCurrencies;
            var currenciesEmojis = Context.CurrencyEmojis;

            return await currenciesEmojis.Where(i => i.Currency.In(displayCurrencies)).ToArrayAsync();
        }

        public async Task<CurrencyEmoji> GetCurrencyEmojiAsync(long chatId)
        {
            return await GetCurrencyEmojiFromCurrencyAsync(await GetCurrencyAsync(chatId));
        }

        public string[] GetAllCurrencies()
        {
            return CurrenciesEmojisRepository.GetCurrencies();
        }

        public string[] GetAllEmojis()
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
