using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyHandler.Models;
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

        private readonly CallBackMessageHandler _handler;
        private readonly ChatSettingsContext _ctx;

        public MessageController(ChatSettingsContext ctx)
        {
            _ctx = ctx;
            _handler = new CallBackMessageHandler(_ctx);
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
                    await _handler.Handle(update.CallbackQuery);
                    return Ok();
                }

                if (update.Type == UpdateType.InlineQuery)
                {
                    await InlineQueryHandler.Handle(update.InlineQuery);
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
                        await command.Execute(message, botClient, _ctx);

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
            _ctx.Dispose();

            base.Dispose(disposing);
        }
    }
}