using OfficeOpenXml;
using OfficeOpenXml.Table;
using PostOffice.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
                    ws.Cells["A1"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);
                    ws.Column(8).Style.Numberformat.Format = "dd/MM/yyyy";
                    ws.Cells.AutoFitColumns();
                    pck.Save();
                }
            });
        }

        public static Task StatisticXls<T>(List<T> datasource, string filePath, statisticReportViewModel vm)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));
                    #region templateInfo
                    ws.Cells["A1:H1"].Merge = true;
                    ws.Cells["A1:H1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM \n BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Cells["A1:H1"].Style.WrapText = true;
                    ws.Cells["A3:H3"].Merge = true;
                    ws.Cells["A3:H3"].Value = "BÁO CÁO TỔNG HỢP";
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Height = 35;
                    ws.Row(1).Style.Font.Bold = true;
                    ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(3).Style.Font.Size = 13;
                    ws.Row(3).Style.Font.Bold = true;

                    ws.Cells["A4:B4"].Merge = true;
                    ws.Row(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(4).Style.Font.Bold = true;
                    ws.Cells["A4:B4"].Value = "Đơn vị báo cáo:";
                    ws.Cells["A4:B4"].Style.Indent = 1;

                    ws.Cells["A5:B5"].Merge = true;
                    ws.Row(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(5).Style.Font.Bold = true;
                    ws.Cells["A5:B5"].Value = "Thời gian báo cáo:";
                    ws.Cells["A5:B5"].Style.Indent = 1;

                    ws.Cells["A6:B6"].Merge = true;
                    ws.Row(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(6).Style.Font.Bold = true;
                    ws.Cells["A6:B6"].Value = "Dịch vụ báo cáo:";
                    ws.Cells["A6:B6"].Style.Indent = 1;

                    // Custom fill
                    ws.Cells["C4:H4"].Merge = true;
                    ws.Cells["C4:H4"].Style.Font.Bold = true;
                    ws.Cells["C4:H4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C4:H4"].Style.Indent = 2;
                    ws.Cells["C4:H4"].Value = vm.PoName;

                    ws.Cells["C5:H5"].Merge = true;
                    ws.Cells["C5:H5"].Style.Font.Bold = true;
                    ws.Cells["C5:H5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C5:H5"].Style.Indent = 2;
                    ws.Cells["C5:H5"].Value = "Từ "+ vm.fromDate + " đến "+ vm.toDate;

                    ws.Cells["C6:H6"].Merge = true;
                    ws.Cells["C6:H6"].Style.Font.Bold = true;
                    ws.Cells["C6:H6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C6:H6"].Style.Indent = 2;
                    ws.Cells["C6:H6"].Value = vm.ServiceName;

                    #endregion
                    int noRow = datasource.Count;
                     
                    ws.Cells["A8"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);
                    ws.Column(8).Style.Numberformat.Format = "dd/MM/yyyy";
                    ws.Cells.AutoFitColumns();

                    ws.Cells[noRow + 10, 6, noRow + 10, 8].Merge = true;
                    ws.Cells[noRow + 10, 6, noRow + 10, 8].Value = "Người lập báo cáo";
                    ws.Cells[noRow + 10, 6, noRow + 10, 8].Style.Font.Bold = true;
                    ws.Cells[noRow + 10, 6, noRow + 10, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 14, 6, noRow + 14, 8].Merge = true;
                    ws.Cells[noRow + 14, 6, noRow + 14, 8].Value = vm.UserName;
                    ws.Cells[noRow + 14, 6, noRow + 14, 8].Style.Font.Bold = true;
                    ws.Cells[noRow + 14, 6, noRow + 14, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 16, 6, noRow + 16, 8].Merge = true;
                    ws.Cells[noRow + 16, 6, noRow + 16, 8].Value = DateTime.Now;
                    ws.Cells[noRow + 16, 6, noRow + 16, 8].Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                    ws.Cells[noRow + 16, 6, noRow + 16, 8].Style.Font.Italic = true;
                    ws.Cells[noRow + 16, 6, noRow + 16, 8].Style.Font.Size = 10;
                    pck.Save();
                }
            });
        }
    }
}