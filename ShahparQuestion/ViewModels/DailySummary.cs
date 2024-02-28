using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShahparQuestion.ViewModels
{
    public class DailySummary
    {
        public DateTime Date { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; } 
        public List<AttendanceType> AttendanceTypes { get; set; }
        public TimeSpan FirstEntry { get; set; }
        public TimeSpan LastExit { get; set; }
        public TimeSpan TotalWorkedHours { get; set; }
        public List<TimeSpan> TimeRecords { get; set; } = new List<TimeSpan>();
        public int NumberOfRecords { get; set; }
    }
    public enum AttendanceType
    {
        Normal,
        Hourly_Leave,
        Late,
        Full_Day_Leave,
        Error,
    }
    // extension ToPersianString method
    public static class EnumExtensions
    {
        public static string ToPersianString(this AttendanceType value)
        {
            switch (value)
            {
                case AttendanceType.Normal:
                    return "عادی";
                case AttendanceType.Hourly_Leave:
                    return "مرخصی ساعتی";
                case AttendanceType.Late:
                    return "تاخیر";
                case AttendanceType.Full_Day_Leave:
                    return "مرخصی روزانه";
                case AttendanceType.Error:
                    return "خطا";
                default:
                    return "نوع ناشناخته";
            }
        }
    }
}
