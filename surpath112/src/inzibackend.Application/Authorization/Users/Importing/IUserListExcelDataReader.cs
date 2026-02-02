using System.Collections.Generic;
using inzibackend.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace inzibackend.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
