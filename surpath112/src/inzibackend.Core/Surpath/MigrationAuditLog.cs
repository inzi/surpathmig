using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("MigrationAuditLogs")]
    [Audited]
    public class MigrationAuditLog : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        /// <summary>
        /// Unique identifier for the migration operation
        /// </summary>
        [Required]
        [StringLength(100)]
        public virtual string MigrationId { get; set; }

        /// <summary>
        /// Type of migration operation (CohortMigration, DepartmentMigration, etc.)
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string MigrationType { get; set; }

        /// <summary>
        /// Current status of the migration (Started, InProgress, Completed, Failed)
        /// </summary>
        [Required]
        [StringLength(20)]
        public virtual string Status { get; set; }

        /// <summary>
        /// ID of the cohort being migrated
        /// </summary>
        public virtual Guid? CohortId { get; set; }

        /// <summary>
        /// Name of the cohort being migrated
        /// </summary>
        [StringLength(200)]
        public virtual string CohortName { get; set; }

        /// <summary>
        /// Source department ID
        /// </summary>
        public virtual Guid? SourceDepartmentId { get; set; }

        /// <summary>
        /// Source department name
        /// </summary>
        [StringLength(200)]
        public virtual string SourceDepartmentName { get; set; }

        /// <summary>
        /// Target department ID
        /// </summary>
        public virtual Guid? TargetDepartmentId { get; set; }

        /// <summary>
        /// Target department name
        /// </summary>
        [StringLength(200)]
        public virtual string TargetDepartmentName { get; set; }

        /// <summary>
        /// Whether a new department was created during migration
        /// </summary>
        public virtual bool IsNewDepartment { get; set; }

        /// <summary>
        /// Number of users affected by the migration
        /// </summary>
        public virtual int AffectedUsersCount { get; set; }

        /// <summary>
        /// Number of records affected by the migration
        /// </summary>
        public virtual int AffectedRecordsCount { get; set; }

        /// <summary>
        /// Number of requirement categories that needed mapping
        /// </summary>
        public virtual int RequirementCategoriesCount { get; set; }

        /// <summary>
        /// JSON serialized mapping decisions made during migration
        /// Format: [{"sourceCategory": "...", "action": "map|copy|skip", "targetCategory": "...", "newRequirement": "..."}]
        /// </summary>
        [Column(TypeName = "LONGTEXT")]
        public virtual string MappingDecisionsJson { get; set; }

        /// <summary>
        /// JSON serialized before state of key entities
        /// </summary>
        [Column(TypeName = "LONGTEXT")]
        public virtual string BeforeStateJson { get; set; }

        /// <summary>
        /// JSON serialized after state of key entities
        /// </summary>
        [Column(TypeName = "LONGTEXT")]
        public virtual string AfterStateJson { get; set; }

        /// <summary>
        /// Migration start timestamp
        /// </summary>
        public virtual DateTime? StartedAt { get; set; }

        /// <summary>
        /// Migration completion timestamp
        /// </summary>
        public virtual DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Duration of migration in milliseconds
        /// </summary>
        public virtual long? DurationMs { get; set; }

        /// <summary>
        /// Error message if migration failed
        /// </summary>
        [StringLength(2000)]
        public virtual string ErrorMessage { get; set; }

        /// <summary>
        /// Detailed error information (stack trace, etc.)
        /// </summary>
        [Column(TypeName = "LONGTEXT")]
        public virtual string ErrorDetails { get; set; }

        /// <summary>
        /// Additional metadata about the migration
        /// </summary>
        [Column(TypeName = "LONGTEXT")]
        public virtual string MetadataJson { get; set; }

        /// <summary>
        /// Whether the migration can be rolled back
        /// </summary>
        public virtual bool CanRollback { get; set; }

        /// <summary>
        /// Whether the migration has been rolled back
        /// </summary>
        public virtual bool IsRolledBack { get; set; }

        /// <summary>
        /// Timestamp when rollback was performed
        /// </summary>
        public virtual DateTime? RolledBackAt { get; set; }

        /// <summary>
        /// User who performed the rollback
        /// </summary>
        public virtual long? RolledBackByUserId { get; set; }

        /// <summary>
        /// Reason for rollback
        /// </summary>
        [StringLength(1000)]
        public virtual string RollbackReason { get; set; }

        // Navigation properties
        [ForeignKey("CohortId")]
        public virtual Cohort CohortFk { get; set; }

        [ForeignKey("SourceDepartmentId")]
        public virtual TenantDepartment SourceDepartmentFk { get; set; }

        [ForeignKey("TargetDepartmentId")]
        public virtual TenantDepartment TargetDepartmentFk { get; set; }
    }
}