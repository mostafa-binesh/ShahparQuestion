using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IEmployeeLogRetrieverService
    {
        Task<EmployeeLogParsingResult> GetEmployeeLogs();
    }
}
