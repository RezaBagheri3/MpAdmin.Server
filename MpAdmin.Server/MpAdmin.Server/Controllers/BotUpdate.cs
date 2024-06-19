using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.Domain;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using MpAdmin.Server.DAL.Entities;
using MpAdmin.Server.DateTimeExtensions;

namespace MpAdmin.Server.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class BotUpdate : ControllerBase
    {
        private MpAdminContext _context;
        private static readonly TelegramBotClient bot = new TelegramBotClient("2079931665:AAH3iHwzWlWAuQ0qdCCSV0JePN0aCDKF7oU");
        private ReplyKeyboardMarkup mainKeyboardMarkup;

        public BotUpdate(MpAdminContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Update([FromBody] Rootobject item)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                mainKeyboardMarkup = new ReplyKeyboardMarkup();

                KeyboardButton[] row1 =
                {
                    new KeyboardButton("راهنماي استفاده" + " " + "\U00002049")
                };
                KeyboardButton[] row2 =
                {
                    new KeyboardButton("ارتباط با ما" + " " + "\U0000260E")
                };

                mainKeyboardMarkup.Keyboard = new KeyboardButton[][]
                {
                    row1,row2
                };

                var chatId = item.message.chat.id;
                var message = item.message.text.Fa2En();
                var from = item.message.from;
                double x;

                if (message != null)
                {
                    string[] checkMessageString = message.Split(' ');

                    if (message.Contains("/start"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("سلام کاربر" + " " + from.first_name + " به ربات استعلام موجودي مجموعه MP خوش آمديد " + "\U0001F339");
                        sb.AppendLine("راهنماي استفاده : /help");
                        sb.AppendLine("ارتباط با ما : /contactus");

                        bot.SendTextMessageAsync(chatId, sb.ToString(), ParseMode.Default, null, false, false, 0, false, mainKeyboardMarkup);
                    }
                    else if (message.Contains("/contactus") || message.Contains("ارتباط با ما"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("آدرس کانال :");
                        sb.AppendLine("@wallpaperstock113");
                        sb.AppendLine("");
                        sb.AppendLine("پیج اینستاگرام :");
                        sb.AppendLine("https://www.instagram.com/mp.wallpaper13");
                        sb.AppendLine("");
                        sb.AppendLine("شماره تماس :");
                        sb.AppendLine("\U0000260E" + "09157698346");
                        sb.AppendLine("\U0000260E" + "09158972595");

                        bot.SendTextMessageAsync(chatId, sb.ToString());
                    }
                    else if (message.Contains("/help") || message.Contains("راهنماي استفاده"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("\U000026AB" + "براي استعلام موجودي به اين صورت عمل کنيد  :");
                        sb.AppendLine("");
                        sb.AppendLine("\U0001F534" + "ابتدا کد کاغذ مورد نظر خود را نوشته  سپس يک فاصله قرار دهيد و بعد تعداد رول را وارد کنيد . ");
                        sb.AppendLine("");
                        sb.AppendLine("\U000026AB" + "مثال : 50 43521  ");
                        sb.AppendLine("");
                        sb.AppendLine("\U0001F534" + "دقت کنيد که اعداد باید به صورت انگلیسی وارد شود ");
                        sb.AppendLine("");
                        sb.AppendLine("با تشکر مديريت مجموعه  MP" + "\U0001F339");

                        bot.SendTextMessageAsync(chatId, sb.ToString());
                    }
                    else if (double.TryParse(checkMessageString[0], out x))
                    {
                        string[] CodeAndStock = message.Split(' ');

                        if (CodeAndStock[0].Length < 3)
                        {
                            StringBuilder sd = new StringBuilder();
                            sd.AppendLine("کد کاغذ وارد شده نامعتبر می باشد " + "\U000026A0");

                            bot.SendTextMessageAsync(chatId, sd.ToString());
                        }
                        else if (CodeAndStock.Length < 2)
                        {
                            StringBuilder sd = new StringBuilder();
                            sd.AppendLine("\U000026AA" + "برای استعلام موجودی کاغذ باید کد کاغذ + یک فاصله + تعداد رول مد نظر خود را وارد کنید .");

                            bot.SendTextMessageAsync(chatId, sd.ToString());
                        }
                        else
                        {
                            var WallPaperItem = unitOfWork.WallPaperRepo.Get(r => r.Code == CodeAndStock[0]).ToList();

                            if (WallPaperItem.Count() > 0)
                            {
                                if (WallPaperItem.Count() < 2)
                                {
                                    try
                                    {
                                        foreach (var wallitem in WallPaperItem)
                                        {
                                            if (wallitem.Stock >= int.Parse(CodeAndStock[1]))
                                            {
                                                StringBuilder sb = new StringBuilder();
                                                sb.AppendLine($"کد کاغذ : {wallitem.Code}");
                                                sb.AppendLine("وضعیت موجودی : موجود" + " " + "\U00002705");
                                                bot.SendTextMessageAsync(chatId, sb.ToString());
                                            }
                                            else
                                            {
                                                StringBuilder sb = new StringBuilder();
                                                sb.AppendLine($"کد کاغذ : {wallitem.Code}");
                                                sb.AppendLine("وضعیت موجودی : ناموجود" + " " + "\U0000274C");
                                                bot.SendTextMessageAsync(chatId, sb.ToString());
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        sb.AppendLine("\U000026AB" + "براي استعلام موجودي به اين صورت عمل کنيد  :");
                                        sb.AppendLine("");
                                        sb.AppendLine("\U0001F534" +
                                                      "ابتدا کد کاغذ مورد نظر خود را نوشته  سپس يک فاصله قرار دهيد و بعد تعداد رول را وارد کنيد . ");
                                        sb.AppendLine("");
                                        sb.AppendLine("\U000026AB" + "مثال : 50 43521  ");
                                        sb.AppendLine("");
                                        sb.AppendLine("\U0001F534" +
                                                      "دقت کنيد که اعداد باید به صورت انگلیسی وارد شود ");
                                        sb.AppendLine("");
                                        sb.AppendLine("\U000026AB" +
                                                      "لطفا دقت کنید که تعداد رول را به صورت عددی وارد کنید ");
                                        sb.AppendLine("با تشکر مديريت مجموعه  MP" + "\U0001F339");
                                        bot.SendTextMessageAsync(chatId, sb.ToString());
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        foreach (var paperitem in WallPaperItem)
                                        {
                                            if (paperitem.Stock >= int.Parse(CodeAndStock[1]))
                                            {
                                                StringBuilder sb = new StringBuilder();
                                                sb.AppendLine($"کد کاغذ : {paperitem.Code}");
                                                sb.AppendLine($"لُت : {paperitem.BatchNumber}");
                                                sb.AppendLine("وضعیت موجودی : موجود" + " " + "\U00002705");
                                                bot.SendTextMessageAsync(chatId, sb.ToString());
                                            }
                                            else
                                            {
                                                StringBuilder sb = new StringBuilder();
                                                sb.AppendLine($"کد کاغذ : {paperitem.Code}");
                                                sb.AppendLine($"لُت : {paperitem.BatchNumber}");
                                                sb.AppendLine("وضعیت موجودی : ناموجود" + " " + "\U0000274C");
                                                bot.SendTextMessageAsync(chatId, sb.ToString());
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        sb.AppendLine("\U000026AB" + "براي استعلام موجودي به اين صورت عمل کنيد  :");
                                        sb.AppendLine("");
                                        sb.AppendLine("\U0001F534" +
                                                      "ابتدا کد کاغذ مورد نظر خود را نوشته  سپس يک فاصله قرار دهيد و بعد تعداد رول را وارد کنيد . ");
                                        sb.AppendLine("");
                                        sb.AppendLine("\U000026AB" + "مثال : 50 43521  ");
                                        sb.AppendLine("");
                                        sb.AppendLine("\U0001F534" +
                                                      "دقت کنيد که اعداد باید به صورت انگلیسی وارد شود ");
                                        sb.AppendLine("");
                                        sb.AppendLine("\U000026AB" +
                                                      "لطفا دقت کنید که تعداد رول را به صورت عددی وارد کنید ");
                                        sb.AppendLine("با تشکر مديريت مجموعه  MP" + "\U0001F339");
                                        bot.SendTextMessageAsync(chatId, sb.ToString());
                                    }
                                }
                            }
                            else
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine("کد کاغذ وارد شده یافت نشد " + "\U0000274C");
                                bot.SendTextMessageAsync(chatId, sb.ToString());
                            }

                        }
                    }
                    else
                    {
                        StringBuilder sg = new StringBuilder();
                        sg.AppendLine("دستور وارد شده معتبر نمي باشد." + "\U0000274C");
                        sg.AppendLine("\U000026A1" + "دکمه راهنماي استفاده را براي اطلاع از نحوه کار ربات بفشاريد يا دستور زير را وارد کنيد : ");
                        sg.AppendLine("/help");

                        bot.SendTextMessageAsync(chatId, sg.ToString());
                    }

                }
                else
                {
                    StringBuilder sg = new StringBuilder();
                    sg.AppendLine("دستور وارد شده معتبر نمي باشد." + "\U0000274C");
                    sg.AppendLine("\U000026A1" + "دکمه راهنماي استفاده را براي اطلاع از نحوه کار ربات بفشاريد يا دستور زير را وارد کنيد : ");
                    sg.AppendLine("/help");

                    bot.SendTextMessageAsync(chatId, sg.ToString());
                }

                var CheckChatId = unitOfWork.BotChatRepo.FirstOrDefault(r => r.ChatId == chatId.ToString());

                if (CheckChatId == null && chatId.ToString() != null && chatId > 0)
                {
                    BotChat botChatId = new BotChat()
                    {
                        ChatId = chatId.ToString()
                    };
                    unitOfWork.BotChatRepo.Create(botChatId);
                }

                var CheckUserName = unitOfWork.TelegramUserRepo.FirstOrDefault(p => p.UserName == from.first_name);

                if (CheckUserName == null && from.first_name != null)
                {
                    TelegramUser Teluser = new TelegramUser()
                    {
                        UserName = from.first_name
                    };
                    unitOfWork.TelegramUserRepo.Create(Teluser);
                }
                unitOfWork.Save();
            }
            catch (Exception)
            {
                return Ok();
            }
            return Ok();
        }
    }
}
