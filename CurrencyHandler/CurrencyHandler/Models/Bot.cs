using CurrencyHandler.Models.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace CurrencyHandler.Models
{
    public static class Bot
    {
        private static TelegramBotClient client;
        private static List<Command> commandList;

        public static IReadOnlyList<Command> Commands => commandList.AsReadOnly();

        public static async Task<TelegramBotClient> Get()
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
                    ValueCurrencyCommand.Instance
                };

            var hook = String.Format(AppSettings.Url, "api/message/update");

            client = new TelegramBotClient(AppSettings.Key);

            await client.SetWebhookAsync(hook);

            return client;
        }
    }
}
