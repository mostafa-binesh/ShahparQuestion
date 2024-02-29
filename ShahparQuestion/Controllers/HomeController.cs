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
            try
            {
                if (file != null && file.Length > 0)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    // file extension validation
                    if (!_checkValidation.IsTextExtension(fileExtension))
                    {
                        fileUploadResult.ErrorMessage = "فایل آپلودی باید متنی با فرمت .txt باشد.";
                        TempData["fileUploadResult"] = JsonConvert.SerializeObject(fileUploadResult);
                        return RedirectToAction(nameof(Index));
                    }

                    fileUploadResult.FileUploaded = true;

                    // retreieve employee log information using its service
                    _textFileEmployeeLogRetrieverService.SetFile(file);
                    var employeeLogResult = await _textFileEmployeeLogRetrieverService.GetEmployeeLogs();

                    // set the structure error from log service
                    fileUploadResult.StructureError = employeeLogResult.Errors;

                    // check for validation errors 
                    var validationResult = _employeeRecordsValidationService.Validate(employeeLogResult.Records);

                    // set the validation errors
                    fileUploadResult.ValidationErrors = validationResult.Errors;

                    // if any validation or structure error was exist, return errror
                    if (!fileUploadResult.ValidationSuccessful)
                    {
                        TempData["fileUploadResult"] = JsonConvert.SerializeObject(fileUploadResult);
                        return RedirectToAction(nameof(Index));
                    }

                    // convert employee logs to daily-summary logs
                    var summaryLogs = _logTimeProcessorService.ProcessLogs(employeeLogResult.Records);

                    // generate excel report
                    _generateExcelReportService.GenerateReport(summaryLogs);

                    var excelFileStream = _generateExcelReportService.ConvertXMlBookToStream();

                    // return generated file to the user
                    return File(excelFileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
                }
                else
                {
                    fileUploadResult.ErrorMessage = "لطفا فایلی آپلود کنید.";
                    TempData["fileUploadResult"] = JsonConvert.SerializeObject(fileUploadResult);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                fileUploadResult.ErrorMessage = "خطایی پیش آمد. لطفا از درست بودن ساختار فایل خود اطمینان حاصل کنید.";
                TempData["fileUploadResult"] = JsonConvert.SerializeObject(fileUploadResult);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
