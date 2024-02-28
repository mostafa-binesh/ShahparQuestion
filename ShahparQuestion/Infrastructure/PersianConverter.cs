using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ShahparQuestion.Infrastructure
{
    public static class PersianConventor
    {
        public static string ToShamsi(this DateTime date)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            return $"{persianCalendar.GetYear(date)}/{persianCalendar.GetMonth(date):00}/{persianCalendar.GetDayOfMonth(date):00}";
        }
        public static string ToDayOfWeek(this DayOfWeek dayOfWeek)
        {
            // map DayOfWeek to Persian
            var days = new Dictionary<DayOfWeek, string>
        {
            { DayOfWeek.Saturday, "شنبه" },
            { DayOfWeek.Sunday, "یکشنبه" },
            { DayOfWeek.Monday, "دوشنبه" },
            { DayOfWeek.Tuesday, "سه شنبه" },
            { DayOfWeek.Wednesday, "چهارشنبه" },
            { DayOfWeek.Thursday, "پنجشنبه" },
            { DayOfWeek.Friday, "جمعه" }
        };

            return days[dayOfWeek];
        }
    }
}
