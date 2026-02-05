using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using inzibackend.Authentication.TwoFactor;
using inzibackend.Editions;
using inzibackend.MultiTenancy.Payments;
using inzibackend.Sessions.Dto;
using inzibackend.UiCustomization;
using inzibackend.Authorization.Delegation;
using inzibackend.Authorization.Users;
using Abp.Domain.Uow;
using Abp.Localization;
using inzibackend.Features;
using inzibackend.Surpath.SurpathPay;
using inzibackend.Surpath.Dtos;
using inzibackend.Surpath;
using Abp.Domain.Repositories;

namespace inzibackend.Sessions
{
    public class SessionAppService : inzibackendAppServiceBase, ISessionAppService
    {
        private readonly IUiThemeCustomizerFactory _uiThemeCustomizerFactory;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IUserDelegationConfiguration _userDelegationConfiguration;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly EditionManager _editionManager;
        private readonly ILocalizationContext _localizationContext;
        private readonly UserManager _userManager;

        private readonly IPurchaseAppService _purchaseAppService;
        private readonly IRepository<TenantDepartmentUser, Guid> _tenantDepartmentUserRepository;
        private readonly IUserAppService _userAppService;
        private readonly HierarchicalPricingManager _hierarchicalPricingManager;
        //private readonly SurpathPayManager _surpathPayManager;

        public SessionAppService(
            IUiThemeCustomizerFactory uiThemeCustomizerFactory,
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            IUserDelegationConfiguration userDelegationConfiguration,
            IUnitOfWorkManager unitOfWorkManager,
            EditionManager editionManager, ILocalizationContext localizationContext,
            IPurchaseAppService purchaseAppService,
            IRepository<TenantDepartmentUser, Guid> tenantDepartmentUserRepository,
            IUserAppService userAppService,
            UserManager userManager,
            HierarchicalPricingManager hierarchicalPricingManager
            //,SurpathPayManager surpathPayManager
            )
        {
            _uiThemeCustomizerFactory = uiThemeCustomizerFactory;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _userDelegationConfiguration = userDelegationConfiguration;
            _unitOfWorkManager = unitOfWorkManager;
            _editionManager = editionManager;
            _localizationContext = localizationContext;
            _purchaseAppService = purchaseAppService;
            _tenantDepartmentUserRepository = tenantDepartmentUserRepository;
            _userAppService = userAppService;
            _hierarchicalPricingManager = hierarchicalPricingManager;
            //_surpathPayManager = surpathPayManager;
            _userManager = userManager;
        }

        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            return await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                var output = new GetCurrentLoginInformationsOutput
                {
                    Application = new ApplicationInfoDto
                    {
                        Version = AppVersionHelper.Version,
                        ReleaseDate = AppVersionHelper.ReleaseDate,
                        Features = new Dictionary<string, bool>(),
                        Currency = inzibackendConsts.Currency,
                        CurrencySign = inzibackendConsts.CurrencySign,
                        AllowTenantsToChangeEmailSettings = inzibackendConsts.AllowTenantsToChangeEmailSettings,
                        UserDelegationIsEnabled = _userDelegationConfiguration.IsEnabled,
                        TwoFactorCodeExpireSeconds = TwoFactorCodeCacheItem.DefaultSlidingExpireTime.TotalSeconds
                    }
                };

                var uiCustomizer = await _uiThemeCustomizerFactory.GetCurrentUiCustomizer();
                output.Theme = await uiCustomizer.GetUiSettings();

                if (AbpSession.TenantId.HasValue)
                {
                    output.Tenant = await GetTenantLoginInfo(AbpSession.GetTenantId());
                }

                if (AbpSession.ImpersonatorTenantId.HasValue)
                {
                    output.ImpersonatorTenant = await GetTenantLoginInfo(AbpSession.ImpersonatorTenantId.Value);
                }


                if (AbpSession.UserId.HasValue)
                {
                    output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
                    output.User.IsPaid = await _purchaseAppService.DonorCurrent(output.User.Id);
                    output.User.CohortUserId = await _purchaseAppService.GetChortUserId(output.User.Id);
                    output.User.IsCohortUser = output.User.CohortUserId.HasValue==true && (Guid)output.User.CohortUserId != Guid.Empty;
                    output.User.Departments = _tenantDepartmentUserRepository.GetAllList(tu=>tu.UserId==output.User.Id).Select(tu=>tu.TenantDepartmentId).ToList();
                    output.User.DepartmentsAuthed = output.User.Departments;
                    output.User.Roles = await _purchaseAppService.GetUserRolesAsStringList(output.User.Id);
                    
                    // Check if user should be invoiced based on hierarchical pricing
                    if (AbpSession.TenantId.HasValue)
                    {
                        var deptId = output.User.Departments?.FirstOrDefault();
                        output.User.IsInvoiced = await _hierarchicalPricingManager.GetIsInvoicedForUserAsync(
                            AbpSession.TenantId.Value,
                            deptId,
                            output.User.CohortUserId,
                            output.User.Id);
                    }
                    
                    //output.User.Departments = await _tenantDepartmentUserRepository.GetAllList().Where(tu=>tu.UserId == output.User.Id).Select(tu=>tu.TenantDepartmentId).ToList();
                }

                if (AbpSession.ImpersonatorUserId.HasValue)
                {
                    output.ImpersonatorUser = ObjectMapper.Map<UserLoginInfoDto>(await GetImpersonatorUserAsync());
                }

                output.SurpathSettings = SurpathSettings.GetDto();


                if (output.Tenant == null)
                {
                    return output;
                }

                if (output.Tenant.Edition != null)
                {
                    var lastPayment =
                        await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(output.Tenant.Id,
                            null, null);
                    if (lastPayment != null)
                    {
                        output.Tenant.Edition.IsHighestEdition = IsEditionHighest(output.Tenant.Edition.Id,
                            lastPayment.GetPaymentPeriodType());
                    }
                }

                output.Tenant.SubscriptionDateString = GetTenantSubscriptionDateString(output);
                output.Tenant.CreationTimeString = output.Tenant.CreationTime.ToString("d");



                return output;
            });
        }

        private async Task<TenantLoginInfoDto> GetTenantLoginInfo(int tenantId)
        {
            var tenant = await TenantManager.Tenants
                .Include(t => t.Edition)
                .FirstAsync(t => t.Id == AbpSession.GetTenantId());

            var tenantLoginInfo = ObjectMapper
                .Map<TenantLoginInfoDto>(tenant);

            if (!tenant.EditionId.HasValue)
            {
                return tenantLoginInfo;
            }
            
            var features = FeatureManager
                .GetAll()
                .Where(feature => (feature[FeatureMetadata.CustomFeatureKey] as FeatureMetadata)?.IsVisibleOnPricingTable ?? false);
            
            var featureDictionary = features.ToDictionary(feature => feature.Name, f => f);
            
            tenantLoginInfo.FeatureValues = (await _editionManager.GetFeatureValuesAsync(tenant.EditionId.Value))
                .Where(featureValue => featureDictionary.ContainsKey(featureValue.Name))
                .Select(fv => new NameValueDto(
                    featureDictionary[fv.Name].DisplayName.Localize(_localizationContext),
                    featureDictionary[fv.Name].GetValueText(fv.Value, _localizationContext))
                )
                .ToList();

            return tenantLoginInfo;
        }

        private bool IsEditionHighest(int editionId, PaymentPeriodType paymentPeriodType)
        {
            var topEdition = GetHighestEditionOrNullByPaymentPeriodType(paymentPeriodType);
            if (topEdition == null)
            {
                return false;
            }

            return editionId == topEdition.Id;
        }

        private SubscribableEdition GetHighestEditionOrNullByPaymentPeriodType(PaymentPeriodType paymentPeriodType)
        {
            var editions = TenantManager.EditionManager.Editions;
            if (editions == null || !editions.Any())
            {
                return null;
            }

            var query = editions.Cast<SubscribableEdition>();

            switch (paymentPeriodType)
            {
                case PaymentPeriodType.Daily:
                    query = query.OrderByDescending(e => e.DailyPrice ?? 0);
                    break;
                case PaymentPeriodType.Weekly:
                    query = query.OrderByDescending(e => e.WeeklyPrice ?? 0);
                    break;
                case PaymentPeriodType.Monthly:
                    query = query.OrderByDescending(e => e.MonthlyPrice ?? 0);
                    break;
                case PaymentPeriodType.Annual:
                    query = query.OrderByDescending(e => e.AnnualPrice ?? 0);
                    break;
            }

            return query.FirstOrDefault();
        }

        private string GetTenantSubscriptionDateString(GetCurrentLoginInformationsOutput output)
        {
            return output.Tenant.SubscriptionEndDateUtc == null
                ? L("Unlimited")
                : output.Tenant.SubscriptionEndDateUtc?.ToString("d");
        }

        public async Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken()
        {
            if (AbpSession.UserId <= 0)
            {
                throw new Exception(L("ThereIsNoLoggedInUser"));
            }

            var user = await UserManager.GetUserAsync(AbpSession.ToUserIdentifier());
            user.SetSignInToken();
            return new UpdateUserSignInTokenOutput
            {
                SignInToken = user.SignInToken,
                EncodedUserId = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Id.ToString())),
                EncodedTenantId = user.TenantId.HasValue
                    ? Convert.ToBase64String(Encoding.UTF8.GetBytes(user.TenantId.Value.ToString()))
                    : ""
            };
        }

        protected virtual async Task<User> GetImpersonatorUserAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.ImpersonatorTenantId))
            {
                var user = await UserManager.FindByIdAsync(AbpSession.ImpersonatorUserId.ToString());
                if (user == null)
                {
                    throw new Exception("User not found!");
                }

                return user;
            }
        }
    }
}
