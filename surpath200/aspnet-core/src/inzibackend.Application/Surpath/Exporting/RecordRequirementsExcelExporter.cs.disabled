using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class RecordRequirementsExcelExporter : NpoiExcelExporterBase, IRecordRequirementsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RecordRequirementsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRecordRequirementForViewDto> recordRequirements)
        {
            return CreateExcelPackage(
                "RecordRequirements.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RecordRequirements"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Metadata"),
                        L("IsSurpathOnly"),
                        (L("TenantDepartment")) + L("Name"),
                        (L("Cohort")) + L("Name"),
                        (L("SurpathService")) + L("Name"),
                        (L("TenantSurpathService")) + L("Name")
                        );

                    AddObjects(
                        sheet, recordRequirements,
                        _ => _.RecordRequirement.Name,
                        _ => _.RecordRequirement.Description,
                        _ => _.RecordRequirement.Metadata,
                        _ => _.RecordRequirement.IsSurpathOnly,
                        _ => _.TenantDepartmentName,
                        _ => _.CohortName,
                        _ => _.SurpathServiceName,
                        _ => _.TenantSurpathServiceName
                        );

                });
        }
    }
}