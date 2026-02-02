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
    [AbpAuthorize(AppPermissions.Pages_Administration_Drugs)]
    public class DrugsAppService : inzibackendAppServiceBase, IDrugsAppService
    {
        private readonly IRepository<Drug, Guid> _drugRepository;
        private readonly IDrugsExcelExporter _drugsExcelExporter;

        public DrugsAppService(IRepository<Drug, Guid> drugRepository, IDrugsExcelExporter drugsExcelExporter)
        {
            _drugRepository = drugRepository;
            _drugsExcelExporter = drugsExcelExporter;

        }

        public async Task<PagedResultDto<GetDrugForViewDto>> GetAll(GetAllDrugsInput input)
        {

                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var filteredDrugs = _drugRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Code.Contains(input.Filter))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter);

                var pagedAndFilteredDrugs = filteredDrugs
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var drugs = from o in pagedAndFilteredDrugs
                            select new
                            {

                                o.Name,
                                o.Code,
                                Id = o.Id
                            };

                var totalCount = await filteredDrugs.CountAsync();

                var dbList = await drugs.ToListAsync();
                var results = new List<GetDrugForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetDrugForViewDto()
                    {
                        Drug = new DrugDto
                        {

                            Name = o.Name,
                            Code = o.Code,
                            Id = o.Id,
                        }
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetDrugForViewDto>(
                    totalCount,
                    results
                );
        }

        public async Task<GetDrugForViewDto> GetDrugForView(Guid id)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var drug = await _drugRepository.GetAsync(id);

                var output = new GetDrugForViewDto { Drug = ObjectMapper.Map<DrugDto>(drug) };

                return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Drugs_Edit)]
        public async Task<GetDrugForEditOutput> GetDrugForEdit(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var drug = await _drugRepository.FirstOrDefaultAsync(input.Id);

                var output = new GetDrugForEditOutput { Drug = ObjectMapper.Map<CreateOrEditDrugDto>(drug) };

                return output;
            
        }

        public async Task CreateOrEdit(CreateOrEditDrugDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Drugs_Create)]
        protected virtual async Task Create(CreateOrEditDrugDto input)
        {
            var drug = ObjectMapper.Map<Drug>(input);

            await _drugRepository.InsertAsync(drug);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Drugs_Edit)]
        protected virtual async Task Update(CreateOrEditDrugDto input)
        {
            var drug = await _drugRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, drug);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Drugs_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                await _drugRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDrugsToExcel(GetAllDrugsForExcelInput input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var filteredDrugs = _drugRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Code.Contains(input.Filter))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter);

                var query = (from o in filteredDrugs
                             select new GetDrugForViewDto()
                             {
                                 Drug = new DrugDto
                                 {
                                     Name = o.Name,
                                     Code = o.Code,
                                     Id = o.Id
                                 }
                             });

                var drugListDtos = await query.ToListAsync();

                return _drugsExcelExporter.ExportToFile(drugListDtos);
        }

    }
}