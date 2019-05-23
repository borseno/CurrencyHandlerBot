using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.Commands
{
    public interface ICommand : IDisposable
    {
        string Name { get; }

        bool Contains(string command);
        Task Execute(Message message);
    }
}