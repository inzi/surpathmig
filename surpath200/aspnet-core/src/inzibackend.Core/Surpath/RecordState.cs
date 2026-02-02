using inzibackend.Surpath;
using inzibackend.Surpath;
using inzibackend.Authorization.Users;
using inzibackend.Surpath;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("RecordStates")]
    [Audited]
    public class RecordState : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual EnumRecordState State { get; set; }

        public virtual string Notes { get; set; }

        public virtual Guid? RecordId { get; set; }

        [ForeignKey("RecordId")]
        public Record RecordFk { get; set; }

        public virtual Guid? RecordCategoryId { get; set; }

        [ForeignKey("RecordCategoryId")]
        public RecordCategory RecordCategoryFk { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        public virtual Guid RecordStatusId { get; set; }

        [ForeignKey("RecordStatusId")]
        public RecordStatus RecordStatusFk { get; set; }

        #region IHasArchiving Members

        /// <summary>
        /// Indicates if this record state is archived (not the current/active document).
        /// When a new document is uploaded, previous records are archived.
        /// Only non-archived records (IsArchived=false) are used for compliance calculations.
        /// </summary>
        public virtual bool IsArchived { get; set; } = false;

        /// <summary>
        /// The date and time when this record was archived.
        /// Automatically set when IsArchived changes from false to true.
        /// </summary>
        public virtual DateTime? ArchivedTime { get; set; }

        /// <summary>
        /// The user who caused this record to be archived (typically by uploading a new document).
        /// Automatically set when IsArchived changes from false to true.
        /// </summary>
        public virtual long? ArchivedByUserId { get; set; }

        #endregion
    }
}