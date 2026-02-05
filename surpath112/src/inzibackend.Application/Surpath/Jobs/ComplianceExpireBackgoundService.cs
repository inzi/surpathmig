using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using inzibackend.Authorization.Roles;
using inzibackend.Configuration;
using inzibackend.Debugging;
using inzibackend.MultiTenancy;
using inzibackend.Notifications;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Abp;
using Abp.Authorization;

using Abp.Authorization.Users;
using Abp.Configuration;

using Abp.Domain.Repositories;

using Abp.Domain.Uow;

using Abp.Localization;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Threading;

using Abp.UI;

using Abp.Zero.Configuration;

using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using inzibackend.Authorization.Roles;
using inzibackend.Configuration;

using inzibackend.Security;
using inzibackend.Authorization.Users;
using inzibackend.Authorization;
using Abp.Application.Features;
using inzibackend.Features;
using Abp.Collections.Extensions;
using inzibackend.Surpath.Compliance;
using inzibackend.Surpath.ComplianceManager;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using AutoMapper;
using inzibackend.Surpath.Dtos;
using Abp.AutoMapper;
using Abp.ObjectMapping;
using NPOI.OpenXmlFormats;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using inzibackend.Surpath.MetaData;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using Abp.EntityFrameworkCore;
using inzibackend.Url;
using SixLabors.ImageSharp.Formats.Tiff.Compression.Decompressors;
using Abp.EntityFrameworkCore.Repositories;

namespace inzibackend.Surpath.Jobs
{
    public class ComplianceExpireBackgoundService : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IFeatureChecker _featureChecker;

        private readonly ILogger<ComplianceExpireBackgoundService> _logger;
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
        private readonly Abp.ObjectMapping.IObjectMapper _objectMapper;
        private readonly UserManager _userManager;
        private readonly IRecordStatusesAppService _recordStatusesAppService;
        private readonly IRecordStatesAppService _recordStatesAppService;
        private readonly IRepository<User, long> _userLookUpRepository;
        private readonly IAppNotifier _appNotifier;
        private readonly IAppUrlService _appUrlService;
        private readonly IUserEmailer _userEmailer;
        public bool IsEnabled { get; } = true;

        private const int CheckPeriodAsMilliseconds = 1 * 1000 * 60 * 60 * 12; // 12 hours

        public ComplianceExpireBackgoundService(
            AbpTimer timer,
            ILogger<ComplianceExpireBackgoundService> logger,
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
            IFeatureChecker featureChecker,
            IPermissionChecker permissionChecker,
            UserManager userManager,
            IRecordStatusesAppService recordStatusesAppService,
            IRecordStatesAppService recordStatesAppService,
            Abp.ObjectMapping.IObjectMapper objectMapper,
            IRepository<User, long> lookup_userRepository,
            IAppNotifier appNotifier,
            IAppUrlService appUrlService,
            IUserEmailer userEmailer)
            : base(timer)
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
            _recordStatusRepository = recordStatusRepository;
            _recordRepository = recordRepository;
            _permissionChecker = permissionChecker;
            _unitOfWorkManager = unitOfWorkManager;
            _recordStatusesAppService = recordStatusesAppService;
            _recordStatesAppService = recordStatesAppService;
            _objectMapper = objectMapper;

            LocalizationSourceName = inzibackendConsts.LocalizationSourceName;

            Timer.Period = CheckPeriodAsMilliseconds;
            Timer.RunOnStart = true;
            _userLookUpRepository = lookup_userRepository;
            _appNotifier = appNotifier;
            _appUrlService = appUrlService;
            _userEmailer = userEmailer;
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            if (!IsEnabled)
            {
                return;
            }

            try
            {
                // Set the record status list
                //_recordStatuses = _recordStatusRepository.GetAll().AsNoTracking().ToList();
                _logger.LogInformation($"ComplianceExpireBackgoundService DoWork() called.");
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                // get all the warn rules
                var _warnRules = _RecordCategoryRuleRepository.GetAll().AsNoTracking().Where(x => x.Notify == true).ToList();

                var _warndays = _warnRules
                    .Where(x => x.WarnDaysBeforeFinal > 0 || x.WarnDaysBeforeSecond > 0 || x.WarnDaysBeforeFirst > 0)
                    .Select(x =>
                        new WarnRuleDays()
                        {
                            RecordCategoryRuleId = x.Id,
                            TenantId = (int)x.TenantId,
                            WarnDaysBeforeFirst = x.WarnDaysBeforeFirst,
                            WarnDaysBeforeSecond = x.WarnDaysBeforeSecond,
                            WarnDaysBeforeFinal = x.WarnDaysBeforeFinal
                        }
                ).Distinct().ToList();

                var _tot = _warndays.Count();
                var _count = 0;
                // do warnings
                foreach (var _warnrule in _warndays)
                {
                    _count++;
                    _logger.LogInformation($"{_count}/{_tot} Processing warnings for {_warnrule.TenantId}");
                    Warn(_warnrule).GetAwaiter().GetResult();
                }

                _count = 0;
                // do expirations
                foreach (var _warnrule in _warndays)
                {
                    _count++;
                    _logger.LogInformation($"{_count}/{_tot} Processing expires for {_warnrule.TenantId}");
                    Expired(_warnrule).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("BACKGROUND JOB ERROR", ex);
                //throw;
            }
        }

        private async Task Expired(WarnRuleDays warnRuleDays)
        {
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            //select* from recordstates rs
            //left outer join records r on rs.RecordId = r.id
            //left outer join recordcategories rc on rs.RecordCategoryId = rc.id
            //left outer join recordcategoryrules rcr on rc.RecordCategoryRuleId = rcr.id
            //left outer join recordstatuses rst on rs.RecordStatusId = rst.id
            //where r.ExpirationDate > Now() and rcr.Expires = 1 and rst.ComplianceImpact = 1;

            // Find all records with an expiration date that is past now and join with category and rule
            var _records = await GetRecordsForExpired(warnRuleDays);

            if (!_records.Any())
            {
                return;
            }

            var _tenantid = warnRuleDays.TenantId;

            _logger.LogInformation($"ComplianceExpireBackgoundService Checking {_records.Count()} expired records for {_tenantid}");

            // get a list of all tenant default record statuses for fallback
            var _recordStatuses = _recordStatusRepository.GetAll().AsNoTracking().Where(x => x.IsDefault == true && x.TenantId == _tenantid).ToList();
            var _defaultStatus = _recordStatuses.Where(x => x.TenantId == _tenantid).FirstOrDefault();
            var _tot = _records.Count();
            var _count = 0;
            foreach (var _recordState in _records)
            {
                _count++;
                var _record = _recordState.RecordFk;
                // process only records that need to be expired
                var _metadataobj = GetMetaDataObject(_record.metadata);
                if (_metadataobj.MetaDataNotifications.ExpiredNotificationSent == true)
                {
                    continue;
                }

                // First try to get the configured expired status from the rule
                var statusId = _recordState.RecordCategoryFk?.RecordCategoryRuleFk?.ExpiredStatusId;

                // If no expired status configured, fall back to tenant default
                if (!statusId.HasValue)
                {
                    if (_defaultStatus != null)
                    {
                        statusId = _defaultStatus.Id;
                    }
                }

                if (_metadataobj.MetaDataNotifications.ExpiredNotificationSent == false)
                {
                    if (_recordState.RecordStatusId == _defaultStatus.Id) continue;
                }

                if (statusId.HasValue)
                {
                    if (_recordState.RecordStatusId == statusId) continue;

                    _logger.LogInformation($"{_count}/{_tot} ComplianceExpireBackgoundService Expiring record {_record.Id}, setting status to {statusId.Value} from {_recordState.RecordStatusId}, expired {_recordState.RecordFk.ExpirationDate} ");
                    // update the record state to the configured or default status
                    using (var uow = _unitOfWorkManager.Begin())
                    {
                        var freshRecordState = await _recordStateRepository.GetAsync(_recordState.Id);
                        freshRecordState.RecordStatusId = statusId.Value;
                        await _recordStateRepository.UpdateAsync(freshRecordState);
                        await uow.CompleteAsync();
                    }

                    StateGhangeNotify(new Dtos.CreateOrEditRecordStateDto()
                    {
                        Id = _record.Id,
                        State = EnumRecordState.NeedsApproval,
                        RecordStatusId = statusId.Value,
                        RecordId = _record.Id,
                        RecordCategoryDto = _objectMapper.Map<RecordCategoryDto>(_recordState.RecordCategoryFk),
                        RecordDto = _objectMapper.Map<RecordDto>(_recordState.RecordFk),
                        RecordCategoryId = _recordState.RecordCategoryId,
                        UserId = _recordState.UserId,
                        Notes = "Record has expired"
                    }, "expired").GetAwaiter().GetResult();

                    // update the metadata
                    _metadataobj.MetaDataNotifications.ExpiredNotificationSent = true;
                    _metadataobj.MetaDataNotifications.ExpiredNotification = DateTime.Now;

                    var _trackedRecord = _recordRepository.GetAll().Where(x => x.Id == _record.Id).FirstOrDefault();
                    _trackedRecord.metadata = JsonConvert.SerializeObject(_metadataobj);
                    _recordRepository.Update(_trackedRecord);
                    CurrentUnitOfWork.SaveChanges();
                }
                else
                {
                    _logger.LogCritical($"[EXPIRED] NO DEFAULT RECORD STATUS FOUND FOR TENANT ID {_tenantid}");
                }
            }
        }

        //private async Task Warn(WarnRuleDays warnRuleDays)
        //{
        //    CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

        //    var _records = _recordStateRepository.GetAll().AsNoTracking()
        //        .Include(x => x.RecordFk)
        //        .Include(x => x.RecordStatusFk)
        //        .Include(x => x.RecordCategoryFk)
        //            .ThenInclude(rc => rc.RecordCategoryRuleFk)
        //        //.Where(x => x.State == EnumRecordState.Approved && x.RecordFk.ExpirationDate > DateTime.Now)
        //        .Where(x =>
        //            x.RecordStatusFk.ComplianceImpact == EnumStatusComplianceImpact.Compliant &&
        //            x.RecordFk.ExpirationDate >= DateTime.Now.AddDays(-warnRuleDays.WarnDaysBeforeFirst) &&
        //            x.RecordFk.ExpirationDate < DateTime.Now &&
        //            x.RecordCategoryFk.RecordCategoryRuleFk.Expires == true)
        //        .ToList();

        //    if (!_records.Any())
        //    {
        //        return;
        //    }

        //    var _tenantStatuses = _recordStatuses.Where(x => x.TenantId == warnRuleDays.TenantId).ToList();

        //    // Get tenant's default status for fallback
        //    var defaultStatus = _recordStatusRepository.GetAll().AsNoTracking()
        //        .Where(x => x.IsDefault == true && x.TenantId == warnRuleDays.TenantId)
        //        .FirstOrDefault();

        //    if (defaultStatus == null)
        //    {
        //        _logger.LogCritical("NO DEFAULT RECORD STATUS FOUND FOR TENANT ID " + warnRuleDays.TenantId.ToString());
        //        return;
        //    }

        //    foreach (var _recordState in _records)
        //    {
        //        var _record = _recordState.RecordFk;
        //        var rule = _recordState.RecordCategoryFk?.RecordCategoryRuleFk;
        //        if (rule == null) continue;

        //        var _metadataobj = GetMetaDataObject(_record.metadata);
        //        var _metaDataNotifications = _metadataobj.MetaDataNotifications;

        //        // Skip records with null expiration dates
        //        if (!_record.ExpirationDate.HasValue) continue;

        //        // Calculate days until expiration
        //        var daysUntilExpiration = (int)(_record.ExpirationDate.Value - DateTime.Now).TotalDays;

        //        bool metadataModified = false;

        //        // Check warnings in order: Final, Second, First
        //        if (!_metaDataNotifications.WarnedDaysBeforeFinal
        //            && daysUntilExpiration <= warnRuleDays.WarnDaysBeforeFinal
        //            && rule.FinalWarnStatusId.HasValue)
        //        {
        //            await SendWarning(_recordState, rule.FinalWarnStatusId.Value, "final", warnRuleDays.WarnDaysBeforeFinal);
        //            _metaDataNotifications.WarnedDaysBeforeFinal = true;
        //            _metaDataNotifications.WarnDaysBeforeFinal = DateTime.Now;
        //            metadataModified = true;
        //        }
        //        else if (!_metaDataNotifications.WarnedDaysBeforeSecond
        //            && daysUntilExpiration <= warnRuleDays.WarnDaysBeforeSecond
        //            && rule.SecondWarnStatusId.HasValue)
        //        {
        //            await SendWarning(_recordState, rule.SecondWarnStatusId.Value, "second", warnRuleDays.WarnDaysBeforeSecond);
        //            _metaDataNotifications.WarnedDaysBeforeSecond = true;
        //            _metaDataNotifications.WarnDaysBeforeSecond = DateTime.Now;
        //            metadataModified = true;
        //        }
        //        else if (!_metaDataNotifications.WarnedDaysBeforeFirst
        //            && daysUntilExpiration <= warnRuleDays.WarnDaysBeforeFirst
        //            && rule.FirstWarnStatusId.HasValue)
        //        {
        //            await SendWarning(_recordState, rule.FirstWarnStatusId.Value, "first", warnRuleDays.WarnDaysBeforeFirst);
        //            _metaDataNotifications.WarnedDaysBeforeFirst = true;
        //            _metaDataNotifications.WarnDaysBeforeFirst = DateTime.Now;
        //            metadataModified = true;
        //        }

        //        // Update metadata if any warnings were sent
        //        if (metadataModified)
        //        {
        //            var _trackedRecord = _recordRepository.GetAll().Where(x => x.Id == _record.Id).FirstOrDefault();
        //            _trackedRecord.metadata = JsonConvert.SerializeObject(_metadataobj);
        //            _recordRepository.Update(_trackedRecord);
        //            CurrentUnitOfWork.SaveChanges();
        //        }
        //    }
        //}

        private async Task Warn(WarnRuleDays warnRuleDays)
        {
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var records = await GetRecordsForWarning(warnRuleDays);

            if (!records.Any())
            {
                return;
            }

            var defaultStatus = await GetDefaultStatusForTenant(warnRuleDays.TenantId);

            if (defaultStatus == null)
            {
                _logger.LogCritical("NO DEFAULT RECORD STATUS FOUND FOR TENANT ID " + warnRuleDays.TenantId);
                return;
            }

            foreach (var recordState in records)
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    await ProcessRecordStateForWarning(recordState, warnRuleDays, defaultStatus);
                    await uow.CompleteAsync();
                }
            }
        }

        private async Task<List<RecordState>> GetRecordsForExpired(WarnRuleDays warnRuleDays)
        {
            return await _recordStateRepository.GetAll().AsNoTracking()
                .Include(x => x.RecordFk)
                .Include(x => x.RecordStatusFk)
                .Include(x => x.RecordCategoryFk)
                    .ThenInclude(rc => rc.RecordCategoryRuleFk)
                .Where(x =>
                    x.TenantId == warnRuleDays.TenantId &&
                    x.RecordCategoryFk.RecordCategoryRuleFk.Id == warnRuleDays.RecordCategoryRuleId &&
                    x.RecordStatusFk.ComplianceImpact == EnumStatusComplianceImpact.Compliant &&
                    x.RecordFk.ExpirationDate < DateTime.Now &&
                    x.RecordCategoryFk.RecordCategoryRuleFk.Expires == true &&
                    !x.IsArchived)  // Only process current (non-archived) documents
                .ToListAsync();
        }

        private async Task<List<RecordState>> GetRecordsForWarning(WarnRuleDays warnRuleDays)
        {
            return await _recordStateRepository.GetAll().AsNoTracking()
                .Include(x => x.RecordFk)
                .Include(x => x.RecordStatusFk)
                .Include(x => x.RecordCategoryFk)
                    .ThenInclude(rc => rc.RecordCategoryRuleFk)
                .Where(x =>
                    x.TenantId == warnRuleDays.TenantId &&
                    x.RecordCategoryFk.RecordCategoryRuleFk.Id == warnRuleDays.RecordCategoryRuleId &&
                    x.RecordStatusFk.ComplianceImpact == EnumStatusComplianceImpact.Compliant &&
                    x.RecordFk.ExpirationDate <= DateTime.Now.AddDays(warnRuleDays.WarnDaysBeforeFirst) &&
                    x.RecordFk.ExpirationDate > DateTime.Now &&
                    x.RecordCategoryFk.RecordCategoryRuleFk.Expires == true &&
                    !x.IsArchived)  // Only process current (non-archived) documents
                .ToListAsync();
        }

        private async Task<RecordStatus> GetDefaultStatusForTenant(int tenantId)
        {
            return await _recordStatusRepository.GetAll().AsNoTracking()
                .Where(x => x.IsDefault == true && x.TenantId == tenantId)
                .FirstOrDefaultAsync();
        }

        private async Task ProcessRecordStateForWarning(RecordState recordState, WarnRuleDays warnRuleDays, RecordStatus defaultStatus)
        {
            var record = recordState.RecordFk;
            var rule = recordState.RecordCategoryFk?.RecordCategoryRuleFk;
            if (rule == null) return;

            var metadataObj = GetMetaDataObject(record.metadata);
            var metaDataNotifications = metadataObj.MetaDataNotifications;

            if (!record.ExpirationDate.HasValue) return;

            var daysUntilExpiration = (int)(record.ExpirationDate.Value - DateTime.Now).TotalDays;

            bool metadataModified = false;

            if (metaDataNotifications.WarnedDaysBeforeFinal == false && await CheckAndSendWarning(recordState, rule, metaDataNotifications, daysUntilExpiration, warnRuleDays.WarnDaysBeforeFinal, rule.FinalWarnStatusId, "final"))
            {
                _logger.LogInformation($"ComplianceExpireBackgoundService Sending final warning for record {record.Id}");

                metaDataNotifications.WarnDaysBeforeFinal = DateTime.Now;
                metaDataNotifications.WarnDaysBeforeSecond = DateTime.Now;
                metaDataNotifications.WarnDaysBeforeFirst = DateTime.Now;
                metaDataNotifications.WarnedDaysBeforeFirst = true;
                metaDataNotifications.WarnedDaysBeforeSecond = true;
                metaDataNotifications.WarnedDaysBeforeFinal = true;
                metadataModified = true;
            }
            else if (metaDataNotifications.WarnedDaysBeforeSecond == false && await CheckAndSendWarning(recordState, rule, metaDataNotifications, daysUntilExpiration, warnRuleDays.WarnDaysBeforeSecond, rule.SecondWarnStatusId, "second"))
            {
                _logger.LogInformation($"ComplianceExpireBackgoundService Sending second warning for record {record.Id}");
                metaDataNotifications.WarnDaysBeforeSecond = DateTime.Now;
                metaDataNotifications.WarnDaysBeforeFirst = DateTime.Now;
                metaDataNotifications.WarnedDaysBeforeFirst = true;
                metaDataNotifications.WarnedDaysBeforeSecond = true;
                metadataModified = true;
            }
            else if (metaDataNotifications.WarnedDaysBeforeFirst == false && await CheckAndSendWarning(recordState, rule, metaDataNotifications, daysUntilExpiration, warnRuleDays.WarnDaysBeforeFirst, rule.FirstWarnStatusId, "first"))
            {
                _logger.LogInformation($"ComplianceExpireBackgoundService Sending first warning for record {record.Id}");
                metaDataNotifications.WarnedDaysBeforeFirst = true;
                metaDataNotifications.WarnDaysBeforeFirst = DateTime.Now;
                metadataModified = true;
            }

            if (metadataModified)
            {
                _logger.LogInformation($"ComplianceExpireBackgoundService Updating metadata for record {record.Id}");
                await UpdateRecordMetadata(record, metadataObj);
            }
        }

        private async Task<bool> CheckAndSendWarning(RecordState recordState, RecordCategoryRule rule, MetaDataNotifications metaDataNotifications, int daysUntilExpiration, int warnDaysBefore, Guid? statusId, string warningType)
        {
            //bool alreadyWarned = false;

            //switch (warningType)
            //{
            //    case "final":
            //        alreadyWarned = !metaDataNotifications.WarnedDaysBeforeFinal;
            //        break;

            //    case "second":
            //        alreadyWarned = !metaDataNotifications.WarnedDaysBeforeSecond;
            //        break;

            //    case "first":
            //        alreadyWarned = !metaDataNotifications.WarnedDaysBeforeFirst;
            //        break;
            //}
            //if (alreadyWarned) return false;

            if (daysUntilExpiration <= warnDaysBefore)
            {
                if (statusId.HasValue)
                {
                    using (var uow = _unitOfWorkManager.Begin())
                    {
                        // Get a fresh instance of the record state
                        var freshRecordState = await _recordStateRepository.GetAsync(recordState.Id);
                        freshRecordState.RecordStatusId = statusId.Value;
                        await _recordStateRepository.UpdateAsync(freshRecordState);
                        await uow.CompleteAsync();
                    }
                }

                await SendWarning(recordState, statusId, warningType, warnDaysBefore);
                return true;
            }
            return false;
        }

        private async Task SendWarning(RecordState recordState, Guid? statusId, string warningType, int daysBeforeExpiration)
        {
            _logger.LogInformation($"ComplianceExpireBackgoundService Sending {warningType} warning for record {recordState.RecordFk.Id} to user {recordState.UserId}, tenant: {recordState.TenantId}");

            await StateGhangeNotify(new Dtos.CreateOrEditRecordStateDto()
            {
                Id = recordState.RecordFk.Id,
                State = recordState.State,
                RecordStatusId = statusId ?? recordState.RecordStatusId,
                RecordId = recordState.RecordFk.Id,
                RecordCategoryDto = _objectMapper.Map<RecordCategoryDto>(recordState.RecordCategoryFk),
                RecordDto = _objectMapper.Map<RecordDto>(recordState.RecordFk),
                RecordCategoryId = recordState.RecordCategoryId,
                UserId = recordState.UserId,
                Notes = $"Record will expire in {daysBeforeExpiration} days!"
            }, warningType);
        }

        private async Task UpdateRecordMetadata(Record record, MetaDataObject metadataObj)
        {
            var trackedRecord = await _recordRepository.GetAll().Where(x => x.Id == record.Id).FirstOrDefaultAsync();
            trackedRecord.metadata = JsonConvert.SerializeObject(metadataObj);
            _recordRepository.Update(trackedRecord);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        private async Task StateGhangeNotify(Dtos.CreateOrEditRecordStateDto input, string warning = "")
        {
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            // Switching to host is necessary for single tenant mode.
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                var _info = from u in _userLookUpRepository.GetAll().AsNoTracking().Where(u => u.Id == (long)input.UserId)
                            join chu in _CohortUserRepository.GetAll().AsNoTracking() on u.Id equals chu.UserId
                            join rs in _recordStateRepository.GetAll().AsNoTracking().Where(r => r.Id == input.Id) on u.Id equals rs.UserId
                            join st in _recordStatusRepository.GetAll().AsNoTracking() on rs.RecordStatusId equals st.Id
                            join rc in _RecordCategoryRepository.GetAll().AsNoTracking() on rs.RecordCategoryId equals rc.Id
                            join req in _RecordRequirementRepository.GetAll().AsNoTracking() on rc.RecordRequirementId equals req.Id
                            join rule in _RecordCategoryRuleRepository.GetAll().AsNoTracking() on rc.RecordCategoryRuleId equals rule.Id
                            join r in _recordRepository.GetAll().AsNoTracking() on rs.RecordId equals r.Id
                            select new
                            {
                                user = u,
                                cohortUser = chu,
                                recordState = rs,
                                recordStatus = st,
                                recordCategory = rc,
                                recordCategoryRule = rule,
                                recordRequirement = req,
                                record = r
                            };

                var thisInfo = _info.FirstOrDefault();

                if (thisInfo != null)
                {
                    if (thisInfo.recordCategoryRule.Notify == true)
                    {
                        //await _appNotifier.RecordStateChanged(
                        //       thisInfo.user.ToUserIdentifier(),
                        //       new LocalizableString(
                        //           AppNotificationNames.RecordStateChanged,
                        //           inzibackendConsts.LocalizationSourceName
                        //       ),
                        //       null,
                        //       Abp.Notifications.NotificationSeverity.Warn);
                        var LocString = AppNotificationNames.RecordStateExpirationWarning;
                        switch (warning)
                        {
                            case "first":
                                LocString = AppNotificationNames.RecordStateExpirationFirstWarning;
                                break;

                            case "second":
                                LocString = AppNotificationNames.RecordStateExpirationSecondWarning;
                                break;

                            case "final":
                                LocString = AppNotificationNames.RecordStateExpirationFinalWarning;
                                break;

                            case "expired":
                                LocString = AppNotificationNames.RecordStateExpirationExpired;
                                break;

                            default:
                                break;
                        }
                        _logger.LogInformation($"ComplianceExpireBackgoundService Notifying {thisInfo.user.FullName} ({thisInfo.user.EmailAddress}) for record {thisInfo.record.Id} with {LocString}");
                        await _appNotifier.RecordStateExpirationWarning(
                               thisInfo.user.ToUserIdentifier(),
                               new LocalizableString(
                                   LocString,
                                   inzibackendConsts.LocalizationSourceName
                               ),
                               null,
                               Abp.Notifications.NotificationSeverity.Warn);
                    }
                    if (thisInfo.recordState.State == EnumRecordState.Rejected)
                    {
                        if (thisInfo.cohortUser != null)
                        {
                            var link = _appUrlService.CreateViewCohortUserUrlFormat(thisInfo.user.TenantId);
                            link = link.Replace("{cohortUserId}", thisInfo.cohortUser.Id.ToString());
                            await _userEmailer.SendEmailForComplianceRelatedNotification(thisInfo.user, link);
                        }
                    }
                }
            }
        }

        private MetaDataObject GetMetaDataObject(string metadatastring)
        {
            try
            {
                if (!metadatastring.IsJson())
                {
                    return new MetaDataObject();
                }
                var metadataObject = JsonConvert.DeserializeObject<MetaDataObject>(metadatastring);

                return metadataObject;
            }
            catch (Exception ex)
            {
                _logger.LogError("metadata error in ComplianceExpireBackgoundService", ex);
                return new MetaDataObject();
            }
        }
    }

    public class WarnRuleDays
    {
        public Guid RecordCategoryRuleId { get; set; }
        public int TenantId { get; set; }
        public int WarnDaysBeforeFirst { get; set; }
        public int WarnDaysBeforeSecond { get; set; }
        public int WarnDaysBeforeFinal { get; set; }
    }
}