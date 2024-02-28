namespace ShahparQuestion.Services;

using ClosedXML.Excel;
using ShahparQuestion.ViewModels;
using System;
using System.Collections.Generic;
//using System.Globalization;
using ShahparQuestion.Infrastructure;
using ShahparQuestion.Interfaces;

public class GenerateExcelReportService : IReportGeneratorService
{
    public void GenerateReport(IEnumerable<DailySummary> summaries, string filePath)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.AddWorksheet("Attendance Report");
            var currentRow = 1;

            // headers
            worksheet.Cell(currentRow, 1).Value = "ردیف";
            worksheet.Cell(currentRow, 2).Value = "تاریخ";
            worksheet.Cell(currentRow, 3).Value = "روز";
            worksheet.Cell(currentRow, 4).Value = "نام فرد";
            worksheet.Cell(currentRow, 5).Value = "نوع کارکرد";
            worksheet.Cell(currentRow, 6).Value = "اولین ورود";
            worksheet.Cell(currentRow, 7).Value = "اخرین خروج";
            worksheet.Cell(currentRow, 8).Value = "رکوردها";

            // data
            foreach (var summary in summaries)
            {
                currentRow++;
                var date = summary.Date;
                var shamsiDate = PersianConventor.ToShamsi(date);
                bool hasError = false;
                if (summary.AttendanceTypes.Where(at => at == AttendanceType.Error).Any()) hasError = true;

                worksheet.Cell(currentRow, 1).Value = currentRow - 1;
                worksheet.Cell(currentRow, 2).Value = shamsiDate;
                worksheet.Cell(currentRow, 3).Value = PersianConventor.ToDayOfWeek(date.DayOfWeek);
                worksheet.Cell(currentRow, 4).Value = summary.Name;
                if (hasError)
                {
                    worksheet.Cell(currentRow, 5).Value = AttendanceType.Error.ToPersianString();
                    worksheet.Cell(currentRow, 6).Value = "-";
                    worksheet.Cell(currentRow, 7).Value = "-";
                    worksheet.Cell(currentRow, 8).Value = "-";
                    worksheet.Cell(currentRow, 9).Value = string.Join(", ", summary.TimeRecords.Select(tr => tr.ToString(@"hh\:mm")));
                }
                else
                {
                    worksheet.Cell(currentRow, 5).Value = string.Join(",", summary.AttendanceTypes.Select(at => at.ToPersianString()));
                    worksheet.Cell(currentRow, 6).Value = summary.TotalWorkedHours;
                    worksheet.Cell(currentRow, 7).Value = summary.FirstEntry.ToString(@"hh\:mm");
                    worksheet.Cell(currentRow, 8).Value = summary.LastExit.ToString(@"hh\:mm");
                    worksheet.Cell(currentRow, 9).Value = string.Join(", ", summary.TimeRecords.Select(tr => tr.ToString(@"hh\:mm")));
                }
            }

            workbook.SaveAs(filePath);
        }
    }
}
