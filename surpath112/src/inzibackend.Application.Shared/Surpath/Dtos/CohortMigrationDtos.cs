using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Compliance;
using Newtonsoft.Json;

namespace inzibackend.Surpath.Dtos
{
    // Main Migration DTOs
    public class CohortMigrationDto
    {
        public Guid CohortId { get; set; }
        public Guid? TargetDepartmentId { get; set; } // null if creating new department
        public string NewDepartmentName { get; set; } // required if TargetDepartmentId is null
        public string NewDepartmentDescription { get; set; }
        public List<RequirementCategoryMappingDto> CategoryMappings { get; set; } = new List<RequirementCategoryMappingDto>();
        public bool ConfirmMigration { get; set; }
    }

    public class CohortMigrationResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? NewDepartmentId { get; set; }
        public int AffectedUsersCount { get; set; }
        public int MigratedRecordStatesCount { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
        public DateTime? MigrationStartTime { get; set; }
        public DateTime? MigrationEndTime { get; set; }
        public string MigrationId { get; set; }
    }

    // Requirement Category Mapping DTOs
    public class RequirementCategoryMappingDto
    {
        public Guid SourceCategoryId { get; set; }
        public string SourceCategoryName { get; set; }
        public string SourceRequirementName { get; set; }
        public Guid SourceRequirementId { get; set; }
        public MappingAction Action { get; set; } // Map, Copy, Skip
        public Guid? TargetCategoryId { get; set; } // for Map action
        public string NewRequirementName { get; set; } // for Copy action
        public string NewCategoryName { get; set; } // for Copy action
        public int AffectedRecordStatesCount { get; set; }
        public int AffectedUsersCount { get; set; }
        public bool HasDataLoss { get; set; } // true for Skip action
    }

    public enum MappingAction
    {
        MapToExisting = 1, // Map to existing category in target department
        CopyToNew = 2,     // Copy category to target department
        Skip = 3           // Skip this category (will lose data)
    }

    // Validation and Analysis DTOs
    public class CohortMigrationAnalysisDto
    {
        public Guid CohortId { get; set; }
        public string CohortName { get; set; }
        public Guid SourceDepartmentId { get; set; }
        public string SourceDepartmentName { get; set; }
        public int TotalUsersCount { get; set; }
        public List<RequirementCategoryAnalysisDto> RequirementCategories { get; set; } = new List<RequirementCategoryAnalysisDto>();
        public List<RequirementCategoryAnalysisDto> NoMigrationRequiredCategories { get; set; } = new List<RequirementCategoryAnalysisDto>();
        public List<string> Warnings { get; set; } = new List<string>();
        public bool CanMigrate { get; set; }
        public string MigrationComplexity { get; set; } // Simple, Moderate, Complex
        public int EstimatedDurationMinutes { get; set; }
        public bool IsSurpathOnly { get; set; } // Whether this migration is only for Surpath requirements
    }

    public class RequirementCategoryAnalysisDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid RequirementId { get; set; }
        public string RequirementName { get; set; }
        public bool IsDepartmentSpecific { get; set; }
        public int RecordStatesCount { get; set; }
        public int AffectedUsersCount { get; set; }
        public List<TargetCategoryOptionDto> TargetOptions { get; set; } = new List<TargetCategoryOptionDto>();
        public bool RequiresMapping { get; set; }

        /// <summary>
        /// Hierarchy level: Tenant, Department, Cohort, CohortAndDepartment
        /// </summary>
        public string HierarchyLevel { get; set; }

        /// <summary>
        /// Whether this requirement is specific to the cohort
        /// </summary>
        public bool IsCohortSpecific { get; set; }

        /// <summary>
        /// Whether this requirement category is Surpath-only (not tenant-specific)
        /// </summary>
        public bool IsSurpathOnly { get; set; }
    }

    public class TargetCategoryOptionDto
    {
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid? RequirementId { get; set; }
        public string RequirementName { get; set; }
        public string RequirementDescription { get; set; }
        public string Instructions { get; set; }
        public bool IsExactMatch { get; set; }
        public bool IsSimilarMatch { get; set; }
        public int MatchScore { get; set; } // 0-100 similarity score
        public double RecommendationScore { get; set; } // Enhanced recommendation score
        public CategoryUsageStatisticsDto UsageStatistics { get; set; }
    }

    public class CategoryUsageStatisticsDto
    {
        public int TotalRecordStates { get; set; }
        public int ActiveUsers { get; set; }
        public double ComplianceRate { get; set; } // Percentage of compliant users
    }

    // Department DTOs
    public class DepartmentLookupDto
    {
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentDescription { get; set; }
        public int CohortsCount { get; set; }
        public int RequirementsCount { get; set; }
        public int UsersCount { get; set; }
        public bool IsActive { get; set; }

        // Enhanced statistics for migration planning
        public int ActiveCohortsCount { get; set; }

        public int TotalRecordStatesCount { get; set; }
        public double DepartmentUtilization { get; set; } // Percentage of approved vs total record states
        public double MigrationCompatibilityScore { get; set; } // 0-100 score for migration suitability
    }

    public class CreateDepartmentDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public int? TenantId { get; set; }
        public bool Active { get; set; } = true;
    }

    // Migration History and Audit DTOs
    public class CohortMigrationHistoryDto : EntityDto<Guid>
    {
        public Guid CohortId { get; set; }
        public string CohortName { get; set; }
        public Guid SourceDepartmentId { get; set; }
        public string SourceDepartmentName { get; set; }
        public Guid TargetDepartmentId { get; set; }
        public string TargetDepartmentName { get; set; }
        public DateTime MigrationDate { get; set; }
        public string MigrationStatus { get; set; } // InProgress, Completed, Failed, RolledBack
        public int AffectedUsersCount { get; set; }
        public int MigratedRecordStatesCount { get; set; }
        public string MigrationData { get; set; } // JSON data of migration details
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ErrorMessage { get; set; }
    }

    // Input DTOs for various operations
    public class GetAllCohortMigrationHistoryInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public Guid? CohortId { get; set; }
        public Guid? DepartmentId { get; set; }
        public string Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class ValidateMigrationMappingsInput
    {
        public Guid CohortId { get; set; }
        public Guid? TargetDepartmentId { get; set; }
        public List<RequirementCategoryMappingDto> Mappings { get; set; } = new List<RequirementCategoryMappingDto>();
    }

    public class GetTargetCategoryOptionsInput
    {
        public Guid SourceCategoryId { get; set; }
        public Guid? TargetDepartmentId { get; set; }
        public Guid? TargetCohortId { get; set; } // Target cohort for cohort-specific requirements
        public int? MinMatchScore { get; set; } // Minimum match score threshold
        public int? MaxResults { get; set; } // Maximum number of results to return
        public string SearchFilter { get; set; } // Optional search filter
        public bool? PrioritizeHighMatches { get; set; } // Whether to prioritize exact/similar matches
    }

    // Department Selection Validation DTOs
    public class ValidateDepartmentSelectionInput
    {
        public Guid CohortId { get; set; }
        public Guid? TargetDepartmentId { get; set; } // null if creating new department
        public string NewDepartmentName { get; set; } // required if TargetDepartmentId is null
        public string NewDepartmentDescription { get; set; }
        public bool ValidateCapacity { get; set; } = true; // Whether to validate department capacity
        public bool ValidatePermissions { get; set; } = true; // Whether to validate user permissions
    }

    public class DepartmentSelectionValidationResultDto
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public DepartmentCompatibilityInfoDto CompatibilityInfo { get; set; }
        public DepartmentCapacityInfoDto CapacityInfo { get; set; }
    }

    public class DepartmentCompatibilityInfoDto
    {
        public double CompatibilityScore { get; set; } // 0-100 score
        public int SimilarRequirementsCount { get; set; }
        public int ConflictingRequirementsCount { get; set; }
        public List<string> SimilarRequirements { get; set; } = new List<string>();
        public List<string> ConflictingRequirements { get; set; } = new List<string>();
        public bool HasSimilarStructure { get; set; }
    }

    public class DepartmentCapacityInfoDto
    {
        public int CurrentUsersCount { get; set; }
        public int CurrentCohortsCount { get; set; }
        public int EstimatedCapacity { get; set; }
        public double UtilizationPercentage { get; set; }
        public bool IsNearCapacity { get; set; } // true if >80% capacity
        public bool IsOverCapacity { get; set; } // true if >100% capacity
        public string CapacityRecommendation { get; set; }
    }

    // Rollback DTOs
    public class CohortMigrationRollbackDto
    {
        public Guid MigrationId { get; set; }
        public string Reason { get; set; }
        public bool ConfirmRollback { get; set; }
    }

    public class CohortMigrationRollbackResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int RestoredUsersCount { get; set; }
        public int RestoredRecordStatesCount { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
        public DateTime? RollbackStartTime { get; set; }
        public DateTime? RollbackEndTime { get; set; }
    }

    // Progress tracking DTOs
    public class CohortMigrationProgressDto
    {
        public string MigrationId { get; set; }
        public string Status { get; set; }
        public int ProgressPercentage { get; set; }
        public string CurrentStep { get; set; }
        public int ProcessedUsers { get; set; }
        public int TotalUsers { get; set; }
        public int ProcessedRecordStates { get; set; }
        public int TotalRecordStates { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EstimatedEndTime { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }

    // Compliance State Preservation DTOs
    public class CompliancePreservationResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int PreservedUsersCount { get; set; }
        public int PreservedRecordStatesCount { get; set; }
        public string MigrationId { get; set; }
        public string TransferId { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<UserComplianceSnapshotDto> ComplianceSnapshots { get; set; } = new List<UserComplianceSnapshotDto>();
    }

    public class UserComplianceSnapshotDto
    {
        public long UserId { get; set; }
        public Guid CohortId { get; set; }
        public string MigrationId { get; set; }
        public DateTime SnapshotTimestamp { get; set; }

        // Current compliance state
        public ComplianceValues ComplianceValues { get; set; }

        // Record states
        public List<RecordStateSnapshotDto> RecordStates { get; set; } = new List<RecordStateSnapshotDto>();

        public int RecordStatesCount { get; set; }

        // Summary statistics
        public int CompliantRecordStatesCount { get; set; }

        public int NonCompliantRecordStatesCount { get; set; }
        public int InformationalRecordStatesCount { get; set; }

        // File associations
        public bool HasFileAssociations { get; set; }

        public int FileAssociationsCount { get; set; }
    }

    public class RecordStateSnapshotDto
    {
        public Guid RecordStateId { get; set; }
        public long UserId { get; set; }
        public Guid? RecordCategoryId { get; set; }
        public Guid? RecordId { get; set; }
        public Guid RecordStatusId { get; set; }
        public EnumRecordState State { get; set; }
        public string Notes { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        // Category and requirement information
        public string CategoryName { get; set; }

        public string RequirementName { get; set; }
        public Guid? RequirementId { get; set; }
        public bool IsSurpathOnly { get; set; }

        // Status information
        public string StatusName { get; set; }

        public EnumStatusComplianceImpact ComplianceImpact { get; set; }

        // File information
        public string FileName { get; set; }

        public long? FileSize { get; set; }
        public DateTime? ExpirationDate { get; set; }

        // Snapshot metadata
        public DateTime SnapshotTimestamp { get; set; }

        public string MigrationId { get; set; }
    }

    // Compliance Integrity Validation DTOs
    public class ComplianceIntegrityValidationResultDto
    {
        public Guid CohortId { get; set; }
        public string MigrationId { get; set; }
        public string TransferId { get; set; }

        public DateTime ValidationTimestamp { get; set; }
        public bool IsValid { get; set; }
        public string ValidationSummary { get; set; }

        // Statistics
        public int TotalUsersValidated { get; set; }

        public int OrphanedRecordsCount { get; set; }
        public int MissingRecordsCount { get; set; }
        public int IntegrityIssuesCount { get; set; }

        // Results and messages
        public List<ComplianceValidationItemDto> ValidationResults { get; set; } = new List<ComplianceValidationItemDto>();

        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    public class ComplianceValidationItemDto
    {
        public long UserId { get; set; }
        public bool IsValid { get; set; }
        public List<string> ValidationIssues { get; set; } = new List<string>();

        // Current state information
        public int RecordStatesCount { get; set; }

        public int CompliantRecordStatesCount { get; set; }
        public int OrphanedRecordsCount { get; set; }
        public int MissingRecordsCount { get; set; }

        // Compliance values
        public ComplianceValues CurrentComplianceValues { get; set; }
    }

    // Compliance Recalculation DTOs
    public class ComplianceRecalculationResultDto
    {
        public Guid CohortId { get; set; }
        public string MigrationId { get; set; }
        public string TransferId { get; set; }

        public Guid TargetDepartmentId { get; set; }
        public DateTime RecalculationTimestamp { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        // Statistics
        public int TotalUsersProcessed { get; set; }

        public int SuccessfulRecalculations { get; set; }
        public int FailedRecalculations { get; set; }

        // Results and messages
        public List<UserComplianceRecalculationDto> UserRecalculationResults { get; set; } = new List<UserComplianceRecalculationDto>();

        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    public class UserComplianceRecalculationDto
    {
        public long UserId { get; set; }
        public bool Success { get; set; }
        public List<string> RecalculationIssues { get; set; } = new List<string>();

        // Compliance state information
        public ComplianceValues PreRecalculationCompliance { get; set; }

        public ComplianceValues PostRecalculationCompliance { get; set; }
        public List<string> ComplianceChanges { get; set; } = new List<string>();
    }

    // Department Requirement DTOs for compliance validation
    public class DepartmentRequirementDto
    {
        public Guid RequirementId { get; set; }
        public string RequirementName { get; set; }
        public string Description { get; set; }
        public bool IsSurpathOnly { get; set; }
        public bool IsRequired { get; set; }
        public List<DepartmentCategoryDto> Categories { get; set; } = new List<DepartmentCategoryDto>();
    }

    public class DepartmentCategoryDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsRequired { get; set; }
    }

    // Migration Execution and State Management DTOs
    public class PreMigrationValidationResult
    {
        public bool CanProceed { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    public class PostMigrationValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
    }

    public class MigrationStateSnapshot
    {
        public string MigrationId { get; set; }
        public Guid CohortId { get; set; }
        public DateTime CaptureTimestamp { get; set; }

        // Original state data
        public CohortStateSnapshot OriginalCohortState { get; set; }

        public List<long> CohortUserIds { get; set; } = new List<long>();
        public List<RecordStateSnapshotDto> OriginalRecordStates { get; set; } = new List<RecordStateSnapshotDto>();

        // Summary statistics
        public int TotalUsersCount { get; set; }

        public int TotalRecordStatesCount { get; set; }
    }

    public class CohortStateSnapshot
    {
        public Guid CohortId { get; set; }
        public string Name { get; set; }
        public Guid? TenantDepartmentId { get; set; }
        public int? TenantId { get; set; }
        public bool IsDeleted { get; set; }
    }

    // Rollback Validation DTOs
    public class RollbackValidationResult
    {
        public bool CanRollback { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    public class RollbackIntegrityValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
    }

    // Migration Audit Logging DTOs
    public class MigrationAuditData
    {
        public Guid CohortId { get; set; }
        public string OperationType { get; set; }
        public string OperationStatus { get; set; }
        public string UserName { get; set; }

        // State information
        public object BeforeState { get; set; }

        public object AfterState { get; set; }

        // Performance metrics
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public double DurationMinutes { get; set; }
        public int ProcessedUsersCount { get; set; }
        public int ProcessedRecordStatesCount { get; set; }

        // Operation details
        public Guid? SourceDepartmentId { get; set; }

        public Guid? TargetDepartmentId { get; set; }
        public int CategoryMappingsCount { get; set; }

        // Results and issues
        public bool Success { get; set; }

        public int ErrorsCount { get; set; }
        public int WarningsCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();

        // Category mapping decisions
        public List<CategoryMappingDecisionDto> CategoryMappingDecisions { get; set; } = new List<CategoryMappingDecisionDto>();

        // Additional metadata
        public Dictionary<string, object> AdditionalMetadata { get; set; } = new Dictionary<string, object>();
    }

    public class CategoryMappingDecisionDto
    {
        public Guid SourceCategoryId { get; set; }
        public string SourceCategoryName { get; set; }
        public MappingAction Action { get; set; }
        public Guid? TargetCategoryId { get; set; }
        public string TargetCategoryName { get; set; }
        public int AffectedUsersCount { get; set; }
        public int AffectedRecordStatesCount { get; set; }
        public bool HasDataLoss { get; set; }
    }

    public class MigrationAuditRecord
    {
        public string MigrationId { get; set; }
        public Guid CohortId { get; set; }
        public DateTime AuditTimestamp { get; set; }
        public string OperationType { get; set; }
        public string OperationStatus { get; set; }

        // User and tenant context
        public long? UserId { get; set; }

        public int? TenantId { get; set; }
        public string UserName { get; set; }

        // Before/After states
        public string BeforeState { get; set; }

        public string AfterState { get; set; }

        // Performance metrics
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public double DurationMinutes { get; set; }
        public int ProcessedUsersCount { get; set; }
        public int ProcessedRecordStatesCount { get; set; }

        // Operation details
        public Guid? SourceDepartmentId { get; set; }

        public Guid? TargetDepartmentId { get; set; }
        public int CategoryMappingsCount { get; set; }

        // Results and issues
        public bool Success { get; set; }

        public int ErrorsCount { get; set; }
        public int WarningsCount { get; set; }
        public string ErrorDetails { get; set; }
        public string WarningDetails { get; set; }

        // Additional metadata
        public string Metadata { get; set; }
    }

    public class MigrationPerformanceMetrics
    {
        public string MigrationId { get; set; }
        public string OperationType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double TotalDurationMinutes { get; set; }

        // Processing metrics
        public int UsersProcessed { get; set; }

        public int RecordStatesProcessed { get; set; }
        public int CategoryMappingsProcessed { get; set; }

        // Performance calculations
        public double UsersPerMinute { get; set; }

        public double RecordStatesPerMinute { get; set; }

        // System metrics
        public int? TenantId { get; set; }

        public bool Success { get; set; }
        public int ErrorsCount { get; set; }
        public int WarningsCount { get; set; }

        // Additional performance data
        public string PerformanceData { get; set; }
    }

    public class MigrationAuditExportDto
    {
        public string MigrationId { get; set; }
        public DateTime ExportTimestamp { get; set; }
        public long? ExportedBy { get; set; }
        public int? TenantId { get; set; }

        // Audit data
        public List<MigrationAuditRecord> AuditRecords { get; set; } = new List<MigrationAuditRecord>();

        public List<MigrationPerformanceMetrics> PerformanceMetrics { get; set; } = new List<MigrationPerformanceMetrics>();
        public List<string> UserActions { get; set; } = new List<string>();

        // Export metadata
        public Dictionary<string, object> ExportMetadata { get; set; } = new Dictionary<string, object>();
    }

    public class AuditDataCleanupResultDto
    {
        public DateTime CleanupTimestamp { get; set; }
        public int RetentionPolicyDays { get; set; }
        public DateTime CutoffDate { get; set; }
        public long? PerformedBy { get; set; }

        // Cleanup results
        public int DeletedAuditRecords { get; set; }

        public int DeletedPerformanceMetrics { get; set; }
        public int ArchivedRecords { get; set; }

        // Status
        public bool Success { get; set; }

        public string Message { get; set; }
    }

    public class AuditIntegrityValidationResultDto
    {
        public string MigrationId { get; set; }
        public DateTime ValidationTimestamp { get; set; }
        public long? ValidatedBy { get; set; }
        public bool IsValid { get; set; }
        public List<string> ValidationIssues { get; set; } = new List<string>();
    }

    // Progress Tracking DTOs
    public class MigrationProgressUpdateDto
    {
        public string Status { get; set; }
        public int ProgressPercentage { get; set; }
        public string CurrentStep { get; set; }
        public string CurrentOperation { get; set; }
        public string Message { get; set; }

        // Processing metrics
        public int ProcessedUsers { get; set; }

        public int TotalUsers { get; set; }
        public int ProcessedRecordStates { get; set; }
        public int TotalRecordStates { get; set; }
        public int ProcessedCategoryMappings { get; set; }
        public int TotalCategoryMappings { get; set; }

        // Timing information
        public DateTime StartTime { get; set; }

        public DateTime CurrentStepStartTime { get; set; }
        public DateTime? EstimatedEndTime { get; set; }

        // Performance metrics
        public double ProcessingRate { get; set; }

        public double AverageStepDuration { get; set; }

        // Error tracking
        public int ErrorsCount { get; set; }

        public int WarningsCount { get; set; }

        // Additional data
        public List<string> DetailedMessages { get; set; } = new List<string>();

        public Dictionary<string, object> AdditionalMetadata { get; set; } = new Dictionary<string, object>();
    }

    public class MigrationProgressData
    {
        public string MigrationId { get; set; }
        public string Status { get; set; }
        public int ProgressPercentage { get; set; }
        public string CurrentStep { get; set; }
        public int ProcessedUsers { get; set; }
        public int TotalUsers { get; set; }
        public int ProcessedRecordStates { get; set; }
        public int TotalRecordStates { get; set; }
        public DateTime StartTime { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }

    public class MigrationProgressRecord
    {
        public string MigrationId { get; set; }
        public DateTime UpdateTimestamp { get; set; }
        public string Status { get; set; }
        public int ProgressPercentage { get; set; }
        public string CurrentStep { get; set; }
        public string CurrentOperation { get; set; }
        public string Message { get; set; }

        // Processing metrics
        public int ProcessedUsers { get; set; }

        public int TotalUsers { get; set; }
        public int ProcessedRecordStates { get; set; }
        public int TotalRecordStates { get; set; }
        public int ProcessedCategoryMappings { get; set; }
        public int TotalCategoryMappings { get; set; }

        // Timing information
        public DateTime StartTime { get; set; }

        public DateTime CurrentStepStartTime { get; set; }
        public DateTime? EstimatedEndTime { get; set; }

        // Performance metrics
        public double ProcessingRate { get; set; }

        public double AverageStepDuration { get; set; }

        // Error tracking
        public int ErrorsCount { get; set; }

        public int WarningsCount { get; set; }

        // Additional data
        public List<string> DetailedMessages { get; set; } = new List<string>();

        public string Metadata { get; set; }
    }

    public class MigrationProgressReportDto
    {
        public string MigrationId { get; set; }
        public DateTime ReportGeneratedAt { get; set; }
        public long? GeneratedBy { get; set; }
        public string Status { get; set; }
        public string Summary { get; set; }

        // Timing information
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public TimeSpan TotalDuration { get; set; }

        // Progress information
        public int FinalProgressPercentage { get; set; }

        public int TotalSteps { get; set; }

        // Performance metrics
        public double AverageUsersPerMinute { get; set; }

        public double AverageRecordStatesPerMinute { get; set; }

        // Detailed breakdowns
        public List<MigrationStepSummaryDto> StepBreakdown { get; set; } = new List<MigrationStepSummaryDto>();

        public MigrationPerformanceAnalysisDto PerformanceMetrics { get; set; }
        public List<MigrationTimelineEventDto> Timeline { get; set; } = new List<MigrationTimelineEventDto>();
    }

    public class MigrationProgressHistoryDto
    {
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
        public int ProgressPercentage { get; set; }
        public string CurrentStep { get; set; }
        public string Message { get; set; }
        public int ProcessedUsers { get; set; }
        public int ProcessedRecordStates { get; set; }
    }

    public class ProgressDataCleanupResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime CleanupTimestamp { get; set; }
        public long? PerformedBy { get; set; }
        public int RetentionDays { get; set; }
        public DateTime CutoffDate { get; set; }
        public int DeletedRecords { get; set; }
        public int ArchivedRecords { get; set; }
    }

    public class MigrationStepSummaryDto
    {
        public string StepName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int StartProgressPercentage { get; set; }
        public int EndProgressPercentage { get; set; }
        public int ProcessedUsers { get; set; }
        public int ProcessedRecordStates { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }

    public class MigrationPerformanceAnalysisDto
    {
        public double PeakProcessingRate { get; set; }
        public double AverageProcessingRate { get; set; }
        public double MinimumProcessingRate { get; set; }
        public double AverageStepDuration { get; set; }
        public double LongestStepDuration { get; set; }
        public double ShortestStepDuration { get; set; }
        public int TotalErrors { get; set; }
        public int TotalWarnings { get; set; }
        public double ErrorRate { get; set; }
    }

    public class MigrationTimelineEventDto
    {
        public DateTime Timestamp { get; set; }
        public string EventType { get; set; }
        public string Description { get; set; }
        public int ProgressPercentage { get; set; }
        public int ProcessedUsers { get; set; }
        public int ProcessedRecordStates { get; set; }
    }
}