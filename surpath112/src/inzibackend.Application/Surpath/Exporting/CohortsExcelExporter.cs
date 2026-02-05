using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using System;

namespace inzibackend.Surpath.Exporting
{
    public class CohortsExcelExporter : NpoiExcelExporterBase, ICohortsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CohortsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCohortForViewDto> cohorts)
        {
            return CreateExcelPackage(
                "Cohorts.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Cohorts"));

                    AddHeader(
                        sheet,
                        L("Cohort") + L("Name"),
                        L("Cohort") + L("Description"),
                        L("TenantDepartment") + L("Name")
                        );

                    AddObjects(
                        sheet, cohorts,
                        _ => _.Cohort.Name,
                        _ => _.Cohort.Description,
                        _ => _.TenantDepartmentName
                        );
                });
        }

        public FileDto ExportImmunizationReportToFile(List<CohortImmunizationReportDto> cohortImmunizationData)
        {
            return CreateExcelPackage(
                "CohortsImmunizationReport.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("CohortImmunizationReport"));

                    // Get the cohort name
                    var cohortName = cohortImmunizationData.FirstOrDefault()?.CohortName ?? "Multiple Cohorts";

                    // Sort data by lastname for consistent ordering
                    var sortedData = cohortImmunizationData.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();

                    // Get all unique categories
                    var allCategories = sortedData
                        .SelectMany(c => c.ImmunizationRequirements)
                        .Select(r => r.CategoryName)
                        .Distinct()
                        .OrderBy(c => c)
                        .ToList();

                    // Track which columns have data for each category
                    var categoryDataTracking = new Dictionary<string, (bool hasStatus, bool hasAdministered, bool hasExpiration)>();

                    foreach (var category in allCategories)
                    {
                        var categoryRequirements = sortedData
                            .SelectMany(c => c.ImmunizationRequirements)
                            .Where(r => r.CategoryName == category)
                            .ToList();

                        var hasStatus = categoryRequirements.Any(r => !string.IsNullOrWhiteSpace(r.ComplianceStatus));
                        var hasAdministered = categoryRequirements.Any(r => r.AdministeredDate.HasValue && r.AdministeredDate.Value != DateTime.MinValue);
                        var hasExpiration = categoryRequirements.Any(r => r.ExpirationDate.HasValue && r.ExpirationDate.Value != DateTime.MinValue);

                        categoryDataTracking[category] = (hasStatus, hasAdministered, hasExpiration);
                    }

                    // Build headers based on what columns have data
                    var headers = new List<string> { L("FirstName"), L("LastName") };
                    var columnMapping = new List<(string category, string columnType)>();

                    foreach (var category in allCategories)
                    {
                        var tracking = categoryDataTracking[category];

                        if (tracking.hasStatus)
                        {
                            headers.Add(category);
                            columnMapping.Add((category, "status"));
                        }

                        if (tracking.hasAdministered)
                        {
                            headers.Add(L("AdministeredDate"));
                            columnMapping.Add((category, "administered"));
                        }

                        if (tracking.hasExpiration)
                        {
                            headers.Add(L("ExpirationDate"));
                            columnMapping.Add((category, "expiration"));
                        }
                    }

                    // Create header row (we'll insert cohort info above this later)
                    var headerRow = sheet.CreateRow(0);
                    for (int i = 0; i < headers.Count; i++)
                    {
                        headerRow.CreateCell(i).SetCellValue(headers[i]);
                    }

                    // Create data rows using sorted data
                    for (int i = 0; i < sortedData.Count; i++)
                    {
                        var cohortData = sortedData[i];
                        var row = sheet.CreateRow(i + 1);
                        var cellIndex = 0;

                        // Basic info
                        row.CreateCell(cellIndex++).SetCellValue(cohortData.FirstName ?? "");
                        row.CreateCell(cellIndex++).SetCellValue(cohortData.LastName ?? "");

                        // Dynamic columns based on mapping
                        foreach (var (category, columnType) in columnMapping)
                        {
                            var userRequirement = cohortData.ImmunizationRequirements
                                .FirstOrDefault(r => r.CategoryName == category);

                            if (columnType == "status")
                            {
                                var statusCell = row.CreateCell(cellIndex++);
                                if (userRequirement != null)
                                {
                                    statusCell.SetCellValue(userRequirement.ComplianceStatus);
                                    ApplyComplianceStatusColor(excelPackage, statusCell, userRequirement.ComplianceStatus, userRequirement.StatusColor);
                                }
                                else
                                {
                                    statusCell.SetCellValue("Not Applicable");
                                }
                            }
                            else if (columnType == "administered")
                            {
                                var dateCell = row.CreateCell(cellIndex++);
                                if (userRequirement?.AdministeredDate.HasValue == true && userRequirement.AdministeredDate.Value != DateTime.MinValue)
                                {
                                    dateCell.SetCellValue(userRequirement.AdministeredDate.Value);
                                    SetCellDataFormat(dateCell, "MM/dd/yyyy");
                                }
                                else
                                {
                                    dateCell.SetCellValue("");
                                }
                            }
                            else if (columnType == "expiration")
                            {
                                var dateCell = row.CreateCell(cellIndex++);
                                if (userRequirement?.ExpirationDate.HasValue == true && userRequirement.ExpirationDate.Value != DateTime.MinValue)
                                {
                                    dateCell.SetCellValue(userRequirement.ExpirationDate.Value);
                                    SetCellDataFormat(dateCell, "MM/dd/yyyy");
                                }
                                else
                                {
                                    dateCell.SetCellValue("");
                                }
                            }
                        }
                    }

                    // Now insert cohort header and blank row above the data
                    // Shift all existing rows down by 2
                    sheet.ShiftRows(0, sheet.LastRowNum, 2);

                    // Insert cohort header in row 0
                    var cohortRow = sheet.CreateRow(0);
                    cohortRow.CreateCell(0).SetCellValue("Cohort:");
                    cohortRow.CreateCell(1).SetCellValue(cohortName);

                    // Row 1 is left blank (created automatically by shift)

                    // Auto-size columns
                    for (int i = 0; i < headers.Count; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }

        private void ApplyComplianceStatusColor(XSSFWorkbook workbook, ICell cell, string statusText, string htmlColor)
        {
            if (string.IsNullOrEmpty(htmlColor) || cell == null)
                return;

            try
            {
                // Parse the HTML color
                var color = ColorTranslator.FromHtml(htmlColor);

                // Create cell style with background color for XSSF
                XSSFCellStyle cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                // Set fill pattern first
                cellStyle.FillPattern = FillPattern.SolidForeground;

                // Apply custom background color
                cellStyle.SetFillForegroundColor(new XSSFColor(new byte[] { color.R, color.G, color.B }));

                // Calculate contrasting text color
                var textColor = GetContrastingTextColor(color);

                // Create font with contrasting color
                XSSFFont font = (XSSFFont)workbook.CreateFont();
                font.SetColor(new XSSFColor(new byte[] { textColor.R, textColor.G, textColor.B }));
                font.IsBold = true;
                cellStyle.SetFont(font);

                // Apply the style to the cell
                cell.CellStyle = cellStyle;
            }
            catch (Exception)
            {
                // If color parsing fails, continue without styling
            }
        }

        private Color GetContrastingTextColor(Color backgroundColor)
        {
            // Calculate relative luminance using W3C formula
            double luminance = (0.299 * backgroundColor.R + 0.587 * backgroundColor.G + 0.114 * backgroundColor.B) / 255;

            // Return white text for dark backgrounds, black text for light backgrounds
            return luminance > 0.5 ? Color.Black : Color.White;
        }

        public FileDto ExportImmunizationReportToFileMultiSheet(List<CohortImmunizationReportDto> cohortImmunizationData)
        {
            return CreateExcelPackage(
                "CohortsImmunizationReport.xlsx",
                excelPackage =>
                {
                    // Track used sheet names to handle duplicates
                    var usedSheetNames = new HashSet<string>();

                    // Sort all data by lastname for consistent ordering
                    var sortedData = cohortImmunizationData.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();

                    // Group data by cohort and sort cohort groups alphabetically
                    var cohortGroups = sortedData
                        .GroupBy(c => c.CohortName)
                        .OrderBy(g => g.Key)
                        .ToList();

                    // Create "All" sheet first with cohort name in first column
                    var allSheetName = GetUniqueSheetName("All", usedSheetNames);
                    var allSheet = excelPackage.CreateSheet(allSheetName);
                    CreateImmunizationSheet(allSheet, sortedData, "All Cohorts", excelPackage, true);

                    // Create individual sheets for each cohort (already sorted alphabetically)
                    foreach (var cohortGroup in cohortGroups)
                    {
                        var cohortName = string.IsNullOrWhiteSpace(cohortGroup.Key) ? "Unknown Cohort" : cohortGroup.Key;
                        var safeCohortName = GetUniqueSheetName(cohortName, usedSheetNames);
                        var sheet = excelPackage.CreateSheet(safeCohortName);

                        // Sort cohort data by lastname as well
                        var cohortData = cohortGroup.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();
                        CreateImmunizationSheet(sheet, cohortData, cohortName, excelPackage, false);
                    }
                });
        }

        private void CreateImmunizationSheet(ISheet sheet, List<CohortImmunizationReportDto> cohortData, string cohortName, XSSFWorkbook workbook, bool includeCohortColumn)
        {
            // Sort cohort data by lastname for consistent ordering
            var sortedCohortData = cohortData.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();

            // Get all unique categories
            var allCategories = sortedCohortData
                .SelectMany(c => c.ImmunizationRequirements)
                .Select(r => r.CategoryName)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // Track which columns have data for each category
            var categoryDataTracking = new Dictionary<string, (bool hasStatus, bool hasAdministered, bool hasExpiration)>();

            foreach (var category in allCategories)
            {
                var categoryRequirements = sortedCohortData
                    .SelectMany(c => c.ImmunizationRequirements)
                    .Where(r => r.CategoryName == category)
                    .ToList();

                var hasStatus = categoryRequirements.Any(r => !string.IsNullOrWhiteSpace(r.ComplianceStatus));
                var hasAdministered = categoryRequirements.Any(r => r.AdministeredDate.HasValue && r.AdministeredDate.Value != DateTime.MinValue);
                var hasExpiration = categoryRequirements.Any(r => r.ExpirationDate.HasValue && r.ExpirationDate.Value != DateTime.MinValue);

                categoryDataTracking[category] = (hasStatus, hasAdministered, hasExpiration);
            }

            // Build headers based on what columns have data
            var headers = new List<string>();
            if (includeCohortColumn)
                headers.Add(L("Cohort"));

            headers.Add(L("FirstName"));
            headers.Add(L("LastName"));

            var columnMapping = new List<(string category, string columnType)>();

            foreach (var category in allCategories)
            {
                var tracking = categoryDataTracking[category];

                if (tracking.hasStatus)
                {
                    headers.Add(category);
                    columnMapping.Add((category, "status"));
                }

                if (tracking.hasAdministered)
                {
                    headers.Add(L("AdministeredDate"));
                    columnMapping.Add((category, "administered"));
                }

                if (tracking.hasExpiration)
                {
                    headers.Add(L("ExpirationDate"));
                    columnMapping.Add((category, "expiration"));
                }
            }

            // Create header row (we'll insert cohort info above this later if not "All" sheet)
            var headerRow = sheet.CreateRow(0);
            for (int i = 0; i < headers.Count; i++)
            {
                headerRow.CreateCell(i).SetCellValue(headers[i]);
            }

            // Create data rows using sorted data
            for (int i = 0; i < sortedCohortData.Count; i++)
            {
                var memberData = sortedCohortData[i];
                var row = sheet.CreateRow(i + 1);
                var cellIndex = 0;

                // Cohort name (only for "All" sheet)
                if (includeCohortColumn)
                {
                    row.CreateCell(cellIndex++).SetCellValue(memberData.CohortName ?? "");
                }

                // Basic info
                row.CreateCell(cellIndex++).SetCellValue(memberData.FirstName ?? "");
                row.CreateCell(cellIndex++).SetCellValue(memberData.LastName ?? "");

                // Dynamic columns based on mapping
                foreach (var (category, columnType) in columnMapping)
                {
                    var userRequirement = memberData.ImmunizationRequirements
                        .FirstOrDefault(r => r.CategoryName == category);

                    if (columnType == "status")
                    {
                        var statusCell = row.CreateCell(cellIndex++);
                        if (userRequirement != null)
                        {
                            statusCell.SetCellValue(userRequirement.ComplianceStatus);
                            ApplyComplianceStatusColor(workbook, statusCell, userRequirement.ComplianceStatus, userRequirement.StatusColor);
                        }
                        else
                        {
                            statusCell.SetCellValue("Not Applicable");
                        }
                    }
                    else if (columnType == "administered")
                    {
                        var dateCell = row.CreateCell(cellIndex++);
                        if (userRequirement?.AdministeredDate.HasValue == true && userRequirement.AdministeredDate.Value != DateTime.MinValue)
                        {
                            dateCell.SetCellValue(userRequirement.AdministeredDate.Value);
                            SetCellDataFormat(dateCell, "MM/dd/yyyy");
                        }
                        else
                        {
                            dateCell.SetCellValue("");
                        }
                    }
                    else if (columnType == "expiration")
                    {
                        var dateCell = row.CreateCell(cellIndex++);
                        if (userRequirement?.ExpirationDate.HasValue == true && userRequirement.ExpirationDate.Value != DateTime.MinValue)
                        {
                            dateCell.SetCellValue(userRequirement.ExpirationDate.Value);
                            SetCellDataFormat(dateCell, "MM/dd/yyyy");
                        }
                        else
                        {
                            dateCell.SetCellValue("");
                        }
                    }
                }
            }

            // Insert cohort header above data for individual cohort sheets (not for "All" sheet)
            if (!includeCohortColumn)
            {
                sheet.ShiftRows(0, sheet.LastRowNum, 2);

                var cohortRow = sheet.CreateRow(0);
                cohortRow.CreateCell(0).SetCellValue("Cohort:");
                cohortRow.CreateCell(1).SetCellValue(cohortName);
            }

            // Auto-size columns
            for (int i = 0; i < headers.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        }

        private string GetUniqueSheetName(string name, HashSet<string> usedSheetNames)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = "Sheet";

            // If the name starts with a number, prepend "Cohort " to make it Excel-compliant
            if (name.Length > 0 && char.IsDigit(name[0]))
            {
                name = "Cohort " + name;
            }

            // Apply Excel sheet name restrictions but keep original name as close as possible
            var safeName = name
                .Replace("[", "(")
                .Replace("]", ")")
                .Replace("*", "")
                .Replace("?", "")
                .Replace("/", "-")
                .Replace("\\", "-")
                .Replace(":", "-")
                .Trim();

            // Truncate to leave room for suffix if needed (Excel limit is 31 chars)
            var maxLength = 31;
            var originalSafeName = safeName;

            if (safeName.Length > maxLength)
                safeName = safeName.Substring(0, maxLength).Trim();

            // Check if the name is already used and add suffix if needed
            var finalName = safeName;
            int suffix = 1;

            while (usedSheetNames.Contains(finalName))
            {
                var suffixText = $"-{suffix}";
                var availableLength = maxLength - suffixText.Length;

                if (availableLength > 0)
                {
                    var truncatedBase = safeName.Length > availableLength
                        ? safeName.Substring(0, availableLength).Trim()
                        : safeName;
                    finalName = $"{truncatedBase}{suffixText}";
                }
                else
                {
                    finalName = $"Sheet{suffix}";
                }

                suffix++;
            }

            usedSheetNames.Add(finalName);
            return finalName;
        }
    }
}