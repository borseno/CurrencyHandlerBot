using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using IO = System.IO;

namespace CurrencyHandler.Models.Commands
{
    public class StartCommand : Command
    {
        private const string StartTextPath = "Texts/Start.txt";

        public static StartCommand Instance { get; } = new StartCommand();

        public override string Name => "Start";

        private StartCommand()
        {

        }

        public override async Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo)
        {
            var chatId = message.Chat.Id;

            var text = await IO.File.ReadAllTextAsync(StartTextPath);

            await client.SendTextMessageAsync(chatId, text);
        }
    }
}
