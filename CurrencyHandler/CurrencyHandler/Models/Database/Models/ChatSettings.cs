namespace CurrencyHandler.Models.DbModels.Models
{
    public class ChatSettings
    {
        public int ChatSettingsId { get; set; }

        public long ChatId { get; set; } 
        public decimal Percents { get; set; }
        public string Currency { get; set; }
    }
}
