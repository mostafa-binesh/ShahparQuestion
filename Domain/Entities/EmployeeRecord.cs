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
        public DateTime LogDateTime { get; set; } // logDate and logTime combined together

        public EmployeeRecord(string employeeId, string name, DateTime logDateTime)
        {
            EmployeeID = employeeId;
            Name = name;
            LogDateTime = logDateTime;
        }
    }
}
