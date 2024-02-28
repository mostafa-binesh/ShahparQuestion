using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EmployeeRecord
    {
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan EntryTime { get; set; }
        public TimeSpan ExitTime { get; set; }

        public EmployeeRecord(DateTime date, string employeeId, string name, TimeSpan entryTime, TimeSpan exitTime)
        {
            Date = date;
            EmployeeID = employeeId;
            Name = name;
            EntryTime = entryTime;
            ExitTime = exitTime;
        }
    }
}
