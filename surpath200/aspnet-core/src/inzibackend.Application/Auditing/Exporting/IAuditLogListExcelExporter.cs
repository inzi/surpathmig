using inzibackend.Auditing.Dto;
using inzibackend.Dto;
using inzibackend.EntityChanges.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace inzibackend.Auditing.Exporting;

public interface IAuditLogListExcelExporter
{
    Task<FileDto> ExportToFile(List<AuditLogListDto> auditLogListDtos);

    Task<FileDto> ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
}
