using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CurrencyHandler.Models.DataCaching;
using Telegram.Bot;
using Telegram.Bot.Types;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.HelperClasses;

namespace CurrencyHandler.Models.Commands
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class CalcCommand : Command
    {
        private readonly char[] charsToIgnore;

        public static CalcCommand Instance { get; } = new CalcCommand();

        private CalcCommand()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            // Reason: The Property shouldn't reference any non-constructed fields and stuff.
            // It should only contain the name of the command.
            // Hence, there shouldn't be any undefined behaviors or runtime exceptions 
            // if one doesn't reference any non-constructed fields in the get method 
            charsToIgnore = $"/{Name.ToLower()}{Name.ToUpper()}".ToCharArray();
        }

        public override string Name => "Calc";

        public override async Task Execute(Message message, TelegramBotClient client, CurrenciesRepository repo)
        {
            var messageText = message?.Text;

            if (messageText == null)
                return;

            var messageId = message.MessageId;
            var chatId = message.Chat.Id;
            var content = messageText
                .TrimStart(charsToIgnore)
                .Replace(',', '.');

            if (decimal.TryParse(content, out var value))
            {
                var dataTask = CurrenciesDataCaching.GetValCursAsync();
                var percentsTask = repo.GetPercentsAsync(chatId);
                var valueCurrencyTask = repo.GetCurrencyEmojiAsync(chatId);
                var displayCurrenciesTask = repo.GetDisplayCurrenciesEmojisAsync(chatId);

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

                await client.SendTextMessageAsync(chatId, textToSend, replyToMessageId: messageId);
            }
            else
            {
                throw new Exception("Problems occured when parsing your message");
            }
        }

        // messages that only contain digits can be processed too.
        public override bool Contains(string command)
        {
            if (command == null)
                return false;

            return base.Contains(command) || decimal.TryParse(command, out _);
        }
    }
}
