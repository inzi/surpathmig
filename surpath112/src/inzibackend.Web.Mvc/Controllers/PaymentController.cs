using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Editions;
using inzibackend.MultiTenancy;
using inzibackend.MultiTenancy.Dto;
using inzibackend.MultiTenancy.Payments;
using inzibackend.MultiTenancy.Payments.Dto;
using inzibackend.Url;
using inzibackend.Web.Models.Payment;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Transactions;
using inzibackend.Authorization;
using inzibackend.Authorization.Roles;
using inzibackend.Authorization.Users;
using inzibackend.Identity;
using inzibackend.Surpath.SurpathPay;
using inzibackend.Surpath;
using Microsoft.Extensions.Configuration;
using inzibackend.Surpath.Dtos;
using inzibackend.Web.Areas.App.Models.Purchase;
using System.Linq;
using HealthChecks.UI.Configuration;
using Abp.Domain.Uow;
using Abp.UI;
using inzibackend.Surpath.Compliance;
using inzibackend.Web.Models.Account;
using System.Data;
using AuthorizeNet.Api.Contracts.V1;
using inzibackend.Surpath.ComplianceManager;
using inzibackend.Migrations;
using Abp.Domain.Repositories;
using System.Data.Entity;

namespace inzibackend.Web.Controllers
{
    public class PaymentController : inzibackendControllerBase
    {
        private readonly IPaymentAppService _paymentAppService;
        private readonly ITenantRegistrationAppService _tenantRegistrationAppService;
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;
        private readonly IWebUrlService _webUrlService;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly UserClaimsPrincipalFactory<User, Role> _userClaimsPrincipalFactory;
        private readonly UserManager _userManager;
        private readonly SignInManager _signInManager;
        private AuthNetManager _authNetManager;
        private SurpathPayManager _surpathPayManager;
        readonly IConfigurationRoot _appConfiguration;
        private readonly SurpathManager _surpathManager;
        private readonly ISurpathComplianceEvaluator _surpathComplianceEvaluator;
        private readonly IRepository<TenantDepartmentUser, Guid> _tenantDepartmentUserRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly HierarchicalPricingManager _hierarchicalPricingManager;

        public PaymentController(
            IPaymentAppService paymentAppService,
            ITenantRegistrationAppService tenantRegistrationAppService,
            TenantManager tenantManager,
            EditionManager editionManager,
            IWebUrlService webUrlService,
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            UserClaimsPrincipalFactory<User, Role> userClaimsPrincipalFactory,
            UserManager userManager,
            SignInManager signInManager,
            AuthNetManager authNetManager,
            SurpathPayManager surpathPayManager,
             SurpathManager surpathManager,
            IConfigurationRoot appConfiguration,
            ISurpathComplianceEvaluator surpathComplianceEvaluator,
            IRepository<TenantDepartmentUser, Guid> tenantDepartmentUserRepository,
            IRepository<CohortUser, Guid> cohortUserRepository,
            HierarchicalPricingManager hierarchicalPricingManager
            )
        {
            _paymentAppService = paymentAppService;
            _tenantRegistrationAppService = tenantRegistrationAppService;
            _tenantManager = tenantManager;
            _editionManager = editionManager;
            _webUrlService = webUrlService;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _userManager = userManager;
            _signInManager = signInManager;
            _appConfiguration = appConfiguration;
            _surpathPayManager = surpathPayManager;
            _surpathManager = surpathManager;
            _surpathComplianceEvaluator = surpathComplianceEvaluator;
            _tenantDepartmentUserRepository = tenantDepartmentUserRepository;
            _cohortUserRepository = cohortUserRepository;
            _hierarchicalPricingManager = hierarchicalPricingManager;
        }

        public async Task<IActionResult> Buy(int tenantId, int editionId, int? subscriptionStartType, int? editionPaymentType)
        {
            SetTenantIdCookie(tenantId);

            var edition = await _tenantRegistrationAppService.GetEdition(editionId);

            var model = new BuyEditionViewModel
            {
                Edition = edition,
                PaymentGateways = _paymentAppService.GetActiveGateways(new GetActiveGatewaysInput())
            };

            if (editionPaymentType.HasValue)
            {
                model.EditionPaymentType = (EditionPaymentType)editionPaymentType.Value;
            }

            if (subscriptionStartType.HasValue)
            {
                model.SubscriptionStartType = (SubscriptionStartType)subscriptionStartType.Value;
            }

            return View("Buy", model);
        }

        public async Task<IActionResult> Upgrade(int upgradeEditionId)
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new ArgumentNullException();
            }

            SubscriptionPaymentType subscriptionPaymentType;

            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());
                subscriptionPaymentType = tenant.SubscriptionPaymentType;

                if (tenant.EditionId.HasValue)
                {
                    var currentEdition = await _editionManager.GetByIdAsync(tenant.EditionId.Value);
                    if (((SubscribableEdition)currentEdition).IsFree)
                    {
                        var upgradeEdition = await _editionManager.GetByIdAsync(upgradeEditionId);
                        if (((SubscribableEdition)upgradeEdition).IsFree)
                        {
                            await _paymentAppService.SwitchBetweenFreeEditions(upgradeEditionId);
                            return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
                        }

                        return RedirectToAction("Buy", "Payment", new
                        {
                            tenantId = AbpSession.GetTenantId(),
                            editionId = upgradeEditionId,
                            editionPaymentType = (int)EditionPaymentType.BuyNow
                        });
                    }

                    if (!await _paymentAppService.HasAnyPayment())
                    {
                        return RedirectToAction("Buy", "Payment", new
                        {
                            tenantId = AbpSession.GetTenantId(),
                            editionId = upgradeEditionId,
                            editionPaymentType = (int)EditionPaymentType.BuyNow
                        });
                    }
                }
            }

            var paymentInfo = await _paymentAppService.GetPaymentInfo(new PaymentInfoInput { UpgradeEditionId = upgradeEditionId });

            if (paymentInfo.IsLessThanMinimumUpgradePaymentAmount())
            {
                await _paymentAppService.UpgradeSubscriptionCostsLessThenMinAmount(upgradeEditionId);
                return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
            }
            var edition = await _tenantRegistrationAppService.GetEdition(upgradeEditionId);

            var lastPayment = await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(
                tenantId: AbpSession.GetTenantId(),
                gateway: null,
                isRecurring: null);

            var model = new UpgradeEditionViewModel
            {
                Edition = edition,
                AdditionalPrice = paymentInfo.AdditionalPrice,
                SubscriptionPaymentType = subscriptionPaymentType,
                PaymentPeriodType = lastPayment.GetPaymentPeriodType()
            };

            if (subscriptionPaymentType.IsRecurring())
            {
                model.PaymentGateways = new List<PaymentGatewayModel>
                {
                    new PaymentGatewayModel
                    {
                        GatewayType = lastPayment.Gateway,
                        SupportsRecurringPayments = true
                    }
                };
            }
            else
            {
                model.PaymentGateways = _paymentAppService.GetActiveGateways(new GetActiveGatewaysInput());
            }

            return View("Upgrade", model);
        }

        public async Task<IActionResult> Extend(int upgradeEditionId, EditionPaymentType editionPaymentType)
        {
            var edition = await _tenantRegistrationAppService.GetEdition(upgradeEditionId);

            var model = new ExtendEditionViewModel
            {
                Edition = edition,
                PaymentGateways = _paymentAppService.GetActiveGateways(new GetActiveGatewaysInput())
            };

            return View("Extend", model);
        }

        [HttpPost]
        public async Task<JsonResult> CreatePayment(CreatePaymentModel model)
        {
            var paymentId = await _paymentAppService.CreatePayment(new CreatePaymentDto
            {
                PaymentPeriodType = model.PaymentPeriodType,
                EditionId = model.EditionId,
                EditionPaymentType = model.EditionPaymentType,
                RecurringPaymentEnabled = model.RecurringPaymentEnabled.HasValue && model.RecurringPaymentEnabled.Value,
                SubscriptionPaymentGatewayType = model.Gateway,
                SuccessUrl = _webUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "Payment/" + model.EditionPaymentType + "Succeed",
                ErrorUrl = _webUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "Payment/PaymentFailed"
            });

            return Json(new AjaxResponse
            {
                TargetUrl = Url.Action("Purchase", model.Gateway.ToString(), new
                {
                    paymentId = paymentId,
                    isUpgrade = model.EditionPaymentType == EditionPaymentType.Upgrade
                })
            });
        }

        [HttpPost]
        public async Task CancelPayment(CancelPaymentModel model)
        {
            await _paymentAppService.CancelPayment(new CancelPaymentDto
            {
                Gateway = model.Gateway,
                PaymentId = model.PaymentId
            });
        }

        public async Task<IActionResult> BuyNowSucceed(long paymentId)
        {
            await _paymentAppService.BuyNowSucceed(paymentId);

            return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
        }

        public async Task<IActionResult> NewRegistrationSucceed(long paymentId)
        {
            await _paymentAppService.NewRegistrationSucceed(paymentId);

            await LoginAdminAsync();

            return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
        }

        public async Task<IActionResult> UpgradeSucceed(long paymentId)
        {
            await _paymentAppService.UpgradeSucceed(paymentId);

            return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
        }

        public async Task<IActionResult> ExtendSucceed(long paymentId)
        {
            await _paymentAppService.ExtendSucceed(paymentId);

            return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
        }

        public async Task<IActionResult> PaymentFailed(long paymentId)
        {
            await _paymentAppService.PaymentFailed(paymentId);

            if (AbpSession.UserId.HasValue)
            {
                return RedirectToAction("Index", "SubscriptionManagement", new { area = "App" });
            }

            return RedirectToAction("Index", "Home", new { area = "App" });
        }

        private async Task LoginAdminAsync()
        {
            var user = await _userManager.GetAdminAsync();
            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(principal.Identity as ClaimsIdentity, false);
        }

        public IActionResult PaymentCompleted()
        {
            return View();
        }

        //public PartialViewResult UserBuy()
        //{
        //    return PartialView("_UserBuyModal");
        //}

        public IActionResult authnet()
        {
            return View();
        }

        public async Task<PartialViewResult> PurchaseModal(Guid? deptId, Guid? cohortId)
        {
            var viewmodel = new BuyServicesViewModel();
            var UseSandbox = _appConfiguration["Payment:AuthorizeNet:UseSandbox"].To<bool>();

            // Check if user is marked as invoiced (bypasses payment entirely)
            if (AbpSession.UserId.HasValue && AbpSession.TenantId.HasValue)
            {
                var isInvoiced = await _hierarchicalPricingManager.GetIsInvoicedForUserAsync(
                    AbpSession.TenantId.Value,
                    deptId,
                    cohortId,
                    AbpSession.UserId.Value
                );
                
                if (isInvoiced)
                {
                    Logger.Debug($"User {AbpSession.UserId} is marked as invoiced - skipping payment modal");
                    viewmodel.TenantSurpathServices = new List<TenantSurpathServiceDto>();
                    viewmodel.AmountDue = 0;
                    return PartialView("_PurchaseModal", viewmodel);
                }
            }

            viewmodel.UseSandboxPayment = UseSandbox;

            // Get all tenant services without filtering by compliance requirements
            // The hierarchical pricing will determine which services should be included
            var _tenantservices = await _surpathPayManager.GetSurpathServicesForTenant((int)AbpSession.TenantId);
            
            var tenantSurpathServices = ObjectMapper.Map<List<TenantSurpathServiceDto>>(_tenantservices);
            // Don't filter by IsEnabled here - let hierarchical pricing determine if service should be included
            // List any with no assignments to department, cohort, or user.
            var unassigned = tenantSurpathServices.Where(s => s.TenantDepartmentId == null && s.CohortId == null && s.UserId == null).ToList();
            // only keep those that apply to the current user.

            // if we have a user, get the pricing for them
            var deptSurpathServices = new List<TenantSurpathServiceDto>();
            var cohortSurpathServices = new List<TenantSurpathServiceDto>();
            var userSurpathServices = new List<TenantSurpathServiceDto>();


            if (AbpSession.UserId != null)
            {
                // Get the current user
                var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);

                // Get the current user's departments
                //var _userDepartmentsId = _tenantDepartmentUserRepository.GetAll().Where(tdu => tdu.UserId == user.Id).ToList();

                var userDepartmentsId = _tenantDepartmentUserRepository.GetAll().AsNoTracking().Where(tdu => tdu.UserId == user.Id).Select(tdu => tdu.TenantDepartmentId).ToList();
                deptSurpathServices = tenantSurpathServices.Where(s => s.TenantDepartmentId != null && userDepartmentsId.Contains(s.TenantDepartmentId.Value)).ToList();
                // Get the current user's cohorts
                var userCohortsId = _cohortUserRepository.GetAll().AsNoTracking().Where(cu => cu.UserId == user.Id).Select(cu => cu.CohortId).ToList();
                cohortSurpathServices = tenantSurpathServices.Where(s => s.CohortId != null && userCohortsId.Contains(s.CohortId.Value)).ToList();

                // lastly, get the services that are assigned to the user.
                userSurpathServices = tenantSurpathServices.Where(s => s.UserId != null && s.UserId == user.Id).ToList();

                // combine unassigned (whole tenant) with department, cohort, and user services.
            }
            else
            {
                // use the deptId and cohortId to get pricing, this is likely a registration
                deptSurpathServices = tenantSurpathServices.Where(s => s.TenantDepartmentId != null && s.TenantDepartmentId.Value == deptId).ToList();
                cohortSurpathServices = tenantSurpathServices.Where(s => s.CohortId != null && s.CohortId.Value == cohortId).ToList();

            }
            var allSurpathServices = unassigned.Concat(deptSurpathServices).Concat(cohortSurpathServices).Concat(userSurpathServices).ToList();

            // The last step is to only keep one allSurpathServices by it's service id
            // assigned to user overrides assigned to cohort.
            // assigned to cohort overrides assigned to tenantdepartment.
            // assigned to tenant department overrides assigned to Tenant.

            // Group by SurpathServiceId and select the highest priority assignment for each service
            var prioritizedServices = allSurpathServices
                .GroupBy(s => s.SurpathServiceId)
                .Select(group => group
                    .OrderByDescending(s => 
                        // Priority order: User > Cohort > Department > Tenant (unassigned)
                        s.UserId != null ? 4 :
                        s.CohortId != null ? 3 :
                        s.TenantDepartmentId != null ? 2 :
                        1
                    )
                    .First()
                )
                .ToList();

            viewmodel.TenantSurpathServices = prioritizedServices;

            // Get hierarchical pricing using the V2 purchase-specific method
            // This ensures proper tenant filter handling for both registration and renewal scenarios
            var hierarchicalPricing = await _hierarchicalPricingManager.GetPricingForPurchaseV2Async(
                (int)AbpSession.TenantId,
                deptId,
                cohortId,
                AbpSession.UserId // Will be null during registration, populated during renewal
            );

            // Update prices with effective prices from hierarchical pricing
            Logger.Debug($"Updating prices using hierarchical pricing V2 for {viewmodel.TenantSurpathServices.Count} services");
            
            foreach (var service in viewmodel.TenantSurpathServices)
            {
                var originalPrice = service.Price;
                double resolvedPrice = originalPrice;
                double amountDue = originalPrice;

                if (service.SurpathServiceId.HasValue)
                {
                    var pricingDecision = await _hierarchicalPricingManager.GetServicePricingDecisionAsync(
                        service.SurpathServiceId.Value,
                        (int)AbpSession.TenantId,
                        deptId,
                        cohortId,
                        AbpSession.UserId
                    );

                    resolvedPrice = pricingDecision.EffectivePrice;
                    amountDue = pricingDecision.PriceToCharge;
                    service.IsInvoiced = pricingDecision.IsInvoiced;
                }
                else
                {
                    service.IsInvoiced = false;
                }

                service.Price = resolvedPrice;
                service.AmountDue = amountDue;
                
                Logger.Debug($"Service: {service.Name}, Original Price: {originalPrice}, Resolved Price: {resolvedPrice}, Amount Due: {amountDue}, " +
                    $"ServiceId: {service.SurpathServiceId}, DeptId: {service.TenantDepartmentId}, CohortId: {service.CohortId}, IsInvoiced: {service.IsInvoiced}");
            }

            // Keep services that either charge the user or are invoiced (so overrides remain visible)
            viewmodel.TenantSurpathServices = viewmodel.TenantSurpathServices
                .Where(s => s.AmountDue > 0 || s.IsInvoiced)
                .ToList();
            
            viewmodel.AmountDue = (decimal)viewmodel.TenantSurpathServices.Sum(s => s.AmountDue);
            
            Logger.Debug($"Total amount due after hierarchical pricing: {viewmodel.AmountDue}");
            
            return PartialView("_PurchaseModal", viewmodel);
        }


    }
}
