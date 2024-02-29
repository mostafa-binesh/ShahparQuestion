using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class DailySummary
    {
        public DateTime Date { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string AttendanceType { get; set; } // Normal, Late, Early Departure, Full Day Leave, Hourly Leave, Error
        public TimeSpan FirstEntry { get; set; }
        public TimeSpan LastExit { get; set; }
        public TimeSpan TotalWorkedHours { get; set; }
        public int NumberOfRecords { get; set; }

        public DailySummary(DateTime date, string employeeId, string name, string attendanceType, TimeSpan firstEntry, TimeSpan lastExit, TimeSpan totalWorkedHours, int numberOfRecords)
        {
            Date = date;
            EmployeeID = employeeId;
            Name = name;
            AttendanceType = attendanceType;
            FirstEntry = firstEntry;
            LastExit = lastExit;
            TotalWorkedHours = totalWorkedHours;
            NumberOfRecords = numberOfRecords;
        }
    }
}
