using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MpAdmin.Server.DateTimeExtensions
{
    public static class PersianExtension
    {
        public static string ToPersianDate(this DateTime input)
        {
            var Calender = new PersianCalendar();
            return Calender.GetYear(input) + "/" + Calender.GetMonth(input).ToString("00") + "/" + Calender.GetDayOfMonth(input).ToString("00");
        }

        public static int GetPersianMonth(this DateTime date)
        {
            var Calender = new PersianCalendar();
            return Calender.GetMonth(date);
        }

        public static int GetPersianYear(this DateTime date)
        {
            var Calender = new PersianCalendar();
            return Calender.GetYear(date);
        }
    }
}
