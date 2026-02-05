using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface ITenantDepartmentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantDepartmentForViewDto>> GetAll(GetAllTenantDepartmentsInput input);

        Task<GetTenantDepartmentForViewDto> GetTenantDepartmentForView(Guid id);

        Task<GetTenantDepartmentForEditOutput> GetTenantDepartmentForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditTenantDepartmentDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetTenantDepartmentsToExcel(GetAllTenantDepartmentsForExcelInput input);

    }
}