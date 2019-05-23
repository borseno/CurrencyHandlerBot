using System.Threading.Tasks;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.InlineKeyboardHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    public class StartCommand : Command
    {
        public StartCommand(IKeyboards keyboards, ICurrenciesRepository repo) : base(keyboards, repo)
        {
        }

        public override string Name => "Start";

        public override async Task Execute(Message message)
        {
            await new InfoCommand(Keyboards, Repo).Execute(message);
        }
    }
}
