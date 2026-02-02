using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class DepartmentUsersExcelExporter : NpoiExcelExporterBase, IDepartmentUsersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DepartmentUsersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDepartmentUserForViewDto> departmentUsers)
        {
            return CreateExcelPackage(
                "DepartmentUsers.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("DepartmentUsers"));

                    AddHeader(
                        sheet,
                        (L("User")) + L("Name"),
                        (L("TenantDepartment")) + L("Name")
                        );

                    AddObjects(
                        sheet, departmentUsers,
                        _ => _.UserName,
                        _ => _.TenantDepartmentName
                        );

                });
        }
    }
}