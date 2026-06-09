using ClosedXML.Excel;
using GymManagement.Application.Interfaces;
using System.Reflection;

namespace GymManagement.Infrastructure.Exports;

public class ExcelExportService : IExcelExportService
{
    public byte[] ExportToExcel<T>(
        IEnumerable<T> data,
        string worksheetName)
    {
        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add(worksheetName);

        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        for (var column = 0; column < properties.Length; column++)
        {
            worksheet.Cell(1, column + 1).Value = properties[column].Name;
            worksheet.Cell(1, column + 1).Style.Font.Bold = true;
        }

        var row = 2;

        foreach (var item in data)
        {
            for (var column = 0; column < properties.Length; column++)
            {
                var value = properties[column].GetValue(item);

                worksheet.Cell(row, column + 1).Value = value?.ToString() ?? string.Empty;
            }

            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();

        workbook.SaveAs(stream);

        return stream.ToArray();
    }
}