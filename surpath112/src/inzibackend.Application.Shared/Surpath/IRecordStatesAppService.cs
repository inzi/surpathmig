using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;
using inzibackend.Surpath.exdtos;

namespace inzibackend.Surpath
{
    public interface IRecordStatesAppService : IApplicationService
    {
        Task<exdtos.PagedResultDto<GetRecordStateForViewDto>> GetAll(GetAllRecordStatesInput input);

        Task<GetRecordStateForViewDto> GetRecordStateForView(Guid id);

        Task<GetRecordStateForEditOutput> GetRecordStateForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditRecordStateDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetRecordStatesToExcel(GetAllRecordStatesForExcelInput input);

        Task<inzibackend.Surpath.exdtos.PagedResultDto<RecordStateRecordLookupTableDto>> GetAllRecordForLookupTable(GetAllForLookupTableInput input);

        Task<inzibackend.Surpath.exdtos.PagedResultDto<RecordStateRecordCategoryLookupTableDto>> GetAllRecordCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<inzibackend.Surpath.exdtos.PagedResultDto<RecordStateUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        Task<List<RecordStateRecordStatusLookupTableDto>> GetAllRecordStatusForTableDropdown(int? TenantId= null);
        Task<List<RecordStateRecordStatusLookupTableDto>> GetAllServiceRecordStatusForTableDropdown();

        Task<exdtos.PagedResultDto<GetRecordStateForViewDto>> GetAllForUserId(GetAllRecordStatesInput input);
        Task<exdtos.PagedResultDto<GetRecordStateCompliancetForViewDto>> GetRecordStateComplianceForUserId(GetAllRecordStatesInput input);

        // Commented out to use GetRecordStateComplianceForUserId
        //Task<exdtos.PagedResultDto<GetRecordStateCompliancetForViewDto>> GetSurpathServiceCompliance(GetAllRecordStatesInput input);

        Task<GetRecordStateForEditOutput> GetRecordStateForReview(EntityDto<Guid> input);

        Task<int?> GetTenantIdForRecordState(EntityDto<Guid> input);
        
        Task StateGhangeNotify(CreateOrEditRecordStateDto input);

        Task<List<GetArchivedDocumentsDto>> GetArchivedDocumentsForUser(GetArchivedDocumentsInput input);

        Task ReassociateDocument(ReassociateDocumentInput input);

    }
}