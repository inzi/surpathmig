using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Transactions;
using Abp.Authorization.Users;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using inzibackend.Authorization.Users;
using inzibackend.MultiTenancy;
using inzibackend.Surpath;
using inzibackend.Surpath.Compliance;
using inzibackend.Surpath.SurpathPay;
using inzibackend.Url;

namespace inzibackend.Surpath.Registration
{
    public class RegistrationPaymentCoordinator : inzibackendAppServiceBase, IRegistrationPaymentCoordinator, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly UserManager _userManager;
        private readonly SurpathManager _surpathManager;
        private readonly SurpathPayManager _surpathPayManager;
        private readonly HierarchicalPricingManager _hierarchicalPricingManager;
        private readonly IAuthNetGateway _authNetGateway;
        private readonly IAppUrlService _appUrlService;

        public RegistrationPaymentCoordinator(
            IUnitOfWorkManager unitOfWorkManager,
            UserRegistrationManager userRegistrationManager,
            UserManager userManager,
            SurpathManager surpathManager,
            SurpathPayManager surpathPayManager,
            HierarchicalPricingManager hierarchicalPricingManager,
            IAuthNetGateway authNetGateway,
            TenantManager tenantManager,
            IAppUrlService appUrlService)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _userRegistrationManager = userRegistrationManager;
            _userManager = userManager;
            _surpathManager = surpathManager;
            _surpathPayManager = surpathPayManager;
            _hierarchicalPricingManager = hierarchicalPricingManager;
            _authNetGateway = authNetGateway;
            TenantManager = tenantManager;
            _appUrlService = appUrlService;
        }

        public async Task<RegistrationPaymentResult> ExecuteAsync(RegistrationPaymentRequest request)
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new InvalidOperationException("Tenant context is required for registration.");
            }

            var tenantId = AbpSession.TenantId.Value;
            var payment = request.Payment ?? new AuthNetSubmit();
            var captureReceipt = request.CaptureResult;
            var userPids = request.UserPids ?? Array.Empty<UserPid>();

            User user;
            Tenant tenant;

            using (var creationUow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    user = await _userRegistrationManager.RegisterAsync(
                        request.Name,
                        request.Surname,
                        request.EmailAddress,
                        request.UserName,
                        request.Password,
                        false,
                        _appUrlService.CreateEmailActivationUrlFormat(tenantId),
                        request.Address,
                        request.SuiteApt,
                        request.City,
                        request.State,
                        request.Zip,
                        request.DateOfBirth,
                        request.MiddleName,
                        request.PhoneNumber
                    );

                    user.IsActive = false;
                    await CurrentUnitOfWork.SaveChangesAsync();

                    if (request.IsExternalLogin && request.ExternalLoginInfo != null)
                    {
                        if (string.Equals(
                                request.ExternalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                                request.EmailAddress,
                                StringComparison.OrdinalIgnoreCase))
                        {
                            user.IsEmailConfirmed = true;
                        }

                        user.Logins = new List<UserLogin>
                        {
                            new UserLogin
                            {
                                LoginProvider = request.ExternalLoginInfo.LoginProvider,
                                ProviderKey = request.ExternalLoginInfo.ProviderKey,
                                TenantId = user.TenantId
                            }
                        };
                    }

                    await CurrentUnitOfWork.SaveChangesAsync();

                    tenant = await TenantManager.GetByIdAsync(user.TenantId.Value);

                    await _surpathManager.AssignUserToTenantDepartment(user.Id, request.TenantDepartmentId, tenantId);
                    await _surpathManager.AssignUserToCohort(user.Id, request.CohortId, tenantId);

                    foreach (var userPid in userPids.Where(p => p != null))
                    {
                        await _surpathManager.CreateOrUpdateUserPid(user.Id, userPid, tenantId);
                    }

                    await CurrentUnitOfWork.SaveChangesAsync();
                    await creationUow.CompleteAsync();
                }
            }

            var hasPaymentToken = request.TenantIsDonorPay &&
                                  payment.amount > 0 &&
                                  captureReceipt != null &&
                                  !captureReceipt.TransactionId.IsNullOrWhiteSpace();

            AuthNetTransactionResult gatewayResult = null;
            decimal amountDue = 0;
            decimal capturedAmount = hasPaymentToken ? payment.amount : 0;

            if (request.SkipPaymentProcessing)
            {
                amountDue = request.AmountDueOverride ?? 0;
                capturedAmount = 0;

                using (var finalizeUow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var persistedUser = await _userManager.GetUserByIdAsync(user.Id);
                        persistedUser.IsPaid = amountDue <= 0;
                        persistedUser.IsActive = true;
                        await CurrentUnitOfWork.SaveChangesAsync();
                        await finalizeUow.CompleteAsync();
                        user = persistedUser;
                    }
                }

                return new RegistrationPaymentResult
                {
                    User = user,
                    Tenant = tenant,
                    PaymentCaptured = false,
                    CapturedAmount = capturedAmount,
                    AmountDue = amountDue,
                    GatewayResult = null
                };
            }

            if (request.TenantIsDonorPay)
            {
                PrepareBillingAddress(request, payment);

                if (hasPaymentToken)
                {
                    gatewayResult = await _authNetGateway.CapturePreAuthAsync(
                        payment,
                        captureReceipt,
                        payment.amount,
                        tenant.TenancyName);

                    if (gatewayResult == null || !gatewayResult.Succeeded)
                    {
                        await TryVoidPreAuthAsync(captureReceipt, tenant);
                        await EnsureUserInactiveAsync(user.Id, tenantId);
                        throw new UserFriendlyException("Payment Failed, please verify billing information");
                    }

                    using (var ledgerUow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                    {
                        using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                        {
                            var persistedUser = await _userManager.GetUserByIdAsync(user.Id);

                            var tenantServices = await _surpathPayManager.GetSurpathServicesForTenant(tenantId);
                            var paidServices = tenantServices
                                .Where(t => payment.TenantSurpathServiceIds.Contains(t.Id))
                                .ToList();

                            decimal totalPrice = 0;
                            foreach (var service in paidServices)
                            {
                                if (!service.SurpathServiceId.HasValue)
                                {
                                    continue;
                                }

                                var pricingDecision = await _hierarchicalPricingManager.GetServicePricingDecisionAsync(
                                    service.SurpathServiceId.Value,
                                    tenantId,
                                    request.TenantDepartmentId,
                                    request.CohortId,
                                    null);

                                totalPrice += (decimal)pricingDecision.PriceToCharge;
                            }

                            amountDue = totalPrice - payment.amount >= 0 ? totalPrice - payment.amount : 0;

                            await _surpathPayManager.CreateLedgerEntry(
                                gatewayResult.Response.transactionResponse,
                                persistedUser.Id,
                                persistedUser.TenantId,
                                payment.amount,
                                payment,
                                totalPrice);

                            persistedUser.IsPaid = amountDue == 0;
                            persistedUser.IsActive = true;

                            await CurrentUnitOfWork.SaveChangesAsync();
                            await ledgerUow.CompleteAsync();

                            user = persistedUser;
                        }
                    }
                }
                else
                {
                    amountDue = 0;
                    capturedAmount = 0;

                    using (var finalizeUow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                    {
                        using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                        {
                            var persistedUser = await _userManager.GetUserByIdAsync(user.Id);
                            persistedUser.IsPaid = true;
                            persistedUser.IsActive = true;
                            await CurrentUnitOfWork.SaveChangesAsync();
                            await finalizeUow.CompleteAsync();
                            user = persistedUser;
                        }
                    }
                }
            }
            else
            {
                using (var finalizeUow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var persistedUser = await _userManager.GetUserByIdAsync(user.Id);
                        persistedUser.IsActive = true;
                        await CurrentUnitOfWork.SaveChangesAsync();
                        await finalizeUow.CompleteAsync();
                        user = persistedUser;
                    }
                }
            }

            return new RegistrationPaymentResult
            {
                User = user,
                Tenant = tenant,
                PaymentCaptured = gatewayResult?.Succeeded ?? false,
                CapturedAmount = capturedAmount,
                AmountDue = amountDue,
                GatewayResult = gatewayResult
            };
        }

        private static void PrepareBillingAddress(RegistrationPaymentRequest request, AuthNetSubmit payment)
        {
            if (payment == null)
            {
                return;
            }

            if (payment.DifferentBillingAddress)
            {
                return;
            }

            payment.BillingAddress = request.Address;
            payment.BillingCity = request.City;
            payment.BillingState = request.State;
            payment.FirstNameOnCard = request.Name;
            payment.LastNameOnCard = request.Surname;
            payment.CardNameOnCard = $"{request.Name} {request.Surname}".Trim();
        }

        private async Task TryVoidPreAuthAsync(AuthNetCaptureResultDto captureReceipt, Tenant tenant)
        {
            if (captureReceipt == null || captureReceipt.TransactionId.IsNullOrWhiteSpace())
            {
                return;
            }

            try
            {
                var voidResult = await _authNetGateway.VoidPreAuthAsync(captureReceipt.TransactionId, tenant?.TenancyName);
                if (voidResult != null && !voidResult.Succeeded)
                {
                    Logger.Warn($"Void pre-authorization failed. TransactionId={captureReceipt.TransactionId}, Code={voidResult.ErrorCode}, Message={voidResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Logger.Warn($"Exception encountered while voiding pre-authorization {captureReceipt.TransactionId}", ex);
            }
        }

        private async Task EnsureUserInactiveAsync(long userId, int tenantId)
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var persistedUser = await _userManager.GetUserByIdAsync(userId);
                    persistedUser.IsActive = false;
                    persistedUser.IsPaid = false;
                    await CurrentUnitOfWork.SaveChangesAsync();
                    await uow.CompleteAsync();
                }
            }
        }
    }
}
