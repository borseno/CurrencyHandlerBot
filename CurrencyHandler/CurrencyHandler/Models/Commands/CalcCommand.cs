using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CurrencyHandler.Models.DataCaching;
using Telegram.Bot.Types;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.HelperClasses;
using CurrencyHandler.Models.InlineKeyboardHandlers.Abstractions;

namespace CurrencyHandler.Models.Commands
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class CalcCommand : Command
    {
        private readonly char[] charsToIgnore;

        public CalcCommand(IKeyboards keyboards, ICurrenciesRepository repo) : base(keyboards, repo)
        {
            charsToIgnore = $"/{Name.ToLower()}{Name.ToUpper()}".ToCharArray();
        }

        public override string Name => "C";

        /// <summary>
        /// Executes the "Calc" command, which is the main command of the app.
        /// It converts ValueCurrency to DisplayCurrencies and prints the results out. 
        /// </summary>
        /// <param name="message">the message a user sent</param>
        /// <param name="client">Bot instance, needed to answer on the message</param>
        /// <param name="repo">Repository for the whole db, allows this command handler to save/read data</param>
        /// <returns>Task to be awaited</returns>
        public override async Task Execute(Message message)
        {
            var messageText = message?.Text;

            if (messageText == null)
                return;

            var content = messageText
                .TrimStart(charsToIgnore) // "/calc" <- remove that if exists
                .Replace(',', '.'); // can process both , and . in the number

            if (!decimal.TryParse(content, out var value))
            {
                throw new Exception("Problems occured when parsing your message");
            }

            var messageId = message.MessageId;
            var chatId = message.Chat.Id;
            var dataTask = CurrenciesDataCaching.GetValCursAsync();
            var percentsTask = Repo.GetPercentsAsync(chatId);
            var valueCurrencyTask = Repo.GetCurrencyEmojiAsync(chatId);
            var displayCurrenciesTask = Repo.GetDisplayCurrenciesEmojisAsync(chatId);

            // execute them all in parallel and wait for the completion
            await Task.WhenAll(dataTask, percentsTask, displayCurrenciesTask, valueCurrencyTask);

            var values =
                await ValuesCalculator.GetCurrenciesValuesAsync(
                    value,
                    valueCurrencyTask.Result,
                    dataTask.Result,
                    displayCurrenciesTask.Result);

            var textToSend = await AnswerBuilder.BuildStringFromValuesAsync(
                values, valueCurrencyTask.Result, percentsTask.Result);

            await Client.SendTextMessageAsync(chatId, textToSend, replyToMessageId: messageId);
        }

        /// <inheritdoc />
        /// <summary>
        /// overrides Command's Contains method, allowing to pass just a number
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override bool Contains(string command)
        {
            if (command == null)
                return false;

            return base.Contains(command) || decimal.TryParse(command, out _);
        }
    }
}
