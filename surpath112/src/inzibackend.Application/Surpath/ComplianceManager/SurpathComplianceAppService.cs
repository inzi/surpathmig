using Abp.Application.Features;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Abp.Timing;
using Abp.UI;
using Castle.Core.Logging;
using inzibackend.Authorization;
using inzibackend.Authorization.Roles;
using inzibackend.Authorization.Users;
using inzibackend.Configuration;
using inzibackend.Features;
using inzibackend.MultiTenancy;
using inzibackend.Surpath.Dtos.Registration;
using inzibackend.Storage;
using inzibackend.Surpath.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using inzibackend.Surpath.Registration;

namespace inzibackend.Surpath
{
    [AbpAuthorize]
    public class SurpathComplianceAppService : inzibackendAppServiceBase, ISurpathComplianceAppService
    {
        private readonly IFeatureChecker _featureChecker;

        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;

        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly IRepository<RecordState, Guid> _recordStateRepository;
        private readonly IRepository<Record, Guid> _recordRepository;
        private readonly IRepository<RecordNote, Guid> _recordNoteRepository;
        private readonly IRepository<RecordCategory, Guid> _recordCategoryRepository;
        private readonly IRepository<RecordRequirement, Guid> _recordRequirementRepository;
        private readonly IRepository<User, long> _userLookUpRepository;
        private readonly IRepository<PidType, Guid> _PidTypeRepository;
        private readonly IRepository<RecordStatus, Guid> _recordStatusRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IRepository<TenantDocumentCategory, Guid> _tenantDocumentCategoryRepository;
        private readonly IRepository<TenantDocument, Guid> _tenantDocumentRepository;
        private readonly IRepository<SurpathService, Guid> _surpathServiceRepository;
        private readonly IRepository<TenantSurpathService, Guid> _tenantSurpathServiceRepository;
        private readonly IRepository<TenantDepartmentUser, Guid> _tenantDepartmentUserRepository;

        private readonly ITenantAppService _tenantAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly TenantManager _tenantManager;
        private readonly ICacheManager _cacheManager;
        private readonly IRegistrationValidationManager _registrationValidationManager;

        //private readonly IRecordStatesAppService _

        private readonly ILogger _logger;
        private string _SurpathRecordsRootFolder { get; set; }

        private const string ValidationRateLimitCacheName = "SurpathRegistrationValidationRateLimit";
        private const int ValidationRateLimitMaxAttempts = 20;
        private static readonly TimeSpan ValidationRateLimitWindow = TimeSpan.FromMinutes(1);

        private ITypedCache<string, ValidationRateLimitState> ValidationRateLimitCache =>
            _cacheManager.GetCache<string, ValidationRateLimitState>(ValidationRateLimitCacheName);

        public SurpathComplianceAppService(
            IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
            IRepository<Cohort, Guid> cohortRepository,
            IRepository<RecordState, Guid> recordStateRepository,
            IRepository<Record, Guid> lookup_recordRepository,
            IRepository<RecordCategory, Guid> lookup_recordCategoryRepository,
            IRepository<RecordRequirement, Guid> recordRequirementRepository,
            IRepository<User, long> lookup_userRepository,
            IRepository<RecordStatus, Guid> lookup_recordStatusRepository,
            IRepository<CohortUser, Guid> cohortUserRepository,
            IRepository<RecordNote, Guid> recordNoteRepository,
            IHttpContextAccessor httpContextAccessor,
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager,
            IRepository<TenantDocument, Guid> tenantDocumentRepository,
            IRepository<PidType, Guid> PidTypeRepository,
            IRepository<TenantDepartmentUser, Guid> tenantDepartmentUserRepository,
            IRepository<SurpathService, Guid> surpathServiceRepository,
            IRepository<TenantSurpathService, Guid> tenantSurpathServiceRepository,
            ITenantAppService tenantAppService,
            IFeatureChecker featureChecker,
            IUnitOfWorkManager unitOfWorkManager,
            ILogger logger,
            TenantManager tenantManager,
            ICacheManager cacheManager,
            IRegistrationValidationManager registrationValidationManager
            )
        {
            _tenantDepartmentRepository = tenantDepartmentRepository;
            _cohortRepository = cohortRepository;
            _recordStateRepository = recordStateRepository;
            _recordRepository = lookup_recordRepository;
            _recordCategoryRepository = lookup_recordCategoryRepository;
            _recordRequirementRepository = recordRequirementRepository;
            _userLookUpRepository = lookup_userRepository;
            _recordStatusRepository = lookup_recordStatusRepository;
            _cohortUserRepository = cohortUserRepository;
            _recordNoteRepository = recordNoteRepository;
            _httpContextAccessor = httpContextAccessor;
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _tenantDocumentRepository = tenantDocumentRepository;
            _PidTypeRepository = PidTypeRepository;
            _tenantDepartmentUserRepository = tenantDepartmentUserRepository;
            _tenantAppService = tenantAppService;
            _featureChecker = featureChecker;
            _surpathServiceRepository = surpathServiceRepository;
            _tenantSurpathServiceRepository = tenantSurpathServiceRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _logger = logger;
            _tenantManager = tenantManager;
            _cacheManager = cacheManager;
            _registrationValidationManager = registrationValidationManager;
        }

        public async Task<List<ComplianceCohortTotalsForViewDto>> GetTenantDeptCompliance(IQueryable<TenantDepartment> pagedAndFilteredTenantDepartments)
        {
            // get the states:
            // IMPORTANT: Only consider current (non-archived) documents for compliance evaluation
            var compliancetotals = from st in _recordStatusRepository.GetAll()
                                   join r in _recordStateRepository.GetAll().Where(rs => !rs.IsArchived) on st.Id equals r.RecordStatusId
                                   join u in _userLookUpRepository.GetAll() on r.UserId equals u.Id
                                   join cu in _cohortUserRepository.GetAll() on u.Id equals cu.UserId
                                   join c in _cohortRepository.GetAll() on cu.CohortId equals c.Id
                                   join d in pagedAndFilteredTenantDepartments on c.TenantDepartmentId equals d.Id // into dept
                                   group new { st, d, c } by new { statusid = st.Id, StatusName = st.StatusName, HtmlColor = st.HtmlColor, CSSCLass = st.CSSCLass, TenantDepartmentId = d.Id, CohortId = c.Id, CohortName = c.Name } into res
                                   select new ComplianceCohortTotalsForViewDto()
                                   {
                                       CohortId = res.Key.CohortId,
                                       CohortName = res.Key.CohortName,
                                       Count = res.Count(),
                                       CSSCLass = res.Key.CSSCLass == null ? "" : res.Key.CSSCLass,
                                       HtmlColor = res.Key.HtmlColor,
                                       Id = res.Key.statusid,
                                       StatusName = res.Key.StatusName,
                                       TenantDepartmentId = res.Key.TenantDepartmentId
                                   };

            //return compliancetotals.Cast<ComplianceCohortTotalsForViewDto>().ToList();
            return compliancetotals.ToList();
        }

        public async Task<List<ComplianceCohortTotalsForViewDto>> GetCohortCompliance(IQueryable<Cohort> pagedAndFilteredCohorts)
        {
            var comp3 = from c in pagedAndFilteredCohorts
                        join cu in _cohortUserRepository.GetAll() on c.Id equals cu.CohortId
                        join u in _userLookUpRepository.GetAll() on cu.UserId equals u.Id

                        join sts in _recordStatusRepository.GetAll() on c.TenantId equals sts.TenantId

                        join sub in (
                            from rs in _recordStateRepository.GetAll()
                            where !rs.IsArchived  // Only consider current (non-archived) documents
                            select new
                            {
                                UserId = rs.UserId,
                                RecordStatusId = rs.RecordStatusId
                            }
                            )
                        on new { UserId = u.Id, RecordStatusId = sts.Id } equals new { UserId = (long)sub.UserId, RecordStatusId = sub.RecordStatusId } into rstates
                        from states in rstates.DefaultIfEmpty()
                        group new { c, sts, states } by new { CohortId = c.Id, CohortName = c.Name, CSSCLass = sts.CSSCLass, HtmlColor = sts.HtmlColor, StatusName = sts.StatusName, statusid = sts.Id, TenantDepartmentId = c.TenantDepartmentId } into res
                        select new ComplianceCohortTotalsForViewDto()
                        {
                            CohortId = res.Key.CohortId,
                            CohortName = res.Key.CohortName,
                            Count = res.Count(),
                            CSSCLass = res.Key.CSSCLass == null ? "" : res.Key.CSSCLass,
                            HtmlColor = res.Key.HtmlColor,
                            Id = res.Key.statusid,
                            StatusName = res.Key.StatusName,
                            TenantDepartmentId = res.Key.TenantDepartmentId == null ? Guid.Empty : (Guid)res.Key.TenantDepartmentId
                        };

            return comp3.ToList();
        }

        [AbpAuthorize(AppPermissions.Pages_RecordNotes_Create)]
        public async Task AddNoteToRecord(int tenantid, bool hostonly, bool authorizedonly, bool sendnotification, Guid recordstateid, long userId, string note)
        {
            var _note = new RecordNote()
            {
                UserId = userId,
                Note = note,
                AuthorizedOnly = authorizedonly,
                Created = DateTime.UtcNow,
                NotifyUserId = null,
                HostOnly = hostonly,
                RecordStateId = recordstateid,
                TenantId = tenantid,
                SendNotification = sendnotification,
            };

            await _recordNoteRepository.InsertAsync(_note);
            return;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration, AppPermissions.Pages_Administration_Host_Dashboard, AppPermissions.Pages_CohortUsers, AppPermissions.Pages_CohortUser)]
        [UnitOfWork]
        public async Task AddSystemNoteToRecord(int tenantid, bool hostonly, bool authorizedonly, bool sendnotification, Guid recordstateid, string note)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                //var hostAdmin = _userLookUpRepository.GetAll().IgnoreQueryFilters().Where(u => u.TenantId == null && u.Name.Equals("admin", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                var hostAdmin = _userLookUpRepository.GetAll().IgnoreQueryFilters().Where(u => u.TenantId == null && u.Name.ToLower() == "admin").FirstOrDefault();
                var _note = new RecordNote()
                {
                    UserId = hostAdmin.Id,
                    Note = note,
                    AuthorizedOnly = authorizedonly,
                    Created = DateTime.UtcNow,
                    NotifyUserId = null,
                    HostOnly = hostonly,
                    RecordStateId = recordstateid,
                    TenantId = tenantid,
                    SendNotification = sendnotification,
                };

                await _recordNoteRepository.InsertAsync(_note);
                await uow.CompleteAsync();
            }
            return;
        }

        public async Task<string> GetRemoteIPAddress()
        {
            var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            return ip.MapToIPv4().ToString();
        }

        public async Task<GetRecordForViewDto> CreateNewRecord(CreateOrEditRecordDto input)
        {
            input.DateUploaded = DateTime.UtcNow;

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var record = ObjectMapper.Map<Record>(input);

            if (AbpSession.TenantId != null)
            {
                record.TenantId = (int?)AbpSession.TenantId;
            }

            await _recordRepository.InsertAsync(record);
            //record.filedata = await GetBinaryObjectFromCache(input.filedataToken);
            var storedFile = await GetBinaryObjectFromCache(input.filedataToken);
            record.filedata = storedFile.Id;
            record.filename = input.filename;
            record.metadata = storedFile.Metadata;
            record.physicalfilepath = storedFile.FileName;
            record.BinaryObjId = storedFile.Id;
            RecordCreateUpdatePostProcess(record);
            var output = new GetRecordForViewDto { Record = ObjectMapper.Map<RecordDto>(record) };
            return output;
        }

        public async Task UpdateRecordState(CreateOrEditRecordStateDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordState = ObjectMapper.Map<RecordState>(input);

            if (AbpSession.TenantId != null)
            {
                recordState.TenantId = (int?)AbpSession.TenantId;
            }
            else
            {
                // get the tenant id for
                var _cat = _recordCategoryRepository.Get((Guid)input.RecordCategoryId);
                recordState.TenantId = _cat.TenantId;
            }
            // archive old recordstate(s) for this requirement category
            var _oldRecords = _recordStateRepository.GetAll().Where(r => r.RecordCategoryId == input.RecordCategoryId && r.UserId == input.UserId).ToList();
            foreach (var _oldRecord in _oldRecords)
            {
                _oldRecord.IsArchived = true;
                await _recordStateRepository.UpdateAsync(_oldRecord);
            }
            await _recordStateRepository.UpdateAsync(recordState);
            string msg = $"Record Updated.\r\nWhen: {DateTime.Now}\r\nFrom: {await GetRemoteIPAddress()}\r\nId: {recordState.Id}\r\nrecord ID: {input.RecordId}\r\n";
            var _userId = AbpSession.ImpersonatorUserId;
            if (_userId == null) _userId = AbpSession.UserId;

            //AddNoteToRecord((int)recordState.TenantId, false, true, false, recordState.Id, (long)recordState.UserId, msg);
            await AddNoteToRecord((int)recordState.TenantId, false, true, false, recordState.Id, (long)_userId, msg);
        }

        public async Task<CreateOrEditRecordStateDto> CreateNewRecordState(CreateOrEditRecordStateDto input)
        {
            //input.DateUploaded = DateTime.UtcNow;

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordState = ObjectMapper.Map<RecordState>(input);

            if (AbpSession.TenantId != null)
            {
                recordState.TenantId = (int?)AbpSession.TenantId;
            }
            else
            {
                // get the tenant id for
                var _cat = _recordCategoryRepository.Get((Guid)input.RecordCategoryId);
                recordState.TenantId = _cat.TenantId;
            }

            // archive old recordstate(s) for this requirement category
            var _oldRecords = _recordStateRepository.GetAll().Where(r => r.RecordCategoryId == input.RecordCategoryId && r.UserId == input.UserId).ToList();
            foreach (var _oldRecord in _oldRecords)
            {
                _oldRecord.IsArchived = true;
                await _recordStateRepository.UpdateAsync(_oldRecord);
            }

            await _recordStateRepository.InsertAsync(recordState);
            input.Id = recordState.Id;
            string msg = $"New record created.\r\nWhen: {DateTime.Now}\r\nFrom: {await GetRemoteIPAddress()}\r\nId: {recordState.Id}\r\nrecord ID: {input.RecordId}\r\n";
            var _userId = AbpSession.ImpersonatorUserId;
            if (_userId == null) _userId = AbpSession.UserId;

            //AddNoteToRecord((int)recordState.TenantId, false, true, false, recordState.Id, (long)recordState.UserId, msg);
            await AddNoteToRecord((int)recordState.TenantId, false, true, false, recordState.Id, (long)_userId, msg);

            return input;
            // StateGhangeNotify(CreateOrEditRecordStateDto input)
        }

        [AbpAuthorize(AppPermissions.Pages_Records_Edit)]
        public async Task RemovefiledataFile(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var record = await _recordRepository.FirstOrDefaultAsync(input.Id);
            if (record == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!record.filedata.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _binaryObjectManager.DeleteAsync(record.filedata.Value);
            record.filedata = null;
        }

        [AbpAuthorize]
        public async Task<exdtos.PagedResultDto<RecordStateRecordCategoryLookupTableDto>> GetAllRecordCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _recordCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var recordCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            if (input.Shuffle)
            {
                Random rand = new Random();
                recordCategoryList = recordCategoryList.OrderBy(c => rand.Next()).ToList();
            }

            var lookupTableDtoList = new List<RecordStateRecordCategoryLookupTableDto>();
            foreach (var recordCategory in recordCategoryList)
            {
                lookupTableDtoList.Add(new RecordStateRecordCategoryLookupTableDto
                {
                    Id = recordCategory.Id.ToString(),
                    DisplayName = recordCategory.Name?.ToString(),
                    DisplayInstructions = recordCategory.Instructions?.ToString(),
                });
            }

            return new exdtos.PagedResultDto<RecordStateRecordCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAllowAnonymous]
        public async Task<exdtos.PagedResultDto<TenantDepartmentLookupTableDto>> GetAllTenantDeptForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _tenantDepartmentRepository.GetAll()
                //.Where(e=>e.Active==true)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Name != null && (e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter)));

            var totalCount = await query.CountAsync();

            var resultList = await query
                .PageBy(input)
                .ToListAsync();

            if (input.Shuffle)
            {
                Random rand = new Random();
                resultList = resultList.OrderBy(c => rand.Next()).ToList();
            }

            var lookupTableDtoList = new List<TenantDepartmentLookupTableDto>();
            foreach (var recordCategory in resultList)
            {
                lookupTableDtoList.Add(new TenantDepartmentLookupTableDto
                {
                    Id = recordCategory.Id.ToString(),
                    DisplayName = recordCategory.Name?.ToString(),
                });
            }

            return new exdtos.PagedResultDto<TenantDepartmentLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAllowAnonymous]
        public async Task<exdtos.PagedResultDto<TenantDepartmentLookupTableDto>> GetAllCohortForLookupTable(ComplianceGetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var query = _cohortRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Name != null && e.Name.Contains(input.Filter))
                .WhereIf(input.TenantId != null, (e => e.TenantId == input.TenantId))
                //.WhereIf((input.TenantDepartmentId!=Guid.Empty || input.TenantDepartmentId!=null), (e=>e.TenantDepartmentId==input.TenantDepartmentId || e.TenantDepartmentId==null || e.DefaultCohort==true));
                .WhereIf(input.TenantDepartmentId != null, (e => e.TenantDepartmentId == input.TenantDepartmentId || e.TenantDepartmentId == null || e.DefaultCohort == true))
                .WhereIf(input.ExcludeCohortId.HasValue, (e => e.Id != input.ExcludeCohortId));

            var totalCount = await query.CountAsync();

            var resultList = await query
                .PageBy(input)
                .ToListAsync();

            if (input.shuffle)
            {
                Random rand = new Random();
                resultList = resultList.OrderBy(c => rand.Next()).ToList();
            }

            var lookupTableDtoList = new List<TenantDepartmentLookupTableDto>();
            foreach (var recordCategory in resultList)
            {
                lookupTableDtoList.Add(new TenantDepartmentLookupTableDto
                {
                    Id = recordCategory.Id.ToString(),
                    DisplayName = recordCategory.Name?.ToString(),
                });
            }

            return new exdtos.PagedResultDto<TenantDepartmentLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<List<RecordStateRecordStatusLookupTableDto>> GetAllRecordStatusForTableDropdown(long? tenantId = null, bool isSurpathService = false)
        {
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            if (tenantId == null) tenantId = AbpSession.TenantId;

            var retval = await _recordStatusRepository.GetAll()
                .WhereIf(isSurpathService == true, e => e.TenantId == null)
                .WhereIf(isSurpathService == false, e => e.TenantId == tenantId)
                .Select(recordStatus => new RecordStateRecordStatusLookupTableDto
                {
                    Id = recordStatus.Id.ToString(),
                    DisplayName = recordStatus == null || recordStatus.StatusName == null ? "" : recordStatus.StatusName.ToString(),
                    IsDefault = recordStatus == null ? false : recordStatus.IsDefault,
                    IsSurpathServiceStatus = recordStatus == null ? false : recordStatus.IsSurpathServiceStatus,
                }).ToListAsync();

            //if (tenantId.HasValue) retval = retval.Where(r=>r.te)
            retval = retval.Where(r => r.IsSurpathServiceStatus == isSurpathService).ToList();

            if (retval.Count < 1)
            {
                // no statuses - clone from host
                // record statuses
                //var _t = _lookup_recordStatusRepository.GetAll().AsNoTracking().ToList();
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                var _recordStatuses = _recordStatusRepository.GetAll().AsNoTracking().Where(r => r.TenantId == null && r.IsSurpathServiceStatus == false).ToList();
                foreach (var recordStatus in _recordStatuses)
                {
                    recordStatus.Id = Guid.NewGuid();
                    recordStatus.TenantId = AbpSession.TenantId;
                    recordStatus.IsSurpathServiceStatus = false;

                    await _recordStatusRepository.InsertAsync(recordStatus);
                }
                await CurrentUnitOfWork.SaveChangesAsync();
                retval = await _recordStatusRepository.GetAll()
                    .WhereIf(AbpSession.TenantId != null, e => e.IsSurpathServiceStatus == false)
               .Select(recordStatus => new RecordStateRecordStatusLookupTableDto
               {
                   Id = recordStatus.Id.ToString(),
                   DisplayName = recordStatus == null || recordStatus.StatusName == null ? "" : recordStatus.StatusName.ToString(),
                   IsDefault = recordStatus == null ? false : recordStatus.IsDefault,
               }).ToListAsync();
            }
            return retval;
        }

        private async Task<BinaryObject> GetBinaryObjectFromCache(string fileToken)
        {
            if (fileToken.IsNullOrWhiteSpace())
            {
                return null;
            }

            var fileCache = _tempFileCacheManager.GetFileInfo(fileToken);

            if (fileCache == null)
            {
                throw new UserFriendlyException("There is no such file with the token: " + fileToken);
            }
            var _folder = GetDestFolder(AbpSession.TenantId, AbpSession.UserId);
            //var storedFile = new BinaryObject(AbpSession.TenantId, fileCache.File, $"{fileCache.FileName} uploaded by {AbpSession.UserId.ToString()}", true, _folder);
            var storedFile = new BinaryObject(AbpSession.TenantId, fileCache.File, $"{DateTime.Now:g} - {fileCache.FileName} uploaded by {AbpSession.UserId.ToString()}", true, _folder, fileCache.FileName, "");
            await _binaryObjectManager.SaveAsync(storedFile);

            return storedFile;
        }

        private void RecordCreateUpdatePostProcess(Record record)
        {
            if (record.TenantDocumentCategoryId != null && record.TenantDocumentCategoryId != Guid.Empty)
            {
                // Create the tenantDocument record
                var _td = _tenantDocumentRepository.GetAll().Where(td => td.RecordId == record.Id).FirstOrDefault();
                if (_td == null)
                {
                    _td = new TenantDocument()
                    {
                        TenantDocumentCategoryId = (Guid)record.TenantDocumentCategoryId,
                        TenantId = record.TenantId,
                        AuthorizedOnly = true,
                        Name = record.filename,
                        RecordId = record.Id,
                    };
                    _tenantDocumentRepository.Insert(_td);
                }
                else
                {
                    _td.RecordId = record.Id;
                }
            }
        }

        private string GetDestFolder(int? TenantId, long? UserId)
        {
            // var destfolder = $"{appFolders.SurpathRootFolder}";
            var _tenantid = TenantId == null ? "surscan" : TenantId.Value.ToString();
            _logger.Debug("GetDestFolder");
            _logger.Debug($"SurpathRecordsRootFolder = {SurpathSettings.SurpathRecordsRootFolder}");
            _logger.Debug($"_tenantid = {_tenantid}");
            _logger.Debug($" UserId = {UserId.ToString()}");

            if (string.IsNullOrEmpty(SurpathSettings.SurpathRecordsRootFolder))
            {
                _logger.Debug($"SurpathSettings.SurpathRecordsRootFolder empty, loading from appsettings");

                string _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                _logger.Debug($"_environment: {_environment}");

                var Configuration = AppConfigurations.Get(Directory.GetCurrentDirectory(), _environment, true);

                SurpathSettings.SurpathRecordsRootFolder = Configuration.GetValue<string>("Surpath:SurpathRecordsRootFolder");
                _logger.Debug($"SurpathSettings.SurpathRecordsRootFolder now: {SurpathSettings.SurpathRecordsRootFolder}");
            }

            var destFolder = Path.Combine(SurpathSettings.SurpathRecordsRootFolder, _tenantid, UserId.ToString());
            destFolder = destFolder.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            return destFolder;
        }

        public async Task<List<UserPid>> GetEmptyUserPidList()
        {
            var _tenantId = AbpSession.TenantId;
            var _PidTypes = await _PidTypeRepository.GetAll().AsNoTracking().ToListAsync();
            var userPidList = new List<UserPid>();
            foreach (var PidType in _PidTypes)
            {
                userPidList.Add(new UserPid()
                {
                    Pid = String.Empty,
                    PidTypeId = PidType.Id,
                    Validated = false,
                    TenantId = (int)_tenantId
                }); ;
            }
            return userPidList;
        }

        public async Task<List<PidType>> GetEmptyPidTypeList()
        {
            return await _PidTypeRepository.GetAll().Where(p => p.IsActive == true).AsNoTracking().ToListAsync();
        }

        public async Task<List<UserPidDto>> GetEmptyPidsList()
        {
            var _tenantId = AbpSession.TenantId;
            var _PidTypes = await _PidTypeRepository.GetAll().AsNoTracking().ToListAsync();
            var userPidList = new List<UserPidDto>();
            foreach (var PidType in _PidTypes)
            {
                userPidList.Add(new UserPidDto()
                {
                    Pid = String.Empty,
                    PidTypeId = PidType.Id,
                    Validated = false,
                    PidType = ObjectMapper.Map<PidTypeDto>(PidType)
                    //MaskPid = PidType.MaskPid,
                });
            }
            return userPidList;
        }

        [AbpAllowAnonymous]
        public async Task<PagedResultDto<CohortTenantDepartmentLookupTableDto>> GetDepartmentsForRegistration(GetAllForLookupTableInput input)
        {
            //if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _tenantDepartmentRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter) && e.Active == true
               );

            var totalCount = await query.CountAsync();

            var tenantDepartmentList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CohortTenantDepartmentLookupTableDto>();
            foreach (var tenantDepartment in tenantDepartmentList)
            {
                lookupTableDtoList.Add(new CohortTenantDepartmentLookupTableDto
                {
                    Id = tenantDepartment.Id.ToString(),
                    DisplayName = tenantDepartment.Name?.ToString()
                });
            }

            return new PagedResultDto<CohortTenantDepartmentLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAllowAnonymous]
        public async Task<bool> UsernameAvailable(UsernameAvailableDto input)
        {
            var tenantId = await ResolveTenantIdAsync(input.TenantId);
            await EnsureWithinRateLimitAsync("username", tenantId);

            if (input.UserName.IsNullOrWhiteSpace())
            {
                AuditValidationAttempt("username", tenantId, "<empty>", true, "No username supplied");
                return true;
            }

            var normalizedUserName = input.UserName.Trim().ToLowerInvariant();
            bool available;

            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                available = !await _userLookUpRepository.GetAll()
                    .Where(u => !u.IsDeleted && u.UserName != null)
                    .AnyAsync(u => u.UserName.ToLower() == normalizedUserName);
            }

            AuditValidationAttempt("username", tenantId, normalizedUserName, available);
            return available;
        }

        [AbpAllowAnonymous]
        public async Task<bool> EmailAvailable(UsernameAvailableDto input)
        {
            var tenantId = await ResolveTenantIdAsync(input.TenantId);
            await EnsureWithinRateLimitAsync("email", tenantId);

            if (input.EmailAddress.IsNullOrWhiteSpace())
            {
                AuditValidationAttempt("email", tenantId, "<empty>", true, "No email supplied");
                return true;
            }

            var normalizedEmail = input.EmailAddress.Trim().ToUpperInvariant();
            Logger.Debug($"EmailAvailable check. TenantId={tenantId}, Email={input.EmailAddress}, Normalized={normalizedEmail}");

            bool available;
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                available = !await _userLookUpRepository
                    .GetAll()
                    .Where(u => !u.IsDeleted && u.NormalizedEmailAddress != null)
                    .AnyAsync(u => u.NormalizedEmailAddress == normalizedEmail);
            }

            AuditValidationAttempt("email", tenantId, normalizedEmail, available);
            Logger.Debug($"EmailAvailable result for {normalizedEmail}: {available}");
            return available;
        }

        [AbpAllowAnonymous]
        public async Task<RegistrationValidationResultDto> ValidateRegistration(RegistrationValidationInputDto input)
        {
            var tenantId = await ResolveTenantIdAsync(input.TenantId);
            await EnsureWithinRateLimitAsync("composite", tenantId);

            var request = new RegistrationValidationRequest
            {
                TenantId = tenantId,
                EmailAddress = input.EmailAddress,
                UserName = input.UserName,
                TenantDepartmentId = input.TenantDepartmentId,
                CohortId = input.CohortId
            };

            var result = await _registrationValidationManager.ValidateAsync(request);
            var errorDetails = (result.Errors != null && result.Errors.Any()) ? result.Errors.JoinAsString("; ") : null;

            AuditValidationAttempt("composite", tenantId, input.UserName ?? input.EmailAddress, result.IsValid, errorDetails);

            return new RegistrationValidationResultDto
            {
                IsValid = result.IsValid,
                EmailAvailable = result.EmailAvailable,
                EmailError = result.EmailError,
                UsernameAvailable = result.UsernameAvailable,
                UsernameError = result.UsernameError,
                DepartmentValid = result.DepartmentValid,
                DepartmentError = result.DepartmentError,
                CohortValid = result.CohortValid,
                CohortError = result.CohortError,
                Errors = result.Errors.ToList()
            };
        }

        private async Task<int> ResolveTenantIdAsync(int? tenantId)
        {
            if (AbpSession.TenantId.HasValue)
            {
                if (tenantId.HasValue && tenantId.Value != AbpSession.TenantId.Value)
                {
                    throw new UserFriendlyException(L("TenantContextMismatch"));
                }

                return AbpSession.TenantId.Value;
            }

            if (!tenantId.HasValue)
            {
                throw new UserFriendlyException(L("TenantNotSpecified"));
            }

            var tenant = await _tenantManager.FindByIdAsync(tenantId.Value);
            if (tenant == null || tenant.IsDeleted)
            {
                throw new UserFriendlyException(L("TenantNotSpecified"));
            }

            return tenant.Id;
        }

        private async Task EnsureWithinRateLimitAsync(string endpoint, int tenantId)
        {
            var fingerprint = GetClientFingerprint();
            var cacheKey = $"{tenantId}:{endpoint}:{fingerprint}";
            var now = Clock.Now;
            var cache = ValidationRateLimitCache;
            var entry = await cache.GetOrDefaultAsync(cacheKey);

            if (entry == null || now - entry.WindowStart >= ValidationRateLimitWindow)
            {
                entry = new ValidationRateLimitState
                {
                    WindowStart = now,
                    Count = 1
                };
            }
            else
            {
                entry.Count += 1;
                if (entry.Count > ValidationRateLimitMaxAttempts)
                {
                    _logger.Warn($"Validation rate limit exceeded. Endpoint={endpoint}, TenantId={tenantId}, Fingerprint={fingerprint}");
                    throw new UserFriendlyException(L("TooManyValidationAttempts"));
                }
            }

            await cache.SetAsync(cacheKey, entry, ValidationRateLimitWindow);
        }

        private void AuditValidationAttempt(string endpoint, int tenantId, string subject, bool succeeded, string additionalInfo = null)
        {
            var fingerprint = GetClientFingerprint();
            _logger.Info($"Registration validation audit -> Endpoint={endpoint}, TenantId={tenantId}, Subject={subject}, Result={(succeeded ? "Available" : "Unavailable")}, Fingerprint={fingerprint}, Info={additionalInfo}");
        }

        private string GetClientFingerprint()
        {
            var context = _httpContextAccessor.HttpContext;
            var ipAddress = context?.Connection?.RemoteIpAddress?.ToString() ?? "unknown-ip";
            var userAgent = context?.Request?.Headers["User-Agent"].ToString() ?? "unknown-agent";
            return $"{ipAddress}|{userAgent}".ToMd5();
        }

        private class ValidationRateLimitState
        {
            public DateTime WindowStart { get; set; }
            public int Count { get; set; }
        }

        #region CreateRequirement

        public async Task<CreateOrEditRecordRequirementDto> CreateEditRequirement(CreateEditRecordRequirementDto input)
        {
            var _CreateOrEditRecordRequirementDto = new CreateOrEditRecordRequirementDto();

            var _cats = input.CreateOrEditRecordCategories;
            var _req = input.CreateOrEditRecordRequirement;
            RecordRequirement recordRequirement;
            if (_req.Id == null)
            {
                recordRequirement = ObjectMapper.Map<RecordRequirement>(_req);

                if (AbpSession.TenantId != null)
                {
                    recordRequirement.TenantId = (int?)AbpSession.TenantId;
                }

                await _recordRequirementRepository.InsertAsync(recordRequirement);
                _req.Id = recordRequirement.Id;
            }
            else
            {
                recordRequirement = await _recordRequirementRepository.FirstOrDefaultAsync((Guid)_req.Id);
                ObjectMapper.Map(_req, recordRequirement);
            }
            _CreateOrEditRecordRequirementDto = ObjectMapper.Map<CreateOrEditRecordRequirementDto>(recordRequirement);

            foreach (var _cat in _cats)
            {
                _cat.RecordRequirementId = _req.Id;
                if (_cat.Id == null)
                {
                    var recordCategory = ObjectMapper.Map<RecordCategory>(_cat);

                    if (AbpSession.TenantId != null)
                    {
                        recordCategory.TenantId = (int?)AbpSession.TenantId;
                    }

                    await _recordCategoryRepository.InsertAsync(recordCategory);
                }
                else
                {
                    var recordCategory = await _recordCategoryRepository.FirstOrDefaultAsync((Guid)_cat.Id);
                    ObjectMapper.Map(_cat, recordCategory);
                }
            }

            // remove any cats not in the input
            var _redcats = _recordCategoryRepository.GetAll().Where(r => r.RecordRequirementId == _req.Id).ToList();

            foreach (var _redcat in _redcats)
            {
                if (!_cats.Exists(c => c.Id == _redcat.Id))
                {
                    await _recordCategoryRepository.DeleteAsync(_redcat.Id);
                }
            }

            return _CreateOrEditRecordRequirementDto;
        }

        [AbpAuthorize(AppPermissions.Pages_RecordCategories_Edit)]
        public async Task<List<GetRecordCategoryForEditOutput>> GetRecordCategoriesForRequirementForEdit(EntityDto<Guid> entityId)
        {
            List<GetRecordCategoryForEditOutput> result = new List<GetRecordCategoryForEditOutput>();

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordCategories = _recordCategoryRepository.GetAll()
                .Include(rc => rc.RecordCategoryRuleFk)
                .Include(rc => rc.RecordRequirementFk)
                .AsNoTracking()
                .Where(rc => rc.RecordRequirementId == entityId.Id).ToList();

            var _counts = (from rc in recordCategories
                           join rs in _recordStateRepository.GetAll().Where(r => !r.IsArchived) on rc.Id equals rs.RecordCategoryId
                           group rs by new { rs.Id, rs.RecordCategoryId } into g
                           select new
                           {
                               id = g.Key.RecordCategoryId,
                               count = g.Count()
                           }).ToList();

            foreach (var recordCategory in recordCategories)
            {
                var output = new GetRecordCategoryForEditOutput { RecordCategory = ObjectMapper.Map<CreateOrEditRecordCategoryDto>(recordCategory) };
                output.RecordCategoryRecordStateCount = _counts.Count(c => c.id == output.RecordCategory.Id);
                if (output.RecordCategory.RecordRequirementId != null)
                {
                    var _lookupRecordRequirement = await _recordRequirementRepository.FirstOrDefaultAsync((Guid)output.RecordCategory.RecordRequirementId);
                    output.RecordRequirementName = _lookupRecordRequirement?.Name?.ToString();
                }

                if (output.RecordCategory.RecordCategoryRuleId != null)
                {
                    var _lookupRecordCategoryRule = await _recordRequirementRepository.FirstOrDefaultAsync((Guid)output.RecordCategory.RecordCategoryRuleId);
                    output.RecordCategoryRuleName = _lookupRecordCategoryRule?.Name?.ToString();
                }
                result.Add(output);
            }
            return result;
        }

        #endregion CreateRequirement
    }
}
