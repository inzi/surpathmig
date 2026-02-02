using inzibackend.Surpath;
using inzibackend.Surpath;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("ConfirmationValues")]
    [Audited]
    public class ConfirmationValue : FullAuditedEntity<Guid>
    {

        public virtual double ScreenValue { get; set; }

        public virtual double ConfirmValue { get; set; }

        public virtual EnumUnitOfMeasurement UnitOfMeasurement { get; set; }

        public virtual Guid DrugId { get; set; }

        [ForeignKey("DrugId")]
        public Drug DrugFk { get; set; }

        public virtual Guid TestCategoryId { get; set; }

        [ForeignKey("TestCategoryId")]
        public TestCategory TestCategoryFk { get; set; }

    }
}