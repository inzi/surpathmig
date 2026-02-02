using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class TenantDocumentCategoriesExcelExporter : NpoiExcelExporterBase, ITenantDocumentCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TenantDocumentCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTenantDocumentCategoryForViewDto> tenantDocumentCategories)
        {
            return CreateExcelPackage(
                "TenantDocumentCategories.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("TenantDocumentCategories"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("AuthorizedOnly"),
                        L("HostOnly"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, tenantDocumentCategories,
                        _ => _.TenantDocumentCategory.Name,
                        _ => _.TenantDocumentCategory.Description,
                        _ => _.TenantDocumentCategory.AuthorizedOnly,
                        _ => _.TenantDocumentCategory.HostOnly,
                        _ => _.UserName
                        );

                });
        }
    }
}