using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using inzibackend.Authorization;
using inzibackend.Dto;
using inzibackend.Surpath.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Abp.Auditing;
using inzibackend.Authorization.Organizations;
using inzibackend.MultiTenancy;
using inzibackend.Surpath.Compliance;
using inzibackend.Surpath.ComplianceManager;
using Abp.Dependency;

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_CohortUsers_Transfer)]
    public class UserTransferAppService : inzibackendAppServiceBase, IUserTransferAppService
    {
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;
        private readonly IRepository<TenantDepartmentUser, Guid> _tenantDepartmentUserRepository;
        private readonly IRepository<RecordRequirement, Guid> _recordRequirementRepository;
        private readonly IRepository<RecordCategory, Guid> _recordCategoryRepository;
        private readonly IRepository<RecordState, Guid> _recordStateRepository;
        private readonly IRepository<RecordNote, Guid> _recordNoteRepository;
        private readonly IRepository<RecordCategoryRule, Guid> _recordCategoryRuleRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IOUSecurityManager _ouSecurityManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ISurpathComplianceEvaluator _surpathComplianceEvaluator;

        public UserTransferAppService(
            IRepository<Cohort, Guid> cohortRepository,
            IRepository<CohortUser, Guid> cohortUserRepository,
            IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
            IRepository<TenantDepartmentUser, Guid> tenantDepartmentUserRepository,
            IRepository<RecordRequirement, Guid> recordRequirementRepository,
            IRepository<RecordCategory, Guid> recordCategoryRepository,
            IRepository<RecordState, Guid> recordStateRepository,
            IRepository<RecordNote, Guid> recordNoteRepository,
            IRepository<RecordCategoryRule, Guid> recordCategoryRuleRepository,
            IRepository<Tenant> tenantRepository,
            IOUSecurityManager ouSecurityManager,
            IUnitOfWorkManager unitOfWorkManager,
            ISurpathComplianceEvaluator surpathComplianceEvaluator)
        {
            _cohortRepository = cohortRepository;
            _cohortUserRepository = cohortUserRepository;
            _tenantDepartmentRepository = tenantDepartmentRepository;
            _tenantDepartmentUserRepository = tenantDepartmentUserRepository;
            _recordRequirementRepository = recordRequirementRepository;
            _recordCategoryRepository = recordCategoryRepository;
            _recordStateRepository = recordStateRepository;
            _recordNoteRepository = recordNoteRepository;
            _recordCategoryRuleRepository = recordCategoryRuleRepository;
            _tenantRepository = tenantRepository;
            _ouSecurityManager = ouSecurityManager;
            _unitOfWorkManager = unitOfWorkManager;
            _surpathComplianceEvaluator = surpathComplianceEvaluator;
        }

        #region Analysis and Validation

        public async Task<UserTransferAnalysisDto> AnalyzeUserTransfer(Guid cohortId, Guid? targetDepartmentId)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var cohort = await _cohortRepository.GetAsync(cohortId);
            if (cohort == null)
                throw new UserFriendlyException(L("CohortNotFound"));

            var sourceDepartment = cohort.TenantDepartmentId.HasValue
                ? await _tenantDepartmentRepository.GetAsync(cohort.TenantDepartmentId.Value)
                : null;

            var usersCount = await _cohortUserRepository.CountAsync(cu => cu.CohortId == cohortId);
            var requirementCategories = await GetCohortRequirementCategories(cohortId);

            // Perform comprehensive business rules validation
            var validationResult = await ValidateTransferBusinessRules(cohort, sourceDepartment, targetDepartmentId, usersCount, requirementCategories);

            // If target department is specified, identify which requirements exist at both source and target
            var noTransferRequiredCategories = new List<RequirementCategoryAnalysisDto>();
            var categoriesToMap = requirementCategories;

            if (targetDepartmentId.HasValue && targetDepartmentId.Value != Guid.Empty)
            {
                // Get all requirements for the target department
                var targetCategories = await _surpathComplianceEvaluator.GetHierarchicalRequirementCategories(
                    departmentId: targetDepartmentId.Value,
                    cohortId: null,
                    includeInherited: true
                );
                targetCategories = targetCategories.Where(tc => tc.IsSurpathOnly == false).ToList();

                // Create a set of target requirement IDs for quick lookup
                var targetRequirementIds = new HashSet<Guid>(targetCategories.Select(tc => tc.RequirementId));

                // Separate categories that exist at both source and target (no transfer required)
                noTransferRequiredCategories = requirementCategories
                    .Where(rc => targetRequirementIds.Contains(rc.RequirementId))
                    .ToList();

                // Categories that need explicit mapping are those that don't exist at target
                categoriesToMap = requirementCategories
                    .Where(rc => !targetRequirementIds.Contains(rc.RequirementId))
                    .ToList();

                Logger.Info($"No transfer required categories: {noTransferRequiredCategories.Count}, Categories to map: {categoriesToMap.Count}");
            }

            var analysis = new UserTransferAnalysisDto
            {
                CohortId = cohortId,
                CohortName = cohort.Name,
                SourceDepartmentId = sourceDepartment?.Id ?? Guid.Empty,
                SourceDepartmentName = sourceDepartment?.Name ?? "No Department",
                TotalUsersCount = usersCount,
                RequirementCategories = categoriesToMap, // Only categories that need mapping
                NoTransferRequiredCategories = noTransferRequiredCategories, // Categories that apply before and after
                CanTransfer = validationResult.CanTransfer,
                TransferComplexity = DetermineTransferComplexity(requirementCategories),
                EstimatedDurationMinutes = CalculateEstimatedDuration(usersCount, requirementCategories.Count),
                Warnings = validationResult.Warnings
            };

            return analysis;
        }

        private async Task<TransferValidationResult> ValidateTransferBusinessRules(
            Cohort cohort,
            TenantDepartment sourceDepartment,
            Guid? targetDepartmentId,
            int usersCount,
            List<RequirementCategoryAnalysisDto> requirementCategories)
        {
            var result = new TransferValidationResult
            {
                CanTransfer = true,
                Warnings = new List<string>()
            };

            // Rule 1: Cohort must exist and be valid
            if (cohort == null)
            {
                result.CanTransfer = false;
                result.Warnings.Add(L("CohortNotFound"));
                return result;
            }

            // Rule 2: Cohort must not be in an active transfer state
            var hasActiveTransfer = await CheckForActiveTransfers(cohort.Id);
            if (hasActiveTransfer)
            {
                result.CanTransfer = false;
                result.Warnings.Add(L("CohortHasActiveTransfer"));
                return result;
            }

            // Rule 3: Validate target department if specified
            if (targetDepartmentId.HasValue && targetDepartmentId.Value != Guid.Empty)
            {
                var targetDepartment = await _tenantDepartmentRepository.FirstOrDefaultAsync(targetDepartmentId.Value);
                if (targetDepartment == null)
                {
                    result.CanTransfer = false;
                    result.Warnings.Add(L("TargetDepartmentNotFound"));
                    return result;
                }

                if (!targetDepartment.Active)
                {
                    result.CanTransfer = false;
                    result.Warnings.Add(L("TargetDepartmentInactive"));
                    return result;
                }

                // Rule 4: Allow transfer to the same department (removed this check)
                // Users can be transferred to different cohorts within the same department

                // Rule 5: Check tenant compatibility
                if (targetDepartment.TenantId != cohort.TenantId)
                {
                    result.CanTransfer = false;
                    result.Warnings.Add(L("CrossTenantTransferNotAllowed"));
                    return result;
                }
            }

            // Rule 6: Cohort must have users (or users must be selected if doing selective transfer)
            if (usersCount == 0)
            {
                result.CanTransfer = false;
                result.Warnings.Add(L("NoUsersToTransfer"));
                return result;
            }

            // Rule 7: Warning if large cohort
            if (usersCount > 1000)
            {
                result.Warnings.Add(L("LargeCohortTransferWarning", usersCount));
            }

            // Rule 8: Warning if many categories to map
            if (requirementCategories.Count > 50)
            {
                result.Warnings.Add(L("ManyCategoriesToMapWarning", requirementCategories.Count));
            }

            return result;
        }

        private async Task<bool> CheckForActiveTransfers(Guid cohortId)
        {
            // TODO: Implement check for active transfers
            return false;
        }

        private string DetermineTransferComplexity(List<RequirementCategoryAnalysisDto> categories)
        {
            if (categories.Count <= 5)
                return "Simple";
            else if (categories.Count <= 20)
                return "Moderate";
            else
                return "Complex";
        }

        private int CalculateEstimatedDuration(int usersCount, int categoriesCount)
        {
            // Base estimate: 1 minute per 100 users + 1 minute per 10 categories
            return Math.Max(1, (usersCount / 100) + (categoriesCount / 10));
        }

        #endregion Analysis and Validation

        #region Transfer Operations

        public async Task<UserTransferResultDto> ExecuteTransfer(UserTransferDto input)
        {
            return await TransferUsers(input);
        }

        public async Task<UserTransferResultDto> TransferUsers(UserTransferDto input)
        {
            if (input.TargetDepartmentId.HasValue)
            {
                return await TransferUsersToExistingDepartment(input);
            }
            else
            {
                return await TransferUsersToNewDepartment(input);
            }
        }

        public async Task<UserTransferResultDto> TransferUsersToNewDepartment(UserTransferDto input)
        {
            var transferId = Guid.NewGuid().ToString();
            var startTime = DateTime.UtcNow;

            try
            {
                // Create new department
                var newDepartmentId = await CreateDepartment(new CreateDepartmentDto
                {
                    Name = input.NewDepartmentName,
                    Description = input.NewDepartmentDescription,
                    TenantId = AbpSession.TenantId,
                    Active = true
                });

                // Set the target department and execute transfer
                input.TargetDepartmentId = newDepartmentId;
                var result = await TransferUsersToExistingDepartment(input);
                result.NewDepartmentId = newDepartmentId;

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error during user transfer to new department: {ex.Message}", ex);
                return new UserTransferResultDto
                {
                    Success = false,
                    Message = L("TransferFailed", ex.Message),
                    TransferId = transferId,
                    TransferStartTime = startTime,
                    TransferEndTime = DateTime.UtcNow,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<UserTransferResultDto> TransferUsersToExistingDepartment(UserTransferDto input)
        {
            var transferId = Guid.NewGuid().ToString();
            var startTime = DateTime.UtcNow;

            try
            {
                // TODO: Implement the actual transfer logic
                // This is a placeholder implementation

                var cohort = await _cohortRepository.GetAsync(input.CohortId);
                var usersCount = await _cohortUserRepository.CountAsync(cu => cu.CohortId == input.CohortId);

                // Update cohort department
                cohort.TenantDepartmentId = input.TargetDepartmentId;
                await _cohortRepository.UpdateAsync(cohort);

                await CurrentUnitOfWork.SaveChangesAsync();

                return new UserTransferResultDto
                {
                    Success = true,
                    Message = L("TransferSuccessful"),
                    AffectedUsersCount = usersCount,
                    TransferredRecordStatesCount = 0, // TODO: Calculate actual count
                    TransferId = transferId,
                    TransferStartTime = startTime,
                    TransferEndTime = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                Logger.Error($"Error during user transfer: {ex.Message}", ex);
                return new UserTransferResultDto
                {
                    Success = false,
                    Message = L("TransferFailed", ex.Message),
                    TransferId = transferId,
                    TransferStartTime = startTime,
                    TransferEndTime = DateTime.UtcNow,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        #endregion Transfer Operations

        #region Category Management

        public async Task<List<TargetCategoryOptionDto>> GetTargetCategoryOptions(GetTargetCategoryOptionsInput input)
        {
            List<HierarchicalRequirementCategoryDto> targetCategories;

            // Handle cohorts without departments (null or Guid.Empty)
            if (!input.TargetDepartmentId.HasValue || input.TargetDepartmentId.Value == Guid.Empty)
            {
                // Get tenant-level requirements and cohort-specific requirements if cohort is specified
                targetCategories = await _surpathComplianceEvaluator.GetHierarchicalRequirementCategories(
                    departmentId: null,
                    cohortId: input.TargetCohortId,
                    includeInherited: true
                );
            }
            else
            {
                var targetDepartment = await _tenantDepartmentRepository.GetAsync(input.TargetDepartmentId.Value);
                if (targetDepartment == null)
                    throw new UserFriendlyException(L("TargetDepartmentNotFound"));

                // Get all categories for the target department and cohort
                targetCategories = await _surpathComplianceEvaluator.GetHierarchicalRequirementCategories(
                    departmentId: targetDepartment.Id,
                    cohortId: input.TargetCohortId,
                    includeInherited: true
                );
            }

            // If a source category is specified, calculate match scores
            if (input.SourceCategoryId != Guid.Empty)
            {
                var sourceCategory = await _recordCategoryRepository.GetAll()
                    .Include(c => c.RecordRequirementFk)
                    .FirstOrDefaultAsync(c => c.Id == input.SourceCategoryId);

                if (sourceCategory != null)
                {
                    // Convert to DTOs with match scores
                    var result = targetCategories
                        .Where(tc => !tc.IsSurpathOnly)
                        .Select(tc => new TargetCategoryOptionDto
                        {
                            CategoryId = tc.CategoryId,
                            CategoryName = tc.CategoryName,
                            RequirementId = tc.RequirementId,
                            RequirementName = tc.RequirementName,
                            MatchScore = CalculateMatchScore(sourceCategory, tc)
                        })
                        .OrderByDescending(tc => tc.MatchScore)
                        .ToList();

                    return result;
                }
            }

            // Convert to DTOs without match scores
            var resultWithoutScores = targetCategories
                .Where(tc => !tc.IsSurpathOnly)
                .Select(tc => new TargetCategoryOptionDto
                {
                    CategoryId = tc.CategoryId,
                    CategoryName = tc.CategoryName,
                    RequirementId = tc.RequirementId,
                    RequirementName = tc.RequirementName,
                    MatchScore = 0
                })
                .ToList();

            return resultWithoutScores;
        }

        private int CalculateMatchScore(RecordCategory sourceCategory, HierarchicalRequirementCategoryDto targetCategory)
        {
            var sourceName = (sourceCategory.Name ?? "").ToLower();
            var targetName = (targetCategory.CategoryName ?? "").ToLower();
            var sourceReqName = (sourceCategory.RecordRequirementFk?.Name ?? "").ToLower();
            var targetReqName = (targetCategory.RequirementName ?? "").ToLower();

            // Exact match
            if (sourceName == targetName && sourceReqName == targetReqName)
                return 100;

            // Category name exact match
            if (sourceName == targetName)
                return 80;

            // Requirement name exact match
            if (sourceReqName == targetReqName)
                return 70;

            // Partial matches
            var score = 0;
            if (sourceName.Contains(targetName) || targetName.Contains(sourceName))
                score += 40;
            if (sourceReqName.Contains(targetReqName) || targetReqName.Contains(sourceReqName))
                score += 30;

            return Math.Min(score, 60); // Cap partial matches at 60%
        }

        public async Task<bool> ValidateMigrationMappings(ValidateMigrationMappingsInput input)
        {
            // TODO: Implement validation logic
            return true;
        }

        #endregion Category Management

        #region Department Management

        public async Task<List<DepartmentLookupDto>> GetAvailableTargetDepartments(Guid excludeDepartmentId)
        {
            var departments = await _tenantDepartmentRepository.GetAll()
                .Where(d => d.Id != excludeDepartmentId && d.Active)
                .OrderBy(d => d.Name)
                .Select(d => new DepartmentLookupDto
                {
                    DepartmentId = d.Id,
                    DepartmentName = d.Name,
                    DepartmentDescription = d.Description,
                    IsActive = d.Active
                })
                .ToListAsync();

            return departments;
        }

        public async Task<DepartmentSelectionValidationResultDto> ValidateDepartmentSelection(ValidateDepartmentSelectionInput input)
        {
            var result = new DepartmentSelectionValidationResultDto
            {
                IsValid = true,
                Errors = new List<string>(),
                Warnings = new List<string>()
            };

            if (input.TargetDepartmentId.HasValue)
            {
                var department = await _tenantDepartmentRepository.FirstOrDefaultAsync(input.TargetDepartmentId.Value);
                if (department == null)
                {
                    result.IsValid = false;
                    result.Errors.Add(L("DepartmentNotFound"));
                }
                else if (!department.Active)
                {
                    result.IsValid = false;
                    result.Errors.Add(L("DepartmentInactive"));
                }
            }
            else if (string.IsNullOrWhiteSpace(input.NewDepartmentName))
            {
                result.IsValid = false;
                result.Errors.Add(L("DepartmentNameRequired"));
            }

            return result;
        }

        public async Task<Guid> CreateDepartment(CreateDepartmentDto input)
        {
            var department = new TenantDepartment
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                Description = input.Description,
                TenantId = input.TenantId ?? AbpSession.TenantId,
                Active = input.Active
            };

            await _tenantDepartmentRepository.InsertAsync(department);
            await CurrentUnitOfWork.SaveChangesAsync();

            return department.Id;
        }

        #endregion Department Management

        #region Utility Methods

        public async Task<int> GetCohortUsersCount(Guid cohortId)
        {
            return await _cohortUserRepository.CountAsync(cu => cu.CohortId == cohortId);
        }

        public async Task<List<RequirementCategoryAnalysisDto>> GetCohortRequirementCategories(Guid cohortId)
        {
            var cohort = await _cohortRepository.GetAsync(cohortId);
            var departmentId = cohort.TenantDepartmentId ?? Guid.Empty;

            var categories = await _surpathComplianceEvaluator.GetHierarchicalRequirementCategories(
                departmentId: departmentId,
                cohortId: cohortId,
                includeInherited: true
            );

            return categories
                .Select(c => new RequirementCategoryAnalysisDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    RequirementId = c.RequirementId,
                    RequirementName = c.RequirementName,
                    IsDepartmentSpecific = c.HierarchyLevel == "Department",
                    IsCohortSpecific = c.HierarchyLevel == "Cohort" || c.HierarchyLevel == "CohortAndDepartment",
                    HierarchyLevel = c.HierarchyLevel,
                    RequiresMapping = true,
                    RecordStatesCount = 0, // TODO: Calculate actual count
                    AffectedUsersCount = 0, // TODO: Calculate actual count
                    IsSurpathOnly = c.IsSurpathOnly
                })
                .ToList();
        }

        public async Task<bool> CanTransferUsers(Guid cohortId)
        {
            var analysis = await AnalyzeUserTransfer(cohortId, null);
            return analysis.CanTransfer;
        }

        // New method for getting cohorts by department
        public async Task<List<CohortLookupDto>> GetCohortsForDepartment(Guid departmentId)
        {
            var cohorts = await _cohortRepository.GetAll()
                .Where(c => c.TenantDepartmentId == departmentId)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var result = new List<CohortLookupDto>();
            foreach (var cohort in cohorts)
            {
                var usersCount = await _cohortUserRepository.CountAsync(cu => cu.CohortId == cohort.Id);
                result.Add(new CohortLookupDto
                {
                    Id = cohort.Id,
                    Name = cohort.Name,
                    Description = cohort.Description,
                    UsersCount = usersCount,
                    IsDefaultCohort = cohort.DefaultCohort
                });
            }

            return result;
        }

        // New method for getting cohorts without department
        public async Task<List<CohortLookupDto>> GetCohortsWithoutDepartment()
        {
            var cohorts = await _cohortRepository.GetAll()
                .Where(c => c.TenantDepartmentId == null)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var result = new List<CohortLookupDto>();
            foreach (var cohort in cohorts)
            {
                var usersCount = await _cohortUserRepository.CountAsync(cu => cu.CohortId == cohort.Id);
                result.Add(new CohortLookupDto
                {
                    Id = cohort.Id,
                    Name = cohort.Name,
                    Description = cohort.Description,
                    UsersCount = usersCount,
                    IsDefaultCohort = cohort.DefaultCohort
                });
            }

            return result;
        }

        // New method for getting cohort users for transfer
        public async Task<PagedResultDto<CohortUserForTransferDto>> GetCohortUsersForTransfer(GetCohortUsersForTransferInput input)
        {
            var query = _cohortUserRepository.GetAll()
                .Include(cu => cu.UserFk)
                .Where(cu => cu.CohortId == input.CohortId)
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    cu => cu.UserFk.Name.Contains(input.Filter) ||
                          cu.UserFk.Surname.Contains(input.Filter) ||
                          cu.UserFk.UserName.Contains(input.Filter) ||
                          cu.UserFk.EmailAddress.Contains(input.Filter));

            var totalCount = await query.CountAsync();

            var cohortUsers = await query
                .PageBy(input)
                .ToListAsync();

            var cohortUserDtos = new List<CohortUserForTransferDto>();
            foreach (var cohortUser in cohortUsers)
            {
                var recordStatesCount = await _recordStateRepository.CountAsync(rs => rs.UserId == cohortUser.UserId);
                cohortUserDtos.Add(new CohortUserForTransferDto
                {
                    Id = cohortUser.Id,
                    UserId = cohortUser.UserId,
                    UserName = cohortUser.UserFk?.UserName,
                    FullName = cohortUser.UserFk?.FullName,
                    Email = cohortUser.UserFk?.EmailAddress,
                    ComplianceStatus = "Pending", // TODO: Calculate actual compliance status
                    RecordStatesCount = recordStatesCount
                });
            }

            return new PagedResultDto<CohortUserForTransferDto>(totalCount, cohortUserDtos);
        }

        // New method for analyzing selective user transfer
        public async Task<UserTransferAnalysisDto> AnalyzeSelectiveUserTransfer(AnalyzeUserTransferInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var sourceCohort = await _cohortRepository.GetAsync(input.SourceCohortId);
            var targetCohort = await _cohortRepository.GetAsync(input.TargetCohortId);

            if (sourceCohort == null)
                throw new UserFriendlyException(L("SourceCohortNotFound"));

            if (targetCohort == null)
                throw new UserFriendlyException(L("TargetCohortNotFound"));

            var sourceDepartment = sourceCohort.TenantDepartmentId.HasValue
                ? await _tenantDepartmentRepository.GetAsync(sourceCohort.TenantDepartmentId.Value)
                : null;

            var targetDepartment = targetCohort.TenantDepartmentId.HasValue
                ? await _tenantDepartmentRepository.GetAsync(targetCohort.TenantDepartmentId.Value)
                : null;

            var totalUsersCount = await _cohortUserRepository.CountAsync(cu => cu.CohortId == input.SourceCohortId);
            var selectedUsersCount = input.SelectedCohortUserIds?.Count ?? 0;

            var requirementCategories = await GetCohortRequirementCategories(input.SourceCohortId);
            var requiresCategoryMapping = sourceCohort.TenantDepartmentId != targetCohort.TenantDepartmentId;

            // Perform comprehensive business rules validation
            // For initial analysis, validate based on total users in cohort, not selected users
            var usersCountForValidation = selectedUsersCount > 0 ? selectedUsersCount : totalUsersCount;
            var validationResult = await ValidateTransferBusinessRules(sourceCohort, sourceDepartment, targetCohort.TenantDepartmentId, usersCountForValidation, requirementCategories);
            
            // Additional validation: Cannot transfer to the same cohort
            if (sourceCohort.Id == targetCohort.Id)
            {
                validationResult.CanTransfer = false;
                validationResult.Warnings.Add(L("CannotTransferToSameCohort"));
            }

            // If different departments, identify which requirements exist at both source and target
            var noTransferRequiredCategories = new List<RequirementCategoryAnalysisDto>();
            var categoriesToMap = requirementCategories;

            if (requiresCategoryMapping && targetDepartment != null)
            {
                // Get all requirements for the target department
                var targetCategories = await _surpathComplianceEvaluator.GetHierarchicalRequirementCategories(
                    departmentId: targetDepartment.Id,
                    cohortId: targetCohort.Id,
                    includeInherited: true
                );

                // Separate categories that exist in both departments from those that need mapping
                noTransferRequiredCategories = requirementCategories.Where(rc =>
                    targetCategories.Any(tc => tc.CategoryName == rc.CategoryName && tc.RequirementName == rc.RequirementName)
                ).ToList();

                categoriesToMap = requirementCategories.Where(rc =>
                    !targetCategories.Any(tc => tc.CategoryName == rc.CategoryName && tc.RequirementName == rc.RequirementName)
                ).ToList();
            }

            var result = new UserTransferAnalysisDto
            {
                CohortId = input.SourceCohortId,
                CohortName = sourceCohort.Name,
                SourceDepartmentId = sourceDepartment?.Id ?? Guid.Empty,
                SourceDepartmentName = sourceDepartment?.Name ?? L("NoDepartment"),
                TargetCohortId = targetCohort.Id,
                TargetCohortName = targetCohort.Name,
                TargetDepartmentId = targetDepartment?.Id,
                TargetDepartmentName = targetDepartment?.Name ?? L("NoDepartment"),
                TotalUsersCount = totalUsersCount,
                SelectedUsersCount = selectedUsersCount,
                RequirementCategories = categoriesToMap,
                NoTransferRequiredCategories = noTransferRequiredCategories,
                Warnings = validationResult.Warnings,
                CanTransfer = validationResult.CanTransfer,
                TransferComplexity = DetermineTransferComplexity(categoriesToMap),
                EstimatedDurationMinutes = CalculateEstimatedDuration(selectedUsersCount, categoriesToMap.Count),
                IsSurpathOnly = requirementCategories.All(c => c.IsSurpathOnly),
                RequiresCategoryMapping = requiresCategoryMapping
            };

            return result;
        }

        // New method for transferring selected users
        public async Task<UserTransferResultDto> TransferSelectedUsers(TransferSelectedUsersInput input)
        {
            var transferId = Guid.NewGuid().ToString();
            var startTime = DateTime.UtcNow;

            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    // Validate input
                    if (!input.SelectedCohortUserIds.Any())
                        throw new UserFriendlyException(L("NoUsersSelected"));

                    // Get source and target cohorts
                    var sourceCohort = await _cohortRepository.GetAsync(input.SourceCohortId);
                    var targetCohort = await _cohortRepository.GetAsync(input.TargetCohortId);

                    if (sourceCohort.Id == targetCohort.Id)
                        throw new UserFriendlyException(L("CannotTransferToSameCohort"));

                    // Category mapping is required when departments are different (including null vs non-null)
                    var requiresCategoryMapping = sourceCohort.TenantDepartmentId != targetCohort.TenantDepartmentId;
                    var transferredRecordStatesCount = 0;
                    var createdNewCategories = new Dictionary<Guid, Guid>(); // Track source -> new category mappings
                    
                    // Log input data for debugging
                    Logger.Info($"Transfer input - RequiresCategoryMapping: {requiresCategoryMapping}");
                    if (input.CategoryMappings != null)
                        Logger.Info($"CategoryMappings count: {input.CategoryMappings.Count}, Keys: {string.Join(", ", input.CategoryMappings.Keys)}");
                    if (input.NewCategories != null)
                    {
                        Logger.Info($"NewCategories count: {input.NewCategories.Count}, Keys: {string.Join(", ", input.NewCategories.Keys)}");
                        foreach (var kvp in input.NewCategories)
                        {
                            Logger.Info($"  Category {kvp.Key}: Requirement='{kvp.Value.Requirement}', Category='{kvp.Value.Category}'");
                        }
                    }
                    if (input.SkippedCategories != null)
                        Logger.Info($"SkippedCategories count: {input.SkippedCategories.Count}");

                    // Process each selected user
                    foreach (var cohortUserId in input.SelectedCohortUserIds)
                    {
                        var cohortUser = await _cohortUserRepository.GetAsync(cohortUserId);
                        if (cohortUser.CohortId != input.SourceCohortId)
                        {
                            Logger.Warn($"CohortUser {cohortUserId} is not in source cohort {input.SourceCohortId}");
                            continue;
                        }

                        // Update the cohort user's cohort
                        cohortUser.CohortId = input.TargetCohortId;
                        await _cohortUserRepository.UpdateAsync(cohortUser);

                        // Handle record states if different departments
                        if (requiresCategoryMapping)
                        {
                            var userRecordStates = await _recordStateRepository.GetAll()
                                .Where(rs => rs.UserId == cohortUser.UserId && !rs.IsArchived)
                                .Include(rs => rs.RecordCategoryFk)
                                .ThenInclude(rc => rc.RecordRequirementFk)
                                .ToListAsync();
                                
                            Logger.Info($"Found {userRecordStates.Count} record states for user {cohortUser.UserId}");

                            // Get all categories that were analyzed for mapping
                            var analyzedCategoryIds = new HashSet<Guid>();
                            if (input.CategoryMappings != null) analyzedCategoryIds.UnionWith(input.CategoryMappings.Keys);
                            if (input.NewCategories != null) analyzedCategoryIds.UnionWith(input.NewCategories.Keys);
                            if (input.SkippedCategories != null) analyzedCategoryIds.UnionWith(input.SkippedCategories);
                            
                            // IMPORTANT: Also include categories that didn't require mapping (e.g., tenant-wide requirements)
                            // These categories exist in both source and target and should NOT be archived
                            if (input.NoTransferRequiredCategoryIds != null) 
                                analyzedCategoryIds.UnionWith(input.NoTransferRequiredCategoryIds);
                            
                            Logger.Info($"Analyzed category IDs: {string.Join(", ", analyzedCategoryIds)}");
                            
                            // Process new categories that need to be created
                            if (input.NewCategories != null && input.NewCategories.Count > 0)
                            {
                                var recordStateCategoryIds = userRecordStates
                                    .Where(rs => rs.RecordCategoryId.HasValue)
                                    .Select(rs => rs.RecordCategoryId.Value)
                                    .ToHashSet();
                                    
                                foreach (var kvp in input.NewCategories)
                                {
                                    var categoryId = kvp.Key;
                                    var newCategoryData = kvp.Value;
                                    
                                    // Create the new requirement/category even if user has no record state
                                    // But only create it once, not for every user
                                    if (!recordStateCategoryIds.Contains(categoryId) && !createdNewCategories.ContainsKey(categoryId))
                                    {
                                        Logger.Info($"Creating new requirement/category for {categoryId} (user has no record state)");
                                        
                                        try
                                        {
                                            var newCategoryId = await CreateNewRequirementAndCategory(
                                                categoryId, 
                                                newCategoryData,
                                                sourceCohort.TenantDepartmentId,
                                                targetCohort.TenantDepartmentId,
                                                targetCohort.Id
                                            );
                                            
                                            createdNewCategories[categoryId] = newCategoryId;
                                            Logger.Info($"Successfully created new category {newCategoryId} without record state");
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.Error($"Failed to create new requirement/category for {categoryId}: {ex.Message}", ex);
                                            throw;
                                        }
                                    }
                                }
                            }

                            foreach (var recordState in userRecordStates)
                            {
                                if (recordState.RecordCategoryId.HasValue)
                                {
                                    var categoryId = recordState.RecordCategoryId.Value;
                                    Logger.Info($"Processing record state {recordState.Id} with category {categoryId}");
                                    
                                    // Check if this category should be mapped to existing
                                    if (input.CategoryMappings != null && input.CategoryMappings.ContainsKey(categoryId))
                                    {
                                        recordState.RecordCategoryId = input.CategoryMappings[categoryId];
                                        await _recordStateRepository.UpdateAsync(recordState);
                                        transferredRecordStatesCount++;
                                    }
                                    // Check if this category should be skipped
                                    else if (input.SkippedCategories != null && input.SkippedCategories.Contains(categoryId))
                                    {
                                        // Archive or mark as orphaned
                                        recordState.IsArchived = true;
                                        await _recordStateRepository.UpdateAsync(recordState);
                                        Logger.Info($"Archived record state for skipped category {categoryId}");
                                    }
                                    // Check if this category needs to be created new
                                    else if (input.NewCategories != null && input.NewCategories.ContainsKey(categoryId))
                                    {
                                        Guid newCategoryId;
                                        
                                        // Check if we already created this category for another user
                                        if (createdNewCategories.ContainsKey(categoryId))
                                        {
                                            newCategoryId = createdNewCategories[categoryId];
                                            Logger.Info($"Using previously created category {newCategoryId} for record state");
                                        }
                                        else
                                        {
                                            // Create the new requirement/category
                                            Logger.Info($"Creating new requirement/category for categoryId: {categoryId}");
                                            var newCategoryData = input.NewCategories[categoryId];
                                            Logger.Info($"New category data - Requirement: '{newCategoryData.Requirement ?? "null"}', " +
                                                       $"Category: '{newCategoryData.Category ?? "null"}', " +
                                                       $"ConfirmedDepartmentLevel: {newCategoryData.ConfirmedDepartmentLevel}");
                                            
                                            try
                                            {
                                                newCategoryId = await CreateNewRequirementAndCategory(
                                                    categoryId, 
                                                    newCategoryData,
                                                    sourceCohort.TenantDepartmentId,
                                                    targetCohort.TenantDepartmentId,
                                                    targetCohort.Id
                                                );
                                                
                                                createdNewCategories[categoryId] = newCategoryId;
                                                Logger.Info($"Successfully created new category with ID: {newCategoryId}");
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.Error($"Failed to create new requirement/category for categoryId {categoryId}: {ex.Message}", ex);
                                                throw;
                                            }
                                        }
                                        
                                        // Update the record state to point to the new category
                                        recordState.RecordCategoryId = newCategoryId;
                                        await _recordStateRepository.UpdateAsync(recordState);
                                        transferredRecordStatesCount++;
                                        
                                        Logger.Info($"Updated record state {recordState.Id} to point to new category {newCategoryId}");
                                    }
                                    // If this category wasn't analyzed (meaning it's no longer applicable in target)
                                    else if (!analyzedCategoryIds.Contains(categoryId))
                                    {
                                        // Archive the record state as it's no longer applicable
                                        recordState.IsArchived = true;
                                        await _recordStateRepository.UpdateAsync(recordState);
                                        
                                        var requirementName = recordState.RecordCategoryFk?.RecordRequirementFk?.Name ?? "Unknown";
                                        Logger.Info($"Archived record state for category {categoryId} (requirement: {requirementName}) " +
                                                   $"as it's no longer applicable in target cohort");
                                    }
                                }
                            }
                        }

                        // Update TenantDepartmentUser if departments are different
                        if (sourceCohort.TenantDepartmentId != targetCohort.TenantDepartmentId)
                        {
                            // If source cohort had a department, find the user's department assignment
                            if (sourceCohort.TenantDepartmentId.HasValue)
                            {
                                var existingDeptUser = await _tenantDepartmentUserRepository.FirstOrDefaultAsync(
                                    tdu => tdu.UserId == cohortUser.UserId &&
                                           tdu.TenantDepartmentId == sourceCohort.TenantDepartmentId);

                                if (existingDeptUser != null)
                                {
                                    if (targetCohort.TenantDepartmentId.HasValue)
                                    {
                                        // Update to new department
                                        existingDeptUser.TenantDepartmentId = targetCohort.TenantDepartmentId.Value;
                                        await _tenantDepartmentUserRepository.UpdateAsync(existingDeptUser);
                                    }
                                    else
                                    {
                                        // Target cohort has no department - remove department assignment
                                        await _tenantDepartmentUserRepository.DeleteAsync(existingDeptUser);
                                    }
                                }
                            }
                            else if (targetCohort.TenantDepartmentId.HasValue)
                            {
                                // Source had no department but target does - create new department assignment
                                var newDeptUser = new TenantDepartmentUser
                                {
                                    TenantId = AbpSession.TenantId,
                                    TenantDepartmentId = targetCohort.TenantDepartmentId.Value,
                                    UserId = cohortUser.UserId
                                };
                                await _tenantDepartmentUserRepository.InsertAsync(newDeptUser);
                            }
                            // If both source and target have no department, no action needed
                        }
                    }

                    await unitOfWork.CompleteAsync();

                    // Recalculate compliance for affected users
                    foreach (var cohortUserId in input.SelectedCohortUserIds)
                    {
                        var cohortUser = await _cohortUserRepository.GetAsync(cohortUserId);
                        await _surpathComplianceEvaluator.RecalculateUserCompliance(cohortUser.UserId);
                    }

                    return new UserTransferResultDto
                    {
                        Success = true,
                        Message = L("TransferSuccessful"),
                        AffectedUsersCount = input.SelectedCohortUserIds.Count,
                        TransferredRecordStatesCount = transferredRecordStatesCount,
                        TransferId = transferId,
                        TransferStartTime = startTime,
                        TransferEndTime = DateTime.UtcNow
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error during selective user transfer: {ex.Message}", ex);
                return new UserTransferResultDto
                {
                    Success = false,
                    Message = L("TransferFailed", ex.Message),
                    TransferId = transferId,
                    TransferStartTime = startTime,
                    TransferEndTime = DateTime.UtcNow,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        #endregion Utility Methods

        #region Transfer History

        public async Task<PagedResultDto<UserTransferHistoryDto>> GetTransferHistory(GetAllUserTransferHistoryInput input)
        {
            // TODO: Implement transfer history retrieval
            return new PagedResultDto<UserTransferHistoryDto>
            {
                TotalCount = 0,
                Items = new List<UserTransferHistoryDto>()
            };
        }

        public async Task<UserTransferHistoryDto> GetTransferHistoryForView(Guid transferId)
        {
            // TODO: Implement single transfer history retrieval
            return new UserTransferHistoryDto();
        }

        public async Task<FileDto> GetTransferHistoryToExcel(GetAllUserTransferHistoryInput input)
        {
            // TODO: Implement Excel export
            return new FileDto();
        }

        #endregion Transfer History

        #region Rollback and Recovery

        public async Task<bool> CanRollbackTransfer(Guid transferId)
        {
            // TODO: Implement rollback check
            return false;
        }

        public async Task<UserTransferRollbackResultDto> RollbackTransfer(UserTransferRollbackDto input)
        {
            // TODO: Implement rollback functionality
            return new UserTransferRollbackResultDto
            {
                Success = false,
                Message = L("RollbackNotImplemented")
            };
        }

        #endregion Rollback and Recovery

        #region Progress Tracking

        public async Task<UserTransferProgressDto> GetTransferProgress(string transferId)
        {
            // TODO: Implement progress tracking
            return new UserTransferProgressDto
            {
                TransferId = transferId,
                Status = "Unknown",
                ProgressPercentage = 0
            };
        }

        public async Task UpdateTransferProgress(string transferId, TransferProgressUpdateDto progressUpdate)
        {
            // TODO: Implement progress update
            await Task.CompletedTask;
        }

        public async Task<TransferProgressReportDto> GenerateTransferProgressReport(string transferId)
        {
            // TODO: Implement progress report generation
            return new TransferProgressReportDto
            {
                TransferId = transferId,
                Status = "Unknown"
            };
        }

        public async Task<List<TransferProgressHistoryDto>> GetTransferProgressHistory(string transferId)
        {
            // TODO: Implement progress history retrieval
            return new List<TransferProgressHistoryDto>();
        }

        public async Task<ProgressDataCleanupResultDto> CleanupProgressData(int retentionDays = 90)
        {
            // TODO: Implement cleanup
            return new ProgressDataCleanupResultDto
            {
                Success = true,
                Message = L("CleanupNotImplemented")
            };
        }

        #endregion Progress Tracking

        #region Requirement and Category Creation

        private async Task<Guid> CreateNewRequirementAndCategory(
            Guid sourceCategoryId, 
            NewCategoryDto newCategoryData,
            Guid? sourceDepartmentId,
            Guid? targetDepartmentId,
            Guid targetCohortId)
        {
            Logger.Info($"CreateNewRequirementAndCategory called - SourceCategoryId: {sourceCategoryId}, " +
                       $"SourceDeptId: {sourceDepartmentId}, TargetDeptId: {targetDepartmentId}, " +
                       $"TargetCohortId: {targetCohortId}");
            
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            // Get source category and requirement details
            var sourceCategory = await _recordCategoryRepository.GetAll()
                .Include(rc => rc.RecordRequirementFk)
                .FirstOrDefaultAsync(rc => rc.Id == sourceCategoryId);

            if (sourceCategory == null)
            {
                Logger.Error($"Source category not found: {sourceCategoryId}");
                throw new UserFriendlyException(L("SourceCategoryNotFound"));
            }

            var sourceRequirement = sourceCategory.RecordRequirementFk;
            Logger.Info($"Source requirement: Name='{sourceRequirement?.Name}', " +
                       $"DeptId={sourceRequirement?.TenantDepartmentId}, " +
                       $"CohortId={sourceRequirement?.CohortId}");
            
            // Determine the hierarchy level of the source requirement
            var sourceHierarchyLevel = DetermineHierarchyLevel(sourceRequirement);
            Logger.Info($"Source hierarchy level: {sourceHierarchyLevel}");
            
            // Check if creating at department level requires confirmation
            if (sourceHierarchyLevel == "Department" && !newCategoryData.ConfirmedDepartmentLevel)
            {
                Logger.Warn("Department level creation requires confirmation but not confirmed");
                throw new UserFriendlyException(L("DepartmentLevelCreationRequiresConfirmation"));
            }

            // Create new requirement at the appropriate hierarchy level
            var newRequirement = new RecordRequirement
            {
                Name = newCategoryData.Requirement,
                Description = sourceRequirement?.Description ?? $"Created during user transfer from {sourceCategory.RecordRequirementFk?.Name}",
                TenantId = AbpSession.TenantId,
                IsSurpathOnly = sourceRequirement?.IsSurpathOnly ?? false,
                Metadata = sourceRequirement?.Metadata ?? JsonConvert.SerializeObject(new
                {
                    CreatedBy = "UserTransfer",
                    SourceRequirementId = sourceRequirement?.Id,
                    SourceRequirementName = sourceRequirement?.Name,
                    CreatedAt = DateTime.UtcNow
                })
            };

            // Set hierarchy level based on source requirement
            switch (sourceHierarchyLevel)
            {
                case "Tenant":
                    // Tenant level - no department or cohort
                    newRequirement.TenantDepartmentId = null;
                    newRequirement.CohortId = null;
                    Logger.Info($"Creating tenant-level requirement: {newRequirement.Name}");
                    break;
                    
                case "Department":
                    // Department level - set target department (can be null for cohorts without departments)
                    newRequirement.TenantDepartmentId = targetDepartmentId;
                    newRequirement.CohortId = null;
                    Logger.Info($"Creating department-level requirement: {newRequirement.Name} for department {targetDepartmentId?.ToString() ?? "null"}");
                    break;
                    
                case "Cohort":
                case "CohortAndDepartment":
                    // Cohort level - set both department (can be null) and cohort
                    newRequirement.TenantDepartmentId = targetDepartmentId;
                    newRequirement.CohortId = targetCohortId;
                    Logger.Info($"Creating cohort-level requirement: {newRequirement.Name} for cohort {targetCohortId} in department {targetDepartmentId?.ToString() ?? "null"}");
                    break;
                    
                default:
                    // Default to cohort level for safety
                    newRequirement.TenantDepartmentId = targetDepartmentId;
                    newRequirement.CohortId = targetCohortId;
                    Logger.Warn($"Unknown hierarchy level '{sourceHierarchyLevel}', defaulting to cohort level for department {targetDepartmentId?.ToString() ?? "null"}");
                    break;
            }

            // ABP should assign the ID when we insert
            newRequirement.Id = Guid.NewGuid();
            await _recordRequirementRepository.InsertAsync(newRequirement);
            
            Logger.Info($"Inserted new requirement: ID={newRequirement.Id}, Name='{newRequirement.Name}', " +
                       $"DeptId={newRequirement.TenantDepartmentId}, CohortId={newRequirement.CohortId}, " +
                       $"TenantId={newRequirement.TenantId}");

            // Create new category under the new requirement
            var newCategory = new RecordCategory
            {
                Name = newCategoryData.Category,
                Instructions = sourceCategory.Instructions,
                TenantId = AbpSession.TenantId,
                RecordRequirementId = newRequirement.Id
            };

            // Copy the rule reference if the source category has one
            if (sourceCategory.RecordCategoryRuleId.HasValue)
            {
                newCategory.RecordCategoryRuleId = sourceCategory.RecordCategoryRuleId;
                Logger.Info($"Copying rule reference: {sourceCategory.RecordCategoryRuleId}");
            }

            // Assign ID before inserting
            newCategory.Id = Guid.NewGuid();
            await _recordCategoryRepository.InsertAsync(newCategory);

            Logger.Info($"Created new requirement '{newRequirement.Name}' (ID: {newRequirement.Id}) " +
                       $"and category '{newCategory.Name}' (ID: {newCategory.Id}) " +
                       $"at {sourceHierarchyLevel} level for user transfer");

            // Verify the requirement can be found (for debugging)
            var verifyRequirement = await _recordRequirementRepository.FirstOrDefaultAsync(r => r.Id == newRequirement.Id);
            if (verifyRequirement == null)
            {
                Logger.Error($"Failed to verify creation of requirement {newRequirement.Id}");
            }
            else
            {
                Logger.Info($"Verified requirement exists in repository: {verifyRequirement.Id}");
            }

            return newCategory.Id;
        }

        private string DetermineHierarchyLevel(RecordRequirement requirement)
        {
            if (requirement == null)
                return "Unknown";

            // Determine hierarchy based on what IDs are set
            if (!requirement.TenantDepartmentId.HasValue && !requirement.CohortId.HasValue)
            {
                return "Tenant";
            }
            else if (requirement.TenantDepartmentId.HasValue && !requirement.CohortId.HasValue)
            {
                return "Department";
            }
            else if (requirement.TenantDepartmentId.HasValue && requirement.CohortId.HasValue)
            {
                return "CohortAndDepartment";
            }
            else if (!requirement.TenantDepartmentId.HasValue && requirement.CohortId.HasValue)
            {
                // This shouldn't happen but handle it
                return "Cohort";
            }

            return "Unknown";
        }

        #endregion Requirement and Category Creation

        #region Compliance State Preservation

        public async Task<CompliancePreservationResultDto> PreserveUserCompliance(Guid cohortId, string transferId)
        {
            // TODO: Implement compliance preservation
            return new CompliancePreservationResultDto
            {
                Success = true,
                Message = L("CompliancePreserved"),
                TransferId = transferId
            };
        }

        public async Task<ComplianceIntegrityValidationResultDto> ValidateComplianceIntegrity(
            Guid cohortId,
            string transferId,
            CompliancePreservationResultDto preservationResult = null)
        {
            // TODO: Implement compliance validation
            return new ComplianceIntegrityValidationResultDto
            {
                CohortId = cohortId,
                TransferId = transferId,
                IsValid = true
            };
        }

        public async Task<ComplianceRecalculationResultDto> RecalculateCompliance(
            Guid cohortId,
            string transferId,
            Guid targetDepartmentId)
        {
            // TODO: Implement compliance recalculation
            return new ComplianceRecalculationResultDto
            {
                CohortId = cohortId,
                TransferId = transferId,
                TargetDepartmentId = targetDepartmentId,
                Success = true
            };
        }

        #endregion Compliance State Preservation

        #region Lookup Tables

        public async Task<PagedResultDto<DepartmentLookupDto>> GetAllDepartmentsForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _tenantDepartmentRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    d => d.Name.Contains(input.Filter) || d.Description.Contains(input.Filter));

            var totalCount = await query.CountAsync();

            var departments = await query
                .OrderBy(input.Sorting ?? "name asc")
                .PageBy(input)
                .Select(d => new DepartmentLookupDto
                {
                    DepartmentId = d.Id,
                    DepartmentName = d.Name,
                    DepartmentDescription = d.Description,
                    IsActive = d.Active
                })
                .ToListAsync();

            return new PagedResultDto<DepartmentLookupDto>(totalCount, departments);
        }

        public async Task<PagedResultDto<CohortTenantDepartmentLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _cohortRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    c => c.Name.Contains(input.Filter));

            var totalCount = await query.CountAsync();

            var cohorts = await query
                .OrderBy(input.Sorting ?? "name asc")
                .PageBy(input)
                .Select(c => new CohortTenantDepartmentLookupTableDto
                {
                    Id = c.Id.ToString(),
                    DisplayName = c.Name
                })
                .ToListAsync();

            return new PagedResultDto<CohortTenantDepartmentLookupTableDto>(totalCount, cohorts);
        }

        #endregion Lookup Tables

        #region Helper Classes

        private class TransferValidationResult
        {
            public bool CanTransfer { get; set; }
            public List<string> Warnings { get; set; } = new List<string>();
        }

        #endregion Helper Classes
    }
}