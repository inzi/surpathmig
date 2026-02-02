using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizeNet.Api.Contracts.V1;
using Abp.Organizations;
using Abp.UI;
using Castle.MicroKernel.Registration;
using inzibackend.Surpath;
using inzibackend.Surpath.Registration;
using inzibackend.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace inzibackend.Tests.Surpath
{
    public class RegistrationWorkflow_Tests : AppTestBase
    {
        private readonly IRegistrationValidationManager _validationManager;
        private readonly IRegistrationPaymentCoordinator _paymentCoordinator;
        private readonly TestAuthNetGateway _gateway;

        public RegistrationWorkflow_Tests()
        {
            _gateway = new TestAuthNetGateway();
            LocalIocManager.IocContainer.Register(
                Component.For<IAuthNetGateway>()
                    .Instance(_gateway)
                    .LifestyleSingleton()
                    .IsDefault());

            _validationManager = Resolve<IRegistrationValidationManager>();
            _paymentCoordinator = Resolve<IRegistrationPaymentCoordinator>();
            _gateway.Reset();
        }

        [Fact]
        public async Task Registration_validation_should_detect_duplicate_email()
        {
            var existingUser = UsingDbContext(context =>
                context.Users.First(user => user.TenantId == AbpSession.TenantId));

            var enrollment = GetEnrollmentTargets();

            var request = new RegistrationValidationRequest
            {
                TenantId = AbpSession.TenantId,
                EmailAddress = existingUser.EmailAddress,
                UserName = Guid.NewGuid().ToString("N"),
                TenantDepartmentId = enrollment.department.Id,
                CohortId = enrollment.cohort.Id
            };

            await Assert.ThrowsAsync<UserFriendlyException>(() => _validationManager.EnsureValidAsync(request));
        }

        [Fact]
        public async Task Registration_should_bypass_payment_for_zero_balance()
        {
            var request = await BuildRegistrationRequestAsync(
                skipPaymentProcessing: true,
                tenantIsDonorPay: false,
                amountDueOverride: 0);

            var result = await _paymentCoordinator.ExecuteAsync(request);

            result.PaymentCaptured.ShouldBeFalse();
            result.AmountDue.ShouldBe(0);

            UsingDbContext(context =>
            {
                var persisted = context.Users.Single(u => u.Id == result.User.Id);
                persisted.IsActive.ShouldBeTrue();
                persisted.IsPaid.ShouldBeTrue();
            });
        }

        [Fact]
        public async Task Registration_should_create_ledger_when_gateway_capture_succeeds()
        {
            var paymentContext = GetBillableServices();
            var payment = BuildPayment(paymentContext.totalPrice, paymentContext.tenantServiceIds);
            var capture = BuildCaptureReceipt("TRANS-CAPTURE-1");
            _gateway.NextCaptureResult = TestGatewayHelpers.Success("CAPTURED-123", authCode: "AUTH123");

            var request = await BuildRegistrationRequestAsync(
                skipPaymentProcessing: false,
                tenantIsDonorPay: true,
                amountDueOverride: null,
                payment: payment,
                capture: capture);

            var result = await _paymentCoordinator.ExecuteAsync(request);

            result.PaymentCaptured.ShouldBeTrue();
            result.AmountDue.ShouldBe(0);

            UsingDbContext(context =>
            {
                var ledger = context.LedgerEntries.Single(le => le.UserId == result.User.Id);
                ledger.Amount.ShouldBe(payment.amount);
                ledger.PaidAmount.ShouldBe(payment.amount);
                ledger.AmountDue.ShouldBe(0);

                var details = context.LedgerEntryDetails.Where(d => d.LedgerEntryId == ledger.Id).ToList();
                details.ShouldNotBeEmpty();
                details.Sum(d => d.Amount).ShouldBe(ledger.TotalPrice);
            });
        }

        [Fact]
        public async Task Registration_should_void_pre_auth_when_capture_fails()
        {
            var paymentContext = GetBillableServices();
            var payment = BuildPayment(paymentContext.totalPrice, paymentContext.tenantServiceIds);
            var capture = BuildCaptureReceipt("TRANS-CAPTURE-2");
            _gateway.NextCaptureResult = TestGatewayHelpers.Failure("E100", "Declined");

            var request = await BuildRegistrationRequestAsync(
                skipPaymentProcessing: false,
                tenantIsDonorPay: true,
                amountDueOverride: null,
                payment: payment,
                capture: capture);

            await Assert.ThrowsAsync<UserFriendlyException>(() => _paymentCoordinator.ExecuteAsync(request));

            _gateway.VoidRequests.ShouldContain(tuple => tuple.transactionId == capture.TransactionId);

            UsingDbContext(context =>
            {
                var user = context.Users.Single(u => u.EmailAddress == request.EmailAddress);
                user.IsActive.ShouldBeFalse();
                user.IsPaid.ShouldBeFalse();
            });
        }

        private async Task<RegistrationPaymentRequest> BuildRegistrationRequestAsync(
            bool skipPaymentProcessing,
            bool tenantIsDonorPay,
            decimal? amountDueOverride,
            AuthNetSubmit payment = null,
            AuthNetCaptureResultDto capture = null)
        {
            var enrollment = GetEnrollmentTargets();
            var unique = Guid.NewGuid().ToString("N");

            var request = new RegistrationPaymentRequest
            {
                Name = "Reg",
                Surname = "Tester",
                MiddleName = "Unit",
                UserName = $"user-{unique}",
                EmailAddress = $"{unique}@example.com",
                Password = "Test123!",
                Address = $"ADDR-{unique}",
                SuiteApt = "Apt 1",
                City = "Testville",
                State = "TS",
                Zip = "99999",
                DateOfBirth = DateTime.UtcNow.AddYears(-25),
                PhoneNumber = "555-0100",
                TenantDepartmentId = enrollment.department.Id,
                CohortId = enrollment.cohort.Id,
                UserPids = Array.Empty<UserPid>(),
                IsExternalLogin = false,
                TenantIsDonorPay = tenantIsDonorPay,
                Payment = payment,
                CaptureResult = capture,
                SkipPaymentProcessing = skipPaymentProcessing,
                AmountDueOverride = amountDueOverride
            };

            return await Task.FromResult(request);
        }

        private (TenantDepartment department, Cohort cohort) GetEnrollmentTargets()
        {
            return UsingDbContext(EnsureEnrollmentTargets);
        }

        private (decimal totalPrice, List<Guid> tenantServiceIds) GetBillableServices()
        {
            return UsingDbContext(context =>
            {
                var enrollment = EnsureEnrollmentTargets(context);
                var service = context.TenantSurpathServices
                    .FirstOrDefault(s => s.TenantId == AbpSession.TenantId && !s.IsDeleted && s.SurpathServiceId != null);

                if (service == null)
                {
                    var surpathService = context.SurpathServices.Add(new SurpathService
                    {
                        TenantId = AbpSession.TenantId,
                        Name = "Integration Service",
                        Description = "Created by registration tests",
                        Price = 25,
                        Discount = 0,
                        IsEnabledByDefault = true
                    }).Entity;

                    service = context.TenantSurpathServices.Add(new TenantSurpathService
                    {
                        TenantId = AbpSession.TenantId,
                        Name = "Tenant Integration Service",
                        Description = "Created by registration tests",
                        Price = 25,
                        SurpathServiceId = surpathService.Id,
                        TenantDepartmentId = enrollment.department.Id,
                        IsInvoiced = false,
                        IsPricingOverrideEnabled = false
                    }).Entity;

                    context.SaveChanges();
                }

                var total = Convert.ToDecimal(service.Price);
                var ids = new List<Guid> { service.Id };
                return (total, ids);
            });
        }

        private static AuthNetSubmit BuildPayment(decimal amount, List<Guid> tenantServiceIds)
        {
            return new AuthNetSubmit
            {
                amount = amount,
                dataDescriptor = "COMMON.ACCEPT.INAPP.PAYMENT",
                dataValue = Guid.NewGuid().ToString("N"),
                TenantSurpathServiceIds = tenantServiceIds,
                FirstNameOnCard = "Reg",
                LastNameOnCard = "Tester",
                CardNameOnCard = "Reg Tester",
                BillingAddress = Guid.NewGuid().ToString("N"), // also used to locate user in failure test
                BillingCity = "Testville",
                BillingState = "TS",
                BillingZipCode = "99999",
                CardLastFour = "4242"
            };
        }

        private static AuthNetCaptureResultDto BuildCaptureReceipt(string transactionId)
        {
            return new AuthNetCaptureResultDto
            {
                TransactionId = transactionId,
                AuthCode = "AUTHCODE"
            };
        }

        private (TenantDepartment department, Cohort cohort) EnsureEnrollmentTargets(inzibackendDbContext context)
        {
            var department = context.TenantDepartments.FirstOrDefault(d =>
                                d.TenantId == AbpSession.TenantId && d.Active) ??
                             CreateDepartment(context);

            var cohort = context.Cohorts.FirstOrDefault(c =>
                              c.TenantId == AbpSession.TenantId &&
                              c.TenantDepartmentId == department.Id) ??
                         CreateCohort(context, department);

            context.SaveChanges();
            return (department, cohort);
        }

        private TenantDepartment CreateDepartment(inzibackendDbContext context)
        {
            var organizationUnit = context.OrganizationUnits.FirstOrDefault(o => o.TenantId == AbpSession.TenantId) ??
                                   context.OrganizationUnits.Add(new OrganizationUnit
                                   {
                                       TenantId = AbpSession.TenantId,
                                       DisplayName = "Registration QA",
                                       Code = Guid.NewGuid().ToString("N")
                                   }).Entity;

            var department = context.TenantDepartments.Add(new TenantDepartment
            {
                TenantId = AbpSession.TenantId,
                Name = "Registration QA Department",
                Description = "Created for registration tests",
                Active = true,
                MROType = EnumClientMROTypes.None,
                OrganizationUnitId = organizationUnit.Id
            }).Entity;

            context.SaveChanges();
            return department;
        }

        private static Cohort CreateCohort(inzibackendDbContext context, TenantDepartment department)
        {
            var cohort = context.Cohorts.Add(new Cohort
            {
                TenantId = department.TenantId,
                Name = "Registration QA Cohort",
                Description = "Created for registration tests",
                TenantDepartmentId = department.Id,
                DefaultCohort = false
            }).Entity;

            context.SaveChanges();
            return cohort;
        }
    }

    public class TestAuthNetGateway : IAuthNetGateway
    {
        public AuthNetTransactionResult NextCaptureResult { get; set; } = TestGatewayHelpers.Success("CAPTURE-DEFAULT");

        public List<(AuthNetSubmit payment, decimal amount, string tenancyName)> CaptureRequests { get; } = new();

        public List<(string transactionId, string tenancyName)> VoidRequests { get; } = new();

        public Task<AuthNetTransactionResult> CapturePreAuthAsync(
            AuthNetSubmit authNetSubmit,
            AuthNetCaptureResultDto captureReceipt,
            decimal amount,
            string tenantIdentifier)
        {
            CaptureRequests.Add((authNetSubmit, amount, tenantIdentifier));
            return Task.FromResult(NextCaptureResult);
        }

        public Task<AuthNetTransactionResult> VoidPreAuthAsync(string transactionId, string tenantIdentifier)
        {
            VoidRequests.Add((transactionId, tenantIdentifier));
            return Task.FromResult(TestGatewayHelpers.Success(transactionId));
        }

        public void Reset()
        {
            CaptureRequests.Clear();
            VoidRequests.Clear();
            NextCaptureResult = TestGatewayHelpers.Success("CAPTURE-DEFAULT");
        }
    }

    internal static class TestGatewayHelpers
    {
        public static AuthNetTransactionResult Success(string transactionId, string authCode = "AUTH")
        {
            var response = new createTransactionResponse
            {
                transactionResponse = new transactionResponse
                {
                    transId = transactionId,
                    authCode = authCode,
                    responseCode = "1"
                },
                messages = new messagesType
                {
                    resultCode = messageTypeEnum.Ok
                }
            };

            return AuthNetTransactionResult.Success(response);
        }

        public static AuthNetTransactionResult Failure(string code, string message)
        {
            return AuthNetTransactionResult.Failure(code, message);
        }
    }
}
