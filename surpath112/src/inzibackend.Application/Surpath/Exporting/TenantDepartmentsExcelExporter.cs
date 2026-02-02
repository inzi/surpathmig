using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class TenantDepartmentsExcelExporter : NpoiExcelExporterBase, ITenantDepartmentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TenantDepartmentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTenantDepartmentForViewDto> tenantDepartments)
        {
            return CreateExcelPackage(
                "TenantDepartments.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("TenantDepartments"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Active"),
                        L("MROType"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, tenantDepartments,
                        _ => _.TenantDepartment.Name,
                        _ => _.TenantDepartment.Active,
                        _ => _.TenantDepartment.MROType,
                        _ => _.TenantDepartment.Description
                        );

                });
        }
    }
}