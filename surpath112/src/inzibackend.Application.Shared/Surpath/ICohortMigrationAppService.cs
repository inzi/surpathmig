using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface ICohortMigrationAppService : IApplicationService
    {
        // Analysis and Validation
        Task<CohortMigrationAnalysisDto> AnalyzeCohortMigration(Guid cohortId, Guid? targetDepartmentId);
        Task<List<TargetCategoryOptionDto>> GetTargetCategoryOptions(GetTargetCategoryOptionsInput input);
        Task<bool> ValidateMigrationMappings(ValidateMigrationMappingsInput input);
        
        // Migration Operations
        Task<CohortMigrationResultDto> ExecuteMigration(CohortMigrationDto input);
        Task<CohortMigrationResultDto> MigrateCohort(CohortMigrationDto input);
        Task<CohortMigrationResultDto> MigrateCohortToNewDepartment(CohortMigrationDto input);
        Task<CohortMigrationResultDto> MigrateCohortToExistingDepartment(CohortMigrationDto input);
        
        // Department Management
        Task<List<DepartmentLookupDto>> GetAvailableTargetDepartments(Guid excludeDepartmentId);
        Task<DepartmentSelectionValidationResultDto> ValidateDepartmentSelection(ValidateDepartmentSelectionInput input);
        Task<Guid> CreateDepartment(CreateDepartmentDto input);
        
        // Utility Methods
        Task<int> GetCohortUsersCount(Guid cohortId);
        Task<List<RequirementCategoryAnalysisDto>> GetCohortRequirementCategories(Guid cohortId);
        Task<bool> CanMigrateCohort(Guid cohortId);
        
        // Migration History
        Task<PagedResultDto<CohortMigrationHistoryDto>> GetMigrationHistory(GetAllCohortMigrationHistoryInput input);
        Task<CohortMigrationHistoryDto> GetMigrationHistoryForView(Guid migrationId);
        Task<FileDto> GetMigrationHistoryToExcel(GetAllCohortMigrationHistoryInput input);
        
        // Rollback and Recovery
        Task<bool> CanRollbackMigration(Guid migrationId);
        Task<CohortMigrationRollbackResultDto> RollbackMigration(CohortMigrationRollbackDto input);
        
        // Progress Tracking
        Task<CohortMigrationProgressDto> GetMigrationProgress(string migrationId);
        Task UpdateMigrationProgress(string migrationId, MigrationProgressUpdateDto progressUpdate);
        Task<MigrationProgressReportDto> GenerateMigrationProgressReport(string migrationId);
        Task<List<MigrationProgressHistoryDto>> GetMigrationProgressHistory(string migrationId);
        Task<ProgressDataCleanupResultDto> CleanupProgressData(int retentionDays = 90);
        
        // Compliance State Preservation
        Task<CompliancePreservationResultDto> PreserveCohortUserCompliance(Guid cohortId, string migrationId);
        Task<ComplianceIntegrityValidationResultDto> ValidateComplianceIntegrity(Guid cohortId, string migrationId, 
            CompliancePreservationResultDto preservationResult = null);
        Task<ComplianceRecalculationResultDto> RecalculateCompliance(Guid cohortId, string migrationId, Guid targetDepartmentId);
        
        // Lookup Tables
        Task<PagedResultDto<DepartmentLookupDto>> GetAllDepartmentsForLookupTable(GetAllForLookupTableInput input);
        Task<PagedResultDto<CohortTenantDepartmentLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input);
    }
} 