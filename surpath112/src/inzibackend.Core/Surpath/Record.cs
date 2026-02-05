using inzibackend.Surpath;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("Records")]
    [Audited]
    public class Record : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        //File

        public virtual Guid? filedata { get; set; } //File, (BinaryObjectId)

        public virtual string filename { get; set; }

        public virtual string physicalfilepath { get; set; }

        public virtual string metadata { get; set; }

        public virtual Guid BinaryObjId { get; set; }

        public virtual DateTime? DateUploaded { get; set; }

        public virtual DateTime? DateLastUpdated { get; set; }

        public virtual bool InstructionsConfirmed { get; set; }

        public virtual DateTime? EffectiveDate { get; set; }

        public virtual DateTime? ExpirationDate { get; set; }

        public virtual Guid? TenantDocumentCategoryId { get; set; }

        [ForeignKey("TenantDocumentCategoryId")]
        public TenantDocumentCategory TenantDocumentCategoryFk { get; set; }

    }
}