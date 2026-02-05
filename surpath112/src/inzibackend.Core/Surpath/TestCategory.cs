using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("TestCategories")]
    [Audited]
    public class TestCategory : FullAuditedEntity<Guid>
    {

        [Required]
        [StringLength(TestCategoryConsts.MaxNameLength, MinimumLength = TestCategoryConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(TestCategoryConsts.MaxInternalNameLength, MinimumLength = TestCategoryConsts.MinInternalNameLength)]
        public virtual string InternalName { get; set; }

    }
}