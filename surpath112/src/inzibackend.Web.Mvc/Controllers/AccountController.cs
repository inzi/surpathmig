using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Caching;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Abp.Web.Models;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using inzibackend.Authentication.TwoFactor.Google;
using inzibackend.Authorization;
using inzibackend.Authorization.Accounts;
using inzibackend.Authorization.Accounts.Dto;
using inzibackend.Authorization.Delegation;
using inzibackend.Authorization.Impersonation;
using inzibackend.Authorization.Users;
using inzibackend.Configuration;
using inzibackend.Debugging;
using inzibackend.Identity;
using inzibackend.MultiTenancy;
using inzibackend.Net.Sms;
using inzibackend.Notifications;
using inzibackend.Web.Models.Account;
using inzibackend.Security;
using inzibackend.Security.Recaptcha;
using inzibackend.Sessions;
using inzibackend.Url;
using inzibackend.Web.Authentication.External;
using inzibackend.Web.Security.Recaptcha;
using inzibackend.Web.Session;
using inzibackend.Web.Views.Shared.Components.TenantChange;
using Abp.CachedUniqueKeys;
using Abp.AspNetCore.Mvc.Caching;
using inzibackend.Surpath;
using inzibackend.Web.Areas.App.Models.UserPids;
using inzibackend.Surpath.Compliance;
using inzibackend.Surpath.Dtos;
using inzibackend.Surpath.SurpathPay;
using inzibackend.Surpath.Registration;
using inzibackend.Web.Areas.App.Models.Purchase;
using Microsoft.Extensions.Configuration;
using Abp.Application.Features;
using inzibackend.Surpath.ComplianceManager;
using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;

namespace inzibackend.Web.Controllers
{
    public class AccountController : inzibackendControllerBase
    {
        private readonly UserManager _userManager;
        private readonly TenantManager _tenantManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IWebUrlService _webUrlService;
        private readonly IAppUrlService _appUrlService;
        private readonly IAppNotifier _appNotifier;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly IUserLinkManager _userLinkManager;
        private readonly LogInManager _logInManager;
        private readonly SignInManager _signInManager;
        private readonly IRecaptchaValidator _recaptchaValidator;
        private readonly IPerRequestSessionCache _sessionCache;
        private readonly ITenantCache _tenantCache;
        private readonly IAccountAppService _accountAppService;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IImpersonationManager _impersonationManager;
        private readonly ISmsSender _smsSender;
        private readonly IEmailSender _emailSender;
        private readonly IPasswordComplexitySettingStore _passwordComplexitySettingStore;
        private readonly IdentityOptions _identityOptions;
        private readonly ISessionAppService _sessionAppService;
        private readonly ExternalLoginInfoManagerFactory _externalLoginInfoManagerFactory;
        private readonly ISettingManager _settingManager;
        private readonly IUserDelegationManager _userDelegationManager;
        private readonly ICachedUniqueKeyPerUser _cachedUniqueKeyPerUser;
        private readonly IGetScriptsResponsePerUserConfiguration _getScriptsResponsePerUserConfiguration;
        private readonly ISurpathComplianceAppService _surpathComplianceAppService;
        private readonly SurpathManager _surpathManager;
        private readonly AuthNetManager _authNetManager;
        private readonly SurpathPayManager _surpathPayManager;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly ISurpathComplianceEvaluator _surpathComplianceEvaluator;
        private readonly HierarchicalPricingManager _hierarchicalPricingManager;
        private readonly IRegistrationValidationManager _registrationValidationManager;
        private readonly IRegistrationPaymentCoordinator _registrationPaymentCoordinator;

        public AccountController(
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            TenantManager tenantManager,
            IUnitOfWorkManager unitOfWorkManager,
            IAppNotifier appNotifier,
            IWebUrlService webUrlService,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            IUserLinkManager userLinkManager,
            LogInManager logInManager,
            SignInManager signInManager,
            IRecaptchaValidator recaptchaValidator,
            ITenantCache tenantCache,
            IAccountAppService accountAppService,
            UserRegistrationManager userRegistrationManager,
            IImpersonationManager impersonationManager,
            IAppUrlService appUrlService,
            IPerRequestSessionCache sessionCache,
            IEmailSender emailSender,
            ISmsSender smsSender,
            IPasswordComplexitySettingStore passwordComplexitySettingStore,
            IOptions<IdentityOptions> identityOptions,
            ISessionAppService sessionAppService,
            ExternalLoginInfoManagerFactory externalLoginInfoManagerFactory,
            ISettingManager settingManager,
            IUserDelegationManager userDelegationManager,
            ICachedUniqueKeyPerUser cachedUniqueKeyPerUser,
            IGetScriptsResponsePerUserConfiguration getScriptsResponsePerUserConfiguration,
            ISurpathComplianceAppService surpathComplianceAppService,
            SurpathManager surpathManager,
            AuthNetManager authNetManager,
            SurpathPayManager surpathPayManager,
            IAppConfigurationAccessor appConfigurationAccessor,
            ISurpathComplianceEvaluator surpathComplianceEvaluator,
            HierarchicalPricingManager hierarchicalPricingManager,
            IRegistrationValidationManager registrationValidationManager,
            IRegistrationPaymentCoordinator registrationPaymentCoordinator

            )
        {
            _userManager = userManager;
            _multiTenancyConfig = multiTenancyConfig;
            _tenantManager = tenantManager;
            _unitOfWorkManager = unitOfWorkManager;
            _webUrlService = webUrlService;
            _appNotifier = appNotifier;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _userLinkManager = userLinkManager;
            _logInManager = logInManager;
            _signInManager = signInManager;
            _recaptchaValidator = recaptchaValidator;
            _tenantCache = tenantCache;
            _accountAppService = accountAppService;
            _userRegistrationManager = userRegistrationManager;
            _impersonationManager = impersonationManager;
            _appUrlService = appUrlService;
            _sessionCache = sessionCache;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _passwordComplexitySettingStore = passwordComplexitySettingStore;
            _identityOptions = identityOptions.Value;
            _sessionAppService = sessionAppService;
            _externalLoginInfoManagerFactory = externalLoginInfoManagerFactory;
            _settingManager = settingManager;
            _userDelegationManager = userDelegationManager;
            _cachedUniqueKeyPerUser = cachedUniqueKeyPerUser;
            _getScriptsResponsePerUserConfiguration = getScriptsResponsePerUserConfiguration;
            _surpathComplianceAppService = surpathComplianceAppService;

            _surpathManager = surpathManager;
            _authNetManager = authNetManager;
            _surpathPayManager = surpathPayManager;
            _appConfiguration = appConfigurationAccessor.Configuration;
            _surpathComplianceEvaluator = surpathComplianceEvaluator;
            _hierarchicalPricingManager = hierarchicalPricingManager;
            _registrationValidationManager = registrationValidationManager;
            _registrationPaymentCoordinator = registrationPaymentCoordinator;
        }

        #region Login / Logout

        public async Task<ActionResult> Login(string userNameOrEmailAddress = "", string returnUrl = "",
                string successMessage = "", string ss = "")
        {
            returnUrl = NormalizeReturnUrl(returnUrl);

            if (!string.IsNullOrEmpty(ss) && ss.Equals("true", StringComparison.OrdinalIgnoreCase) &&
                AbpSession.UserId > 0)
            {
                var updateUserSignInTokenOutput = await _sessionAppService.UpdateUserSignInToken();
                returnUrl = AddSingleSignInParametersToReturnUrl(
                    returnUrl, updateUserSignInTokenOutput.SignInToken,
                    AbpSession.UserId.Value,
                    AbpSession.TenantId
                );

                return Redirect(returnUrl);
            }

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
            ViewBag.SingleSignIn = ss;
            ViewBag.UseCaptcha = UseCaptchaOnLogin();

            if (AbpSession.UserId.HasValue)
            {
                await _signInManager.SignOutAsync();
                return Redirect(
                    $"/Account/Login?userNameOrEmailAddress={userNameOrEmailAddress}&returnUrl={returnUrl}&successMessage={successMessage}&ss={ss}"
                );
            }

            return View(
                new LoginFormViewModel
                {
                    IsSelfRegistrationEnabled = IsSelfRegistrationEnabled(),
                    IsTenantSelfRegistrationEnabled = IsTenantSelfRegistrationEnabled(),
                    SuccessMessage = successMessage,
                    UserNameOrEmailAddress = userNameOrEmailAddress
                }
            );
        }

        [HttpPost]
        public virtual async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "",
            string returnUrlHash = "", string ss = "")
        {
            returnUrl = NormalizeReturnUrl(returnUrl);
            if (!string.IsNullOrWhiteSpace(returnUrlHash))
            {
                returnUrl = returnUrl + returnUrlHash;
            }

            if (UseCaptchaOnLogin())
            {
                await _recaptchaValidator.ValidateAsync(
                    HttpContext.Request.Form[RecaptchaValidator.RecaptchaResponseKey]);
            }

            var loginResult = await GetLoginResultAsync(loginModel.UsernameOrEmailAddress, loginModel.Password,
                GetTenancyNameOrNull());

            if (!string.IsNullOrEmpty(ss) && ss.Equals("true", StringComparison.OrdinalIgnoreCase) &&
                loginResult.Result == AbpLoginResultType.Success)
            {
                loginResult.User.SetSignInToken();
                returnUrl = AddSingleSignInParametersToReturnUrl(returnUrl, loginResult.User.SignInToken,
                    loginResult.User.Id, loginResult.User.TenantId);
            }

            if (_settingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowOneConcurrentLoginPerUser))
            {
                await _userManager.UpdateSecurityStampAsync(loginResult.User);
            }

            if (loginResult.User.ShouldChangePasswordOnNextLogin)
            {
                loginResult.User.SetNewPasswordResetCode();

                return Json(new AjaxResponse
                {
                    TargetUrl = Url.Action(
                        "ResetPassword",
                        new ResetPasswordViewModel
                        {
                            TenantId = AbpSession.TenantId,
                            UserId = loginResult.User.Id,
                            ResetCode = loginResult.User.PasswordResetCode,
                            ReturnUrl = returnUrl,
                            SingleSignIn = ss
                        })
                });
            }

            var signInResult = await _signInManager.SignInOrTwoFactorAsync(loginResult, loginModel.RememberMe);
            if (signInResult.RequiresTwoFactor)
            {
                return Json(new AjaxResponse
                {
                    TargetUrl = Url.Action(
                        "SendSecurityCode",
                        new
                        {
                            returnUrl = returnUrl,
                            rememberMe = loginModel.RememberMe
                        })
                });
            }

            Debug.Assert(signInResult.Succeeded);

            await UnitOfWorkManager.Current.SaveChangesAsync();

            return Json(new AjaxResponse { TargetUrl = returnUrl });
        }

        public async Task<ActionResult> Logout(string returnUrl = "")
        {
            await _signInManager.SignOutAsync();
            var userIdentifier = AbpSession.ToUserIdentifier();

            if (userIdentifier != null &&
                _settingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowOneConcurrentLoginPerUser))
            {
                var user = await _userManager.GetUserAsync(userIdentifier);
                await _userManager.UpdateSecurityStampAsync(user);
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = NormalizeReturnUrl(returnUrl);
                return Redirect(returnUrl);
            }

            return RedirectToAction("Login");
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress,
            string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;

                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result,
                        usernameOrEmailAddress, tenancyName);
            }
        }

        private string AddSingleSignInParametersToReturnUrl(string returnUrl, string signInToken, long userId,
            int? tenantId)
        {
            returnUrl += (returnUrl.Contains("?") ? "&" : "?") +
                         "accessToken=" + signInToken +
                         "&userId=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(userId.ToString()));
            if (tenantId.HasValue)
            {
                returnUrl += "&tenantId=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(tenantId.Value.ToString()));
            }

            return returnUrl;
        }

        public ActionResult SessionLockScreen()
        {
            ViewBag.UseCaptcha = UseCaptchaOnLogin();
            return View();
        }

        #endregion Login / Logout

        #region Two Factor Auth

        public async Task<ActionResult> SendSecurityCode(string returnUrl, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            CheckCurrentTenant(await _signInManager.GetVerifiedTenantIdAsync());

            var userProviders = await _userManager.GetValidTwoFactorProvidersAsync(user);

            var factorOptions = userProviders.Select(
                userProvider =>
                    new SelectListItem
                    {
                        Text = userProvider,
                        Value = userProvider
                    }).ToList();

            return View(
                new SendSecurityCodeViewModel
                {
                    Providers = factorOptions,
                    ReturnUrl = returnUrl,
                    RememberMe = rememberMe
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> SendSecurityCode(SendSecurityCodeViewModel model)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            CheckCurrentTenant(await _signInManager.GetVerifiedTenantIdAsync());

            if (model.SelectedProvider != GoogleAuthenticatorProvider.Name)
            {
                var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
                var message = L("EmailSecurityCodeBody", code);

                if (model.SelectedProvider == "Email")
                {
                    await _emailSender.SendAsync(await _userManager.GetEmailAsync(user), L("EmailSecurityCodeSubject"),
                        message);
                }
                else if (model.SelectedProvider == "Phone")
                {
                    await _smsSender.SendAsync(await _userManager.GetPhoneNumberAsync(user), message);
                }
            }

            return RedirectToAction(
                "VerifySecurityCode",
                new
                {
                    provider = model.SelectedProvider,
                    returnUrl = model.ReturnUrl,
                    rememberMe = model.RememberMe
                }
            );
        }

        public async Task<ActionResult> VerifySecurityCode(string provider, string returnUrl, bool rememberMe)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new UserFriendlyException(L("VerifySecurityCodeNotLoggedInErrorMessage"));
            }

            CheckCurrentTenant(await _signInManager.GetVerifiedTenantIdAsync());

            var isRememberBrowserEnabled = await IsRememberBrowserEnabledAsync();

            return View(
                new VerifySecurityCodeViewModel
                {
                    Provider = provider,
                    ReturnUrl = returnUrl,
                    RememberMe = rememberMe,
                    IsRememberBrowserEnabled = isRememberBrowserEnabled
                }
            );
        }

        [HttpPost]
        public async Task<JsonResult> VerifySecurityCode(VerifySecurityCodeViewModel model)
        {
            model.ReturnUrl = NormalizeReturnUrl(model.ReturnUrl);

            CheckCurrentTenant(await _signInManager.GetVerifiedTenantIdAsync());

            var result = await _signInManager.TwoFactorSignInAsync(
                model.Provider,
                model.Code,
                model.RememberMe,
                await IsRememberBrowserEnabledAsync() && model.RememberBrowser
            );

            if (result.Succeeded)
            {
                return Json(new AjaxResponse { TargetUrl = model.ReturnUrl });
            }

            if (result.IsLockedOut)
            {
                throw new UserFriendlyException(L("UserLockedOutMessage"));
            }

            throw new UserFriendlyException(L("InvalidSecurityCode"));
        }

        private Task<bool> IsRememberBrowserEnabledAsync()
        {
            return SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin
                .IsRememberBrowserEnabled);
        }

        #endregion Two Factor Auth

        #region Register

        public async Task<ActionResult> Register(string returnUrl = "", string ss = "")
        {
            //var _emptyPids = await _surpathComplianceAppService.GetEmptyUserPidList();

            //var _activePidTypes = await _surpathComplianceAppService.GetEmptyPidTypeList();

            //var _pids = new List<UserPidViewModel>();
            Logger.Info("Register Called");

            var tenantIsDonorPay = await _surpathManager.IsTenantDonorPay((int)AbpSession.TenantId);

            var viewmodel = new BuyServicesViewModel();
            var UseSandbox = _appConfiguration["Payment:AuthorizeNet:UseSandbox"].To<bool>();
            viewmodel.UseSandboxPayment = UseSandbox;

            // Get all tenant services without filtering by compliance requirements
            // Note: This is the registration GET method, so we don't have department/cohort context yet
            // The actual pricing will be calculated when the user selects their department/cohort
            var _tenantservices = await _surpathPayManager.GetSurpathServicesForTenant((int)AbpSession.TenantId);

            viewmodel.TenantSurpathServices = ObjectMapper.Map<List<TenantSurpathServiceDto>>(_tenantservices);

            // For display purposes, show base prices. The actual hierarchical pricing will be
            // calculated on the frontend when department/cohort are selected
            viewmodel.AmountDue = 0;

            var regVm = new RegisterViewModel
            {
                PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync(),
                ReturnUrl = returnUrl,
                SingleSignIn = ss,
                UserPidViewModels = new List<UserPidViewModel>(),
                IsDonorPay = tenantIsDonorPay,
                UseSandboxPayment = UseSandbox,
                TenantSurpathServices = viewmodel.TenantSurpathServices
            };

            regVm.AuthNetSubmit.TenantSurpathServiceIds = viewmodel.TenantSurpathServices.Select(s => s.Id).ToList();

            var model = RegisterView(regVm);

            return model;
        }

        [HttpPost]
        [AbpAllowAnonymous]
        [UnitOfWork]
        public async Task<JsonResult> GetRegistrationPricing([FromBody] RegistrationPricingInput input)
        {
            input ??= new RegistrationPricingInput();

            if (!AbpSession.TenantId.HasValue)
            {
                return Json(new RegistrationPricingResponse());
            }

            var tenantId = AbpSession.TenantId.Value;
            var departmentId = input.TenantDepartmentId;
            var cohortId = input.CohortId;
            var serviceIds = input.TenantSurpathServiceIds ?? new List<Guid>();

            var response = await CalculateRegistrationPricingAsync(tenantId, departmentId, cohortId, serviceIds);

            return Json(response);
        }

        public class RegistrationPricingInput
        {
            public Guid? TenantDepartmentId { get; set; }
            public Guid? CohortId { get; set; }
            public List<Guid> TenantSurpathServiceIds { get; set; } = new List<Guid>();
        }

        public class RegistrationPricingResponse
        {
            public decimal AmountDue { get; set; }
            public bool RequiresPayment { get; set; }
            public bool AllInvoiced { get; set; }
        }

        private async Task<RegistrationPricingResponse> CalculateRegistrationPricingAsync(
            int tenantId,
            Guid? departmentId,
            Guid? cohortId,
            IEnumerable<Guid> tenantServiceIds)
        {
            var serviceIdList = tenantServiceIds?
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList() ?? new List<Guid>();

            decimal amountDue = 0;
            var allInvoiced = false;
            var evaluated = false;

            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var tenantServices = await _surpathPayManager.GetSurpathServicesForTenant(tenantId);

                if (serviceIdList.Count == 0)
                {
                    serviceIdList = tenantServices.Select(s => s.Id).Distinct().ToList();
                }

                allInvoiced = serviceIdList.Count > 0;

                foreach (var serviceId in serviceIdList)
                {
                    var service = tenantServices.FirstOrDefault(s => s.Id == serviceId);
                    if (service == null || !service.SurpathServiceId.HasValue)
                    {
                        continue;
                    }

                    var decision = await _hierarchicalPricingManager.GetServicePricingDecisionAsync(
                        service.SurpathServiceId.Value,
                        tenantId,
                        departmentId,
                        cohortId,
                        null);

                    if (decision == null || !decision.IsEnabled)
                    {
                        continue;
                    }

                    evaluated = true;
                    amountDue += (decimal)decision.PriceToCharge;

                    if (!decision.IsInvoiced)
                    {
                        allInvoiced = false;
                    }
                }
            }

            if (!evaluated)
            {
                allInvoiced = false;
            }

            if (amountDue < 0)
            {
                amountDue = 0;
            }

            return new RegistrationPricingResponse
            {
                AmountDue = amountDue,
                RequiresPayment = amountDue > 0 && !allInvoiced,
                AllInvoiced = allInvoiced && amountDue <= 0
            };
        }

        //public async Task<ActionResult<RegisterViewModel>> TestNaming()
        //{
        //    var _pids = new List<UserPidViewModel>()
        //    {
        //        new UserPidViewModel(){PidTypeName="",UserPid=new Surpath.Dtos.UserPidDto(){Pid="bingo",PidTypeId=Guid.NewGuid()}},
        //        new UserPidViewModel(){PidTypeName="",UserPid=new Surpath.Dtos.UserPidDto(){Pid="bingo",PidTypeId=Guid.NewGuid()}},
        //        new UserPidViewModel(){PidTypeName="",UserPid=new Surpath.Dtos.UserPidDto(){Pid="bingo",PidTypeId=Guid.NewGuid()}},
        //    };

        //    var obj = new RegisterViewModel
        //    {
        //        PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync(),
        //        ReturnUrl = "",
        //        SingleSignIn = "",
        //        UserPidViewModels = _pids
        //    };

        //    var model = RegisterViewX(obj);

        //    return View(model);
        //}

        //private ActionResult RegisterViewX(RegisterViewModel model)
        //{
        //    CheckSelfRegistrationIsEnabled();

        //    ViewBag.UseCaptcha = !model.IsExternalLogin && UseCaptchaOnRegistration();

        //    return View(model);
        //}

        private ActionResult RegisterView(RegisterViewModel model)
        {
            CheckSelfRegistrationIsEnabled();

            ViewBag.UseCaptcha = !model.IsExternalLogin && UseCaptchaOnRegistration();

            return View("Register", model);
        }

        [HttpPost]
        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            Logger.Info("Register Called [POST]");
            Logger.Info("RegisterCheckoutBtnClick click count: " + model.RegisterCheckoutBtnClick);
            Logger.Debug("RegisterViewModel: " + model);

            if (!AbpSession.TenantId.HasValue)
            {
                Logger.Error("Register aborted - AbpSession.TenantId is null");
                throw new UserFriendlyException(L("TenantNotSpecified"));
            }

            Logger.Debug($"Registration executing under tenant {AbpSession.TenantId}");

            using (_unitOfWorkManager.Current.SetTenantId(AbpSession.TenantId))
            {
                var tenantIsDonorPay = await _surpathManager.IsTenantDonorPay((int)AbpSession.TenantId);
                Logger.Debug($"Tenant donor pay flag: {tenantIsDonorPay}");

                try
                {
                    Logger.Debug("Beginning registration pipeline");

                    if (!model.IsExternalLogin && UseCaptchaOnRegistration())
                    {
                        Logger.Debug("Validating captcha token");
                        await _recaptchaValidator.ValidateAsync(
                            HttpContext.Request.Form[RecaptchaValidator.RecaptchaResponseKey]);
                    }

                    var trimmedPids = model.UserPidViewModels.Where(p => !string.IsNullOrWhiteSpace(p.UserPid.Pid)).ToList();
                    model.UserPidViewModels = trimmedPids;
                    Logger.Debug($"PID entries retained: {trimmedPids.Count}");

                    ExternalLoginInfo externalLoginInfo = null;

                    if (model.IsExternalLogin)
                    {
                        Logger.Debug("External registration detected");
                        externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
                        if (externalLoginInfo == null)
                        {
                            Logger.Error("External login info was null");
                            throw new Exception("Can not external login!");
                        }

                        using (var providerManager =
                               _externalLoginInfoManagerFactory.GetExternalLoginInfoManager(externalLoginInfo.LoginProvider))
                        {
                            model.UserName =
                                providerManager.Object.GetUserNameFromClaims(externalLoginInfo.Principal.Claims.ToList());
                        }

                        model.Password = await _userManager.CreateRandomPassword();
                        Logger.Debug("Generated password for external login user");
                    }
                    else
                    {
                        if (model.UserName.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
                        {
                            Logger.Warn("Submitted form missing username or password");
                            throw new UserFriendlyException(L("FormIsNotValidMessage"));
                        }
                    }

                    Logger.Debug("Username/password validation passed");

                    Logger.Debug("Running registration pre-validation checks");
                    await _registrationValidationManager.EnsureValidAsync(new RegistrationValidationRequest
                    {
                        TenantId = AbpSession.TenantId,
                        EmailAddress = model.EmailAddress,
                        UserName = model.UserName,
                        TenantDepartmentId = model.TenantDepartmentId,
                        CohortId = model.CohortId
                    });
                    Logger.Debug("Registration pre-validation checks complete");

                    if (!model.TenantDepartmentId.HasValue || !model.CohortId.HasValue)
                    {
                        throw new UserFriendlyException("Department and cohort selection are required.");
                    }

                    var pricing = await CalculateRegistrationPricingAsync(
                        AbpSession.TenantId.Value,
                        model.TenantDepartmentId,
                        model.CohortId,
                        model.AuthNetSubmit?.TenantSurpathServiceIds);

                    Logger.Debug($"Computed registration pricing for registration. AmountDue={pricing.AmountDue}, RequiresPayment={pricing.RequiresPayment}, AllInvoiced={pricing.AllInvoiced}");

                    model.AmountDue = pricing.AmountDue;

                    var paymentSubmission = pricing.RequiresPayment ? model.AuthNetSubmit : null;
                    var captureSubmission = pricing.RequiresPayment ? model.AuthNetCaptureResultDto : null;

                    var userPidEntities = model.UserPidViewModels
                        .Select(pid => ObjectMapper.Map<UserPid>(pid.UserPid))
                        .ToList();

                    var coordinatorRequest = new RegistrationPaymentRequest
                    {
                        Name = model.Name,
                        Surname = model.Surname,
                        MiddleName = model.Middlename,
                        UserName = model.UserName,
                        EmailAddress = model.EmailAddress,
                        Password = model.Password,
                        Address = model.Address,
                        SuiteApt = model.SuiteApt,
                        City = model.City,
                        State = model.State,
                        Zip = model.Zip,
                        DateOfBirth = model.DateOfBirth,
                        PhoneNumber = model.PhoneNumber,
                        TenantDepartmentId = model.TenantDepartmentId.Value,
                        CohortId = model.CohortId.Value,
                        UserPids = userPidEntities,
                        IsExternalLogin = model.IsExternalLogin,
                        ExternalLoginInfo = externalLoginInfo,
                        TenantIsDonorPay = tenantIsDonorPay,
                        Payment = paymentSubmission,
                        CaptureResult = captureSubmission,
                        SkipPaymentProcessing = !pricing.RequiresPayment,
                        AmountDueOverride = pricing.AmountDue
                    };

                    Logger.Debug("Delegating registration workflow to RegistrationPaymentCoordinator");
                    var coordinationResult = await _registrationPaymentCoordinator.ExecuteAsync(coordinatorRequest);
                    var user = coordinationResult.User;
                    var tenant = coordinationResult.Tenant;

                    Logger.Debug($"Registration orchestrator complete. PaymentCaptured={coordinationResult.PaymentCaptured}, AmountDue={coordinationResult.AmountDue}");

                    var isEmailConfirmationRequiredForLogin =
                        await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                            .IsEmailConfirmationRequiredForLogin);
                    Logger.Debug($"Email confirmation required: {isEmailConfirmationRequiredForLogin}");

                    Logger.Debug("Attempting automatic login");
                    if (user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin))
                    {
                        AbpLoginResult<Tenant, User> loginResult;
                        if (externalLoginInfo != null)
                        {
                            loginResult = await _logInManager.LoginAsync(externalLoginInfo, tenant.TenancyName);
                        }
                        else
                        {
                            loginResult = await GetLoginResultAsync(user.UserName, model.Password, tenant.TenancyName);
                        }

                        if (loginResult.Result == AbpLoginResultType.Success)
                        {
                            Logger.Debug("Automatic login succeeded");

                            await _signInManager.SignInAsync(loginResult.Identity, false);
                            if (!string.IsNullOrEmpty(model.SingleSignIn) &&
                                model.SingleSignIn.Equals("true", StringComparison.OrdinalIgnoreCase) &&
                                loginResult.Result == AbpLoginResultType.Success)
                            {
                                var returnUrl = NormalizeReturnUrl(model.ReturnUrl);
                                loginResult.User.SetSignInToken();
                                returnUrl = AddSingleSignInParametersToReturnUrl(returnUrl, loginResult.User.SignInToken,
                                    loginResult.User.Id, loginResult.User.TenantId);
                                Logger.Debug("Redirecting to single sign-on return url");
                                return Redirect(returnUrl);
                            }

                            Logger.Debug("Redirecting to app home after login");
                            return Redirect(GetAppHomeUrl());
                        }

                        Logger.Warn("Automatic login failed for new registration. Result=" + loginResult.Result);
                    }

                    Logger.Debug("Registration complete - returning confirmation view");
                    return View("RegisterResult", new RegisterResultViewModel
                    {
                        TenancyName = tenant.TenancyName,
                        NameAndSurname = user.Name + " " + user.Surname,
                        UserName = user.UserName,
                        EmailAddress = user.EmailAddress,
                        IsActive = user.IsActive,
                        IsEmailConfirmationRequired = isEmailConfirmationRequiredForLogin
                    });
                }
                catch (UserFriendlyException ex)
                {
                    Logger.Error("UserFriendlyException during registration: " + ex.Message, ex);
                    Logger.Error($"Registration context -> TenantId={AbpSession.TenantId}, Email={model.EmailAddress}, Username={model.UserName}, DeptId={model.TenantDepartmentId}, CohortId={model.CohortId}");
                    ViewBag.UseCaptcha = !model.IsExternalLogin && UseCaptchaOnRegistration();
                    ViewBag.ErrorMessage = ex.Message;

                    model.PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync();

                    return View("Register", model);
                }
                catch (Exception ex)
                {
                    Logger.Error("Unexpected exception during registration", ex);
                    Logger.Error($"Registration context -> TenantId={AbpSession.TenantId}, Email={model.EmailAddress}, Username={model.UserName}, DeptId={model.TenantDepartmentId}, CohortId={model.CohortId}");

                    ViewBag.UseCaptcha = !model.IsExternalLogin && UseCaptchaOnRegistration();
                    ViewBag.ErrorMessage = ex.Message;

                    model.PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync();

                    return View("Register", model);
                }
            }
        }
        private bool UseCaptchaOnRegistration()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                //Host users can not register
                throw new InvalidOperationException();
            }

            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration);
        }

        private bool UseCaptchaOnLogin()
        {
            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnLogin);
        }

        private void CheckSelfRegistrationIsEnabled()
        {
            if (!IsSelfRegistrationEnabled())
            {
                throw new UserFriendlyException(L("SelfUserRegistrationIsDisabledMessage_Detail"));
            }
        }

        private bool IsSelfRegistrationEnabled()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return false; //No registration enabled for host users!
            }

            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowSelfRegistration);
        }

        private bool IsTenantSelfRegistrationEnabled()
        {
            if (AbpSession.TenantId.HasValue)
            {
                return false;
            }

            return SettingManager.GetSettingValue<bool>(AppSettings.TenantManagement.AllowSelfRegistration);
        }

        #endregion Register

        #region Payment

        //         BuyServicesViewModel

        //[HttpPost]
        //public async Task authnetAsync(AuthNetSubmit authNetSubmit)
        //{
        //    //var opaqueData = new opaqueDataType
        //    //{
        //    //    dataDescriptor = authNetSubmit.dataDescriptor,
        //    //    dataValue = authNetSubmit.dataValue

        //    //};
        //    var res = await _authNetManager.ChargeCreditCardRequest(authNetSubmit, (long)AbpSession.UserId, authNetSubmit.amount);

        //}

        #endregion Payment

        #region ForgotPassword / ResetPassword

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SendPasswordResetLink(SendPasswordResetLinkViewModel model)
        {
            await _accountAppService.SendPasswordResetCode(
                new SendPasswordResetCodeInput
                {
                    EmailAddress = model.EmailAddress
                });

            return Json(new AjaxResponse());
        }

        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            await SwitchToTenantIfNeeded(model.TenantId);

            var user = await _userManager.GetUserByIdAsync(model.UserId);
            if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != model.ResetCode)
            {
                throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));
            }

            model.PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordInput input)
        {
            var output = await _accountAppService.ResetPassword(input);

            if (output.CanLogin)
            {
                var user = await _userManager.GetUserByIdAsync(input.UserId);
                await _signInManager.SignInAsync(user, false);

                if (!string.IsNullOrEmpty(input.SingleSignIn) &&
                    input.SingleSignIn.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    user.SetSignInToken();
                    var returnUrl =
                        AddSingleSignInParametersToReturnUrl(input.ReturnUrl, user.SignInToken, user.Id, user.TenantId);
                    return Redirect(returnUrl);
                }
            }

            return Redirect(NormalizeReturnUrl(input.ReturnUrl));
        }

        #endregion ForgotPassword / ResetPassword

        #region Email activation / confirmation

        public ActionResult EmailActivation()
        {
            return View();
        }

        [HttpPost]
        public virtual async Task<JsonResult> SendEmailActivationLink(SendEmailActivationLinkInput model)
        {
            Logger.Debug($"Sending email activation link {model.EmailAddress}");
            await _accountAppService.SendEmailActivationLink(model);
            return Json(new AjaxResponse());
        }

        public virtual async Task<ActionResult> EmailConfirmation(EmailConfirmationViewModel input)
        {
            await SwitchToTenantIfNeeded(input.TenantId);
            await _accountAppService.ActivateEmail(input);
            return RedirectToAction(
                "Login",
                new
                {
                    successMessage = L("YourEmailIsConfirmedMessage"),
                    userNameOrEmailAddress = (await _userManager.GetUserByIdAsync(input.UserId)).UserName
                });
        }

        #endregion Email activation / confirmation

        #region External Login

        [HttpPost]
        public ActionResult ExternalLogin(string provider, string returnUrl, string ss = "")
        {
            var redirectUrl = Url.Action(
                "ExternalLoginCallback",
                "Account",
                new
                {
                    ReturnUrl = returnUrl,
                    authSchema = provider,
                    ss = ss
                });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl, string remoteError = null,
            string ss = "")
        {
            returnUrl = NormalizeReturnUrl(returnUrl);

            if (remoteError != null)
            {
                Logger.Error("Remote Error in ExternalLoginCallback: " + remoteError);
                throw new UserFriendlyException(L("CouldNotCompleteLoginOperation"));
            }

            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                Logger.Warn("Could not get information from external login.");
                return RedirectToAction(nameof(Login));
            }

            var tenancyName = GetTenancyNameOrNull();

            var loginResult = await _logInManager.LoginAsync(externalLoginInfo, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    {
                        await _signInManager.SignInAsync(loginResult.Identity, false);

                        if (!string.IsNullOrEmpty(ss) && ss.Equals("true", StringComparison.OrdinalIgnoreCase) &&
                            loginResult.Result == AbpLoginResultType.Success)
                        {
                            loginResult.User.SetSignInToken();
                            returnUrl = AddSingleSignInParametersToReturnUrl(returnUrl, loginResult.User.SignInToken,
                                loginResult.User.Id, loginResult.User.TenantId);
                        }

                        return Redirect(returnUrl);
                    }
                case AbpLoginResultType.UnknownExternalLogin:
                    return await RegisterForExternalLogin(externalLoginInfo);

                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                        loginResult.Result,
                        externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email) ?? externalLoginInfo.ProviderKey,
                        tenancyName
                    );
            }
        }

        private async Task<ActionResult> RegisterForExternalLogin(ExternalLoginInfo externalLoginInfo)
        {
            var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);

            (string name, string surname) nameInfo;
            using (var providerManager =
                   _externalLoginInfoManagerFactory.GetExternalLoginInfoManager(externalLoginInfo.LoginProvider))
            {
                nameInfo = providerManager.Object.GetNameAndSurnameFromClaims(
                    externalLoginInfo.Principal.Claims.ToList(), _identityOptions);
            }

            var viewModel = new RegisterViewModel
            {
                EmailAddress = email,
                Name = nameInfo.name,
                Surname = nameInfo.surname,
                IsExternalLogin = true,
                ExternalLoginAuthSchema = externalLoginInfo.LoginProvider
            };

            if (nameInfo.name != null &&
                nameInfo.surname != null &&
                email != null)
            {
                return await Register(viewModel);
            }

            return RegisterView(viewModel);
        }

        #endregion External Login

        #region Impersonation

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users_Impersonation)]
        public virtual async Task<JsonResult> ImpersonateUser([FromBody] ImpersonateUserInput input)
        {
            var output = await _accountAppService.ImpersonateUser(input);

            await _signInManager.SignOutAsync();

            return Json(new AjaxResponse
            {
                TargetUrl = _webUrlService.GetSiteRootAddress(output.TenancyName) +
                            "Account/ImpersonateSignIn?tokenId=" + output.ImpersonationToken
            });
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenants_Impersonation)]
        public virtual async Task<JsonResult> ImpersonateTenant([FromBody] ImpersonateTenantInput input)
        {
            var output = await _accountAppService.ImpersonateTenant(input);

            await _signInManager.SignOutAsync();

            return Json(new AjaxResponse
            {
                TargetUrl = _webUrlService.GetSiteRootAddress(output.TenancyName) +
                            "Account/ImpersonateSignIn?tokenId=" + output.ImpersonationToken
            });
        }

        public virtual async Task<ActionResult> ImpersonateSignIn(string tokenId)
        {
            await ClearGetScriptsResponsePerUserCache();

            var result = await _impersonationManager.GetImpersonatedUserAndIdentity(tokenId);
            await _signInManager.SignInAsync(result.Identity, false);
            return RedirectToAppHome();
        }

        [AbpMvcAuthorize]
        public virtual async Task<JsonResult> DelegatedImpersonate([FromBody] DelegatedImpersonateInput input)
        {
            var output = await _accountAppService.DelegatedImpersonate(new DelegatedImpersonateInput
            {
                UserDelegationId = input.UserDelegationId
            });

            await _signInManager.SignOutAsync();

            return Json(new AjaxResponse
            {
                TargetUrl = _webUrlService.GetSiteRootAddress(output.TenancyName) +
                            "Account/DelegatedImpersonateSignIn?userDelegationId=" + input.UserDelegationId +
                            "&tokenId=" + output.ImpersonationToken
            });
        }

        public virtual async Task<ActionResult> DelegatedImpersonateSignIn(long userDelegationId, string tokenId)
        {
            await ClearGetScriptsResponsePerUserCache();

            var userDelegation = await _userDelegationManager.GetAsync(userDelegationId);
            var result = await _impersonationManager.GetImpersonatedUserAndIdentity(tokenId);

            if (userDelegation.SourceUserId != result.User.Id)
            {
                throw new UserFriendlyException("User delegation error...");
            }

            await _signInManager.SignInWithClaimsAsync(result.User, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = userDelegation.EndTime.ToUniversalTime()
            }, result.Identity.Claims);

            return RedirectToAppHome();
        }

        public virtual JsonResult IsImpersonatedLogin()
        {
            return Json(new AjaxResponse { Result = AbpSession.ImpersonatorUserId.HasValue });
        }

        public virtual async Task<JsonResult> BackToImpersonator()
        {
            var output = await _accountAppService.BackToImpersonator();

            await _signInManager.SignOutAsync();

            return Json(new AjaxResponse
            {
                TargetUrl = _webUrlService.GetSiteRootAddress(output.TenancyName) +
                            "Account/ImpersonateSignIn?tokenId=" + output.ImpersonationToken
            });
        }

        private async Task ClearGetScriptsResponsePerUserCache()
        {
            if (!_getScriptsResponsePerUserConfiguration.IsEnabled)
            {
                return;
            }

            await _cachedUniqueKeyPerUser.RemoveKeyAsync(GetScriptsResponsePerUserCache.CacheName);
        }

        #endregion Impersonation

        #region Linked Account

        [AbpMvcAuthorize]
        public virtual async Task<JsonResult> SwitchToLinkedAccount([FromBody] SwitchToLinkedAccountInput model)
        {
            var output = await _accountAppService.SwitchToLinkedAccount(model);

            await _signInManager.SignOutAsync();

            return Json(new AjaxResponse
            {
                TargetUrl = _webUrlService.GetSiteRootAddress(output.TenancyName) +
                            "Account/SwitchToLinkedAccountSignIn?tokenId=" + output.SwitchAccountToken
            });
        }

        public virtual async Task<ActionResult> SwitchToLinkedAccountSignIn(string tokenId)
        {
            var result = await _userLinkManager.GetSwitchedUserAndIdentity(tokenId);

            await _signInManager.SignInAsync(result.Identity, false);
            return RedirectToAppHome();
        }

        #endregion Linked Account

        #region Change Tenant

        public async Task<ActionResult> TenantChangeModal()
        {
            var loginInfo = await _sessionCache.GetCurrentLoginInformationsAsync();
            return View("/Views/Shared/Components/TenantChange/_ChangeModal.cshtml", new ChangeModalViewModel
            {
                TenancyName = loginInfo.Tenant?.TenancyName
            });
        }

        #endregion Change Tenant

        #region Common

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private void CheckCurrentTenant(int? tenantId)
        {
            if (AbpSession.TenantId != tenantId)
            {
                throw new Exception(
                    $"Current tenant is different than given tenant. AbpSession.TenantId: {AbpSession.TenantId}, given tenantId: {tenantId}");
            }
        }

        private async Task SwitchToTenantIfNeeded(int? tenantId)
        {
            if (tenantId != AbpSession.TenantId)
            {
                if (_webUrlService.SupportsTenancyNameInUrl)
                {
                    throw new InvalidOperationException($"Given tenantid ({tenantId}) does not match to tenant's URL!");
                }

                SetTenantIdCookie(tenantId);
                CurrentUnitOfWork.SetTenantId(tenantId);
                await _signInManager.SignOutAsync();
            }
        }

        #endregion Common

        #region Helpers

        public ActionResult RedirectToAppHome()
        {
            return RedirectToAction("Index", "Home", new { area = "App" });
        }

        public string GetAppHomeUrl()
        {
            return Url.Action("Index", "Home", new { area = "App" });
        }

        private string NormalizeReturnUrl(string returnUrl, Func<string> defaultValueBuilder = null)
        {
            if (defaultValueBuilder == null)
            {
                defaultValueBuilder = GetAppHomeUrl;
            }

            if (returnUrl.IsNullOrEmpty())
            {
                return defaultValueBuilder();
            }

            if (AbpSession.UserId.HasValue)
            {
                return defaultValueBuilder();
            }

            if (Url.IsLocalUrl(returnUrl) ||
                _webUrlService.GetRedirectAllowedExternalWebSites().Any(returnUrl.Contains))
            {
                return returnUrl;
            }

            return defaultValueBuilder();
        }

        #endregion Helpers

        #region Etc

        [AbpMvcAuthorize]
        public async Task<ActionResult> TestNotification(string message = "", string severity = "info")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            await _appNotifier.SendMessageAsync(
                AbpSession.ToUserIdentifier(),
                message,
                severity.ToPascalCase().ToEnum<NotificationSeverity>()
            );

            return Content("Sent notification: " + message);
        }

        #endregion Etc
    }
}
