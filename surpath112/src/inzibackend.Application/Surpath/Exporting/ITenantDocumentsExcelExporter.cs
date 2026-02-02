using System.Collections.Generic;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath.Exporting
{
    public interface ITenantDocumentsExcelExporter
    {
        FileDto ExportToFile(List<GetTenantDocumentForViewDto> tenantDocuments);
    }
}