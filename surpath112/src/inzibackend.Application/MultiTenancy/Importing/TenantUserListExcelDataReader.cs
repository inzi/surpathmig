using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Dependency;
using Abp.Localization;
using Abp.Localization.Sources;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.MultiTenancy.Importing.Dto;
using NPOI.SS.UserModel;

namespace inzibackend.MultiTenancy.Importing
{
    public class TenantUserListExcelDataReader : NpoiExcelImporterBase<ImportTenantUserDto>, ITenantUserListExcelDataReader, ITransientDependency
    {
        private readonly ILocalizationSource _localizationSource;

        public TenantUserListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(inzibackendConsts.LocalizationSourceName);
        }

        public List<ImportTenantUserDto> GetTenantUsersFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportTenantUserDto ProcessExcelRow(ISheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var user = new ImportTenantUserDto();

            try
            {
                user.UserName = GetRequiredValueFromRowOrNull(worksheet, row, 0, nameof(user.UserName), exceptionMessage);
                user.Name = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(user.Name), exceptionMessage);
                user.Surname = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(user.Surname), exceptionMessage);
                user.EmailAddress = GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(user.EmailAddress), exceptionMessage);
                user.PhoneNumber = GetOptionalValueFromRowOrNull(worksheet, row, 4, exceptionMessage, CellType.String);
                user.Password = GetOptionalValueFromRowOrNull(worksheet, row, 5, exceptionMessage, CellType.String); // Optional - will generate random if empty
                user.AssignedRoleNames = GetAssignedRoleNamesFromRow(worksheet, row, 6);
                user.DepartmentName = GetOptionalValueFromRowOrNull(worksheet, row, 7, exceptionMessage, CellType.String);
                user.CohortName = GetOptionalValueFromRowOrNull(worksheet, row, 8, exceptionMessage, CellType.String);
                user.Address = GetOptionalValueFromRowOrNull(worksheet, row, 9, exceptionMessage, CellType.String);
                user.SuiteApt = GetOptionalValueFromRowOrNull(worksheet, row, 10, exceptionMessage, CellType.String);
                user.City = GetOptionalValueFromRowOrNull(worksheet, row, 11, exceptionMessage, CellType.String);
                user.State = GetOptionalValueFromRowOrNull(worksheet, row, 12, exceptionMessage, CellType.String);
                user.Zip = GetOptionalValueFromRowOrNull(worksheet, row, 13, exceptionMessage, CellType.String);
                user.DateOfBirth = GetOptionalValueFromRowOrNull(worksheet, row, 14, exceptionMessage, CellType.String);
                user.SSN = GetOptionalValueFromRowOrNull(worksheet, row, 15, exceptionMessage, CellType.String);

                if (exceptionMessage.Length > 0)
                {
                    user.Exception = exceptionMessage.ToString();
                }
            }
            catch (System.Exception exception)
            {
                user.Exception = exception.Message;
            }

            return user;
        }

        private string GetRequiredValueFromRowOrNull(
            ISheet worksheet,
            int row,
            int column,
            string columnName,
            StringBuilder exceptionMessage,
            CellType? cellType = null)
        {
            var cell = worksheet.GetRow(row).GetCell(column);

            if (cellType.HasValue)
            {
                cell.SetCellType(cellType.Value);
            }

            var cellValue = cell.StringCellValue;
            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue))
            {
                return cellValue;
            }

            exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
            return null;
        }

        private string GetOptionalValueFromRowOrNull(ISheet worksheet, int row, int column, StringBuilder exceptionMessage, CellType? cellType = null)
        {
            var cell = worksheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                return string.Empty;
            }

            if (cellType != null)
            {
                cell.SetCellType(cellType.Value);
            }

            var cellValue = worksheet.GetRow(row).GetCell(column).StringCellValue;
            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue))
            {
                return cellValue;
            }

            return String.Empty;
        }

        private string[] GetAssignedRoleNamesFromRow(ISheet worksheet, int row, int column)
        {
            var cellValue = worksheet.GetRow(row).GetCell(column)?.StringCellValue;
            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue))
            {
                return new string[0];
            }

            return cellValue.ToString().Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray();
        }

        private string GetLocalizedExceptionMessagePart(string parameter)
        {
            return _localizationSource.GetString("{0}IsInvalid", _localizationSource.GetString(parameter)) + "; ";
        }

        private bool IsRowEmpty(ISheet worksheet, int row)
        {
            var cell = worksheet.GetRow(row)?.Cells.FirstOrDefault();
            return cell == null || string.IsNullOrWhiteSpace(cell.StringCellValue);
        }
    }
}
