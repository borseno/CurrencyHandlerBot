﻿// <auto-generated />
using CurrencyHandler.Models.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CurrencyHandler.Migrations
{
    [DbContext(typeof(ChatSettingsContext))]
    [Migration("20190510134810_ChangedArrayToList")]
    partial class ChangedArrayToList
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("CurrencyHandler.Models.Database.Models.ChatSettings", b =>
                {
                    b.Property<long>("ChatId");

                    b.Property<string>("DisplayCurrencies")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("UAH,RUB,EUR,USD,BYN");

                    b.Property<decimal>("Percents")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(100m);

                    b.Property<string>("ValueCurrency")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("UAH");

                    b.HasKey("ChatId");

                    b.ToTable("ChatSettings");
                });

            modelBuilder.Entity("CurrencyHandler.Models.Database.Models.CurrencyEmoji", b =>
                {
                    b.Property<string>("Currency")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Emoji")
                        .IsRequired();

                    b.HasKey("Currency");

                    b.ToTable("CurrencyEmojis");

                    b.HasData(
                        new
                        {
                            Currency = "AUD",
                            Emoji = "🇦🇺"
                        },
                        new
                        {
                            Currency = "GBP",
                            Emoji = "🇬🇧"
                        },
                        new
                        {
                            Currency = "DKK",
                            Emoji = "🇩🇰"
                        },
                        new
                        {
                            Currency = "USD",
                            Emoji = "🇺🇸"
                        },
                        new
                        {
                            Currency = "EUR",
                            Emoji = "🇪🇺"
                        },
                        new
                        {
                            Currency = "BYN",
                            Emoji = "🇧🇾"
                        },
                        new
                        {
                            Currency = "KZT",
                            Emoji = "🇰🇿"
                        },
                        new
                        {
                            Currency = "CAD",
                            Emoji = "🇨🇦"
                        },
                        new
                        {
                            Currency = "NOK",
                            Emoji = "🇳🇴"
                        },
                        new
                        {
                            Currency = "SGD",
                            Emoji = "🇸🇬"
                        },
                        new
                        {
                            Currency = "TRY",
                            Emoji = "🇹🇷"
                        },
                        new
                        {
                            Currency = "UAH",
                            Emoji = "🇺🇦"
                        },
                        new
                        {
                            Currency = "SEK",
                            Emoji = "🇸🇪"
                        },
                        new
                        {
                            Currency = "CHF",
                            Emoji = "🇨🇭"
                        },
                        new
                        {
                            Currency = "JPY",
                            Emoji = "🇯🇵"
                        },
                        new
                        {
                            Currency = "RUB",
                            Emoji = "🇷🇺"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}