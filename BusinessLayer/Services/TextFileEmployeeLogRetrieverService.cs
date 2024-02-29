using BusinessLayer.Interfaces;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Services
{
    public class TextFileEmployeeLogRetrieverService : ITextFileEmployeeLogRetrieverService
    {
        private IFormFile _file;

        public void SetFile(IFormFile file)
        {
            _file = file;
        }

        public async Task<EmployeeLogParsingResult> GetEmployeeLogs()
        {
            var result = new EmployeeLogParsingResult();

            if (_file == null || _file.Length == 0)
            {
                throw new Exception("A file must be provided.");
            }

            // read the file line by line
            using (var stream = _file.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    // split the data by space and \t
                    string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length < 4)
                    {
                        result.Errors.Add($"داده ی خط {line} از فرمت درست پیروی نمی کند");
                        continue;
                    }

                    // extract the information from seperated parts
                    var employeeId = parts[0].Trim();
                    var name = parts[1].Trim();
                    var dateTimePart = parts[2].Trim() + " " + parts[3].Trim();

                    // Attempt to parse the DateTime part
                    if (!DateTime.TryParse(dateTimePart, out var logDateTime))
                    {
                        result.Errors.Add($"تاریخ و زمان {dateTimePart} در خط {line} نامعتبر است");
                    }

                    var record = new EmployeeLog
                    {
                        EmployeeID = employeeId,
                        Name = name,
                        LogDateTime = logDateTime,
                    };
                    result.Records.Add(record);
                };
            }
            return result;
        }
    }
}
