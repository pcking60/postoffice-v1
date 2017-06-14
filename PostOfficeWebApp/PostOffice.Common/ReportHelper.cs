using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PostOffice.Common
{
    public static class ReportHelper
    {
        public static Task GenerateXls<T>(List<T> datasource, string filePath)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));
                    ws.Cells["A1:H1"].Merge = true;
                    ws.Cells["A2:H2"].Merge = true;
                    ws.Cells["A1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM";
                    ws.Cells["A2"].Value = "BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Cells["A4"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);
                    ws.Column(8).Style.Numberformat.Format = "dd/MM/yyyy";
                    ws.Cells.AutoFitColumns();
                    pck.Save();
                }
            });
        }
        

    }
}