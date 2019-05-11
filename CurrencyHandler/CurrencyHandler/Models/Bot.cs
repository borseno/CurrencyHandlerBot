using CurrencyHandler.Models.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using static System.String;

namespace CurrencyHandler.Models
{
    public static class Bot
    {
        private static TelegramBotClient client;
        private static List<Command> commandList;

        public static IReadOnlyList<Command> Commands => commandList.AsReadOnly();

        public static async Task<TelegramBotClient> GetAsync()
        {
            if (client != null)
                return client;

            commandList =
                new List<Command>
                {
                    InfoCommand.Instance,
                    CalcCommand.Instance,
                    PercentsCommand.Instance,
                    SettingsCommand.Instance,
                    StartCommand.Instance,
                    ValueCurrencyCommand.Instance,
                    DisplayCurrenciesCommand.Instance
                };

            var hook = Format(AppSettings.Url, "api/message/update");

            client = new TelegramBotClient(AppSettings.Key);

            await client.SetWebhookAsync(hook);

            return client;
        }
    }
}
