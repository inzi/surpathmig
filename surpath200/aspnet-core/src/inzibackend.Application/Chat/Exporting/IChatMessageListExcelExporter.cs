using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using inzibackend.Chat.Dto;
using inzibackend.Dto;

namespace inzibackend.Chat.Exporting;

public interface IChatMessageListExcelExporter
{
    Task<FileDto> ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
}