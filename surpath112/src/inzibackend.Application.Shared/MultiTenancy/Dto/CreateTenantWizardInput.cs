using System;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.MultiTenancy;

namespace inzibackend.MultiTenancy.Dto
{
    public class CreateTenantWizardInput
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        [RegularExpression(TenantConsts.TenancyNameRegex)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(TenantConsts.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }

        [StringLength(AbpUserBase.MaxNameLength)]
        public string AdminName { get; set; }

        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string AdminSurname { get; set; }

        [StringLength(AbpUserBase.MaxPasswordLength)]
        [DisableAuditing]
        public string AdminPassword { get; set; }

        [MaxLength(AbpTenantBase.MaxConnectionStringLength)]
        [DisableAuditing]
        public string ConnectionString { get; set; }

        public bool ShouldChangePasswordOnNextLogin { get; set; }

        public bool SendActivationEmail { get; set; }

        public int? EditionId { get; set; }

        public bool IsActive { get; set; }

        public DateTime? SubscriptionEndDateUtc { get; set; }

        public bool IsInTrialPeriod { get; set; }

        // Surpath
        [Required]
        public string ClientCode { get; set; }

        public string UserAdminName { get; set; }
        public string UserAdminMiddlename { get; set; }
        public string UserAdminSurname { get; set; }
        public string UserAdminEmailAddress { get; set; }
        public string UserAdminPhoneNumber { get; set; }
        public string UserAdminUserName { get; set; }
        public string UserAdminPassword { get; set; }
        public bool UserAdminIsActive { get; set; }
        public bool UserAdminShouldChangePasswordOnNextLogin { get; set; }

        public virtual bool UserAdminIsTwoFactorEnabled { get; set; }

        public virtual bool UserAdminIsLockoutEnabled { get; set; }

        public bool IsDonorPay { get; set; } = false;
        public bool IsDeferDonorPerpetualPay { get; set; } = false;
        public int? ClientPaymentType { get; set; } = 0;
    }
}