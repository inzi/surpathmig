using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using inzibackend.Authorization.Users;
using inzibackend.Surpath.Dtos;
using inzibackend.Validation;

namespace inzibackend.Authorization.Accounts.Dto
{
    public class RegisterInput : IValidatableObject
    {
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        [DisableAuditing]
        public string CaptchaResponse { get; set; }

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

        public string Middlename { get; set; }


        [Required]
        public string Address { get; set; }

        public string SuiteApt { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [StringLength(UserConsts.MaxPhoneNumberLength, MinimumLength =UserConsts.MinPhoneNumberLength,ErrorMessage = "The phone number must be 10 digits")]
        public string PhoneNumber { get; set; }

        //public List<UserPidDto> UserPidViewModels { get; set; } = new List<UserPidDto>();
        //public Guid? TenantDepartmentId { get; set; }
        //public Guid? CohortId { get; set; }
    }
}