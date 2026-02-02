using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class TenantDepartmentUsersExcelExporter : NpoiExcelExporterBase, ITenantDepartmentUsersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TenantDepartmentUsersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTenantDepartmentUserForViewDto> tenantDepartmentUsers)
        {
            return CreateExcelPackage(
                "TenantDepartmentUsers.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("TenantDepartmentUsers"));

                    AddHeader(
                        sheet,
                        (L("User")) + L("Name"),
                        (L("TenantDepartment")) + L("Name")
                        );

                    AddObjects(
                        sheet, tenantDepartmentUsers,
                        _ => _.UserName,
                        _ => _.TenantDepartmentName
                        );

                });
        }
    }
}