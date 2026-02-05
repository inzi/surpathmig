using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class DeptCodesExcelExporter : NpoiExcelExporterBase, IDeptCodesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DeptCodesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDeptCodeForViewDto> deptCodes)
        {
            return CreateExcelPackage(
                "DeptCodes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("DeptCodes"));

                    AddHeader(
                        sheet,
                        L("Code"),
                        (L("CodeType")) + L("Name"),
                        (L("TenantDepartment")) + L("Name")
                        );

                    AddObjects(
                        sheet, deptCodes,
                        _ => _.DeptCode.Code,
                        _ => _.CodeTypeName,
                        _ => _.TenantDepartmentName
                        );

                });
        }
    }
}