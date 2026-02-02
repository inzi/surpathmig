using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface IRecordCategoryRulesAppService : IApplicationService
    {
        Task<PagedResultDto<GetRecordCategoryRuleForViewDto>> GetAll(GetAllRecordCategoryRulesInput input);

        Task<GetRecordCategoryRuleForViewDto> GetRecordCategoryRuleForView(Guid id);

        Task<GetRecordCategoryRuleForEditOutput> GetRecordCategoryRuleForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditRecordCategoryRuleDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetRecordCategoryRulesToExcel(GetAllRecordCategoryRulesForExcelInput input);

    }
}