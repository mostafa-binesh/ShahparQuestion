using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    internal class InputFileParserService : IInputParserService
    {
        public IEnumerable<EmployeeRecord> ParseInputFile(string filePath)
        {
            var records = new List<EmployeeRecord>();
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length < 5) continue; // Skip invalid lines

                try
                {
                    var date = DateTime.ParseExact(parts[0], "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    var employeeId = parts[1].Trim();
                    var name = parts[2].Trim();
                    var entryTime = TimeSpan.Parse(parts[3].Trim());
                    var exitTime = TimeSpan.Parse(parts[4].Trim());

                    if (employeeId.Length == 9) // Validate employee ID length
                    {
                        var record = new EmployeeRecord(date, employeeId, name, entryTime, exitTime);
                        records.Add(record);
                    }
                }
                catch (FormatException) // Handle parsing errors
                {
                    // Log or handle the error as needed
                    continue;
                }
            }

            return records;
        }
    }
}
