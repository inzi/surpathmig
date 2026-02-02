using System.Collections.Generic;
using inzibackend.Dto;
using inzibackend.MultiTenancy.Importing.Dto;

namespace inzibackend.MultiTenancy.Importing
{
    public interface IInvalidTenantUserExporter
    {
        FileDto ExportToFile(List<ImportTenantUserDto> invalidUsers);
    }
}
