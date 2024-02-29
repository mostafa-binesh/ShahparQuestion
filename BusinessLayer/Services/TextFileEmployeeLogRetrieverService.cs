using BusinessLayer.Interfaces;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Services
{
    public class TextFileEmployeeLogRetrieverService : ITextFileEmployeeLogRetrieverService
    {
        //private string? _filePath;
        private IFormFile _file;

        public void SetFile(IFormFile file)
        {
            //_filePath = filePath;
            _file = file;
        }

        public async Task<EmployeeLogParsingResult> GetEmployeeLogs()
        {
            var result = new EmployeeLogParsingResult();

            //if (string.IsNullOrEmpty(_filePath))
            //{
            //    throw new Exception(message: "filePath should be provided by using SetFilePath function");
            //}
            if (_file == null || _file.Length == 0)
            {
                throw new Exception("A file must be provided.");
            }

            //var lines = File.ReadAllLines(_filePath);

            //foreach (var line in lines)
            //{
            using (var stream = _file.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var parts = line.Split('\t');
                    if (parts.Length < 3)
                    {
                        result.Errors.Add($"داده ی خط {line} از فرمت درست پیروی نمی کند");
                        //continue;
                    }

                    var employeeId = parts[0].Trim();
                    var name = parts[1].Trim();
                    var dateTimePart = parts[2].Trim();

                    // Attempt to parse the DateTime part
                    if (!DateTime.TryParse(dateTimePart, out var logDateTime))
                    {
                        result.Errors.Add($"تاریخ و زمان {dateTimePart} در خط {line} نامعتبر است");
                        //continue;
                    }
                    //var groupedByEmployeeAndDate = result.Records
                    //        .GroupBy(r => new { r.EmployeeID, r.LogDateTime.Date })
                    //.ToList();

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
