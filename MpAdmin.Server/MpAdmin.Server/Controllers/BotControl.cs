using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

namespace MpAdmin.Server.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class BotControl : ControllerBase
    {
        private MpAdminContext _context;
        private Thread botThread;
        private TelegramBotClient bot;
        private ReplyKeyboardMarkup mainKeyboardMarkup;

        public BotControl(MpAdminContext context)
        {
            _context = context;
        }

        void RunBot()
        {
            bot = new TelegramBotClient("1919785864:AAGVm7IOAAaZekxYSgauTwIOk4bFRrvrskY");

            mainKeyboardMarkup = new ReplyKeyboardMarkup();
            KeyboardButton[] row1 =
            {
                new KeyboardButton("استعلام موجودی کاغذ")
            };
            KeyboardButton[] row2 =
            {
                new KeyboardButton("کانال ما" + " " + "\U0001F4E2"),
                new KeyboardButton("گروه ما" + " " + "\U0001F4CE")
            };

            mainKeyboardMarkup.Keyboard = new KeyboardButton[][]
            {
                row1,row2
            };

            int offset = 0;
            var wallPaperStockControl = 0;

            while (true)
            {
                Update[] update = bot.GetUpdatesAsync(offset).Result;

                foreach (var item in update)
                {
                    offset = item.Id + 1;



                    if (item.Message == null)
                    {
                        continue;
                    }

                    var chatId = item.Message.Chat.Id;
                    var message = item.Message.Text;
                    var from = item.Message.From;
                    double x;


                    if (message != null)
                    {
                        string[] checkMessageString = message.Split(' ');

                        if (message.Contains("/start"))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("یا الله " + from.FirstName);
                            sb.AppendLine("سلام علیکم و رحمت الله و برکاته");
                            sb.AppendLine("کانال ما  : /channel");

                            bot.SendTextMessageAsync(chatId, sb.ToString(), ParseMode.Default, null, false, false, 0, false, mainKeyboardMarkup);
                        }
                        else if (message.Contains("/channel") || message.Contains("کانال ما"))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("کانال ما : t.me/wallpaperstock113");

                            bot.SendTextMessageAsync(chatId, sb.ToString());
                        }
                        else if (message.Contains("/group") || message.Contains("گروه ما"))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("گروه ما : t.me/salewallpaperstock");

                            bot.SendTextMessageAsync(chatId, sb.ToString());
                        }
                        else if (message.Contains("بازگشت به منوي اصلي"))
                        {
                            bot.SendTextMessageAsync(chatId, "به منوي اصلي بازگشتيد", ParseMode.Default, null, false, false, 0, false, mainKeyboardMarkup);
                        }
                        else if (message.Contains("/group") || message.Contains("گروه ما"))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("گروه ما : t.me/salewallpaperstock");

                            bot.SendTextMessageAsync(chatId, sb.ToString());
                        }
                        else if (message.Contains("استعلام موجودی کاغذ"))
                        {
                            wallPaperStockControl = 1;
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("کد کاغذ + یک فاصله + تعداد رول مد نظر خود را وارد کنید .");

                            bot.SendTextMessageAsync(chatId, sb.ToString());
                        }
                        else if (double.TryParse(checkMessageString[0], out x))
                        {
                            if (wallPaperStockControl == 1)
                            {
                                string[] CodeAndStock = message.Split(' ');

                                if (CodeAndStock[0].Length < 3 || CodeAndStock[0].Length > 6)
                                {
                                    StringBuilder sd = new StringBuilder();
                                    sd.AppendLine("کد کاغذ وارد شده نامعتبر می باشد .");

                                    bot.SendTextMessageAsync(chatId, sd.ToString());
                                }
                                else if (CodeAndStock.Length < 2)
                                {
                                    StringBuilder sd = new StringBuilder();
                                    sd.AppendLine("برای استعلام موجودی کاغذ باید کد کاغذ + یک فاصله + تعداد رول مد نظر خود را وارد کنید .");

                                    bot.SendTextMessageAsync(chatId, sd.ToString());
                                }
                                else
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendLine("کد کاغذ " + CodeAndStock[0]);
                                    sb.AppendLine("تعداد رول " + CodeAndStock[1]);

                                    bot.SendTextMessageAsync(chatId, sb.ToString());
                                }
                            }
                            else
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine("برای استعلام موجودی ابتدا دکمه استعلام موجودی کاغذ را بفشارید .");


                                bot.SendTextMessageAsync(chatId, sb.ToString());
                            }
                        }
                        else
                        {
                            StringBuilder sg = new StringBuilder();
                            sg.AppendLine("دستور وارد شده معتبر نمي باشد.");
                            sg.AppendLine("از دکمه ها براي دسترسي به دستور مورد نياز خود استفاده کنيد .");

                            bot.SendTextMessageAsync(chatId, sg.ToString());
                        }

                    }
                    else
                    {
                        StringBuilder sg = new StringBuilder();
                        sg.AppendLine("دستور وارد شده معتبر نمي باشد.");

                        bot.SendTextMessageAsync(chatId, sg.ToString());
                    }
                }
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<int>> StartBot()
        {
            try
            {
                botThread = new Thread(new ThreadStart(RunBot));
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
    }
}
