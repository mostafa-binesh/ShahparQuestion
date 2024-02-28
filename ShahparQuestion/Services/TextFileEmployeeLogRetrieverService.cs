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
    internal class TextFileEmployeeLogRetrieverService : IEmployeeLogRetrieverService
    {
        private string? _filePath;

        public void SetFilePath(string filePath)
        {
            _filePath = filePath;
        }

        public EmployeeLogParsingResult GetEmployeeLogs()
        {
            var result = new EmployeeLogParsingResult();

            if (string.IsNullOrEmpty(_filePath))
            {
                throw new Exception(message: "filePath should be provided by using SetFilePath function");
            }

            var lines = File.ReadAllLines(_filePath);

            foreach (var line in lines)
            {
                var parts = line.Split('\t');
                if (parts.Length < 3)
                {
                    result.Errors.Add($"داده ی خط {line} از فرمت درست پیروی نمی کند");
                    continue;
                }

                var employeeId = parts[0].Trim();
                var name = parts[1].Trim(); 
                var dateTimePart = parts[2].Trim();

                // Attempt to parse the DateTime part
                if (!DateTime.TryParse(dateTimePart, out var logDateTime))
                {
                    result.Errors.Add($"تاریخ و زمان {dateTimePart} در خط {line} نامعتبر است");
                    continue;
                }
                var groupedByEmployeeAndDate = result.Records
                        .GroupBy(r => new { r.EmployeeID, r.LogDateTime.Date })
                .ToList();

                //foreach (var group in groupedByEmployeeAndDate)
                //{
                //    if (group.Count() % 2 == 1)
                //    {
                //        result.Errors.Add($"کاربر {group.Key.EmployeeID} تعداد فردی رکورد ورود و خروج در تاریخ {group.Key.Date:d} دارد. تعداد کل رکورد های این روز کاربر:: {group.Count()}");
                //        continue;
                //    }
                //}

            }
            return result;
        }
    }
}
