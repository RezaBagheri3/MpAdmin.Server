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
            return Calender.GetYear(input) + "/" + Calender.GetMonth(input) + "/" + Calender.GetDayOfMonth(input);
        }
    }
}
