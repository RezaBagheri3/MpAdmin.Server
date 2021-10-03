﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.Domain;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using MpAdmin.Server.Models;

namespace MpAdmin.Server.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class BotControl : ControllerBase
    {
        private Thread botThread;

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<int>> StartBot()
        {
            try
            {
                Bot bot = new Bot();
                botThread = new Thread(new ThreadStart(bot.RunBot));
                botThread.Start();

                return Ok(
                    new
                    {
                        result = 1
                    }
                );
            }
            catch (Exception e)
            {
                return BadRequest(
                    new
                    {
                        e
                    }
                );
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<int>> SendBotMessage([FromBody] BotSendMessageModel model)
        {
            try
            {
                Bot bot = new Bot();
                bot.SendMessage(model.message);

                return Ok(
                    new
                    {
                        result = 1
                    }
                );
            }
            catch (Exception e)
            {
                return BadRequest(
                    new
                    {
                        e
                    }
                );
            }
        }
    }
}
