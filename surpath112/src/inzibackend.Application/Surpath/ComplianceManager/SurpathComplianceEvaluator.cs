using Abp.Application.Features;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using inzibackend.Authorization.Users;
using inzibackend.MultiTenancy;
using inzibackend.Surpath.Compliance;
using inzibackend.Surpath.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using inzibackend.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace inzibackend.Surpath.ComplianceManager
{
    public class SurpathComplianceEvaluator : inzibackendAppServiceBase, ISurpathComplianceEvaluator
    {
        private readonly IFeatureChecker _featureChecker;

        private readonly ILogger<SurpathComplianceEvaluator> _logger;
        private readonly IRepository<UserPid, Guid> _UserPidRepository;
        private readonly IRepository<TenantDepartment, Guid> _TenantDepartmentRepository;
        private readonly IRepository<TenantDepartmentUser, Guid> _TenantDepartmentUserRepository;
        private readonly IRepository<Cohort, Guid> _CohortRepository;
        private readonly IRepository<CohortUser, Guid> _CohortUserRepository;
        private readonly IRepository<RecordRequirement, Guid> _RecordRequirementRepository;
        private readonly IRepository<RecordCategory, Guid> _RecordCategoryRepository;
        private readonly IRepository<RecordCategoryRule, Guid> _RecordCategoryRuleRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<RecordState, Guid> _recordStateRepository;
        private readonly IRepository<RecordStatus, Guid> _recordStatusRepository;
        private readonly IRepository<Record, Guid> _recordRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IPermissionChecker _permissionChecker;
        private readonly IRepository<SurpathService, Guid> _surpathServiceRepository;
        private readonly IRepository<TenantSurpathService, Guid> _tenantSurpathServiceRepository;

        //private UserManager UserManager { get; set; }
        private readonly UserManager _userManager;

        private readonly IRepository<User, long> _lookup_userRepository;

        public SurpathComplianceEvaluator(
            ILogger<SurpathComplianceEvaluator> logger,
            IRepository<UserPid, Guid> UserPidRepository,
            IRepository<TenantDepartment, Guid> TenantDepartmentRepository,
            IRepository<TenantDepartmentUser, Guid> TenantDepartmentUserRepository,
            IRepository<Cohort, Guid> CohortRepository,
            IRepository<CohortUser, Guid> CohortUserRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<Tenant> tenantRepository,
            IRepository<RecordRequirement, Guid> RecordRequirementRepository,
            IRepository<RecordCategory, Guid> RecordCategoryRepository,
            IRepository<RecordCategoryRule, Guid> RecordCategoryRuleRepository,
            IRepository<RecordState, Guid> recordStateRepository,
            IRepository<RecordStatus, Guid> recordStatusRepository,
            IRepository<Record, Guid> recordRepository,
            IRepository<SurpathService, Guid> surpathServiceRepository,
            IRepository<TenantSurpathService, Guid> tenantSurpathServiceRepository,
            IFeatureChecker featureChecker,
            IPermissionChecker permissionChecker,
            UserManager userManager,
            IRepository<User, long> lookup_userRepository
            )
        {
            _logger = logger;
            _UserPidRepository = UserPidRepository;
            _TenantDepartmentRepository = TenantDepartmentRepository;
            _TenantDepartmentUserRepository = TenantDepartmentUserRepository;
            _CohortRepository = CohortRepository;
            _CohortUserRepository = CohortUserRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _tenantRepository = tenantRepository;
            _RecordRequirementRepository = RecordRequirementRepository;
            _RecordCategoryRepository = RecordCategoryRepository;
            _RecordCategoryRuleRepository = RecordCategoryRuleRepository;
            _featureChecker = featureChecker;
            _userManager = userManager;
            _recordStateRepository = recordStateRepository;
            _recordRepository = recordRepository;
            _permissionChecker = permissionChecker;
            _recordStatusRepository = recordStatusRepository;
            _lookup_userRepository = lookup_userRepository;
            _surpathServiceRepository = surpathServiceRepository;
            _tenantSurpathServiceRepository = tenantSurpathServiceRepository;
        }

        public async Task<ComplianceValues> GetComplianceValuesForCohortUser(Guid cohortuserid)
        {
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var _cohortuser = await _CohortUserRepository.GetAll().AsNoTracking().Where(x => x.Id == cohortuserid).FirstOrDefaultAsync();

            if (_cohortuser == null)
            {
                throw new UserFriendlyException("Cohort User not found");
            }

            var _userid = _cohortuser.UserId;
            var _tenantid = _cohortuser.TenantId;
            return await GetComplianceValuesForUser((int)_tenantid, _userid);
        }

        public async Task<ComplianceValues> GetComplianceValuesForUser(int _tenantId, long _userId)
        {
            var _complianceInfo = await GetComplianceInfo(_tenantId, _userId);

            // next, get all the records for the user, along with record categories and record category rules and record states and record statuses
            var _userRecords = await GetUserRecordSecureStates((int)_tenantId, _userId);

            // Create our return value:
            var _cv = new ComplianceValues()
            {
                Background = false,
                Drug = false,
                Immunization = false,
                InCompliance = false,
                UserId = _userId,
                TenantId = (int)_tenantId
            };

            var _AllRequiredCategoryIDs = new List<Guid>();

            // Extract requirement IDs from user's existing records for service matching
            var userRecordRequirementIds = _userRecords
                .Where(r => r.RecordRequirementId.HasValue)
                .Select(r => r.RecordRequirementId.Value)
                .Distinct()
                .ToList();

            // Drug Test
            // Identify the drug test requirement - now considering user's existing records
            var _drugTestRequirement = SurpathOnlyRequirements.GetSurpathRequirementFromComplianceInfo(_complianceInfo, AppFeatures.SurpathFeatureDrugTest, userRecordRequirementIds);

            // if there is a drug test requiremet
            if (_drugTestRequirement != null)
            {
                _AllRequiredCategoryIDs.Add(_drugTestRequirement.RecordCategory.Id);
                // Identify the drug test category
                var _drugTestCategory = _userRecords.Where(_r => _r.RecordRequirementId == _drugTestRequirement.RecordRequirement.Id && _r.RecordRequirementIsSurpathOnly == true).FirstOrDefault();
                // if the status of the record is compliant, then the user is compliant
                if (_drugTestCategory != null)
                {
                    //_cv.Drug = _drugTestCategory.RecordStatus.ComplianceImpact == EnumStatusComplianceImpact.Compliant;
                    _cv.Drug = _drugTestCategory.RecordStatusComplianceImpact == EnumStatusComplianceImpact.Compliant;
                }
            }

            // Background Check
            // Identify the background check requirement - now considering user's existing records
            var _backgroundCheckRequirement = SurpathOnlyRequirements.GetSurpathRequirementFromComplianceInfo(_complianceInfo, AppFeatures.SurpathFeatureBackgroundCheck, userRecordRequirementIds);
            // if there is a background check requirement
            if (_backgroundCheckRequirement != null)
            {
                _AllRequiredCategoryIDs.Add(_backgroundCheckRequirement.RecordCategory.Id);

                // identify the background check category
                var _backgroundTestCategory = _userRecords.Where(_r => _r.RecordRequirementId == _backgroundCheckRequirement.RecordRequirement.Id && _r.RecordRequirementIsSurpathOnly == true).FirstOrDefault();
                // if the status of the record is compliant, then the user is compliant
                if (_backgroundTestCategory != null)
                {
                    //_cv.Background = _backgroundTestCategory.RecordStatus.ComplianceImpact == EnumStatusComplianceImpact.Compliant;
                    _cv.Background = _backgroundTestCategory.RecordStatusComplianceImpact == EnumStatusComplianceImpact.Compliant;
                }
            }

            var _nonSurpathOnlyRequiredCategoryIDs = _complianceInfo.NonSurpathOnlyRequirements.Where(_r => _r.RecordCategoryRule != null && _r.RecordCategoryRule.Required == true).Select(r => r.RecordCategory.Id).ToList();
            var _compliantUserCategoryIds = _userRecords.Where(_r => _r.RecordStatusComplianceImpact == EnumStatusComplianceImpact.Compliant && _r.RecordRequirementIsSurpathOnly == false).Select(r => (Guid)r.RecordCategoryId).ToList();
            // Immunization
            // If there are any non-surpath requirements required by rule that are not compliant, then the user is not compliant
            _cv.Immunization = !_nonSurpathOnlyRequiredCategoryIDs.Except(_compliantUserCategoryIds).Any();

            // InCompliance
            // Determine the overall compliance of the user for all requirements that are not surepath only but required by rule, and the surpath only requirements
            //_cv.InCompliance = _userRecords.Where(_r => _r.RecordRequirement.IsSurpathOnly == false && _r.RecordCategoryRule.Required == true && _r.RecordStatus.ComplianceImpact != EnumStatusComplianceImpact.Compliant).Count() == 0
            //    && _userRecords.Where(_r => _r.RecordRequirement.IsSurpathOnly == true && _r.RecordStatus.ComplianceImpact != EnumStatusComplianceImpact.Compliant).Count() == 0;

            var _AllRequiredCategoryIDsNonSurpath = _complianceInfo.NonSurpathOnlyRequirements.Where(_r => _r.RecordCategoryRule != null && _r.RecordCategoryRule.Required == true).Select(r => (Guid)r.RecordCategory.Id).ToList();

            _AllRequiredCategoryIDs = _AllRequiredCategoryIDs.Concat(_AllRequiredCategoryIDsNonSurpath).ToList();

            var _AllUserCompliantCategoryIDs = _userRecords.Where(_r => _r.RecordStatusComplianceImpact == EnumStatusComplianceImpact.Compliant).Select(r => (Guid)r.RecordCategoryId).ToList();

            // var _incompliance = _nonSurpathOnlyRequirementsRequiredIds.Where(_r => !_compliantUserRecordsRequirementIds.Contains(_r)).Count() > 0;
            var _overallCompliance = !_AllRequiredCategoryIDs.Except(_AllUserCompliantCategoryIDs).Any();
            _cv.InCompliance = _cv.Drug == true && _cv.Background == true && _cv.Immunization == true && _overallCompliance == true;
            // lastly, return compliance values
            return _cv;
        }

        /// <summary>
        /// Retrieves the compliance states for a specific user, including their record states, categories, and requirements.
        /// This function aggregates all compliance-related information for displaying a user's compliance status.
        /// </summary>
        /// <param name="_userId">The ID of the user to get compliance states for</param>
        /// <returns>A list of GetRecordStateCompliancetForViewDto containing detailed compliance information</returns>
        /// <exception cref="UserFriendlyException">Thrown when the specified user is not found</exception>
        public async Task<List<GetRecordStateCompliancetForViewDto>> GetComplianceStatesForUser(long _userId)
        {
            // Get secure user information including tenant context
            var _userSecure = await GetUserSecure(_userId);
            if (_userSecure == null) throw new UserFriendlyException("User not found");

            var _tenantId = (int)_userSecure.TenantId;

            // Log for debugging
            Logger.Debug($"GetComplianceStatesForUser called for userId: {_userId}, tenantId: {_tenantId}, AbpSession.TenantId: {AbpSession.TenantId}");

            // Get all compliance information for the user including requirements, rules, and categories
            var _complianceInfo = await GetComplianceInfo(_tenantId, _userId);

            // Retrieve all record statuses for the user including their associated categories and rules
            var _userRecords = await GetUserRecordStatuses((int)_tenantId, _userId);

            // Initialize collections to store the results
            var _RecordStatesForViewList = new List<GetRecordStateForViewDto>();
            var results = new List<GetRecordStateCompliancetForViewDto>();

            // Track the last requirement name to handle grouping/organization
            var LastRequirementName = string.Empty;

            // Iterate through each requirement assigned to the user
            foreach (var _requirement in _complianceInfo.RequirementsForUser)
            {
                // Get the associated category rule for this requirement
                var _rule = _complianceInfo.RecordCategoryRules.Where(_r => _r.Id == _requirement.RecordCategory.RecordCategoryRuleId).FirstOrDefault();

                // Initialize the view model for the record state
                var _GetRecordStateForViewDto = new GetRecordStateForViewDto();

                // Find the user's record that matches this requirement and category
                var _thisUserRecord = _userRecords.Where(_r =>
                    _r.RecordRequirement.Id == _requirement.RecordRequirement.Id &&
                    _r.RecordCategory.Id == _requirement.RecordCategory.Id).FirstOrDefault();

                // If a matching record exists, populate the view model with its details
                if (_thisUserRecord != null)
                {
                    // Ensure all required objects exist to prevent null reference exceptions
                    _thisUserRecord.RecordCategory = _thisUserRecord.RecordCategory == null ? new RecordCategory() : _thisUserRecord.RecordCategory;
                    _thisUserRecord.RecordState = _thisUserRecord.RecordState == null ? new RecordState() : _thisUserRecord.RecordState;
                    _thisUserRecord.Record = _thisUserRecord.Record == null ? new Record() : _thisUserRecord.Record;
                    _thisUserRecord.RecordStatus = _thisUserRecord.RecordStatus == null ? new RecordStatus() : _thisUserRecord.RecordStatus;

                    // Map the record state information to the DTO
                    _GetRecordStateForViewDto.RecordState = new RecordStateDto
                    {
                        State = _thisUserRecord.RecordState.State,
                        Notes = _thisUserRecord.RecordState.Notes,
                        Id = _thisUserRecord.RecordState.Id,
                        RecordCategoryId = _thisUserRecord.RecordState.RecordCategoryId,
                        RecordId = _thisUserRecord.RecordState.RecordId,
                        RecordStatusId = _thisUserRecord.RecordState.RecordStatusId,
                        UserId = _userId,
                        RecordDto = new RecordDto()
                        {
                            BinaryObjId = _thisUserRecord.Record.BinaryObjId,
                            TenantDocumentCategoryId = _thisUserRecord.Record.TenantDocumentCategoryId
                        }
                    };

                    // Map the record status information to the DTO
                    _GetRecordStateForViewDto.RecordStatus = new RecordStatusDto
                    {
                        Id = _thisUserRecord.RecordStatus.Id,
                        StatusName = _thisUserRecord.RecordStatus.StatusName,
                        HtmlColor = _thisUserRecord.RecordStatus.HtmlColor,
                        CSSCLass = _thisUserRecord.RecordStatus.CSSCLass,
                        ComplianceImpact = _thisUserRecord.RecordStatus.ComplianceImpact,
                        IsSurpathServiceStatus = _thisUserRecord.RecordStatus.IsSurpathServiceStatus,
                        IsDefault = _thisUserRecord.RecordStatus.IsDefault,
                    };

                    // Set additional user and record information
                    _GetRecordStateForViewDto.Recordfilename = _thisUserRecord.Record.filename;
                    _GetRecordStateForViewDto.RecordCategoryName = _requirement.RecordCategory.Name;
                    _GetRecordStateForViewDto.UserName = _userSecure.FullName;
                    _GetRecordStateForViewDto.FullName = _userSecure.FullName;
                    _GetRecordStateForViewDto.UserId = _userId;
                    _GetRecordStateForViewDto.RecordStatusStatusName = _thisUserRecord.RecordStatus.StatusName;
                }

                // If no record status exists, create a default non-compliant status
                if (_GetRecordStateForViewDto.RecordStatus == null)
                {
                    _GetRecordStateForViewDto.RecordStatus = new RecordStatusDto()
                    {
                        ComplianceImpact = EnumStatusComplianceImpact.NotCompliant,
                        IsSurpathServiceStatus = _requirement.RecordRequirement.IsSurpathOnly,
                    };
                }

                // Get associated Surpath service information if available
                var _surpathService = _complianceInfo.SurpathServices.Where(_s => _s.Id == _requirement.RecordRequirement.SurpathServiceId).FirstOrDefault();
                var _tenantSurpathService = _complianceInfo.TenantSurpathServices.Where(_s => _s.Id == _requirement.RecordRequirement.TenantSurpathServiceId).FirstOrDefault();

                // If no SurpathService is directly linked, try to find one based on requirement's associated services
                if (_surpathService == null && _requirement.RecordRequirement.IsSurpathOnly)
                {
                    // Check if we can determine the service type from the TenantSurpathService
                    if (_tenantSurpathService != null && _tenantSurpathService.SurpathServiceId.HasValue)
                    {
                        _surpathService = _complianceInfo.SurpathServices.FirstOrDefault(s =>
                            s.Id == _tenantSurpathService.SurpathServiceId.Value);
                    }

                    // If still not found, check by the SurpathServiceId directly
                    if (_surpathService == null && _requirement.RecordRequirement.SurpathServiceId.HasValue)
                    {
                        _surpathService = _complianceInfo.SurpathServices.FirstOrDefault(s =>
                            s.Id == _requirement.RecordRequirement.SurpathServiceId.Value);
                    }
                }

                // Create the final compliance state view model
                var res = new GetRecordStateCompliancetForViewDto()
                {
                    IsChildRow = true,  // Indicates this is a child record in the UI hierarchy
                    RecCount = 1,       // Indicates the number of records in this group
                    RecordCategory = ObjectMapper.Map<RecordCategoryDto>(_requirement.RecordCategory), // Map the record category information
                    RecordRequirement = ObjectMapper.Map<RecordRequirementDto>(_requirement.RecordRequirement), // Map the record requirement information
                    GetRecordStateForViewDto = _GetRecordStateForViewDto, // Map the record state information
                    // Map Surpath service information if available
                    SurpathService = _surpathService == null ? null : new SurpathServiceDto()
                    {
                        Id = _surpathService.Id,
                        FeatureIdentifier = _surpathService.FeatureIdentifier
                    },
                    TenantSurpathServiceDto = _tenantSurpathService == null ? null : ObjectMapper.Map<TenantSurpathServiceDto>(_tenantSurpathService) // Map the tenant Surpath service information
                };

                // If a rule exists for this category, map its properties
                if (_rule != null)
                {
                    res.RecordCategory.RecordCategoryRule = new RecordCategoryRuleDto()
                    {
                        Required = _rule.Required,
                        Name = _rule.Name,
                        Description = _rule.Description,
                        Id = _rule.Id,
                        Notify = _rule.Notify,
                        Expires = _rule.Expires,
                        ExpireInDays = _rule.ExpireInDays,
                        IsSurpathOnly = _rule.IsSurpathOnly,
                        WarnDaysBeforeFinal = _rule.WarnDaysBeforeFinal,
                        WarnDaysBeforeFirst = _rule.WarnDaysBeforeFirst,
                        WarnDaysBeforeSecond = _rule.WarnDaysBeforeSecond,
                    };
                }

                // Add the result to the collection and update the last requirement name
                results.Add(res);
                LastRequirementName = _requirement.RecordRequirement.Name;
            }
            ;

            // Return distinct results to avoid duplicates
            return results.Distinct().ToList();
        }

        /// <summary>
        /// Gets detailed compliance values for a user by evaluating the full compliance state.
        /// This provides more accurate compliance evaluation than GetComplianceValuesForUser
        /// while still returning the compact ComplianceValues format.
        /// </summary>
        /// <param name="_userId">The ID of the user to evaluate</param>
        /// <returns>ComplianceValues object containing the evaluated compliance state</returns>
        public async Task<ComplianceValues> GetDetailedComplianceValuesForUser(long _userId)
        {
            // Get secure user information including tenant context
            var _userSecure = await GetUserSecure(_userId);
            if (_userSecure == null) throw new UserFriendlyException("User not found");

            var _tenantId = (int)_userSecure.TenantId;

            // Get all compliance information for the user including requirements, rules, and categories
            var _complianceInfo = await GetComplianceInfo(_tenantId, _userId);

            // Retrieve all record statuses for the user including their associated categories and rules
            var _userRecords = await GetUserRecordStatuses((int)_tenantId, _userId);

            // Initialize the compliance values object
            var _cv = new ComplianceValues()
            {
                Background = false,
                Drug = false,
                Immunization = false,
                InCompliance = false,
                UserId = _userId,
                TenantId = (int)_tenantId
            };

            // Track required categories and compliant categories
            var _AllRequiredCategoryIDs = new List<Guid>();
            var _compliantUserCategoryIds = _userRecords
                .Where(_r => _r.RecordStatus.ComplianceImpact == EnumStatusComplianceImpact.Compliant)
                .Select(r => r.RecordCategory.Id)
                .ToList();

            // Evaluate each requirement type (restored from working usertransfer logic)
            foreach (var requirement in _complianceInfo.RequirementsForUser)
            {
                var _rule = _complianceInfo.RecordCategoryRules
                    .FirstOrDefault(_r => _r.Id == requirement.RecordCategory.RecordCategoryRuleId);

                if (_rule?.Required == true)
                {
                    _AllRequiredCategoryIDs.Add(requirement.RecordCategory.Id);
                }

                // Find user's record for this requirement (immediate one-to-one evaluation)
                var _userRecord = _userRecords.FirstOrDefault(_r =>
                    _r.RecordRequirement.Id == requirement.RecordRequirement.Id &&
                    _r.RecordCategory.Id == requirement.RecordCategory.Id);
                if (_userRecord == null) continue;
                
                // Evaluate specific requirement types (immediate evaluation - no collection)
                if (requirement.RecordRequirement.IsSurpathOnly)
                {
                    // Get the FeatureIdentifier from either the SurpathService or TenantSurpathService
                    string featureIdentifier = null;
                    if (requirement.RecordRequirement.SurpathServiceFk != null)
                    {
                        featureIdentifier = requirement.RecordRequirement.SurpathServiceFk.FeatureIdentifier;
                    }
                    else if (requirement.TenantSurpathService != null && requirement.TenantSurpathService.SurpathServiceFk != null)
                    {
                        featureIdentifier = requirement.TenantSurpathService.SurpathServiceFk.FeatureIdentifier;
                    }
                    // Fallback to searching in the services list
                    else if (requirement.RecordRequirement.SurpathServiceId.HasValue)
                    {
                        var service = _complianceInfo.SurpathServices.FirstOrDefault(s => s.Id == requirement.RecordRequirement.SurpathServiceId.Value);
                        featureIdentifier = service?.FeatureIdentifier;
                    }

                    // Check the feature identifier
                    if (featureIdentifier == AppFeatures.SurpathFeatureDrugTest)
                    {
                        _cv.Drug = _userRecord?.RecordStatus.ComplianceImpact == EnumStatusComplianceImpact.Compliant;
                    }
                    else if (featureIdentifier == AppFeatures.SurpathFeatureBackgroundCheck)
                    {
                        _cv.Background = _userRecord?.RecordStatus.ComplianceImpact == EnumStatusComplianceImpact.Compliant;
                    }
                }
            }

            // Evaluate immunization compliance
            var _nonSurpathOnlyRequiredCategoryIDs = _complianceInfo.NonSurpathOnlyRequirements
                .Where(_r => _r.RecordCategoryRule?.Required == true)
                .Select(r => r.RecordCategory.Id)
                .ToList();

            _cv.Immunization = !_nonSurpathOnlyRequiredCategoryIDs.Except(_compliantUserCategoryIds).Any();

            // Evaluate overall compliance
            var _allCompliantCategoryIds = _userRecords
                .Where(_r => _r.RecordStatus.ComplianceImpact == EnumStatusComplianceImpact.Compliant)
                .Select(r => r.RecordCategory.Id)
                .ToList();

            var _overallCompliance = !_AllRequiredCategoryIDs.Except(_allCompliantCategoryIds).Any();
            _cv.InCompliance = _cv.Drug && _cv.Background && _cv.Immunization && _overallCompliance;

            return _cv;
        }

        /// <summary>
        /// Efficiently evaluates compliance values for multiple users in bulk.
        /// This method optimizes database queries by batching data retrieval and
        /// processing compliance rules in memory.
        /// </summary>
        /// <param name="userIds">List of user IDs to evaluate</param>
        /// <param name="tenantId">The tenant ID for these users</param>
        /// <returns>Dictionary mapping user IDs to their compliance values</returns>
        public async Task<Dictionary<long, ComplianceValues>> GetBulkComplianceValuesForUsers(List<long> userIds, int tenantId)
        {
            if (!userIds.Any()) return new Dictionary<long, ComplianceValues>();

            // Disable tenant filter for cross-tenant queries if needed
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            try
            {
                // 1. Get all shared tenant-level data once
                await SetTenantRequirements(tenantId);
                var surpathServices = (await GetSurpathServices(tenantId)).ToList();
                var tenantSurpathServices = (await GetTenantSurpathServices(tenantId)).ToList();

                // 2. Batch load all user data
                var userSecureList = await (from u in _lookup_userRepository.GetAll().AsNoTracking()
                                            where userIds.Contains(u.Id)
                                            select new UserSecure
                                            {
                                                Id = u.Id,
                                                FullName = u.FullName,
                                                UserName = u.UserName,
                                                TenantId = u.TenantId
                                            }).ToListAsync();

                // 3. Batch load all user memberships
                var userMemberships = await (from td in _TenantDepartmentRepository.GetAll().AsNoTracking()
                                             join tdu in _TenantDepartmentUserRepository.GetAll().AsNoTracking()
                                                 on td.Id equals tdu.TenantDepartmentId into tduj
                                             from tdu in tduj.DefaultIfEmpty()
                                             join co in _CohortRepository.GetAll().AsNoTracking()
                                                 on td.Id equals co.TenantDepartmentId into coj
                                             from co in coj.DefaultIfEmpty()
                                             join cu in _CohortUserRepository.GetAll().AsNoTracking()
                                                 on co.Id equals cu.CohortId into cuj
                                             from cu in cuj.DefaultIfEmpty()
                                             where (cu.UserId > 0 && userIds.Contains((long)cu.UserId)) ||
                                                   (tdu.UserId > 0 && userIds.Contains((long)tdu.UserId))
                                             select new UserMembership
                                             {
                                                 TenantDepartmentId = td.Id,
                                                 TenantDepartmentName = td.Name,
                                                 CohortId = co.Id,
                                                 CohortName = co.Name,
                                                 CohortDepartmentId = co.TenantDepartmentId,
                                                 CohortUserId = cu.Id,
                                                 UserId = cu.UserId > 0 ? (long)cu.UserId :
                                                         (tdu.UserId > 0 ? (long)tdu.UserId : 0L)
                                             }).ToListAsync();

                // 4. Batch load all record states and statuses
                // IMPORTANT: Only consider current (non-archived) documents for compliance evaluation
                var userRecords = await (from rs in _recordStateRepository.GetAll().AsNoTracking()
                                         join rc in _RecordCategoryRepository.GetAll().AsNoTracking()
                                             on rs.RecordCategoryId equals rc.Id
                                         join rr in _RecordRequirementRepository.GetAll().AsNoTracking()
                                             on rc.RecordRequirementId equals rr.Id
                                         join rcr in _RecordCategoryRuleRepository.GetAll().AsNoTracking()
                                             on rc.RecordCategoryRuleId equals rcr.Id
                                         join rst in _recordStatusRepository.GetAll().AsNoTracking()
                                             on rs.RecordStatusId equals rst.Id
                                         join rec in _recordRepository.GetAll().AsNoTracking()
                                             on rs.RecordId equals rec.Id
                                         where userIds.Contains(rs.UserId ?? 0) && !rs.IsArchived
                                         select new UserRecordStatus
                                         {
                                             RecordState = rs,
                                             RecordCategory = rc,
                                             RecordRequirement = rr,
                                             RecordCategoryRule = rcr,
                                             RecordStatus = rst,
                                             Record = rec
                                         }).ToListAsync();

                // 5. Process compliance for each user
                var results = new Dictionary<long, ComplianceValues>();

                foreach (var userId in userIds)
                {
                    var userMembershipsList = userMemberships.Where(m => m.UserId == userId).ToList();
                    var userRecordsList = userRecords.Where(r => r.RecordState.UserId == userId).ToList();

                    // Initialize compliance values
                    var cv = new ComplianceValues
                    {
                        Background = false,
                        Drug = false,
                        Immunization = false,
                        InCompliance = false,
                        UserId = userId,
                        TenantId = tenantId
                    };

                    // Get user's department list
                    var userDeptList = userMembershipsList
                        .Where(um => um.TenantDepartmentId != null)
                        .Select(um => um.TenantDepartmentId)
                        .Distinct()
                        .ToList();

                    userDeptList.AddRange(userMembershipsList
                        .Where(um => um.CohortId != null)
                        .Select(um => um.CohortDepartmentId)
                        .Where(id => id != null));

                    // Track required and compliant categories
                    var requiredCategoryIds = new List<Guid>();
                    var compliantCategoryIds = userRecordsList
                        .Where(r => r.RecordStatus.ComplianceImpact == EnumStatusComplianceImpact.Compliant)
                        .Select(r => r.RecordCategory.Id)
                        .ToList();

                    // Extract user's record requirement IDs for better service matching
                    var userRecordRequirementIds = userRecordsList
                        .Where(r => r.RecordRequirement != null)
                        .Select(r => r.RecordRequirement.Id)
                        .Distinct()
                        .ToList();

                    // Find the correct drug test requirement for this user based on their existing records
                    var drugTestRequirements = TenantRequirements
                        .Where(r => r.RecordRequirement.IsSurpathOnly)
                        .Where(r =>
                        {
                            // Check various ways the feature identifier could be specified
                            if (r.RecordRequirement.SurpathServiceFk != null)
                                return r.RecordRequirement.SurpathServiceFk.FeatureIdentifier == AppFeatures.SurpathFeatureDrugTest;

                            if (r.TenantSurpathService?.SurpathServiceFk != null)
                                return r.TenantSurpathService.SurpathServiceFk.FeatureIdentifier == AppFeatures.SurpathFeatureDrugTest;

                            if (r.RecordRequirement.SurpathServiceId.HasValue)
                            {
                                var service = surpathServices.FirstOrDefault(s => s.Id == r.RecordRequirement.SurpathServiceId.Value);
                                return service?.FeatureIdentifier == AppFeatures.SurpathFeatureDrugTest;
                            }

                            // Fallback to name matching for legacy data
                            return r.RecordRequirement.Name.ToLower().Contains("drug");
                        })
                        .ToList();

                    TenantRequirement drugTestRequirement = null;
                    if (drugTestRequirements.Any())
                    {
                        // First check if user has existing records for any drug test requirement
                        drugTestRequirement = drugTestRequirements
                            .FirstOrDefault(r => userRecordRequirementIds.Contains(r.RecordRequirement.Id));

                        // If no existing records, fall back to hierarchy
                        if (drugTestRequirement == null)
                        {
                            // Check cohort-specific
                            drugTestRequirement = drugTestRequirements
                                .FirstOrDefault(r => r.RecordRequirement.CohortId != null &&
                                               userMembershipsList.Any(m => m.CohortId == r.RecordRequirement.CohortId));

                            // Check department-specific
                            if (drugTestRequirement == null)
                            {
                                drugTestRequirement = drugTestRequirements
                                    .FirstOrDefault(r => r.RecordRequirement.TenantDepartmentId != null &&
                                                   userDeptList.Contains(r.RecordRequirement.TenantDepartmentId));
                            }

                            // Tenant-wide fallback
                            if (drugTestRequirement == null)
                            {
                                drugTestRequirement = drugTestRequirements
                                    .FirstOrDefault(r => r.RecordRequirement.TenantDepartmentId == null &&
                                                   r.RecordRequirement.CohortId == null);
                            }
                        }
                    }

                    // Find the correct background check requirement for this user
                    var backgroundRequirements = TenantRequirements
                        .Where(r => r.RecordRequirement.IsSurpathOnly)
                        .Where(r =>
                        {
                            // Check various ways the feature identifier could be specified
                            if (r.RecordRequirement.SurpathServiceFk != null)
                                return r.RecordRequirement.SurpathServiceFk.FeatureIdentifier == AppFeatures.SurpathFeatureBackgroundCheck;

                            if (r.TenantSurpathService?.SurpathServiceFk != null)
                                return r.TenantSurpathService.SurpathServiceFk.FeatureIdentifier == AppFeatures.SurpathFeatureBackgroundCheck;

                            if (r.RecordRequirement.SurpathServiceId.HasValue)
                            {
                                var service = surpathServices.FirstOrDefault(s => s.Id == r.RecordRequirement.SurpathServiceId.Value);
                                return service?.FeatureIdentifier == AppFeatures.SurpathFeatureBackgroundCheck;
                            }

                            // Fallback to name matching for legacy data
                            return r.RecordRequirement.Name.ToLower().Contains("background");
                        })
                        .ToList();

                    TenantRequirement backgroundRequirement = null;
                    if (backgroundRequirements.Any())
                    {
                        // First check if user has existing records for any background requirement
                        backgroundRequirement = backgroundRequirements
                            .FirstOrDefault(r => userRecordRequirementIds.Contains(r.RecordRequirement.Id));

                        // If no existing records, fall back to hierarchy
                        if (backgroundRequirement == null)
                        {
                            // Check cohort-specific
                            backgroundRequirement = backgroundRequirements
                                .FirstOrDefault(r => r.RecordRequirement.CohortId != null &&
                                               userMembershipsList.Any(m => m.CohortId == r.RecordRequirement.CohortId));

                            // Check department-specific
                            if (backgroundRequirement == null)
                            {
                                backgroundRequirement = backgroundRequirements
                                    .FirstOrDefault(r => r.RecordRequirement.TenantDepartmentId != null &&
                                                   userDeptList.Contains(r.RecordRequirement.TenantDepartmentId));
                            }

                            // Tenant-wide fallback
                            if (backgroundRequirement == null)
                            {
                                backgroundRequirement = backgroundRequirements
                                    .FirstOrDefault(r => r.RecordRequirement.TenantDepartmentId == null &&
                                                   r.RecordRequirement.CohortId == null);
                            }
                        }
                    }

                    // Process requirements (restored from working usertransfer logic)
                    foreach (var requirement in TenantRequirements)
                    {
                        if (requirement.RecordCategoryRule?.Required == true)
                        {
                            requiredCategoryIds.Add(requirement.RecordCategory.Id);
                        }
                    }

                    // Check drug test compliance
                    if (drugTestRequirement != null)
                    {
                        requiredCategoryIds.Add(drugTestRequirement.RecordCategory.Id);
                        var userDrugRecord = userRecordsList.FirstOrDefault(r =>
                            r.RecordRequirement.Id == drugTestRequirement.RecordRequirement.Id &&
                            r.RecordCategory.Id == drugTestRequirement.RecordCategory.Id);
                        cv.Drug = userDrugRecord?.RecordStatus.ComplianceImpact == EnumStatusComplianceImpact.Compliant;
                    }

                    // Check background check compliance
                    if (backgroundRequirement != null)
                    {
                        requiredCategoryIds.Add(backgroundRequirement.RecordCategory.Id);
                        var userBackgroundRecord = userRecordsList.FirstOrDefault(r =>
                            r.RecordRequirement.Id == backgroundRequirement.RecordRequirement.Id &&
                            r.RecordCategory.Id == backgroundRequirement.RecordCategory.Id);
                        cv.Background = userBackgroundRecord?.RecordStatus.ComplianceImpact == EnumStatusComplianceImpact.Compliant;
                    }

                    // Evaluate immunization compliance
                    var nonSurpathOnlyRequiredIds = TenantRequirements
                        .Where(r => !r.RecordRequirement.IsSurpathOnly && r.RecordCategoryRule?.Required == true)
                        .Select(r => r.RecordCategory.Id)
                        .ToList();

                    cv.Immunization = !nonSurpathOnlyRequiredIds.Except(compliantCategoryIds).Any();

                    // Evaluate overall compliance
                    var overallCompliance = !requiredCategoryIds.Except(compliantCategoryIds).Any();
                    cv.InCompliance = cv.Drug && cv.Background && cv.Immunization && overallCompliance;

                    results[userId] = cv;
                }

                return results;
            }
            finally
            {
                CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);
            }
        }

        private async Task<IQueryable<SurpathService>> GetSurpathServices(int _tenantId)
        {
            var _q = from ss in _surpathServiceRepository.GetAll().AsNoTracking().IgnoreQueryFilters().Where(s => s.IsDeleted == false)
                     select ss;
            return _q;
        }

        private async Task<IQueryable<TenantSurpathService>> GetTenantSurpathServices(int _tenantId)
        {
            var _q = from ss in _tenantSurpathServiceRepository.GetAll().AsNoTracking().IgnoreQueryFilters().Where(s => s.IsDeleted == false && s.TenantId == _tenantId)
                     select ss;
            return _q;
        }

        public async Task<ComplianceInfo> GetComplianceInfo(int _tenantId, long _userId = 0)
        {
            await SetTenantRequirements(_tenantId);
            var _userMembershipQuery = await GetUserDeptAndCohortMembership(_userId, _tenantId);
            var _surpathServiceQuery = await GetSurpathServices(_tenantId);
            var _tenantSurpathServiceQuery = await GetTenantSurpathServices(_tenantId);

            // having all requirements for the tenant, and all the user's department and cohort memberships,
            // we can now determine the user's compliance status for each requirement
            // that applies to them as a cohort member
            // or as a department member
            // or as a cohort member of a department
            // or a requirement with no cohort or department, meaning it applies to all users in the tenant

            var _requirementsForUser = new List<TenantRequirement>();
            var _usermembershipsList = _userMembershipQuery.ToList();

            // make a list of all departments the user is affected by
            var _userDeptList = _userMembershipQuery.Where(_um => _um.TenantDepartmentId != null)
                .Select(_um => _um.TenantDepartmentId).ToList().Distinct().ToList();

            // now add the user's cohort department if they are in a cohort and the cohort is in a department
            _userDeptList.AddRange(_userMembershipQuery.Where(_um => _um.CohortId != null)
                .Select(_um => _um.CohortDepartmentId).ToList());

            _userDeptList = _userDeptList.Distinct().ToList();

            // Get the user's cohort if they are in a cohort
            var _userCohort = _userMembershipQuery.Where(_um => _um.CohortId != null).FirstOrDefault();

            // Debug log user membership
            Logger.Debug($"GetComplianceInfo - User {_userId} memberships: {_usermembershipsList.Count} total");
            foreach (var membership in _usermembershipsList)
            {
                Logger.Debug($"  - DeptId={membership.TenantDepartmentId}, DeptName={membership.TenantDepartmentName}, " +
                    $"CohortId={membership.CohortId}, CohortName={membership.CohortName}, CohortDeptId={membership.CohortDepartmentId}");
            }

            // HIERARCHICAL REQUIREMENT COLLECTION WITH DEDUPLICATION
            // Process requirements from most specific to least specific
            // Track added requirements to prevent duplicates based on service/requirement type
            var addedRequirementKeys = new HashSet<string>();
            var categoryRuleInheritance = new Dictionary<string, (Guid? categoryId, Guid? ruleId)>();

            // Helper function to get a unique key for deduplication
            Func<TenantRequirement, string> getRequirementKey = (req) =>
            {
                // Include category ID to ensure multi-category requirements are all included
                var categoryPart = req.RecordCategory != null ? $"_Cat_{req.RecordCategory.Id}" : "";

                // Use service ID if available, otherwise use requirement name
                if (req.RecordRequirement.SurpathServiceId.HasValue)
                    return $"Service_{req.RecordRequirement.SurpathServiceId}{categoryPart}";
                else if (req.RecordRequirement.TenantSurpathServiceId.HasValue)
                    return $"TenantService_{req.RecordRequirement.TenantSurpathServiceId}{categoryPart}";
                else
                    return $"Requirement_{req.RecordRequirement.Name.ToLower().Replace(" ", "")}{categoryPart}";
            };

            // Helper function to process requirements with deduplication and inheritance
            Action<IEnumerable<TenantRequirement>> processRequirements = (requirements) =>
            {
                foreach (var req in requirements)
                {
                    var key = getRequirementKey(req);

                    // Only add if we haven't already added a requirement for this key
                    if (addedRequirementKeys.Add(key))
                    {
                        // Check for category/rule inheritance
                        if (req.RecordCategory != null)
                        {
                            // If this requirement has no category rule, try to inherit
                            if (req.RecordCategory.RecordCategoryRuleId == null ||
                                req.RecordCategory.RecordCategoryRuleId == Guid.Empty)
                            {
                                if (categoryRuleInheritance.ContainsKey(key))
                                {
                                    var (inheritedCategoryId, inheritedRuleId) = categoryRuleInheritance[key];
                                    if (inheritedRuleId.HasValue && req.RecordCategoryRule == null)
                                    {
                                        req.RecordCategory.RecordCategoryRuleId = inheritedRuleId.Value;
                                        Logger.Debug($"Inherited rule {inheritedRuleId} for requirement {req.RecordRequirement.Name}");
                                    }
                                }
                            }
                            else
                            {
                                // This requirement has a rule, save it for inheritance
                                if (!categoryRuleInheritance.ContainsKey(key))
                                {
                                    categoryRuleInheritance[key] = (req.RecordCategory.Id, req.RecordCategory.RecordCategoryRuleId);
                                }
                            }
                        }

                        _requirementsForUser.Add(req);
                        Logger.Debug($"Added requirement: {req.RecordRequirement.Name} (Key: {key})");
                    }
                    else
                    {
                        Logger.Debug($"Skipped duplicate requirement: {req.RecordRequirement.Name} (Key: {key}) - less specific than already added");
                    }
                }
            };

            // 1. MOST SPECIFIC: CohortUser-specific requirements (assigned to specific cohort user)
            if (_userCohort != null)
            {
                var cohortUserSpecificRequirements = TenantRequirements
                    .Where(_r => _r.RecordRequirement != null
                                 && _r.RecordRequirement.TenantSurpathServiceId != null
                                 && _r.TenantSurpathService != null
                                 && _r.TenantSurpathService.CohortUserId == _userCohort.CohortUserId)
                    .ToList();
                
                Logger.Debug($"Processing {cohortUserSpecificRequirements.Count} CohortUser-specific requirements (most specific)");
                processRequirements(cohortUserSpecificRequirements);
            }

            // 2. Cohort-specific requirements (assigned to the cohort)
            if (_userCohort != null)
            {
                var cohortOnlyRequirements = TenantRequirements.Where(_r =>
                    _r.RecordRequirement.CohortId != null &&
                    _r.RecordRequirement.TenantDepartmentId == null &&
                    _userCohort.CohortId == _r.RecordRequirement.CohortId).ToList();

                Logger.Debug($"Processing {cohortOnlyRequirements.Count} Cohort-specific requirements");
                processRequirements(cohortOnlyRequirements);
            }

            // 2.5. NEW: Cohort+Department-specific requirements (assigned to both)
            if (_userCohort != null && _userDeptList.Any())
            {
                var cohortAndDeptRequirements = TenantRequirements.Where(_r =>
                    _r.RecordRequirement.CohortId != null &&
                    _r.RecordRequirement.TenantDepartmentId != null &&
                    _userCohort.CohortId == _r.RecordRequirement.CohortId &&
                    _userDeptList.Contains(_r.RecordRequirement.TenantDepartmentId)).ToList();

                Logger.Debug($"Processing {cohortAndDeptRequirements.Count} Cohort+Department-specific requirements");
                processRequirements(cohortAndDeptRequirements);
            }

            // 3. Department-specific requirements (assigned to the department)
            if (_userDeptList.Any())
            {
                var deptOnlyRequirements = TenantRequirements
                    .Where(_r => _r.RecordRequirement.CohortId == null &&
                                 _r.RecordRequirement.TenantDepartmentId != null &&
                                 _userDeptList.Contains(_r.RecordRequirement.TenantDepartmentId))
                    .ToList();

                Logger.Debug($"Processing {deptOnlyRequirements.Count} Department-specific requirements");
                processRequirements(deptOnlyRequirements);
            }

            // 4. LEAST SPECIFIC: Tenant-wide requirements (apply to all users in tenant)
            var tenantWideRequirements = TenantRequirements
                .Where(_r => _r.RecordRequirement.CohortId == null &&
                             _r.RecordRequirement.TenantDepartmentId == null)
                .ToList();

            Logger.Debug($"Processing {tenantWideRequirements.Count} Tenant-wide requirements (least specific)");
            processRequirements(tenantWideRequirements);

            // Log final requirement count
            var _requirementsForUserTotalCount = _requirementsForUser.Count();
            Logger.Debug($"Total unique requirements for user after correct hierarchical processing: {_requirementsForUserTotalCount}");

            // Separate requirements by type
            var _surpathOnlyRequirements = _requirementsForUser.Where(_r => _r.RecordRequirement.IsSurpathOnly == true).ToList();
            var _nonSurpathOnlyRequirements = _requirementsForUser.Where(_r => _r.RecordRequirement.IsSurpathOnly == false).ToList();

            Logger.Debug($"GetComplianceInfo complete: Total requirements: {_requirementsForUser.Count}, " +
                $"SurpathOnly: {_surpathOnlyRequirements.Count}, NonSurpathOnly: {_nonSurpathOnlyRequirements.Count}");

            return new ComplianceInfo()
            {
                UserId = _userId,
                NonSurpathOnlyRequirements = _nonSurpathOnlyRequirements,
                SurpathOnlyRequirements = _surpathOnlyRequirements,
                RequirementsForUser = _requirementsForUser,
                UserDeptList = _userDeptList,
                UserMembershipsList = _usermembershipsList,
                RecordCategoryRules = await RecordCategoryRules(_tenantId),
                SurpathServices = _surpathServiceQuery.ToList(),
                TenantSurpathServices = _tenantSurpathServiceQuery.ToList()
            };
        }

        private async Task<List<UserRecordSecureState>> GetUserRecordSecureStates(long _tenantId, long _userId)
        {
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            // get all the records for the user, along with record categories and record category rules and record states and record statuses
            // IMPORTANT: Only consider current (non-archived) documents for compliance evaluation
            var _q = from rs in _recordStateRepository.GetAll().AsNoTracking().Where(_rs => _rs.UserId == _userId && !_rs.IsArchived)
                     join rc in _RecordCategoryRepository.GetAll().AsNoTracking() on rs.RecordCategoryId equals rc.Id
                     join rr in _RecordRequirementRepository.GetAll().AsNoTracking() on rc.RecordRequirementId equals rr.Id
                     join rcr in _RecordCategoryRuleRepository.GetAll().AsNoTracking() on rc.RecordCategoryRuleId equals rcr.Id
                     join rst in _recordStatusRepository.GetAll().AsNoTracking() on rs.RecordStatusId equals rst.Id
                     select new UserRecordSecureState
                     {
                         RecordRequirementId = rr.Id,
                         RecordRequirementIsSurpathOnly = rr.IsSurpathOnly,
                         RecordStatusComplianceImpact = rst.ComplianceImpact,
                         RecordCategoryRuleRequired = rcr.Required,
                         RecordCategoryId = rc.Id,
                         UserId = rs.UserId.GetValueOrDefault(0L)
                     }
                     ;

            var _userRecords = _q.ToList();
            //// get the rules with tenant filter disabled
            //var _rules = await RecordCategoryRules(_tenantId);
            //// set the rules on the objects

            CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);

            return _userRecords;
        }

        private async Task<List<UserRecordStatus>> GetUserRecordStatuses(int _tenantId, long _userId)
        {
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            // get all the records for the user, along with record categories and record category rules and record states and record statuses
            // IMPORTANT: Only consider current (non-archived) documents for compliance evaluation
            var _q = from rs in _recordStateRepository.GetAll().AsNoTracking().Where(_rs => _rs.UserId == _userId && !_rs.IsArchived)
                     join rc in _RecordCategoryRepository.GetAll().AsNoTracking() on rs.RecordCategoryId equals rc.Id
                     join rr in _RecordRequirementRepository.GetAll().AsNoTracking() on rc.RecordRequirementId equals rr.Id
                     join rcr in _RecordCategoryRuleRepository.GetAll().AsNoTracking() on rc.RecordCategoryRuleId equals rcr.Id
                     join rst in _recordStatusRepository.GetAll().AsNoTracking() on rs.RecordStatusId equals rst.Id
                     join rec in _recordRepository.GetAll().AsNoTracking() on rs.RecordId equals rec.Id
                     select new UserRecordStatus
                     {
                         RecordState = rs,
                         RecordCategory = rc,
                         RecordRequirement = rr,
                         RecordCategoryRule = rcr,
                         RecordStatus = rst,
                         Record = rec
                     }
                     ;

            var _Records = _q.ToList();

            CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);

            return _Records;
        }

        private class UserRecordStatus
        {
            public RecordState RecordState { get; set; }
            public RecordCategory RecordCategory { get; set; }
            public RecordRequirement RecordRequirement { get; set; }
            public RecordStatus RecordStatus { get; set; }
            public RecordCategoryRule RecordCategoryRule { get; set; }
            public Record Record { get; set; }
        }

        private async Task<List<RecordCategoryRule>> RecordCategoryRules(int _tenantId)
        {
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var r = _RecordCategoryRuleRepository.GetAll().AsNoTracking().Where(_rcr => _rcr.TenantId == _tenantId || _rcr.TenantId == null).ToList();
            CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);
            return r;
        }

        private class UserRecordSecureState
        {
            public Guid? RecordRequirementId { get; set; }
            public bool RecordRequirementIsSurpathOnly { get; set; }
            public EnumStatusComplianceImpact RecordStatusComplianceImpact { get; set; }
            public bool RecordCategoryRuleRequired { get; set; }
            public Guid? RecordCategoryId { get; set; }
            public long UserId { get; set; }
        }

        /// <summary>
        /// This gets a user's department and cohort membership
        /// </summary>
        /// <param name="_userId">The ID of the user to get memberships for</param>
        /// <param name="_tenantId">The tenant ID to filter departments by. If not provided, uses AbpSession.TenantId</param>
        /// <returns>IQueryable of UserMembership objects</returns>
        private async Task<IQueryable<UserMembership>> GetUserDeptAndCohortMembership(long _userId, int? _tenantId = null)
        {
            if (_userId < 1) return new List<UserMembership>().AsQueryable();

            var tenantIdToUse = _tenantId ?? AbpSession.TenantId;

            // Get department memberships
            var deptMemberships = from td in _TenantDepartmentRepository.GetAll().AsNoTracking().Where(_td => _td.TenantId == tenantIdToUse)
                                  join tdu in _TenantDepartmentUserRepository.GetAll().AsNoTracking().Where(_tdu => _tdu.UserId == _userId)
                                      on td.Id equals tdu.TenantDepartmentId
                                  select new UserMembership()
                                  {
                                      TenantDepartmentId = td.Id,
                                      TenantDepartmentName = td.Name,
                                      CohortId = null,
                                      CohortName = "",
                                      CohortDepartmentId = null,
                                      CohortUserId = null,
                                      UserId = (long)tdu.UserId
                                  };

            // Get cohort memberships (including cohorts without departments)
            var cohortMemberships = from cu in _CohortUserRepository.GetAll().AsNoTracking().Where(_cu => _cu.UserId == _userId)
                                    join co in _CohortRepository.GetAll().AsNoTracking().Where(_co => _co.TenantId == tenantIdToUse)
                                        on cu.CohortId equals co.Id
                                    join td in _TenantDepartmentRepository.GetAll().AsNoTracking().Where(_td => _td.TenantId == tenantIdToUse)
                                        on co.TenantDepartmentId equals td.Id into tdj
                                    from td in tdj.DefaultIfEmpty()
                                    select new UserMembership()
                                    {
                                        TenantDepartmentId = td == null ? null : td.Id,
                                        TenantDepartmentName = td == null ? "" : td.Name,
                                        CohortId = co.Id,
                                        CohortName = co.Name,
                                        CohortDepartmentId = co.TenantDepartmentId,
                                        CohortUserId = cu.Id,
                                        UserId = (long)cu.UserId
                                    };

            // Union the results
            var _q = deptMemberships.Union(cohortMemberships);
            return _q;
        }

        private async Task<UserSecure> GetUserSecure(long _userId)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }
            ;

            var _thisUser = from u in _lookup_userRepository.GetAll().AsNoTracking().Where(e => e.Id == _userId)
                            join t1 in _TenantDepartmentUserRepository.GetAll().AsNoTracking() on u.Id equals t1.UserId into t2
                            from t in t2.DefaultIfEmpty()

                            join cu1 in _CohortUserRepository.GetAll().AsNoTracking() on u.Id equals cu1.UserId into cu2
                            from cu in cu2.DefaultIfEmpty()

                            select new UserSecure
                            {
                                Id = u.Id,
                                CohortId = cu == null ? Guid.Empty : cu.CohortId,
                                TenantDepartmentId = t == null ? Guid.Empty : t.TenantDepartmentId,
                                FullName = u.FullName,
                                UserName = u.UserName,
                                TenantId = u.TenantId
                            };

            return _thisUser.FirstOrDefault();
        }

        private class UserSecure
        {
            public long Id { get; set; }
            public Guid? CohortId { get; set; }
            public Guid? TenantDepartmentId { get; set; }
            public string FullName { get; set; }
            public string UserName { get; set; }
            public int? TenantId { get; set; }
        }

        private List<TenantRequirement>? TenantRequirements { get; set; } // = new List<TenantRequirement>();
        private Dictionary<Guid, List<TenantSurpathService>>? _cachedServicesBySurpathServiceId; // Cache for performance

        /// <summary>
        /// Checks if a SurpathService is effectively enabled at any applicable level in the hierarchy
        /// </summary>
        private bool IsServiceEffectivelyEnabled(
            Guid? surpathServiceId,
            Guid? requirementDepartmentId,
            Guid? requirementCohortId,
            Dictionary<Guid, List<TenantSurpathService>> serviceCache)
        {
            if (!surpathServiceId.HasValue)
                return true; // If no service is linked, requirement is not service-dependent

            // Get all TenantSurpathServices for this SurpathService from cache
            if (!serviceCache.ContainsKey(surpathServiceId.Value))
            {
                // No services found for this SurpathService
                // This might mean the service exists at a level we're evaluating
                // Don't filter out - let the user-level filtering handle it
                return true;
            }

            var allServices = serviceCache[surpathServiceId.Value];

            // Check based on requirement assignment level
            if (requirementCohortId.HasValue)
            {
                // Cohort-level requirement: check cohort → department → tenant
                // Check cohort level
                if (allServices.Any(s => s.CohortId == requirementCohortId && s.IsPricingOverrideEnabled))
                    return true;

                // Check department level (if cohort has a department)
                if (requirementDepartmentId.HasValue &&
                    allServices.Any(s => s.TenantDepartmentId == requirementDepartmentId &&
                                        s.CohortId == null && s.IsPricingOverrideEnabled))
                    return true;

                // Check tenant level
                if (allServices.Any(s => s.TenantDepartmentId == null &&
                                        s.CohortId == null &&
                                        s.UserId == null && s.IsPricingOverrideEnabled))
                    return true;
            }
            else if (requirementDepartmentId.HasValue)
            {
                // Department-level requirement: check if service is enabled at the department level
                // For department-level requirements, we only need to check if the service is enabled
                // at the department level itself. We don't require tenant-level enablement.
                if (allServices.Any(s => s.TenantDepartmentId == requirementDepartmentId &&
                                        s.CohortId == null && s.IsPricingOverrideEnabled))
                    return true;

                // Also include if no specific service assignment exists at department level
                // This allows department requirements to be included even if only the requirement
                // is assigned at department level
                if (!allServices.Any(s => s.TenantDepartmentId == requirementDepartmentId))
                    return true;
            }
            else
            {
                // Tenant-level requirement: check if service is enabled at ANY level
                // This allows tenant-level requirements to apply when the service is enabled
                // at department/cohort level even if disabled at tenant level
                if (allServices.Any(s => s.IsPricingOverrideEnabled))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// This gets ALL tenant requirements for the tenant, including those that are not required by rule
        /// </summary>
        /// <param name="_tenantid"></param>
        /// <returns></returns>
        //private async Task<IQueryable<TenantRequirement>> GetTenantRequirements(int _tenantid)
        private async Task SetTenantRequirements(int _tenantid)
        {
            if (TenantRequirements != null)
            {
                return;
            }

            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var _q = (from rr in _RecordRequirementRepository.GetAll().Include(x => x.SurpathServiceFk).AsNoTracking().Where(_rr => _rr.TenantId == _tenantid)
                      join tssq in _tenantSurpathServiceRepository.GetAll().Include(x => x.SurpathServiceFk).IgnoreQueryFilters().AsNoTracking() on rr.TenantSurpathServiceId equals tssq.Id into tssq2
                      from tss in tssq2.DefaultIfEmpty()
                      from td in _TenantDepartmentRepository.GetAll().AsNoTracking().Where(_td => _td.Id == rr.TenantDepartmentId && _td.TenantId == _tenantid).DefaultIfEmpty()
                      from co in _CohortRepository.GetAll().AsNoTracking().Where(_co => (_co.Id == rr.CohortId || _co.TenantDepartmentId == rr.TenantDepartmentId) && _co.TenantId == _tenantid).DefaultIfEmpty()
                      from rc in _RecordCategoryRepository.GetAll().AsNoTracking().Where(_rc => _rc.RecordRequirementId == rr.Id && _rc.TenantId == _tenantid).DefaultIfEmpty()
                      from rcr in _RecordCategoryRuleRepository.GetAll().AsNoTracking().Where(_rcr => _rcr.Id == rc.RecordCategoryRuleId).DefaultIfEmpty()
                      where
                      rr.IsDeleted == false
                      && (tss == null || tss.IsDeleted == false) // Only check if not deleted, not if enabled
                      && rc.IsDeleted == false
                      && rcr.IsDeleted == false
                      &&
                       (
                       (rr.CohortId == null && rr.TenantDepartmentId == null) ||
                       (rr.CohortId != null && rr.TenantDepartmentId != null && co.TenantDepartmentId == rr.TenantDepartmentId) ||
                       (rr.CohortId != null && rr.TenantDepartmentId == null) ||
                       (rr.CohortId == null && rr.TenantDepartmentId != null && co.TenantDepartmentId == rr.TenantDepartmentId)
                       )
                      select new TenantRequirement()
                      {
                          RecordRequirement = rr,
                          RecordCategory = rc,
                          RecordCategoryRule = rcr != null ? rcr : new RecordCategoryRule() { Required = false, IsSurpathOnly = false, IsDeleted = false, MetaData = "{}", Name = "No Rule", Notify = false, Expires = false, TenantId = _tenantid, TemplateRuleId = Guid.Empty, Id = Guid.Empty },
                          TenantSurpathService = tss // Include the service for checking
                      }).Distinct();

            var allRequirements = await _q.ToListAsync();

            // Build cache of all TenantSurpathServices grouped by SurpathServiceId for performance
            var allTenantServices = await _tenantSurpathServiceRepository.GetAll()
                .Include(s => s.SurpathServiceFk)
                .Where(s => s.TenantId == _tenantid && s.IsDeleted == false)
                .ToListAsync();

            var serviceCache = allTenantServices
                .Where(s => s.SurpathServiceId.HasValue)
                .GroupBy(s => s.SurpathServiceId.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Post-process to check hierarchical service enablement
            var filteredRequirements = new List<TenantRequirement>();
            foreach (var req in allRequirements)
            {
                // Get the SurpathServiceId from the linked TenantSurpathService
                var surpathServiceId = req.TenantSurpathService?.SurpathServiceId;

                // Check if the service is effectively enabled through the hierarchy
                var isEffectivelyEnabled = IsServiceEffectivelyEnabled(
                    surpathServiceId,
                    req.RecordRequirement.TenantDepartmentId,
                    req.RecordRequirement.CohortId,
                    serviceCache);

                if (isEffectivelyEnabled)
                {
                    filteredRequirements.Add(req);
                }
            }

            TenantRequirements = filteredRequirements;

            // Debug logging
            Logger.Debug($"SetTenantRequirements for tenant {_tenantid}: " +
                $"Loaded {allRequirements.Count} requirements initially, " +
                $"{TenantRequirements.Count} after hierarchical filtering");

            var filteredOutCount = allRequirements.Count - filteredRequirements.Count;
            if (filteredOutCount > 0)
            {
                Logger.Debug($"  - Filtered out {filteredOutCount} requirements due to disabled services without enabled ancestors");

                // Log details of filtered requirements for debugging
                var filteredOut = allRequirements.Except(filteredRequirements);
                foreach (var req in filteredOut.Take(5)) // Log first 5 for brevity
                {
                    Logger.Debug($"    - Requirement '{req.RecordRequirement.Name}' " +
                        $"(Dept: {req.RecordRequirement.TenantDepartmentId}, " +
                        $"Cohort: {req.RecordRequirement.CohortId}) " +
                        $"linked to disabled service: {req.TenantSurpathService?.Name}");
                }
            }

            var cohortOnlyCount = TenantRequirements.Count(r => r.RecordRequirement.CohortId != null && r.RecordRequirement.TenantDepartmentId == null);
            Logger.Debug($"  - Cohort-only requirements: {cohortOnlyCount}");

            return;
        }

        private async Task<List<UserRecordSecureState>> GetAllUserRecordSecureStates(int _tenantId)
        {
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            // get all the records for all users in the tenant, along with record categories and record category rules and record states and record statuses
            // IMPORTANT: Only consider current (non-archived) documents for compliance evaluation
            var _q = from rs in _recordStateRepository.GetAll().AsNoTracking()
                     join rc in _RecordCategoryRepository.GetAll().AsNoTracking() on rs.RecordCategoryId equals rc.Id
                     join rr in _RecordRequirementRepository.GetAll().AsNoTracking() on rc.RecordRequirementId equals rr.Id
                     join rcr in _RecordCategoryRuleRepository.GetAll().AsNoTracking() on rc.RecordCategoryRuleId equals rcr.Id
                     join rst in _recordStatusRepository.GetAll().AsNoTracking() on rs.RecordStatusId equals rst.Id
                     where rs.TenantId == _tenantId && !rs.IsArchived
                     select new UserRecordSecureState
                     {
                         RecordRequirementId = rr.Id,
                         RecordRequirementIsSurpathOnly = rr.IsSurpathOnly,
                         RecordStatusComplianceImpact = rst.ComplianceImpact,
                         RecordCategoryRuleRequired = rcr.Required,
                         RecordCategoryId = rc.Id,
                         UserId = rs.UserId.GetValueOrDefault(0L)
                     };

            var _userRecords = _q.ToList();

            CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);

            return _userRecords;
        }

        private async Task<IQueryable<UserMembership>> GetUserDeptAndCohortMemberships()
        {
            var _q = from td in _TenantDepartmentRepository.GetAll().AsNoTracking().Where(_td => _td.TenantId == AbpSession.TenantId)
                     join tdu in _TenantDepartmentUserRepository.GetAll().AsNoTracking() on td.Id equals tdu.TenantDepartmentId into tduj
                     from tdu in tduj.DefaultIfEmpty()
                     join co in _CohortRepository.GetAll().AsNoTracking() on td.Id equals co.TenantDepartmentId into coj
                     from co in coj.DefaultIfEmpty()
                     join cu in _CohortUserRepository.GetAll().AsNoTracking() on co.Id equals cu.CohortId into cuj
                     from cu in cuj.DefaultIfEmpty()
                     select new UserMembership()
                     {
                         TenantDepartmentId = td == null ? null : td.Id,
                         TenantDepartmentName = td == null ? "" : td.Name,
                         CohortId = co == null ? null : co.Id,
                         CohortName = co == null ? "" : co.Name,
                         CohortDepartmentId = co.TenantDepartmentId == null ? null : co.TenantDepartmentId,
                         CohortUserId = cu == null ? null : cu.Id,
                         UserId = cu == null ?
                                 (tdu == null || tdu.UserId == null ? 0L : (long)tdu.UserId) :
                                 (cu.UserId == null ? 0L : (long)cu.UserId)
                     };
            return _q;
        }

        /// <summary>
        /// Gets all applicable requirement categories using the hierarchical approach (Tenant -> Department -> Cohort -> User)
        /// This method can be used for various scenarios like cohort migration, compliance evaluation, etc.
        /// </summary>
        /// <param name="departmentId">Optional department ID to get requirements for</param>
        /// <param name="cohortId">Optional cohort ID to get requirements for</param>
        /// <param name="userId">Optional user ID to get requirements for</param>
        /// <param name="includeInherited">Whether to include inherited requirements from higher levels</param>
        /// <returns>List of requirement categories with their hierarchy level</returns>
        public async Task<List<HierarchicalRequirementCategoryDto>> GetHierarchicalRequirementCategories(
            Guid? departmentId = null,
            Guid? cohortId = null,
            long? userId = null,
            bool includeInherited = true)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var query = _RecordCategoryRepository.GetAll()
                .Include(rc => rc.RecordRequirementFk)
                .Include(rc => rc.RecordCategoryRuleFk)
                .AsNoTracking();

            // Build the hierarchical query based on parameters
            if (includeInherited)
            {
                // Get the cohort if specified to check its department
                Guid? cohortDepartmentId = null;
                if (cohortId.HasValue)
                {
                    var cohort = await _CohortRepository.FirstOrDefaultAsync(c => c.Id == cohortId.Value);
                    cohortDepartmentId = cohort?.TenantDepartmentId;
                }

                query = query.Where(rc =>
                    // 1. Tenant-level requirements (apply to all)
                    (rc.RecordRequirementFk.TenantDepartmentId == null && rc.RecordRequirementFk.CohortId == null) ||

                    // 2. Department-level requirements
                    (departmentId.HasValue &&
                     rc.RecordRequirementFk.TenantDepartmentId == departmentId &&
                     rc.RecordRequirementFk.CohortId == null) ||

                    // 3. Department-level requirements inherited by cohort
                    (cohortDepartmentId.HasValue &&
                     rc.RecordRequirementFk.TenantDepartmentId == cohortDepartmentId &&
                     rc.RecordRequirementFk.CohortId == null) ||

                    // 4. Cohort-specific requirements
                    (cohortId.HasValue &&
                     rc.RecordRequirementFk.CohortId == cohortId &&
                     rc.RecordRequirementFk.TenantDepartmentId == null) ||

                    // 5. Requirements for cohort AND department combination
                    (cohortId.HasValue && cohortDepartmentId.HasValue &&
                     rc.RecordRequirementFk.TenantDepartmentId == cohortDepartmentId &&
                     rc.RecordRequirementFk.CohortId == cohortId)
                );
            }
            else
            {
                // Only get requirements at the specified level
                query = query.Where(rc =>
                    (cohortId.HasValue && rc.RecordRequirementFk.CohortId == cohortId) ||
                    (departmentId.HasValue && !cohortId.HasValue && rc.RecordRequirementFk.TenantDepartmentId == departmentId) ||
                    (!departmentId.HasValue && !cohortId.HasValue && rc.RecordRequirementFk.TenantDepartmentId == null && rc.RecordRequirementFk.CohortId == null)
                );
            }

            var categories = await query
                .Select(rc => new HierarchicalRequirementCategoryDto
                {
                    CategoryId = rc.Id,
                    CategoryName = rc.Name,
                    CategoryInstructions = rc.Instructions,
                    RequirementId = rc.RecordRequirementId ?? Guid.Empty,
                    RequirementName = rc.RecordRequirementFk.Name,
                    RequirementDescription = rc.RecordRequirementFk.Description,
                    IsDepartmentSpecific = rc.RecordRequirementFk.TenantDepartmentId.HasValue,
                    IsCohortSpecific = rc.RecordRequirementFk.CohortId.HasValue,
                    IsRequired = rc.RecordCategoryRuleFk != null && rc.RecordCategoryRuleFk.Required,
                    HierarchyLevel = DetermineHierarchyLevel(
                        rc.RecordRequirementFk.TenantDepartmentId,
                        rc.RecordRequirementFk.CohortId),
                    DepartmentId = rc.RecordRequirementFk.TenantDepartmentId,
                    CohortId = rc.RecordRequirementFk.CohortId,
                    IsSurpathOnly = rc.RecordRequirementFk != null && rc.RecordRequirementFk.IsSurpathOnly == true
                })
                .ToListAsync();

            return categories;
        }

        /// <summary>
        /// Determines the hierarchy level of a requirement based on its associations
        /// </summary>
        private static string DetermineHierarchyLevel(Guid? departmentId, Guid? cohortId)
        {
            if (!departmentId.HasValue && !cohortId.HasValue)
                return "Tenant";
            if (departmentId.HasValue && cohortId.HasValue)
                return "CohortAndDepartment";
            if (cohortId.HasValue)
                return "Cohort";
            if (departmentId.HasValue)
                return "Department";

            return "Unknown";
        }

        /// <summary>
        /// Converts hierarchical requirement categories to TenantRequirement format
        /// </summary>
        private async Task<List<TenantRequirement>> ConvertHierarchicalToTenantRequirements(List<HierarchicalRequirementCategoryDto> hierarchicalCategories)
        {
            var result = new List<TenantRequirement>();

            // Get all category IDs to fetch full data
            var categoryIds = hierarchicalCategories.Select(h => h.CategoryId).ToList();

            if (!categoryIds.Any())
                return result;

            // Fetch full requirement and category data including rules
            var fullData = await _RecordCategoryRepository.GetAll()
                .Include(rc => rc.RecordRequirementFk)
                .Include(rc => rc.RecordCategoryRuleFk)
                .Where(rc => categoryIds.Contains(rc.Id))
                .Select(rc => new
                {
                    Category = rc,
                    Requirement = rc.RecordRequirementFk,
                    Rule = rc.RecordCategoryRuleFk
                })
                .ToListAsync();

            // Convert to TenantRequirement format
            foreach (var data in fullData)
            {
                // Skip if either requirement or category is null
                if (data.Requirement == null || data.Category == null)
                {
                    Logger.Warn($"Skipping requirement with null data: RequirementNull={data.Requirement == null}, CategoryNull={data.Category == null}");
                    continue;
                }

                result.Add(new TenantRequirement
                {
                    RecordRequirement = data.Requirement,
                    RecordCategory = data.Category,
                    RecordCategoryRule = data.Rule ?? new RecordCategoryRule
                    {
                        Required = false,
                        IsSurpathOnly = false,
                        IsDeleted = false,
                        MetaData = "{}",
                        Name = "No Rule",
                        Notify = false,
                        Expires = false,
                        TenantId = AbpSession.TenantId ?? 0,
                        TemplateRuleId = Guid.Empty,
                        Id = Guid.Empty
                    }
                });
            }

            return result;
        }

        /// <summary>
        /// Recalculates compliance for a specific user, typically after a cohort migration or transfer.
        /// This method ensures that the user's compliance state is accurately reflected after changes
        /// to their cohort or department assignment.
        /// </summary>
        /// <param name="userId">The ID of the user to recalculate compliance for</param>
        /// <returns>The updated compliance values after recalculation</returns>
        public async Task<ComplianceValues> RecalculateUserCompliance(long userId)
        {
            try
            {
                _logger.LogInformation($"Starting compliance recalculation for user {userId}");

                // Get the user's detailed compliance values
                // This method already performs a fresh calculation based on current data
                var updatedCompliance = await GetDetailedComplianceValuesForUser(userId);

                _logger.LogInformation($"Compliance recalculation completed for user {userId}. " +
                    $"Drug: {updatedCompliance.Drug}, Background: {updatedCompliance.Background}, " +
                    $"Immunization: {updatedCompliance.Immunization}, InCompliance: {updatedCompliance.InCompliance}");

                return updatedCompliance;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error recalculating compliance for user {userId}");

                // Return a default non-compliant state if recalculation fails
                return new ComplianceValues
                {
                    UserId = userId,
                    TenantId = AbpSession.TenantId ?? 0,
                    Background = false,
                    Drug = false,
                    Immunization = false,
                    InCompliance = false
                };
            }
        }
    }
}