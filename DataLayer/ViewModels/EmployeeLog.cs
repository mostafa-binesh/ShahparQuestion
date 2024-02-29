using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class EmployeeLog
    {
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public DateTime LogDateTime { get; set; } // logDate and logTime combined together
    }
}
