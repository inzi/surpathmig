using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    public interface IDeptCodesAppService : IApplicationService
    {
        Task<PagedResultDto<GetDeptCodeForViewDto>> GetAll(GetAllDeptCodesInput input);

        Task<GetDeptCodeForViewDto> GetDeptCodeForView(Guid id);

        Task<GetDeptCodeForEditOutput> GetDeptCodeForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditDeptCodeDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetDeptCodesToExcel(GetAllDeptCodesForExcelInput input);

        Task<List<DeptCodeCodeTypeLookupTableDto>> GetAllCodeTypeForTableDropdown();

        //Task<PagedResultDto<DeptCodeTenantDepartmentLookupTableDto>> GetAllTenantDepartmentForLookupTable(GetAllForLookupTableInput input);

    }
}