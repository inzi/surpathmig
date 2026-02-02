using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.MultiTenancy;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;

namespace inzibackend.MultiTenancy.Dto
{
    public class TenantInfoDto : EntityDto
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(TenantConsts.MaxNameLength)]
        public string Name { get; set; }

    }
}