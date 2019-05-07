using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyHandler.Models.HelperClasses
{
    public static class DefaultValues
    {
        public const int DefaultPercents = 100;

        public const string DefaultValueCurrency = "UAH";

        public const string APICurrency = "RUB";

        public static readonly string[] DefaultDisplayCurrencies =
        {
            "UAH",
            "RUB",
            "EUR",
            "USD",
            "BYN"
        };
    }
}
