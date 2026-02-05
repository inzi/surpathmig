using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    public interface IRecordNotesAppService : IApplicationService
    {
        Task<PagedResultDto<GetRecordNoteForViewDto>> GetAll(GetAllRecordNotesInput input);

        Task<GetRecordNoteForViewDto> GetRecordNoteForView(Guid id);

        Task<GetRecordNoteForEditOutput> GetRecordNoteForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditRecordNoteDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetRecordNotesToExcel(GetAllRecordNotesForExcelInput input);

        Task<PagedResultDto<RecordNoteRecordStateLookupTableDto>> GetAllRecordStateForLookupTable(GetAllForLookupTableInput input);

        Task<List<RecordNoteUserLookupTableDto>> GetAllUserForTableDropdown();

        Task<PagedResultDto<RecordNoteUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
        
        Task AddNoteToRecordState(CreateOrEditRecordNoteDto input);

    }
}