using System.Collections.Generic;
using inzibackend.Auditing.Dto;
using inzibackend.Dto;

namespace inzibackend.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
