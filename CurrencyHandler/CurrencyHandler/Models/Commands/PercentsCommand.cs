using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    public class PercentsCommand : Command
    {
        public static PercentsCommand Instance { get; } = new PercentsCommand();

        public override string Name => "Percents";

        private PercentsCommand()
        {

        }

        public override async Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo)
        {
            var messageId = message.MessageId;
            var chatId = message.Chat.Id;
            var messageText = message.Text;

            const string pattern = @"\/percents \-{0,1}\d+(.\d+){0,1}";
            const string firstNums = "123456789"; // TODO: Fix wrong constraint (can't have 0 as the first digit)
            const string lastNums = "0123456789";

            if (!Regex.IsMatch(messageText, pattern))
            {
                var errorMsg = "<<< /percents n% >>> pattern matching failed for unknown reason";
                throw new Exception(errorMsg);
            }

            // get substring containing decimal value
            int firstNumIndex = messageText.IndexOfAny(firstNums.ToCharArray());
            int lastNumIndex = messageText.LastIndexOfAny(lastNums.ToCharArray());
            int length = lastNumIndex - firstNumIndex + 1;
            var substring = messageText.Substring(firstNumIndex, length);

            // convert it to decimal and process
            if (decimal.TryParse(substring, out var number))
            {
                await repo.SetPercentsAsync(number, chatId);

                var text = "Successfuly set your new percent settings!";
                await client.SendTextMessageAsync(chatId, text, replyToMessageId: messageId);
            }
            else 
            {
                var errorMsg = $"<<< /percents n% >>> pattern matching failed on n = {substring} value";
                throw new Exception(errorMsg);
            }
        }
    }
}
