using OfficeOpenXml;
using OfficeOpenXml.Table;
using PostOffice.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public static Task StatisticXls<T>(List<T> datasource, string filePath, statisticReportViewModel vm)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));

                    #region templateInfo

                    ws.Cells["A1:E1"].Merge = true;
                    ws.Cells["A1:E1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM \n BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Cells["A1:E1"].Style.WrapText = true;
                    ws.Cells["A3:E3"].Merge = true;
                    ws.Cells["A3:E3"].Value = "BÁO CÁO TỔNG HỢP";
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Height = 35;
                    ws.Row(1).Style.Font.Bold = true;
                    ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(3).Style.Font.Size = 13;
                    ws.Row(3).Style.Font.Bold = true;

                    // Custom fill
                    ws.Cells["C4:E4"].Merge = true;
                    ws.Cells["C4:E4"].Style.Font.Bold = true;
                    ws.Cells["C4:E4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C4:E4"].Style.Indent = 2;
                    ws.Cells["C4:E4"].Value = vm.PoName;

                    ws.Cells["C5:E5"].Merge = true;
                    ws.Cells["C5:E5"].Style.Font.Bold = true;
                    ws.Cells["C5:E5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C5:E5"].Style.Indent = 2;
                    ws.Cells["C5:E5"].Value = "Từ " + vm.fromDate + " đến " + vm.toDate;

                    ws.Cells["C6:E6"].Merge = true;
                    ws.Cells["C6:E6"].Style.Font.Bold = true;
                    ws.Cells["C6:E6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C6:E6"].Style.Indent = 2;
                    ws.Cells["C6:E6"].Value = vm.ServiceName;

                    #endregion templateInfo

                    int noRow = datasource.Count;

                    ws.Cells["A8"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);
                    ws.Column(8).Style.Numberformat.Format = "dd/MM/yyyy";
                    ws.Cells.AutoFitColumns();

                    ws.Cells[noRow + 10, 3, noRow + 10, 5].Merge = true;
                    ws.Cells[noRow + 10, 3, noRow + 10, 5].Value = "Người lập báo cáo";
                    ws.Cells[noRow + 10, 3, noRow + 10, 5].Style.Font.Bold = true;
                    ws.Cells[noRow + 10, 3, noRow + 10, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 14, 3, noRow + 14, 5].Merge = true;
                    ws.Cells[noRow + 14, 3, noRow + 14, 5].Value = vm.UserName;
                    ws.Cells[noRow + 14, 3, noRow + 14, 5].Style.Font.Bold = true;
                    ws.Cells[noRow + 14, 3, noRow + 14, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Merge = true;
                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Value = DateTime.Now;
                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Style.Font.Italic = true;
                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Style.Font.Size = 10;
                    pck.Save();
                }
            });
        }

        /*
            code: RP1
            name: Bảng kê tổng hợp thu tiền tại bưu cục
        */

        public static Task RP1<T>(List<T> datasource, string filePath, ReportTemplate vm, IEnumerable<RP1Advance> rp1)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));

                    #region templateInfo

                    //header
                    ws.Cells["A1:E1"].Merge = true;
                    ws.Cells["A1:E1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM \n BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Height = 35;
                    ws.Row(1).Style.Font.Bold = true;
                    ws.Row(1).Style.Font.Size = 15;

                    //functionName
                    ws.Cells["A1:E1"].Style.WrapText = true;
                    ws.Cells["A3:E3"].Merge = true;
                    ws.Cells["A3:E3"].Formula = "upper(\"" +vm.FunctionName.ToString() +"\")";

                    ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(3).Style.Font.Size = 13;
                    ws.Row(3).Style.Font.Bold = true;

                    // Custom fill
                    //district
                    ws.Cells["C4:E4"].Merge = true;
                    ws.Cells["C4:E4"].Style.Font.Bold = true;
                    ws.Cells["C4:E4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C4:E4"].Style.Indent = 2;
                    if (vm.District == null)
                    {
                        vm.District = "Tất cả";
                    }
                    ws.Cells["C4:E4"].Value = vm.District;

                    //unit
                    ws.Cells["C5:E5"].Merge = true;
                    ws.Cells["C5:E5"].Style.Font.Bold = true;
                    ws.Cells["C5:E5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C5:E5"].Style.Indent = 2;
                    if (vm.Unit == null)
                    {
                        vm.Unit = "Tất cả";
                    }
                    ws.Cells["C5:e5"].Value = vm.Unit;
                    
                    //time
                    ws.Cells["C6:E6"].Merge = true;
                    ws.Cells["C6:E6"].Style.Font.Bold = true;
                    ws.Cells["C6:E6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C6:E6"].Style.Indent = 2;
                    ws.Cells["C6:E6"].Value = "Từ " + vm.FromDate.ToString("dd/MM/yyyy") + " đến " + vm.ToDate.ToString("dd/MM/yyyy"); ;

                    #endregion templateInfo

                    int noRow = datasource.Count;

                    // load data
                    ws.Cells["A8"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);

                    //header
                    ws.Cells["A8"].Value = "STT";
                    ws.Cells["B8"].Value = "Nhóm dịch vụ";
                    ws.Cells["C8"].Value = "Doanh thu";
                    ws.Cells["D8"].Value = "Thuế";
                    ws.Cells["E8"].Value = "Tổng cộng";
                    ws.Cells["A8:E8"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A8:E8"].Style.Font.Bold = true;
                    ws.Cells[8, 1, 8, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[8, 1, 8, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));

                    ws.Column(8).Style.Numberformat.Format = "dd/MM/yyyy";
                    ws.Cells.AutoFitColumns();    
                                  
                    //format col 1
                    ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;                    
                    
                    //format col 3,4,5
                    //ws.Cells[9, 3, noRow + 10, 8].Style.Numberformat.Format = "#,##0.00";

                    //sum part 1
                    ws.Cells[noRow + 9, 2].Value = "Tổng cộng doanh thu";
                    ws.Cells[noRow + 9, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(noRow + 9).Style.Font.Bold = true;
                    ws.Cells[noRow + 9, 3].Formula = "sum(c9:c" + (noRow + 8) + ")";
                    ws.Cells[noRow + 9, 4].Formula = "sum(d9:c" + (noRow + 8) + ")";
                    ws.Cells[noRow + 9, 5].Formula = "sum(e9:c" + (noRow + 8) + ")";

                    //part 2
                    ws.Cells[noRow + 11, 2].Value = "Tiền giữ hộ";
                    ws.Cells[noRow + 11, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[noRow + 11, 2].Style.Font.Bold = true;
                    ws.Cells[noRow + 12, 1].Value = "1";
                    ws.Cells[noRow + 13, 1].Value = "2";
                    ws.Cells[noRow + 12, 2].Value = "Phụ thu nước ngoài";
                    ws.Cells[noRow + 12, 1, noRow + 12, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[noRow + 12, 1, noRow + 12, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));
                    ws.Cells[noRow + 13, 2].Value = "EMS Ngoại giao công vụ";

                    //fill data
                    int i = 12;
                    int j = 3;
                    foreach (var item in rp1)
                    {
                        ws.Cells[noRow + i, j].Value = item.Revenue;
                        ws.Cells[noRow + i, j + 1].Value = item.Tax;
                        ws.Cells[noRow + i, j + 2].Value = item.TotalMoney;
                        i++;
                    }
                    //format col 3,4,5
                    ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                    ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.00";                    

                    //sum part 2
                    ws.Cells[noRow + 14, 2].Value = "Tổng tiền thu hộ";
                    ws.Cells[noRow + 14, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(noRow+14).Style.Font.Bold = true;
                    ws.Cells[noRow + 14, 3].Formula = "sum(c" + (noRow + 12) + ":c" + (noRow + 13) + ")";
                    ws.Cells[noRow + 14, 4].Formula = "sum(d" + (noRow + 12) + ":d" + (noRow + 13) + ")";
                    ws.Cells[noRow + 14, 5].Formula = "sum(e" + (noRow + 12) + ":e" + (noRow + 13) + ")";

                    // ------Tổng thu---------
                    ws.Cells[noRow + 15, 2].Value = "TỔNG THU";
                    ws.Cells[noRow + 15, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(noRow + 15).Style.Font.Bold = true;
                    ws.Cells[noRow + 15, 3].Formula = "C" + (noRow + 9) + "+" + "C" + (noRow + 14);
                    ws.Cells[noRow + 15, 4].Formula = "D" + (noRow + 9) + "+" + "D" + (noRow + 14);
                    ws.Cells[noRow + 15, 5].Formula = "E" + (noRow + 9) + "+" + "E" + (noRow + 14);                    

                    #region template 2

                    //info
                    ws.Cells["A4:B4"].Merge = true;
                    ws.Cells["A4:B4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(4).Style.Font.Bold = true;
                    ws.Cells["A4:B4"].Value = "Huyện: ";
                    ws.Cells["A4:B4"].Style.Indent = 1;

                    ws.Cells["A5:B5"].Merge = true;
                    ws.Cells["A5:B5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(5).Style.Font.Bold = true;
                    ws.Cells["A5:B5"].Value = "Bưu cục: ";
                    ws.Cells["A5:B5"].Style.Indent = 1;

                    ws.Cells["A6:B6"].Merge = true;
                    ws.Cells["A6:B6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(6).Style.Font.Bold = true;
                    ws.Cells["A6:B6"].Value = "Thời gian:";
                    ws.Cells["A6:B6"].Style.Indent = 1;

                    //fix width
                    ws.Column(1).Width = 6;
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 18;
                    ws.Column(4).Width = 14;
                    ws.Column(5).Width = 20;

                    //border table
                    ws.Cells[8, 1, noRow + 15, 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[8, 1, noRow + 15, 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[8, 1, noRow + 15, 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[8, 1, noRow + 15, 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    //signal
                    ws.Cells[noRow + 17, 1, noRow + 17, 2].Merge = true;
                    ws.Cells[noRow + 17, 1, noRow + 17, 2].Value = "Người lập bảng";
                    ws.Cells[noRow + 17, 1, noRow + 17, 2].Style.Font.Bold = true;
                    ws.Cells[noRow + 17, 1, noRow + 17, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 21, 1, noRow + 21, 2].Merge = true;
                    ws.Cells[noRow + 21, 1, noRow + 21, 2].Value = vm.CreatedBy;
                    ws.Cells[noRow + 21, 1, noRow + 21, 2].Style.Font.Bold = true;
                    ws.Cells[noRow + 21, 1, noRow + 21, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 17, 3, noRow + 17, 5].Merge = true;
                    ws.Cells[noRow + 17, 3, noRow + 17, 5].Value = "Người phê duyệt";
                    ws.Cells[noRow + 17, 3, noRow + 17, 5].Style.Font.Bold = true;
                    ws.Cells[noRow + 17, 3, noRow + 17, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Merge = true;
                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Value = DateTime.Now;
                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Style.Font.Italic = true;
                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Style.Font.Size = 10;
                    #endregion template 2

                    pck.Save();
                }
            });
        }
    }
}