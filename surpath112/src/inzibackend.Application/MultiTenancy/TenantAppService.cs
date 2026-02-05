using Abp;
using Abp.Application.Features;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Notifications;
using Abp.Organizations;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.Zero.Configuration;
using inzibackend.Authorization;
using inzibackend.Authorization.Roles;
using inzibackend.Authorization.Users;
using inzibackend.Authorization.Users.Dto;
using inzibackend.Authorization.Users.Exporting;
using inzibackend.Dto;
using inzibackend.Editions.Dto;
using inzibackend.Features;
using inzibackend.MultiTenancy.Dto;
using inzibackend.MultiTenancy.Importing;
using inzibackend.MultiTenancy.Importing.Dto;
using inzibackend.Net.Emailing;
using inzibackend.Notifications;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using inzibackend.Surpath.Exporting;
using inzibackend.Url;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace inzibackend.MultiTenancy
{
    [AbpAuthorize(AppPermissions.Pages_Tenants)]
    public class TenantAppService : inzibackendAppServiceBase, ITenantAppService
    {
        public IAppUrlService AppUrlService { get; set; }
        public IEventBus EventBus { get; set; }

        private readonly RoleManager _roleManager;
        private readonly IUserEmailer _userEmailer;
        private readonly IUserListExcelExporter _userListExcelExporter;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionRepository;
        private readonly IRepository<UserPermissionSetting, long> _userPermissionRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUserPolicy _userPolicy;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRoleManagementConfig _roleManagementConfig;
        private readonly UserManager _userManager;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnitRole, long> _organizationUnitRoleRepository;
        private readonly IOptions<UserOptions> _userOptions;
        private readonly IEmailSettingsChecker _emailSettingsChecker;

        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly IRepository<RecordCategoryRule, Guid> _recordCategoryRuleRepository;
        private readonly IRepository<RecordCategory, Guid> _recordCategoryRepository;
        private readonly IRepository<RecordRequirement, Guid> _recordRequirementRepository;
        private readonly IRepository<TenantDocumentCategory, Guid> _tenantDocumentCategoryRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<RecordStatus, Guid> _recordStatusRepository;
        private readonly IRepository<RecordState, Guid> _recordStateRepository;

        private readonly IRepository<TenantSurpathService, Guid> _tenantSurpathServiceRepository;
        private readonly IRepository<SurpathService, Guid> _surpathServiceRepository;
        private readonly IRepository<PidType, Guid> _pidTypeRepository;

        private readonly IRecordStatusesAppService _recordStatusesAppService;

        private readonly IRepository<LedgerEntryDetail, Guid> _ledgerEntryDetailRepository;
        private readonly IRepository<LedgerEntry, Guid> _ledgerEntryRepository;
        private readonly ITenantUserExporter _tenantUserExporter;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<DepartmentUser, Guid> _departmentUserRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;
        private readonly IRepository<UserPid, Guid> _userPidRepository;

        public TenantAppService(
            RoleManager roleManager,
            IUserEmailer userEmailer,
            IUserListExcelExporter userListExcelExporter,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier,
            IRepository<RolePermissionSetting, long> rolePermissionRepository,
            IRepository<UserPermissionSetting, long> userPermissionRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IUserPolicy userPolicy,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRoleManagementConfig roleManagementConfig,
            UserManager userManager,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository,
            IOptions<UserOptions> userOptions, IEmailSettingsChecker emailSettingsChecker,
            IRepository<Cohort, Guid> cohortRepository,
            IRepository<RecordState, Guid> recordStateRepository,
            IRepository<RecordCategoryRule, Guid> recordCategoryRuleRepository,
            IRepository<RecordCategory, Guid> recordCategoryRepository,
            IRepository<RecordRequirement, Guid> recordRequirementRepository,
            IRepository<TenantDocumentCategory, Guid> tenantDocumentCategoryRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<RecordStatus, Guid> recordStatusRepository,
            IRepository<TenantSurpathService, Guid> tenantSurpathServiceRepository,
            IRepository<SurpathService, Guid> surpathServiceRepository,
             IRepository<PidType, Guid> pidTypeRepository,
             IRecordStatusesAppService recordStatusesAppService,
             IRepository<CohortUser, Guid> cohortUserRepository,
         IRepository<LedgerEntryDetail, Guid> ledgerEntryDetailRepository,
        IRepository<LedgerEntry, Guid> ledgerEntryRepository,
        ITenantUserExporter tenantUserExporter,
        IRepository<User, long> userRepository,
        IRepository<DepartmentUser, Guid> departmentUserRepository,
        IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
        IRepository<UserPid, Guid> userPidRepository
    )
        {
            AppUrlService = NullAppUrlService.Instance;
            EventBus = NullEventBus.Instance;

            _roleManager = roleManager;
            _userEmailer = userEmailer;
            _userListExcelExporter = userListExcelExporter;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _rolePermissionRepository = rolePermissionRepository;
            _userPermissionRepository = userPermissionRepository;
            _userRoleRepository = userRoleRepository;
            _userPolicy = userPolicy;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
            _organizationUnitRepository = organizationUnitRepository;
            _roleManagementConfig = roleManagementConfig;
            _userManager = userManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRoleRepository = organizationUnitRoleRepository;
            _userOptions = userOptions;
            _emailSettingsChecker = emailSettingsChecker;
            _roleRepository = roleRepository;

            _cohortRepository = cohortRepository;
            _cohortUserRepository = cohortUserRepository;
            _recordStateRepository = recordStateRepository;
            _recordCategoryRuleRepository = recordCategoryRuleRepository;
            _recordCategoryRepository = recordCategoryRepository;
            _recordRequirementRepository = recordRequirementRepository;
            _tenantDocumentCategoryRepository = tenantDocumentCategoryRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _recordStatusRepository = recordStatusRepository;

            _tenantSurpathServiceRepository = tenantSurpathServiceRepository;
            _surpathServiceRepository = surpathServiceRepository;
            _pidTypeRepository = pidTypeRepository;

            _recordStatusesAppService = recordStatusesAppService;

            _ledgerEntryDetailRepository = ledgerEntryDetailRepository;
            _ledgerEntryRepository = ledgerEntryRepository;
            _tenantUserExporter = tenantUserExporter;
            _userRepository = userRepository;
            _departmentUserRepository = departmentUserRepository;
            _tenantDepartmentRepository = tenantDepartmentRepository;
            _userPidRepository = userPidRepository;
        }

        public async Task<PagedResultDto<TenantListDto>> GetTenants(GetTenantsInput input)
        {
            var query = TenantManager.Tenants
                .Include(t => t.Edition)
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), t => t.Name.Contains(input.Filter) || t.TenancyName.Contains(input.Filter))
                .WhereIf(input.CreationDateStart.HasValue, t => t.CreationTime >= input.CreationDateStart.Value)
                .WhereIf(input.CreationDateEnd.HasValue, t => t.CreationTime <= input.CreationDateEnd.Value)
                .WhereIf(input.SubscriptionEndDateStart.HasValue, t => t.SubscriptionEndDateUtc >= input.SubscriptionEndDateStart.Value.ToUniversalTime())
                .WhereIf(input.SubscriptionEndDateEnd.HasValue, t => t.SubscriptionEndDateUtc <= input.SubscriptionEndDateEnd.Value.ToUniversalTime())
                .WhereIf(input.EditionIdSpecified, t => t.EditionId == input.EditionId);

            var tenantCount = await query.CountAsync();
            var tenants = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<TenantListDto>(
                tenantCount,
                ObjectMapper.Map<List<TenantListDto>>(tenants)
                );
        }

        public async Task<List<TenantListDto>> GetTenantsList()
        {
            var query = TenantManager.Tenants
                .Include(t => t.Edition);

            var tenantCount = await query.CountAsync();
            var tenants = await query.ToListAsync();

            return
                ObjectMapper.Map<List<TenantListDto>>(tenants);
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_Create)]
        [UnitOfWork(IsDisabled = true)]
        public async Task CreateTenant(CreateTenantInput input)
        {
            var _tenantid = await TenantManager.CreateWithAdminUserAsync(input.TenancyName,
                input.Name,
                input.AdminPassword,
                input.AdminEmailAddress,
                input.ConnectionString,
                input.IsActive,
                input.EditionId,
                input.ShouldChangePasswordOnNextLogin,
                input.SendActivationEmail,
                input.SubscriptionEndDateUtc?.ToUniversalTime(),
                input.IsInTrialPeriod,
                input.ClientCode,
                AppUrlService.CreateEmailActivationUrlFormat(input.TenancyName),
                adminName: input.AdminName,
                adminSurname: input.AdminSurname

            );
            await EnsureTenantFeaturesAndPidTypes(_tenantid);


        }

        private async Task EnsureTenantFeaturesAndPidTypes(int _tenantid, bool deferDonorPay = false, bool perpetualDonorPay = false)
        {
            //CurrentUnitOfWork.SaveChanges();
            using (var uow = _unitOfWorkManager.Begin())
            {
                var output = await GetTenantFeaturesForEdit(new EntityDto(_tenantid));
                var _tFeatureinput = new UpdateTenantFeaturesInput();
                _tFeatureinput.Id = _tenantid;
                _tFeatureinput.FeatureValues = output.FeatureValues;

                // This needs to be updated to clone settings that don't exist for the tenant
                await EnsureTenantFeaturesSurpathSettings(_tenantid, _tFeatureinput);
                // This also needs to be updated to clone settings that don't exist for the tenant
                await _recordStatusesAppService.EnsureSurpathServiceStatuses(_tenantid);
                // This also needs to be update to clone settings that don'te exist for the tenant
                await EnsureTenantPidTypes(_tenantid);

                // If the tenent has donor pay enabled. we need to check.
                if (deferDonorPay == true)
                {
                    // For every donor for this tenant, we need to set the defered date to 1 year from their registration if tenant is set to defer donor pay
                    var _expirationDate = DateTime.MinValue;
                    if (deferDonorPay) _expirationDate = DateTime.UtcNow.AddYears(1);
                    if (perpetualDonorPay) _expirationDate = DateTime.MaxValue;

                    // should we do this for the users, not the cohort users?
                    var _cohortUsers = _cohortUserRepository.GetAll().Include(cu => cu.UserFk).IgnoreQueryFilters().Where(cu => cu.TenantId == _tenantid).ToList();
                    foreach (var _cu in _cohortUsers)
                    {
                        var _regdate = _cu.UserFk.CreationTime.ToUniversalTime();
                        var _deferdate = _regdate.AddYears(1);
                        if (perpetualDonorPay) _deferdate = DateTime.MaxValue;
                        _cu.UserFk.DonorPayPromptDelayUntil = _deferdate;

                        // Create a ledger entry for the donor along with ledger detail for the transaction
                        var _ledgerEntry = _ledgerEntryRepository.GetAll().IgnoreQueryFilters().Where(le => le.TenantId == _tenantid && le.UserId == _cu.UserId).FirstOrDefault();
                        if (_ledgerEntry == null)
                        {
                            // get all prices for all services current for tenant

                            var _tenantServices = _tenantSurpathServiceRepository.GetAll().IgnoreQueryFilters().Where(ss => ss.TenantId == _tenantid && ss.IsDeleted == false).ToList();
                            var _totalDue = _tenantServices.Sum(ss => Convert.ToDecimal(ss.Price));


                            _ledgerEntry = new LedgerEntry()
                            {
                                TenantId = _tenantid,
                                UserId = _cu.UserId,
                                IsDeleted = false,
                                AmountDue = _totalDue,
                                PaidAmount = 0m,
                                TotalPrice = _totalDue,
                                ExpirationDate = _expirationDate
                            };
                            await _ledgerEntryRepository.InsertAsync(_ledgerEntry);

                            // add a detail for each service
                            foreach (var _ts in _tenantServices)
                            {
                                var _ledgerEntryDetail = new LedgerEntryDetail()
                                {
                                    TenantId = _tenantid,
                                    LedgerEntryId = _ledgerEntry.Id,
                                    SurpathServiceId = (Guid)_ts.SurpathServiceId,
                                    Amount = Convert.ToDecimal(_ts.Price),
                                    TenantSurpathServiceId = _ts.Id,
                                    IsDeleted = false,
                                };
                                await _ledgerEntryDetailRepository.InsertAsync(_ledgerEntryDetail);
                            }
                        }

                    }
                }
                CurrentUnitOfWork.SaveChanges();

                uow.Complete();
            }
        }


        [AbpAuthorize(AppPermissions.Pages_Tenants_Create)]
        [UnitOfWork(IsDisabled = true)]
        public async Task CreateTenantWizard(CreateTenantWizardInput input)
        {

            var _tenantId = await TenantManager.CreateWithAdminUserAsync(input.TenancyName,
            input.Name,
            input.AdminPassword,
            input.AdminEmailAddress,
            input.ConnectionString,
            input.IsActive,
            input.EditionId,
            input.ShouldChangePasswordOnNextLogin,
            input.SendActivationEmail,
            input.SubscriptionEndDateUtc?.ToUniversalTime(),
            input.IsInTrialPeriod,
            input.ClientCode,
            AppUrlService.CreateEmailActivationUrlFormat(input.TenancyName),
            adminName: input.AdminName,
            adminSurname: input.AdminSurname,
            input.IsDonorPay,
            input.IsDeferDonorPerpetualPay,
            (int)input.ClientPaymentType

        );
            await EnsureTenantFeaturesAndPidTypes(_tenantId);

            long _userId;
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(_tenantId))
                {
                    CreateOrUpdateUserInput newUser = new CreateOrUpdateUserInput()
                    {
                        AssignedRoleNames = new string[1] { "admin" },
                        SendActivationEmail = true,
                        SetRandomPassword = string.IsNullOrWhiteSpace(input.UserAdminPassword),
                        OrganizationUnits = new List<long>(),
                    };
                    newUser.User = new UserEditDto()
                    {
                        Name = input.UserAdminName,
                        Middlename = input.UserAdminMiddlename,
                        Surname = input.UserAdminSurname,
                        UserName = input.UserAdminUserName,
                        EmailAddress = input.UserAdminEmailAddress,
                        PhoneNumber = input.UserAdminPhoneNumber,
                        Password = input.UserAdminPassword,
                        ShouldChangePasswordOnNextLogin = input.UserAdminShouldChangePasswordOnNextLogin,
                        IsActive = input.UserAdminIsActive,
                        IsLockoutEnabled = input.UserAdminIsLockoutEnabled,
                        IsTwoFactorEnabled = input.UserAdminIsTwoFactorEnabled,
                    };
                    _userId = await CreateAdminUserForTenantWizard(newUser, _tenantId);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    uow.Complete();


                }
            }
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(_tenantId))
                {
                    await CreateTenantDefaultObjects(_tenantId, _userId);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    uow.Complete();

                }
            }

        }
        private async Task CreateTenantDefaultObjects(int _tenantId, long _userId)
        {
            // Default Cohort
            //CreateOrEditCohortDto _cohort = new CreateOrEditCohortDto()
            //{
            //    DefaultCohort = true,
            //    Description = "This is the default cohort. New users will be automatically assigned to this cohort.",
            //    Name = "Default Cohort",
            //    TenantDepartmentId = null
            //};
            //var cohort = ObjectMapper.Map<Cohort>(_cohort){

            //};
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(_tenantId))
                {

                    Cohort cohort = new Cohort()
                    {
                        TenantId = _tenantId,
                        DefaultCohort = true,
                        Description = "This is the default cohort. New users will be automatically assigned to this cohort.",
                        Name = "Default Cohort",
                    };

                    await _cohortRepository.InsertAsync(cohort);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    RecordCategoryRule recordCategoryRule = new RecordCategoryRule()
                    {
                        TenantId = _tenantId,
                        Description = "This is an example Rule.  If required is set to true, a user will not be complianct until the rule is completed and accepted.",
                        Name = "Example Rule",
                        ExpireInDays = 90,
                        Expires = true,
                        WarnDaysBeforeFinal = 30,
                        WarnDaysBeforeFirst = 15,
                        WarnDaysBeforeSecond = 2,
                        Notify = true,
                        Required = false
                    };
                    await _recordCategoryRuleRepository.InsertAsync(recordCategoryRule);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    RecordCategory recordCategory = new RecordCategory()
                    {
                        TenantId = _tenantId,
                        Name = "Example Category",
                        Instructions = "This is an example Requirement, that requires two steps to be compliant. "
                    };
                    await _recordCategoryRepository.InsertAsync(recordCategory);
                    await CurrentUnitOfWork.SaveChangesAsync();


                    RecordRequirement recordRequirement = new RecordRequirement()
                    {
                        TenantId = _tenantId,
                        Name = "Example Step 1",
                        Description = "This is an example of a requrement, number one of two.",
                        CohortId = cohort.Id,
                        Metadata = "{}"
                    };

                    await _recordRequirementRepository.InsertAsync(recordRequirement);
                    RecordRequirement recordRequirement2 = new RecordRequirement()
                    {
                        TenantId = _tenantId,
                        Name = "Example Step 2",
                        Description = "This is an example of a requrement, number two of two.",
                        CohortId = cohort.Id,
                        Metadata = "{}"
                    };

                    await _recordRequirementRepository.InsertAsync(recordRequirement2);
                    await CurrentUnitOfWork.SaveChangesAsync();


                    // Default library
                    var tenantDocumentCategory = new TenantDocumentCategory()
                    {
                        TenantId = _tenantId,
                        Name = "Example Library Folder",
                        Description = "This is an example document folder for storing documents of various categories. If admin only is checked, only admins can see these documents.",
                        UserId = _userId,
                        AuthorizedOnly = true,
                        HostOnly = false,
                    };
                    await _tenantDocumentCategoryRepository.InsertAsync(tenantDocumentCategory);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    // record statuses
                    //await EnsureSurpathServiceStatuses(_tenantId);
                    await _recordStatusesAppService.EnsureSurpathServiceStatuses(_tenantId);
                    await EnsureTenantPidTypes(_tenantId);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    uow.Complete();

                }
            }

        }
        private async Task<long> CreateAdminUserForTenantWizard(CreateOrUpdateUserInput input, int _tenantId)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(_tenantId))
                {
                    //if (AbpSession.TenantId.HasValue)
                    //{
                    //    await _userPolicy.CheckMaxUserCountAsync(AbpSession.GetTenantId());
                    //}

                    var user = ObjectMapper.Map<User>(input.User); //Passwords is not mapped (see mapping configuration)
                    user.TenantId = _tenantId; //  AbpSession.TenantId;

                    //Set password
                    if (input.SetRandomPassword)
                    {
                        var randomPassword = await _userManager.CreateRandomPassword();
                        user.Password = _passwordHasher.HashPassword(user, randomPassword);
                        input.User.Password = randomPassword;
                    }
                    else if (!input.User.Password.IsNullOrEmpty())
                    {
                        await UserManager.InitializeOptionsAsync(_tenantId);
                        foreach (var validator in _passwordValidators)
                        {
                            CheckErrors(await validator.ValidateAsync(UserManager, user, input.User.Password));
                        }

                        user.Password = _passwordHasher.HashPassword(user, input.User.Password);
                    }

                    user.ShouldChangePasswordOnNextLogin = input.User.ShouldChangePasswordOnNextLogin;

                    //Assign roles
                    user.Roles = new Collection<UserRole>();
                    foreach (var roleName in input.AssignedRoleNames)
                    {
                        var role = await _roleManager.GetRoleByNameAsync(roleName);
                        user.Roles.Add(new UserRole(_tenantId, user.Id, role.Id));
                    }

                    CheckErrors(await UserManager.CreateAsync(user));
                    await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.

                    //Notifications
                    await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
                    await _appNotifier.WelcomeToTheApplicationAsync(user);

                    //Organization Units
                    await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());

                    //Send activation email
                    if (input.SendActivationEmail)
                    {
                        user.SetNewEmailConfirmationCode();
                        await _userEmailer.SendEmailActivationLinkAsync(
                            user,
                            AppUrlService.CreateEmailActivationUrlFormat(_tenantId),
                            input.User.Password
                        );
                    }
                    uow.Complete();

                    return user.Id;
                }
            }

        }




        [AbpAuthorize(AppPermissions.Pages_Tenants_Edit)]
        public async Task<TenantEditDto> GetTenantForEdit(EntityDto input)
        {
            var tenantEditDto = ObjectMapper.Map<TenantEditDto>(await TenantManager.GetByIdAsync(input.Id));
            tenantEditDto.ConnectionString = SimpleStringCipher.Instance.Decrypt(tenantEditDto.ConnectionString);
            return tenantEditDto;
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_Edit)]
        public async Task UpdateTenant(TenantEditDto input)
        {
            await TenantManager.CheckEditionAsync(input.EditionId, input.IsInTrialPeriod);

            input.ConnectionString = SimpleStringCipher.Instance.Encrypt(input.ConnectionString);
            var tenant = await TenantManager.GetByIdAsync(input.Id);

            if (tenant.EditionId != input.EditionId)
            {
                await EventBus.TriggerAsync(new TenantEditionChangedEventData
                {
                    TenantId = input.Id,
                    OldEditionId = tenant.EditionId,
                    NewEditionId = input.EditionId
                });
            }

            ObjectMapper.Map(input, tenant);
            tenant.SubscriptionEndDateUtc = tenant.SubscriptionEndDateUtc?.ToUniversalTime();
            await TenantManager.UpdateAsync(tenant);
            await EnsureTenantFeaturesAndPidTypes(tenant.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_Delete)]
        public async Task DeleteTenant(EntityDto input)
        {
            var tenant = await TenantManager.GetByIdAsync(input.Id);
            await TenantManager.DeleteAsync(tenant);
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_ChangeFeatures)]
        public async Task<GetTenantFeaturesEditOutput> GetTenantFeaturesForEdit(EntityDto input)
        {
            var features = FeatureManager.GetAll()
                .Where(f => f.Scope.HasFlag(FeatureScopes.Tenant));
            var featureValues = await TenantManager.GetFeatureValuesAsync(input.Id);

            return new GetTenantFeaturesEditOutput
            {
                Features = ObjectMapper.Map<List<FlatFeatureDto>>(features).OrderBy(f => f.DisplayName).ToList(),
                FeatureValues = featureValues.Select(fv => new NameValueDto(fv)).ToList()
            };
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_ChangeFeatures)]
        public async Task UpdateTenantFeatures(UpdateTenantFeaturesInput input)
        {
            await TenantManager.SetFeatureValuesAsync(input.Id, input.FeatureValues.Select(fv => new NameValue(fv.Name, fv.Value)).ToArray());
            CurrentUnitOfWork.SaveChanges();
            await EnsureTenantFeaturesSurpathSettings(input.Id, input);
            //await EnsureSurpathServiceStatuses(input.Id);
            await _recordStatusesAppService.EnsureSurpathServiceStatuses(input.Id);

            await EnsureTenantPidTypes(input.Id);
            CurrentUnitOfWork.SaveChanges();

        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_ChangeFeatures)]
        public async Task ResetTenantSpecificFeatures(EntityDto input)
        {
            await TenantManager.ResetAllFeaturesAsync(input.Id);
        }

        public async Task UnlockTenantAdmin(EntityDto input)
        {
            using (CurrentUnitOfWork.SetTenantId(input.Id))
            {
                var tenantAdmin = await UserManager.GetAdminAsync();
                if (tenantAdmin != null)
                {
                    tenantAdmin.Unlock();
                }
            }
        }


        private async Task EnsureTenantFeaturesSurpathSettings(int tenantId, UpdateTenantFeaturesInput input)
        {
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            //var tenantFeatures = input.FeatureValues.Select(fv => new NameValue(fv.Name, fv.Value)).ToArray();
            var tenantFeatureNames = input.FeatureValues.Where(fv => fv.Value.Equals("true", StringComparison.CurrentCultureIgnoreCase)).Select(fv => fv.Name).ToList();
            var thisTenant = TenantManager.Tenants.AsNoTracking().IgnoreQueryFilters().Where(t => t.Id == tenantId).FirstOrDefault();
            var allFeatures = FeatureManager.GetAll()
                .Where(f => f.Scope.HasFlag(FeatureScopes.Tenant));

            var allFeatureNames = allFeatures.Select(f => f.Name).ToList();

            var allSurpathServices = _surpathServiceRepository.GetAll().ToList();
            // var availableSurpathServicesToTenant = _surpathServiceRepository.GetAll().Where(s => tenantFeatureNames.Contains(s.FeatureIdentifier)).ToList();
            var availableSurpathServicesToTenant = allSurpathServices.Where(s => tenantFeatureNames.Contains(s.FeatureIdentifier)).ToList();


            var availableSurpathServicesToTenantIds = availableSurpathServicesToTenant.Select(s => s.Id).ToList();
            var allSurpathServicesIds = allSurpathServices.Select(s => s.Id).ToList();
            //var tenantServiceIds = surpathServices.Where(s=> tenantFeatureNames.Contains(s.FeatureIdentifier)).Select(s => s.Id).ToList();




            var tenantSurpathServices = _tenantSurpathServiceRepository.GetAll().Where(t => t.TenantId == tenantId).ToList();

            // find all surpathServices where there is not a coresponding tenantSurpathService, and if any, create a tenantSurpathService

            var tenantSurpathServicesSurpathServiceIds = tenantSurpathServices
                .Where(s => s.TenantId == tenantId)
                .Where(s => s.SurpathServiceId != null)
                .Where(s => s.SurpathServiceId != Guid.Empty)
                .Select(s => (Guid)s.SurpathServiceId).ToList();

            // all available services except one that already exist
            var missingSurpathServicesIds = availableSurpathServicesToTenantIds.Except(tenantSurpathServicesSurpathServiceIds).ToList();
            // all services that are not available to the tenant
            var removedSurpathServicesIds = allSurpathServicesIds.Except(availableSurpathServicesToTenantIds).ToList();

            var tenantSurpathServicesToEnableIds = tenantSurpathServices.Where(ts => availableSurpathServicesToTenantIds.Contains((Guid)ts.SurpathServiceId) && ts.IsPricingOverrideEnabled == false).Select(ts => ts.Id).ToList();

            var tenantSurpathServiceRecordRequirements = _recordRequirementRepository.GetAll().Where(r => r.TenantId == tenantId && r.IsSurpathOnly == true).ToList();
            var tenantSurpathServiceRecordRequirementsSurpathServiceIds = tenantSurpathServiceRecordRequirements.Select(r => r.SurpathServiceId).ToList();

            foreach (var serviceId in missingSurpathServicesIds)
            {
                // We need to create the surpath service and requirements - using the ss and it's rule as the template. User can edit as needed
                var surpathService = availableSurpathServicesToTenant.Where(s => s.Id == serviceId).FirstOrDefault();
                if (surpathService != null)
                {
                    var tenantSurpathService = new TenantSurpathService()
                    {
                        RecordCategoryRuleId = surpathService.RecordCategoryRuleId,
                        IsPricingOverrideEnabled = surpathService.IsEnabledByDefault,
                        SurpathServiceId = serviceId,
                        TenantId = tenantId,
                        Price = surpathService.Price,
                        Name = $"{surpathService.Name} ({thisTenant.TenancyName})",
                        Description = $"{surpathService.Name} - {thisTenant.Name}",
                    };

                    await _tenantSurpathServiceRepository.InsertAsync(tenantSurpathService);
                    //await CurrentUnitOfWork.SaveChangesAsync();
                    tenantSurpathServicesSurpathServiceIds.Add(tenantSurpathService.Id);
                    // Create if we don't have already 
                    if (!tenantSurpathServiceRecordRequirementsSurpathServiceIds.Contains(tenantSurpathService.Id))
                    {
                        var surpathServiceRecordRequirement = new RecordRequirement()
                        {
                            TenantId = tenantId,
                            SurpathServiceId = serviceId,
                            TenantSurpathServiceId = tenantSurpathService.Id,
                            CohortId = tenantSurpathService.CohortId,
                            TenantDepartmentId = tenantSurpathService.TenantDepartmentId,
                            Description = tenantSurpathService.Description,
                            IsSurpathOnly = true,
                            Name = tenantSurpathService.Name,
                        };
                        await _recordRequirementRepository.InsertAsync(surpathServiceRecordRequirement);
                        //await CurrentUnitOfWork.SaveChangesAsync();

                        // Requirements need a category
                        var surpathServiceRecordRequirementCategory = new RecordCategory()
                        {
                            TenantId = tenantId,
                            Instructions = "This is an internal surpath service requirement",
                            RecordCategoryRuleId = tenantSurpathService.RecordCategoryRuleId,
                            Name = $"{tenantSurpathService.Name} Category",
                            RecordRequirementId = surpathServiceRecordRequirement.Id,
                        };
                        await _recordCategoryRepository.InsertAsync(surpathServiceRecordRequirementCategory);
                    }
                }
            }
            CurrentUnitOfWork.SaveChanges();

            foreach (var serviceId in removedSurpathServicesIds)
            {
                var _tss = tenantSurpathServices.Where(t => t.SurpathServiceId == serviceId).FirstOrDefault();
                if (_tss != null)
                {
                    var tss = _tenantSurpathServiceRepository.Get(_tss.Id);
                    if (tss != null)
                    {
                        tss.IsPricingOverrideEnabled = false;
                        await _tenantSurpathServiceRepository.UpdateAsync(tss);
                        tenantSurpathServicesSurpathServiceIds.Remove(tss.Id);
                    }
                }
            }
            CurrentUnitOfWork.SaveChanges();

            // make sure all rules are setup.?

            // var tenantSurpathServicesNow = _tenantSurpathServiceRepository.GetAll().Where(t => t.TenantId == tenantId).ToList();



            //foreach (var tenantServiceId in tenantSurpathServicesSurpathServiceIds)
            foreach (var tenantServiceId in tenantSurpathServicesToEnableIds)
            {
                var tss = _tenantSurpathServiceRepository.Get(tenantServiceId);
                if (tss != null)
                {
                    tss.IsPricingOverrideEnabled = true;
                    await _tenantSurpathServiceRepository.UpdateAsync(tss);
                }
                if (!tenantSurpathServiceRecordRequirementsSurpathServiceIds.Contains(tenantServiceId))
                {
                    var surpathServiceRecordRequirement = new RecordRequirement()
                    {
                        TenantId = tenantId,
                        SurpathServiceId = tss.SurpathServiceId,
                        TenantSurpathServiceId = tenantServiceId,
                        CohortId = tss.CohortId,
                        TenantDepartmentId = tss.TenantDepartmentId,
                        Description = tss.Description,
                        IsSurpathOnly = true,
                        Name = tss.Name
                    };
                    await _recordRequirementRepository.InsertAsync(surpathServiceRecordRequirement);
                    //await CurrentUnitOfWork.SaveChangesAsync();

                    // Requirements need a category
                    var surpathServiceRecordRequirementCategory = new RecordCategory()
                    {
                        TenantId = tenantId,
                        Instructions = "This is an internal surpath service requirement",
                        RecordCategoryRuleId = tss.RecordCategoryRuleId,
                        Name = $"{tss.Name} Category",
                        RecordRequirementId = surpathServiceRecordRequirement.Id

                    };
                    await _recordCategoryRepository.InsertAsync(surpathServiceRecordRequirementCategory);
                }

            }
            CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);

        }



        private async Task EnsureTenantPidTypes(int tenantId)
        {

            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);


            if (!_pidTypeRepository.GetAll().IgnoreQueryFilters().Any(r => r.TenantId == tenantId))
            {
                var _HostPidTypes = _pidTypeRepository.GetAll().AsNoTracking().IgnoreQueryFilters().Where(p => p.TenantId == null).ToList();

                _HostPidTypes.ForEach(p =>
                {
                    _pidTypeRepository.InsertOrUpdate(new PidType()
                    {
                        Name = p.Name,
                        Description = p.Description,
                        CreatedBy = 1,
                        CreatedOn = DateTime.UtcNow,
                        MaskPid = p.MaskPid,
                        PidRegex = p.PidRegex,
                        IsActive = p.IsActive,
                        TenantId = tenantId,
                        PidInputMask = p.PidInputMask,
                        Required = p.Required,
                    });
                });
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_ImportUsers)]
        public async Task<FileDto> GetTenantUsersToExcel(EntityDto input)
        {
            var tenantId = input.Id;

            // Disable tenant filter if we're in host context querying tenant data
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            // Get users with Roles included
            var users = await _userRepository.GetAll()
                .Where(u => u.TenantId == tenantId)
                .Include(u => u.Roles)
                .ToListAsync();

            var userIds = users.Select(u => u.Id).ToList();

            // Bulk load all department associations
            var departmentUsers = await _departmentUserRepository.GetAll()
                .Where(du => du.UserId.HasValue && userIds.Contains(du.UserId.Value))
                .ToListAsync();

            var departmentIds = departmentUsers
                .Where(du => du.TenantDepartmentId.HasValue)
                .Select(du => du.TenantDepartmentId.Value)
                .Distinct()
                .ToList();

            var departments = await _tenantDepartmentRepository.GetAll()
                .Where(d => departmentIds.Contains(d.Id))
                .ToListAsync();

            var departmentDict = departments.ToDictionary(d => d.Id, d => d.Name);
            var userDepartmentDict = departmentUsers
                .Where(du => du.UserId.HasValue && du.TenantDepartmentId.HasValue)
                .ToDictionary(du => du.UserId.Value, du => du.TenantDepartmentId.Value);

            // Bulk load all cohort associations
            var cohortUsers = await _cohortUserRepository.GetAll()
                .Where(cu => userIds.Contains(cu.UserId))
                .ToListAsync();

            var cohortIds = cohortUsers
                .Where(cu => cu.CohortId.HasValue)
                .Select(cu => cu.CohortId.Value)
                .Distinct()
                .ToList();

            var cohorts = await _cohortRepository.GetAll()
                .Where(c => cohortIds.Contains(c.Id))
                .ToListAsync();

            var cohortDict = cohorts.ToDictionary(c => c.Id, c => c.Name);
            var cohortDepartmentDict = cohorts
                .Where(c => c.TenantDepartmentId.HasValue)
                .ToDictionary(c => c.Id, c => c.TenantDepartmentId.Value);
            var userCohortDict = cohortUsers
                .Where(cu => cu.CohortId.HasValue)
                .ToDictionary(cu => cu.UserId, cu => cu.CohortId.Value);

            // Also load departments referenced by cohorts (for inferred department membership)
            var cohortDepartmentIds = cohorts
                .Where(c => c.TenantDepartmentId.HasValue)
                .Select(c => c.TenantDepartmentId.Value)
                .Distinct()
                .ToList();

            // Add cohort departments to the list if not already loaded
            var additionalDepartmentIds = cohortDepartmentIds.Except(departmentIds).ToList();
            if (additionalDepartmentIds.Any())
            {
                var additionalDepartments = await _tenantDepartmentRepository.GetAll()
                    .Where(d => additionalDepartmentIds.Contains(d.Id))
                    .ToListAsync();

                foreach (var dept in additionalDepartments)
                {
                    if (!departmentDict.ContainsKey(dept.Id))
                    {
                        departmentDict[dept.Id] = dept.Name;
                    }
                }
            }

            // Build results with department inference from cohort (matching import logic)
            var results = users.Select(user =>
            {
                string departmentName = null;
                string cohortName = null;
                Guid? effectiveDepartmentId = null;

                // Get cohort name
                if (userCohortDict.ContainsKey(user.Id))
                {
                    var cohortId = userCohortDict[user.Id];
                    cohortName = cohortDict.ContainsKey(cohortId) ? cohortDict[cohortId] : null;
                }

                // Determine department:
                // 1. Direct DepartmentUser record (if exists)
                if (userDepartmentDict.ContainsKey(user.Id))
                {
                    effectiveDepartmentId = userDepartmentDict[user.Id];
                }
                // 2. Inferred from cohort (if user in cohort but no direct department)
                else if (userCohortDict.ContainsKey(user.Id))
                {
                    var cohortId = userCohortDict[user.Id];
                    if (cohortDepartmentDict.ContainsKey(cohortId))
                    {
                        effectiveDepartmentId = cohortDepartmentDict[cohortId];
                    }
                }

                // Get department name from effective ID
                if (effectiveDepartmentId.HasValue && departmentDict.ContainsKey(effectiveDepartmentId.Value))
                {
                    departmentName = departmentDict[effectiveDepartmentId.Value];
                }

                return new
                {
                    User = user,
                    DepartmentName = departmentName,
                    CohortName = cohortName
                };
            }).ToList();

            // Get SSN PidType and bulk load SSNs
            var ssnPidType = await _pidTypeRepository.FirstOrDefaultAsync(
                pt => pt.TenantId == tenantId && pt.Name.ToLower() == "ssn"
            );

            var userPids = ssnPidType != null
                ? await _userPidRepository.GetAll()
                    .Where(up => userIds.Contains(up.UserId.Value) && up.PidTypeId == ssnPidType.Id)
                    .ToListAsync()
                : new List<UserPid>();
            var ssnDict = userPids.ToDictionary(up => up.UserId.Value, up => up.Pid);

            // Get all role IDs and load roles in one query (Roles is now included, so this won't be null)
            var roleIds = results
                .Where(r => r.User.Roles != null)
                .SelectMany(r => r.User.Roles.Select(ur => ur.RoleId))
                .Distinct()
                .ToList();
            var roles = await _roleManager.Roles
                .Where(r => roleIds.Contains(r.Id))
                .ToListAsync();
            var roleDict = roles.ToDictionary(r => r.Id, r => r.DisplayName);

            // Build export list
            var exportUsers = results.Select(r =>
            {
                var user = r.User;
                var roleDisplayNames = (user.Roles ?? new List<UserRole>())
                    .Select(ur => roleDict.ContainsKey(ur.RoleId) ? roleDict[ur.RoleId] : null)
                    .Where(name => name != null)
                    .Distinct()
                    .ToList();

                return new TenantUserExportDto
                {
                    UserName = user.UserName,
                    Name = user.Name,
                    Surname = user.Surname,
                    EmailAddress = user.EmailAddress,
                    PhoneNumber = user.PhoneNumber,
                    Password = string.Empty,
                    AssignedRoleNames = string.Join(",", roleDisplayNames),
                    DepartmentName = r.DepartmentName,
                    CohortName = r.CohortName,
                    Address = user.Address,
                    SuiteApt = user.SuiteApt,
                    City = user.City,
                    State = user.State,
                    Zip = user.Zip,
                    DateOfBirth = user.DateOfBirth != default(DateTime) ? user.DateOfBirth.ToString("MM/dd/yyyy") : string.Empty,
                    SSN = ssnDict.ContainsKey(user.Id) ? ssnDict[user.Id] : null
                };
            }).ToList();

            return _tenantUserExporter.ExportToFile(exportUsers);
        }

    }
}
