using System.Collections.Generic;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath.Exporting
{
    public interface ICohortsExcelExporter
    {
        FileDto ExportToFile(List<GetCohortForViewDto> cohorts);
        FileDto ExportImmunizationReportToFile(List<CohortImmunizationReportDto> cohortImmunizationData);
        FileDto ExportImmunizationReportToFileMultiSheet(List<CohortImmunizationReportDto> cohortImmunizationData);
    }
}