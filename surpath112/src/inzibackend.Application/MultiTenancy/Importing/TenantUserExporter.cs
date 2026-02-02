using System.Collections.Generic;
using Abp.Dependency;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Dto;
using inzibackend.MultiTenancy.Importing.Dto;
using inzibackend.Storage;

namespace inzibackend.MultiTenancy.Importing
{
    public class TenantUserExporter : NpoiExcelExporterBase, ITenantUserExporter, ITransientDependency
    {
        public TenantUserExporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<TenantUserExportDto> users)
        {
            return CreateExcelPackage(
                "TenantUsers.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Users"));

                    AddHeader(
                        sheet,
                        L("UserName"),
                        L("Name"),
                        L("Surname"),
                        L("EmailAddress"),
                        L("PhoneNumber"),
                        L("PasswordOptional"),
                        L("AssignedRoleNames"),
                        L("DepartmentName"),
                        L("CohortName"),
                        L("Address"),
                        L("SuiteApt"),
                        L("City"),
                        L("State"),
                        L("Zip"),
                        L("DateOfBirth"),
                        L("SSN")
                    );

                    AddObjects(
                        sheet, users,
                        _ => _.UserName,
                        _ => _.Name,
                        _ => _.Surname,
                        _ => _.EmailAddress,
                        _ => _.PhoneNumber ?? string.Empty,
                        _ => string.Empty, // Password - empty for export
                        _ => _.AssignedRoleNames ?? string.Empty,
                        _ => _.DepartmentName ?? string.Empty,
                        _ => _.CohortName ?? string.Empty,
                        _ => _.Address ?? string.Empty,
                        _ => _.SuiteApt ?? string.Empty,
                        _ => _.City ?? string.Empty,
                        _ => _.State ?? string.Empty,
                        _ => _.Zip ?? string.Empty,
                        _ => _.DateOfBirth ?? string.Empty,
                        _ => _.SSN ?? string.Empty
                    );

                    // Auto-size all columns
                    for (var i = 0; i < 16; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
