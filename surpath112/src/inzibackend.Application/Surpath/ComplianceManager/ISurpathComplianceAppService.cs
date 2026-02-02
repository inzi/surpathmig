using System;
using inzibackend.Surpath.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inzibackend.Storage;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos.Registration;

namespace inzibackend.Surpath
{
    public interface ISurpathComplianceAppService
    {
        Task<List<ComplianceCohortTotalsForViewDto>> GetTenantDeptCompliance(IQueryable<TenantDepartment> pagedAndFilteredTenantDepartments);
        Task<List<ComplianceCohortTotalsForViewDto>> GetCohortCompliance(IQueryable<Cohort> pagedAndFilteredTenantDepartments);
        Task AddNoteToRecord(int tenantid, bool hostonly, bool authorizedonly, bool sendnotification, Guid recordstateid, long userId, string note);
        Task AddSystemNoteToRecord(int tenantid, bool hostonly, bool authorizedonly, bool sendnotification, Guid recordstateid, string note);
        Task<string> GetRemoteIPAddress();
        //Task<string> SurpathRecordsRootFolder();
        //Task<BinaryObject> GetBinaryObjectFromCache(string fileToken);
        Task<GetRecordForViewDto> CreateNewRecord(CreateOrEditRecordDto input);
        //Task CreateNewRecordState(CreateOrEditRecordStateDto input);
        Task<CreateOrEditRecordStateDto> CreateNewRecordState(CreateOrEditRecordStateDto input);
        Task RemovefiledataFile(EntityDto<Guid> input);

        Task<exdtos.PagedResultDto<RecordStateRecordCategoryLookupTableDto>> GetAllRecordCategoryForLookupTable(GetAllForLookupTableInput input);
        Task<List<RecordStateRecordStatusLookupTableDto>> GetAllRecordStatusForTableDropdown(long? tenantId = null, bool isSurpathService = false);

        Task<List<UserPid>> GetEmptyUserPidList();

        Task<List<PidType>> GetEmptyPidTypeList();

        Task<List<UserPidDto>> GetEmptyPidsList();


        // Registration interfaces

        Task<PagedResultDto<CohortTenantDepartmentLookupTableDto>> GetDepartmentsForRegistration(GetAllForLookupTableInput input);
        Task<bool> UsernameAvailable(UsernameAvailableDto input);
        Task<bool> EmailAvailable(UsernameAvailableDto input);
        Task<RegistrationValidationResultDto> ValidateRegistration(RegistrationValidationInputDto input);
        Task<exdtos.PagedResultDto<TenantDepartmentLookupTableDto>> GetAllCohortForLookupTable(ComplianceGetAllForLookupTableInput input);

        Task<List<GetRecordCategoryForEditOutput>> GetRecordCategoriesForRequirementForEdit(EntityDto<Guid> requirementId);


        // Requirement
        Task<CreateOrEditRecordRequirementDto> CreateEditRequirement(CreateEditRecordRequirementDto input);
        


    }
}
