using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AspNetZeroCore.Validation;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using inzibackend.Authorization.Users;
using inzibackend.Security;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using inzibackend.Web.Areas.App.Models.UserPids;

namespace inzibackend.Web.Models.Account
{
    public class RegisterViewModel : IValidatableObject
    {
        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(User.MaxSurnameLength)]
        public string Surname { get; set; }

        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [StringLength(User.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public bool IsExternalLogin { get; set; }

        public string ExternalLoginAuthSchema { get; set; }

        public string ReturnUrl { get; set; }

        public string SingleSignIn { get; set; }

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!UserName.IsNullOrEmpty())
            {
                if (!UserName.Equals(EmailAddress, StringComparison.OrdinalIgnoreCase) && ValidationHelper.IsEmail(UserName))
                {
                    yield return new ValidationResult("Username cannot be an email address unless it's same with your email address !");
                }
            }
        }

        //[Required]
        [StringLength(User.MaxNameLength)]
        public string Middlename { get; set; }


        [Required]
        public string Address { get; set; }

        public string SuiteApt { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public List<UserPidViewModel> UserPidViewModels { get; set; } = new List<UserPidViewModel>();
        public Guid? TenantDepartmentId { get; set; }
        public Guid? CohortId { get; set; }

        [StringLength(UserConsts.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        public bool IsDonorPay { get; set; } = false;
        public AuthNetSubmit AuthNetSubmit { get; set; } = new AuthNetSubmit();
        public AuthNetCaptureResultDto AuthNetCaptureResultDto { get; set; } = new AuthNetCaptureResultDto();
        public decimal AmountDue { get; set; }
        // public List<SurpathServiceDto> SurpathServices { get; set; } = new List<SurpathServiceDto>();
        public List<TenantSurpathServiceDto> TenantSurpathServices { get; set; } = new List<TenantSurpathServiceDto>();
        public bool UseSandboxPayment { get; set; } = true;
        public int RegisterCheckoutBtnClick { get; set; } = 0;
    }
}