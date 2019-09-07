using System.Collections.Generic;
using System.Net.Http;

namespace GenericExcelExport.ExcelExport
{
    public interface IExcelExport
    {
        HttpResponseMessage Export<T>(List<T> exportData, string fileName,
            bool appendDateTimeInFileName = false, string sheetName = Utility.DEFAULT_SHEET_NAME);
    }
}
