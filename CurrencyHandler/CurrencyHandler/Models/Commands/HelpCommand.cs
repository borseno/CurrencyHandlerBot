using Telegram.Bot.Types;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using static System.IO.File;
using CurrencyHandler.Models.InlineKeyboardHandlers.Abstractions;

namespace CurrencyHandler.Models.Commands
{
    public class HelpCommand : Command
    { 
        private const string HelpTextPath = "Texts/Help.txt";

        public HelpCommand(IKeyboards keyboards, ICurrenciesRepository repo) : base(keyboards, repo)
        {
        }

        public override string Name => "Help";

        /// <summary>
        /// Sends Info.txt content to the chat 
        /// </summary>
        /// <param name="message">the message a user sent</param>
        /// <param name="client">Bot instance, needed to answer on the message</param>
        /// <param name="repo">Repository for the whole db, allows this command handler to save/read data</param>
        /// <returns>Task to be awaited</returns>
        public override async Task Execute(Message message)
        {
            var messageId = message.MessageId;
            var chatId = message.Chat.Id;

            var text = await ReadAllTextAsync(HelpTextPath);

            await Client.SendTextMessageAsync(chatId, text, replyToMessageId: messageId);
        }
    }
}
