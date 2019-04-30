using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Contexts;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.DbModels;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    public class PercentsCommand : Command
    {
        private PercentsCommand()
        {

        }

        public static PercentsCommand Instance { get; } = new PercentsCommand();

        public override string Name => "Percents";

        public override async Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo)
        {
            var messageId = message.MessageId;
            var chatSettingsId = message.Chat.Id;
            var messageText = message.Text;

            const string pattern = @"\/percents \-{0,1}\d+(.\d+){0,1}";
            const string firstNums = "123456789";
            const string lastNums = "0123456789";

            if (Regex.IsMatch(messageText, pattern))
            {
                int firstNumIndex = messageText.IndexOfAny(firstNums.ToCharArray());
                int lastNumIndex = messageText.LastIndexOfAny(lastNums.ToCharArray());
                int length = lastNumIndex - firstNumIndex + 1;

                var substring = messageText.Substring(firstNumIndex, length);

                decimal number;
                if (decimal.TryParse(substring, out number))
                {
                    await repo.SetPercentsAsync(number, chatSettingsId);

                    var text = "Successfuly set your new percent settings!";
                    await client.SendTextMessageAsync(chatSettingsId, text, replyToMessageId: messageId);
                }
                else
                {
                    var errorMsg = $"<<< /percents n% >>> pattern matching failed on n = {substring} value";
                    throw new Exception(errorMsg);
                }
            }
            else
            {
                var errorMsg = "<<< /percents n% >>> pattern matching failed for unknown reason";
                throw new Exception(errorMsg);
            }
        }
    }
}
