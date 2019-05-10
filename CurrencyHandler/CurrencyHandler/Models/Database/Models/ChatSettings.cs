using System.Collections.Generic;

namespace CurrencyHandler.Models.Database.Models
{
    public class ChatSettings
    {
        public long ChatId { get; set; }

        public decimal Percents { get; set; }

        public string ValueCurrency { get; set; }

        public List<string> DisplayCurrencies { get; set; }
    }
}
