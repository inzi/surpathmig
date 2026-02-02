using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class TenantSurpathServicesExcelExporter : NpoiExcelExporterBase, ITenantSurpathServicesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TenantSurpathServicesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTenantSurpathServiceForViewDto> tenantSurpathServices)
        {
            return CreateExcelPackage(
                "TenantSurpathServices.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("TenantSurpathServices"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Price"),
                        L("Description"),
                        L("IsEnabled"),
                        (L("SurpathService")) + L("Name"),
                        (L("TenantDepartment")) + L("Name"),
                        (L("Cohort")) + L("Name"),
                        (L("User")) + L("Name"),
                        (L("RecordCategoryRule")) + L("Name")
                        );

                    AddObjects(
                        sheet, tenantSurpathServices,
                        _ => _.TenantSurpathService.Name,
                        _ => _.TenantSurpathService.Price,
                        _ => _.TenantSurpathService.Description,
                        _ => _.TenantSurpathService.IsPricingOverrideEnabled,
                        _ => _.SurpathServiceName,
                        _ => _.TenantDepartmentName,
                        _ => _.CohortName,
                        _ => _.UserName,
                        _ => _.RecordCategoryRuleName
                        );

                });
        }
    }
}