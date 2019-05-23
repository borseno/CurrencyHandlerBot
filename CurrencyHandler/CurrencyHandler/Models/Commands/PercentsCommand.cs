using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.InlineKeyboardHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    public class PercentsCommand : Command
    {
        public PercentsCommand(Keyboards keyboards, CurrenciesRepository repo) : base(keyboards, repo)
        {
        }

        public override string Name => "Percents";

        /// <summary>
        /// Handles percents settings change for the chat 
        /// </summary>
        /// <param name="message">the message a user sent</param>
        /// <param name="client">Bot instance, needed to answer on the message</param>
        /// <param name="repo">Repository for the whole db, allows this command handler to save/read data</param>
        /// <returns>Task to be awaited</returns>
        public override async Task Execute(Message message)
        {
            const string pattern = @"\/percents \-{0,1}\d+(.\d+){0,1}";
            const string numbers = "0123456789";

            var messageText = message.Text;

            if (!Regex.IsMatch(messageText, pattern))
            {
                var errorMsg = "<<< /percents n >>> pattern matching failed for unknown reason";
                throw new Exception(errorMsg);
            }

            // get substring containing decimal value
            var firstNumIndex = messageText.IndexOfAny(numbers.ToCharArray());
            var lastNumIndex = messageText.LastIndexOfAny(numbers.ToCharArray());
            var length = lastNumIndex - firstNumIndex + 1;
            var substring = messageText
                .Substring(firstNumIndex, length)
                .Replace(',', '.'); // can process both , and . in the number;

            // convert it to decimal and if unsuccessful, return
            if (!decimal.TryParse(substring, out var number))
            {
                var errorMsg = $"<<< /percents n >>> pattern matching failed on n = {substring} value";
                throw new Exception(errorMsg);
            }

            // else, do the processing
            var messageId = message.MessageId;
            var chatId = message.Chat.Id;
            var text = "Successfuly set your new percent settings!";

            await Repo.SetPercentsAsync(number, chatId);

            await Client.SendTextMessageAsync(chatId, text, replyToMessageId: messageId);
        }
    }
}
