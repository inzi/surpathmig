using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class TenantDocumentsExcelExporter : NpoiExcelExporterBase, ITenantDocumentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TenantDocumentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTenantDocumentForViewDto> tenantDocuments)
        {
            return CreateExcelPackage(
                "TenantDocuments.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("TenantDocuments"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("AuthorizedOnly"),
                        L("Description"),
                        (L("TenantDocumentCategory")) + L("Name"),
                        (L("Record")) + L("filename")
                        );

                    AddObjects(
                        sheet, tenantDocuments,
                        _ => _.TenantDocument.Name,
                        _ => _.TenantDocument.AuthorizedOnly,
                        _ => _.TenantDocument.Description,
                        _ => _.TenantDocumentCategoryName,
                        _ => _.Recordfilename
                        );

                });
        }
    }
}