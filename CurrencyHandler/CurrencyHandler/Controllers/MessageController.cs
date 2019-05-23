﻿using System;
using System.Threading.Tasks;
using CurrencyHandler.Models;
using CurrencyHandler.Models.Commands;
using CurrencyHandler.Models.ExceptionsHandling;
using CurrencyHandler.Models.Extensions;
using CurrencyHandler.Models.InlineKeyboardHandlers;
using CurrencyHandler.Models.QueryHandling;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace CurrencyHandler.Controllers
{
    [Route("api/message/update")]
    public class MessageController : Controller
    {
        private static readonly UpdateType[] Types =
        {
            UpdateType.Message,
            UpdateType.InlineQuery,
            UpdateType.CallbackQuery
        };

        private readonly InlineQueryHandler inlineQueryHandler;
        private readonly Commands commands;
        private readonly Keyboards keyboards;

        public MessageController(InlineQueryHandler inlineQueryHandler, Keyboards keyboards, Commands commands)
        {
            this.inlineQueryHandler = inlineQueryHandler;
            this.commands = commands;
            this.keyboards = keyboards;
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
                    foreach (var i in keyboards.Get())
                    {
                        if (i.Contains(update.CallbackQuery.Data))
                        {
                            await i.HandleCallBackAsync(update.CallbackQuery);
                            return Ok();
                        }
                    }
                }

                if (update.Type == UpdateType.InlineQuery)
                {
                    await inlineQueryHandler.HandleAsync(update.InlineQuery);
                    return Ok();
                }

                var message = update.Message;

                if (message == null || String.IsNullOrEmpty(message.Text))
                {
                    return Ok();
                }

                foreach (var command in commands.Get())
                {
                    if (command?.Contains(message.Text) ?? false)
                    {
                        await command.Execute(message);

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

            keyboards.Dispose();
            commands.Dispose();
            inlineQueryHandler.Dispose();
        }
    }
}