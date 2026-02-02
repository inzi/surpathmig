using System.Collections.Generic;
using System.Threading.Tasks;
using inzibackend.Authorization.Users.Dto;
using inzibackend.Dto;

namespace inzibackend.Authorization.Users.Exporting;

public interface IUserListExcelExporter
{
    Task<FileDto> ExportToFile(List<UserListDto> userListDtos, List<string> selectedColumns);
}