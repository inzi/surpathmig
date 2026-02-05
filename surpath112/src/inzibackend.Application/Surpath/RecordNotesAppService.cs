using inzibackend.Surpath;
using inzibackend.Authorization.Users;
using inzibackend.Authorization.Users;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using inzibackend.Surpath.Exporting;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using Abp.Application.Services.Dto;
using inzibackend.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using inzibackend.Storage;
using Abp.Domain.Uow;

namespace inzibackend.Surpath
{
    //[AbpAuthorize(AppPermissions.Pages_RecordNotes)]
    public class RecordNotesAppService : inzibackendAppServiceBase, IRecordNotesAppService
    {
        private readonly IRepository<RecordNote, Guid> _recordNoteRepository;
        private readonly IRecordNotesExcelExporter _recordNotesExcelExporter;
        private readonly IRepository<RecordState, Guid> _recordStateLookUpRepository;
        private readonly IRepository<User, long> _userLookUpRepository;

        public RecordNotesAppService(IRepository<RecordNote, Guid> recordNoteRepository, IRecordNotesExcelExporter recordNotesExcelExporter, IRepository<RecordState, Guid> lookup_recordStateRepository, IRepository<User, long> lookup_userRepository)
        {
            _recordNoteRepository = recordNoteRepository;
            _recordNotesExcelExporter = recordNotesExcelExporter;
            _recordStateLookUpRepository = lookup_recordStateRepository;
            _userLookUpRepository = lookup_userRepository;

        }

        [AbpAuthorize(AppPermissions.Pages_RecordNotes, AppPermissions.Pages_CohortUsers, AppPermissions.Pages_CohortUser)]
        
        public async Task<PagedResultDto<GetRecordNoteForViewDto>> GetAll(GetAllRecordNotesInput input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var _RightToHostOnlyNotes = PermissionChecker.IsGranted(AppPermissions.Surpath_View_Host_Only_notes);
            var _RightToAuthorizedOnlyNotes = PermissionChecker.IsGranted(AppPermissions.Surpath_View_Authorized_Only_notes);

            var filteredRecordNotes = _recordNoteRepository.GetAll()
                        .Include(e => e.RecordStateFk)
                        //.Include(e => e.UserFk)
                        .Include(e => e.NotifyUserFk)
                        .Where(e => (e.HostOnly == false || e.HostOnly == _RightToHostOnlyNotes) && (e.AuthorizedOnly == false || e.AuthorizedOnly == _RightToAuthorizedOnlyNotes || e.AuthorizedOnly == _RightToHostOnlyNotes))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Note.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoteFilter), e => e.Note == input.NoteFilter)
                        .WhereIf(input.MinCreatedFilter != null, e => e.Created >= input.MinCreatedFilter)
                        .WhereIf(input.MaxCreatedFilter != null, e => e.Created <= input.MaxCreatedFilter)
                        //.WhereIf(input.AuthorizedOnlyFilter.HasValue && input.AuthorizedOnlyFilter > -1, e => (input.AuthorizedOnlyFilter == 1 && e.AuthorizedOnly) || (input.AuthorizedOnlyFilter == 0 && !e.AuthorizedOnly))
                        //.WhereIf(input.HostOnlyFilter.HasValue && input.HostOnlyFilter > -1, e => (input.HostOnlyFilter == 1 && e.HostOnly) || (input.HostOnlyFilter == 0 && !e.HostOnly))
                        .WhereIf(input.SendNotificationFilter.HasValue && input.SendNotificationFilter > -1, e => (input.SendNotificationFilter == 1 && e.SendNotification) || (input.SendNotificationFilter == 0 && !e.SendNotification))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordStateNotesFilter), e => e.RecordStateFk != null && e.RecordStateFk.Notes == input.RecordStateNotesFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name.Contains(input.UserNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.NotifyUserFk != null && e.NotifyUserFk.Name.Contains(input.UserName2Filter))
                        .WhereIf(input.RecordStateIdFilter.HasValue, e => false || e.RecordStateId == input.RecordStateIdFilter.Value);
            //.WhereIf(input.RecordStateIdFilter.HasValue && !input.RecordCategoryIdFilter.HasValue, e => e.RecordStateId == input.RecordStateIdFilter.Value)
            //.WhereIf(input.RecordCategoryIdFilter.HasValue && !input.RecordStateIdFilter.HasValue, e => e.RecordStateFk.RecordCategoryId == input.RecordCategoryIdFilter.Value)
            //.WhereIf(input.RecordCategoryIdFilter.HasValue && input.RecordStateIdFilter.HasValue, e => e.RecordStateFk.RecordCategoryId == input.RecordCategoryIdFilter && e.UserId == e.RecordStateFk.UserId);
            //.WhereIf(input.RecordCategoryIdFilter.HasValue && input.RecordStateIdFilter.HasValue, e => e.RecordStateFk.RecordCategoryId == input.RecordCategoryIdFilter.Value);



            if (input.RecordCategoryIdFilter.HasValue && input.RecordStateIdFilter.HasValue)
            {
                var _rs = _recordStateLookUpRepository.Get(input.RecordStateIdFilter.Value);
                if (_rs != null)
                {
                    filteredRecordNotes = _recordNoteRepository.GetAll()
                        .Include(e => e.RecordStateFk)
                        .ThenInclude(e => e.RecordCategoryFk)
                        .Where(e => e.RecordStateFk.UserId == _rs.UserId)
                        .Where(e => e.RecordStateFk.RecordCategoryFk.Id == _rs.RecordCategoryId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Note.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoteFilter), e => e.Note == input.NoteFilter)
                        .WhereIf(input.MinCreatedFilter != null, e => e.Created >= input.MinCreatedFilter)
                        .WhereIf(input.MaxCreatedFilter != null, e => e.Created <= input.MaxCreatedFilter)
                        .WhereIf(input.SendNotificationFilter.HasValue && input.SendNotificationFilter > -1, e => (input.SendNotificationFilter == 1 && e.SendNotification) || (input.SendNotificationFilter == 0 && !e.SendNotification))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordStateNotesFilter), e => e.RecordStateFk != null && e.RecordStateFk.Notes == input.RecordStateNotesFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name.Contains(input.UserNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.NotifyUserFk != null && e.NotifyUserFk.Name.Contains(input.UserName2Filter));
                }

            }

            //var filteredRecordNotes = _recordNoteRepository.GetAll()
            //            .Include(e => e.RecordStateFk)
            //            ////.Include(e => e.UserFk)
            //            ////.Include(e => e.NotifyUserFk)
            //            .Where(e => (e.HostOnly == false || e.HostOnly == _RightToHostOnlyNotes) && (e.AuthorizedOnly == false || e.AuthorizedOnly == _RightToAuthorizedOnlyNotes || e.AuthorizedOnly == _RightToHostOnlyNotes))
            //            //.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Note.Contains(input.Filter))
            //            //.WhereIf(!string.IsNullOrWhiteSpace(input.NoteFilter), e => e.Note == input.NoteFilter)
            //            //.WhereIf(input.MinCreatedFilter != null, e => e.Created >= input.MinCreatedFilter)
            //            //.WhereIf(input.MaxCreatedFilter != null, e => e.Created <= input.MaxCreatedFilter)
            //            ////.WhereIf(input.AuthorizedOnlyFilter.HasValue && input.AuthorizedOnlyFilter > -1, e => (input.AuthorizedOnlyFilter == 1 && e.AuthorizedOnly) || (input.AuthorizedOnlyFilter == 0 && !e.AuthorizedOnly))
            //            ////.WhereIf(input.HostOnlyFilter.HasValue && input.HostOnlyFilter > -1, e => (input.HostOnlyFilter == 1 && e.HostOnly) || (input.HostOnlyFilter == 0 && !e.HostOnly))
            //            //.WhereIf(input.SendNotificationFilter.HasValue && input.SendNotificationFilter > -1, e => (input.SendNotificationFilter == 1 && e.SendNotification) || (input.SendNotificationFilter == 0 && !e.SendNotification))
            //            //.WhereIf(!string.IsNullOrWhiteSpace(input.RecordStateNotesFilter), e => e.RecordStateFk != null && e.RecordStateFk.Notes == input.RecordStateNotesFilter)
            //            //.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name.Contains(input.UserNameFilter))
            //            //.WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.NotifyUserFk != null && e.NotifyUserFk.Name.Contains(input.UserName2Filter))
            //            ////.WhereIf(input.RecordStateIdFilter.HasValue, e => false || e.RecordStateId == input.RecordStateIdFilter.Value);
            //            //.WhereIf(input.RecordStateIdFilter.HasValue && !input.RecordCategoryIdFilter.HasValue, e => e.RecordStateId == input.RecordStateIdFilter.Value)
            //            //.WhereIf(input.RecordCategoryIdFilter.HasValue && !input.RecordStateIdFilter.HasValue, e => e.RecordStateFk.RecordCategoryId == input.RecordCategoryIdFilter.Value)
            //            ////.WhereIf(input.RecordCategoryIdFilter.HasValue && input.RecordStateIdFilter.HasValue, e => e.RecordStateFk.RecordCategoryId == input.RecordCategoryIdFilter && e.UserId == e.RecordStateFk.UserId);
            //            .WhereIf(input.RecordCategoryIdFilter.HasValue && input.RecordStateIdFilter.HasValue, e => e.RecordStateFk.RecordCategoryId == input.RecordCategoryIdFilter.Value);

            var pagedAndFilteredRecordNotes = filteredRecordNotes
                .OrderBy(input.Sorting ?? "created desc")
                .PageBy(input);

            var testlistQ = from o in pagedAndFilteredRecordNotes
                            select o;
            var testlist = testlistQ.ToList();

            var recordNotes = from o in pagedAndFilteredRecordNotes
                              join o1 in _recordStateLookUpRepository.GetAll() on o.RecordStateId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              join o2 in _userLookUpRepository.GetAll().IgnoreQueryFilters() on o.UserId equals o2.Id into j2
                              from s2 in j2.DefaultIfEmpty()

                              join o3 in _userLookUpRepository.GetAll().IgnoreQueryFilters() on o.NotifyUserId equals o3.Id into j3
                              from s3 in j3.DefaultIfEmpty()

                              select new
                              {

                                  o.Note,
                                  //o.Created,
                                  Created = DateTime.SpecifyKind(o.Created, DateTimeKind.Utc),
                                  o.AuthorizedOnly,
                                  o.HostOnly,
                                  o.SendNotification,
                                  Id = o.Id,
                                  RecordStateNotes = s1 == null || s1.Notes == null ? "" : s1.Notes.ToString(),
                                  UserName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString(),
                                  UserName2 = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                              };

            var totalCount = await filteredRecordNotes.CountAsync();

            var dbList = await recordNotes.ToListAsync();
            var results = new List<GetRecordNoteForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRecordNoteForViewDto()
                {
                    RecordNote = new RecordNoteDto
                    {

                        Note = o.Note,
                        Created = o.Created,
                        AuthorizedOnly = o.AuthorizedOnly,
                        HostOnly = o.HostOnly,
                        SendNotification = o.SendNotification,
                        Id = o.Id,
                    },
                    RecordStateNotes = o.RecordStateNotes,
                    UserName = o.UserName,
                    UserName2 = o.UserName2
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRecordNoteForViewDto>(
                totalCount,
                results
            );
        }
        [AbpAuthorize(AppPermissions.Pages_RecordNotes)]
        public async Task<GetRecordNoteForViewDto> GetRecordNoteForView(Guid id)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var recordNote = await _recordNoteRepository.GetAsync(id);

                var output = new GetRecordNoteForViewDto { RecordNote = ObjectMapper.Map<RecordNoteDto>(recordNote) };

                if (output.RecordNote.RecordStateId != null)
                {
                    var _lookupRecordState = await _recordStateLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordNote.RecordStateId);
                    output.RecordStateNotes = _lookupRecordState?.Notes?.ToString();
                }

                if (output.RecordNote.UserId != null)
                {
                    var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.RecordNote.UserId);
                    output.UserName = _lookupUser?.Name?.ToString();
                }

                if (output.RecordNote.NotifyUserId != null)
                {
                    var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.RecordNote.NotifyUserId);
                    output.UserName2 = _lookupUser?.Name?.ToString();
                }

                return output;
        }

        [AbpAuthorize(AppPermissions.Pages_RecordNotes_Edit)]
        public async Task<GetRecordNoteForEditOutput> GetRecordNoteForEdit(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var recordNote = await _recordNoteRepository.FirstOrDefaultAsync(input.Id);

                var output = new GetRecordNoteForEditOutput { RecordNote = ObjectMapper.Map<CreateOrEditRecordNoteDto>(recordNote) };

                if (output.RecordNote.RecordStateId != null)
                {
                    var _lookupRecordState = await _recordStateLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordNote.RecordStateId);
                    output.RecordStateNotes = _lookupRecordState?.Notes?.ToString();
                }

                if (output.RecordNote.UserId != null)
                {
                    var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.RecordNote.UserId);
                    output.UserName = _lookupUser?.Name?.ToString();
                }

                if (output.RecordNote.NotifyUserId != null)
                {
                    var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.RecordNote.NotifyUserId);
                    output.UserName2 = _lookupUser?.Name?.ToString();
                }

                return output;
        }
        [AbpAuthorize(AppPermissions.Pages_RecordNotes, AppPermissions.Pages_CohortUsers, AppPermissions.Pages_CohortUser)]
        public async Task CreateOrEdit(CreateOrEditRecordNoteDto input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                if (input.Id == null)
                {
                    await Create(input);
                }
                else
                {
                    await Update(input);
                }
        }
        [AbpAuthorize(AppPermissions.Pages_RecordNotes)]
        public async Task AddNoteToRecordState(CreateOrEditRecordNoteDto input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                input.Created = DateTime.UtcNow;
                input.UserId = AbpSession.UserId;

                await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_RecordNotes, AppPermissions.Pages_CohortUsers, AppPermissions.Pages_CohortUser)]
        protected virtual async Task Create(CreateOrEditRecordNoteDto input)
        {
            var recordNote = ObjectMapper.Map<RecordNote>(input);

            if (AbpSession.TenantId != null)
            {
                recordNote.TenantId = (int?)AbpSession.TenantId;
            }

            await _recordNoteRepository.InsertAsync(recordNote);

        }

        [AbpAuthorize(AppPermissions.Pages_RecordNotes_Edit)]
        protected virtual async Task Update(CreateOrEditRecordNoteDto input)
        {
            var recordNote = await _recordNoteRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, recordNote);

        }

        [AbpAuthorize(AppPermissions.Pages_RecordNotes_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                await _recordNoteRepository.DeleteAsync(input.Id);
        }
        [AbpAuthorize(AppPermissions.Pages_RecordNotes)]
        public async Task<FileDto> GetRecordNotesToExcel(GetAllRecordNotesForExcelInput input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var filteredRecordNotes = _recordNoteRepository.GetAll()
                            .Include(e => e.RecordStateFk)
                            .Include(e => e.UserFk)
                            .Include(e => e.NotifyUserFk)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Note.Contains(input.Filter))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.NoteFilter), e => e.Note == input.NoteFilter)
                            .WhereIf(input.MinCreatedFilter != null, e => e.Created >= input.MinCreatedFilter)
                            .WhereIf(input.MaxCreatedFilter != null, e => e.Created <= input.MaxCreatedFilter)
                            .WhereIf(input.AuthorizedOnlyFilter.HasValue && input.AuthorizedOnlyFilter > -1, e => (input.AuthorizedOnlyFilter == 1 && e.AuthorizedOnly) || (input.AuthorizedOnlyFilter == 0 && !e.AuthorizedOnly))
                            .WhereIf(input.HostOnlyFilter.HasValue && input.HostOnlyFilter > -1, e => (input.HostOnlyFilter == 1 && e.HostOnly) || (input.HostOnlyFilter == 0 && !e.HostOnly))
                            .WhereIf(input.SendNotificationFilter.HasValue && input.SendNotificationFilter > -1, e => (input.SendNotificationFilter == 1 && e.SendNotification) || (input.SendNotificationFilter == 0 && !e.SendNotification))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.RecordStateNotesFilter), e => e.RecordStateFk != null && e.RecordStateFk.Notes == input.RecordStateNotesFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.NotifyUserFk != null && e.NotifyUserFk.Name == input.UserName2Filter);

                var query = (from o in filteredRecordNotes
                             join o1 in _recordStateLookUpRepository.GetAll() on o.RecordStateId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _userLookUpRepository.GetAll() on o.UserId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             join o3 in _userLookUpRepository.GetAll() on o.NotifyUserId equals o3.Id into j3
                             from s3 in j3.DefaultIfEmpty()

                             select new GetRecordNoteForViewDto()
                             {
                                 RecordNote = new RecordNoteDto
                                 {
                                     Note = o.Note,
                                     Created = o.Created,
                                     AuthorizedOnly = o.AuthorizedOnly,
                                     HostOnly = o.HostOnly,
                                     SendNotification = o.SendNotification,
                                     Id = o.Id
                                 },
                                 RecordStateNotes = s1 == null || s1.Notes == null ? "" : s1.Notes.ToString(),
                                 UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                 UserName2 = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                             });

                var recordNoteListDtos = await query.ToListAsync();

                return _recordNotesExcelExporter.ExportToFile(recordNoteListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_RecordNotes)]
        public async Task<PagedResultDto<RecordNoteRecordStateLookupTableDto>> GetAllRecordStateForLookupTable(GetAllForLookupTableInput input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                var query = _recordStateLookUpRepository.GetAll().WhereIf(
                       !string.IsNullOrWhiteSpace(input.Filter),
                      e => e.Notes != null && e.Notes.Contains(input.Filter)
                   );

                var totalCount = await query.CountAsync();

                var recordStateList = await query
                    .PageBy(input)
                    .ToListAsync();

                var lookupTableDtoList = new List<RecordNoteRecordStateLookupTableDto>();
                foreach (var recordState in recordStateList)
                {
                    lookupTableDtoList.Add(new RecordNoteRecordStateLookupTableDto
                    {
                        Id = recordState.Id.ToString(),
                        DisplayName = recordState.Notes?.ToString()
                    });
                }

                return new PagedResultDto<RecordNoteRecordStateLookupTableDto>(
                    totalCount,
                    lookupTableDtoList
                );
        }
        [AbpAuthorize(AppPermissions.Pages_RecordNotes)]
        public async Task<List<RecordNoteUserLookupTableDto>> GetAllUserForTableDropdown()
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                return await _userLookUpRepository.GetAll()
                    .Select(user => new RecordNoteUserLookupTableDto
                    {
                        Id = user.Id,
                        DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                    }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_RecordNotes)]
        public async Task<PagedResultDto<RecordNoteUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                var query = _userLookUpRepository.GetAll().WhereIf(
                       !string.IsNullOrWhiteSpace(input.Filter),
                      e => (e.Name != null && e.Name.Contains(input.Filter))
                || (e.MiddleName != null && e.MiddleName.Contains(input.Filter))
                || (e.Surname != null && e.Surname.Contains(input.Filter))
                || (e.EmailAddress != null && e.EmailAddress.Contains(input.Filter))
                || (e.PhoneNumber != null && e.PhoneNumber.Contains(input.Filter))
                   );

                var totalCount = await query.CountAsync();

                var userList = await query
                    .PageBy(input)
                    .ToListAsync();

                var lookupTableDtoList = new List<RecordNoteUserLookupTableDto>();
                foreach (var user in userList)
                {
                    lookupTableDtoList.Add(new RecordNoteUserLookupTableDto
                    {
                        Id = user.Id,
                        DisplayName = user.Name?.ToString()
                    });
                }

                return new PagedResultDto<RecordNoteUserLookupTableDto>(
                    totalCount,
                    lookupTableDtoList
                );
        }

    }
}