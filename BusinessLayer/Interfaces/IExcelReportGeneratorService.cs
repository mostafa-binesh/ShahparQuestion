namespace BusinessLayer.Interfaces
{
    public interface IExcelReportGeneratorService : IReportGeneratorService
    {
        MemoryStream ConvertXMlBookToStream();
    }
}