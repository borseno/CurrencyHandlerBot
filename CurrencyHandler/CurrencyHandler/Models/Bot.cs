using Telegram.Bot;
using static System.String;

namespace CurrencyHandler.Models
{
    public static class Bot
    {
        private static readonly TelegramBotClient client;

        static Bot()
        {
            var hook = Format(AppSettings.Url, "api/message/update");

            client = new TelegramBotClient(AppSettings.Key);

            client.SetWebhookAsync(hook).GetAwaiter().GetResult();
        }

        public static void Init()
        {
            // static ctor has been called..
        }

        public static TelegramBotClient GetClient()
        {
            return client;
        }
    }
}
