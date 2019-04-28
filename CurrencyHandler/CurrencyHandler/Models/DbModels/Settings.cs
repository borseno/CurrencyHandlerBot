using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CurrencyHandler.Models.DbModels
{
    public static class Settings
    {
        private const decimal DefaultPercents = 100;
        private const string DefaultCurrency = "UAH";

        public static async Task<decimal> GetPercentsForChat(long chatId, ChatSettingsContext context)
        {
            var chat = await context.ChatSettings.FirstOrDefaultAsync(t => t.ChatId == chatId);

            if (chat == null)
            {
                await InitChat(chatId, context);
                return DefaultPercents;
            }

            return chat.Percents;
        }

        public static async Task SetPercentsForChat(long chatId, decimal value, ChatSettingsContext context)
        {
            var chat = await context.ChatSettings.FirstOrDefaultAsync(t => t.ChatId == chatId);

            if (chat == null)
            {
                await context.ChatSettings.AddAsync(
                    new ChatSettings
                    {
                        ChatId = chatId,
                        Percents = value,
                        Currency = DefaultCurrency
                    });
            }
            else
            {
                chat.Percents = value;
            }

            await context.SaveChangesAsync();
        }

        public static async Task<string> GetCurrencyForChat(long chatId, ChatSettingsContext ctx)
        {
            var chat = await ctx.ChatSettings.FirstOrDefaultAsync(t => t.ChatId == chatId);

            if (chat == null)
            {
                await InitChat(chatId, ctx);
                return DefaultCurrency;
            }

            return chat.Currency;
        }

        public static async Task SetCurrencyForChat(long chatId, string currency, ChatSettingsContext context)
        {
            var chat = await context.ChatSettings.FirstOrDefaultAsync(t => t.ChatId == chatId);

            if (chat == null)
            {
                await context.ChatSettings.AddAsync(
                    new ChatSettings
                    {
                        ChatId = chatId,
                        Percents = DefaultPercents,
                        Currency = currency
                    });
            }
            else
            {
                chat.Currency = currency;
            }

            await context.SaveChangesAsync();
        }

        private static async Task InitChat(long chatId, ChatSettingsContext context)
        {
            if (!await context.ChatSettings.AnyAsync(t => t.ChatId == chatId))
            {
                await context.ChatSettings.AddAsync(
                    new ChatSettings
                    {
                        ChatId = chatId,
                        Percents = DefaultPercents,
                        Currency = DefaultCurrency
                    });

                await context.SaveChangesAsync();
            }
            else
                throw new InvalidOperationException("settings for this chat id already exist!");
        }
    }
}
