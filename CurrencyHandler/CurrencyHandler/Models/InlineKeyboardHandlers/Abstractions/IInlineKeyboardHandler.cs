using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CurrencyHandler.Models.InlineKeyboardHandlers.Abstractions
{
    public interface IInlineKeyboardHandler : IDisposable
    {
        string Name { get; }

        bool Contains(string callBackData);
        void HandleCallBack(CallbackQuery callbackQuery);
        Task HandleCallBackAsync(CallbackQuery callbackQuery);
        void SendKeyboard(Message message);
        Task SendKeyboardAsync(Message message);
    }
}