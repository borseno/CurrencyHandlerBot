using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.QueryHandling
{
    public interface IInlineQueryHandler : IDisposable
    {
        Task HandleAsync(InlineQuery q);
    }
}