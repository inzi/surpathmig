using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Abp.AspNetZeroCore.Net;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Runtime.Session;
using inzibackend.Dto;
using inzibackend.Storage;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace inzibackend.DataExporting.Excel.NPOI
{
    public abstract class NpoiExcelExporterBase : inzibackendServiceBase, ITransientDependency
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public IAbpSession AbpSession { get; set; } // Property injection for session

        private IWorkbook _workbook;

        private readonly Dictionary<string, ICellStyle> _dateCellStyles = new();
        private readonly Dictionary<string, IDataFormat> _dateDateDataFormats = new();

        private ICellStyle GetDateCellStyle(ICell cell, string dateFormat)
        {
            if (_workbook != cell.Sheet.Workbook)
            {
                _dateCellStyles.Clear();
                _dateDateDataFormats.Clear();
                _workbook = cell.Sheet.Workbook;
            }

            if (_dateCellStyles.ContainsKey(dateFormat))
            {
                return _dateCellStyles.GetValueOrDefault(dateFormat);
            }

            var cellStyle = cell.Sheet.Workbook.CreateCellStyle();
            _dateCellStyles.Add(dateFormat, cellStyle);
            return cellStyle;
        }

        private IDataFormat GetDateDataFormat(ICell cell, string dateFormat)
        {
            if (_workbook != cell.Sheet.Workbook)
            {
                _dateDateDataFormats.Clear();
                _workbook = cell.Sheet.Workbook;
            }

            if (_dateDateDataFormats.ContainsKey(dateFormat))
            {
                return _dateDateDataFormats.GetValueOrDefault(dateFormat);
            }

            var dataFormat = cell.Sheet.Workbook.CreateDataFormat();
            _dateDateDataFormats.Add(dateFormat, dataFormat);
            return dataFormat;
        }

        protected NpoiExcelExporterBase(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        protected NpoiExcelExporterBase(ITempFileCacheManager tempFileCacheManager, IAbpSession abpSession)
        {
            _tempFileCacheManager = tempFileCacheManager;
            AbpSession = abpSession;
        }

        //private XSSFCellStyle nonCompliantCellStyle { get; set; }
        //private XSSFCellStyle CompliantCellStyle { get; set; }

        private XSSFWorkbook workbook { get; set; }

        protected FileDto CreateExcelPackage(string fileName, Action<XSSFWorkbook> creator)
        {
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            workbook = new XSSFWorkbook();

            //nonCompliantCellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            //byte[] rgb = new byte[3] { 220, 170, 200 };
            //XSSFColor color = new XSSFColor(rgb);
            //nonCompliantCellStyle.FillBackgroundXSSFColor = color;

            //CompliantCellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            //byte[] rgb2 = new byte[3] { 170, 220, 200 };
            //XSSFColor color2 = new XSSFColor(rgb);
            //CompliantCellStyle.FillBackgroundXSSFColor = color2;

            //ICellStyle nonCompliantCellStyle = workbook.CreateCellStyle();

            //nonCompliantCellStyle.FillBackgroundColor = NPOI.XSSF.Util.HSSFColor.Plug.Index;
            //nonCompliantCellStyle.FillPattern = FillPattern.SolidForeground;

            //ICellStyle CompliantCellStyle = workbook.CreateCellStyle();
            //CompliantCellStyle.FillBackgroundColor = NPOI.XSSF.
            //// .XSSF.Util.HSSFColor.LIGHT_TURQUOISE.Index;
            //CompliantCellStyle.FillPattern = FillPattern.SolidForeground;

            creator(workbook);

            Save(workbook, file);

            return file;
        }

        protected void AddHeader(ISheet sheet, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            sheet.CreateRow(0);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i, headerTexts[i]);
            }
        }

        protected void AddHeader(ISheet sheet, int columnIndex, string headerText)
        {
            var cell = sheet.GetRow(0).CreateCell(columnIndex);
            cell.SetCellValue(headerText);
            var cellStyle = sheet.Workbook.CreateCellStyle();
            var font = sheet.Workbook.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = 12;
            cellStyle.SetFont(font);
            cell.CellStyle = cellStyle;
        }

        protected void AddObjects<T>(ISheet sheet, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            ////ICell cell = CurrentRow.CreateCell(CellIndex);
            ////cell.CellStyle = cellStyle;
            //ICellStyle style = workbook.CreateCellStyle();
            //style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
            //style.FillPattern = FillPattern.SolidForeground;

            XSSFCellStyle nonCompliantCellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            byte[] rgb = new byte[3] { 220, 170, 200 };
            XSSFColor color = new XSSFColor(rgb);
            nonCompliantCellStyle.FillBackgroundXSSFColor = color;
            nonCompliantCellStyle.FillPattern = FillPattern.SolidForeground;

            XSSFCellStyle CompliantCellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            byte[] rgb2 = new byte[3] { 170, 220, 200 };
            XSSFColor color2 = new XSSFColor(rgb);
            CompliantCellStyle.FillBackgroundXSSFColor = color2;
            CompliantCellStyle.FillPattern = FillPattern.SolidForeground;

            ICellStyle testeStyle = workbook.CreateCellStyle();
            testeStyle.BorderBottom = BorderStyle.Medium;
            testeStyle.FillBackgroundColor = IndexedColors.LightBlue.Index;
            testeStyle.FillPattern = FillPattern.SolidForeground;

            XSSFCellStyle style5 = (XSSFCellStyle)workbook.CreateCellStyle();
            // Define cell style according to input color parameter
            XSSFColor colorToFill = new XSSFColor(Color.LightBlue);
            style5.SetFillForegroundColor(colorToFill);
            style5.FillPattern = FillPattern.ThickHorizontalBands;

            XSSFCellStyle styleCompliant = (XSSFCellStyle)workbook.CreateCellStyle();
            byte[] rgbCompliant = new byte[3] { 170, 220, 200 };
            XSSFColor colorToFillCompliant = new XSSFColor(rgbCompliant);
            //XSSFColor colorToFill = new XSSFColor(Color.LightBlue);
            styleCompliant.SetFillForegroundColor(colorToFillCompliant);
            styleCompliant.FillPattern = FillPattern.SolidForeground;

            XSSFCellStyle styleNonCompliant = (XSSFCellStyle)workbook.CreateCellStyle();
            byte[] rgbNonCompliant = new byte[3] { 255, 102, 102 };
            XSSFColor colorToFillNonCompliant = new XSSFColor(rgbNonCompliant);
            //XSSFColor colorToFill = new XSSFColor(Color.LightBlue);
            styleNonCompliant.SetFillForegroundColor(colorToFillNonCompliant);
            styleNonCompliant.FillPattern = FillPattern.SolidForeground;

            // --rgb(77, 255, 166)
            // rgb(255,102,102)

            //            SetFillForegroundColor, not SetFillBackgroundColor;
            //Setstyle.FillPattern = FillPattern.SolidForeground;

            //hStyle.FillForegroundColor = IndexedColors.Black.Index;
            //hStyle.FillPattern = FillPattern.SolidForeground;

            for (var i = 1; i <= items.Count; i++)
            {
                var row = sheet.CreateRow(i);

                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    var cell = row.CreateCell(j);
                    var value = propertySelectors[j](items[i - 1]);
                    if (value != null)
                    {
                        cell.SetCellValue(value.ToString());

                        if (value.ToString().ToLower().Equals("true") || value.ToString().ToLower().Equals("false"))
                        {
                            if (value.ToString().ToLower().Equals("true"))
                            {
                                cell.CellStyle = styleCompliant;
                                cell.SetCellValue("Compliant");
                            }
                            else
                            {
                                cell.CellStyle = styleNonCompliant;
                                cell.SetCellValue("Not Compliant");
                            }
                        }
                    }
                }
            }

            // Auto-size columns ONCE after all rows are added (moved outside row loop for performance)
            // This prevents O(n*m) complexity - was causing timeouts on large exports
            for (int j = 0; j < propertySelectors.Length; j++)
            {
                sheet.AutoSizeColumn(j);
            }
            GC.Collect(); // Single garbage collection after all columns are sized
        }

        protected virtual void Save(XSSFWorkbook excelPackage, FileDto file)
        {
            using (var stream = new MemoryStream())
            {
                excelPackage.Write(stream);
                // Security: Associate file with current user/tenant for access control
                _tempFileCacheManager.SetFile(file.FileToken, stream.ToArray(), AbpSession?.UserId, AbpSession?.TenantId);
            }
        }

        protected void SetCellDataFormat(ICell cell, string dataFormat)
        {
            if (cell == null)
                return;

            var dateStyle = GetDateCellStyle(cell, dataFormat);
            var format = GetDateDataFormat(cell, dataFormat);

            dateStyle.DataFormat = format.GetFormat(dataFormat);
            cell.CellStyle = dateStyle;

            // Only try to parse and convert if the cell currently contains a string
            // If it already has a DateTime value (numeric cell type), the style is sufficient
            if (cell.CellType == CellType.String && !string.IsNullOrEmpty(cell.StringCellValue))
            {
                if (DateTime.TryParse(cell.StringCellValue, out var datetime))
                    cell.SetCellValue(datetime);
            }
        }
    }
}