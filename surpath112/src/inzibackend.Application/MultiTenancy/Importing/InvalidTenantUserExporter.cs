using System.Collections.Generic;
using Abp.Collections.Extensions;
using Abp.Dependency;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Dto;
using inzibackend.MultiTenancy.Importing.Dto;
using inzibackend.Storage;

namespace inzibackend.MultiTenancy.Importing
{
    public class InvalidTenantUserExporter : NpoiExcelExporterBase, IInvalidTenantUserExporter, ITransientDependency
    {
        public InvalidTenantUserExporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ImportTenantUserDto> invalidUsers)
        {
            return CreateExcelPackage(
                "InvalidTenantUserImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("InvalidUserImports"));

                    AddHeader(
                        sheet,
                        L("UserName"),
                        L("Name"),
                        L("Surname"),
                        L("EmailAddress"),
                        L("PhoneNumber"),
                        L("PasswordOptional"),
                        L("Roles"),
                        L("DepartmentName"),
                        L("CohortName"),
                        L("Address"),
                        L("SuiteApt"),
                        L("City"),
                        L("State"),
                        L("Zip"),
                        L("DateOfBirth"),
                        L("SSN"),
                        L("Refuse Reason")
                    );

                    AddObjects(
                        sheet, invalidUsers,
                        _ => _.UserName,
                        _ => _.Name,
                        _ => _.Surname,
                        _ => _.EmailAddress,
                        _ => _.PhoneNumber,
                        _ => _.Password,
                        _ => _.AssignedRoleNames?.JoinAsString(","),
                        _ => _.DepartmentName,
                        _ => _.CohortName,
                        _ => _.Address,
                        _ => _.SuiteApt,
                        _ => _.City,
                        _ => _.State,
                        _ => _.Zip,
                        _ => _.DateOfBirth,
                        _ => _.SSN,
                        _ => _.Exception
                    );

                    // Auto-size all columns
                    for (var i = 0; i < 17; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
