using System.Collections.Generic;
using inzibackend.MultiTenancy.Importing.Dto;

namespace inzibackend.MultiTenancy.Importing
{
    public interface ITenantUserListExcelDataReader
    {
        List<ImportTenantUserDto> GetTenantUsersFromExcel(byte[] fileBytes);
    }
}
