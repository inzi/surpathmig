using System.Collections.Generic;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath.Exporting
{
    public interface IRecordsExcelExporter
    {
        FileDto ExportToFile(List<GetRecordForViewDto> records);
    }
}