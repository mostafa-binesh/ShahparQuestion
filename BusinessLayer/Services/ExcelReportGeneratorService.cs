namespace BusinessLayer.Services;

using ClosedXML.Excel;
using System.Collections.Generic;
using ShahparQuestion.Infrastructure;
using BusinessLayer.Interfaces;
using DataLayer.ViewModels;
using DocumentFormat.OpenXml.Spreadsheet;

public class ExcelReportGeneratorService : IExcelReportGeneratorService
{
    private XLWorkbook _xmlBook;
    public void GenerateReport(IEnumerable<DailySummary> summaries)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.AddWorksheet("Attendance Report");
        var currentRow = 1;

        // headers
        worksheet.Cell(currentRow, 1).Value = "ردیف";
        worksheet.Cell(currentRow, 2).Value = "تاریخ";
        worksheet.Cell(currentRow, 3).Value = "روز";
        worksheet.Cell(currentRow, 4).Value = "نام فرد";
        worksheet.Cell(currentRow, 5).Value = "نوع کارکرد";
        worksheet.Cell(currentRow, 6).Value = "کارکرد";
        worksheet.Cell(currentRow, 7).Value = "اولین ورود";
        worksheet.Cell(currentRow, 8).Value = "اخرین خروج";
        worksheet.Cell(currentRow, 9).Value = "رکوردها";

        // data
        foreach (var summary in summaries)
        {
            currentRow++;
            var date = summary.Date;
            var shamsiDate = PersianConventor.ToShamsi(date);
            bool canPrintTimeData = true;
            if (summary.AttendanceTypes.Where(at => at == AttendanceType.Error || at == AttendanceType.Full_Day_Leave).Any()) canPrintTimeData = false;

            worksheet.Cell(currentRow, 1).Value = currentRow - 1;
            worksheet.Cell(currentRow, 2).Value = shamsiDate;
            worksheet.Cell(currentRow, 3).Value = PersianConventor.ToDayOfWeek(date.DayOfWeek);
            worksheet.Cell(currentRow, 4).Value = summary.Name;
            if (!canPrintTimeData)
            {
                worksheet.Cell(currentRow, 5).Value = summary.AttendanceTypes.First().ToPersianString();
                worksheet.Cell(currentRow, 6).Value = "-";
                worksheet.Cell(currentRow, 7).Value = "-";
                worksheet.Cell(currentRow, 8).Value = "-";
            }
            else
            {
                worksheet.Cell(currentRow, 5).Value = string.Join(",", summary.AttendanceTypes.Select(at => at.ToPersianString()));
                worksheet.Cell(currentRow, 6).Value = summary.TotalWorkedHours;
                worksheet.Cell(currentRow, 7).Value = summary.FirstEntry.ToString(@"hh\:mm");
                worksheet.Cell(currentRow, 8).Value = summary.LastExit.ToString(@"hh\:mm");
            }
            worksheet.Cell(currentRow, 9).Value = string.Join(", ", summary.TimeRecords.Select(tr => tr.ToString(@"hh\:mm")));
        }

        _xmlBook = workbook;
    }
    public MemoryStream ConvertXMlBookToStream()
    {
        if (_xmlBook == null)
            throw new InvalidOperationException("The report has not been generated yet. Please generate it using GenerateReport function first.");

        var stream = new MemoryStream();
        _xmlBook.SaveAs(stream);
        stream.Position = 0; // Reset the stream position to the beginning
        _xmlBook.Dispose();
        return stream;
    }
}
