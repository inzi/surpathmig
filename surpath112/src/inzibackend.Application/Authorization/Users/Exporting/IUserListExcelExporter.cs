using System.Collections.Generic;
using inzibackend.Authorization.Users.Dto;
using inzibackend.Dto;

namespace inzibackend.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}