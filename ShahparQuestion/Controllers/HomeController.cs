using Microsoft.AspNetCore.Mvc;
using DataLayer.ViewModels;
using BusinessLayer.Services;
using BusinessLayer.Interfaces;
using ShahparQuestion.Infrastructure;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json; // Ensure Newtonsoft.Json is installed

namespace ShahparQuestion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITextFileEmployeeLogRetrieverService _textFileEmployeeLogRetrieverService;
        private readonly ITimeLogProcessorService _logTimeProcessorService;
        private readonly IExcelReportGeneratorService _generateExcelReportService;
        private readonly IEmployeeRecordsValidationService _employeeRecordsValidationService;
        private readonly CheckValidation _checkValidation;

        public HomeController(ITextFileEmployeeLogRetrieverService textFileEmployeeLogRetrieverService, ITimeLogProcessorService logTimeProcessorService, IExcelReportGeneratorService generateExcelReportService, IEmployeeRecordsValidationService employeeRecordsValidationService)
        {
            _textFileEmployeeLogRetrieverService = textFileEmployeeLogRetrieverService;
            _logTimeProcessorService = logTimeProcessorService;
            _generateExcelReportService = generateExcelReportService;
            _employeeRecordsValidationService = employeeRecordsValidationService;
            _checkValidation = new CheckValidation();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var fileUploadResult = new DataLayer.ViewModels.UploadFileResult();
            if (TempData["fileUploadResult"] != null)
            {
                fileUploadResult = JsonConvert.DeserializeObject<UploadFileResult>((string)TempData["fileUploadResult"]);
            }
            return View(fileUploadResult);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            var fileUploadResult = new DataLayer.ViewModels.UploadFileResult();
            if (file != null && file.Length > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                if (!_checkValidation.IsTextExtension(fileExtension))
                {
                    fileUploadResult.ErrorMessage = "فایل آپلودی باید متنی با فرمت .txt باشد.";
                    TempData["fileUploadResult"] = JsonConvert.SerializeObject(fileUploadResult);
                    return RedirectToAction(nameof(Index));
                }

                fileUploadResult.FileUploaded = true;

                _textFileEmployeeLogRetrieverService.SetFile(file);
                var employeeLogResult = await _textFileEmployeeLogRetrieverService.GetEmployeeLogs();

                fileUploadResult.StructureError = employeeLogResult.Errors;

                // validation
                var validationResult = _employeeRecordsValidationService.Validate(employeeLogResult.Records);

                fileUploadResult.ValidationErrors = validationResult.Errors;

                if (!fileUploadResult.ValidationSuccessful)
                {
                    TempData["fileUploadResult"] = JsonConvert.SerializeObject(fileUploadResult);
                    return RedirectToAction(nameof(Index));
                }

                var summaryLogs = _logTimeProcessorService.ProcessLogs(employeeLogResult.Records);

                _generateExcelReportService.GenerateReport(summaryLogs);

                var excelFileStream = _generateExcelReportService.ConvertXMlBookToStream();

                return File(excelFileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
            }
            else
            {
                fileUploadResult.ErrorMessage = "لطفا فایلی آپلود کنید.";
                TempData["fileUploadResult"] = JsonConvert.SerializeObject(fileUploadResult);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
