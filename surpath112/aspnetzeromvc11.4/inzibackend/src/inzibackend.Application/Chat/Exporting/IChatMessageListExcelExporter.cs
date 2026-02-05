using System.Collections.Generic;
using Abp;
using inzibackend.Chat.Dto;
using inzibackend.Dto;

namespace inzibackend.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
