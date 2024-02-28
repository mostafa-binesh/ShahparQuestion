using ShahparQuestion.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShahparQuestion.Interfaces
{
    public interface ITimeLogProcessorService
    {
        IEnumerable<DailySummary> ProcessLogs(IEnumerable<EmployeeLog> records);

    }
}
