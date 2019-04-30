using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyHandler.Models;
using CurrencyHandler.Models.Database.Contexts;
using CurrencyHandler.Models.Database.Repositories;
using CurrencyHandler.Models.DbModels;
using CurrencyHandler.Models.ExceptionsHandling;
using CurrencyHandler.Models.Extensions;
using CurrencyHandler.Models.QueryHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace CurrencyHandler.Controllers
{
    [Route("api/message/update")]
    public class MessageController : Controller
    {
        private static readonly UpdateType[] Types = new UpdateType[]
            {
                UpdateType.Message,
                UpdateType.InlineQuery,
                UpdateType.CallbackQuery
            };

        private readonly CallBackMessageHandler _callBackMessageHandler;
        private readonly InlineQueryHandler _inlineQueryHandler;
        private readonly CurrenciesRepository _repo;

        public MessageController(CurrenciesRepository repo)
        {
            _callBackMessageHandler = new CallBackMessageHandler(repo);
            _inlineQueryHandler = new InlineQueryHandler(repo);
            
            _repo = repo;
        }

        [HttpGet]
        public string Get()
        {
            return "Hi there! :D";
        }

        [HttpPost]
        public async Task<OkResult> Post([FromBody]Update update)
        {
            if (update == null)
                return Ok();

            if (!update.Type.In(Types))
                return Ok();

            try
            {
                if (update.Type == UpdateType.CallbackQuery)
                {
                    await _callBackMessageHandler.Handle(update.CallbackQuery);
                    return Ok();
                }

                if (update.Type == UpdateType.InlineQuery)
                {
                    await _inlineQueryHandler.HandleAsync(update.InlineQuery);
                    return Ok();
                }

                var commands = Bot.Commands;
                var botClient = await Bot.Get();
                var message = update.Message;

                if (message == null || String.IsNullOrEmpty(message.Text))
                {
                    return Ok();
                }

                foreach (var command in commands)
                {
                    if (command?.Contains(message.Text) ?? false)
                    {
                        await command.Execute(message, botClient, _repo);

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                await ExceptionsHandling.HandleExceptionAsync(e, update, update.Type);
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _repo.Dispose();
        }
    }
}