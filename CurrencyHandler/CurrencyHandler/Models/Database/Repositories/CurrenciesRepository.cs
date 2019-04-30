using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Contexts;
using CurrencyHandler.Models.DbModels.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyHandler.Models.Database.Repositories
{
    public class CurrenciesRepository : IDisposable
    {
        protected const decimal DefaultPercents = 100;
        protected const string DefaultCurrency = "UAH";

        protected ChatSettingsContext Context { get; }

        public CurrenciesRepository(ChatSettingsContext ctx)
        {
            Context = ctx;
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
                Percents = DefaultPercents,
                Currency = DefaultCurrency
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
                Percents = DefaultPercents,
                Currency = DefaultCurrency
            };

            await Context.ChatSettings.AddAsync(entity);

            await Context.SaveChangesAsync();

            return entity;
        }

        public string GetCurrency(long chatId)
        {
            var chat = Context.ChatSettings.FirstOrDefault(t => t.ChatId == chatId);

            if (chat == null)
                return InitChat(chatId).Currency;

            return chat.Currency;
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
                    ChatId = chatId, Percents = DefaultPercents, Currency = value
                };

                AddChat(chat);
            }
            else
            {
                chat.Currency = value;

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
                    ChatId = chatId, Currency = DefaultCurrency, Percents = value
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
                    Percents = DefaultPercents,
                    Currency = value
                };

                await AddChatAsync(chat);
            }
            else
            {
                chat.Currency = value;

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
                    Percents = value,
                    Currency = DefaultCurrency
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
                return (await InitChatAsync(chatId)).Currency;

            return chat.Currency;
        }

        public async Task<decimal> GetPercentsAsync(long chatId)
        {
            var chat = await Context.ChatSettings.FirstOrDefaultAsync(t => t.ChatId == chatId);

            if (chat == null)
                return (await InitChatAsync(chatId)).Percents;

            return chat.Percents;
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
