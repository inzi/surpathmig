using System;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace inzibackend.Authorization.Users.Profile.Dto
{
    public class CurrentUserProfileEditDto
    {
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        //[Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Middlename{ get; set; }


        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [StringLength(UserConsts.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        public virtual bool IsPhoneNumberConfirmed { get; set; }

        public string Timezone { get; set; }

        public string QrCodeSetupImageUrl { get; set; }

        public bool IsGoogleAuthenticatorEnabled { get; set; }

        [Required]
        public string Address { get; set; }

        public string SuiteApt { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}