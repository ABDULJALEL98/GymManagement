namespace GymManagement.Application.Interfaces;

public interface IExcelExportService
{
    byte[] ExportToExcel<T>(
        IEnumerable<T> data,
        string worksheetName);
}