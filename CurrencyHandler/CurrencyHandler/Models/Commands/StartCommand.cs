using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using static System.IO.File;

namespace CurrencyHandler.Models.Commands
{
    public class StartCommand : Command
    {
        public static StartCommand Instance { get; } = new StartCommand();

        public override string Name => "Start";

        private StartCommand()
        {

        }

        public override async Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo)
        {
            await InfoCommand.Instance.Execute(message, client, repo);
        }
    }
}
