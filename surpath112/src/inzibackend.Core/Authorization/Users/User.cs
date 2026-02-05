using System;
using System.Collections.Generic;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using Abp.Timing;

namespace inzibackend.Authorization.Users
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User : AbpUser<User>
    {
        public virtual Guid? ProfilePictureId { get; set; }

        public virtual bool ShouldChangePasswordOnNextLogin { get; set; }

        public DateTime? SignInTokenExpireTimeUtc { get; set; }

        public string SignInToken { get; set; }

        public string GoogleAuthenticatorKey { get; set; }

        public List<UserOrganizationUnit> OrganizationUnits { get; set; }

        //Can add application specific user properties here

        public User()
        {
            IsLockoutEnabled = true;
            IsTwoFactorEnabled = true;
        }

        /// <summary>
        /// Creates admin <see cref="User"/> for a tenant.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="emailAddress">Email address</param>
        /// <param name="name">Name of admin user</param>
        /// <param name="surname">Surname of admin user</param>
        /// <returns>Created <see cref="User"/> object</returns>
        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string name = null, string surname = null, string middlename = null)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = string.IsNullOrWhiteSpace(name) ? AdminUserName : name,
                Surname = string.IsNullOrWhiteSpace(surname) ? AdminUserName : surname,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>(),
                OrganizationUnits = new List<UserOrganizationUnit>(),

                MiddleName = middlename,
                Address = "2030 G Avenue",
                SuiteApt = "STE 1102",
                City = "Plano",
                State = "TX",
                Zip = "75074",
                DateOfBirth = DateTime.Now,
            };

            user.SetNormalizedNames();

            return user;
        }

        public override void SetNewPasswordResetCode()
        {
            /* This reset code is intentionally kept short.
             * It should be short and easy to enter in a mobile application, where user can not click a link.
             */
            PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(10).ToUpperInvariant();
        }

        public void Unlock()
        {
            AccessFailedCount = 0;
            LockoutEndDateUtc = null;
        }

        public void SetSignInToken()
        {
            SignInToken = Guid.NewGuid().ToString();
            SignInTokenExpireTimeUtc = Clock.Now.AddMinutes(1).ToUniversalTime();
        }

        public string MiddleName { get; set; }
        public string Address { get; set; }

        public string SuiteApt { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool IsPaid { get; set; } = false;
        public bool IsAlwaysDonor { get; set; } = false;
        public DateTime DonorPayPromptDelayUntil { get; set; }
    }
}