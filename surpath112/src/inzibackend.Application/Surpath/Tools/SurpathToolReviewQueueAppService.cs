using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Core.Logging;
using inzibackend.Authorization;
using inzibackend.Authorization.Users;
using inzibackend.Configuration;
using inzibackend.Storage;
using inzibackend.Surpath.Compliance;
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

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Surpath_Administration_Surpath_Compliance_Review)]

    public class SurpathToolReviewQueueAppService : inzibackendAppServiceBase, ITransientDependency, ISurpathToolReviewQueueAppService
    {
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;

        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly IRepository<RecordState, Guid> _recordStateRepository;
        private readonly IRepository<Record, Guid> _recordLookUpRepository;
        private readonly IRepository<RecordNote, Guid> _recordNoteRepository;
        private readonly IRepository<RecordCategory, Guid> _recordCategoryLookUpRepository;
        private readonly IRepository<User, long> _userLookUpRepository;
        private readonly IRepository<PidType, Guid> _PidTypeRepository;
        private readonly IRepository<RecordStatus, Guid> _recordStatusLookUpRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IRepository<TenantDocumentCategory, Guid> _tenantDocumentCategoryLookUpRepository;
        private readonly IRepository<TenantDocument, Guid> _tenantDocumentRepository;
        private readonly ILogger _logger;
        private readonly SurpathManager _surpathManager;

        private string _SurpathRecordsRootFolder { get; set; }


        public SurpathToolReviewQueueAppService(
            IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
            IRepository<Cohort, Guid> cohortRepository,
            IRepository<RecordState, Guid> recordStateRepository,
            IRepository<Record, Guid> lookup_recordRepository,
            IRepository<RecordCategory, Guid> lookup_recordCategoryRepository,
            IRepository<User, long> lookup_userRepository,
            IRepository<RecordStatus, Guid> lookup_recordStatusRepository,
            IRepository<CohortUser, Guid> cohortUserRepository,
            IRepository<RecordNote, Guid> recordNoteRepository,
            IHttpContextAccessor httpContextAccessor,
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager,
            IRepository<TenantDocument, Guid> tenantDocumentRepository,
            IRepository<PidType, Guid> PidTypeRepository,
            ILogger logger,
            SurpathManager surpathManager
            )
        {
            _tenantDepartmentRepository = tenantDepartmentRepository;
            _cohortRepository = cohortRepository;
            _recordStateRepository = recordStateRepository;
            _recordLookUpRepository = lookup_recordRepository;
            _recordCategoryLookUpRepository = lookup_recordCategoryRepository;
            _userLookUpRepository = lookup_userRepository;
            _recordStatusLookUpRepository = lookup_recordStatusRepository;
            _cohortUserRepository = cohortUserRepository;
            _recordNoteRepository = recordNoteRepository;
            _httpContextAccessor = httpContextAccessor;
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _tenantDocumentRepository = tenantDocumentRepository;
            _PidTypeRepository = PidTypeRepository;
            _surpathManager = surpathManager;
            _logger = logger;



        }

        public async Task<exdtos.PagedResultDto<GetRecordStateForQueueViewDto>> GetAll(GetAllRecordStatesInput input)
        {

            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var stateFilter = input.StateFilter.HasValue
                        ? (EnumRecordState)input.StateFilter
                        : default;

            var filteredRecordStates = _recordStateRepository.GetAll()
                       .Include(e => e.RecordFk)
                       .Include(e => e.RecordCategoryFk)
                       .Include(e => e.UserFk)
                       .Include(e => e.RecordStatusFk)
                       .Where(e => e.UserFk.IsDeleted == false)
                       .Where(e => e.RecordStatusFk.IsDefault == true)
                       .WhereIf(AbpSession.TenantId == null, e => e.RecordStatusFk.IsSurpathServiceStatus == false);


            var _sorting = "RecordFk.DateUploaded desc";
            var pagedAndFilteredRecordStates = filteredRecordStates
                .OrderBy(_sorting)
                .PageBy(input);

            var recordStates = from o in pagedAndFilteredRecordStates
                               join o1 in _recordLookUpRepository.GetAll() on o.RecordId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               join o2 in _recordCategoryLookUpRepository.GetAll() on o.RecordCategoryId equals o2.Id into j2
                               from s2 in j2.DefaultIfEmpty()

                               join o3 in _userLookUpRepository.GetAll() on o.UserId equals o3.Id into j3
                               from s3 in j3.DefaultIfEmpty()

                               join o4 in _recordStatusLookUpRepository.GetAll() on o.RecordStatusId equals o4.Id into j4
                               from s4 in j4.DefaultIfEmpty()

                               select new
                               {

                                   o.State,
                                   o.Notes,
                                   Id = o.Id,
                                   Recordfilename = s1 == null || s1.filename == null ? "" : s1.filename.ToString(),
                                   RecordCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                   FullName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString(),
                                   UserName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString(),
                                   UserId = s3 == null || s3.Id == null ? -1 : s3.Id,
                                   RecordStatusStatusName = s4 == null || s4.StatusName == null ? "" : s4.StatusName.ToString(),
                                   RecordStatusHighlight = s4 == null || s4.HtmlColor == null ? "" : s4.HtmlColor,
                                   RecordStatusCSSClass = s4 == null || s4.CSSCLass == null ? "" : s4.CSSCLass,
                                   RecordStatusId = s4 == null || s4.Id == Guid.Empty ? Guid.Empty : s4.Id,
                                   TenantId = o.TenantId

                               };

            var totalCount = await filteredRecordStates.CountAsync();

            var dbList = await recordStates.ToListAsync();
            var results = new List<GetRecordStateForQueueViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRecordStateForQueueViewDto()
                {
                    RecordState = new RecordStateDto
                    {

                        State = o.State,
                        Notes = o.Notes,
                        Id = o.Id,
                    },
                    RecordStatus = new RecordStatusDto
                    {
                        Id = o.RecordStatusId,
                        StatusName = o.RecordStatusStatusName,
                        HtmlColor = o.RecordStatusHighlight,
                        CSSCLass = o.RecordStatusCSSClass,
                    },
                    Recordfilename = o.Recordfilename,
                    RecordCategoryName = o.RecordCategoryName,
                    UserName = o.FullName,
                    FullName = o.FullName,
                    UserId = o.UserId,
                    RecordStatusStatusName = o.RecordStatusStatusName,
                    TenantId = (int)o.TenantId,
                    CohortUserId = await _surpathManager.GetCohortUserIdForUser((long)o.UserId, (int)o.TenantId)

                };

                results.Add(res);
            }

            return new exdtos.PagedResultDto<GetRecordStateForQueueViewDto>(
                totalCount,
                results,
                input.SortingUser
            );
        }
    }
}
