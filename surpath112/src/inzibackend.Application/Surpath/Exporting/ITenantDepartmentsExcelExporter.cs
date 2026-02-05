using System.Collections.Generic;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath.Exporting
{
    public interface ITenantDepartmentsExcelExporter
    {
        FileDto ExportToFile(List<GetTenantDepartmentForViewDto> tenantDepartments);
    }
}