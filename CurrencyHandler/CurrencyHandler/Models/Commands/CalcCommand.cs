using System;
using System.Linq;
using System.Threading.Tasks;
using CurrencyHandler.Models.DataCaching;
using CurrencyHandler.Models.WorkerClasses;
using Telegram.Bot;
using Telegram.Bot.Types;
using CurrencyHandler.Models.Database.Repositories;

namespace CurrencyHandler.Models.Commands
{
    public class CalcCommand : Command
    {
        private readonly string nl = Environment.NewLine;

        private CalcCommand()
        {

        }

        public static CalcCommand Instance { get; } = new CalcCommand();

        public override string Name => "Calc";

        public override async Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo)
        {
            var messageText = message?.Text;

            if (messageText == null)
                return;

            var messageId = message.MessageId;
            var chatSettingsId = message.Chat.Id;
            var content =
                messageText
                    .TrimStart(
                        $"/{Name.ToLower()}".ToCharArray()
                    .Concat($"/{Name.ToUpper()}".ToCharArray())
                    .ToArray()
                        ).Replace(',', '.');

            if (decimal.TryParse(content, out var value))
            {
                var data = await CurrenciesDataCaching.GetValCurs();
                var percents = await repo.GetPercentsAsync(chatSettingsId);
                var currency = await repo.GetCurrencyAsync(chatSettingsId);
                var neededCurrencies = await repo.GetDisplayCurrenciesEmojisAsync(chatSettingsId);

                var values =
                    await ValuesCalculator.GetCurrenciesValuesAsync(value, currency, data, neededCurrencies);

                var textToSend = await AnswerBuilder.BuildStringFromValuesAsync(values, currency, percents);

                await client.SendTextMessageAsync(chatSettingsId, textToSend, replyToMessageId: messageId);
            }
            else
                throw new Exception("Problems occured when parsing your message");
        }

        public override bool Contains(string command)
        {
            if (command == null)
                return false;

            return base.Contains(command) || decimal.TryParse(command, out _);
        }
    }
}
