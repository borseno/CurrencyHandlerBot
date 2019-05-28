using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.InlineKeyboardHandlers.Abstractions;
using Telegram.Bot.Types;
using static System.IO.File;

namespace CurrencyHandler.Models.Commands
{
    public class StartCommand : Command
    {
        private const string StartTextPath = "Texts/Start.txt";
        public StartCommand(IKeyboards keyboards, ICurrenciesRepository repo) : base(keyboards, repo)
        {
        }

        public override string Name => "Start";

        public override async Task Execute(Message message)
        {
            var messageId = message.MessageId;
            var chatId = message.Chat.Id;

            var text = await ReadAllTextAsync(StartTextPath);

            await Client.SendTextMessageAsync(chatId, text, replyToMessageId: messageId);
        }
    }
}
