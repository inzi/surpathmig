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
    [AbpAuthorize(AppPermissions.Pages_Administration_CodeTypes)]
    public class CodeTypesAppService : inzibackendAppServiceBase, ICodeTypesAppService
    {
        private readonly IRepository<CodeType, Guid> _codeTypeRepository;
        private readonly ICodeTypesExcelExporter _codeTypesExcelExporter;

        public CodeTypesAppService(IRepository<CodeType, Guid> codeTypeRepository, ICodeTypesExcelExporter codeTypesExcelExporter)
        {
            _codeTypeRepository = codeTypeRepository;
            _codeTypesExcelExporter = codeTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetCodeTypeForViewDto>> GetAll(GetAllCodeTypesInput input)
        {

                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var filteredCodeTypes = _codeTypeRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

                var pagedAndFilteredCodeTypes = filteredCodeTypes
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var codeTypes = from o in pagedAndFilteredCodeTypes
                                select new
                                {

                                    o.Name,
                                    Id = o.Id
                                };

                var totalCount = await filteredCodeTypes.CountAsync();

                var dbList = await codeTypes.ToListAsync();
                var results = new List<GetCodeTypeForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetCodeTypeForViewDto()
                    {
                        CodeType = new CodeTypeDto
                        {

                            Name = o.Name,
                            Id = o.Id,
                        }
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetCodeTypeForViewDto>(
                    totalCount,
                    results
                );
        }

        public async Task<GetCodeTypeForViewDto> GetCodeTypeForView(Guid id)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var codeType = await _codeTypeRepository.GetAsync(id);

                var output = new GetCodeTypeForViewDto { CodeType = ObjectMapper.Map<CodeTypeDto>(codeType) };

                return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_CodeTypes_Edit)]
        public async Task<GetCodeTypeForEditOutput> GetCodeTypeForEdit(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var codeType = await _codeTypeRepository.FirstOrDefaultAsync(input.Id);

                var output = new GetCodeTypeForEditOutput { CodeType = ObjectMapper.Map<CreateOrEditCodeTypeDto>(codeType) };

                return output;
        }

        public async Task CreateOrEdit(CreateOrEditCodeTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_CodeTypes_Create)]
        protected virtual async Task Create(CreateOrEditCodeTypeDto input)
        {
            var codeType = ObjectMapper.Map<CodeType>(input);

            if (AbpSession.TenantId != null)
            {
                codeType.TenantId = (int?)AbpSession.TenantId;
            }

            await _codeTypeRepository.InsertAsync(codeType);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_CodeTypes_Edit)]
        protected virtual async Task Update(CreateOrEditCodeTypeDto input)
        {
            var codeType = await _codeTypeRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, codeType);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_CodeTypes_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                await _codeTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCodeTypesToExcel(GetAllCodeTypesForExcelInput input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var filteredCodeTypes = _codeTypeRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

                var query = (from o in filteredCodeTypes
                             select new GetCodeTypeForViewDto()
                             {
                                 CodeType = new CodeTypeDto
                                 {
                                     Name = o.Name,
                                     Id = o.Id
                                 }
                             });

                var codeTypeListDtos = await query.ToListAsync();

                return await _codeTypesExcelExporter.ExportToFile(codeTypeListDtos);
        }

    }
}