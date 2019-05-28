using CurrencyHandler.Models.Database.Models;
using System.Collections.Generic;

namespace CurrencyHandler.Models.HelperClasses
{
    public static class DefaultValues
    {
        public const int DefaultPercents = 100;

        public const string DefaultValueCurrency = "UAH";

        public const string APICurrency = "RUB";

        public static List<string> DefaultDisplayCurrencies { get; } = new List<string>
        {
            "UAH",
            "RUB",
            "EUR",
            "USD",
            "BYN"
        };

        public static ChatSettings DefaultEntity => new ChatSettings
        {
            Percents = DefaultPercents,
            ValueCurrency = DefaultValueCurrency,
            DisplayCurrencies = DefaultDisplayCurrencies
        };
    }
}
