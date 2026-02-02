using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using inzibackend.Authorization.Users;
using inzibackend.MultiTenancy;
using inzibackend.Surpath;

namespace inzibackend.Surpath.Registration
{
    public interface IRegistrationPaymentCoordinator
    {
        Task<RegistrationPaymentResult> ExecuteAsync(RegistrationPaymentRequest request);
    }

    public class RegistrationPaymentRequest
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string MiddleName { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string Address { get; set; }

        public string SuiteApt { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public Guid TenantDepartmentId { get; set; }

        public Guid CohortId { get; set; }

        public IReadOnlyList<UserPid> UserPids { get; set; } = Array.Empty<UserPid>();

        public bool IsExternalLogin { get; set; }

        public ExternalLoginInfo ExternalLoginInfo { get; set; }

        public bool TenantIsDonorPay { get; set; }

        public AuthNetSubmit Payment { get; set; }

        public AuthNetCaptureResultDto CaptureResult { get; set; }

        public bool SkipPaymentProcessing { get; set; }

        public decimal? AmountDueOverride { get; set; }
    }

    public class RegistrationPaymentResult
    {
        public User User { get; set; }

        public Tenant Tenant { get; set; }

        public bool PaymentCaptured { get; set; }

        public decimal CapturedAmount { get; set; }

        public decimal AmountDue { get; set; }

        public AuthNetTransactionResult GatewayResult { get; set; }
    }
}

