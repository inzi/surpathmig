using System.Collections.Generic;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using static inzibackend.Surpath.CohortUsersAppService;

namespace inzibackend.Surpath.Exporting
{
    public interface ICohortUsersExcelExporter
    {
        FileDto ExportToFile(List<GetCohortUserForViewDto> cohortUsers);
        FileDto ExportToFileFromObject(List<CohortUserComplianceExcel> cohortUsersCompliance);
        FileDto ExportToFileFromObjectMultiSheet(List<CohortUserComplianceExcel> cohortUsersCompliance);
        FileDto ExportImmunizationReportToFile(List<CohortImmunizationReportDto> cohortImmunizationData);
        FileDto ExportImmunizationReportToFileMultiSheet(List<CohortImmunizationReportDto> cohortImmunizationData);
    }
}