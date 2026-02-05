using inzibackend.Surpath;
using inzibackend.Surpath;

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
    [AbpAuthorize(AppPermissions.Pages_DeptCodes)]
    public class DeptCodesAppService : inzibackendAppServiceBase, IDeptCodesAppService
    {
        private readonly IRepository<DeptCode, Guid> _deptCodeRepository;
        private readonly IDeptCodesExcelExporter _deptCodesExcelExporter;
        private readonly IRepository<CodeType, Guid> _codeTypeLookUpRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentLookUpRepository;

        public DeptCodesAppService(IRepository<DeptCode, Guid> deptCodeRepository, IDeptCodesExcelExporter deptCodesExcelExporter, IRepository<CodeType, Guid> lookup_codeTypeRepository, IRepository<TenantDepartment, Guid> lookup_tenantDepartmentRepository)
        {
            _deptCodeRepository = deptCodeRepository;
            _deptCodesExcelExporter = deptCodesExcelExporter;
            _codeTypeLookUpRepository = lookup_codeTypeRepository;
            _tenantDepartmentLookUpRepository = lookup_tenantDepartmentRepository;

        }

        public async Task<PagedResultDto<GetDeptCodeForViewDto>> GetAll(GetAllDeptCodesInput input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredDeptCodes = _deptCodeRepository.GetAll()
                        .Include(e => e.CodeTypeFk)
                        .Include(e => e.TenantDepartmentFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeTypeNameFilter), e => e.CodeTypeFk != null && e.CodeTypeFk.Name == input.CodeTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter);

            var pagedAndFilteredDeptCodes = filteredDeptCodes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var deptCodes = from o in pagedAndFilteredDeptCodes
                            join o1 in _codeTypeLookUpRepository.GetAll() on o.CodeTypeId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _tenantDepartmentLookUpRepository.GetAll() on o.TenantDepartmentId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            select new
                            {

                                o.Code,
                                Id = o.Id,
                                CodeTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                TenantDepartmentName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                            };

            var totalCount = await filteredDeptCodes.CountAsync();

            var dbList = await deptCodes.ToListAsync();
            var results = new List<GetDeptCodeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDeptCodeForViewDto()
                {
                    DeptCode = new DeptCodeDto
                    {

                        Code = o.Code,
                        Id = o.Id,
                    },
                    CodeTypeName = o.CodeTypeName,
                    TenantDepartmentName = o.TenantDepartmentName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDeptCodeForViewDto>(
                totalCount,
                results
            );
        }

        public async Task<GetDeptCodeForViewDto> GetDeptCodeForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var deptCode = await _deptCodeRepository.GetAsync(id);

            var output = new GetDeptCodeForViewDto { DeptCode = ObjectMapper.Map<DeptCodeDto>(deptCode) };

            if (output.DeptCode.CodeTypeId != null)
            {
                var _lookupCodeType = await _codeTypeLookUpRepository.FirstOrDefaultAsync((Guid)output.DeptCode.CodeTypeId);
                output.CodeTypeName = _lookupCodeType?.Name?.ToString();
            }

            if (output.DeptCode.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)output.DeptCode.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DeptCodes_Edit)]
        public async Task<GetDeptCodeForEditOutput> GetDeptCodeForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var deptCode = await _deptCodeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDeptCodeForEditOutput { DeptCode = ObjectMapper.Map<CreateOrEditDeptCodeDto>(deptCode) };

            if (output.DeptCode.CodeTypeId != null)
            {
                var _lookupCodeType = await _codeTypeLookUpRepository.FirstOrDefaultAsync((Guid)output.DeptCode.CodeTypeId);
                output.CodeTypeName = _lookupCodeType?.Name?.ToString();
            }

            if (output.DeptCode.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)output.DeptCode.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDeptCodeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DeptCodes_Create)]
        protected virtual async Task Create(CreateOrEditDeptCodeDto input)
        {
            var deptCode = ObjectMapper.Map<DeptCode>(input);

            if (AbpSession.TenantId != null)
            {
                deptCode.TenantId = (int?)AbpSession.TenantId;
            }

            await _deptCodeRepository.InsertAsync(deptCode);

        }

        [AbpAuthorize(AppPermissions.Pages_DeptCodes_Edit)]
        protected virtual async Task Update(CreateOrEditDeptCodeDto input)
        {
            var deptCode = await _deptCodeRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, deptCode);

        }

        [AbpAuthorize(AppPermissions.Pages_DeptCodes_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            await _deptCodeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDeptCodesToExcel(GetAllDeptCodesForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredDeptCodes = _deptCodeRepository.GetAll()
                        .Include(e => e.CodeTypeFk)
                        .Include(e => e.TenantDepartmentFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeTypeNameFilter), e => e.CodeTypeFk != null && e.CodeTypeFk.Name == input.CodeTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter);

            var query = (from o in filteredDeptCodes
                         join o1 in _codeTypeLookUpRepository.GetAll() on o.CodeTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _tenantDepartmentLookUpRepository.GetAll() on o.TenantDepartmentId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetDeptCodeForViewDto()
                         {
                             DeptCode = new DeptCodeDto
                             {
                                 Code = o.Code,
                                 Id = o.Id
                             },
                             CodeTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             TenantDepartmentName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var deptCodeListDtos = await query.ToListAsync();

            return _deptCodesExcelExporter.ExportToFile(deptCodeListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_DeptCodes)]
        public async Task<List<DeptCodeCodeTypeLookupTableDto>> GetAllCodeTypeForTableDropdown()
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            return await _codeTypeLookUpRepository.GetAll()
                .Select(codeType => new DeptCodeCodeTypeLookupTableDto
                {
                    Id = codeType.Id.ToString(),
                    DisplayName = codeType == null || codeType.Name == null ? "" : codeType.Name.ToString()
                }).ToListAsync();
        }

        
    }
}