using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyHandler.Models.DataCaching;
using CurrencyHandler.Models.WorkerClasses;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Diagnostics;
using CurrencyHandler.Models.DbModels;

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

        public override async Task Execute(Message message, TelegramBotClient client, DbContext db)
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

                var watch = Stopwatch.StartNew();

                var percents = await Settings.GetPercentsForChat(chatSettingsId, (ChatSettingsContext)db);
                var currency = await Settings.GetCurrencyForChat(chatSettingsId, (ChatSettingsContext)db);

                watch.Stop();

                var values =
                    (await ValuesCalculator.GetValuesInRubAndOtherCurrencies(value, currency, data))
                    .ToDictionary(t => t.Key, t => t.Value);

                var textToSend = await AnswerBuilder.BuildStringFromValuesAsync(values, percents);

                await client.SendTextMessageAsync(chatSettingsId, textToSend, replyToMessageId: messageId);

                await client.SendTextMessageAsync(chatSettingsId, $"Elapsed: {watch.Elapsed}{nl}" +
                    $"ElapsedTicks: {watch.ElapsedTicks}{nl}" +
                    $"ElapsedMs: {watch.ElapsedMilliseconds}");
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
