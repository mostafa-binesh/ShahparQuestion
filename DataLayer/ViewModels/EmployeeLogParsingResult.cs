using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.ViewModels
{
    public class EmployeeLogParsingResult
    {
        public List<EmployeeLog> Records { get; set; } = new List<EmployeeLog>();
        public List<string> Errors { get; set; } = new List<string>();
    }
}
