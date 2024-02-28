using ShahparQuestion.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ShahparQuestion.Infrastructure
{
    public static class AttendanceTypeHelper
    {
        public static List<AttendanceType> DetermineAttendanceType(List<EmployeeLog> sortedRecords, DateTime date, IEnumerable<EmployeeLog> allRecords)
        {
            var allowedEntryEnd = new TimeSpan(8, 45, 0);
            var allowedExitStart = new TimeSpan(17, 0, 0);
            var minimumWorkHours = new TimeSpan(8, 30, 0); // 8.5 hours

            var attendanceTypes = new List<AttendanceType>();

            // set attendance type to Error if records count is even
            if (sortedRecords.Count() % 2 != 0)
            {
                attendanceTypes.Add(AttendanceType.Error);
                return attendanceTypes;
            }
            // check if type is full day leave
            if (!sortedRecords.Any() && allRecords.Any(r => r.LogDateTime.Date == date))
            {
                attendanceTypes.Add(AttendanceType.Full_Day_Leave);
                return attendanceTypes;
            }

            var firstLog = sortedRecords.First();
            var lastLog = sortedRecords.Last();
            var totalWorkedHours = lastLog.LogDateTime - firstLog.LogDateTime;

            // define conditions
            bool isWorkHoursInsufficient = totalWorkedHours < minimumWorkHours;
            bool isLateArrival = firstLog.LogDateTime.TimeOfDay > allowedEntryEnd || lastLog.LogDateTime.TimeOfDay < allowedExitStart || isWorkHoursInsufficient;
            bool isHourlyLeave = sortedRecords.Count > 2 && isWorkHoursInsufficient;

            // determine attendance type based on defined conditions
            if (isLateArrival) attendanceTypes.Add(AttendanceType.Late);
            if (isHourlyLeave) attendanceTypes.Add(AttendanceType.Hourly_Leave);

            // if no other attendance type was added, then it's normal
            if (!attendanceTypes.Any())
            {
                attendanceTypes.Add(AttendanceType.Normal);
            }
            return attendanceTypes;
        }
    }
}
