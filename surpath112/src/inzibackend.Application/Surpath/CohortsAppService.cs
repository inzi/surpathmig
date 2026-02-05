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
using Abp.BackgroundJobs;
using inzibackend.Surpath.Importing;
using inzibackend.Authorization.Users.Dto;
using inzibackend.Surpath.Compliance;
using inzibackend.MultiTenancy;
using Abp.Zero.Configuration;
using MySqlX.XDevAPI.Common;
using PayPalCheckoutSdk.Orders;
using Abp.Collections.Extensions;
using System.Drawing.Text;
using NPOI.SS.Formula.Functions;
using inzibackend.Authorization.Organizations;

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_Cohorts)]
    public class CohortsAppService : inzibackendAppServiceBase, ICohortsAppService
    {
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly ICohortUsersAppService cohortUsersAppService;
        private readonly ICohortsExcelExporter _cohortsExcelExporter;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentLoookUpRepository;
        private readonly IRepository<RecordState, Guid> _recordStateRepository;

        protected readonly IBackgroundJobManager BackgroundJobManager;

        private readonly ISurpathComplianceAppService _surpathComplianceAppService;
        private readonly SurpathManager _surpathManager;

        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IOUSecurityManager _ouSecurityManager;

        public CohortsAppService(IOUSecurityManager ouSecurityManager, IRepository<Cohort, Guid> cohortRepository, ICohortsExcelExporter cohortsExcelExporter, IRepository<TenantDepartment, Guid> lookup_tenantDepartmentRepository, IBackgroundJobManager backgroundJobManager,
             ISurpathComplianceAppService surpathComplianceAppService,
             SurpathManager surpathManager,
             IRepository<CohortUser, Guid> cohortUserRepository,
             ICohortUsersAppService cohortUsersAppService,
             IRepository<Tenant> tenantRepository,
             IRepository<RecordState, Guid> recordStateRepository
            )
        {
            _cohortRepository = cohortRepository;
            _cohortUserRepository = cohortUserRepository;
            _cohortsExcelExporter = cohortsExcelExporter;
            _tenantDepartmentLoookUpRepository = lookup_tenantDepartmentRepository;
            _recordStateRepository = recordStateRepository;

            BackgroundJobManager = backgroundJobManager;

            _surpathComplianceAppService = surpathComplianceAppService;
            _surpathManager = surpathManager;
            _tenantRepository = tenantRepository;
            _ouSecurityManager = ouSecurityManager;
        }

        private class CohortResultTempDto
        {
            public Guid Id { get; set;}
            public string Name { get; set; }
            public string Description { get; set; }
            public bool DefaultCohort { get; set; }
            public string TenantDepartmentName { get; set; }
            public Guid? TenantDepartmentId { get; set; }
            public string TenantName { get; set; }
            public int? TenantId { get; set; }
            public bool UserExists { get; set; } = false;
            public int CohortusersCount { get; set; }
        }

        public async Task<PagedResultDto<GetCohortForViewDto>> GetAll(GetAllCohortsInput input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);


            var filteredCohorts = _cohortRepository.GetAll().AsNoTracking().Include(e => e.TenantDepartmentFk)
                      .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                      .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                      .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                      .WhereIf(input.DefaultCohortFilter.HasValue && input.DefaultCohortFilter > -1, e => (input.DefaultCohortFilter == 1 && e.DefaultCohort) || (input.DefaultCohortFilter == 0 && !e.DefaultCohort))
                      .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter)
                      ;

            filteredCohorts = _ouSecurityManager.ApplyTenantDepartmentCohortVisibilityFilter(filteredCohorts, AbpSession.UserId.Value);

            var totalCount = await filteredCohorts.CountAsync();
            List<Guid?> _cohortIds = new List<Guid?>();


            if (totalCount < 1)
            {
                var cusearch = _cohortUserRepository.GetAll().AsNoTracking()
                   .Include(e => e.CohortFk)
                   .Include(e => e.UserFk)
                   .Where(e => e.UserFk.IsDeleted == false && e.IsDeleted == false)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.UserFk != null && e.UserFk.Name.Contains(input.Filter) || e.UserFk.Surname.Contains(input.Filter) || e.UserFk.EmailAddress.Contains(input.Filter))
                   .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.UserFk != null && e.UserFk.Name.Contains(input.NameFilter) || e.UserFk.Surname.Contains(input.NameFilter));

                var _cucount = await cusearch.CountAsync();


                if (cusearch.Count() > 0)
                {
                    // we found some users. 
                    // reset our results to only be cohorts with that user

                    // get all the cohort ids and reset our filtered results for the query
                    _cohortIds = cusearch.Select(cu => cu.CohortId).ToList();

                    filteredCohorts = _cohortRepository.GetAll().AsNoTracking()
                        .Include(e => e.TenantDepartmentFk)
                        .Where(e => _cohortIds.Contains(e.Id))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.DefaultCohortFilter.HasValue && input.DefaultCohortFilter > -1, e => (input.DefaultCohortFilter == 1 && e.DefaultCohort) || (input.DefaultCohortFilter == 0 && !e.DefaultCohort))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter);
                       

                    //totalCount = await filteredCohorts.CountAsync();
                    // now that that is done, we need to tell the UI that it needs to autoexpand
                    // each tenant and search using the filter supplied
                    // so the client side script will auto-expand members
                    // and use the search filter
                }
            }


            var pagedAndFilteredCohorts = filteredCohorts
               .OrderBy(input.Sorting ?? "Name asc")
               .PageBy(input);

            var _dbList = (from c in pagedAndFilteredCohorts
                           join tenantTable in _tenantRepository.GetAll().AsNoTracking().Where(_t => _t.Id == AbpSession.TenantId || AbpSession.TenantId == null) on c.TenantId equals tenantTable.Id into tJoin
                           from t in tJoin.DefaultIfEmpty()
                           join cuTable in _cohortUserRepository.GetAll().AsNoTracking().Include(u => u.UserFk).Where(e => e.UserFk.IsDeleted == false && e.IsDeleted == false) on c.Id equals cuTable.CohortId into cuJoin
                           from cu in cuJoin.DefaultIfEmpty()
                           where c.IsDeleted == false && (c.TenantId == AbpSession.TenantId || AbpSession.TenantId == null) && (cu.IsDeleted == false || cu == null) && (t.IsDeleted == false || t == null)
                           select new CohortResultTempDto()
                           {
                               Id = c.Id,
                               TenantId = c.TenantId,
                               Name = c.Name,
                               Description = c.Description,
                               DefaultCohort = c.DefaultCohort,
                               TenantDepartmentId = c.TenantDepartmentId,
                               TenantDepartmentName = c.TenantDepartmentFk.Name,
                               TenantName = t.Name,
                               UserExists = cu.UserFk != null
                           }).ToList();

            var dbList = _dbList
                .GroupBy(e=>new { e.Id, e.TenantId, e.Name, e.TenantName, e.TenantDepartmentName, e.TenantDepartmentId, e.Description, e.DefaultCohort })
                .Select(c=> new CohortResultTempDto()
                {
                    Id = c.Key.Id,
                    TenantId = c.Key.TenantId,
                    Name = c.Key.Name,
                    Description = c.Key.Description,
                    DefaultCohort = c.Key.DefaultCohort,
                    TenantDepartmentId = c.Key.TenantDepartmentId,
                    TenantDepartmentName = c.Key.TenantDepartmentName,
                    TenantName = c.Key.TenantName,
                    CohortusersCount = _dbList.Where(r=>r.Id==c.Key.Id).Count(cu => cu.UserExists == true)
                }).ToList();
                

            // get the list of cohorts known
            // then auto expand members for any cohorts that contain a user with this filter


            //var dbList = await filteredCohortsWithTenant.ToListAsync();
            var results = new List<GetCohortForViewDto>();
            totalCount = dbList.Count();

            foreach (var o in dbList)
            {
                var res = new GetCohortForViewDto()
                {
                    Cohort = new CohortDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        DefaultCohort = o.DefaultCohort,
                        Id = o.Id,
                    },
                    TenantDepartmentName = o.TenantDepartmentName,
                    TenantName = await _surpathManager.GetTenantNameById((int)o.TenantId),
                    TenantId = (int)o.TenantId,
                    CohortusersCount = o.CohortusersCount,
                    ExpandAndSearch = _cohortIds.Contains(o.Id) || input.ReOpen.Contains(o.Id),
                };

                results.Add(res);
            }


            return new PagedResultDto<GetCohortForViewDto>(
                totalCount,
                results
            );
        }

        public async Task<GetCohortForViewDto> GetCohortForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var cohort = await _cohortRepository.GetAsync(id);

            var output = new GetCohortForViewDto { Cohort = ObjectMapper.Map<CohortDto>(cohort) };

            if (output.Cohort.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLoookUpRepository.FirstOrDefaultAsync((Guid)output.Cohort.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Cohorts_Edit)]
        public async Task<GetCohortForEditOutput> GetCohortForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var cohort = await _cohortRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCohortForEditOutput { Cohort = ObjectMapper.Map<CreateOrEditCohortDto>(cohort) };

            if (output.Cohort.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLoookUpRepository.FirstOrDefaultAsync((Guid)output.Cohort.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCohortDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var _defcohort = await _cohortRepository.FirstOrDefaultAsync(c => c.DefaultCohort == true);

            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
            if (input.DefaultCohort == true)
            {
                if (_defcohort != null && _defcohort.Id != input.Id)
                {
                    _defcohort.DefaultCohort = false;
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Cohorts_Create)]
        protected virtual async Task Create(CreateOrEditCohortDto input)
        {
            var cohort = ObjectMapper.Map<Cohort>(input);

            if (AbpSession.TenantId != null)
            {
                cohort.TenantId = (int?)AbpSession.TenantId;
            }

            await _cohortRepository.InsertAsync(cohort);

        }

        [AbpAuthorize(AppPermissions.Pages_Cohorts_Edit)]
        protected virtual async Task Update(CreateOrEditCohortDto input)
        {
            var cohort = await _cohortRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, cohort);

        }

        [AbpAuthorize(AppPermissions.Pages_Cohorts_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            await _cohortRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCohortsToExcel(GetAllCohortsForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredCohorts = _cohortRepository.GetAll()
                        .Include(e => e.TenantDepartmentFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.DefaultCohortFilter.HasValue && input.DefaultCohortFilter > -1, e => (input.DefaultCohortFilter == 1 && e.DefaultCohort) || (input.DefaultCohortFilter == 0 && !e.DefaultCohort))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter);

            var query = (from o in filteredCohorts
                         join o1 in _tenantDepartmentLoookUpRepository.GetAll() on o.TenantDepartmentId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetCohortForViewDto()
                         {
                             Cohort = new CohortDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 DefaultCohort = o.DefaultCohort,
                                 Id = o.Id
                             },
                             TenantDepartmentName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var cohortListDtos = await query.ToListAsync();

            return _cohortsExcelExporter.ExportToFile(cohortListDtos);
        }

        public async Task<FileDto> GetCohortsImmunizationReportToExcel(GetAllCohortsForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredCohorts = _cohortRepository.GetAll()
                        .Include(e => e.TenantDepartmentFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.DefaultCohortFilter.HasValue && input.DefaultCohortFilter > -1, e => (input.DefaultCohortFilter == 1 && e.DefaultCohort) || (input.DefaultCohortFilter == 0 && !e.DefaultCohort))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter);

            var cohorts = await filteredCohorts.ToListAsync();
            var cohortImmunizationData = new List<CohortImmunizationReportDto>();

            foreach (var cohort in cohorts)
            {
                // Get cohort users
                var cohortUsers = await _cohortUserRepository.GetAll()
                    .Include(cu => cu.UserFk)
                    .Where(cu => cu.CohortId == cohort.Id && !cu.UserFk.IsDeleted)
                    .ToListAsync();

                foreach (var cohortUser in cohortUsers)
                {
                    var memberData = new CohortImmunizationReportDto
                    {
                        CohortName = cohort.Name,
                        FirstName = cohortUser.UserFk.Name,
                        LastName = cohortUser.UserFk.Surname
                    };

                    // Get immunization requirements for this user's department/cohort
                    var immunizationRequirements = await _surpathManager.GetRequirementListFor(cohort.Id, cohort.TenantDepartmentId ?? Guid.Empty);
                    
                    foreach (var requirement in immunizationRequirements)
                    {
                        var categories = await _surpathManager.GetRecordCategoriesForRequirementAsync(requirement.Id);
                        
                        foreach (var category in categories)
                        {
                            // Get the user's record states for this category by querying the database directly
                            // IMPORTANT: Only consider current (non-archived) documents for compliance evaluation
                            var recordStates = await _recordStateRepository.GetAll()
                                .Include(rs => rs.RecordFk)
                                .Include(rs => rs.RecordStatusFk)
                                .Where(rs => rs.UserId == cohortUser.UserId && rs.RecordCategoryId == category.Id && !rs.IsDeleted && !rs.IsArchived)
                                .OrderByDescending(rs => rs.CreationTime)
                                .ToListAsync();

                            var latestRecordState = recordStates.FirstOrDefault();

                            var immunizationReq = new ImmunizationRequirementDto
                            {
                                RequirementName = requirement.Name,
                                CategoryName = category.Name,
                                ComplianceStatus = "Not Compliant",
                                AdministeredDate = null,
                                ExpirationDate = null
                            };

                            if (latestRecordState != null)
                            {
                                // Determine compliance status based on record status
                                immunizationReq.ComplianceStatus = latestRecordState.RecordStatusFk?.ComplianceImpact == EnumStatusComplianceImpact.Compliant ? "Compliant" : "Not Compliant";
                                immunizationReq.StatusColor = latestRecordState.RecordStatusFk?.HtmlColor ?? "#67c777"; // Default green for compliant
                                
                                // Get administered date (effective date) and expiration date from the record
                                if (latestRecordState.RecordFk != null)
                                {
                                    immunizationReq.AdministeredDate = latestRecordState.RecordFk.EffectiveDate;
                                    immunizationReq.ExpirationDate = latestRecordState.RecordFk.ExpirationDate;
                                }
                            }

                            memberData.ImmunizationRequirements.Add(immunizationReq);
                        }
                    }

                    cohortImmunizationData.Add(memberData);
                }
            }

            // Determine if we have multiple cohorts and should use multi-sheet format
            var uniqueCohorts = cohortImmunizationData.Select(c => c.CohortName).Distinct().Count();
            if (uniqueCohorts > 1)
            {
                return _cohortsExcelExporter.ExportImmunizationReportToFileMultiSheet(cohortImmunizationData);
            }

            return _cohortsExcelExporter.ExportImmunizationReportToFile(cohortImmunizationData);
        }

        public async Task<PagedResultDto<GetCohortForComplianceViewDto>> GetCompliance(GetAllCohortsInput input)
        //public async Task<List<GetCohortForComplianceViewDto>> GetCompliance(GetAllCohortsInput input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredCohorts = _cohortRepository.GetAll()
                        .Include(e => e.TenantDepartmentFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.DefaultCohortFilter.HasValue && input.DefaultCohortFilter > -1, e => (input.DefaultCohortFilter == 1 && e.DefaultCohort) || (input.DefaultCohortFilter == 0 && !e.DefaultCohort))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && (e.TenantDepartmentFk.Name.Contains(input.Filter) || e.TenantDepartmentFk.Description.Contains(input.Filter)))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter);

            var pagedAndFilteredCohorts = filteredCohorts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var cohorts = from o in pagedAndFilteredCohorts
                          join o1 in _tenantDepartmentLoookUpRepository.GetAll() on o.TenantDepartmentId equals o1.Id into j1
                          from s1 in j1.DefaultIfEmpty()

                          select new
                          {

                              o.Name,
                              o.Description,
                              o.DefaultCohort,
                              Id = o.Id,
                              TenantDepartmentName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                          };

            List<ComplianceCohortTotalsForViewDto> compliancetotals = await _surpathComplianceAppService.GetCohortCompliance(pagedAndFilteredCohorts);

            var totalCount = await filteredCohorts.CountAsync();

            var dbList = await cohorts.ToListAsync();
            var results = new List<GetCohortForComplianceViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCohortForComplianceViewDto()
                {
                    Cohort = new CohortComplianceDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        DefaultCohort = o.DefaultCohort,
                        Id = o.Id
                    },
                    TenantDepartmentName = o.TenantDepartmentName
                };
                var ctlist = compliancetotals.Where(ct => ct.CohortId.Equals(o.Id)).ToList();
                res.Cohort.ComplianceSummary = ctlist;
                res.ComplianceRecords = res.Cohort.ComplianceSummary.Select(ct => ct.Count).Sum();
                results.Add(res);
            }


            //return results;

            return new PagedResultDto<GetCohortForComplianceViewDto>(
                totalCount,
                results
            );
        }
        public async Task<List<GetCohortForComplianceViewDto>> GetComplianceList(GetAllCohortsInput input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredCohorts = _cohortRepository.GetAll()
                        .Include(e => e.TenantDepartmentFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.DefaultCohortFilter.HasValue && input.DefaultCohortFilter > -1, e => (input.DefaultCohortFilter == 1 && e.DefaultCohort) || (input.DefaultCohortFilter == 0 && !e.DefaultCohort))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && (e.TenantDepartmentFk.Name.Contains(input.Filter) || e.TenantDepartmentFk.Description.Contains(input.Filter)))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter);

            var pagedAndFilteredCohorts = filteredCohorts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var cohorts = from o in pagedAndFilteredCohorts
                          join o1 in _tenantDepartmentLoookUpRepository.GetAll() on o.TenantDepartmentId equals o1.Id into j1
                          from s1 in j1.DefaultIfEmpty()

                          select new
                          {

                              o.Name,
                              o.Description,
                              o.DefaultCohort,
                              Id = o.Id,
                              TenantDepartmentName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                          };

            List<ComplianceCohortTotalsForViewDto> compliancetotals = await _surpathComplianceAppService.GetCohortCompliance(pagedAndFilteredCohorts);

            var totalCount = await filteredCohorts.CountAsync();

            var dbList = await cohorts.ToListAsync();
            var results = new List<GetCohortForComplianceViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCohortForComplianceViewDto()
                {
                    Cohort = new CohortComplianceDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        DefaultCohort = o.DefaultCohort,
                        Id = o.Id
                    },
                    TenantDepartmentName = o.TenantDepartmentName
                };
                var ctlist = compliancetotals.Where(ct => ct.CohortId.Equals(o.Id)).ToList();
                res.Cohort.ComplianceSummary = ctlist;
                res.ComplianceRecords = res.Cohort.ComplianceSummary.Select(ct => ct.Count).Sum();
                results.Add(res);
            }


            return results;
        }
    }
}