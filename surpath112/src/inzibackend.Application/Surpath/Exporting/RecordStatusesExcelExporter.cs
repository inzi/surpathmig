using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class RecordStatusesExcelExporter : NpoiExcelExporterBase, IRecordStatusesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RecordStatusesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRecordStatusForViewDto> recordStatuses)
        {
            return CreateExcelPackage(
                "RecordStatuses.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RecordStatuses"));

                    AddHeader(
                        sheet,
                        L("StatusName"),
                        L("HtmlColor"),
                        L("CSSCLass"),
                        L("IsDefault"),
                        L("RequireNoteOnSet"),
                        L("IsSurpathServiceStatus"),
                        L("ComplianceImpact"),
                        (L("TenantDepartment")) + L("Name")
                        );

                    AddObjects(
                        sheet, recordStatuses,
                        _ => _.RecordStatus.StatusName,
                        _ => _.RecordStatus.HtmlColor,
                        _ => _.RecordStatus.CSSCLass,
                        _ => _.RecordStatus.IsDefault,
                        _ => _.RecordStatus.RequireNoteOnSet,
                        _ => _.RecordStatus.IsSurpathServiceStatus,
                        _ => _.RecordStatus.ComplianceImpact,
                        _ => _.TenantDepartmentName
                        );

                });
        }
    }
}