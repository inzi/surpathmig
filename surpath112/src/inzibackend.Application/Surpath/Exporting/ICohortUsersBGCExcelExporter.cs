using System.Collections.Generic;
using inzibackend.Dto;
using inzibackend.Surpath.Dtos;

namespace inzibackend.Surpath.Exporting
{
    public interface ICohortUsersBGCExcelExporter
    {
        FileDto ExportToFile(List<GetCohortUserForBGCExportDto> cohortUsers);
    }
}
