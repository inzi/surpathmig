using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using inzibackend.Storage;
using inzibackend.Surpath.Dtos;
using inzibackend.Surpath.Dtos.Registration;
using Volo.Abp.Application.Dtos;

namespace inzibackend.Surpath.ComplianceManager
{
    public interface ISurpathComplianceAppService
    {
        Task<List<ComplianceCohortTotalsForViewDto>> GetTenantDeptCompliance(IQueryable<TenantDepartment> pagedAndFilteredTenantDepartments);
        Task<List<ComplianceCohortTotalsForViewDto>> GetCohortCompliance(IQueryable<Cohort> pagedAndFilteredCohorts);
        Task AddNoteToRecord(int tenantid, bool hostonly, bool authorizedonly, bool sendnotification, Guid recordstateid, long userId, string note);
        Task AddSystemNoteToRecord(int tenantid, bool hostonly, bool authorizedonly, bool sendnotification, Guid recordstateid, string note);
        Task<string> GetRemoteIPAddress();
        Task<GetRecordForViewDto> CreateNewRecord(CreateOrEditRecordDto input);
        Task<CreateOrEditRecordStateDto> CreateNewRecordState(CreateOrEditRecordStateDto input);
        Task RemovefiledataFile(EntityDto<Guid> input);
        Task<PagedResultDto<RecordStateRecordCategoryLookupTableDto>> GetAllRecordCategoryForLookupTable(GetAllForLookupTableInput input);
        Task<List<RecordStateRecordStatusLookupTableDto>> GetAllRecordStatusForTableDropdown(long? tenantId = null, bool isSurpathService = false);
        Task<List<UserPid>> GetEmptyUserPidList();
        Task<List<PidType>> GetEmptyPidTypeList();
        Task<List<UserPidDto>> GetEmptyPidsList();
        Task<PagedResultDto<CohortTenantDepartmentLookupTableDto>> GetDepartmentsForRegistration(GetAllForLookupTableInput input);
        Task<bool> UsernameAvailable(UsernameAvailableDto input);
        Task<bool> EmailAvailable(UsernameAvailableDto input);
        Task<RegistrationValidationResultDto> ValidateRegistration(RegistrationValidationInputDto input);
        Task<PagedResultDto<TenantDepartmentLookupTableDto>> GetAllCohortForLookupTable(ComplianceGetAllForLookupTableInput input);
        Task<List<GetRecordCategoryForEditOutput>> GetRecordCategoriesForRequirementForEdit(EntityDto<Guid> requirementId);
        Task<CreateOrEditRecordRequirementDto> CreateEditRequirement(CreateEditRecordRequirementDto input);
    }
}