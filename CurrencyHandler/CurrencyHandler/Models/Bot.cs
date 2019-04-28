using CurrencyHandler.Models.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace CurrencyHandler.Models
{
    public static class Bot
    {
        private static TelegramBotClient _client;
        private static List<Command> _commandList;

        public static IReadOnlyList<Command> Commands => _commandList.AsReadOnly();

        public static async Task<TelegramBotClient> Get()
        {
            if (_client != null)
                return _client;

            _commandList =
                new List<Command>
                {
                    InfoCommand.Instance,
                    CalcCommand.Instance,
                    PercentsCommand.Instance,
                    SettingsCommand.Instance,
                    StartCommand.Instance,
                    CurrencyCommand.Instance
                };

            var hook = String.Format(AppSettings.Url, "api/message/update");

            _client = new TelegramBotClient(AppSettings.Key);

            await _client.SetWebhookAsync(hook);

            return _client;
        }
    }
}
