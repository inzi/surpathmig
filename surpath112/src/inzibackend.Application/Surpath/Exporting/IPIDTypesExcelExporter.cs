using System.Collections.Generic;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath.Exporting
{
    public interface IPidTypesExcelExporter
    {
        FileDto ExportToFile(List<GetPidTypeForViewDto> pidTypes);
    }
}