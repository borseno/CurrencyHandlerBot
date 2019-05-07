using IO = System.IO;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;

namespace CurrencyHandler.Models.Commands
{
    public class InfoCommand : Command
    {
        private const string InfoTextPath = "Texts/Info.txt";

        private InfoCommand()
        {

        }

        public static InfoCommand Instance { get; } = new InfoCommand();

        public override string Name => "Info";

        public override async Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo)
        {
            var messageId = message.MessageId;
            var chatId = message.Chat.Id;

            var text = await IO.File.ReadAllTextAsync(InfoTextPath);

            await client.SendTextMessageAsync(chatId, text, replyToMessageId: messageId);
        }
    }
}
