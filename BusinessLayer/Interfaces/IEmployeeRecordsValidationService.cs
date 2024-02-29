using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IEmployeeRecordsValidationService
    {
        ValidationResult Validate(IEnumerable<EmployeeLog> record);
    }
}
