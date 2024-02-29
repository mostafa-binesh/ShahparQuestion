
using BusinessLayer.Interfaces;
using DataLayer.ViewModels;
using ShahparQuestion.Infrastructure;

namespace BusinessLayer.Services
{
    public class LogTimeProcessorService : ITimeLogProcessorService
    {
        public IEnumerable<DailySummary> ProcessLogs(IEnumerable<EmployeeLog> records)
        {
            // groupBy data by their EmployeeId and logtime
            var groupedByEmployeeAndDate = records
                .GroupBy(r => new { r.EmployeeID, Date = r.LogDateTime.Date })
                .ToList();

            var summaries = new List<DailySummary>();
            var allRecords = records.ToList();
            var allDates = records.Select(r => r.LogDateTime.Date).Distinct();
            var allEmployeeIDs = records.Select(r => r.EmployeeID).Distinct();

            // check and creating full day leave records
            foreach (var date in allDates)
            {
                var employeesWithLogs = allRecords.Where(r => r.LogDateTime.Date == date).Select(r => r.EmployeeID).Distinct();

                // selecting employees without logs for this date
                var employeesWithoutLogs = allEmployeeIDs.Except(employeesWithLogs);

                foreach (var employeeID in employeesWithoutLogs)
                {
                    summaries.Add(new DailySummary
                    {
                        Date = date,
                        EmployeeID = employeeID,
                        Name = allRecords.Where(r => r.EmployeeID == employeeID).Select(r => r.Name).First(),
                        AttendanceTypes = new List<AttendanceType> { AttendanceType.Full_Day_Leave }
                    });
                }
            }

            foreach (var group in groupedByEmployeeAndDate)
            {
                var sortedRecords = group.OrderBy(r => r.LogDateTime).ToList();
                var date = group.Key.Date;

                // create a summary for the current group
                var summary = new DailySummary
                {
                    Date = date,
                    EmployeeID = group.Key.EmployeeID,
                    Name = sortedRecords.First().Name, // Assuming name doesn't change
                    NumberOfRecords = group.Count(),
                    TimeRecords = sortedRecords.Select(r => r.LogDateTime.TimeOfDay).ToList()
                };
                summary.AttendanceTypes = AttendanceTypeHelper.DetermineAttendanceType(sortedRecords, date, allRecords);

                // set first entry, last exit, and total worked hours for the summary
                if (sortedRecords.Any())
                {
                    summary.FirstEntry = sortedRecords.First().LogDateTime.TimeOfDay;
                    summary.LastExit = sortedRecords.Last().LogDateTime.TimeOfDay;
                    summary.TotalWorkedHours = sortedRecords.Last().LogDateTime - sortedRecords.First().LogDateTime;
                }

                summaries.Add(summary);
            }

            return summaries.OrderBy(s => s.Date).ThenBy(s => s.EmployeeID);
        }


    }

}
