using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditDeptCodeDto : EntityDto<Guid?>
    {

        [Required]
        [StringLength(DeptCodeConsts.MaxCodeLength, MinimumLength = DeptCodeConsts.MinCodeLength)]
        public string Code { get; set; }

        public Guid CodeTypeId { get; set; }

        public Guid TenantDepartmentId { get; set; }

    }
}