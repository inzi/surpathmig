using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface IUserTransferAppService : IApplicationService
    {
        // Analysis and Validation
        Task<UserTransferAnalysisDto> AnalyzeUserTransfer(Guid cohortId, Guid? targetDepartmentId);

        Task<UserTransferAnalysisDto> AnalyzeSelectiveUserTransfer(AnalyzeUserTransferInput input); // New method for selective transfer

        Task<List<TargetCategoryOptionDto>> GetTargetCategoryOptions(GetTargetCategoryOptionsInput input);

        Task<bool> ValidateMigrationMappings(ValidateMigrationMappingsInput input);

        // Transfer Operations
        Task<UserTransferResultDto> ExecuteTransfer(UserTransferDto input);

        Task<UserTransferResultDto> TransferUsers(UserTransferDto input);

        Task<UserTransferResultDto> TransferSelectedUsers(TransferSelectedUsersInput input); // New method for selective transfer

        Task<UserTransferResultDto> TransferUsersToNewDepartment(UserTransferDto input);

        Task<UserTransferResultDto> TransferUsersToExistingDepartment(UserTransferDto input);

        // Department Management
        Task<List<DepartmentLookupDto>> GetAvailableTargetDepartments(Guid excludeDepartmentId);

        Task<DepartmentSelectionValidationResultDto> ValidateDepartmentSelection(ValidateDepartmentSelectionInput input);

        Task<Guid> CreateDepartment(CreateDepartmentDto input);

        // Cohort and User Management - New methods
        Task<List<CohortLookupDto>> GetCohortsForDepartment(Guid departmentId);
        
        Task<List<CohortLookupDto>> GetCohortsWithoutDepartment();

        Task<PagedResultDto<CohortUserForTransferDto>> GetCohortUsersForTransfer(GetCohortUsersForTransferInput input);

        // Utility Methods
        Task<int> GetCohortUsersCount(Guid cohortId);

        Task<List<RequirementCategoryAnalysisDto>> GetCohortRequirementCategories(Guid cohortId);

        //Task<bool> CanTransferUsers(Guid cohortId);

        // Transfer History
        Task<PagedResultDto<UserTransferHistoryDto>> GetTransferHistory(GetAllUserTransferHistoryInput input);

        Task<UserTransferHistoryDto> GetTransferHistoryForView(Guid transferId);

        Task<FileDto> GetTransferHistoryToExcel(GetAllUserTransferHistoryInput input);

        // Rollback and Recovery
        Task<bool> CanRollbackTransfer(Guid transferId);

        Task<UserTransferRollbackResultDto> RollbackTransfer(UserTransferRollbackDto input);

        // Progress Tracking
        Task<UserTransferProgressDto> GetTransferProgress(string transferId);

        Task UpdateTransferProgress(string transferId, TransferProgressUpdateDto progressUpdate);

        Task<TransferProgressReportDto> GenerateTransferProgressReport(string transferId);

        Task<List<TransferProgressHistoryDto>> GetTransferProgressHistory(string transferId);

        Task<ProgressDataCleanupResultDto> CleanupProgressData(int retentionDays = 90);

        // Compliance State Preservation
        Task<CompliancePreservationResultDto> PreserveUserCompliance(Guid cohortId, string transferId);

        Task<ComplianceIntegrityValidationResultDto> ValidateComplianceIntegrity(Guid cohortId, string transferId,
            CompliancePreservationResultDto preservationResult = null);

        Task<ComplianceRecalculationResultDto> RecalculateCompliance(Guid cohortId, string transferId, Guid targetDepartmentId);

        // Lookup Tables
        Task<PagedResultDto<DepartmentLookupDto>> GetAllDepartmentsForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CohortTenantDepartmentLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input);
    }
}