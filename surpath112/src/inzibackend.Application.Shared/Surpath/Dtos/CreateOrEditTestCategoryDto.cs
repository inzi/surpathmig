using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditTestCategoryDto : EntityDto<Guid?>
    {

        [Required]
        [StringLength(TestCategoryConsts.MaxNameLength, MinimumLength = TestCategoryConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(TestCategoryConsts.MaxInternalNameLength, MinimumLength = TestCategoryConsts.MinInternalNameLength)]
        public string InternalName { get; set; }

    }
}