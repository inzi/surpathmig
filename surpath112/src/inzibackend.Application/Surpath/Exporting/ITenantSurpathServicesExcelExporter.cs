using System.Collections.Generic;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath.Exporting
{
    public interface ITenantSurpathServicesExcelExporter
    {
        FileDto ExportToFile(List<GetTenantSurpathServiceForViewDto> tenantSurpathServices);
    }
}