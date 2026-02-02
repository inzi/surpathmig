using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class SurpathServicesExcelExporter : NpoiExcelExporterBase, ISurpathServicesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SurpathServicesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSurpathServiceForViewDto> surpathServices)
        {
            return CreateExcelPackage(
                "SurpathServices.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("SurpathServices"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Price"),
                        L("Discount"),
                        L("Description"),
                        L("IsEnabledByDefault"),
                        (L("TenantDepartment")) + L("Name"),
                        (L("Cohort")) + L("Name"),
                        (L("User")) + L("Name"),
                        (L("RecordCategoryRule")) + L("Name")
                        );

                    AddObjects(
                        sheet, surpathServices,
                        _ => _.SurpathService.Name,
                        _ => _.SurpathService.Price,
                        _ => _.SurpathService.Discount,
                        _ => _.SurpathService.Description,
                        _ => _.SurpathService.IsEnabledByDefault,
                        _ => _.TenantDepartmentName,
                        _ => _.CohortName,
                        _ => _.UserName,
                        _ => _.RecordCategoryRuleName
                        );

                });
        }
    }
}