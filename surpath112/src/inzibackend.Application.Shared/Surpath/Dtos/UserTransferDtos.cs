using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Compliance;
using Newtonsoft.Json;

namespace inzibackend.Surpath.Dtos
{
    // Main Transfer DTOs
    public class UserTransferDto
    {
        public Guid CohortId { get; set; }
        public Guid? TargetDepartmentId { get; set; } // null if creating new department
        public string NewDepartmentName { get; set; } // required if TargetDepartmentId is null
        public string NewDepartmentDescription { get; set; }
        public List<RequirementCategoryMappingDto> CategoryMappings { get; set; } = new List<RequirementCategoryMappingDto>();
        public bool ConfirmTransfer { get; set; }
    }

    // New DTOs for selective user transfer
    public class GetCohortUsersForTransferInput : PagedAndSortedResultRequestDto
    {
        public Guid CohortId { get; set; }
        public string Filter { get; set; }
    }

    public class CohortUserForTransferDto
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ComplianceStatus { get; set; }
        public int RecordStatesCount { get; set; }
    }

    public class TransferSelectedUsersInput
    {
        public Guid SourceCohortId { get; set; }
        public Guid TargetCohortId { get; set; }
        public List<Guid> SelectedCohortUserIds { get; set; }
        public Dictionary<Guid, Guid> CategoryMappings { get; set; } = new Dictionary<Guid, Guid>();
        public Dictionary<Guid, NewCategoryDto> NewCategories { get; set; } = new Dictionary<Guid, NewCategoryDto>();
        public List<Guid> SkippedCategories { get; set; } = new List<Guid>();
        public List<Guid> NoTransferRequiredCategoryIds { get; set; } = new List<Guid>(); // Categories that exist in both source and target (e.g., tenant-wide requirements)
        public bool ConfirmTransfer { get; set; }
    }
    
    public class NewCategoryDto
    {
        public string Requirement { get; set; }
        public string Category { get; set; }
        public bool ConfirmedDepartmentLevel { get; set; }
    }

    public class AnalyzeUserTransferInput
    {
        public Guid SourceCohortId { get; set; }
        public Guid TargetCohortId { get; set; }
        public List<Guid> SelectedCohortUserIds { get; set; }
    }

    public class CohortLookupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UsersCount { get; set; }
        public bool IsDefaultCohort { get; set; }
    }

    public class UserTransferResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? NewDepartmentId { get; set; }
        public int AffectedUsersCount { get; set; }
        public int TransferredRecordStatesCount { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
        public DateTime? TransferStartTime { get; set; }
        public DateTime? TransferEndTime { get; set; }
        public string TransferId { get; set; }
    }

    // Validation and Analysis DTOs
    public class UserTransferAnalysisDto
    {
        public Guid CohortId { get; set; }
        public string CohortName { get; set; }
        public Guid SourceDepartmentId { get; set; }
        public string SourceDepartmentName { get; set; }
        public Guid? TargetCohortId { get; set; }
        public string TargetCohortName { get; set; }
        public Guid? TargetDepartmentId { get; set; }
        public string TargetDepartmentName { get; set; }
        public int TotalUsersCount { get; set; }
        public int SelectedUsersCount { get; set; }
        public List<RequirementCategoryAnalysisDto> RequirementCategories { get; set; } = new List<RequirementCategoryAnalysisDto>();
        public List<RequirementCategoryAnalysisDto> NoTransferRequiredCategories { get; set; } = new List<RequirementCategoryAnalysisDto>();
        public List<string> Warnings { get; set; } = new List<string>();
        public bool CanTransfer { get; set; }
        public string TransferComplexity { get; set; } // Simple, Moderate, Complex
        public int EstimatedDurationMinutes { get; set; }
        public bool IsSurpathOnly { get; set; } // Whether this transfer is only for Surpath requirements
        public bool RequiresCategoryMapping { get; set; } // Whether category mapping is needed (different departments)
    }

    // Transfer History and Audit DTOs
    public class UserTransferHistoryDto : EntityDto<Guid>
    {
        public Guid CohortId { get; set; }
        public string CohortName { get; set; }
        public Guid SourceDepartmentId { get; set; }
        public string SourceDepartmentName { get; set; }
        public Guid TargetDepartmentId { get; set; }
        public string TargetDepartmentName { get; set; }
        public DateTime TransferDate { get; set; }
        public string TransferStatus { get; set; } // InProgress, Completed, Failed, RolledBack
        public int AffectedUsersCount { get; set; }
        public int TransferredRecordStatesCount { get; set; }
        public string TransferData { get; set; } // JSON data of transfer details
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ErrorMessage { get; set; }
    }

    // Input DTOs for various operations
    public class GetAllUserTransferHistoryInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public Guid? CohortId { get; set; }
        public Guid? DepartmentId { get; set; }
        public string Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    // Rollback DTOs
    public class UserTransferRollbackDto
    {
        public Guid TransferId { get; set; }
        public string Reason { get; set; }
        public bool ConfirmRollback { get; set; }
    }

    public class UserTransferRollbackResultDto
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
    public class UserTransferProgressDto
    {
        public string TransferId { get; set; }
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

    // Transfer Audit Logging DTOs
    public class TransferAuditData
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

    public class TransferAuditRecord
    {
        public string TransferId { get; set; }
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

    public class TransferPerformanceMetrics
    {
        public string TransferId { get; set; }
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

    public class TransferAuditExportDto
    {
        public string TransferId { get; set; }
        public DateTime ExportTimestamp { get; set; }
        public long? ExportedBy { get; set; }
        public int? TenantId { get; set; }

        // Audit data
        public List<TransferAuditRecord> AuditRecords { get; set; } = new List<TransferAuditRecord>();

        public List<TransferPerformanceMetrics> PerformanceMetrics { get; set; } = new List<TransferPerformanceMetrics>();
        public List<string> UserActions { get; set; } = new List<string>();

        // Export metadata
        public Dictionary<string, object> ExportMetadata { get; set; } = new Dictionary<string, object>();
    }

    // Progress Tracking DTOs
    public class TransferProgressUpdateDto
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

    public class TransferProgressData
    {
        public string TransferId { get; set; }
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

    public class TransferProgressRecord
    {
        public string TransferId { get; set; }
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

    public class TransferProgressReportDto
    {
        public string TransferId { get; set; }
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
        public List<TransferStepSummaryDto> StepBreakdown { get; set; } = new List<TransferStepSummaryDto>();

        public TransferPerformanceAnalysisDto PerformanceMetrics { get; set; }
        public List<TransferTimelineEventDto> Timeline { get; set; } = new List<TransferTimelineEventDto>();
    }

    public class TransferProgressHistoryDto
    {
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
        public int ProgressPercentage { get; set; }
        public string CurrentStep { get; set; }
        public string Message { get; set; }
        public int ProcessedUsers { get; set; }
        public int ProcessedRecordStates { get; set; }
    }

    public class TransferStepSummaryDto
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

    public class TransferPerformanceAnalysisDto
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

    public class TransferTimelineEventDto
    {
        public DateTime Timestamp { get; set; }
        public string EventType { get; set; }
        public string Description { get; set; }
        public int ProgressPercentage { get; set; }
        public int ProcessedUsers { get; set; }
        public int ProcessedRecordStates { get; set; }
    }

    // Transfer Execution and State Management DTOs
    public class PreTransferValidationResult
    {
        public bool CanProceed { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    public class PostTransferValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
    }

    public class TransferStateSnapshot
    {
        public string TransferId { get; set; }
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

    //// Rollback Validation DTOs
    //public class RollbackValidationResult
    //{
    //    public bool CanRollback { get; set; }
    //    public List<string> Errors { get; set; } = new List<string>();
    //    public List<string> Warnings { get; set; } = new List<string>();
    //}

    //public class RollbackIntegrityValidationResult
    //{
    //    public bool IsValid { get; set; }
    //    public List<string> Warnings { get; set; } = new List<string>();
    //}
}