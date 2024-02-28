using ShahparQuestion.Interfaces;
using ShahparQuestion.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    internal class EmployeeRecordsValidationService : IEmployeeRecordsValidationService
    {
        public ValidationResult Validate(IEnumerable<EmployeeLog> records)
        {
            var result = new ValidationResult();

            // Validate that each employeeId is 9 digits
            foreach (var record in records)
            {
                if (record.EmployeeID.Length != 9 || !record.EmployeeID.All(char.IsDigit))
                {
                    //results.Add(new ValidationResult(false, ));
                    result.Errors.Add($"آیدی کارمند '{record.EmployeeID}' دقیقا باید به صورت عدد و 9 رقمی باشد.");
                }
            }

            // Validate that there is no employeeId with two names
            var employeeIdNamePairs = records
                .GroupBy(r => r.EmployeeID)
                .Select(g => new { EmployeeID = g.Key, Names = g.Select(x => x.Name).Distinct() });

            foreach (var pair in employeeIdNamePairs)
            {
                if (pair.Names.Count() > 1)
                {
                    result.Errors.Add($"کارمند با آیدی '{pair.EmployeeID}' با نام های متفاوت وجود دارد.");
                }
            }
            return result;
        }
    }

}
