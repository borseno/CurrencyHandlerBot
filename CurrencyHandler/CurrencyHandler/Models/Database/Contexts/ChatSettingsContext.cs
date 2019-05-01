﻿using CurrencyHandler.Models.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CurrencyHandler.Models.Database.Contexts
{
    public class ChatSettingsContext : DbContext
    {
        public DbSet<ChatSettings> ChatSettings { get; set; }
        public DbSet<CurrencyEmoji> CurrencyEmojis { get; set; }

        public ChatSettingsContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            mb.Entity<CurrencyEmoji>()
                .Property(e => e.Emoji)
                .IsRequired(true);

            mb.Entity<ChatSettings>()
                .Property(cs => cs.DisplayCurrencies)
                .IsRequired(true);

            mb.Entity<ChatSettings>()
                .Property(cs => cs.ValueCurrency)
                .IsRequired(true);

            mb.Entity<ChatSettings>()
                .Property(cs => cs.Percents)
                .IsRequired(true);

            mb.Entity<ChatSettings>()
                .Property(e => e.DisplayCurrencies)
                .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            mb.Entity<ChatSettings>()
                .HasKey(cs => cs.ChatId);

            mb.Entity<ChatSettings>()
                .Property(cs => cs.ChatId)
                .ValueGeneratedNever(); // disable increment

            mb.Entity<ChatSettings>()
                .Property(cs => cs.Percents)
                .HasDefaultValue(100);

            mb.Entity<CurrencyEmoji>()
                .HasKey(ce => ce.Currency);

            mb.Entity<CurrencyEmoji>().HasData(
                new CurrencyEmoji { Currency = "AUD", Emoji = "🇦🇺" },
                new CurrencyEmoji { Currency = "GBP", Emoji = "🇬🇧" },
                new CurrencyEmoji { Currency = "DKK", Emoji = "🇩🇰" },
                new CurrencyEmoji { Currency = "USD", Emoji = "🇺🇸" },
                new CurrencyEmoji { Currency = "EUR", Emoji = "🇪🇺" },
                new CurrencyEmoji { Currency = "BYN", Emoji = "🇧🇾" },
                new CurrencyEmoji { Currency = "KZT", Emoji = "🇰🇿" },
                new CurrencyEmoji { Currency = "CAD", Emoji = "🇩🇰" },
                new CurrencyEmoji { Currency = "NOK", Emoji = "🇳🇴" },
                new CurrencyEmoji { Currency = "SGD", Emoji = "🇸🇬" },
                new CurrencyEmoji { Currency = "TRY", Emoji = "🇹🇷" },
                new CurrencyEmoji { Currency = "UAH", Emoji = "🇺🇦" },
                new CurrencyEmoji { Currency = "SEK", Emoji = "🇸🇪" },
                new CurrencyEmoji { Currency = "CHF", Emoji = "🇨🇭" },
                new CurrencyEmoji { Currency = "JPY", Emoji = "🇯🇵" },
                new CurrencyEmoji { Currency = "RUB", Emoji = "🇷🇺" }
            );

            mb.Entity<ChatSettings>()
                .Property(cs => cs.ValueCurrency)
                .HasDefaultValue("UAH");

            mb.Entity<ChatSettings>()
                .Property(cs => cs.DisplayCurrencies)
                .HasDefaultValue(new string[] { "UAH", "RUB", "EUR", "USD", "BYN" });
        }

    }
}