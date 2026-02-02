using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
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
    [AbpAuthorize]
    public class WelcomemessagesAppService : inzibackendAppServiceBase, IWelcomemessagesAppService
    {
        private readonly IRepository<Welcomemessage> _welcomemessageRepository;

        public WelcomemessagesAppService(IRepository<Welcomemessage> welcomemessageRepository)
        {
            _welcomemessageRepository = welcomemessageRepository;

        }
        [AbpAuthorize]
        public async Task<WelcomemessageDto> GetCurrent()
        {
            var _w = new WelcomemessageDto()
            {
                Message = L("WelcomePage_Info"),
                Title = @L("WelcomePage_Title")
            };
            var welcomemessages = _welcomemessageRepository.GetAll().Where(w => w.IsDefault == true || (w.DisplayStart.Date >= DateTime.Now.Date && w.DisplayEnd.Date <= DateTime.Now.Date)).ToList();
            // var defaultWelcomeMessage = welcomemessages.Where(w => w.Default == true).FirstOrDefault();
            welcomemessages.Sort((x, y) => DateTime.Compare(y.DisplayStart.Date, x.DisplayEnd.Date));
            var currentMessage = welcomemessages.FirstOrDefault();
            if (currentMessage != null)
            {
                _w.Title = currentMessage.Title;
                _w.Message = currentMessage.Message;
            }
            return _w;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Welcomemessages)]

        public async Task<PagedResultDto<GetWelcomemessageForViewDto>> GetAll(GetAllWelcomemessagesInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var filteredWelcomemessages = _welcomemessageRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Message.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title.Contains(input.TitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MessageFilter), e => e.Message.Contains(input.MessageFilter))
                        .WhereIf(input.IsDefaultFilter.HasValue && input.IsDefaultFilter > -1, e => (input.IsDefaultFilter == 1 && e.IsDefault) || (input.IsDefaultFilter == 0 && !e.IsDefault))
                        .WhereIf(input.MinDisplayStartFilter != null, e => e.DisplayStart >= input.MinDisplayStartFilter)
                        .WhereIf(input.MaxDisplayStartFilter != null, e => e.DisplayStart <= input.MaxDisplayStartFilter)
                        .WhereIf(input.MinDisplayEndFilter != null, e => e.DisplayEnd >= input.MinDisplayEndFilter)
                        .WhereIf(input.MaxDisplayEndFilter != null, e => e.DisplayEnd <= input.MaxDisplayEndFilter);

            var pagedAndFilteredWelcomemessages = filteredWelcomemessages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var welcomemessages = from o in pagedAndFilteredWelcomemessages
                                  join o2 in TenantManager.Tenants on o.TenantId equals o2.Id into j2
                                  from tn in j2.DefaultIfEmpty()

                                  select new
                                  {

                                      o.Title,
                                      o.Message,
                                      o.IsDefault,
                                      o.DisplayStart,
                                      o.DisplayEnd,
                                      Id = o.Id,
                                      o.TenantId,
                                      TenantName = String.IsNullOrWhiteSpace(tn.Name) ? "" : tn.Name,
                                  };

            var totalCount = await filteredWelcomemessages.CountAsync();

            var dbList = await welcomemessages.ToListAsync();
            var results = new List<GetWelcomemessageForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetWelcomemessageForViewDto()
                {
                    Welcomemessage = new WelcomemessageDto
                    {

                        Title = o.Title,
                        Message = o.Message,
                        IsDefault = o.IsDefault,
                        DisplayStart = o.DisplayStart,
                        DisplayEnd = o.DisplayEnd,
                        Id = o.Id,
                    },
                    TenantName = o.TenantName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetWelcomemessageForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Welcomemessages_Edit)]
        public async Task<GetWelcomemessageForEditOutput> GetWelcomemessageForEdit(EntityDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var welcomemessage = await _welcomemessageRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetWelcomemessageForEditOutput { Welcomemessage = ObjectMapper.Map<CreateOrEditWelcomemessageDto>(welcomemessage) };

            return output;

        }

        public async Task CreateOrEdit(CreateOrEditWelcomemessageDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Welcomemessages_Create)]
        protected virtual async Task Create(CreateOrEditWelcomemessageDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var welcomemessage = ObjectMapper.Map<Welcomemessage>(input);

            if (AbpSession.TenantId != null)
            {
                welcomemessage.TenantId = (int?)AbpSession.TenantId;
            }

            await _welcomemessageRepository.InsertAsync(welcomemessage);
            if (welcomemessage.IsDefault)
                await SetDefaultWelcomeMessage(welcomemessage.Id, (int)welcomemessage.TenantId);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Welcomemessages_Edit)]
        protected virtual async Task Update(CreateOrEditWelcomemessageDto input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var welcomemessage = await _welcomemessageRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, welcomemessage);
            if (welcomemessage.IsDefault)
                await SetDefaultWelcomeMessage(welcomemessage.Id, (int)welcomemessage.TenantId);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Welcomemessages_Delete)]
        public async Task Delete(EntityDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            await _welcomemessageRepository.DeleteAsync(input.Id);

        }


        private async Task SetDefaultWelcomeMessage(int Id, int TenantId)
        {
                var others = _welcomemessageRepository.GetAll().Where(r => r.TenantId == TenantId && r.Id != Id);
                if (others != null)
                {
                    foreach (var other in others)
                    {
                        other.IsDefault = false;
                        _welcomemessageRepository.Update(other);
                    }
                }
        }
    }
}