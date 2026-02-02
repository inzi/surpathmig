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
    public class TenantEditDto : EntityDto
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(TenantConsts.MaxNameLength)]
        public string Name { get; set; }

        [DisableAuditing]
        public string ConnectionString { get; set; }

        public int? EditionId { get; set; }

        public bool IsActive { get; set; }

        public DateTime? SubscriptionEndDateUtc { get; set; }

        public bool IsInTrialPeriod { get; set; }

        public List<TenantSurpathServiceDto> ServiceItems { get; set; }
        public bool IsDonorPay { get; set; } = false;
        public bool DeferDonorPay { get; set; } = false;
        public bool DeferDonorPerpetualPay { get; set; } = false;

        public EnumClientPaymentType ClientPaymentType { get; set; } = EnumClientPaymentType.InvoiceClient;
    }
}