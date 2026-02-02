using System.Collections.Generic;
using inzibackend.Authorization.Users.Importing.Dto;
using inzibackend.Dto;

namespace inzibackend.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
