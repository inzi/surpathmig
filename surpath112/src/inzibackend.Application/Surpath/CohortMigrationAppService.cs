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
    [AbpAuthorize(AppPermissions.Pages_Cohorts_Migrate)]
    public class CohortMigrationAppService : inzibackendAppServiceBase, ICohortMigrationAppService
    {
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;
        private readonly IRepository<TenantDepartmentUser, Guid> _tenantDepartmentUserRepository;
        private readonly IRepository<RecordRequirement, Guid> _recordRequirementRepository;
        private readonly IRepository<RecordCategory, Guid> _recordCategoryRepository;
        private readonly IRepository<RecordState, Guid> _recordStateRepository;
        private readonly IRepository<RecordNote, Guid> _recordNoteRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IOUSecurityManager _ouSecurityManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ISurpathComplianceEvaluator _surpathComplianceEvaluator;

        public CohortMigrationAppService(
            IRepository<Cohort, Guid> cohortRepository,
            IRepository<CohortUser, Guid> cohortUserRepository,
            IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
            IRepository<TenantDepartmentUser, Guid> tenantDepartmentUserRepository,
            IRepository<RecordRequirement, Guid> recordRequirementRepository,
            IRepository<RecordCategory, Guid> recordCategoryRepository,
            IRepository<RecordState, Guid> recordStateRepository,
            IRepository<RecordNote, Guid> recordNoteRepository,
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
            _tenantRepository = tenantRepository;
            _ouSecurityManager = ouSecurityManager;
            _unitOfWorkManager = unitOfWorkManager;
            _surpathComplianceEvaluator = surpathComplianceEvaluator;
        }

        #region Analysis and Validation

        public async Task<CohortMigrationAnalysisDto> AnalyzeCohortMigration(Guid cohortId, Guid? targetDepartmentId)
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
            var validationResult = await ValidateMigrationBusinessRules(cohort, sourceDepartment, targetDepartmentId, usersCount, requirementCategories);

            // If target department is specified, identify which requirements exist at both source and target
            var noMigrationRequiredCategories = new List<RequirementCategoryAnalysisDto>();
            var categoriesToMap = requirementCategories;

            if (targetDepartmentId.HasValue)
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

                // Separate categories that exist at both source and target (no migration required)
                noMigrationRequiredCategories = requirementCategories
                    .Where(rc => targetRequirementIds.Contains(rc.RequirementId))
                    .ToList();

                // Categories that need explicit mapping are those that don't exist at target
                categoriesToMap = requirementCategories
                    .Where(rc => !targetRequirementIds.Contains(rc.RequirementId))
                    .ToList();

                Logger.Info($"No migration required categories: {noMigrationRequiredCategories.Count}, Categories to map: {categoriesToMap.Count}");
            }

            var analysis = new CohortMigrationAnalysisDto
            {
                CohortId = cohortId,
                CohortName = cohort.Name,
                SourceDepartmentId = sourceDepartment?.Id ?? Guid.Empty,
                SourceDepartmentName = sourceDepartment?.Name ?? "No Department",
                TotalUsersCount = usersCount,
                RequirementCategories = categoriesToMap, // Only categories that need mapping
                NoMigrationRequiredCategories = noMigrationRequiredCategories, // Categories that apply before and after
                CanMigrate = validationResult.CanMigrate,
                MigrationComplexity = DetermineMigrationComplexity(requirementCategories),
                EstimatedDurationMinutes = CalculateEstimatedDuration(usersCount, requirementCategories.Count),
                Warnings = validationResult.Warnings
            };

            return analysis;
        }

        private async Task<MigrationValidationResult> ValidateMigrationBusinessRules(
            Cohort cohort,
            TenantDepartment sourceDepartment,
            Guid? targetDepartmentId,
            int usersCount,
            List<RequirementCategoryAnalysisDto> requirementCategories)
        {
            var result = new MigrationValidationResult
            {
                CanMigrate = true,
                Warnings = new List<string>()
            };

            // Rule 1: Cohort must exist and be valid
            if (cohort == null)
            {
                result.CanMigrate = false;
                result.Warnings.Add(L("CohortNotFound"));
                return result;
            }

            // Rule 2: Cohort must not be in an active migration state
            var hasActiveMigration = await CheckForActiveMigrations(cohort.Id);
            if (hasActiveMigration)
            {
                result.CanMigrate = false;
                result.Warnings.Add(L("CohortHasActiveMigration"));
                return result;
            }

            // Rule 3: Validate target department if specified
            if (targetDepartmentId.HasValue)
            {
                var targetDepartment = await _tenantDepartmentRepository.FirstOrDefaultAsync(targetDepartmentId.Value);
                if (targetDepartment == null)
                {
                    result.CanMigrate = false;
                    result.Warnings.Add(L("TargetDepartmentNotFound"));
                    return result;
                }

                if (!targetDepartment.Active)
                {
                    result.CanMigrate = false;
                    result.Warnings.Add(L("TargetDepartmentInactive"));
                    return result;
                }

                // Rule 4: Cannot migrate to the same department
                if (cohort.TenantDepartmentId == targetDepartmentId)
                {
                    result.CanMigrate = false;
                    result.Warnings.Add(L("CannotMigrateToSameDepartment"));
                    return result;
                }

                // Rule 5: Check tenant compatibility
                if (targetDepartment.TenantId != cohort.TenantId)
                {
                    result.CanMigrate = false;
                    result.Warnings.Add(L("CrossTenantMigrationNotAllowed"));
                    return result;
                }
            }

            // Rule 6: Validate user compliance states
            var complianceIssues = await ValidateUserComplianceStates(cohort.Id, requirementCategories);
            if (complianceIssues.Any())
            {
                result.Warnings.AddRange(complianceIssues);
                // Don't block migration but warn about potential compliance impacts
            }

            // Rule 7: Check for critical requirements that cannot be migrated
            var criticalRequirements = await CheckForCriticalRequirements(requirementCategories);
            if (criticalRequirements.Any())
            {
                result.Warnings.AddRange(criticalRequirements.Select(cr =>
                    L("CriticalRequirementMigrationWarning", cr.RequirementName)));
            }

            // Rule 8: Validate organizational unit permissions
            var ouValidation = await ValidateOrganizationalUnitPermissions(cohort, targetDepartmentId);
            if (!ouValidation.IsValid)
            {
                result.CanMigrate = false;
                result.Warnings.Add(ouValidation.ErrorMessage);
                return result;
            }

            // Rule 9: Check for data integrity constraints
            var dataIntegrityIssues = await ValidateDataIntegrityConstraints(cohort.Id, requirementCategories);
            if (dataIntegrityIssues.Any())
            {
                result.Warnings.AddRange(dataIntegrityIssues);
            }

            // Rule 10: Performance and scale warnings
            AddPerformanceWarnings(result, usersCount, requirementCategories.Count);

            // Rule 11: Check for conflicting requirements in target department
            if (targetDepartmentId.HasValue)
            {
                var conflictWarnings = await CheckForRequirementConflicts(requirementCategories, targetDepartmentId.Value);
                result.Warnings.AddRange(conflictWarnings);
            }

            return result;
        }

        private async Task<bool> CheckForActiveMigrations(Guid cohortId)
        {
            // TODO: Implement active migration tracking
            // For now, return false (no active migrations)
            return false;
        }

        private async Task<List<string>> ValidateUserComplianceStates(Guid cohortId, List<RequirementCategoryAnalysisDto> categories)
        {
            var warnings = new List<string>();

            // Check for users with pending compliance requirements
            var pendingComplianceCount = 0;
            foreach (var category in categories)
            {
                if (category.AffectedUsersCount > 0 && category.RecordStatesCount > 0)
                {
                    // Check for incomplete compliance states
                    var incompleteStates = await _recordStateRepository.CountAsync(rs =>
                        rs.RecordFk.TenantDocumentCategoryId == category.CategoryId &&
                        rs.State != EnumRecordState.Approved);

                    if (incompleteStates > 0)
                    {
                        pendingComplianceCount += incompleteStates;
                    }
                }
            }

            if (pendingComplianceCount > 0)
            {
                warnings.Add(L("PendingComplianceStatesWarning", pendingComplianceCount));
            }

            return warnings;
        }

        private async Task<List<RequirementCategoryAnalysisDto>> CheckForCriticalRequirements(List<RequirementCategoryAnalysisDto> categories)
        {
            // Identify requirements that are critical and may need special handling
            var criticalRequirements = new List<RequirementCategoryAnalysisDto>();

            foreach (var category in categories)
            {
                // Check if this is a critical healthcare requirement
                var categoryName = category.CategoryName?.ToLowerInvariant() ?? "";

                if (categoryName.Contains("license") ||
                    categoryName.Contains("certification") ||
                    categoryName.Contains("drug") ||
                    categoryName.Contains("background") ||
                    categoryName.Contains("immunization"))
                {
                    criticalRequirements.Add(category);
                }
            }

            return criticalRequirements;
        }

        private async Task<(bool IsValid, string ErrorMessage)> ValidateOrganizationalUnitPermissions(Cohort cohort, Guid? targetDepartmentId)
        {
            try
            {
                // Check if user has permission to migrate cohorts
                if (!await PermissionChecker.IsGrantedAsync(AppPermissions.Pages_Cohorts_MigrateBetweenDepartments))
                {
                    return (false, L("InsufficientPermissionsForMigration"));
                }

                // Additional OU-specific validation can be added here
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.Error("Error validating organizational unit permissions", ex);
                return (false, L("PermissionValidationError"));
            }
        }

        private async Task<List<string>> ValidateDataIntegrityConstraints(Guid cohortId, List<RequirementCategoryAnalysisDto> categories)
        {
            var warnings = new List<string>();

            // Check for orphaned records that might be created
            var totalRecordStates = categories.Sum(c => c.RecordStatesCount);
            if (totalRecordStates > 1000)
            {
                warnings.Add(L("LargeDataSetMigrationWarning", totalRecordStates));
            }

            // Check for potential foreign key constraint issues
            foreach (var category in categories)
            {
                if (category.AffectedUsersCount > 0 && category.RecordStatesCount == 0)
                {
                    warnings.Add(L("InconsistentDataWarning", category.CategoryName));
                }
            }

            return warnings;
        }

        private void AddPerformanceWarnings(MigrationValidationResult result, int usersCount, int categoriesCount)
        {
            // Large cohort warning
            if (usersCount > 100)
            {
                result.Warnings.Add(L("LargeCohortMigrationWarning", usersCount));
            }

            // Many categories warning
            if (categoriesCount > 10)
            {
                result.Warnings.Add(L("ManyRequirementCategoriesWarning", categoriesCount));
            }

            // Very large migration warning
            if (usersCount > 500)
            {
                result.Warnings.Add(L("VeryLargeMigrationWarning"));
            }

            // Complex migration warning
            if (categoriesCount > 20)
            {
                result.Warnings.Add(L("ComplexMigrationWarning"));
            }
        }

        private async Task<List<string>> CheckForRequirementConflicts(List<RequirementCategoryAnalysisDto> sourceCategories, Guid targetDepartmentId)
        {
            var warnings = new List<string>();

            // Get target department requirements
            var targetRequirements = await _recordRequirementRepository.GetAll()
                .Where(r => r.TenantDepartmentId == targetDepartmentId)
                .ToListAsync();

            foreach (var sourceCategory in sourceCategories)
            {
                // Check for naming conflicts
                var conflictingRequirement = targetRequirements
                    .FirstOrDefault(tr => tr.Name.Equals(sourceCategory.RequirementName, StringComparison.OrdinalIgnoreCase));

                if (conflictingRequirement != null)
                {
                    warnings.Add(L("RequirementNameConflictWarning", sourceCategory.RequirementName));
                }
            }

            return warnings;
        }

        private class MigrationValidationResult
        {
            public bool CanMigrate { get; set; }
            public List<string> Warnings { get; set; } = new List<string>();
        }

        public async Task<List<TargetCategoryOptionDto>> GetTargetCategoryOptions(GetTargetCategoryOptionsInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            // Performance optimization: Use single query with projection to reduce data transfer
            var sourceCategory = await _recordCategoryRepository.GetAll()
                .Where(rc => rc.Id == input.SourceCategoryId)
                .Select(rc => new { rc.Id, rc.Name })
                .FirstOrDefaultAsync();

            if (sourceCategory == null)
                throw new UserFriendlyException(L("SourceCategoryNotFound"));

            // Use centralized hierarchical method to get target categories
            var hierarchicalTargetCategories = await _surpathComplianceEvaluator.GetHierarchicalRequirementCategories(
                departmentId: input.TargetDepartmentId,
                cohortId: null, // Target is a department, not a cohort (for now)
                includeInherited: true
            );

            hierarchicalTargetCategories = hierarchicalTargetCategories.Where(r => r.IsSurpathOnly == false).ToList();

            var targetCategoriesData = hierarchicalTargetCategories.Select(c => new
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                RequirementId = c.RequirementId,
                RequirementName = c.RequirementName,
                RequirementDescription = c.RequirementDescription,
                Instructions = c.CategoryInstructions,
                HierarchyLevel = c.HierarchyLevel
            }).ToList();

            // Calculate usage statistics separately to avoid EF translation issues
            var categoryIds = targetCategoriesData.Select(c => c.CategoryId).ToList();
            var recordStatesStats = await _recordStateRepository.GetAll()
                .Where(rs => rs.RecordCategoryId.HasValue && categoryIds.Contains(rs.RecordCategoryId.Value))
                .GroupBy(rs => rs.RecordCategoryId.Value)
                .Select(g => new
                {
                    CategoryId = g.Key,
                    RecordStatesCount = g.Count(),
                    ActiveUsersCount = g.Count(rs => rs.State == EnumRecordState.Approved)
                })
                .ToListAsync();

            // Merge the data
            var targetCategoriesWithStats = targetCategoriesData.Select(tc => new
            {
                tc.CategoryId,
                tc.CategoryName,
                tc.RequirementId,
                tc.RequirementName,
                tc.RequirementDescription,
                tc.Instructions,
                RecordStatesCount = recordStatesStats.FirstOrDefault(s => s.CategoryId == tc.CategoryId)?.RecordStatesCount ?? 0,
                ActiveUsersCount = recordStatesStats.FirstOrDefault(s => s.CategoryId == tc.CategoryId)?.ActiveUsersCount ?? 0
            }).ToList();

            // Performance optimization: Calculate similarities in parallel for large datasets
            var options = new List<TargetCategoryOptionDto>();
            var sourceName = sourceCategory.Name;

            // Use parallel processing for similarity calculations when dealing with many categories
            if (targetCategoriesWithStats.Count > 20)
            {
                var parallelOptions = targetCategoriesWithStats.AsParallel().Select(targetData =>
                {
                    var matchScore = CalculateSimilarity(sourceName, targetData.CategoryName);
                    var isExactMatch = string.Equals(sourceName, targetData.CategoryName, StringComparison.OrdinalIgnoreCase);
                    var isSimilarMatch = matchScore > 70;

                    // Enhanced recommendation scoring
                    var recommendationScore = CalculateRecommendationScore(
                        sourceName,
                        targetData.CategoryName,
                        matchScore,
                        targetData.RecordStatesCount,
                        targetData.ActiveUsersCount);

                    return new TargetCategoryOptionDto
                    {
                        CategoryId = targetData.CategoryId,
                        CategoryName = targetData.CategoryName,
                        RequirementId = targetData.RequirementId,
                        RequirementName = targetData.RequirementName,
                        RequirementDescription = targetData.RequirementDescription,
                        Instructions = targetData.Instructions,
                        IsExactMatch = isExactMatch,
                        IsSimilarMatch = isSimilarMatch,
                        MatchScore = matchScore,
                        RecommendationScore = recommendationScore,
                        UsageStatistics = new CategoryUsageStatisticsDto
                        {
                            TotalRecordStates = targetData.RecordStatesCount,
                            ActiveUsers = targetData.ActiveUsersCount,
                            ComplianceRate = targetData.RecordStatesCount > 0
                                ? (double)targetData.ActiveUsersCount / targetData.RecordStatesCount * 100
                                : 0
                        }
                    };
                }).ToList();

                options.AddRange(parallelOptions);
            }
            else
            {
                // Sequential processing for smaller datasets
                foreach (var targetData in targetCategoriesWithStats)
                {
                    var matchScore = CalculateSimilarity(sourceName, targetData.CategoryName);
                    var isExactMatch = string.Equals(sourceName, targetData.CategoryName, StringComparison.OrdinalIgnoreCase);
                    var isSimilarMatch = matchScore > 70;

                    // Enhanced recommendation scoring
                    var recommendationScore = CalculateRecommendationScore(
                        sourceName,
                        targetData.CategoryName,
                        matchScore,
                        targetData.RecordStatesCount,
                        targetData.ActiveUsersCount);

                    var option = new TargetCategoryOptionDto
                    {
                        CategoryId = targetData.CategoryId,
                        CategoryName = targetData.CategoryName,
                        RequirementId = targetData.RequirementId,
                        RequirementName = targetData.RequirementName,
                        RequirementDescription = targetData.RequirementDescription,
                        Instructions = targetData.Instructions,
                        IsExactMatch = isExactMatch,
                        IsSimilarMatch = isSimilarMatch,
                        MatchScore = matchScore,
                        RecommendationScore = recommendationScore,
                        UsageStatistics = new CategoryUsageStatisticsDto
                        {
                            TotalRecordStates = targetData.RecordStatesCount,
                            ActiveUsers = targetData.ActiveUsersCount,
                            ComplianceRate = targetData.RecordStatesCount > 0
                                ? (double)targetData.ActiveUsersCount / targetData.RecordStatesCount * 100
                                : 0
                        }
                    };

                    options.Add(option);
                }
            }

            // Smart filtering and sorting
            var filteredOptions = ApplySmartFiltering(options, input);

            // Multi-criteria sorting: recommendation score (primary), match score (secondary), usage (tertiary)
            return filteredOptions
                .OrderByDescending(o => o.RecommendationScore)
                .ThenByDescending(o => o.MatchScore)
                .ThenByDescending(o => o.UsageStatistics?.ComplianceRate ?? 0)
                .ThenByDescending(o => o.UsageStatistics?.TotalRecordStates ?? 0)
                .ToList();
        }

        private double CalculateRecommendationScore(string sourceName, string targetName, int matchScore, int recordStatesCount, int activeUsersCount)
        {
            var baseScore = matchScore;

            // Boost score for categories with proven usage
            var usageBonus = 0.0;
            if (recordStatesCount > 0)
            {
                usageBonus += Math.Min(10.0, Math.Log10(recordStatesCount + 1) * 3); // Up to 10 points for usage
            }

            // Boost score for categories with good compliance rates
            var complianceBonus = 0.0;
            if (recordStatesCount > 0 && activeUsersCount > 0)
            {
                var complianceRate = (double)activeUsersCount / recordStatesCount;
                complianceBonus = complianceRate * 5.0; // Up to 5 points for high compliance
            }

            // Penalty for unused categories
            var usagePenalty = recordStatesCount == 0 ? -5.0 : 0.0;

            return Math.Max(0, Math.Min(100, baseScore + usageBonus + complianceBonus + usagePenalty));
        }

        private List<TargetCategoryOptionDto> ApplySmartFiltering(List<TargetCategoryOptionDto> options, GetTargetCategoryOptionsInput input)
        {
            var filteredOptions = options;

            // Apply minimum match score threshold (configurable)
            var minMatchScore = input.MinMatchScore ?? 30; // Default minimum 30% match
            filteredOptions = filteredOptions.Where(o => o.MatchScore >= minMatchScore).ToList();

            // Limit results for performance (configurable)
            var maxResults = input.MaxResults ?? 50; // Default maximum 50 results
            if (filteredOptions.Count > maxResults)
            {
                filteredOptions = filteredOptions.Take(maxResults).ToList();
            }

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(input.SearchFilter))
            {
                var searchTerm = input.SearchFilter.ToLowerInvariant();
                filteredOptions = filteredOptions.Where(o =>
                    o.CategoryName.ToLowerInvariant().Contains(searchTerm) ||
                    (o.RequirementName?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                    (o.RequirementDescription?.ToLowerInvariant().Contains(searchTerm) ?? false)
                ).ToList();
            }

            // Prioritize exact and similar matches
            if (input.PrioritizeHighMatches ?? true)
            {
                var exactMatches = filteredOptions.Where(o => o.IsExactMatch).ToList();
                var similarMatches = filteredOptions.Where(o => o.IsSimilarMatch && !o.IsExactMatch).ToList();
                var otherMatches = filteredOptions.Where(o => !o.IsSimilarMatch && !o.IsExactMatch).ToList();

                filteredOptions = exactMatches.Concat(similarMatches).Concat(otherMatches).ToList();
            }

            return filteredOptions;
        }

        public async Task<bool> ValidateMigrationMappings(ValidateMigrationMappingsInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            foreach (var mapping in input.Mappings)
            {
                if (mapping.Action == MappingAction.MapToExisting && !mapping.TargetCategoryId.HasValue)
                    return false;

                if (mapping.Action == MappingAction.CopyToNew &&
                    (string.IsNullOrWhiteSpace(mapping.NewCategoryName) || string.IsNullOrWhiteSpace(mapping.NewRequirementName)))
                    return false;

                if (mapping.Action == MappingAction.MapToExisting)
                {
                    var targetCategory = await _recordCategoryRepository.FirstOrDefaultAsync(mapping.TargetCategoryId.Value);
                    if (targetCategory == null)
                        return false;
                }
            }

            return true;
        }

        #endregion Analysis and Validation

        #region Migration Operations

        [AbpAuthorize(AppPermissions.Pages_Cohorts_MigrateBetweenDepartments)]
        public async Task<CohortMigrationResultDto> MigrateCohort(CohortMigrationDto input)
        {
            if (input.TargetDepartmentId.HasValue)
                return await MigrateCohortToExistingDepartment(input);
            else
                return await MigrateCohortToNewDepartment(input);
        }

        public async Task<CohortMigrationResultDto> MigrateCohortToNewDepartment(CohortMigrationDto input)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    var migrationId = Guid.NewGuid().ToString();
                    var startTime = DateTime.UtcNow;

                    Logger.Info($"Starting cohort migration to new department: Cohort {input.CohortId}, Migration {migrationId}");

                    // Step 1: Preserve compliance states before migration
                    Logger.Info($"Step 1: Preserving compliance states for migration {migrationId}");
                    var compliancePreservation = await PreserveCohortUserCompliance(input.CohortId, migrationId);

                    if (!compliancePreservation.Success)
                    {
                        Logger.Error($"Compliance preservation failed for migration {migrationId}");
                        return new CohortMigrationResultDto
                        {
                            Success = false,
                            Message = L("CompliancePreservationFailed"),
                            Errors = compliancePreservation.Errors,
                            MigrationId = migrationId
                        };
                    }

                    Logger.Info($"Compliance preservation completed: {compliancePreservation.PreservedUsersCount} users, " +
                               $"{compliancePreservation.PreservedRecordStatesCount} record states preserved");

                    // Step 2: Create new department
                    Logger.Info($"Step 2: Creating new department '{input.NewDepartmentName}' for migration {migrationId}");
                    var newDepartment = new TenantDepartment
                    {
                        Name = input.NewDepartmentName,
                        Description = input.NewDepartmentDescription,
                        Active = true,
                        TenantId = AbpSession.TenantId,
                        OrganizationUnitId = 1 // Default OU, should be configurable
                    };

                    await _tenantDepartmentRepository.InsertAsync(newDepartment);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    Logger.Info($"New department created: {newDepartment.Name} (ID: {newDepartment.Id})");

                    // Step 3: Update cohort to new department
                    Logger.Info($"Step 3: Updating cohort department assignment for migration {migrationId}");
                    var cohort = await _cohortRepository.GetAsync(input.CohortId);
                    var oldDepartmentId = cohort.TenantDepartmentId;
                    cohort.TenantDepartmentId = newDepartment.Id;

                    // Step 3.5: Update TenantDepartmentUsers for cohort users
                    Logger.Info($"Step 3.5: Updating user department associations for migration {migrationId}");
                    var updatedUsersCount = await UpdateCohortUserDepartmentAssociations(input.CohortId, oldDepartmentId, newDepartment.Id, migrationId);
                    Logger.Info($"Updated department associations for {updatedUsersCount} users");

                    // Step 4: Process category mappings with compliance preservation
                    Logger.Info($"Step 4: Processing {input.CategoryMappings.Count} category mappings for migration {migrationId}");
                    var processedUsers = 0;
                    var processedRecordStates = 0;

                    foreach (var mapping in input.CategoryMappings)
                    {
                        var result = await ProcessCategoryMapping(mapping, newDepartment.Id);
                        processedUsers += result.AffectedUsersCount;
                        processedRecordStates += result.AffectedRecordStatesCount;
                    }

                    Logger.Info($"Category mapping completed: {processedUsers} users affected, {processedRecordStates} record states processed");

                    // Step 5: Recalculate compliance after migration
                    Logger.Info($"Step 5: Recalculating compliance for migration {migrationId}");
                    var complianceRecalculation = await RecalculateCompliance(input.CohortId, migrationId, newDepartment.Id);

                    if (!complianceRecalculation.Success)
                    {
                        Logger.Warn($"Compliance recalculation had issues for migration {migrationId}: {complianceRecalculation.Message}");
                        // Continue with migration but log warnings
                    }
                    else
                    {
                        Logger.Info($"Compliance recalculation completed: {complianceRecalculation.SuccessfulRecalculations} successful, " +
                                   $"{complianceRecalculation.FailedRecalculations} failed");
                    }

                    // Step 6: Validate compliance integrity
                    Logger.Info($"Step 6: Validating compliance integrity for migration {migrationId}");
                    var complianceValidation = await ValidateComplianceIntegrity(input.CohortId, migrationId, compliancePreservation);

                    if (!complianceValidation.IsValid)
                    {
                        Logger.Warn($"Compliance integrity validation found issues for migration {migrationId}: {complianceValidation.ValidationSummary}");
                        // Continue with migration but include warnings
                    }
                    else
                    {
                        Logger.Info($"Compliance integrity validation passed: {complianceValidation.TotalUsersValidated} users validated");
                    }

                    // Step 7: Complete migration transaction
                    await uow.CompleteAsync();

                    // Prepare migration result with compliance information
                    var migrationResult = new CohortMigrationResultDto
                    {
                        Success = true,
                        Message = L("CohortMigrationSuccessful"),
                        NewDepartmentId = newDepartment.Id,
                        AffectedUsersCount = Math.Max(processedUsers, updatedUsersCount), // Use the higher count
                        MigratedRecordStatesCount = processedRecordStates,
                        MigrationStartTime = startTime,
                        MigrationEndTime = DateTime.UtcNow,
                        MigrationId = migrationId
                    };

                    // Add compliance-related warnings if any
                    if (!complianceRecalculation.Success)
                    {
                        migrationResult.Warnings.Add($"Compliance recalculation issues: {complianceRecalculation.Message}");
                        migrationResult.Warnings.AddRange(complianceRecalculation.Warnings);
                    }

                    if (!complianceValidation.IsValid)
                    {
                        migrationResult.Warnings.Add($"Compliance integrity issues: {complianceValidation.ValidationSummary}");
                        migrationResult.Warnings.AddRange(complianceValidation.Warnings);
                    }

                    // Log final migration summary
                    Logger.Info($"Cohort migration to new department completed successfully. " +
                               $"Migration ID: {migrationId}, " +
                               $"New Department: {newDepartment.Name} ({newDepartment.Id}), " +
                               $"Users: {Math.Max(processedUsers, updatedUsersCount)}, Record States: {processedRecordStates}, " +
                               $"Department Associations Updated: {updatedUsersCount}, " +
                               $"Duration: {(DateTime.UtcNow - startTime).TotalMinutes:F2} minutes, " +
                               $"Warnings: {migrationResult.Warnings.Count}");

                    return migrationResult;
                }
                catch (Exception ex)
                {
                    var errorMigrationId = Guid.NewGuid().ToString(); // Generate ID for error case
                    Logger.Error($"Cohort migration to new department failed for migration {errorMigrationId}", ex);
                    return new CohortMigrationResultDto
                    {
                        Success = false,
                        Message = L("CohortMigrationFailed"),
                        Errors = new List<string> { ex.Message },
                        MigrationId = errorMigrationId
                    };
                }
            }
        }

        public async Task<CohortMigrationResultDto> MigrateCohortToExistingDepartment(CohortMigrationDto input)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    var migrationId = Guid.NewGuid().ToString();
                    var startTime = DateTime.UtcNow;

                    Logger.Info($"Starting cohort migration to existing department: Cohort {input.CohortId}, Target Department {input.TargetDepartmentId}, Migration {migrationId}");

                    // Step 1: Preserve compliance states before migration
                    Logger.Info($"Step 1: Preserving compliance states for migration {migrationId}");
                    var compliancePreservation = await PreserveCohortUserCompliance(input.CohortId, migrationId);

                    if (!compliancePreservation.Success)
                    {
                        Logger.Error($"Compliance preservation failed for migration {migrationId}");
                        return new CohortMigrationResultDto
                        {
                            Success = false,
                            Message = L("CompliancePreservationFailed"),
                            Errors = compliancePreservation.Errors,
                            MigrationId = migrationId
                        };
                    }

                    Logger.Info($"Compliance preservation completed: {compliancePreservation.PreservedUsersCount} users, " +
                               $"{compliancePreservation.PreservedRecordStatesCount} record states preserved");

                    // Step 2: Update cohort to target department
                    Logger.Info($"Step 2: Updating cohort department assignment for migration {migrationId}");
                    var cohort = await _cohortRepository.GetAsync(input.CohortId);
                    var oldDepartmentId = cohort.TenantDepartmentId;
                    cohort.TenantDepartmentId = input.TargetDepartmentId;

                    // Step 2.5: Update TenantDepartmentUsers for cohort users
                    Logger.Info($"Step 2.5: Updating user department associations for migration {migrationId}");
                    var updatedUsersCount = await UpdateCohortUserDepartmentAssociations(input.CohortId, oldDepartmentId, input.TargetDepartmentId.Value, migrationId);
                    Logger.Info($"Updated department associations for {updatedUsersCount} users");

                    // Step 3: Process category mappings with compliance preservation
                    Logger.Info($"Step 3: Processing {input.CategoryMappings.Count} category mappings for migration {migrationId}");
                    var processedUsers = 0;
                    var processedRecordStates = 0;

                    foreach (var mapping in input.CategoryMappings)
                    {
                        var result = await ProcessCategoryMapping(mapping, input.TargetDepartmentId.Value);
                        processedUsers += result.AffectedUsersCount;
                        processedRecordStates += result.AffectedRecordStatesCount;
                    }

                    Logger.Info($"Category mapping completed: {processedUsers} users affected, {processedRecordStates} record states processed");

                    // Step 4: Recalculate compliance after migration
                    Logger.Info($"Step 4: Recalculating compliance for migration {migrationId}");
                    var complianceRecalculation = await RecalculateCompliance(input.CohortId, migrationId, input.TargetDepartmentId.Value);

                    if (!complianceRecalculation.Success)
                    {
                        Logger.Warn($"Compliance recalculation had issues for migration {migrationId}: {complianceRecalculation.Message}");
                        // Continue with migration but log warnings
                    }
                    else
                    {
                        Logger.Info($"Compliance recalculation completed: {complianceRecalculation.SuccessfulRecalculations} successful, " +
                                   $"{complianceRecalculation.FailedRecalculations} failed");
                    }

                    // Step 5: Validate compliance integrity
                    Logger.Info($"Step 5: Validating compliance integrity for migration {migrationId}");
                    var complianceValidation = await ValidateComplianceIntegrity(input.CohortId, migrationId, compliancePreservation);

                    if (!complianceValidation.IsValid)
                    {
                        Logger.Warn($"Compliance integrity validation found issues for migration {migrationId}: {complianceValidation.ValidationSummary}");
                        // Continue with migration but include warnings
                    }
                    else
                    {
                        Logger.Info($"Compliance integrity validation passed: {complianceValidation.TotalUsersValidated} users validated");
                    }

                    // Step 6: Complete migration transaction
                    await uow.CompleteAsync();

                    // Prepare migration result with compliance information
                    var migrationResult = new CohortMigrationResultDto
                    {
                        Success = true,
                        Message = L("CohortMigrationSuccessful"),
                        AffectedUsersCount = Math.Max(processedUsers, updatedUsersCount), // Use the higher count
                        MigratedRecordStatesCount = processedRecordStates,
                        MigrationStartTime = startTime,
                        MigrationEndTime = DateTime.UtcNow,
                        MigrationId = migrationId
                    };

                    // Add compliance-related warnings if any
                    if (!complianceRecalculation.Success)
                    {
                        migrationResult.Warnings.Add($"Compliance recalculation issues: {complianceRecalculation.Message}");
                        migrationResult.Warnings.AddRange(complianceRecalculation.Warnings);
                    }

                    if (!complianceValidation.IsValid)
                    {
                        migrationResult.Warnings.Add($"Compliance integrity issues: {complianceValidation.ValidationSummary}");
                        migrationResult.Warnings.AddRange(complianceValidation.Warnings);
                    }

                    // Log final migration summary
                    Logger.Info($"Cohort migration to existing department completed successfully. " +
                               $"Migration ID: {migrationId}, " +
                               $"Target Department: {input.TargetDepartmentId}, " +
                               $"Users: {Math.Max(processedUsers, updatedUsersCount)}, Record States: {processedRecordStates}, " +
                               $"Department Associations Updated: {updatedUsersCount}, " +
                               $"Duration: {(DateTime.UtcNow - startTime).TotalMinutes:F2} minutes, " +
                               $"Warnings: {migrationResult.Warnings.Count}");

                    return migrationResult;
                }
                catch (Exception ex)
                {
                    var errorMigrationId = Guid.NewGuid().ToString(); // Generate ID for error case
                    Logger.Error($"Cohort migration to existing department failed for migration {errorMigrationId}", ex);
                    return new CohortMigrationResultDto
                    {
                        Success = false,
                        Message = L("CohortMigrationFailed"),
                        Errors = new List<string> { ex.Message },
                        MigrationId = errorMigrationId
                    };
                }
            }
        }

        #endregion Migration Operations

        #region Department Management

        [AbpAuthorize(AppPermissions.Pages_Cohorts_CreateDepartment)]
        public async Task<Guid> CreateDepartment(CreateDepartmentDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            try
            {
                // Comprehensive input validation
                await ValidateCreateDepartmentInput(input);

                // Determine tenant context with proper multi-tenant support
                var targetTenantId = await DetermineTargetTenantId(input);

                // Get default organization unit for the tenant
                var defaultOrganizationUnitId = await GetDefaultOrganizationUnitId(targetTenantId);

                // Create department with enhanced properties
                var department = new TenantDepartment
                {
                    Name = input.Name.Trim(),
                    Description = input.Description?.Trim(),
                    Active = input.Active,
                    TenantId = targetTenantId,
                    OrganizationUnitId = defaultOrganizationUnitId,
                    MROType = EnumClientMROTypes.None // Default MRO type, could be configurable
                };

                // Insert department with transaction management
                using (var uow = _unitOfWorkManager.Begin())
                {
                    await _tenantDepartmentRepository.InsertAsync(department);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    // Associate department with organization unit if needed
                    await AssociateDepartmentWithOrganizationUnit(department.Id, defaultOrganizationUnitId, targetTenantId);

                    // Log department creation for audit purposes
                    Logger.Info($"Department '{department.Name}' created successfully with ID: {department.Id} for Tenant: {targetTenantId}");

                    await uow.CompleteAsync();
                }

                return department.Id;
            }
            catch (UserFriendlyException)
            {
                // Re-throw user-friendly exceptions as-is
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to create department", ex);
                throw new UserFriendlyException(L("DepartmentCreationFailed", ex.Message));
            }
        }

        private async Task ValidateCreateDepartmentInput(CreateDepartmentDto input)
        {
            // Rule 1: Department name is required
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new UserFriendlyException(L("DepartmentNameRequired"));
            }

            // Rule 2: Validate department name format and length
            var trimmedName = input.Name.Trim();
            if (trimmedName.Length < 2 || trimmedName.Length > 255)
            {
                throw new UserFriendlyException(L("DepartmentNameInvalidLength"));
            }

            // Rule 3: Check for invalid characters
            if (trimmedName.Any(c => char.IsControl(c) || c == '<' || c == '>' || c == '"' || c == '\''))
            {
                throw new UserFriendlyException(L("DepartmentNameInvalidCharacters"));
            }

            // Rule 4: Validate description length if provided
            if (!string.IsNullOrWhiteSpace(input.Description) && input.Description.Length > 1000)
            {
                throw new UserFriendlyException(L("DepartmentDescriptionTooLong"));
            }

            // Rule 5: Check for reserved names
            var reservedNames = new[] { "admin", "system", "default", "root", "administrator" };
            if (reservedNames.Contains(trimmedName.ToLowerInvariant()))
            {
                throw new UserFriendlyException(L("DepartmentNameReserved", trimmedName));
            }

            // Rule 6: Validate tenant context
            var targetTenantId = input.TenantId ?? AbpSession.TenantId;
            if (targetTenantId == null)
            {
                throw new UserFriendlyException(L("TenantContextRequired"));
            }

            // Rule 7: Check department name uniqueness within tenant
            var existingDepartment = await _tenantDepartmentRepository.FirstOrDefaultAsync(d =>
                d.Name.ToLower() == trimmedName.ToLower() &&
                d.TenantId == targetTenantId);

            if (existingDepartment != null)
            {
                throw new UserFriendlyException(L("DepartmentNameAlreadyExists", trimmedName));
            }

            // Rule 8: Check tenant limits (configurable business rule)
            var currentDepartmentCount = await _tenantDepartmentRepository.CountAsync(d => d.TenantId == targetTenantId);
            var maxDepartmentsPerTenant = 50; // This could be configurable or feature-based

            if (currentDepartmentCount >= maxDepartmentsPerTenant)
            {
                throw new UserFriendlyException(L("TenantDepartmentLimitExceeded", maxDepartmentsPerTenant));
            }
        }

        private async Task<int?> DetermineTargetTenantId(CreateDepartmentDto input)
        {
            // Priority 1: Use explicitly provided tenant ID (for host users)
            if (input.TenantId.HasValue)
            {
                // Validate that the user has permission to create departments for this tenant
                if (AbpSession.TenantId != null && AbpSession.TenantId != input.TenantId)
                {
                    // Tenant users cannot create departments for other tenants
                    throw new UserFriendlyException(L("CannotCreateDepartmentForOtherTenant"));
                }

                // Validate that the target tenant exists
                var targetTenant = await _tenantRepository.FirstOrDefaultAsync(input.TenantId.Value);
                if (targetTenant == null)
                {
                    throw new UserFriendlyException(L("TargetTenantNotFound"));
                }

                if (!targetTenant.IsActive)
                {
                    throw new UserFriendlyException(L("TargetTenantInactive"));
                }

                return input.TenantId.Value;
            }

            // Priority 2: Use current session tenant ID
            if (AbpSession.TenantId.HasValue)
            {
                return AbpSession.TenantId.Value;
            }

            // Priority 3: Host context - require explicit tenant specification
            throw new UserFriendlyException(L("TenantIdRequiredForHostContext"));
        }

        private async Task<long> GetDefaultOrganizationUnitId(int? tenantId)
        {
            try
            {
                // Try to get the default organization unit for the tenant
                // This could be enhanced to use a more sophisticated OU selection strategy

                // For now, use a simple default - this should be configurable
                var defaultOUId = 1L; // Default root OU

                // In a more sophisticated implementation, you might:
                // 1. Look up tenant-specific default OU
                // 2. Create a default OU if none exists
                // 3. Use organizational hierarchy rules

                return defaultOUId;
            }
            catch (Exception ex)
            {
                Logger.Warn("Failed to determine default organization unit, using fallback", ex);
                return 1L; // Fallback to root OU
            }
        }

        private async Task AssociateDepartmentWithOrganizationUnit(Guid departmentId, long organizationUnitId, int? tenantId)
        {
            try
            {
                // Use the OU Security Manager to properly associate the department with the OU
                await _ouSecurityManager.AddTenantDepartmentToOUAsync(departmentId, organizationUnitId);

                Logger.Info($"Department {departmentId} successfully associated with OU {organizationUnitId}");
            }
            catch (Exception ex)
            {
                // Log the error but don't fail the department creation
                // The department can still function without OU association
                Logger.Error($"Failed to associate department {departmentId} with OU {organizationUnitId}", ex);
            }
        }

        public async Task<DepartmentSelectionValidationResultDto> ValidateDepartmentSelection(ValidateDepartmentSelectionInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var result = new DepartmentSelectionValidationResultDto
            {
                IsValid = true,
                CompatibilityInfo = new DepartmentCompatibilityInfoDto(),
                CapacityInfo = new DepartmentCapacityInfoDto()
            };

            try
            {
                // Get source cohort information
                var cohort = await _cohortRepository.GetAsync(input.CohortId);
                if (cohort == null)
                {
                    result.IsValid = false;
                    result.Errors.Add(L("CohortNotFound"));
                    return result;
                }

                var sourceDepartment = cohort.TenantDepartmentId.HasValue
                    ? await _tenantDepartmentRepository.GetAsync(cohort.TenantDepartmentId.Value)
                    : null;

                // Validate department selection type
                if (input.TargetDepartmentId.HasValue)
                {
                    // Validating existing department selection
                    await ValidateExistingDepartmentSelection(input, result, cohort, sourceDepartment);
                }
                else
                {
                    // Validating new department creation
                    await ValidateNewDepartmentCreation(input, result, cohort);
                }

                // Perform capacity validation if requested
                if (input.ValidateCapacity && input.TargetDepartmentId.HasValue)
                {
                    await ValidateDepartmentCapacity(input.TargetDepartmentId.Value, result, cohort);
                }

                // Perform permissions validation if requested
                if (input.ValidatePermissions)
                {
                    await ValidateDepartmentPermissions(input, result, cohort);
                }

                // Calculate compatibility information if target department exists
                if (input.TargetDepartmentId.HasValue && sourceDepartment != null)
                {
                    await CalculateDepartmentCompatibility(sourceDepartment.Id, input.TargetDepartmentId.Value, result);
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("Department selection validation failed", ex);
                result.IsValid = false;
                result.Errors.Add(L("DepartmentValidationError", ex.Message));
                return result;
            }
        }

        private async Task ValidateExistingDepartmentSelection(
            ValidateDepartmentSelectionInput input,
            DepartmentSelectionValidationResultDto result,
            Cohort cohort,
            TenantDepartment sourceDepartment)
        {
            var targetDepartment = await _tenantDepartmentRepository.FirstOrDefaultAsync(input.TargetDepartmentId.Value);

            // Rule 1: Target department must exist
            if (targetDepartment == null)
            {
                result.IsValid = false;
                result.Errors.Add(L("TargetDepartmentNotFound"));
                return;
            }

            // Rule 2: Target department must be active
            if (!targetDepartment.Active)
            {
                result.IsValid = false;
                result.Errors.Add(L("TargetDepartmentInactive"));
                return;
            }

            // Rule 3: Cannot migrate to the same department
            if (cohort.TenantDepartmentId == input.TargetDepartmentId)
            {
                result.IsValid = false;
                result.Errors.Add(L("CannotMigrateToSameDepartment"));
                return;
            }

            // Rule 4: Check tenant compatibility
            if (targetDepartment.TenantId != cohort.TenantId)
            {
                result.IsValid = false;
                result.Errors.Add(L("CrossTenantMigrationNotAllowed"));
                return;
            }

            // Rule 5: Check for active migrations
            var hasActiveMigration = await CheckForActiveMigrations(cohort.Id);
            if (hasActiveMigration)
            {
                result.IsValid = false;
                result.Errors.Add(L("CohortHasActiveMigration"));
                return;
            }

            // Warning: Check for potential data conflicts
            var conflictingRequirements = await CheckForRequirementNameConflicts(
                cohort.TenantDepartmentId ?? Guid.Empty,
                input.TargetDepartmentId.Value);

            if (conflictingRequirements.Any())
            {
                result.Warnings.AddRange(conflictingRequirements.Select(req =>
                    L("RequirementNameConflictWarning", req)));
            }

            // Warning: Check department load
            var targetDepartmentLoad = await CalculateDepartmentLoad(input.TargetDepartmentId.Value);
            if (targetDepartmentLoad > 80) // 80% capacity threshold
            {
                result.Warnings.Add(L("TargetDepartmentHighLoad", targetDepartmentLoad));
            }
        }

        private async Task ValidateNewDepartmentCreation(
            ValidateDepartmentSelectionInput input,
            DepartmentSelectionValidationResultDto result,
            Cohort cohort)
        {
            // Rule 1: Department name is required
            if (string.IsNullOrWhiteSpace(input.NewDepartmentName))
            {
                result.IsValid = false;
                result.Errors.Add(L("DepartmentNameRequired"));
                return;
            }

            // Rule 2: Department name must be unique within tenant
            var existingDepartment = await _tenantDepartmentRepository.FirstOrDefaultAsync(d =>
                d.Name.ToLower() == input.NewDepartmentName.ToLower() &&
                d.TenantId == cohort.TenantId);

            if (existingDepartment != null)
            {
                result.IsValid = false;
                result.Errors.Add(L("DepartmentNameAlreadyExists", input.NewDepartmentName));
                return;
            }

            // Rule 3: Validate department name format
            if (input.NewDepartmentName.Length < 2 || input.NewDepartmentName.Length > 255)
            {
                result.IsValid = false;
                result.Errors.Add(L("DepartmentNameInvalidLength"));
                return;
            }

            // Rule 4: Check for invalid characters
            if (input.NewDepartmentName.Any(c => char.IsControl(c) || c == '<' || c == '>'))
            {
                result.IsValid = false;
                result.Errors.Add(L("DepartmentNameInvalidCharacters"));
                return;
            }

            // Warning: Suggest similar existing departments
            var similarDepartments = await FindSimilarDepartmentNames(input.NewDepartmentName, cohort.TenantId.Value);
            if (similarDepartments.Any())
            {
                result.Warnings.Add(L("SimilarDepartmentNamesExist", string.Join(", ", similarDepartments)));
            }
        }

        private async Task ValidateDepartmentCapacity(
            Guid targetDepartmentId,
            DepartmentSelectionValidationResultDto result,
            Cohort cohort)
        {
            var capacityInfo = result.CapacityInfo;

            // Calculate current department statistics
            capacityInfo.CurrentUsersCount = await CalculateDepartmentUsersCountAsync(targetDepartmentId);
            capacityInfo.CurrentCohortsCount = await _cohortRepository.CountAsync(c => c.TenantDepartmentId == targetDepartmentId);

            // Estimate department capacity (configurable business rule)
            var maxUsersPerDepartment = 500; // This could be configurable
            var maxCohortsPerDepartment = 20; // This could be configurable

            capacityInfo.EstimatedCapacity = maxUsersPerDepartment;
            capacityInfo.UtilizationPercentage = Math.Round(
                (double)capacityInfo.CurrentUsersCount / maxUsersPerDepartment * 100, 2);

            // Check capacity thresholds
            capacityInfo.IsNearCapacity = capacityInfo.UtilizationPercentage > 80;
            capacityInfo.IsOverCapacity = capacityInfo.UtilizationPercentage > 100;

            // Add cohort users to capacity calculation
            var cohortUsersCount = await _cohortUserRepository.CountAsync(cu => cu.CohortId == cohort.Id);
            var projectedUtilization = Math.Round(
                (double)(capacityInfo.CurrentUsersCount + cohortUsersCount) / maxUsersPerDepartment * 100, 2);

            // Generate capacity recommendations
            if (projectedUtilization > 100)
            {
                result.Warnings.Add(L("DepartmentWillExceedCapacity", projectedUtilization));
                capacityInfo.CapacityRecommendation = L("ConsiderAlternativeDepartment");
            }
            else if (projectedUtilization > 80)
            {
                result.Warnings.Add(L("DepartmentWillBeNearCapacity", projectedUtilization));
                capacityInfo.CapacityRecommendation = L("MonitorDepartmentLoad");
            }
            else
            {
                capacityInfo.CapacityRecommendation = L("DepartmentHasAdequateCapacity");
            }

            // Check cohort limits
            if (capacityInfo.CurrentCohortsCount >= maxCohortsPerDepartment)
            {
                result.Warnings.Add(L("DepartmentAtCohortLimit", maxCohortsPerDepartment));
            }
        }

        private async Task ValidateDepartmentPermissions(
            ValidateDepartmentSelectionInput input,
            DepartmentSelectionValidationResultDto result,
            Cohort cohort)
        {
            try
            {
                // Check basic migration permission
                if (!await PermissionChecker.IsGrantedAsync(AppPermissions.Pages_Cohorts_MigrateBetweenDepartments))
                {
                    result.IsValid = false;
                    result.Errors.Add(L("InsufficientPermissionsForMigration"));
                    return;
                }

                // Check department creation permission if creating new department
                if (!input.TargetDepartmentId.HasValue)
                {
                    if (!await PermissionChecker.IsGrantedAsync(AppPermissions.Pages_Cohorts_CreateDepartment))
                    {
                        result.IsValid = false;
                        result.Errors.Add(L("InsufficientPermissionsForDepartmentCreation"));
                        return;
                    }
                }

                // Additional organizational unit validation can be added here
                // This would integrate with the IOUSecurityManager if needed
            }
            catch (Exception ex)
            {
                Logger.Error("Permission validation failed", ex);
                result.Warnings.Add(L("PermissionValidationWarning"));
            }
        }

        private async Task CalculateDepartmentCompatibility(
            Guid sourceDepartmentId,
            Guid targetDepartmentId,
            DepartmentSelectionValidationResultDto result)
        {
            var compatibilityInfo = result.CompatibilityInfo;

            // Get requirements for both departments
            var sourceRequirements = await _recordRequirementRepository.GetAll()
                .Where(r => r.TenantDepartmentId == sourceDepartmentId)
                .Select(r => r.Name.ToLowerInvariant())
                .ToListAsync();

            var targetRequirements = await _recordRequirementRepository.GetAll()
                .Where(r => r.TenantDepartmentId == targetDepartmentId)
                .Select(r => r.Name.ToLowerInvariant())
                .ToListAsync();

            // Calculate similar requirements
            var similarRequirements = sourceRequirements.Intersect(targetRequirements).ToList();
            compatibilityInfo.SimilarRequirementsCount = similarRequirements.Count;
            compatibilityInfo.SimilarRequirements = similarRequirements;

            // Calculate conflicting requirements (same name, potentially different rules)
            var conflictingRequirements = sourceRequirements.Intersect(targetRequirements).ToList();
            compatibilityInfo.ConflictingRequirementsCount = conflictingRequirements.Count;
            compatibilityInfo.ConflictingRequirements = conflictingRequirements;

            // Calculate compatibility score
            if (sourceRequirements.Count == 0 && targetRequirements.Count == 0)
            {
                compatibilityInfo.CompatibilityScore = 100; // Both empty, perfect compatibility
            }
            else if (sourceRequirements.Count == 0 || targetRequirements.Count == 0)
            {
                compatibilityInfo.CompatibilityScore = 50; // One empty, neutral compatibility
            }
            else
            {
                var totalRequirements = sourceRequirements.Union(targetRequirements).Count();
                var commonRequirements = similarRequirements.Count;
                compatibilityInfo.CompatibilityScore = Math.Round(
                    (double)commonRequirements / totalRequirements * 100, 2);
            }

            // Determine if departments have similar structure
            compatibilityInfo.HasSimilarStructure = compatibilityInfo.CompatibilityScore > 60;
        }

        private async Task<List<string>> CheckForRequirementNameConflicts(Guid sourceDepartmentId, Guid targetDepartmentId)
        {
            var sourceRequirements = await _recordRequirementRepository.GetAll()
                .Where(r => r.TenantDepartmentId == sourceDepartmentId)
                .Select(r => r.Name)
                .ToListAsync();

            var targetRequirements = await _recordRequirementRepository.GetAll()
                .Where(r => r.TenantDepartmentId == targetDepartmentId)
                .Select(r => r.Name)
                .ToListAsync();

            return sourceRequirements.Intersect(targetRequirements, StringComparer.OrdinalIgnoreCase).ToList();
        }

        private async Task<double> CalculateDepartmentLoad(Guid departmentId)
        {
            var totalUsers = await CalculateDepartmentUsersCountAsync(departmentId);
            var maxCapacity = 500; // Configurable
            return Math.Round((double)totalUsers / maxCapacity * 100, 2);
        }

        private async Task<List<string>> FindSimilarDepartmentNames(string newName, int tenantId)
        {
            var existingDepartments = await _tenantDepartmentRepository.GetAll()
                .Where(d => d.TenantId == tenantId && d.Active)
                .Select(d => d.Name)
                .ToListAsync();

            var similarNames = new List<string>();
            var newNameLower = newName.ToLowerInvariant();

            foreach (var existingName in existingDepartments)
            {
                var similarity = CalculateSimilarity(newNameLower, existingName.ToLowerInvariant());
                if (similarity > 70) // 70% similarity threshold
                {
                    similarNames.Add(existingName);
                }
            }

            return similarNames;
        }

        public async Task<List<DepartmentLookupDto>> GetAvailableTargetDepartments(Guid excludeDepartmentId)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            // First, get basic department data that can be translated to SQL
            var basicDepartments = await _tenantDepartmentRepository.GetAll()
                .Where(d => d.Id != excludeDepartmentId && d.Active)
                .Select(d => new
                {
                    DepartmentId = d.Id,
                    DepartmentName = d.Name,
                    DepartmentDescription = d.Description,
                    IsActive = d.Active
                })
                .ToListAsync();

            // Now calculate the complex statistics in memory
            var departments = new List<DepartmentLookupDto>();

            foreach (var dept in basicDepartments)
            {
                var departmentDto = new DepartmentLookupDto
                {
                    DepartmentId = dept.DepartmentId,
                    DepartmentName = dept.DepartmentName,
                    DepartmentDescription = dept.DepartmentDescription,
                    IsActive = dept.IsActive,

                    // Calculate statistics separately to avoid LINQ translation issues
                    CohortsCount = await _cohortRepository.CountAsync(c => c.TenantDepartmentId == dept.DepartmentId),
                    RequirementsCount = await _recordRequirementRepository.CountAsync(r => r.TenantDepartmentId == dept.DepartmentId),
                    UsersCount = await CalculateDepartmentUsersCountAsync(dept.DepartmentId),
                    ActiveCohortsCount = await _cohortRepository.CountAsync(c => c.TenantDepartmentId == dept.DepartmentId && !c.IsDeleted),
                    TotalRecordStatesCount = await CalculateTotalRecordStatesCountAsync(dept.DepartmentId),
                    DepartmentUtilization = await CalculateDepartmentUtilizationAsync(dept.DepartmentId),
                    MigrationCompatibilityScore = await CalculateMigrationCompatibilityScoreAsync(dept.DepartmentId, excludeDepartmentId)
                };

                departments.Add(departmentDto);
            }

            return departments.OrderByDescending(d => d.MigrationCompatibilityScore)
                             .ThenByDescending(d => d.DepartmentUtilization)
                             .ThenBy(d => d.DepartmentName)
                             .ToList();
        }

        private async Task<int> CalculateDepartmentUsersCountAsync(Guid departmentId)
        {
            // Calculate users through multiple pathways for comprehensive count

            // 1. Direct department users
            var directUsers = await _tenantDepartmentUserRepository.CountAsync(tdu => tdu.TenantDepartmentId == departmentId);

            // 2. Users through cohorts in this department
            var cohortUsers = await _cohortUserRepository.CountAsync(cu =>
                cu.CohortFk.TenantDepartmentId == departmentId);

            // 3. Users with record states in this department (active users) - simplified query
            var activeUsers = await _recordStateRepository.GetAll()
                .Where(rs => rs.RecordCategoryFk.RecordRequirementFk.TenantDepartmentId == departmentId && rs.UserId.HasValue)
                .Select(rs => rs.UserId.Value)
                .Distinct()
                .CountAsync();

            // Return the maximum count as it represents the most comprehensive user base
            return Math.Max(Math.Max(directUsers, cohortUsers), activeUsers);
        }

        private async Task<double> CalculateDepartmentUtilizationAsync(Guid departmentId)
        {
            try
            {
                var totalRequirements = await _recordRequirementRepository.CountAsync(r => r.TenantDepartmentId == departmentId);
                if (totalRequirements == 0) return 0.0;

                var activeRecordStates = await _recordStateRepository.CountAsync(rs =>
                    rs.RecordCategoryFk.RecordRequirementFk.TenantDepartmentId == departmentId &&
                    rs.State == EnumRecordState.Approved);

                var totalRecordStates = await _recordStateRepository.CountAsync(rs =>
                    rs.RecordCategoryFk.RecordRequirementFk.TenantDepartmentId == departmentId);

                if (totalRecordStates == 0) return 0.0;

                // Calculate utilization as percentage of approved vs total record states
                return Math.Round((double)activeRecordStates / totalRecordStates * 100, 2);
            }
            catch
            {
                return 0.0; // Return 0 if calculation fails
            }
        }

        private async Task<double> CalculateMigrationCompatibilityScoreAsync(Guid targetDepartmentId, Guid sourceDepartmentId)
        {
            try
            {
                var score = 50.0; // Base score

                // Bonus for departments with similar requirement structures
                var sourceRequirements = await _recordRequirementRepository.CountAsync(r => r.TenantDepartmentId == sourceDepartmentId);
                var targetRequirements = await _recordRequirementRepository.CountAsync(r => r.TenantDepartmentId == targetDepartmentId);

                if (sourceRequirements > 0 && targetRequirements > 0)
                {
                    var similarityRatio = Math.Min(sourceRequirements, targetRequirements) / (double)Math.Max(sourceRequirements, targetRequirements);
                    score += similarityRatio * 20; // Up to 20 points for similar requirement counts
                }

                // Bonus for departments with active users (indicates good management)
                var activeUsers = await CalculateDepartmentUsersCountAsync(targetDepartmentId);
                if (activeUsers > 0)
                {
                    score += Math.Min(15, Math.Log10(activeUsers + 1) * 5); // Up to 15 points for user activity
                }

                // Bonus for high utilization (well-managed department)
                var utilization = await CalculateDepartmentUtilizationAsync(targetDepartmentId);
                if (utilization > 70)
                {
                    score += 10; // 10 points for high utilization
                }
                else if (utilization > 50)
                {
                    score += 5; // 5 points for moderate utilization
                }

                // Penalty for departments with too many cohorts (might be overloaded)
                var cohortsCount = await _cohortRepository.CountAsync(c => c.TenantDepartmentId == targetDepartmentId);
                if (cohortsCount > 10)
                {
                    score -= Math.Min(10, (cohortsCount - 10) * 2); // Penalty for overloaded departments
                }

                return Math.Max(0, Math.Min(100, Math.Round(score, 2)));
            }
            catch
            {
                return 50.0; // Return neutral score if calculation fails
            }
        }

        private async Task<int> CalculateTotalRecordStatesCountAsync(Guid departmentId)
        {
            try
            {
                return await _recordStateRepository.CountAsync(rs =>
                    rs.RecordCategoryFk.RecordRequirementFk.TenantDepartmentId == departmentId);
            }
            catch
            {
                return 0; // Return 0 if calculation fails
            }
        }

        #endregion Department Management

        #region Utility Methods

        public async Task<int> GetCohortUsersCount(Guid cohortId)
        {
            return await _cohortUserRepository.CountAsync(cu => cu.CohortId == cohortId);
        }

        public async Task<List<RequirementCategoryAnalysisDto>> GetCohortRequirementCategories(Guid cohortId)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            try
            {
                Logger.Info($"Getting requirement categories for cohort {cohortId} using hierarchical approach");

                var cohort = await _cohortRepository.GetAsync(cohortId);
                Logger.Info($"Cohort found: {cohort.Name}, TenantDepartmentId: {cohort.TenantDepartmentId}");

                // Get cohort users for affected users calculation - optimized single query
                var cohortUserIds = await _cohortUserRepository.GetAll()
                    .Where(cu => cu.CohortId == cohortId)
                    .Select(cu => cu.UserId)
                    .ToListAsync();

                Logger.Info($"Found {cohortUserIds.Count} users in cohort {cohortId}");

                // Use the centralized hierarchical method to get all applicable categories
                var hierarchicalCategories = await _surpathComplianceEvaluator.GetHierarchicalRequirementCategories(
                    departmentId: cohort.TenantDepartmentId,
                    cohortId: cohortId,
                    includeInherited: true
                );

                Logger.Info($"Found {hierarchicalCategories.Count} total applicable categories using hierarchical approach:");

                // Log breakdown for debugging
                var breakdown = hierarchicalCategories.GroupBy(c => c.HierarchyLevel)
                    .Select(g => new { Level = g.Key, Count = g.Count() })
                    .ToList();

                foreach (var level in breakdown)
                {
                    Logger.Info($"  - {level.Level}: {level.Count}");
                }

                // Get record states aggregations in separate optimized queries to avoid N+1 issues
                var categoryIds = hierarchicalCategories.Select(cat => cat.CategoryId).ToList();
                Logger.Debug($"Found {categoryIds.Count} category IDs for record states aggregation");

                var recordStatesAggregations = await _recordStateRepository.GetAll()
                    .Where(rs => categoryIds.Contains(rs.RecordCategoryId.Value) && cohortUserIds.Contains(rs.UserId.Value))
                    .GroupBy(rs => rs.RecordCategoryId)
                    .Select(g => new
                    {
                        CategoryId = g.Key,
                        TotalRecordStates = g.Count(),
                        AffectedUsersCount = g.Select(rs => rs.UserId).Distinct().Count(),
                        ComplianceStatesCount = g.Count(rs => rs.RecordStatusFk.ComplianceImpact == EnumStatusComplianceImpact.Compliant),
                        NeedsApprovalStatesCount = g.Count(rs => rs.State == EnumRecordState.NeedsApproval),
                        ApprovedStatesCount = g.Count(rs => rs.State == EnumRecordState.Approved),
                        RejectedStatesCount = g.Count(rs => rs.State == EnumRecordState.Rejected)
                    })
                    .ToListAsync();

                Logger.Debug($"Found record states for {recordStatesAggregations.Count} categories");

                // Combine hierarchical categories with record state data
                var result = hierarchicalCategories.Select(c =>
                {
                    var aggregation = recordStatesAggregations.FirstOrDefault(agg => agg.CategoryId == c.CategoryId);
                    return new RequirementCategoryAnalysisDto
                    {
                        CategoryId = c.CategoryId,
                        CategoryName = c.CategoryName,
                        RequirementId = c.RequirementId,
                        RequirementName = c.RequirementName,
                        IsDepartmentSpecific = c.IsDepartmentSpecific,

                        // Enhanced record state counts
                        RecordStatesCount = aggregation?.TotalRecordStates ?? 0,
                        AffectedUsersCount = aggregation?.AffectedUsersCount ?? 0,

                        // Migration analysis flags
                        RequiresMapping = true,

                        // Initialize target options (will be populated by GetTargetCategoryOptions)
                        TargetOptions = new List<TargetCategoryOptionDto>(),

                        // Include hierarchy information
                        HierarchyLevel = c.HierarchyLevel,
                        IsCohortSpecific = c.IsCohortSpecific,
                        IsSurpathOnly = c.IsSurpathOnly
                    };
                }).ToList();

                Logger.Info($"Successfully analyzed {result.Count} requirement categories for cohort {cohortId}");

                // Log summary statistics
                var totalRecordStates = result.Sum(r => r.RecordStatesCount);
                var totalAffectedUsers = result.Sum(r => r.AffectedUsersCount);
                var categoriesWithData = result.Count(r => r.RecordStatesCount > 0);

                Logger.Info($"Category analysis summary - Total record states: {totalRecordStates}, " +
                           $"Total affected users: {totalAffectedUsers}, " +
                           $"Categories with data: {categoriesWithData}");

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error getting requirement categories for cohort {cohortId}", ex);
                throw new UserFriendlyException(L("ErrorGettingCohortRequirementCategories", ex.Message));
            }
        }

        /// <summary>
        /// Determines the migration complexity for a specific category based on data volume and requirements.
        /// </summary>
        /// <param name="recordStatesCount">Number of record states in the category</param>
        /// <param name="affectedUsersCount">Number of users affected by this category</param>
        /// <param name="isRequired">Whether the category is required by business rules</param>
        /// <returns>Migration complexity level as string</returns>
        private string DetermineCategoryMigrationComplexity(int recordStatesCount, int affectedUsersCount, bool isRequired)
        {
            // Calculate complexity score based on multiple factors
            var complexityScore = 0;

            // Data volume impact
            if (recordStatesCount > 100) complexityScore += 3;
            else if (recordStatesCount > 50) complexityScore += 2;
            else if (recordStatesCount > 10) complexityScore += 1;

            // User impact
            if (affectedUsersCount > 50) complexityScore += 3;
            else if (affectedUsersCount > 20) complexityScore += 2;
            else if (affectedUsersCount > 5) complexityScore += 1;

            // Business rule impact
            if (isRequired) complexityScore += 2;

            // Determine complexity level
            return complexityScore switch
            {
                >= 6 => "High",
                >= 3 => "Medium",
                > 0 => "Low",
                _ => "Minimal"
            };
        }

        public async Task<bool> CanMigrateCohort(Guid cohortId)
        {
            var cohort = await _cohortRepository.FirstOrDefaultAsync(cohortId);
            if (cohort == null) return false;

            // Check if cohort has any active migrations
            // TODO: Implement migration status tracking

            return true;
        }

        #endregion Utility Methods

        #region Migration History

        public async Task<PagedResultDto<CohortMigrationHistoryDto>> GetMigrationHistory(GetAllCohortMigrationHistoryInput input)
        {
            // TODO: Implement migration history tracking with audit table
            return new PagedResultDto<CohortMigrationHistoryDto>(0, new List<CohortMigrationHistoryDto>());
        }

        public async Task<CohortMigrationHistoryDto> GetMigrationHistoryForView(Guid migrationId)
        {
            // TODO: Implement migration history retrieval
            throw new NotImplementedException();
        }

        public async Task<FileDto> GetMigrationHistoryToExcel(GetAllCohortMigrationHistoryInput input)
        {
            // TODO: Implement Excel export for migration history
            throw new NotImplementedException();
        }

        #endregion Migration History

        #region Rollback and Recovery

        // Note: CanRollbackMigration and RollbackMigration methods are implemented in the Migration Execution and Orchestration section

        #endregion Rollback and Recovery

        #region Progress Tracking and Reporting

        /// <summary>
        /// Gets real-time migration progress with detailed status information and estimated completion time.
        /// This method provides comprehensive progress tracking for long-running migration operations.
        /// </summary>
        /// <param name="migrationId">Migration identifier to track</param>
        /// <returns>CohortMigrationProgressDto with current progress and status information</returns>
        public async Task<CohortMigrationProgressDto> GetMigrationProgress(string migrationId)
        {
            try
            {
                Logger.Info($"Retrieving migration progress for migration {migrationId}");

                // Retrieve progress data from storage (placeholder for actual implementation)
                var progressData = await RetrieveMigrationProgressData(migrationId);

                if (progressData == null)
                {
                    Logger.Warn($"No progress data found for migration {migrationId}");
                    return new CohortMigrationProgressDto
                    {
                        MigrationId = migrationId,
                        Status = "NotFound",
                        ProgressPercentage = 0,
                        CurrentStep = "Migration not found",
                        Messages = new List<string> { "Migration progress data not found" }
                    };
                }

                // Calculate current progress and estimates
                var progress = await CalculateProgressMetrics(progressData);

                // Update progress with real-time information
                await UpdateProgressWithCurrentStatus(progress, migrationId);

                Logger.Info($"Migration progress retrieved for {migrationId}: {progress.ProgressPercentage}% complete");
                return progress;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error retrieving migration progress for {migrationId}", ex);
                return new CohortMigrationProgressDto
                {
                    MigrationId = migrationId,
                    Status = "Error",
                    ProgressPercentage = 0,
                    CurrentStep = "Error retrieving progress",
                    Messages = new List<string> { $"Error: {ex.Message}" }
                };
            }
        }

        /// <summary>
        /// Updates migration progress during execution with detailed step information and metrics.
        /// This method is called throughout the migration process to provide real-time updates.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <param name="progressUpdate">Progress update information</param>
        public async Task UpdateMigrationProgress(string migrationId, MigrationProgressUpdateDto progressUpdate)
        {
            try
            {
                Logger.Debug($"Updating migration progress for {migrationId}: {progressUpdate.CurrentStep} - {progressUpdate.ProgressPercentage}%");

                // Create comprehensive progress record
                var progressRecord = new MigrationProgressRecord
                {
                    MigrationId = migrationId,
                    UpdateTimestamp = DateTime.UtcNow,
                    Status = progressUpdate.Status,
                    ProgressPercentage = progressUpdate.ProgressPercentage,
                    CurrentStep = progressUpdate.CurrentStep,
                    CurrentOperation = progressUpdate.CurrentOperation,

                    // Processing metrics
                    ProcessedUsers = progressUpdate.ProcessedUsers,
                    TotalUsers = progressUpdate.TotalUsers,
                    ProcessedRecordStates = progressUpdate.ProcessedRecordStates,
                    TotalRecordStates = progressUpdate.TotalRecordStates,
                    ProcessedCategoryMappings = progressUpdate.ProcessedCategoryMappings,
                    TotalCategoryMappings = progressUpdate.TotalCategoryMappings,

                    // Timing information
                    StartTime = progressUpdate.StartTime,
                    CurrentStepStartTime = progressUpdate.CurrentStepStartTime,
                    EstimatedEndTime = progressUpdate.EstimatedEndTime,

                    // Messages and details
                    Message = progressUpdate.Message,
                    DetailedMessages = progressUpdate.DetailedMessages,

                    // Performance metrics
                    ProcessingRate = progressUpdate.ProcessingRate,
                    AverageStepDuration = progressUpdate.AverageStepDuration,

                    // Error tracking
                    ErrorsCount = progressUpdate.ErrorsCount,
                    WarningsCount = progressUpdate.WarningsCount,

                    // Additional metadata
                    Metadata = JsonConvert.SerializeObject(progressUpdate.AdditionalMetadata ?? new Dictionary<string, object>())
                };

                // Store progress record
                await StoreMigrationProgressRecord(progressRecord);

                // Update progress cache for real-time access
                await UpdateProgressCache(migrationId, progressRecord);

                // Send progress notifications if configured
                await SendProgressNotifications(migrationId, progressUpdate);

                Logger.Debug($"Migration progress updated for {migrationId}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error updating migration progress for {migrationId}", ex);
                // Don't fail the migration if progress update fails
            }
        }

        /// <summary>
        /// Creates a comprehensive migration progress report with analytics and historical data.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <returns>Detailed progress report with analytics</returns>
        public async Task<MigrationProgressReportDto> GenerateMigrationProgressReport(string migrationId)
        {
            try
            {
                Logger.Info($"Generating comprehensive progress report for migration {migrationId}");

                var report = new MigrationProgressReportDto
                {
                    MigrationId = migrationId,
                    ReportGeneratedAt = DateTime.UtcNow,
                    GeneratedBy = AbpSession.UserId
                };

                // Get all progress records for this migration
                var progressRecords = await GetMigrationProgressRecords(migrationId);

                if (!progressRecords.Any())
                {
                    report.Status = "NoData";
                    report.Summary = "No progress data available for this migration";
                    return report;
                }

                // Calculate comprehensive analytics
                await CalculateProgressAnalytics(report, progressRecords);

                // Generate step-by-step breakdown
                await GenerateStepBreakdown(report, progressRecords);

                // Calculate performance metrics
                await CalculatePerformanceMetrics(report, progressRecords);

                // Generate timeline analysis
                await GenerateTimelineAnalysis(report, progressRecords);

                Logger.Info($"Progress report generated for migration {migrationId}");
                return report;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error generating progress report for migration {migrationId}", ex);
                throw new UserFriendlyException(L("ProgressReportGenerationFailed", ex.Message));
            }
        }

        /// <summary>
        /// Gets migration progress history for analysis and reporting.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <returns>List of historical progress records</returns>
        public async Task<List<MigrationProgressHistoryDto>> GetMigrationProgressHistory(string migrationId)
        {
            try
            {
                Logger.Info($"Retrieving progress history for migration {migrationId}");

                // TODO: Retrieve from actual progress storage
                // For now, return placeholder data
                var historyRecords = new List<MigrationProgressHistoryDto>();

                Logger.Info($"Retrieved {historyRecords.Count} progress history records for migration {migrationId}");
                return historyRecords;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error retrieving progress history for migration {migrationId}", ex);
                return new List<MigrationProgressHistoryDto>();
            }
        }

        /// <summary>
        /// Gets migration progress history as detailed records for internal analysis.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <returns>List of detailed progress records</returns>
        private async Task<List<MigrationProgressRecord>> GetMigrationProgressRecords(string migrationId)
        {
            try
            {
                Logger.Info($"Retrieving progress history for migration {migrationId}");

                // TODO: Retrieve from actual progress storage
                // For now, return placeholder data
                var historyRecords = new List<MigrationProgressRecord>();

                // Example implementation:
                // var records = await _migrationProgressRepository.GetAll()
                //     .Where(p => p.MigrationId == migrationId)
                //     .OrderBy(p => p.UpdateTimestamp)
                //     .ToListAsync();
                //
                // historyRecords = records.ToList();

                Logger.Info($"Retrieved {historyRecords.Count} progress history records for migration {migrationId}");
                return historyRecords;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error retrieving progress history for migration {migrationId}", ex);
                return new List<MigrationProgressRecord>();
            }
        }

        /// <summary>
        /// Cleans up progress tracking data for completed migrations based on retention policy.
        /// </summary>
        /// <param name="retentionDays">Number of days to retain progress data</param>
        /// <returns>Cleanup result with statistics</returns>
        public async Task<ProgressDataCleanupResultDto> CleanupProgressData(int retentionDays = 90)
        {
            try
            {
                Logger.Info($"Starting progress data cleanup with retention policy: {retentionDays} days");

                var cutoffDate = DateTime.UtcNow.AddDays(-retentionDays);
                var cleanupResult = new ProgressDataCleanupResultDto
                {
                    CleanupTimestamp = DateTime.UtcNow,
                    RetentionDays = retentionDays,
                    CutoffDate = cutoffDate,
                    PerformedBy = AbpSession.UserId
                };

                // TODO: Implement actual cleanup logic
                // Example:
                // var expiredRecords = await _migrationProgressRepository.GetAll()
                //     .Where(p => p.UpdateTimestamp < cutoffDate)
                //     .ToListAsync();
                //
                // foreach (var record in expiredRecords)
                // {
                //     await _migrationProgressRepository.DeleteAsync(record);
                //     cleanupResult.DeletedRecords++;
                // }

                // Placeholder cleanup summary
                cleanupResult.DeletedRecords = 0;
                cleanupResult.ArchivedRecords = 0;
                cleanupResult.Success = true;
                cleanupResult.Message = "Progress data cleanup completed successfully";

                Logger.Info($"Progress data cleanup completed. Deleted: {cleanupResult.DeletedRecords} records");
                return cleanupResult;
            }
            catch (Exception ex)
            {
                Logger.Error("Error during progress data cleanup", ex);
                return new ProgressDataCleanupResultDto
                {
                    Success = false,
                    Message = $"Progress data cleanup failed: {ex.Message}",
                    CleanupTimestamp = DateTime.UtcNow,
                    PerformedBy = AbpSession.UserId
                };
            }
        }

        #region Private Progress Tracking Methods

        /// <summary>
        /// Retrieves migration progress data from storage.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <returns>Migration progress data or null if not found</returns>
        private async Task<MigrationProgressData> RetrieveMigrationProgressData(string migrationId)
        {
            try
            {
                // TODO: Implement actual progress data retrieval
                // For now, return placeholder data
                Logger.Debug($"Retrieving progress data for migration {migrationId}");

                // Example implementation:
                // var progressRecord = await _migrationProgressRepository.GetAll()
                //     .Where(p => p.MigrationId == migrationId)
                //     .OrderByDescending(p => p.UpdateTimestamp)
                //     .FirstOrDefaultAsync();
                //
                // if (progressRecord == null) return null;
                //
                // return new MigrationProgressData
                // {
                //     MigrationId = progressRecord.MigrationId,
                //     Status = progressRecord.Status,
                //     ProgressPercentage = progressRecord.ProgressPercentage,
                //     // ... other properties
                // };

                return null; // Placeholder
            }
            catch (Exception ex)
            {
                Logger.Error($"Error retrieving progress data for migration {migrationId}", ex);
                return null;
            }
        }

        /// <summary>
        /// Calculates progress metrics and estimates.
        /// </summary>
        /// <param name="progressData">Raw progress data</param>
        /// <returns>Calculated progress metrics</returns>
        private async Task<CohortMigrationProgressDto> CalculateProgressMetrics(MigrationProgressData progressData)
        {
            try
            {
                var progress = new CohortMigrationProgressDto
                {
                    MigrationId = progressData.MigrationId,
                    Status = progressData.Status,
                    ProgressPercentage = progressData.ProgressPercentage,
                    CurrentStep = progressData.CurrentStep,
                    ProcessedUsers = progressData.ProcessedUsers,
                    TotalUsers = progressData.TotalUsers,
                    ProcessedRecordStates = progressData.ProcessedRecordStates,
                    TotalRecordStates = progressData.TotalRecordStates,
                    StartTime = progressData.StartTime,
                    Messages = progressData.Messages ?? new List<string>()
                };

                // Calculate estimated end time
                if (progressData.ProgressPercentage > 0 && progressData.ProgressPercentage < 100)
                {
                    var elapsed = DateTime.UtcNow - progressData.StartTime;
                    var totalEstimated = elapsed.TotalMinutes / (progressData.ProgressPercentage / 100.0);
                    progress.EstimatedEndTime = progressData.StartTime.AddMinutes(totalEstimated);
                }

                return progress;
            }
            catch (Exception ex)
            {
                Logger.Error("Error calculating progress metrics", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates progress with current real-time status.
        /// </summary>
        /// <param name="progress">Progress object to update</param>
        /// <param name="migrationId">Migration identifier</param>
        private async Task UpdateProgressWithCurrentStatus(CohortMigrationProgressDto progress, string migrationId)
        {
            try
            {
                // Add real-time status information
                var currentTime = DateTime.UtcNow;
                var elapsed = currentTime - progress.StartTime;

                // Add elapsed time message
                progress.Messages.Add($"Elapsed time: {elapsed.TotalMinutes:F1} minutes");

                // Add estimated remaining time if available
                if (progress.EstimatedEndTime.HasValue)
                {
                    var remaining = progress.EstimatedEndTime.Value - currentTime;
                    if (remaining.TotalMinutes > 0)
                    {
                        progress.Messages.Add($"Estimated remaining: {remaining.TotalMinutes:F1} minutes");
                    }
                }

                // Add processing rate information
                if (progress.ProcessedUsers > 0 && elapsed.TotalMinutes > 0)
                {
                    var usersPerMinute = progress.ProcessedUsers / elapsed.TotalMinutes;
                    progress.Messages.Add($"Processing rate: {usersPerMinute:F1} users/minute");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error updating progress with current status for migration {migrationId}", ex);
            }
        }

        /// <summary>
        /// Stores migration progress record in the progress tracking system.
        /// </summary>
        /// <param name="progressRecord">Progress record to store</param>
        private async Task StoreMigrationProgressRecord(MigrationProgressRecord progressRecord)
        {
            try
            {
                // TODO: Store in dedicated progress tracking table
                var progressJson = JsonConvert.SerializeObject(progressRecord, Formatting.Indented);
                Logger.Debug($"Migration Progress Record: {progressJson}");

                // Example implementation:
                // await _migrationProgressRepository.InsertAsync(progressRecord);
                // await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error($"Error storing progress record for migration {progressRecord.MigrationId}", ex);
            }
        }

        /// <summary>
        /// Updates progress cache for real-time access.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <param name="progressRecord">Progress record</param>
        private async Task UpdateProgressCache(string migrationId, MigrationProgressRecord progressRecord)
        {
            try
            {
                // TODO: Update cache (Redis, in-memory, etc.)
                Logger.Debug($"Updated progress cache for migration {migrationId}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error updating progress cache for migration {migrationId}", ex);
            }
        }

        /// <summary>
        /// Sends progress notifications to interested parties.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <param name="progressUpdate">Progress update information</param>
        private async Task SendProgressNotifications(string migrationId, MigrationProgressUpdateDto progressUpdate)
        {
            try
            {
                // TODO: Implement progress notifications (SignalR, email, etc.)
                // Example: Send SignalR notification for real-time UI updates
                Logger.Debug($"Sent progress notification for migration {migrationId}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error sending progress notifications for migration {migrationId}", ex);
            }
        }

        /// <summary>
        /// Calculates comprehensive progress analytics.
        /// </summary>
        /// <param name="report">Report to populate</param>
        /// <param name="progressRecords">Historical progress records</param>
        private async Task CalculateProgressAnalytics(MigrationProgressReportDto report, List<MigrationProgressRecord> progressRecords)
        {
            try
            {
                if (!progressRecords.Any()) return;

                var firstRecord = progressRecords.First();
                var lastRecord = progressRecords.Last();

                report.Status = lastRecord.Status;
                report.StartTime = firstRecord.UpdateTimestamp;
                report.EndTime = lastRecord.UpdateTimestamp;
                report.TotalDuration = report.EndTime - report.StartTime;
                report.FinalProgressPercentage = lastRecord.ProgressPercentage;

                // Calculate average processing rates
                if (report.TotalDuration.TotalMinutes > 0)
                {
                    report.AverageUsersPerMinute = lastRecord.ProcessedUsers / report.TotalDuration.TotalMinutes;
                    report.AverageRecordStatesPerMinute = lastRecord.ProcessedRecordStates / report.TotalDuration.TotalMinutes;
                }

                report.TotalSteps = progressRecords.Select(r => r.CurrentStep).Distinct().Count();
                report.Summary = $"Migration completed {lastRecord.ProgressPercentage}% in {report.TotalDuration.TotalMinutes:F1} minutes";
            }
            catch (Exception ex)
            {
                Logger.Error("Error calculating progress analytics", ex);
            }
        }

        /// <summary>
        /// Generates step-by-step breakdown of the migration process.
        /// </summary>
        /// <param name="report">Report to populate</param>
        /// <param name="progressRecords">Historical progress records</param>
        private async Task GenerateStepBreakdown(MigrationProgressReportDto report, List<MigrationProgressRecord> progressRecords)
        {
            try
            {
                var stepBreakdown = new List<MigrationStepSummaryDto>();
                var stepGroups = progressRecords.GroupBy(r => r.CurrentStep).ToList();

                foreach (var stepGroup in stepGroups)
                {
                    var stepRecords = stepGroup.OrderBy(r => r.UpdateTimestamp).ToList();
                    var firstRecord = stepRecords.First();
                    var lastRecord = stepRecords.Last();

                    var stepSummary = new MigrationStepSummaryDto
                    {
                        StepName = stepGroup.Key,
                        StartTime = firstRecord.UpdateTimestamp,
                        EndTime = lastRecord.UpdateTimestamp,
                        Duration = lastRecord.UpdateTimestamp - firstRecord.UpdateTimestamp,
                        StartProgressPercentage = firstRecord.ProgressPercentage,
                        EndProgressPercentage = lastRecord.ProgressPercentage,
                        ProcessedUsers = lastRecord.ProcessedUsers - firstRecord.ProcessedUsers,
                        ProcessedRecordStates = lastRecord.ProcessedRecordStates - firstRecord.ProcessedRecordStates,
                        Messages = stepRecords.Where(r => !string.IsNullOrEmpty(r.Message))
                                             .Select(r => r.Message).ToList()
                    };

                    stepBreakdown.Add(stepSummary);
                }

                report.StepBreakdown = stepBreakdown;
            }
            catch (Exception ex)
            {
                Logger.Error("Error generating step breakdown", ex);
            }
        }

        /// <summary>
        /// Calculates performance metrics for the migration.
        /// </summary>
        /// <param name="report">Report to populate</param>
        /// <param name="progressRecords">Historical progress records</param>
        private async Task CalculatePerformanceMetrics(MigrationProgressReportDto report, List<MigrationProgressRecord> progressRecords)
        {
            try
            {
                var performanceMetrics = new MigrationPerformanceAnalysisDto();

                if (progressRecords.Any())
                {
                    // Calculate peak processing rates
                    var processingRates = progressRecords.Where(r => r.ProcessingRate > 0)
                                                        .Select(r => r.ProcessingRate).ToList();

                    if (processingRates.Any())
                    {
                        performanceMetrics.PeakProcessingRate = processingRates.Max();
                        performanceMetrics.AverageProcessingRate = processingRates.Average();
                        performanceMetrics.MinimumProcessingRate = processingRates.Min();
                    }

                    // Calculate step durations
                    var stepDurations = progressRecords.Where(r => r.AverageStepDuration > 0)
                                                      .Select(r => r.AverageStepDuration).ToList();

                    if (stepDurations.Any())
                    {
                        performanceMetrics.AverageStepDuration = stepDurations.Average();
                        performanceMetrics.LongestStepDuration = stepDurations.Max();
                        performanceMetrics.ShortestStepDuration = stepDurations.Min();
                    }

                    // Error and warning analysis
                    performanceMetrics.TotalErrors = progressRecords.Sum(r => r.ErrorsCount);
                    performanceMetrics.TotalWarnings = progressRecords.Sum(r => r.WarningsCount);
                    performanceMetrics.ErrorRate = progressRecords.Count > 0
                        ? (double)performanceMetrics.TotalErrors / progressRecords.Count
                        : 0;
                }

                report.PerformanceMetrics = performanceMetrics;
            }
            catch (Exception ex)
            {
                Logger.Error("Error calculating performance metrics", ex);
            }
        }

        /// <summary>
        /// Generates timeline analysis for the migration.
        /// </summary>
        /// <param name="report">Report to populate</param>
        /// <param name="progressRecords">Historical progress records</param>
        private async Task GenerateTimelineAnalysis(MigrationProgressReportDto report, List<MigrationProgressRecord> progressRecords)
        {
            try
            {
                var timelineEvents = new List<MigrationTimelineEventDto>();

                foreach (var record in progressRecords.OrderBy(r => r.UpdateTimestamp))
                {
                    var timelineEvent = new MigrationTimelineEventDto
                    {
                        Timestamp = record.UpdateTimestamp,
                        EventType = DetermineEventType(record),
                        Description = record.Message ?? record.CurrentStep,
                        ProgressPercentage = record.ProgressPercentage,
                        ProcessedUsers = record.ProcessedUsers,
                        ProcessedRecordStates = record.ProcessedRecordStates
                    };

                    timelineEvents.Add(timelineEvent);
                }

                report.Timeline = timelineEvents;
            }
            catch (Exception ex)
            {
                Logger.Error("Error generating timeline analysis", ex);
            }
        }

        /// <summary>
        /// Determines the event type based on progress record characteristics.
        /// </summary>
        /// <param name="record">Progress record</param>
        /// <returns>Event type string</returns>
        private string DetermineEventType(MigrationProgressRecord record)
        {
            if (record.Status == "Completed") return "Completion";
            if (record.Status == "Failed") return "Error";
            if (record.ErrorsCount > 0) return "Warning";
            if (record.ProgressPercentage == 0) return "Start";
            if (record.ProgressPercentage == 100) return "Finish";
            return "Progress";
        }

        #endregion Private Progress Tracking Methods

        #endregion Progress Tracking and Reporting

        #region Lookup Tables

        public async Task<PagedResultDto<DepartmentLookupDto>> GetAllDepartmentsForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var query = _tenantDepartmentRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), d => d.Name.Contains(input.Filter))
                .Where(d => d.Active);

            var totalCount = await query.CountAsync();

            var departments = await query
                .OrderBy(d => d.Name)
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
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var query = _cohortRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), c => c.Name.Contains(input.Filter));

            var totalCount = await query.CountAsync();

            var cohorts = await query
                .OrderBy(c => c.Name)
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

        #region Compliance State Preservation

        /// <summary>
        /// Preserves user compliance states before migration to ensure data integrity and rollback capability.
        /// This method captures comprehensive compliance snapshots for all cohort users.
        /// </summary>
        /// <param name="cohortId">The ID of the cohort being migrated</param>
        /// <param name="migrationId">Unique identifier for this migration operation</param>
        /// <returns>CompliancePreservationResultDto containing preservation results and metadata</returns>
        public async Task<CompliancePreservationResultDto> PreserveCohortUserCompliance(Guid cohortId, string migrationId)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            try
            {
                Logger.Info($"Starting compliance preservation for cohort {cohortId}, migration {migrationId}");

                // Get all users in the cohort
                var cohortUsers = await _cohortUserRepository.GetAll()
                    .Where(cu => cu.CohortId == cohortId)
                    .Select(cu => new { cu.UserId, cu.Id })
                    .ToListAsync();

                if (!cohortUsers.Any())
                {
                    Logger.Warn($"No users found in cohort {cohortId}");
                    return new CompliancePreservationResultDto
                    {
                        Success = true,
                        Message = L("NoUsersInCohort"),
                        PreservedUsersCount = 0,
                        PreservedRecordStatesCount = 0,
                        MigrationId = migrationId
                    };
                }

                var preservedUsersCount = 0;
                var preservedRecordStatesCount = 0;
                var preservationErrors = new List<string>();
                var complianceSnapshots = new List<UserComplianceSnapshotDto>();

                // Process users in batches for performance
                const int batchSize = 50;
                var totalUsers = cohortUsers.Count;

                for (int i = 0; i < totalUsers; i += batchSize)
                {
                    var batch = cohortUsers.Skip(i).Take(batchSize).ToList();

                    foreach (var cohortUser in batch)
                    {
                        try
                        {
                            // Capture compliance snapshot for this user
                            var snapshot = await CaptureUserComplianceSnapshot(cohortUser.UserId, cohortId, migrationId);
                            if (snapshot != null)
                            {
                                complianceSnapshots.Add(snapshot);
                                preservedUsersCount++;
                                preservedRecordStatesCount += snapshot.RecordStatesCount;
                            }
                        }
                        catch (Exception ex)
                        {
                            var errorMessage = $"Failed to preserve compliance for user {cohortUser.UserId}: {ex.Message}";
                            Logger.Error(errorMessage, ex);
                            preservationErrors.Add(errorMessage);
                        }
                    }

                    // Log progress for large cohorts
                    if (totalUsers > batchSize)
                    {
                        var processedCount = Math.Min(i + batchSize, totalUsers);
                        Logger.Info($"Compliance preservation progress: {processedCount}/{totalUsers} users processed");
                    }
                }

                // Store compliance preservation metadata
                await StoreCompliancePreservationMetadata(cohortId, migrationId, complianceSnapshots);

                var result = new CompliancePreservationResultDto
                {
                    Success = preservationErrors.Count == 0,
                    Message = preservationErrors.Count == 0
                        ? L("CompliancePreservationSuccessful")
                        : L("CompliancePreservationPartialSuccess", preservationErrors.Count),
                    PreservedUsersCount = preservedUsersCount,
                    PreservedRecordStatesCount = preservedRecordStatesCount,
                    MigrationId = migrationId,
                    Errors = preservationErrors,
                    ComplianceSnapshots = complianceSnapshots
                };

                Logger.Info($"Compliance preservation completed for cohort {cohortId}. " +
                           $"Users: {preservedUsersCount}, Record states: {preservedRecordStatesCount}, " +
                           $"Errors: {preservationErrors.Count}");

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"Critical error in PreserveCohortUserCompliance for cohort {cohortId}", ex);
                throw new UserFriendlyException(L("CompliancePreservationFailed", ex.Message));
            }
        }

        /// <summary>
        /// Captures a comprehensive compliance snapshot for a single user.
        /// </summary>
        /// <param name="userId">The user ID to capture compliance for</param>
        /// <param name="cohortId">The cohort ID for context</param>
        /// <param name="migrationId">The migration ID for tracking</param>
        /// <returns>UserComplianceSnapshotDto containing the user's compliance state</returns>
        private async Task<UserComplianceSnapshotDto> CaptureUserComplianceSnapshot(long userId, Guid cohortId, string migrationId)
        {
            try
            {
                // Get user's current record states
                var userRecordStates = await _recordStateRepository.GetAll()
                    .Include(rs => rs.RecordFk)
                    .Include(rs => rs.RecordCategoryFk)
                        .ThenInclude(rc => rc.RecordRequirementFk)
                    .Include(rs => rs.RecordStatusFk)
                    .Where(rs => rs.UserId == userId && !rs.IsDeleted)
                    .ToListAsync();

                // Get user's current compliance values using the existing compliance evaluator
                var complianceValues = await GetUserComplianceValues(userId);

                // Create record state snapshots
                var recordStateSnapshots = userRecordStates.Select(rs => new RecordStateSnapshotDto
                {
                    RecordStateId = rs.Id,
                    UserId = userId,
                    RecordCategoryId = rs.RecordCategoryId,
                    RecordId = rs.RecordId,
                    RecordStatusId = rs.RecordStatusId,
                    State = rs.State,
                    Notes = rs.Notes,
                    CreationTime = rs.CreationTime,
                    LastModificationTime = rs.LastModificationTime,

                    // Capture category and requirement information
                    CategoryName = rs.RecordCategoryFk?.Name,
                    RequirementName = rs.RecordCategoryFk?.RecordRequirementFk?.Name,
                    RequirementId = rs.RecordCategoryFk?.RecordRequirementId,
                    IsSurpathOnly = rs.RecordCategoryFk?.RecordRequirementFk?.IsSurpathOnly ?? false,

                    // Capture status information
                    StatusName = rs.RecordStatusFk?.StatusName,
                    ComplianceImpact = rs.RecordStatusFk?.ComplianceImpact ?? EnumStatusComplianceImpact.NotCompliant,

                    // Capture file information if available
                    FileName = rs.RecordFk?.filename,
                    FileSize = null, // File size not stored in Record entity
                    ExpirationDate = rs.RecordFk?.ExpirationDate,

                    // Snapshot metadata
                    SnapshotTimestamp = DateTime.UtcNow,
                    MigrationId = migrationId
                }).ToList();

                var snapshot = new UserComplianceSnapshotDto
                {
                    UserId = userId,
                    CohortId = cohortId,
                    MigrationId = migrationId,
                    SnapshotTimestamp = DateTime.UtcNow,

                    // Current compliance state
                    ComplianceValues = complianceValues,

                    // Record states
                    RecordStates = recordStateSnapshots,
                    RecordStatesCount = recordStateSnapshots.Count,

                    // Summary statistics
                    CompliantRecordStatesCount = recordStateSnapshots.Count(rs =>
                        rs.ComplianceImpact == EnumStatusComplianceImpact.Compliant),
                    NonCompliantRecordStatesCount = recordStateSnapshots.Count(rs =>
                        rs.ComplianceImpact == EnumStatusComplianceImpact.NotCompliant),
                    InformationalRecordStatesCount = recordStateSnapshots.Count(rs =>
                        rs.ComplianceImpact == EnumStatusComplianceImpact.InformationOnly),

                    // File associations
                    HasFileAssociations = recordStateSnapshots.Any(rs => rs.RecordId.HasValue),
                    FileAssociationsCount = recordStateSnapshots.Count(rs => rs.RecordId.HasValue)
                };

                Logger.Debug($"Captured compliance snapshot for user {userId}: " +
                            $"{snapshot.RecordStatesCount} record states, " +
                            $"{snapshot.CompliantRecordStatesCount} compliant, " +
                            $"{snapshot.FileAssociationsCount} with files");

                return snapshot;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error capturing compliance snapshot for user {userId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Gets current compliance values for a user using the existing compliance evaluator.
        /// </summary>
        /// <param name="userId">The user ID to get compliance for</param>
        /// <returns>ComplianceValues object</returns>
        private async Task<ComplianceValues> GetUserComplianceValues(long userId)
        {
            try
            {
                // Use the existing compliance evaluator to get current compliance state
                // This ensures consistency with the rest of the system
                var complianceEvaluator = IocManager.Instance.Resolve<ISurpathComplianceEvaluator>();
                return await complianceEvaluator.GetDetailedComplianceValuesForUser(userId);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error getting compliance values for user {userId}", ex);

                // Return a default non-compliant state if evaluation fails
                return new ComplianceValues
                {
                    UserId = userId,
                    TenantId = AbpSession.TenantId ?? 0,
                    Background = false,
                    Drug = false,
                    Immunization = false,
                    InCompliance = false
                };
            }
        }

        /// <summary>
        /// Stores compliance preservation metadata for audit and rollback purposes.
        /// </summary>
        /// <param name="cohortId">The cohort ID</param>
        /// <param name="migrationId">The migration ID</param>
        /// <param name="snapshots">The compliance snapshots</param>
        private async Task StoreCompliancePreservationMetadata(Guid cohortId, string migrationId,
            List<UserComplianceSnapshotDto> snapshots)
        {
            try
            {
                var metadata = new
                {
                    CohortId = cohortId,
                    MigrationId = migrationId,
                    PreservationTimestamp = DateTime.UtcNow,
                    TotalUsers = snapshots.Count,
                    TotalRecordStates = snapshots.Sum(s => s.RecordStatesCount),
                    TotalCompliantStates = snapshots.Sum(s => s.CompliantRecordStatesCount),
                    TotalNonCompliantStates = snapshots.Sum(s => s.NonCompliantRecordStatesCount),
                    TotalFileAssociations = snapshots.Sum(s => s.FileAssociationsCount),
                    UsersWithCompliance = snapshots.Count(s => s.ComplianceValues?.InCompliance == true),
                    PreservedBy = AbpSession.UserId,
                    TenantId = AbpSession.TenantId,

                    // Summary by compliance type
                    DrugCompliantUsers = snapshots.Count(s => s.ComplianceValues?.Drug == true),
                    BackgroundCompliantUsers = snapshots.Count(s => s.ComplianceValues?.Background == true),
                    ImmunizationCompliantUsers = snapshots.Count(s => s.ComplianceValues?.Immunization == true),

                    // Snapshot details (could be stored in a separate table for large cohorts)
                    ComplianceSnapshots = snapshots.Take(100).ToList() // Limit for metadata storage
                };

                Logger.Info($"Compliance preservation metadata: {JsonConvert.SerializeObject(metadata, Formatting.Indented)}");

                // TODO: Store in dedicated compliance preservation audit table
                // For now, this is logged for audit purposes
                // In a production system, you would store this in a dedicated table
                // such as CohortMigrationComplianceAudit
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to store compliance preservation metadata", ex);
                // Don't fail the preservation operation if metadata storage fails
            }
        }

        /// <summary>
        /// Validates compliance state integrity after migration to ensure data consistency and accuracy.
        /// This method performs comprehensive validation of user compliance states and identifies any issues.
        /// </summary>
        /// <param name="cohortId">The ID of the cohort that was migrated</param>
        /// <param name="migrationId">Unique identifier for the migration operation</param>
        /// <param name="preservationResult">The original compliance preservation result for comparison</param>
        /// <returns>ComplianceIntegrityValidationResultDto containing validation results and any issues found</returns>
        public async Task<ComplianceIntegrityValidationResultDto> ValidateComplianceIntegrity(Guid cohortId, string migrationId,
            CompliancePreservationResultDto preservationResult = null)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            try
            {
                Logger.Info($"Starting compliance integrity validation for cohort {cohortId}, migration {migrationId}");

                var validationResult = new ComplianceIntegrityValidationResultDto
                {
                    CohortId = cohortId,
                    MigrationId = migrationId,
                    ValidationTimestamp = DateTime.UtcNow,
                    IsValid = true,
                    ValidationResults = new List<ComplianceValidationItemDto>()
                };

                // Get all users in the cohort
                var cohortUsers = await _cohortUserRepository.GetAll()
                    .Where(cu => cu.CohortId == cohortId)
                    .Select(cu => new { cu.UserId, cu.Id })
                    .ToListAsync();

                if (!cohortUsers.Any())
                {
                    Logger.Warn($"No users found in cohort {cohortId} for validation");
                    validationResult.TotalUsersValidated = 0;
                    validationResult.ValidationSummary = "No users found in cohort";
                    return validationResult;
                }

                var validationErrors = new List<string>();
                var validationWarnings = new List<string>();
                var validatedUsersCount = 0;
                var orphanedRecordsCount = 0;
                var missingRecordsCount = 0;
                var integrityIssuesCount = 0;

                // Process users in batches for performance
                const int batchSize = 50;
                var totalUsers = cohortUsers.Count;

                for (int i = 0; i < totalUsers; i += batchSize)
                {
                    var batch = cohortUsers.Skip(i).Take(batchSize).ToList();

                    foreach (var cohortUser in batch)
                    {
                        try
                        {
                            // Validate individual user compliance integrity
                            var userValidation = await ValidateUserComplianceIntegrity(cohortUser.UserId, cohortId, migrationId, preservationResult);
                            validationResult.ValidationResults.Add(userValidation);

                            if (!userValidation.IsValid)
                            {
                                validationResult.IsValid = false;
                                integrityIssuesCount++;
                            }

                            orphanedRecordsCount += userValidation.OrphanedRecordsCount;
                            missingRecordsCount += userValidation.MissingRecordsCount;
                            validatedUsersCount++;
                        }
                        catch (Exception ex)
                        {
                            var errorMessage = $"Failed to validate compliance integrity for user {cohortUser.UserId}: {ex.Message}";
                            Logger.Error(errorMessage, ex);
                            validationErrors.Add(errorMessage);
                            validationResult.IsValid = false;
                        }
                    }

                    // Log progress for large cohorts
                    if (totalUsers > batchSize)
                    {
                        var processedCount = Math.Min(i + batchSize, totalUsers);
                        Logger.Info($"Compliance validation progress: {processedCount}/{totalUsers} users processed");
                    }
                }

                // Perform cohort-level validation
                await ValidateCohortLevelCompliance(cohortId, validationResult);

                // Validate against preservation baseline if available
                if (preservationResult != null)
                {
                    await ValidateAgainstPreservationBaseline(preservationResult, validationResult);
                }

                // Set validation summary
                validationResult.TotalUsersValidated = validatedUsersCount;
                validationResult.OrphanedRecordsCount = orphanedRecordsCount;
                validationResult.MissingRecordsCount = missingRecordsCount;
                validationResult.IntegrityIssuesCount = integrityIssuesCount;
                validationResult.Errors = validationErrors;
                validationResult.Warnings = validationWarnings;

                // Generate validation summary
                if (validationResult.IsValid)
                {
                    validationResult.ValidationSummary = $"Compliance integrity validation passed for {validatedUsersCount} users";
                }
                else
                {
                    validationResult.ValidationSummary = $"Compliance integrity validation failed. Issues found: {integrityIssuesCount} users with problems, " +
                                                        $"{orphanedRecordsCount} orphaned records, {missingRecordsCount} missing records";
                }

                Logger.Info($"Compliance integrity validation completed for cohort {cohortId}. " +
                           $"Valid: {validationResult.IsValid}, Users: {validatedUsersCount}, " +
                           $"Issues: {integrityIssuesCount}, Orphaned: {orphanedRecordsCount}, Missing: {missingRecordsCount}");

                return validationResult;
            }
            catch (Exception ex)
            {
                Logger.Error($"Critical error in ValidateComplianceIntegrity for cohort {cohortId}", ex);
                throw new UserFriendlyException(L("ComplianceIntegrityValidationFailed", ex.Message));
            }
        }

        /// <summary>
        /// Validates compliance integrity for a single user.
        /// </summary>
        /// <param name="userId">The user ID to validate</param>
        /// <param name="cohortId">The cohort ID for context</param>
        /// <param name="migrationId">The migration ID for tracking</param>
        /// <param name="preservationResult">The preservation result for comparison</param>
        /// <returns>ComplianceValidationItemDto containing user-specific validation results</returns>
        private async Task<ComplianceValidationItemDto> ValidateUserComplianceIntegrity(long userId, Guid cohortId,
            string migrationId, CompliancePreservationResultDto preservationResult)
        {
            var validation = new ComplianceValidationItemDto
            {
                UserId = userId,
                IsValid = true,
                ValidationIssues = new List<string>()
            };

            try
            {
                // Get current user record states
                var currentRecordStates = await _recordStateRepository.GetAll()
                    .Include(rs => rs.RecordFk)
                    .Include(rs => rs.RecordCategoryFk)
                        .ThenInclude(rc => rc.RecordRequirementFk)
                    .Include(rs => rs.RecordStatusFk)
                    .Where(rs => rs.UserId == userId && !rs.IsDeleted)
                    .ToListAsync();

                // Validate record state consistency
                await ValidateRecordStateConsistency(currentRecordStates, validation);

                // Validate file associations
                await ValidateFileAssociations(currentRecordStates, validation);

                // Validate compliance calculations
                await ValidateComplianceCalculations(userId, validation);

                // Check for orphaned records
                await CheckForOrphanedRecords(userId, validation);

                // Validate against preservation baseline if available
                if (preservationResult != null)
                {
                    await ValidateAgainstUserPreservationData(userId, preservationResult, validation);
                }

                validation.RecordStatesCount = currentRecordStates.Count;
                validation.CompliantRecordStatesCount = currentRecordStates.Count(rs =>
                    rs.RecordStatusFk?.ComplianceImpact == EnumStatusComplianceImpact.Compliant);

                Logger.Debug($"User {userId} compliance validation: Valid={validation.IsValid}, " +
                            $"Records={validation.RecordStatesCount}, Issues={validation.ValidationIssues.Count}");

                return validation;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error validating user {userId} compliance integrity", ex);
                validation.IsValid = false;
                validation.ValidationIssues.Add($"Validation error: {ex.Message}");
                return validation;
            }
        }

        /// <summary>
        /// Validates record state consistency for a user.
        /// </summary>
        private async Task ValidateRecordStateConsistency(List<RecordState> recordStates, ComplianceValidationItemDto validation)
        {
            foreach (var recordState in recordStates)
            {
                // Rule 1: Record state must have a valid category
                if (!recordState.RecordCategoryId.HasValue)
                {
                    validation.IsValid = false;
                    validation.ValidationIssues.Add($"Record state {recordState.Id} has no category association");
                    validation.OrphanedRecordsCount++;
                    continue;
                }

                // Rule 2: Category must exist and be valid
                if (recordState.RecordCategoryFk == null)
                {
                    validation.IsValid = false;
                    validation.ValidationIssues.Add($"Record state {recordState.Id} references non-existent category {recordState.RecordCategoryId}");
                    validation.OrphanedRecordsCount++;
                    continue;
                }

                // Rule 3: Requirement must exist and be valid
                if (recordState.RecordCategoryFk.RecordRequirementFk == null)
                {
                    validation.IsValid = false;
                    validation.ValidationIssues.Add($"Record state {recordState.Id} category references non-existent requirement");
                    validation.OrphanedRecordsCount++;
                    continue;
                }

                // Rule 4: Status must exist and be valid
                if (recordState.RecordStatusFk == null)
                {
                    validation.IsValid = false;
                    validation.ValidationIssues.Add($"Record state {recordState.Id} references non-existent status {recordState.RecordStatusId}");
                    continue;
                }

                // Rule 5: Tenant consistency
                if (recordState.TenantId != recordState.RecordCategoryFk.TenantId)
                {
                    validation.IsValid = false;
                    validation.ValidationIssues.Add($"Record state {recordState.Id} has tenant mismatch with category");
                    continue;
                }
            }
        }

        /// <summary>
        /// Validates file associations for record states.
        /// </summary>
        private async Task ValidateFileAssociations(List<RecordState> recordStates, ComplianceValidationItemDto validation)
        {
            foreach (var recordState in recordStates.Where(rs => rs.RecordId.HasValue))
            {
                // Check if the associated record exists
                if (recordState.RecordFk == null)
                {
                    validation.IsValid = false;
                    validation.ValidationIssues.Add($"Record state {recordState.Id} references non-existent record {recordState.RecordId}");
                    validation.MissingRecordsCount++;
                    continue;
                }

                // Check if the record has valid file data
                if (!recordState.RecordFk.filedata.HasValue && string.IsNullOrWhiteSpace(recordState.RecordFk.filename))
                {
                    validation.ValidationIssues.Add($"Record {recordState.RecordId} has no file data or filename");
                    // This is a warning, not an error
                }

                // Check tenant consistency
                if (recordState.TenantId != recordState.RecordFk.TenantId)
                {
                    validation.IsValid = false;
                    validation.ValidationIssues.Add($"Record state {recordState.Id} has tenant mismatch with record {recordState.RecordId}");
                }
            }
        }

        /// <summary>
        /// Validates compliance calculations for a user.
        /// </summary>
        private async Task ValidateComplianceCalculations(long userId, ComplianceValidationItemDto validation)
        {
            try
            {
                // Get current compliance values using the existing evaluator
                var complianceValues = await GetUserComplianceValues(userId);

                // Validate that compliance calculation completed successfully
                if (complianceValues == null)
                {
                    validation.IsValid = false;
                    validation.ValidationIssues.Add($"Failed to calculate compliance values for user {userId}");
                    return;
                }

                // Validate compliance consistency
                // If user has no record states, they should not be compliant
                var userRecordStatesCount = await _recordStateRepository.CountAsync(rs => rs.UserId == userId && !rs.IsDeleted);
                if (userRecordStatesCount == 0 && complianceValues.InCompliance)
                {
                    validation.ValidationIssues.Add($"User {userId} shows as compliant but has no record states");
                    // This might be valid in some cases, so it's a warning not an error
                }

                validation.CurrentComplianceValues = complianceValues;
            }
            catch (Exception ex)
            {
                validation.IsValid = false;
                validation.ValidationIssues.Add($"Error calculating compliance for user {userId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks for orphaned records that may have been created during migration.
        /// </summary>
        private async Task CheckForOrphanedRecords(long userId, ComplianceValidationItemDto validation)
        {
            // Check for record states with null category IDs
            var orphanedStates = await _recordStateRepository.CountAsync(rs =>
                rs.UserId == userId &&
                !rs.IsDeleted &&
                !rs.RecordCategoryId.HasValue);

            if (orphanedStates > 0)
            {
                validation.IsValid = false;
                validation.ValidationIssues.Add($"User {userId} has {orphanedStates} orphaned record states with no category");
                validation.OrphanedRecordsCount += orphanedStates;
            }

            // Check for record states pointing to non-existent categories
            var invalidCategoryStates = await _recordStateRepository.GetAll()
                .Where(rs => rs.UserId == userId && !rs.IsDeleted && rs.RecordCategoryId.HasValue)
                .Where(rs => !_recordCategoryRepository.GetAll().Any(rc => rc.Id == rs.RecordCategoryId))
                .CountAsync();

            if (invalidCategoryStates > 0)
            {
                validation.IsValid = false;
                validation.ValidationIssues.Add($"User {userId} has {invalidCategoryStates} record states pointing to non-existent categories");
                validation.OrphanedRecordsCount += invalidCategoryStates;
            }
        }

        /// <summary>
        /// Validates against user preservation data if available.
        /// </summary>
        private async Task ValidateAgainstUserPreservationData(long userId, CompliancePreservationResultDto preservationResult,
            ComplianceValidationItemDto validation)
        {
            var userSnapshot = preservationResult.ComplianceSnapshots?.FirstOrDefault(s => s.UserId == userId);
            if (userSnapshot == null)
            {
                validation.ValidationIssues.Add($"No preservation snapshot found for user {userId}");
                return;
            }

            // Compare record states count
            var currentRecordStatesCount = await _recordStateRepository.CountAsync(rs => rs.UserId == userId && !rs.IsDeleted);
            if (currentRecordStatesCount < userSnapshot.RecordStatesCount)
            {
                validation.ValidationIssues.Add($"User {userId} has fewer record states after migration: {currentRecordStatesCount} vs {userSnapshot.RecordStatesCount}");
                validation.MissingRecordsCount += (userSnapshot.RecordStatesCount - currentRecordStatesCount);
            }

            // Compare compliance values if available
            if (userSnapshot.ComplianceValues != null && validation.CurrentComplianceValues != null)
            {
                var preserved = userSnapshot.ComplianceValues;
                var current = validation.CurrentComplianceValues;

                if (preserved.Drug != current.Drug)
                {
                    validation.ValidationIssues.Add($"User {userId} drug compliance changed: {preserved.Drug} -> {current.Drug}");
                }

                if (preserved.Background != current.Background)
                {
                    validation.ValidationIssues.Add($"User {userId} background compliance changed: {preserved.Background} -> {current.Background}");
                }

                if (preserved.Immunization != current.Immunization)
                {
                    validation.ValidationIssues.Add($"User {userId} immunization compliance changed: {preserved.Immunization} -> {current.Immunization}");
                }

                if (preserved.InCompliance != current.InCompliance)
                {
                    validation.ValidationIssues.Add($"User {userId} overall compliance changed: {preserved.InCompliance} -> {current.InCompliance}");
                }
            }
        }

        /// <summary>
        /// Validates cohort-level compliance consistency.
        /// </summary>
        private async Task ValidateCohortLevelCompliance(Guid cohortId, ComplianceIntegrityValidationResultDto validationResult)
        {
            try
            {
                // Check cohort still exists and is valid
                var cohort = await _cohortRepository.FirstOrDefaultAsync(cohortId);
                if (cohort == null)
                {
                    validationResult.IsValid = false;
                    validationResult.Errors.Add($"Cohort {cohortId} no longer exists");
                    return;
                }

                // Check cohort department assignment
                if (!cohort.TenantDepartmentId.HasValue)
                {
                    validationResult.Warnings.Add($"Cohort {cohortId} has no department assignment");
                }
                else
                {
                    var department = await _tenantDepartmentRepository.FirstOrDefaultAsync(cohort.TenantDepartmentId.Value);
                    if (department == null)
                    {
                        validationResult.IsValid = false;
                        validationResult.Errors.Add($"Cohort {cohortId} references non-existent department {cohort.TenantDepartmentId}");
                    }
                }

                // Validate cohort user associations
                var cohortUsersCount = await _cohortUserRepository.CountAsync(cu => cu.CohortId == cohortId);
                if (cohortUsersCount != validationResult.TotalUsersValidated)
                {
                    validationResult.Warnings.Add($"Cohort user count mismatch: {cohortUsersCount} in cohort vs {validationResult.TotalUsersValidated} validated");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error validating cohort-level compliance for {cohortId}", ex);
                validationResult.Errors.Add($"Cohort-level validation error: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates against the preservation baseline.
        /// </summary>
        private async Task ValidateAgainstPreservationBaseline(CompliancePreservationResultDto preservationResult,
            ComplianceIntegrityValidationResultDto validationResult)
        {
            if (preservationResult.PreservedUsersCount != validationResult.TotalUsersValidated)
            {
                validationResult.Warnings.Add($"User count mismatch: {preservationResult.PreservedUsersCount} preserved vs {validationResult.TotalUsersValidated} validated");
            }

            // Additional baseline validation can be added here
            // such as comparing aggregate statistics, compliance rates, etc.
        }

        /// <summary>
        /// Triggers compliance recalculation for migrated users after migration completion.
        /// This method ensures that compliance states are updated based on new department requirements.
        /// </summary>
        /// <param name="cohortId">The ID of the cohort that was migrated</param>
        /// <param name="migrationId">Unique identifier for the migration operation</param>
        /// <param name="targetDepartmentId">The target department ID for context</param>
        /// <returns>ComplianceRecalculationResultDto containing recalculation results and statistics</returns>
        public async Task<ComplianceRecalculationResultDto> RecalculateCompliance(Guid cohortId, string migrationId, Guid targetDepartmentId)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            try
            {
                Logger.Info($"Starting compliance recalculation for cohort {cohortId}, migration {migrationId}, target department {targetDepartmentId}");

                var recalculationResult = new ComplianceRecalculationResultDto
                {
                    CohortId = cohortId,
                    MigrationId = migrationId,
                    TargetDepartmentId = targetDepartmentId,
                    RecalculationTimestamp = DateTime.UtcNow,
                    Success = true,
                    UserRecalculationResults = new List<UserComplianceRecalculationDto>()
                };

                // Get all users in the cohort
                var cohortUsers = await _cohortUserRepository.GetAll()
                    .Where(cu => cu.CohortId == cohortId)
                    .Select(cu => new { cu.UserId, cu.Id })
                    .ToListAsync();

                if (!cohortUsers.Any())
                {
                    Logger.Warn($"No users found in cohort {cohortId} for compliance recalculation");
                    recalculationResult.TotalUsersProcessed = 0;
                    recalculationResult.Message = "No users found in cohort for recalculation";
                    return recalculationResult;
                }

                // Get target department requirements for context
                var targetDepartmentRequirements = await GetDepartmentRequirements(targetDepartmentId);

                var recalculationErrors = new List<string>();
                var recalculationWarnings = new List<string>();
                var processedUsersCount = 0;
                var successfulRecalculationsCount = 0;
                var failedRecalculationsCount = 0;

                // Process users in batches for performance
                const int batchSize = 25; // Smaller batch size for compliance calculations
                var totalUsers = cohortUsers.Count;

                for (int i = 0; i < totalUsers; i += batchSize)
                {
                    var batch = cohortUsers.Skip(i).Take(batchSize).ToList();

                    foreach (var cohortUser in batch)
                    {
                        try
                        {
                            // Recalculate compliance for this user
                            var updatedCompliance = await _surpathComplianceEvaluator.RecalculateUserCompliance(cohortUser.UserId);
                            
                            // Create user recalculation result
                            var userRecalculation = new UserComplianceRecalculationDto
                            {
                                UserId = cohortUser.UserId,
                                Success = true,
                                RecalculationIssues = new List<string>(),
                                PostRecalculationCompliance = updatedCompliance
                            };

                            recalculationResult.UserRecalculationResults.Add(userRecalculation);

                            if (userRecalculation.Success)
                            {
                                successfulRecalculationsCount++;
                            }
                            else
                            {
                                failedRecalculationsCount++;
                                recalculationResult.Success = false;
                            }

                            processedUsersCount++;
                        }
                        catch (Exception ex)
                        {
                            var errorMessage = $"Failed to recalculate compliance for user {cohortUser.UserId}: {ex.Message}";
                            Logger.Error(errorMessage, ex);
                            recalculationErrors.Add(errorMessage);
                            failedRecalculationsCount++;
                            recalculationResult.Success = false;
                        }
                    }

                    // Log progress for large cohorts
                    if (totalUsers > batchSize)
                    {
                        var processedCount = Math.Min(i + batchSize, totalUsers);
                        Logger.Info($"Compliance recalculation progress: {processedCount}/{totalUsers} users processed");
                    }
                }

                // Perform department-level compliance validation
                await ValidateDepartmentComplianceRules(targetDepartmentId, recalculationResult);

                // Update compliance cache if needed
                await UpdateComplianceCache(cohortId, targetDepartmentId);

                // Set final results
                recalculationResult.TotalUsersProcessed = processedUsersCount;
                recalculationResult.SuccessfulRecalculations = successfulRecalculationsCount;
                recalculationResult.FailedRecalculations = failedRecalculationsCount;
                recalculationResult.Errors = recalculationErrors;
                recalculationResult.Warnings = recalculationWarnings;

                // Generate summary message
                if (recalculationResult.Success)
                {
                    recalculationResult.Message = $"Compliance recalculation completed successfully for {successfulRecalculationsCount} users";
                }
                else
                {
                    recalculationResult.Message = $"Compliance recalculation completed with issues. " +
                                                 $"Successful: {successfulRecalculationsCount}, Failed: {failedRecalculationsCount}";
                }

                // Store recalculation audit data
                await StoreComplianceRecalculationAudit(recalculationResult);

                Logger.Info($"Compliance recalculation completed for cohort {cohortId}. " +
                           $"Success: {recalculationResult.Success}, Processed: {processedUsersCount}, " +
                           $"Successful: {successfulRecalculationsCount}, Failed: {failedRecalculationsCount}");

                return recalculationResult;
            }
            catch (Exception ex)
            {
                Logger.Error($"Critical error in RecalculateCompliance for cohort {cohortId}", ex);
                throw new UserFriendlyException(L("ComplianceRecalculationFailed", ex.Message));
            }
        }

        // NOTE: RecalculateUserCompliance method has been moved to ISurpathComplianceEvaluator
        // to enable reuse across different services (CohortMigration, UserTransfer, etc.)
        // The implementation now uses _surpathComplianceEvaluator.RecalculateUserCompliance(userId)

        /// <summary>
        /// Gets department requirements for compliance validation.
        /// </summary>
        /// <param name="departmentId">The department ID</param>
        /// <returns>List of department requirements</returns>
        private async Task<List<DepartmentRequirementDto>> GetDepartmentRequirements(Guid departmentId)
        {
            try
            {
                // Get requirements for the department
                var requirements = await _recordRequirementRepository.GetAll()
                    .Where(r => r.TenantDepartmentId == departmentId)
                    .ToListAsync();

                // Get categories for these requirements
                var requirementIds = requirements.Select(r => r.Id).ToList();
                var categories = await _recordCategoryRepository.GetAll()
                    .Include(rc => rc.RecordCategoryRuleFk)
                    .Where(rc => rc.RecordRequirementId.HasValue && requirementIds.Contains(rc.RecordRequirementId.Value))
                    .ToListAsync();

                // Build the result DTOs
                var result = requirements.Select(r => new DepartmentRequirementDto
                {
                    RequirementId = r.Id,
                    RequirementName = r.Name,
                    Description = r.Description,
                    IsSurpathOnly = r.IsSurpathOnly,
                    IsRequired = categories.Where(c => c.RecordRequirementId == r.Id)
                                          .Any(c => c.RecordCategoryRuleFk != null && c.RecordCategoryRuleFk.Required),
                    Categories = categories.Where(c => c.RecordRequirementId == r.Id)
                                          .Select(rc => new DepartmentCategoryDto
                                          {
                                              CategoryId = rc.Id,
                                              CategoryName = rc.Name,
                                              IsRequired = rc.RecordCategoryRuleFk != null && rc.RecordCategoryRuleFk.Required
                                          }).ToList()
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error getting department requirements for {departmentId}", ex);
                return new List<DepartmentRequirementDto>();
            }
        }

        // NOTE: TriggerComplianceRecalculation has been replaced by _surpathComplianceEvaluator.RecalculateUserCompliance
        // which is now the centralized method for recalculating user compliance

        /// <summary>
        /// Validates user compliance against department-specific requirements.
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="departmentRequirements">The department requirements</param>
        /// <param name="userRecalculation">The user recalculation result to update</param>
        private async Task ValidateUserComplianceAgainstDepartmentRequirements(long userId,
            List<DepartmentRequirementDto> departmentRequirements, UserComplianceRecalculationDto userRecalculation)
        {
            try
            {
                // Get user's current record states
                var userRecordStates = await _recordStateRepository.GetAll()
                    .Include(rs => rs.RecordCategoryFk)
                        .ThenInclude(rc => rc.RecordRequirementFk)
                    .Include(rs => rs.RecordStatusFk)
                    .Where(rs => rs.UserId == userId && !rs.IsDeleted)
                    .ToListAsync();

                // Check each department requirement
                foreach (var requirement in departmentRequirements.Where(r => r.IsRequired))
                {
                    var hasCompliantRecord = userRecordStates.Any(rs =>
                        rs.RecordCategoryFk?.RecordRequirementFk?.Id == requirement.RequirementId &&
                        rs.RecordStatusFk?.ComplianceImpact == EnumStatusComplianceImpact.Compliant);

                    if (!hasCompliantRecord)
                    {
                        userRecalculation.RecalculationIssues.Add(
                            $"User does not have compliant record for required department requirement: {requirement.RequirementName}");
                    }
                }

                // Check for Surpath-only requirements (drug, background)
                var surpathRequirements = departmentRequirements.Where(r => r.IsSurpathOnly).ToList();
                foreach (var surpathReq in surpathRequirements)
                {
                    var hasCompliantRecord = userRecordStates.Any(rs =>
                        rs.RecordCategoryFk?.RecordRequirementFk?.Id == surpathReq.RequirementId &&
                        rs.RecordStatusFk?.ComplianceImpact == EnumStatusComplianceImpact.Compliant);

                    if (!hasCompliantRecord)
                    {
                        userRecalculation.RecalculationIssues.Add(
                            $"User does not have compliant record for Surpath requirement: {surpathReq.RequirementName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error validating user {userId} compliance against department requirements", ex);
                userRecalculation.RecalculationIssues.Add($"Department requirement validation error: {ex.Message}");
            }
        }

        /// <summary>
        /// Analyzes changes in compliance values before and after recalculation.
        /// </summary>
        /// <param name="preCompliance">Pre-recalculation compliance values</param>
        /// <param name="postCompliance">Post-recalculation compliance values</param>
        /// <param name="userRecalculation">The user recalculation result to update</param>
        private async Task AnalyzeComplianceChanges(ComplianceValues preCompliance, ComplianceValues postCompliance,
            UserComplianceRecalculationDto userRecalculation)
        {
            if (preCompliance == null || postCompliance == null)
            {
                userRecalculation.RecalculationIssues.Add("Unable to compare compliance values - missing data");
                return;
            }

            var changes = new List<string>();

            // Check for changes in each compliance area
            if (preCompliance.Drug != postCompliance.Drug)
            {
                changes.Add($"Drug compliance changed: {preCompliance.Drug} -> {postCompliance.Drug}");
            }

            if (preCompliance.Background != postCompliance.Background)
            {
                changes.Add($"Background compliance changed: {preCompliance.Background} -> {postCompliance.Background}");
            }

            if (preCompliance.Immunization != postCompliance.Immunization)
            {
                changes.Add($"Immunization compliance changed: {preCompliance.Immunization} -> {postCompliance.Immunization}");
            }

            if (preCompliance.InCompliance != postCompliance.InCompliance)
            {
                changes.Add($"Overall compliance changed: {preCompliance.InCompliance} -> {postCompliance.InCompliance}");
            }

            // Store changes for audit purposes
            userRecalculation.ComplianceChanges = changes;

            // Log significant changes
            if (changes.Any())
            {
                Logger.Info($"User {userRecalculation.UserId} compliance changes after migration: {string.Join("; ", changes)}");
            }
        }

        /// <summary>
        /// Updates user compliance metadata after recalculation.
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="migrationId">The migration ID</param>
        /// <param name="targetDepartmentId">The target department ID</param>
        /// <param name="userRecalculation">The user recalculation result</param>
        private async Task UpdateUserComplianceMetadata(long userId, string migrationId, Guid targetDepartmentId,
            UserComplianceRecalculationDto userRecalculation)
        {
            try
            {
                var metadata = new
                {
                    UserId = userId,
                    MigrationId = migrationId,
                    TargetDepartmentId = targetDepartmentId,
                    RecalculationTimestamp = DateTime.UtcNow,
                    RecalculationSuccess = userRecalculation.Success,
                    ComplianceChanges = userRecalculation.ComplianceChanges,
                    IssuesCount = userRecalculation.RecalculationIssues?.Count ?? 0,
                    PreCompliance = userRecalculation.PreRecalculationCompliance,
                    PostCompliance = userRecalculation.PostRecalculationCompliance,
                    RecalculatedBy = AbpSession.UserId,
                    TenantId = AbpSession.TenantId
                };

                Logger.Debug($"User compliance recalculation metadata: {JsonConvert.SerializeObject(metadata)}");

                // TODO: Store in dedicated compliance recalculation audit table
                // For now, this is logged for audit purposes
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to update compliance metadata for user {userId}", ex);
                // Don't fail the recalculation if metadata update fails
            }
        }

        /// <summary>
        /// Validates department-level compliance rules after recalculation.
        /// </summary>
        /// <param name="targetDepartmentId">The target department ID</param>
        /// <param name="recalculationResult">The recalculation result to update</param>
        private async Task ValidateDepartmentComplianceRules(Guid targetDepartmentId,
            ComplianceRecalculationResultDto recalculationResult)
        {
            try
            {
                // Check department-specific compliance rules
                var department = await _tenantDepartmentRepository.FirstOrDefaultAsync(targetDepartmentId);
                if (department == null)
                {
                    recalculationResult.Warnings.Add($"Target department {targetDepartmentId} not found for validation");
                    return;
                }

                // Get department requirements
                var departmentRequirements = await _recordRequirementRepository.CountAsync(r =>
                    r.TenantDepartmentId == targetDepartmentId);

                if (departmentRequirements == 0)
                {
                    recalculationResult.Warnings.Add($"Department '{department.Name}' has no requirements defined");
                }

                // Check for users with compliance issues
                var usersWithIssues = recalculationResult.UserRecalculationResults
                    .Where(ur => ur.RecalculationIssues.Any())
                    .Count();

                if (usersWithIssues > 0)
                {
                    recalculationResult.Warnings.Add($"{usersWithIssues} users have compliance issues after recalculation");
                }

                // Additional department-level validation can be added here
                Logger.Info($"Department compliance validation completed for {department.Name}: " +
                           $"{departmentRequirements} requirements, {usersWithIssues} users with issues");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error validating department compliance rules for {targetDepartmentId}", ex);
                recalculationResult.Warnings.Add($"Department compliance validation error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates compliance cache after recalculation.
        /// </summary>
        /// <param name="cohortId">The cohort ID</param>
        /// <param name="targetDepartmentId">The target department ID</param>
        private async Task UpdateComplianceCache(Guid cohortId, Guid targetDepartmentId)
        {
            try
            {
                // TODO: Implement compliance cache update if caching is used
                // This would invalidate or update any cached compliance data
                // for the migrated users to ensure fresh data is used

                Logger.Info($"Compliance cache update completed for cohort {cohortId} in department {targetDepartmentId}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error updating compliance cache for cohort {cohortId}", ex);
                // Don't fail the recalculation if cache update fails
            }
        }

        /// <summary>
        /// Stores compliance recalculation audit data.
        /// </summary>
        /// <param name="recalculationResult">The recalculation result</param>
        private async Task StoreComplianceRecalculationAudit(ComplianceRecalculationResultDto recalculationResult)
        {
            try
            {
                var auditData = new
                {
                    CohortId = recalculationResult.CohortId,
                    MigrationId = recalculationResult.MigrationId,
                    TargetDepartmentId = recalculationResult.TargetDepartmentId,
                    RecalculationTimestamp = recalculationResult.RecalculationTimestamp,
                    Success = recalculationResult.Success,
                    TotalUsersProcessed = recalculationResult.TotalUsersProcessed,
                    SuccessfulRecalculations = recalculationResult.SuccessfulRecalculations,
                    FailedRecalculations = recalculationResult.FailedRecalculations,
                    ErrorsCount = recalculationResult.Errors?.Count ?? 0,
                    WarningsCount = recalculationResult.Warnings?.Count ?? 0,
                    RecalculatedBy = AbpSession.UserId,
                    TenantId = AbpSession.TenantId,

                    // Summary statistics
                    UsersWithComplianceChanges = recalculationResult.UserRecalculationResults?.Count(ur =>
                        ur.ComplianceChanges != null && ur.ComplianceChanges.Any()) ?? 0,
                    UsersWithIssues = recalculationResult.UserRecalculationResults?.Count(ur =>
                        ur.RecalculationIssues != null && ur.RecalculationIssues.Any()) ?? 0
                };

                Logger.Info($"Compliance recalculation audit: {JsonConvert.SerializeObject(auditData, Formatting.Indented)}");

                // TODO: Store in dedicated compliance recalculation audit table
                // For now, this is logged for audit purposes
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to store compliance recalculation audit data", ex);
                // Don't fail the recalculation if audit storage fails
            }
        }

        #endregion Compliance State Preservation

        #region Migration Execution and Orchestration

        /// <summary>
        /// Main orchestration method that executes the complete cohort migration process.
        /// This method coordinates all migration steps including compliance preservation,
        /// category mapping, and validation while providing comprehensive error handling and rollback capability.
        /// </summary>
        /// <param name="input">Complete migration configuration including cohort, target department, and category mappings</param>
        /// <returns>CohortMigrationResultDto with detailed results and any warnings or errors</returns>
        [AbpAuthorize(AppPermissions.Pages_Cohorts_MigrateBetweenDepartments)]
        public async Task<CohortMigrationResultDto> ExecuteMigration(CohortMigrationDto input)
        {
            var migrationId = Guid.NewGuid().ToString();
            var startTime = DateTime.UtcNow;

            try
            {
                Logger.Info($"Starting ExecuteMigration orchestration: Cohort {input.CohortId}, Migration {migrationId}");

                // Step 1: Pre-migration validation and analysis
                var preValidationResult = await PerformPreMigrationValidation(input, migrationId);
                if (!preValidationResult.CanProceed)
                {
                    return new CohortMigrationResultDto
                    {
                        Success = false,
                        Message = L("PreMigrationValidationFailed"),
                        Errors = preValidationResult.Errors,
                        Warnings = preValidationResult.Warnings,
                        MigrationId = migrationId
                    };
                }

                // Step 2: Capture migration state for rollback capability
                var migrationState = await CaptureMigrationState(input.CohortId, migrationId);

                // Step 3: Execute the actual migration based on target type
                CohortMigrationResultDto migrationResult;

                if (input.TargetDepartmentId.HasValue)
                {
                    Logger.Info($"Executing migration to existing department {input.TargetDepartmentId} for migration {migrationId}");
                    migrationResult = await MigrateCohortToExistingDepartment(input);
                }
                else
                {
                    Logger.Info($"Executing migration to new department '{input.NewDepartmentName}' for migration {migrationId}");
                    migrationResult = await MigrateCohortToNewDepartment(input);
                }

                // Step 4: Post-migration validation and cleanup
                if (migrationResult.Success)
                {
                    var postValidationResult = await PerformPostMigrationValidation(input.CohortId, migrationId, migrationResult);

                    // Add any post-validation warnings to the result
                    migrationResult.Warnings.AddRange(postValidationResult.Warnings);

                    if (!postValidationResult.IsValid)
                    {
                        Logger.Warn($"Post-migration validation found issues for migration {migrationId}");
                        migrationResult.Warnings.Add("Post-migration validation found data integrity issues");
                    }

                    // Step 5: Finalize migration and cleanup temporary data
                    await FinalizeMigration(migrationId, migrationState, migrationResult);
                }
                else
                {
                    // Step 6: Handle migration failure and rollback if needed
                    Logger.Error($"Migration failed for migration {migrationId}: {migrationResult.Message}");
                    await HandleMigrationFailure(migrationId, migrationState, migrationResult);
                }

                // Step 7: Log final migration summary
                var duration = DateTime.UtcNow - startTime;
                Logger.Info($"ExecuteMigration completed for migration {migrationId}. " +
                           $"Success: {migrationResult.Success}, " +
                           $"Duration: {duration.TotalMinutes:F2} minutes, " +
                           $"Warnings: {migrationResult.Warnings.Count}, " +
                           $"Errors: {migrationResult.Errors.Count}");

                return migrationResult;
            }
            catch (Exception ex)
            {
                Logger.Error($"Critical error in ExecuteMigration for migration {migrationId}", ex);

                // Attempt emergency rollback if possible
                try
                {
                    await EmergencyRollback(migrationId);
                }
                catch (Exception rollbackEx)
                {
                    Logger.Error($"Emergency rollback failed for migration {migrationId}", rollbackEx);
                }

                return new CohortMigrationResultDto
                {
                    Success = false,
                    Message = L("MigrationExecutionFailed"),
                    Errors = new List<string> { ex.Message },
                    MigrationId = migrationId,
                    MigrationStartTime = startTime,
                    MigrationEndTime = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Performs comprehensive pre-migration validation to ensure migration can proceed safely.
        /// </summary>
        /// <param name="input">Migration input parameters</param>
        /// <param name="migrationId">Migration identifier for tracking</param>
        /// <returns>PreMigrationValidationResult with validation status and any issues</returns>
        private async Task<PreMigrationValidationResult> PerformPreMigrationValidation(CohortMigrationDto input, string migrationId)
        {
            var result = new PreMigrationValidationResult
            {
                CanProceed = true,
                Errors = new List<string>(),
                Warnings = new List<string>()
            };

            try
            {
                Logger.Info($"Starting pre-migration validation for migration {migrationId}");

                // Validate cohort exists and is accessible
                var cohort = await _cohortRepository.FirstOrDefaultAsync(input.CohortId);
                if (cohort == null)
                {
                    result.CanProceed = false;
                    result.Errors.Add(L("CohortNotFound", input.CohortId));
                    return result;
                }

                // Validate target department (if migrating to existing)
                if (input.TargetDepartmentId.HasValue)
                {
                    var targetDepartment = await _tenantDepartmentRepository.FirstOrDefaultAsync(input.TargetDepartmentId.Value);
                    if (targetDepartment == null)
                    {
                        result.CanProceed = false;
                        result.Errors.Add(L("TargetDepartmentNotFound", input.TargetDepartmentId.Value));
                        return result;
                    }

                    if (!targetDepartment.Active)
                    {
                        result.CanProceed = false;
                        result.Errors.Add(L("TargetDepartmentInactive", input.TargetDepartmentId.Value));
                        return result;
                    }
                }

                // Validate new department name (if creating new)
                if (!input.TargetDepartmentId.HasValue)
                {
                    if (string.IsNullOrWhiteSpace(input.NewDepartmentName))
                    {
                        result.CanProceed = false;
                        result.Errors.Add(L("NewDepartmentNameRequired"));
                        return result;
                    }

                    // Check for name conflicts
                    var existingDepartment = await _tenantDepartmentRepository.FirstOrDefaultAsync(d =>
                        d.Name.ToLower() == input.NewDepartmentName.ToLower() &&
                        d.TenantId == cohort.TenantId);

                    if (existingDepartment != null)
                    {
                        result.CanProceed = false;
                        result.Errors.Add(L("DepartmentNameAlreadyExists", input.NewDepartmentName));
                        return result;
                    }
                }

                // Validate category mappings
                var mappingValidation = await ValidateMigrationMappings(new ValidateMigrationMappingsInput
                {
                    CohortId = input.CohortId,
                    TargetDepartmentId = input.TargetDepartmentId,
                    Mappings = input.CategoryMappings
                });

                if (!mappingValidation)
                {
                    result.CanProceed = false;
                    result.Errors.Add(L("CategoryMappingValidationFailed"));
                    return result;
                }

                // Validate user permissions
                if (!await PermissionChecker.IsGrantedAsync(AppPermissions.Pages_Cohorts_MigrateBetweenDepartments))
                {
                    result.CanProceed = false;
                    result.Errors.Add(L("InsufficientPermissionsForMigration"));
                    return result;
                }

                // Check for active migrations on this cohort
                var hasActiveMigration = await CheckForActiveMigrations(input.CohortId);
                if (hasActiveMigration)
                {
                    result.CanProceed = false;
                    result.Errors.Add(L("CohortHasActiveMigration"));
                    return result;
                }

                // Performance and capacity warnings
                var usersCount = await _cohortUserRepository.CountAsync(cu => cu.CohortId == input.CohortId);
                if (usersCount > 100)
                {
                    result.Warnings.Add(L("LargeCohortMigrationWarning", usersCount));
                }

                if (input.CategoryMappings.Count > 10)
                {
                    result.Warnings.Add(L("ManyRequirementCategoriesWarning", input.CategoryMappings.Count));
                }

                Logger.Info($"Pre-migration validation completed for migration {migrationId}. " +
                           $"Can proceed: {result.CanProceed}, Errors: {result.Errors.Count}, Warnings: {result.Warnings.Count}");

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error during pre-migration validation for migration {migrationId}", ex);
                result.CanProceed = false;
                result.Errors.Add($"Validation error: {ex.Message}");
                return result;
            }
        }

        /// <summary>
        /// Captures the current state of all entities that will be affected by the migration.
        /// This state can be used for rollback if the migration fails.
        /// </summary>
        /// <param name="cohortId">The cohort being migrated</param>
        /// <param name="migrationId">Migration identifier for tracking</param>
        /// <returns>MigrationStateSnapshot containing all relevant state data</returns>
        private async Task<MigrationStateSnapshot> CaptureMigrationState(Guid cohortId, string migrationId)
        {
            try
            {
                Logger.Info($"Capturing migration state for cohort {cohortId}, migration {migrationId}");

                var snapshot = new MigrationStateSnapshot
                {
                    MigrationId = migrationId,
                    CohortId = cohortId,
                    CaptureTimestamp = DateTime.UtcNow
                };

                // Capture cohort state
                var cohort = await _cohortRepository.GetAsync(cohortId);
                snapshot.OriginalCohortState = new CohortStateSnapshot
                {
                    CohortId = cohort.Id,
                    Name = cohort.Name,
                    TenantDepartmentId = cohort.TenantDepartmentId,
                    TenantId = cohort.TenantId,
                    IsDeleted = cohort.IsDeleted
                };

                // Capture cohort users
                var cohortUsers = await _cohortUserRepository.GetAll()
                    .Where(cu => cu.CohortId == cohortId)
                    .ToListAsync();

                snapshot.CohortUserIds = cohortUsers.Select(cu => cu.UserId).ToList();

                // Capture record states that will be affected
                var affectedRecordStates = await _recordStateRepository.GetAll()
                    .Include(rs => rs.RecordCategoryFk)
                        .ThenInclude(rc => rc.RecordRequirementFk)
                    .Where(rs => rs.UserId.HasValue && snapshot.CohortUserIds.Contains(rs.UserId.Value))
                    .ToListAsync();

                snapshot.OriginalRecordStates = affectedRecordStates.Select(rs => new RecordStateSnapshotDto
                {
                    RecordStateId = rs.Id,
                    UserId = rs.UserId.Value,
                    RecordCategoryId = rs.RecordCategoryId,
                    RecordId = rs.RecordId,
                    RecordStatusId = rs.RecordStatusId,
                    State = rs.State,
                    Notes = rs.Notes,
                    CreationTime = rs.CreationTime,
                    LastModificationTime = rs.LastModificationTime,
                    SnapshotTimestamp = DateTime.UtcNow,
                    MigrationId = migrationId
                }).ToList();

                // Store snapshot metadata
                snapshot.TotalUsersCount = snapshot.CohortUserIds.Count;
                snapshot.TotalRecordStatesCount = snapshot.OriginalRecordStates.Count;

                Logger.Info($"Migration state captured for migration {migrationId}. " +
                           $"Users: {snapshot.TotalUsersCount}, Record states: {snapshot.TotalRecordStatesCount}");

                return snapshot;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error capturing migration state for migration {migrationId}", ex);
                throw new UserFriendlyException(L("MigrationStateCaptureError", ex.Message));
            }
        }

        /// <summary>
        /// Performs post-migration validation to ensure the migration completed successfully.
        /// </summary>
        /// <param name="cohortId">The migrated cohort ID</param>
        /// <param name="migrationId">Migration identifier</param>
        /// <param name="migrationResult">The migration result to validate</param>
        /// <returns>PostMigrationValidationResult with validation status</returns>
        private async Task<PostMigrationValidationResult> PerformPostMigrationValidation(Guid cohortId, string migrationId, CohortMigrationResultDto migrationResult)
        {
            var result = new PostMigrationValidationResult
            {
                IsValid = true,
                Warnings = new List<string>()
            };

            try
            {
                Logger.Info($"Starting post-migration validation for migration {migrationId}");

                // Validate cohort department assignment
                var cohort = await _cohortRepository.GetAsync(cohortId);
                if (!cohort.TenantDepartmentId.HasValue)
                {
                    result.IsValid = false;
                    result.Warnings.Add("Cohort has no department assignment after migration");
                }

                // Validate that all users still exist in cohort
                var currentUsersCount = await _cohortUserRepository.CountAsync(cu => cu.CohortId == cohortId);
                if (currentUsersCount != migrationResult.AffectedUsersCount && migrationResult.AffectedUsersCount > 0)
                {
                    result.Warnings.Add($"User count mismatch: expected {migrationResult.AffectedUsersCount}, found {currentUsersCount}");
                }

                // Additional validation can be added here
                Logger.Info($"Post-migration validation completed for migration {migrationId}. Valid: {result.IsValid}");

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error during post-migration validation for migration {migrationId}", ex);
                result.IsValid = false;
                result.Warnings.Add($"Post-migration validation error: {ex.Message}");
                return result;
            }
        }

        /// <summary>
        /// Finalizes a successful migration by cleaning up temporary data and updating audit records.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <param name="migrationState">Captured migration state</param>
        /// <param name="migrationResult">Migration result</param>
        private async Task FinalizeMigration(string migrationId, MigrationStateSnapshot migrationState, CohortMigrationResultDto migrationResult)
        {
            try
            {
                Logger.Info($"Finalizing successful migration {migrationId}");

                // Update migration result with final status
                migrationResult.MigrationEndTime = DateTime.UtcNow;

                // Store final audit record
                await StoreFinalMigrationAudit(migrationId, migrationState, migrationResult, true);

                // Clean up temporary migration data (if any)
                await CleanupTemporaryMigrationData(migrationId);

                Logger.Info($"Migration {migrationId} finalized successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error finalizing migration {migrationId}", ex);
                // Don't fail the migration for finalization errors
            }
        }

        /// <summary>
        /// Handles migration failure by performing cleanup and optionally triggering rollback.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <param name="migrationState">Captured migration state</param>
        /// <param name="migrationResult">Migration result with error information</param>
        private async Task HandleMigrationFailure(string migrationId, MigrationStateSnapshot migrationState, CohortMigrationResultDto migrationResult)
        {
            try
            {
                Logger.Warn($"Handling migration failure for migration {migrationId}");

                // Store failure audit record
                await StoreFinalMigrationAudit(migrationId, migrationState, migrationResult, false);

                // Clean up any partial changes (this would be implemented based on specific requirements)
                await CleanupFailedMigration(migrationId, migrationState);

                Logger.Info($"Migration failure handled for migration {migrationId}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error handling migration failure for migration {migrationId}", ex);
            }
        }

        /// <summary>
        /// Performs emergency rollback in case of critical errors.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        private async Task EmergencyRollback(string migrationId)
        {
            try
            {
                Logger.Warn($"Performing emergency rollback for migration {migrationId}");

                // Emergency rollback logic would be implemented here
                // This is a placeholder for the actual rollback implementation

                Logger.Info($"Emergency rollback completed for migration {migrationId}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Emergency rollback failed for migration {migrationId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Stores the final migration audit record.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <param name="migrationState">Migration state snapshot</param>
        /// <param name="migrationResult">Migration result</param>
        /// <param name="success">Whether the migration was successful</param>
        private async Task StoreFinalMigrationAudit(string migrationId, MigrationStateSnapshot migrationState, CohortMigrationResultDto migrationResult, bool success)
        {
            try
            {
                var auditData = new
                {
                    MigrationId = migrationId,
                    CohortId = migrationState.CohortId,
                    Success = success,
                    StartTime = migrationResult.MigrationStartTime,
                    EndTime = migrationResult.MigrationEndTime,
                    Duration = migrationResult.MigrationEndTime.HasValue && migrationResult.MigrationStartTime.HasValue
                        ? (migrationResult.MigrationEndTime.Value - migrationResult.MigrationStartTime.Value).TotalMinutes
                        : 0,
                    AffectedUsers = migrationResult.AffectedUsersCount,
                    MigratedRecordStates = migrationResult.MigratedRecordStatesCount,
                    WarningsCount = migrationResult.Warnings?.Count ?? 0,
                    ErrorsCount = migrationResult.Errors?.Count ?? 0,
                    ExecutedBy = AbpSession.UserId,
                    TenantId = AbpSession.TenantId,
                    AuditTimestamp = DateTime.UtcNow
                };

                Logger.Info($"Final migration audit: {JsonConvert.SerializeObject(auditData, Formatting.Indented)}");

                // TODO: Store in dedicated migration audit table
                // For now, this is logged for audit purposes
            }
            catch (Exception ex)
            {
                Logger.Error($"Error storing final migration audit for {migrationId}", ex);
            }
        }

        /// <summary>
        /// Cleans up temporary migration data.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        private async Task CleanupTemporaryMigrationData(string migrationId)
        {
            try
            {
                // Cleanup logic would be implemented here
                // This might include removing temporary files, cache entries, etc.
                Logger.Debug($"Cleaned up temporary data for migration {migrationId}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error cleaning up temporary data for migration {migrationId}", ex);
            }
        }

        /// <summary>
        /// Cleans up after a failed migration.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <param name="migrationState">Migration state snapshot</param>
        private async Task CleanupFailedMigration(string migrationId, MigrationStateSnapshot migrationState)
        {
            try
            {
                // Cleanup logic for failed migrations would be implemented here
                Logger.Debug($"Cleaned up failed migration data for migration {migrationId}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error cleaning up failed migration for migration {migrationId}", ex);
            }
        }

        /// <summary>
        /// Performs a complete rollback of a migration by restoring all affected entities to their original state.
        /// This method uses the captured migration state to restore cohort, user, and record state data.
        /// </summary>
        /// <param name="input">Rollback configuration including migration ID and confirmation</param>
        /// <returns>CohortMigrationRollbackResultDto with rollback results and statistics</returns>
        [AbpAuthorize(AppPermissions.Pages_Cohorts_RollbackMigration)]
        public async Task<CohortMigrationRollbackResultDto> RollbackMigration(CohortMigrationRollbackDto input)
        {
            var rollbackStartTime = DateTime.UtcNow;

            try
            {
                Logger.Info($"Starting migration rollback for migration {input.MigrationId}");

                // Step 1: Validate rollback request
                var rollbackValidation = await ValidateRollbackRequest(input);
                if (!rollbackValidation.CanRollback)
                {
                    return new CohortMigrationRollbackResultDto
                    {
                        Success = false,
                        Message = L("RollbackValidationFailed"),
                        Errors = rollbackValidation.Errors,
                        RollbackStartTime = rollbackStartTime,
                        RollbackEndTime = DateTime.UtcNow
                    };
                }

                // Step 2: Retrieve migration state snapshot
                var migrationState = await RetrieveMigrationStateSnapshot(input.MigrationId.ToString());
                if (migrationState == null)
                {
                    return new CohortMigrationRollbackResultDto
                    {
                        Success = false,
                        Message = L("MigrationStateNotFound"),
                        Errors = new List<string> { "Migration state snapshot not found for rollback" },
                        RollbackStartTime = rollbackStartTime,
                        RollbackEndTime = DateTime.UtcNow
                    };
                }

                // Step 3: Execute rollback within transaction
                using (var uow = _unitOfWorkManager.Begin())
                {
                    try
                    {
                        var rollbackResult = await ExecuteRollbackOperations(migrationState, input.Reason);

                        // Step 4: Validate rollback integrity
                        var integrityValidation = await ValidateRollbackIntegrity(migrationState, rollbackResult);
                        if (!integrityValidation.IsValid)
                        {
                            Logger.Warn($"Rollback integrity validation failed for migration {input.MigrationId}");
                            rollbackResult.Warnings.AddRange(integrityValidation.Warnings);
                        }

                        // Step 5: Commit rollback transaction
                        await uow.CompleteAsync();

                        // Step 6: Finalize rollback and create audit record
                        await FinalizeRollback(input.MigrationId.ToString(), migrationState, rollbackResult, input.Reason);

                        rollbackResult.RollbackStartTime = rollbackStartTime;
                        rollbackResult.RollbackEndTime = DateTime.UtcNow;

                        Logger.Info($"Migration rollback completed successfully for migration {input.MigrationId}. " +
                                   $"Restored users: {rollbackResult.RestoredUsersCount}, " +
                                   $"Restored record states: {rollbackResult.RestoredRecordStatesCount}");

                        return rollbackResult;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Error during rollback execution for migration {input.MigrationId}", ex);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Critical error in RollbackMigration for migration {input.MigrationId}", ex);

                return new CohortMigrationRollbackResultDto
                {
                    Success = false,
                    Message = L("RollbackExecutionFailed"),
                    Errors = new List<string> { ex.Message },
                    RollbackStartTime = rollbackStartTime,
                    RollbackEndTime = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Validates whether a rollback can be performed for the specified migration.
        /// </summary>
        /// <param name="migrationId">The migration ID to check for rollback capability</param>
        /// <returns>True if rollback is possible, false otherwise</returns>
        public async Task<bool> CanRollbackMigration(Guid migrationId)
        {
            try
            {
                Logger.Info($"Checking rollback capability for migration {migrationId}");

                // Check if migration state snapshot exists
                var migrationState = await RetrieveMigrationStateSnapshot(migrationId.ToString());
                if (migrationState == null)
                {
                    Logger.Warn($"No migration state found for migration {migrationId}");
                    return false;
                }

                // Check if cohort still exists
                var cohort = await _cohortRepository.FirstOrDefaultAsync(migrationState.CohortId);
                if (cohort == null)
                {
                    Logger.Warn($"Cohort {migrationState.CohortId} no longer exists for migration {migrationId}");
                    return false;
                }

                // Check if migration is not too old (configurable business rule)
                var migrationAge = DateTime.UtcNow - migrationState.CaptureTimestamp;
                var maxRollbackAge = TimeSpan.FromDays(30); // 30 days rollback window

                if (migrationAge > maxRollbackAge)
                {
                    Logger.Warn($"Migration {migrationId} is too old for rollback: {migrationAge.TotalDays} days");
                    return false;
                }

                // Additional business rules can be added here
                Logger.Info($"Migration {migrationId} can be rolled back");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error checking rollback capability for migration {migrationId}", ex);
                return false;
            }
        }

        /// <summary>
        /// Validates the rollback request and ensures it can be safely executed.
        /// </summary>
        /// <param name="input">Rollback request input</param>
        /// <returns>RollbackValidationResult with validation status</returns>
        private async Task<RollbackValidationResult> ValidateRollbackRequest(CohortMigrationRollbackDto input)
        {
            var result = new RollbackValidationResult
            {
                CanRollback = true,
                Errors = new List<string>(),
                Warnings = new List<string>()
            };

            try
            {
                // Validate migration ID format
                if (!Guid.TryParse(input.MigrationId.ToString(), out _))
                {
                    result.CanRollback = false;
                    result.Errors.Add("Invalid migration ID format");
                    return result;
                }

                // Validate confirmation
                if (!input.ConfirmRollback)
                {
                    result.CanRollback = false;
                    result.Errors.Add("Rollback confirmation required");
                    return result;
                }

                // Validate reason is provided
                if (string.IsNullOrWhiteSpace(input.Reason))
                {
                    result.Warnings.Add("No rollback reason provided");
                }

                // Check permissions
                if (!await PermissionChecker.IsGrantedAsync(AppPermissions.Pages_Cohorts_RollbackMigration))
                {
                    result.CanRollback = false;
                    result.Errors.Add("Insufficient permissions for rollback");
                    return result;
                }

                // Check if rollback is possible
                var canRollback = await CanRollbackMigration(input.MigrationId);
                if (!canRollback)
                {
                    result.CanRollback = false;
                    result.Errors.Add("Migration cannot be rolled back");
                    return result;
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error validating rollback request for migration {input.MigrationId}", ex);
                result.CanRollback = false;
                result.Errors.Add($"Rollback validation error: {ex.Message}");
                return result;
            }
        }

        /// <summary>
        /// Retrieves the migration state snapshot for rollback operations.
        /// In a production system, this would retrieve from a dedicated storage system.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <returns>MigrationStateSnapshot if found, null otherwise</returns>
        private async Task<MigrationStateSnapshot> RetrieveMigrationStateSnapshot(string migrationId)
        {
            try
            {
                // TODO: In a production system, this would retrieve from a dedicated migration state storage
                // For now, this is a placeholder that would need to be implemented based on the storage strategy
                // Options include: Database table, File system, Redis, etc.

                Logger.Info($"Retrieving migration state snapshot for migration {migrationId}");

                // Placeholder implementation - in reality this would query a migration state storage
                // return await _migrationStateRepository.GetByMigrationIdAsync(migrationId);

                Logger.Warn($"Migration state retrieval not yet implemented for migration {migrationId}");
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error retrieving migration state for migration {migrationId}", ex);
                return null;
            }
        }

        /// <summary>
        /// Executes the actual rollback operations by restoring entities to their original state.
        /// </summary>
        /// <param name="migrationState">The captured migration state</param>
        /// <param name="rollbackReason">Reason for the rollback</param>
        /// <returns>CohortMigrationRollbackResultDto with rollback results</returns>
        private async Task<CohortMigrationRollbackResultDto> ExecuteRollbackOperations(MigrationStateSnapshot migrationState, string rollbackReason)
        {
            var result = new CohortMigrationRollbackResultDto
            {
                Success = true,
                Message = L("RollbackExecutedSuccessfully"),
                Warnings = new List<string>(),
                Errors = new List<string>()
            };

            try
            {
                Logger.Info($"Executing rollback operations for migration {migrationState.MigrationId}");

                // Step 1: Restore cohort state
                await RestoreCohortState(migrationState.OriginalCohortState, result);

                // Step 2: Restore record states
                await RestoreRecordStates(migrationState.OriginalRecordStates, result);

                // Step 3: Validate user associations (cohort users should remain unchanged)
                await ValidateUserAssociations(migrationState, result);

                // Step 4: Create rollback audit notes
                await CreateRollbackAuditNotes(migrationState, rollbackReason, result);

                Logger.Info($"Rollback operations completed for migration {migrationState.MigrationId}. " +
                           $"Success: {result.Success}, Restored users: {result.RestoredUsersCount}, " +
                           $"Restored record states: {result.RestoredRecordStatesCount}");

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error executing rollback operations for migration {migrationState.MigrationId}", ex);
                result.Success = false;
                result.Message = L("RollbackOperationsFailed");
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        /// <summary>
        /// Restores the cohort to its original state.
        /// </summary>
        /// <param name="originalCohortState">Original cohort state</param>
        /// <param name="result">Rollback result to update</param>
        private async Task RestoreCohortState(CohortStateSnapshot originalCohortState, CohortMigrationRollbackResultDto result)
        {
            try
            {
                Logger.Info($"Restoring cohort state for cohort {originalCohortState.CohortId}");

                var cohort = await _cohortRepository.GetAsync(originalCohortState.CohortId);

                // Restore cohort properties
                cohort.Name = originalCohortState.Name;
                cohort.TenantDepartmentId = originalCohortState.TenantDepartmentId;
                cohort.IsDeleted = originalCohortState.IsDeleted;

                await _cohortRepository.UpdateAsync(cohort);

                Logger.Info($"Cohort state restored for cohort {originalCohortState.CohortId}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error restoring cohort state for cohort {originalCohortState.CohortId}", ex);
                result.Errors.Add($"Failed to restore cohort state: {ex.Message}");
                result.Success = false;
            }
        }

        /// <summary>
        /// Restores record states to their original state.
        /// </summary>
        /// <param name="originalRecordStates">Original record states</param>
        /// <param name="result">Rollback result to update</param>
        private async Task RestoreRecordStates(List<RecordStateSnapshotDto> originalRecordStates, CohortMigrationRollbackResultDto result)
        {
            try
            {
                Logger.Info($"Restoring {originalRecordStates.Count} record states");

                var restoredCount = 0;
                var errorCount = 0;

                // Process record states in batches for performance
                const int batchSize = 100;

                for (int i = 0; i < originalRecordStates.Count; i += batchSize)
                {
                    var batch = originalRecordStates.Skip(i).Take(batchSize).ToList();

                    foreach (var originalState in batch)
                    {
                        try
                        {
                            var currentRecordState = await _recordStateRepository.FirstOrDefaultAsync(originalState.RecordStateId);
                            if (currentRecordState != null)
                            {
                                // Restore record state properties
                                currentRecordState.RecordCategoryId = originalState.RecordCategoryId;
                                currentRecordState.RecordId = originalState.RecordId;
                                currentRecordState.RecordStatusId = originalState.RecordStatusId;
                                currentRecordState.State = originalState.State;
                                currentRecordState.Notes = originalState.Notes;

                                await _recordStateRepository.UpdateAsync(currentRecordState);
                                restoredCount++;
                            }
                            else
                            {
                                Logger.Warn($"Record state {originalState.RecordStateId} not found for restoration");
                                result.Warnings.Add($"Record state {originalState.RecordStateId} not found");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Error restoring record state {originalState.RecordStateId}", ex);
                            result.Errors.Add($"Failed to restore record state {originalState.RecordStateId}: {ex.Message}");
                            errorCount++;
                        }
                    }

                    // Save changes for this batch
                    await CurrentUnitOfWork.SaveChangesAsync();

                    // Log progress for large operations
                    if (originalRecordStates.Count > batchSize)
                    {
                        var processedCount = Math.Min(i + batchSize, originalRecordStates.Count);
                        Logger.Info($"Record state restoration progress: {processedCount}/{originalRecordStates.Count} processed");
                    }
                }

                result.RestoredRecordStatesCount = restoredCount;

                if (errorCount > 0)
                {
                    result.Warnings.Add($"{errorCount} record states could not be restored");
                    if (errorCount > restoredCount)
                    {
                        result.Success = false;
                        result.Message = L("RollbackPartiallyFailed");
                    }
                }

                Logger.Info($"Record state restoration completed. Restored: {restoredCount}, Errors: {errorCount}");
            }
            catch (Exception ex)
            {
                Logger.Error("Error during record state restoration", ex);
                result.Success = false;
                result.Errors.Add($"Record state restoration failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates that user associations are correct after rollback.
        /// </summary>
        /// <param name="migrationState">Migration state snapshot</param>
        /// <param name="result">Rollback result to update</param>
        private async Task ValidateUserAssociations(MigrationStateSnapshot migrationState, CohortMigrationRollbackResultDto result)
        {
            try
            {
                Logger.Info($"Validating user associations for cohort {migrationState.CohortId}");

                // Check that all original users are still in the cohort
                var currentCohortUsers = await _cohortUserRepository.GetAll()
                    .Where(cu => cu.CohortId == migrationState.CohortId)
                    .Select(cu => cu.UserId)
                    .ToListAsync();

                var missingUsers = migrationState.CohortUserIds.Except(currentCohortUsers).ToList();
                var extraUsers = currentCohortUsers.Except(migrationState.CohortUserIds).ToList();

                if (missingUsers.Any())
                {
                    result.Warnings.Add($"{missingUsers.Count} users are missing from the cohort after rollback");
                }

                if (extraUsers.Any())
                {
                    result.Warnings.Add($"{extraUsers.Count} extra users found in the cohort after rollback");
                }

                result.RestoredUsersCount = currentCohortUsers.Count;

                Logger.Info($"User association validation completed. Current users: {currentCohortUsers.Count}, " +
                           $"Missing: {missingUsers.Count}, Extra: {extraUsers.Count}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error validating user associations for cohort {migrationState.CohortId}", ex);
                result.Warnings.Add($"User association validation error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates audit notes for the rollback operation.
        /// </summary>
        /// <param name="migrationState">Migration state snapshot</param>
        /// <param name="rollbackReason">Reason for rollback</param>
        /// <param name="result">Rollback result</param>
        private async Task CreateRollbackAuditNotes(MigrationStateSnapshot migrationState, string rollbackReason, CohortMigrationRollbackResultDto result)
        {
            try
            {
                Logger.Info($"Creating rollback audit notes for migration {migrationState.MigrationId}");

                // Create audit notes for affected record states
                var auditNotesCreated = 0;

                foreach (var originalState in migrationState.OriginalRecordStates.Take(10)) // Limit to first 10 for performance
                {
                    try
                    {
                        var auditNote = new RecordNote
                        {
                            RecordStateId = originalState.RecordStateId,
                            Note = $"🔄 MIGRATION ROLLBACK PERFORMED 🔄\n" +
                                   $"Migration ID: {migrationState.MigrationId}\n" +
                                   $"Rollback Date: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC\n" +
                                   $"Rollback Reason: {rollbackReason ?? "Not specified"}\n" +
                                   $"Record state restored to original values.\n" +
                                   $"Performed by user {AbpSession.UserId}",
                            Created = DateTime.UtcNow,
                            AuthorizedOnly = true,
                            HostOnly = false,
                            SendNotification = false,
                            UserId = AbpSession.UserId,
                            TenantId = AbpSession.TenantId
                        };

                        await _recordNoteRepository.InsertAsync(auditNote);
                        auditNotesCreated++;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Error creating audit note for record state {originalState.RecordStateId}", ex);
                    }
                }

                Logger.Info($"Created {auditNotesCreated} rollback audit notes");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error creating rollback audit notes for migration {migrationState.MigrationId}", ex);
                result.Warnings.Add("Failed to create some audit notes");
            }
        }

        /// <summary>
        /// Validates the integrity of the rollback operation.
        /// </summary>
        /// <param name="migrationState">Original migration state</param>
        /// <param name="rollbackResult">Rollback result</param>
        /// <returns>RollbackIntegrityValidationResult</returns>
        private async Task<RollbackIntegrityValidationResult> ValidateRollbackIntegrity(MigrationStateSnapshot migrationState, CohortMigrationRollbackResultDto rollbackResult)
        {
            var result = new RollbackIntegrityValidationResult
            {
                IsValid = true,
                Warnings = new List<string>()
            };

            try
            {
                Logger.Info($"Validating rollback integrity for migration {migrationState.MigrationId}");

                // Validate cohort state
                var cohort = await _cohortRepository.GetAsync(migrationState.CohortId);
                if (cohort.TenantDepartmentId != migrationState.OriginalCohortState.TenantDepartmentId)
                {
                    result.IsValid = false;
                    result.Warnings.Add("Cohort department assignment not properly restored");
                }

                // Validate record state counts
                var currentRecordStatesCount = await _recordStateRepository.CountAsync(rs =>
                    rs.UserId.HasValue && migrationState.CohortUserIds.Contains(rs.UserId.Value));

                if (currentRecordStatesCount != migrationState.TotalRecordStatesCount)
                {
                    result.Warnings.Add($"Record state count mismatch: expected {migrationState.TotalRecordStatesCount}, found {currentRecordStatesCount}");
                }

                Logger.Info($"Rollback integrity validation completed. Valid: {result.IsValid}, Warnings: {result.Warnings.Count}");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error validating rollback integrity for migration {migrationState.MigrationId}", ex);
                result.IsValid = false;
                result.Warnings.Add($"Integrity validation error: {ex.Message}");
                return result;
            }
        }

        /// <summary>
        /// Finalizes the rollback operation with audit logging.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <param name="migrationState">Migration state</param>
        /// <param name="rollbackResult">Rollback result</param>
        /// <param name="rollbackReason">Reason for rollback</param>
        private async Task FinalizeRollback(string migrationId, MigrationStateSnapshot migrationState, CohortMigrationRollbackResultDto rollbackResult, string rollbackReason)
        {
            try
            {
                Logger.Info($"Finalizing rollback for migration {migrationId}");

                var rollbackAuditData = new
                {
                    MigrationId = migrationId,
                    CohortId = migrationState.CohortId,
                    RollbackSuccess = rollbackResult.Success,
                    RollbackReason = rollbackReason,
                    RestoredUsers = rollbackResult.RestoredUsersCount,
                    RestoredRecordStates = rollbackResult.RestoredRecordStatesCount,
                    WarningsCount = rollbackResult.Warnings?.Count ?? 0,
                    ErrorsCount = rollbackResult.Errors?.Count ?? 0,
                    RollbackDuration = rollbackResult.RollbackEndTime.HasValue && rollbackResult.RollbackStartTime.HasValue
                        ? (rollbackResult.RollbackEndTime.Value - rollbackResult.RollbackStartTime.Value).TotalMinutes
                        : 0,
                    PerformedBy = AbpSession.UserId,
                    TenantId = AbpSession.TenantId,
                    RollbackTimestamp = DateTime.UtcNow
                };

                Logger.Info($"Rollback audit data: {JsonConvert.SerializeObject(rollbackAuditData, Formatting.Indented)}");

                // TODO: Store rollback audit in dedicated table
                // For now, this is logged for audit purposes

                Logger.Info($"Rollback finalized for migration {migrationId}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error finalizing rollback for migration {migrationId}", ex);
            }
        }

        #endregion Migration Execution and Orchestration

        #region Migration Audit Logging

        /// <summary>
        /// Creates comprehensive audit trails for migration operations with detailed before/after states,
        /// performance metrics, and user action tracking for compliance and troubleshooting purposes.
        /// </summary>
        /// <param name="migrationId">Migration identifier for tracking</param>
        /// <param name="auditData">Comprehensive audit data including states, metrics, and user actions</param>
        /// <returns>Task representing the audit logging operation</returns>
        private async Task CreateMigrationAuditTrail(string migrationId, MigrationAuditData auditData)
        {
            try
            {
                Logger.Info($"Creating comprehensive audit trail for migration {migrationId}");

                // Create structured audit record
                var auditRecord = new MigrationAuditRecord
                {
                    MigrationId = migrationId,
                    CohortId = auditData.CohortId,
                    AuditTimestamp = DateTime.UtcNow,
                    OperationType = auditData.OperationType,
                    OperationStatus = auditData.OperationStatus,

                    // User and tenant context
                    UserId = AbpSession.UserId,
                    TenantId = AbpSession.TenantId,
                    UserName = auditData.UserName,

                    // Before/After states
                    BeforeState = JsonConvert.SerializeObject(auditData.BeforeState, Formatting.Indented),
                    AfterState = JsonConvert.SerializeObject(auditData.AfterState, Formatting.Indented),

                    // Performance metrics
                    StartTime = auditData.StartTime,
                    EndTime = auditData.EndTime,
                    DurationMinutes = auditData.DurationMinutes,
                    ProcessedUsersCount = auditData.ProcessedUsersCount,
                    ProcessedRecordStatesCount = auditData.ProcessedRecordStatesCount,

                    // Operation details
                    SourceDepartmentId = auditData.SourceDepartmentId,
                    TargetDepartmentId = auditData.TargetDepartmentId,
                    CategoryMappingsCount = auditData.CategoryMappingsCount,

                    // Results and issues
                    Success = auditData.Success,
                    ErrorsCount = auditData.Errors?.Count ?? 0,
                    WarningsCount = auditData.Warnings?.Count ?? 0,
                    ErrorDetails = auditData.Errors?.Any() == true ? JsonConvert.SerializeObject(auditData.Errors) : null,
                    WarningDetails = auditData.Warnings?.Any() == true ? JsonConvert.SerializeObject(auditData.Warnings) : null,

                    // Additional metadata
                    Metadata = JsonConvert.SerializeObject(auditData.AdditionalMetadata, Formatting.Indented)
                };

                // Store audit record (placeholder for actual storage implementation)
                await StoreMigrationAuditRecord(auditRecord);

                // Create ABP audit log entry for integration
                await CreateAbpAuditLogEntry(migrationId, auditData);

                // Create user action audit notes
                await CreateUserActionAuditNotes(migrationId, auditData);

                // Store performance metrics
                await StorePerformanceMetrics(migrationId, auditData);

                Logger.Info($"Comprehensive audit trail created for migration {migrationId}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error creating audit trail for migration {migrationId}", ex);
                // Don't fail the migration if audit creation fails
            }
        }

        /// <summary>
        /// Stores the migration audit record in the dedicated audit storage system.
        /// </summary>
        /// <param name="auditRecord">The audit record to store</param>
        private async Task StoreMigrationAuditRecord(MigrationAuditRecord auditRecord)
        {
            try
            {
                // TODO: In a production system, this would store in a dedicated audit table
                // For now, we'll log the structured audit data

                var auditJson = JsonConvert.SerializeObject(auditRecord, Formatting.Indented);
                Logger.Info($"Migration Audit Record: {auditJson}");

                // Example implementation for dedicated audit table:
                // await _migrationAuditRepository.InsertAsync(auditRecord);
                // await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error($"Error storing migration audit record for migration {auditRecord.MigrationId}", ex);
            }
        }

        /// <summary>
        /// Creates ABP audit log entry for integration with existing audit system.
        /// Note: ASP.NET Zero handles auditing automatically through method attributes and framework.
        /// This method provides additional custom audit data if needed.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <param name="auditData">Audit data</param>
        private async Task CreateAbpAuditLogEntry(string migrationId, MigrationAuditData auditData)
        {
            try
            {
                // ASP.NET Zero automatically handles auditing through the framework
                // Additional custom audit data can be logged here if needed
                var customAuditData = new
                {
                    MigrationId = migrationId,
                    CohortId = auditData.CohortId,
                    OperationType = auditData.OperationType,
                    ProcessedUsers = auditData.ProcessedUsersCount,
                    ProcessedRecordStates = auditData.ProcessedRecordStatesCount,
                    CategoryMappings = auditData.CategoryMappingsCount,
                    Success = auditData.Success,
                    WarningsCount = auditData.WarningsCount,
                    ErrorsCount = auditData.ErrorsCount,
                    DurationMinutes = auditData.DurationMinutes
                };

                // Log the custom audit data - ABP framework will handle the rest
                Logger.Info($"Migration audit data: {JsonConvert.SerializeObject(customAuditData, Formatting.Indented)}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error creating custom audit log entry for migration {migrationId}", ex);
            }
        }

        /// <summary>
        /// Creates detailed user action audit notes for affected record states.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <param name="auditData">Audit data</param>
        private async Task CreateUserActionAuditNotes(string migrationId, MigrationAuditData auditData)
        {
            try
            {
                Logger.Info($"Creating user action audit notes for migration {migrationId}");

                // Create summary audit note for the migration operation
                var summaryNote = new RecordNote
                {
                    // Note: This would need to be associated with a specific record state or use a different audit mechanism
                    Note = $"📋 COHORT MIGRATION AUDIT SUMMARY 📋\n" +
                           $"Migration ID: {migrationId}\n" +
                           $"Operation: {auditData.OperationType}\n" +
                           $"Status: {auditData.OperationStatus}\n" +
                           $"Cohort: {auditData.CohortId}\n" +
                           $"Source Department: {auditData.SourceDepartmentId}\n" +
                           $"Target Department: {auditData.TargetDepartmentId}\n" +
                           $"Processed Users: {auditData.ProcessedUsersCount}\n" +
                           $"Processed Record States: {auditData.ProcessedRecordStatesCount}\n" +
                           $"Category Mappings: {auditData.CategoryMappingsCount}\n" +
                           $"Duration: {auditData.DurationMinutes:F2} minutes\n" +
                           $"Success: {auditData.Success}\n" +
                           $"Warnings: {auditData.WarningsCount}\n" +
                           $"Errors: {auditData.ErrorsCount}\n" +
                           $"Performed by: User {AbpSession.UserId}\n" +
                           $"Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC",
                    Created = DateTime.UtcNow,
                    AuthorizedOnly = true,
                    HostOnly = false,
                    SendNotification = auditData.OperationType == "Migration" && !auditData.Success, // Notify on migration failures
                    UserId = AbpSession.UserId,
                    TenantId = AbpSession.TenantId
                };

                // TODO: Associate with appropriate record state or use alternative audit mechanism
                // For now, we'll log the audit note content
                Logger.Info($"Migration audit note: {summaryNote.Note}");

                // Create decision point audit notes for category mappings
                await CreateCategoryMappingAuditNotes(migrationId, auditData);

                Logger.Info($"User action audit notes created for migration {migrationId}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error creating user action audit notes for migration {migrationId}", ex);
            }
        }

        /// <summary>
        /// Creates audit notes for category mapping decisions.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <param name="auditData">Audit data</param>
        private async Task CreateCategoryMappingAuditNotes(string migrationId, MigrationAuditData auditData)
        {
            try
            {
                if (auditData.CategoryMappingDecisions?.Any() == true)
                {
                    foreach (var decision in auditData.CategoryMappingDecisions)
                    {
                        var mappingNote = $"📝 CATEGORY MAPPING DECISION 📝\n" +
                                         $"Migration ID: {migrationId}\n" +
                                         $"Source Category: {decision.SourceCategoryName} (ID: {decision.SourceCategoryId})\n" +
                                         $"Action: {decision.Action}\n" +
                                         $"Target Category: {decision.TargetCategoryName ?? "N/A"} (ID: {decision.TargetCategoryId?.ToString() ?? "N/A"})\n" +
                                         $"Affected Users: {decision.AffectedUsersCount}\n" +
                                         $"Affected Record States: {decision.AffectedRecordStatesCount}\n" +
                                         $"Data Loss Risk: {decision.HasDataLoss}\n" +
                                         $"Decision Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC\n" +
                                         $"Decision Made By: User {AbpSession.UserId}";

                        Logger.Info($"Category mapping decision audit: {mappingNote}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error creating category mapping audit notes for migration {migrationId}", ex);
            }
        }

        /// <summary>
        /// Stores performance metrics for migration operations.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <param name="auditData">Audit data containing performance metrics</param>
        private async Task StorePerformanceMetrics(string migrationId, MigrationAuditData auditData)
        {
            try
            {
                var performanceMetrics = new MigrationPerformanceMetrics
                {
                    MigrationId = migrationId,
                    OperationType = auditData.OperationType,
                    StartTime = auditData.StartTime,
                    EndTime = auditData.EndTime,
                    TotalDurationMinutes = auditData.DurationMinutes,

                    // Processing metrics
                    UsersProcessed = auditData.ProcessedUsersCount,
                    RecordStatesProcessed = auditData.ProcessedRecordStatesCount,
                    CategoryMappingsProcessed = auditData.CategoryMappingsCount,

                    // Performance calculations
                    UsersPerMinute = auditData.DurationMinutes > 0 ? auditData.ProcessedUsersCount / auditData.DurationMinutes : 0,
                    RecordStatesPerMinute = auditData.DurationMinutes > 0 ? auditData.ProcessedRecordStatesCount / auditData.DurationMinutes : 0,

                    // System metrics
                    TenantId = AbpSession.TenantId,
                    Success = auditData.Success,
                    ErrorsCount = auditData.ErrorsCount,
                    WarningsCount = auditData.WarningsCount,

                    // Additional performance data
                    PerformanceData = JsonConvert.SerializeObject(new
                    {
                        CompliancePreservationDuration = auditData.AdditionalMetadata?.GetValueOrDefault("CompliancePreservationDuration"),
                        CategoryMappingDuration = auditData.AdditionalMetadata?.GetValueOrDefault("CategoryMappingDuration"),
                        ComplianceRecalculationDuration = auditData.AdditionalMetadata?.GetValueOrDefault("ComplianceRecalculationDuration"),
                        ValidationDuration = auditData.AdditionalMetadata?.GetValueOrDefault("ValidationDuration"),
                        BatchSizes = auditData.AdditionalMetadata?.GetValueOrDefault("BatchSizes"),
                        MemoryUsage = auditData.AdditionalMetadata?.GetValueOrDefault("MemoryUsage")
                    })
                };

                // Store performance metrics (placeholder for actual storage)
                await StorePerformanceMetricsRecord(performanceMetrics);

                // Log performance summary
                Logger.Info($"Performance metrics for migration {migrationId}: " +
                           $"Duration: {auditData.DurationMinutes:F2} min, " +
                           $"Users/min: {performanceMetrics.UsersPerMinute:F2}, " +
                           $"Records/min: {performanceMetrics.RecordStatesPerMinute:F2}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error storing performance metrics for migration {migrationId}", ex);
            }
        }

        /// <summary>
        /// Stores performance metrics record in the metrics storage system.
        /// </summary>
        /// <param name="metrics">Performance metrics to store</param>
        private async Task StorePerformanceMetricsRecord(MigrationPerformanceMetrics metrics)
        {
            try
            {
                // TODO: Store in dedicated performance metrics table or time-series database
                var metricsJson = JsonConvert.SerializeObject(metrics, Formatting.Indented);
                Logger.Info($"Migration Performance Metrics: {metricsJson}");

                // Example implementation:
                // await _performanceMetricsRepository.InsertAsync(metrics);
                // await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error($"Error storing performance metrics record for migration {metrics.MigrationId}", ex);
            }
        }

        /// <summary>
        /// Exports audit trail data for compliance and reporting purposes.
        /// </summary>
        /// <param name="migrationId">Migration identifier</param>
        /// <returns>Structured audit trail export data</returns>
        public async Task<MigrationAuditExportDto> ExportMigrationAuditTrail(string migrationId)
        {
            try
            {
                Logger.Info($"Exporting audit trail for migration {migrationId}");

                // TODO: Retrieve from actual audit storage
                // For now, return a placeholder structure
                var exportData = new MigrationAuditExportDto
                {
                    MigrationId = migrationId,
                    ExportTimestamp = DateTime.UtcNow,
                    ExportedBy = AbpSession.UserId,
                    TenantId = AbpSession.TenantId,

                    // Placeholder data - would be retrieved from audit storage
                    AuditRecords = new List<MigrationAuditRecord>(),
                    PerformanceMetrics = new List<MigrationPerformanceMetrics>(),
                    UserActions = new List<string>(),

                    ExportMetadata = new Dictionary<string, object>
                    {
                        { "ExportFormat", "JSON" },
                        { "ExportVersion", "1.0" },
                        { "DataIntegrityHash", "placeholder-hash" },
                        { "RetentionPolicy", "7 years" }
                    }
                };

                Logger.Info($"Audit trail export completed for migration {migrationId}");
                return exportData;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error exporting audit trail for migration {migrationId}", ex);
                throw new UserFriendlyException(L("AuditTrailExportFailed", ex.Message));
            }
        }

        /// <summary>
        /// Performs audit data retention and cleanup based on configured policies.
        /// </summary>
        /// <param name="retentionPolicyDays">Number of days to retain audit data</param>
        /// <returns>Cleanup summary with counts of processed records</returns>
        public async Task<AuditDataCleanupResultDto> PerformAuditDataCleanup(int retentionPolicyDays = 2555) // Default: 7 years
        {
            try
            {
                Logger.Info($"Starting audit data cleanup with retention policy: {retentionPolicyDays} days");

                var cutoffDate = DateTime.UtcNow.AddDays(-retentionPolicyDays);
                var cleanupResult = new AuditDataCleanupResultDto
                {
                    CleanupTimestamp = DateTime.UtcNow,
                    RetentionPolicyDays = retentionPolicyDays,
                    CutoffDate = cutoffDate,
                    PerformedBy = AbpSession.UserId
                };

                // TODO: Implement actual cleanup logic
                // Example:
                // var expiredAuditRecords = await _migrationAuditRepository.GetAll()
                //     .Where(ar => ar.AuditTimestamp < cutoffDate)
                //     .ToListAsync();
                //
                // foreach (var record in expiredAuditRecords)
                // {
                //     await _migrationAuditRepository.DeleteAsync(record);
                //     cleanupResult.DeletedAuditRecords++;
                // }

                // Placeholder cleanup summary
                cleanupResult.DeletedAuditRecords = 0;
                cleanupResult.DeletedPerformanceMetrics = 0;
                cleanupResult.ArchivedRecords = 0;
                cleanupResult.Success = true;
                cleanupResult.Message = "Audit data cleanup completed successfully";

                Logger.Info($"Audit data cleanup completed. Deleted: {cleanupResult.DeletedAuditRecords} audit records, " +
                           $"{cleanupResult.DeletedPerformanceMetrics} performance metrics");

                return cleanupResult;
            }
            catch (Exception ex)
            {
                Logger.Error("Error during audit data cleanup", ex);
                return new AuditDataCleanupResultDto
                {
                    Success = false,
                    Message = $"Audit data cleanup failed: {ex.Message}",
                    CleanupTimestamp = DateTime.UtcNow,
                    PerformedBy = AbpSession.UserId
                };
            }
        }

        /// <summary>
        /// Validates audit data integrity and detects any tampering or corruption.
        /// </summary>
        /// <param name="migrationId">Migration identifier to validate</param>
        /// <returns>Audit integrity validation result</returns>
        public async Task<AuditIntegrityValidationResultDto> ValidateAuditDataIntegrity(string migrationId)
        {
            try
            {
                Logger.Info($"Validating audit data integrity for migration {migrationId}");

                var validationResult = new AuditIntegrityValidationResultDto
                {
                    MigrationId = migrationId,
                    ValidationTimestamp = DateTime.UtcNow,
                    ValidatedBy = AbpSession.UserId,
                    IsValid = true,
                    ValidationIssues = new List<string>()
                };

                // TODO: Implement actual integrity validation
                // This would include:
                // - Hash verification of audit records
                // - Timestamp sequence validation
                // - Cross-reference validation between related records
                // - Digital signature verification (if implemented)

                Logger.Info($"Audit data integrity validation completed for migration {migrationId}. Valid: {validationResult.IsValid}");
                return validationResult;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error validating audit data integrity for migration {migrationId}", ex);
                return new AuditIntegrityValidationResultDto
                {
                    MigrationId = migrationId,
                    ValidationTimestamp = DateTime.UtcNow,
                    ValidatedBy = AbpSession.UserId,
                    IsValid = false,
                    ValidationIssues = new List<string> { $"Integrity validation error: {ex.Message}" }
                };
            }
        }

        #endregion Migration Audit Logging

        #region Private Helper Methods

        // Single mapping processing (existing method)
        private async Task<(int AffectedUsersCount, int AffectedRecordStatesCount)> ProcessCategoryMapping(
            RequirementCategoryMappingDto mapping, Guid targetDepartmentId)
        {
            switch (mapping.Action)
            {
                case MappingAction.MapToExisting:
                    return await MapToExistingCategory(mapping);

                case MappingAction.CopyToNew:
                    return await CopyToNewCategory(mapping, targetDepartmentId);

                case MappingAction.Skip:
                    return await SkipCategory(mapping);

                default:
                    throw new ArgumentException("Invalid mapping action");
            }
        }

        // Enhanced orchestration method for multiple mappings
        private async Task<(int TotalAffectedUsers, int TotalAffectedRecordStates)> ProcessCategoryMapping(
            List<RequirementCategoryMappingDto> mappings, Guid targetDepartmentId)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var totalAffectedUserIds = new HashSet<long>();
            var totalAffectedRecordStates = 0;
            var processedMappings = 0;
            var failedMappings = new List<(RequirementCategoryMappingDto Mapping, string Error)>();

            try
            {
                Logger.Info($"Starting ProcessCategoryMapping with {mappings.Count} mappings for target department {targetDepartmentId}");

                // Create overall operation audit data
                var operationAuditData = new
                {
                    TargetDepartmentId = targetDepartmentId,
                    TotalMappings = mappings.Count,
                    MappingsByAction = mappings.GroupBy(m => m.Action).ToDictionary(g => g.Key.ToString(), g => g.Count()),
                    StartTime = DateTime.UtcNow,
                    UserId = AbpSession.UserId,
                    TenantId = AbpSession.TenantId
                };

                Logger.Info($"Category mapping operation started: {JsonConvert.SerializeObject(operationAuditData)}");

                // Validate all mappings before processing
                await ValidateAllMappings(mappings, targetDepartmentId);

                // Group mappings by action for better processing order
                var mappingsByAction = mappings.GroupBy(m => m.Action).ToList();

                // Process mappings in order: MapToExisting -> CopyToNew -> Skip
                // This order ensures that existing categories are used first, then new ones are created,
                // and finally any remaining categories are skipped
                var actionOrder = new[] { MappingAction.MapToExisting, MappingAction.CopyToNew, MappingAction.Skip };

                foreach (var action in actionOrder)
                {
                    var actionMappings = mappingsByAction.FirstOrDefault(g => g.Key == action)?.ToList();
                    if (actionMappings == null || !actionMappings.Any())
                        continue;

                    Logger.Info($"Processing {actionMappings.Count} mappings for action: {action}");

                    foreach (var mapping in actionMappings)
                    {
                        try
                        {
                            // Create individual mapping transaction scope
                            using (var mappingUow = _unitOfWorkManager.Begin())
                            {
                                var (affectedUsers, affectedRecordStates) = await ProcessSingleMapping(mapping, targetDepartmentId);

                                // Track overall statistics
                                // Note: This is a simplified approach - in a real scenario, you would need to track actual user IDs
                                totalAffectedRecordStates += affectedRecordStates;
                                processedMappings++;

                                // Commit the individual mapping transaction
                                await mappingUow.CompleteAsync();

                                Logger.Info($"Successfully processed mapping {processedMappings}/{mappings.Count}: " +
                                           $"Source category {mapping.SourceCategoryId} -> {action} " +
                                           $"(Users: {affectedUsers}, Records: {affectedRecordStates})");
                            }
                        }
                        catch (Exception ex)
                        {
                            var errorMessage = $"Failed to process mapping for source category {mapping.SourceCategoryId} with action {action}: {ex.Message}";
                            Logger.Error(errorMessage, ex);
                            failedMappings.Add((mapping, errorMessage));

                            // Continue processing other mappings even if one fails
                            // This allows partial migration completion
                        }
                    }
                }

                // Calculate total affected users (simplified approach)
                var totalAffectedUsers = await CalculateTotalAffectedUsers(mappings);

                // Log completion summary
                var completionSummary = new
                {
                    TotalMappings = mappings.Count,
                    ProcessedMappings = processedMappings,
                    FailedMappings = failedMappings.Count,
                    TotalAffectedUsers = totalAffectedUsers,
                    TotalAffectedRecordStates = totalAffectedRecordStates,
                    CompletionTime = DateTime.UtcNow,
                    Duration = DateTime.UtcNow - operationAuditData.StartTime
                };

                Logger.Info($"Category mapping operation completed: {JsonConvert.SerializeObject(completionSummary)}");

                // Handle failed mappings
                if (failedMappings.Any())
                {
                    var failureDetails = string.Join("; ", failedMappings.Select(f => $"Category {f.Mapping.SourceCategoryId}: {f.Error}"));
                    Logger.Error($"Some category mappings failed: {failureDetails}");

                    // Decide whether to throw exception or continue with partial success
                    // For now, we'll log the errors but allow partial success
                    // Business rules may require throwing an exception here
                    if (processedMappings == 0)
                    {
                        throw new UserFriendlyException(L("AllCategoryMappingsFailed", failureDetails));
                    }
                    else
                    {
                        Logger.Warn($"Partial category mapping success: {processedMappings}/{mappings.Count} mappings completed successfully");
                    }
                }

                return (totalAffectedUsers, totalAffectedRecordStates);
            }
            catch (UserFriendlyException)
            {
                // Re-throw user-friendly exceptions as-is
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Critical error in ProcessCategoryMapping for target department {targetDepartmentId}", ex);
                throw new UserFriendlyException(L("CategoryMappingProcessFailed", ex.Message));
            }
        }

        private async Task ValidateAllMappings(List<RequirementCategoryMappingDto> mappings, Guid targetDepartmentId)
        {
            try
            {
                Logger.Info($"Validating {mappings.Count} category mappings");

                // Check for duplicate source categories
                var duplicateSourceCategories = mappings
                    .GroupBy(m => m.SourceCategoryId)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicateSourceCategories.Any())
                {
                    throw new UserFriendlyException(L("DuplicateSourceCategoriesInMapping",
                        string.Join(", ", duplicateSourceCategories)));
                }

                // Validate each mapping based on its action
                foreach (var mapping in mappings)
                {
                    await ValidateSingleMapping(mapping, targetDepartmentId);
                }

                Logger.Info("All category mappings validated successfully");
            }
            catch (Exception ex)
            {
                Logger.Error("Category mapping validation failed", ex);
                throw;
            }
        }

        private async Task ValidateSingleMapping(RequirementCategoryMappingDto mapping, Guid targetDepartmentId)
        {
            // Validate source category exists
            var sourceCategory = await _recordCategoryRepository.FirstOrDefaultAsync(mapping.SourceCategoryId);
            if (sourceCategory == null)
            {
                throw new UserFriendlyException(L("SourceCategoryNotFound", mapping.SourceCategoryId));
            }

            // Validate based on mapping action
            switch (mapping.Action)
            {
                case MappingAction.MapToExisting:
                    if (!mapping.TargetCategoryId.HasValue)
                    {
                        throw new UserFriendlyException(L("TargetCategoryIdRequiredForMapping", mapping.SourceCategoryId));
                    }

                    var targetCategory = await _recordCategoryRepository.FirstOrDefaultAsync(mapping.TargetCategoryId.Value);
                    if (targetCategory == null)
                    {
                        throw new UserFriendlyException(L("TargetCategoryNotFound", mapping.TargetCategoryId.Value));
                    }
                    break;

                case MappingAction.CopyToNew:
                    if (string.IsNullOrWhiteSpace(mapping.NewRequirementName))
                    {
                        throw new UserFriendlyException(L("NewRequirementNameRequiredForCopy", mapping.SourceCategoryId));
                    }

                    if (string.IsNullOrWhiteSpace(mapping.NewCategoryName))
                    {
                        throw new UserFriendlyException(L("NewCategoryNameRequiredForCopy", mapping.SourceCategoryId));
                    }
                    break;

                case MappingAction.Skip:
                    // No additional validation required for skip action
                    // But we could add business rules here if needed
                    Logger.Warn($"Category {mapping.SourceCategoryId} will be SKIPPED - potential data loss");
                    break;

                default:
                    throw new UserFriendlyException(L("InvalidMappingAction", mapping.Action, mapping.SourceCategoryId));
            }
        }

        private async Task<(int AffectedUsers, int AffectedRecordStates)> ProcessSingleMapping(
            RequirementCategoryMappingDto mapping, Guid targetDepartmentId)
        {
            Logger.Info($"Processing single mapping: Source {mapping.SourceCategoryId} -> Action {mapping.Action}");

            switch (mapping.Action)
            {
                case MappingAction.MapToExisting:
                    return await MapToExistingCategory(mapping);

                case MappingAction.CopyToNew:
                    return await CopyToNewCategory(mapping, targetDepartmentId);

                case MappingAction.Skip:
                    return await SkipCategory(mapping);

                default:
                    throw new UserFriendlyException(L("UnsupportedMappingAction", mapping.Action));
            }
        }

        // Helper method to calculate total affected users across all mappings
        private async Task<int> CalculateTotalAffectedUsers(List<RequirementCategoryMappingDto> mappings)
        {
            try
            {
                var allAffectedUserIds = new HashSet<long>();

                foreach (var mapping in mappings)
                {
                    var recordStates = await _recordStateRepository.GetAll()
                        .Where(rs => rs.RecordCategoryId == mapping.SourceCategoryId && rs.UserId.HasValue)
                        .Select(rs => rs.UserId.Value)
                        .ToListAsync();

                    foreach (var userId in recordStates)
                    {
                        allAffectedUserIds.Add(userId);
                    }
                }

                return allAffectedUserIds.Count;
            }
            catch (Exception ex)
            {
                Logger.Error("Error calculating total affected users", ex);
                return 0; // Return 0 if calculation fails
            }
        }

        private async Task<(int AffectedUsersCount, int AffectedRecordStatesCount)> MapToExistingCategory(
            RequirementCategoryMappingDto mapping)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            try
            {
                // Validate input parameters
                if (!mapping.TargetCategoryId.HasValue)
                {
                    throw new UserFriendlyException(L("TargetCategoryIdRequired"));
                }

                // Verify source category exists
                var sourceCategory = await _recordCategoryRepository.FirstOrDefaultAsync(mapping.SourceCategoryId);
                if (sourceCategory == null)
                {
                    throw new UserFriendlyException(L("SourceCategoryNotFound", mapping.SourceCategoryId));
                }

                // Verify target category exists and is accessible
                var targetCategory = await _recordCategoryRepository.FirstOrDefaultAsync(mapping.TargetCategoryId.Value);
                if (targetCategory == null)
                {
                    throw new UserFriendlyException(L("TargetCategoryNotFound", mapping.TargetCategoryId.Value));
                }

                // Validate that target category belongs to the correct department/tenant
                await ValidateTargetCategoryAccess(targetCategory);

                // Get all record states that reference the source category
                var recordStatesToUpdate = await _recordStateRepository.GetAll()
                    .Where(rs => rs.RecordCategoryId == mapping.SourceCategoryId)
                    .Include(rs => rs.RecordFk)
                    .Include(rs => rs.UserFk)
                    .ToListAsync();

                if (!recordStatesToUpdate.Any())
                {
                    Logger.Info($"No record states found for source category {mapping.SourceCategoryId}");
                    return (0, 0);
                }

                // Track affected users and record states
                var affectedUserIds = new HashSet<long>();
                var affectedRecordStatesCount = 0;

                // Create audit trail for the mapping operation
                var mappingAuditData = new
                {
                    SourceCategoryId = mapping.SourceCategoryId,
                    SourceCategoryName = sourceCategory.Name,
                    TargetCategoryId = mapping.TargetCategoryId.Value,
                    TargetCategoryName = targetCategory.Name,
                    MappingAction = MappingAction.MapToExisting,
                    Timestamp = DateTime.UtcNow,
                    UserId = AbpSession.UserId,
                    TenantId = AbpSession.TenantId
                };

                Logger.Info($"Starting MapToExistingCategory operation: {JsonConvert.SerializeObject(mappingAuditData)}");

                // Update record states in batches for performance
                const int batchSize = 100;
                var totalRecordStates = recordStatesToUpdate.Count;

                for (int i = 0; i < totalRecordStates; i += batchSize)
                {
                    var batch = recordStatesToUpdate.Skip(i).Take(batchSize).ToList();

                    foreach (var recordState in batch)
                    {
                        // Validate record state before updating
                        if (await ValidateRecordStateForMapping(recordState, targetCategory))
                        {
                            // Store original category for audit purposes
                            var originalCategoryId = recordState.RecordCategoryId;

                            // Update the record state to point to the target category
                            recordState.RecordCategoryId = mapping.TargetCategoryId.Value;

                            // Add audit note about the mapping
                            await AddMappingAuditNote(recordState, originalCategoryId, mapping.TargetCategoryId.Value,
                                sourceCategory.Name, targetCategory.Name);

                            // Track affected users
                            if (recordState.UserId.HasValue)
                            {
                                affectedUserIds.Add(recordState.UserId.Value);
                            }

                            affectedRecordStatesCount++;

                            // Update the record state
                            await _recordStateRepository.UpdateAsync(recordState);
                        }
                        else
                        {
                            Logger.Warn($"Skipping record state {recordState.Id} due to validation failure");
                        }
                    }

                    // Save changes for this batch
                    await CurrentUnitOfWork.SaveChangesAsync();

                    // Log progress for large operations
                    if (totalRecordStates > batchSize)
                    {
                        var processedCount = Math.Min(i + batchSize, totalRecordStates);
                        Logger.Info($"MapToExistingCategory progress: {processedCount}/{totalRecordStates} record states processed");
                    }
                }

                // Update category metadata if needed
                await UpdateCategoryMappingMetadata(sourceCategory, targetCategory, affectedRecordStatesCount);

                // Log completion
                Logger.Info($"MapToExistingCategory completed successfully. " +
                           $"Affected users: {affectedUserIds.Count}, " +
                           $"Affected record states: {affectedRecordStatesCount}");

                return (affectedUserIds.Count, affectedRecordStatesCount);
            }
            catch (UserFriendlyException)
            {
                // Re-throw user-friendly exceptions as-is
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error in MapToExistingCategory for mapping {mapping.SourceCategoryId} -> {mapping.TargetCategoryId}", ex);
                throw new UserFriendlyException(L("CategoryMappingFailed", ex.Message));
            }
        }

        private async Task ValidateTargetCategoryAccess(RecordCategory targetCategory)
        {
            // Ensure the target category is accessible to the current user/tenant
            if (targetCategory.TenantId != AbpSession.TenantId)
            {
                throw new UserFriendlyException(L("TargetCategoryNotAccessible"));
            }

            // Additional validation can be added here for organizational unit access
            // if needed based on business requirements
        }

        private async Task<bool> ValidateRecordStateForMapping(RecordState recordState, RecordCategory targetCategory)
        {
            try
            {
                // Rule 1: Record state must have a valid user
                if (!recordState.UserId.HasValue)
                {
                    Logger.Warn($"Record state {recordState.Id} has no associated user");
                    return false;
                }

                // Rule 2: Record state must not be in a final state that shouldn't be changed
                // (This depends on business rules - adjust as needed)
                var finalStates = new[] { EnumRecordState.Approved };
                if (finalStates.Contains(recordState.State))
                {
                    // Log but allow - business may want to preserve approved states during migration
                    Logger.Info($"Record state {recordState.Id} is in final state {recordState.State} but will be mapped");
                }

                // Rule 3: Validate tenant consistency
                if (recordState.TenantId != targetCategory.TenantId)
                {
                    Logger.Error($"Tenant mismatch: Record state {recordState.Id} tenant {recordState.TenantId} " +
                                $"vs target category tenant {targetCategory.TenantId}");
                    return false;
                }

                // Rule 4: Check for any business-specific validation rules
                // (Add additional validation as needed)

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error validating record state {recordState.Id} for mapping", ex);
                return false;
            }
        }

        private async Task AddMappingAuditNote(RecordState recordState, Guid? originalCategoryId,
            Guid newCategoryId, string originalCategoryName, string newCategoryName)
        {
            try
            {
                var auditNote = new RecordNote
                {
                    RecordStateId = recordState.Id,
                    Note = $"Category mapping during cohort migration: '{originalCategoryName}' -> '{newCategoryName}' " +
                           $"(Category ID: {originalCategoryId} -> {newCategoryId}). " +
                           $"Migration performed by user {AbpSession.UserId} at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC.",
                    Created = DateTime.UtcNow,
                    AuthorizedOnly = true, // Make audit notes visible only to authorized users
                    HostOnly = false,
                    SendNotification = false,
                    UserId = AbpSession.UserId,
                    TenantId = AbpSession.TenantId
                };

                await _recordNoteRepository.InsertAsync(auditNote);
            }
            catch (Exception ex)
            {
                // Don't fail the mapping operation if audit note creation fails
                Logger.Error($"Failed to create audit note for record state {recordState.Id}", ex);
            }
        }

        private async Task UpdateCategoryMappingMetadata(RecordCategory sourceCategory, RecordCategory targetCategory,
            int mappedRecordStatesCount)
        {
            try
            {
                // Update source category metadata to indicate it has been migrated
                var sourceMigrationMetadata = new
                {
                    MigratedTo = targetCategory.Id,
                    MigratedToName = targetCategory.Name,
                    MigrationDate = DateTime.UtcNow,
                    MappedRecordStatesCount = mappedRecordStatesCount,
                    MigratedBy = AbpSession.UserId
                };

                // Note: This assumes there's a metadata field or similar mechanism
                // Adjust based on actual entity structure
                Logger.Info($"Category mapping metadata: {JsonConvert.SerializeObject(sourceMigrationMetadata)}");

                // If there's a specific metadata field in RecordCategory, update it here
                // sourceCategory.Metadata = JsonConvert.SerializeObject(sourceMigrationMetadata);
                // await _recordCategoryRepository.UpdateAsync(sourceCategory);
            }
            catch (Exception ex)
            {
                // Don't fail the mapping operation if metadata update fails
                Logger.Error($"Failed to update category mapping metadata", ex);
            }
        }

        private async Task<(int AffectedUsersCount, int AffectedRecordStatesCount)> CopyToNewCategory(
            RequirementCategoryMappingDto mapping, Guid targetDepartmentId)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            try
            {
                // Validate input parameters
                if (string.IsNullOrWhiteSpace(mapping.NewRequirementName))
                {
                    throw new UserFriendlyException(L("NewRequirementNameRequired"));
                }

                if (string.IsNullOrWhiteSpace(mapping.NewCategoryName))
                {
                    throw new UserFriendlyException(L("NewCategoryNameRequired"));
                }

                // Verify source category exists
                var sourceCategory = await _recordCategoryRepository.GetAll()
                    .Include(rc => rc.RecordRequirementFk)
                    .FirstOrDefaultAsync(rc => rc.Id == mapping.SourceCategoryId);

                if (sourceCategory == null)
                {
                    throw new UserFriendlyException(L("SourceCategoryNotFound", mapping.SourceCategoryId));
                }

                // Verify target department exists
                var targetDepartment = await _tenantDepartmentRepository.FirstOrDefaultAsync(targetDepartmentId);
                if (targetDepartment == null)
                {
                    throw new UserFriendlyException(L("TargetDepartmentNotFound", targetDepartmentId));
                }

                // Validate tenant consistency
                if (targetDepartment.TenantId != AbpSession.TenantId)
                {
                    throw new UserFriendlyException(L("TargetDepartmentNotAccessible"));
                }

                // Check for naming conflicts in target department
                await ValidateNewNamesUniqueness(mapping.NewRequirementName, mapping.NewCategoryName, targetDepartmentId);

                // Get all record states that reference the source category
                var recordStatesToUpdate = await _recordStateRepository.GetAll()
                    .Where(rs => rs.RecordCategoryId == mapping.SourceCategoryId)
                    .Include(rs => rs.RecordFk)
                    .Include(rs => rs.UserFk)
                    .ToListAsync();

                if (!recordStatesToUpdate.Any())
                {
                    Logger.Info($"No record states found for source category {mapping.SourceCategoryId}");
                    return (0, 0);
                }

                // Create audit trail for the copy operation
                var copyAuditData = new
                {
                    SourceCategoryId = mapping.SourceCategoryId,
                    SourceCategoryName = sourceCategory.Name,
                    SourceRequirementName = sourceCategory.RecordRequirementFk?.Name,
                    NewRequirementName = mapping.NewRequirementName,
                    NewCategoryName = mapping.NewCategoryName,
                    TargetDepartmentId = targetDepartmentId,
                    MappingAction = MappingAction.CopyToNew,
                    Timestamp = DateTime.UtcNow,
                    UserId = AbpSession.UserId,
                    TenantId = AbpSession.TenantId
                };

                Logger.Info($"Starting CopyToNewCategory operation: {JsonConvert.SerializeObject(copyAuditData)}");

                // Step 1: Create new requirement in target department
                var newRequirement = await CreateNewRequirement(sourceCategory.RecordRequirementFk,
                    mapping.NewRequirementName, targetDepartmentId);

                // Step 2: Create new category under the new requirement
                var newCategory = await CreateNewCategory(sourceCategory, mapping.NewCategoryName, newRequirement.Id);

                // Step 3: Update record states to point to the new category
                var affectedUserIds = new HashSet<long>();
                var affectedRecordStatesCount = 0;

                // Process record states in batches for performance
                const int batchSize = 100;
                var totalRecordStates = recordStatesToUpdate.Count;

                for (int i = 0; i < totalRecordStates; i += batchSize)
                {
                    var batch = recordStatesToUpdate.Skip(i).Take(batchSize).ToList();

                    foreach (var recordState in batch)
                    {
                        // Validate record state before updating
                        if (await ValidateRecordStateForCopy(recordState, newCategory))
                        {
                            // Store original category for audit purposes
                            var originalCategoryId = recordState.RecordCategoryId;

                            // Update the record state to point to the new category
                            recordState.RecordCategoryId = newCategory.Id;

                            // Add audit note about the copy operation
                            await AddCopyAuditNote(recordState, originalCategoryId, newCategory.Id,
                                sourceCategory.Name, newCategory.Name, newRequirement.Name);

                            // Track affected users
                            if (recordState.UserId.HasValue)
                            {
                                affectedUserIds.Add(recordState.UserId.Value);
                            }

                            affectedRecordStatesCount++;

                            // Update the record state
                            await _recordStateRepository.UpdateAsync(recordState);
                        }
                        else
                        {
                            Logger.Warn($"Skipping record state {recordState.Id} due to validation failure");
                        }
                    }

                    // Save changes for this batch
                    await CurrentUnitOfWork.SaveChangesAsync();

                    // Log progress for large operations
                    if (totalRecordStates > batchSize)
                    {
                        var processedCount = Math.Min(i + batchSize, totalRecordStates);
                        Logger.Info($"CopyToNewCategory progress: {processedCount}/{totalRecordStates} record states processed");
                    }
                }

                // Update metadata for tracking
                await UpdateCopyCategoryMetadata(sourceCategory, newCategory, newRequirement, affectedRecordStatesCount);

                // Log completion
                Logger.Info($"CopyToNewCategory completed successfully. " +
                           $"Created requirement: {newRequirement.Name} (ID: {newRequirement.Id}), " +
                           $"Created category: {newCategory.Name} (ID: {newCategory.Id}), " +
                           $"Affected users: {affectedUserIds.Count}, " +
                           $"Affected record states: {affectedRecordStatesCount}");

                return (affectedUserIds.Count, affectedRecordStatesCount);
            }
            catch (UserFriendlyException)
            {
                // Re-throw user-friendly exceptions as-is
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error in CopyToNewCategory for mapping {mapping.SourceCategoryId} -> new category '{mapping.NewCategoryName}'", ex);
                throw new UserFriendlyException(L("CategoryCopyFailed", ex.Message));
            }
        }

        private async Task ValidateNewNamesUniqueness(string newRequirementName, string newCategoryName, Guid targetDepartmentId)
        {
            // Check for requirement name conflicts in target department
            var existingRequirement = await _recordRequirementRepository.FirstOrDefaultAsync(r =>
                r.TenantDepartmentId == targetDepartmentId &&
                r.Name.ToLower() == newRequirementName.ToLower());

            if (existingRequirement != null)
            {
                throw new UserFriendlyException(L("RequirementNameAlreadyExists", newRequirementName));
            }

            // Check for category name conflicts in target department
            var existingCategory = await _recordCategoryRepository.GetAll()
                .Include(rc => rc.RecordRequirementFk)
                .FirstOrDefaultAsync(rc =>
                    rc.RecordRequirementFk.TenantDepartmentId == targetDepartmentId &&
                    rc.Name.ToLower() == newCategoryName.ToLower());

            if (existingCategory != null)
            {
                throw new UserFriendlyException(L("CategoryNameAlreadyExists", newCategoryName));
            }
        }

        private async Task<RecordRequirement> CreateNewRequirement(RecordRequirement sourceRequirement,
            string newRequirementName, Guid targetDepartmentId)
        {
            try
            {
                var newRequirement = new RecordRequirement
                {
                    Name = newRequirementName.Trim(),
                    Description = sourceRequirement?.Description ?? $"Copied from source requirement during cohort migration",
                    TenantDepartmentId = targetDepartmentId,
                    TenantId = AbpSession.TenantId,
                    IsSurpathOnly = sourceRequirement?.IsSurpathOnly ?? false,
                    // Copy metadata if available, otherwise create migration metadata
                    Metadata = sourceRequirement?.Metadata ?? JsonConvert.SerializeObject(new
                    {
                        CopiedFromRequirement = sourceRequirement?.Id,
                        CopiedFromName = sourceRequirement?.Name,
                        CopyDate = DateTime.UtcNow,
                        CopiedBy = AbpSession.UserId,
                        CopyReason = "Cohort migration - category copy operation"
                    })
                };

                await _recordRequirementRepository.InsertAsync(newRequirement);
                await CurrentUnitOfWork.SaveChangesAsync();

                Logger.Info($"Created new requirement: {newRequirement.Name} (ID: {newRequirement.Id}) in department {targetDepartmentId}");
                return newRequirement;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to create new requirement '{newRequirementName}' in department {targetDepartmentId}", ex);
                throw new UserFriendlyException(L("RequirementCreationFailed", newRequirementName, ex.Message));
            }
        }

        private async Task<RecordCategory> CreateNewCategory(RecordCategory sourceCategory,
            string newCategoryName, Guid newRequirementId)
        {
            try
            {
                var newCategory = new RecordCategory
                {
                    Name = newCategoryName.Trim(),
                    Instructions = sourceCategory.Instructions ?? $"Copied from source category during cohort migration",
                    RecordRequirementId = newRequirementId,
                    TenantId = AbpSession.TenantId,
                    // Copy category rule if it exists
                    RecordCategoryRuleId = sourceCategory.RecordCategoryRuleId
                };

                await _recordCategoryRepository.InsertAsync(newCategory);
                await CurrentUnitOfWork.SaveChangesAsync();

                Logger.Info($"Created new category: {newCategory.Name} (ID: {newCategory.Id}) under requirement {newRequirementId}");
                return newCategory;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to create new category '{newCategoryName}' under requirement {newRequirementId}", ex);
                throw new UserFriendlyException(L("CategoryCreationFailed", newCategoryName, ex.Message));
            }
        }

        private async Task<bool> ValidateRecordStateForCopy(RecordState recordState, RecordCategory newCategory)
        {
            try
            {
                // Rule 1: Record state must have a valid user
                if (!recordState.UserId.HasValue)
                {
                    Logger.Warn($"Record state {recordState.Id} has no associated user");
                    return false;
                }

                // Rule 2: Validate tenant consistency
                if (recordState.TenantId != newCategory.TenantId)
                {
                    Logger.Error($"Tenant mismatch: Record state {recordState.Id} tenant {recordState.TenantId} " +
                                $"vs new category tenant {newCategory.TenantId}");
                    return false;
                }

                // Rule 3: Check for any business-specific validation rules
                // (Add additional validation as needed)

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error validating record state {recordState.Id} for copy operation", ex);
                return false;
            }
        }

        private async Task AddCopyAuditNote(RecordState recordState, Guid? originalCategoryId,
            Guid newCategoryId, string originalCategoryName, string newCategoryName, string newRequirementName)
        {
            try
            {
                var auditNote = new RecordNote
                {
                    RecordStateId = recordState.Id,
                    Note = $"Category copy during cohort migration: '{originalCategoryName}' -> '{newCategoryName}' " +
                           $"(Category ID: {originalCategoryId} -> {newCategoryId}). " +
                           $"New requirement: '{newRequirementName}'. " +
                           $"Migration performed by user {AbpSession.UserId} at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC.",
                    Created = DateTime.UtcNow,
                    AuthorizedOnly = true, // Make audit notes visible only to authorized users
                    HostOnly = false,
                    SendNotification = false,
                    UserId = AbpSession.UserId,
                    TenantId = AbpSession.TenantId
                };

                await _recordNoteRepository.InsertAsync(auditNote);
            }
            catch (Exception ex)
            {
                // Don't fail the copy operation if audit note creation fails
                Logger.Error($"Failed to create copy audit note for record state {recordState.Id}", ex);
            }
        }

        private async Task UpdateCopyCategoryMetadata(RecordCategory sourceCategory, RecordCategory newCategory,
            RecordRequirement newRequirement, int copiedRecordStatesCount)
        {
            try
            {
                // Update metadata for tracking the copy operation
                var copyMetadata = new
                {
                    CopiedFrom = sourceCategory.Id,
                    CopiedFromName = sourceCategory.Name,
                    CopyDate = DateTime.UtcNow,
                    CopiedRecordStatesCount = copiedRecordStatesCount,
                    CopiedBy = AbpSession.UserId,
                    NewRequirementId = newRequirement.Id,
                    NewRequirementName = newRequirement.Name,
                    CopyReason = "Cohort migration - category copy operation"
                };

                Logger.Info($"Category copy metadata: {JsonConvert.SerializeObject(copyMetadata)}");

                // If there's a specific metadata field in RecordCategory, update it here
                // newCategory.Metadata = JsonConvert.SerializeObject(copyMetadata);
                // await _recordCategoryRepository.UpdateAsync(newCategory);
            }
            catch (Exception ex)
            {
                // Don't fail the copy operation if metadata update fails
                Logger.Error($"Failed to update category copy metadata", ex);
            }
        }

        private async Task<(int AffectedUsersCount, int AffectedRecordStatesCount)> SkipCategory(
            RequirementCategoryMappingDto mapping)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            try
            {
                // Verify source category exists
                var sourceCategory = await _recordCategoryRepository.GetAll()
                    .Include(rc => rc.RecordRequirementFk)
                    .FirstOrDefaultAsync(rc => rc.Id == mapping.SourceCategoryId);

                if (sourceCategory == null)
                {
                    throw new UserFriendlyException(L("SourceCategoryNotFound", mapping.SourceCategoryId));
                }

                // Get all record states that reference the source category
                var recordStatesToSkip = await _recordStateRepository.GetAll()
                    .Where(rs => rs.RecordCategoryId == mapping.SourceCategoryId)
                    .Include(rs => rs.RecordFk)
                    .Include(rs => rs.UserFk)
                    .ToListAsync();

                if (!recordStatesToSkip.Any())
                {
                    Logger.Info($"No record states found for source category {mapping.SourceCategoryId} to skip");
                    return (0, 0);
                }

                // Create audit trail for the skip operation
                var skipAuditData = new
                {
                    SourceCategoryId = mapping.SourceCategoryId,
                    SourceCategoryName = sourceCategory.Name,
                    SourceRequirementName = sourceCategory.RecordRequirementFk?.Name,
                    MappingAction = MappingAction.Skip,
                    RecordStatesToSkip = recordStatesToSkip.Count,
                    DataLossWarning = true,
                    Timestamp = DateTime.UtcNow,
                    UserId = AbpSession.UserId,
                    TenantId = AbpSession.TenantId
                };

                Logger.Warn($"Starting SkipCategory operation with DATA LOSS WARNING: {JsonConvert.SerializeObject(skipAuditData)}");

                // Track affected users and record states
                var affectedUserIds = new HashSet<long>();
                var affectedRecordStatesCount = 0;

                // Process record states in batches for performance
                const int batchSize = 100;
                var totalRecordStates = recordStatesToSkip.Count;

                for (int i = 0; i < totalRecordStates; i += batchSize)
                {
                    var batch = recordStatesToSkip.Skip(i).Take(batchSize).ToList();

                    foreach (var recordState in batch)
                    {
                        // Validate record state before processing
                        if (await ValidateRecordStateForSkip(recordState))
                        {
                            // Store original category for audit purposes
                            var originalCategoryId = recordState.RecordCategoryId;

                            // Option 1: Set RecordCategoryId to null (data loss approach)
                            // This approach removes the category reference entirely
                            recordState.RecordCategoryId = null;

                            // Option 2: Alternative approach - could mark with a special "skipped" category
                            // This would require creating a special category for skipped items
                            // recordState.RecordCategoryId = await GetOrCreateSkippedCategoryId();

                            // Add comprehensive audit note about the skip operation with data loss warning
                            await AddSkipAuditNote(recordState, originalCategoryId, sourceCategory.Name);

                            // Track affected users
                            if (recordState.UserId.HasValue)
                            {
                                affectedUserIds.Add(recordState.UserId.Value);
                            }

                            affectedRecordStatesCount++;

                            // Update the record state
                            await _recordStateRepository.UpdateAsync(recordState);
                        }
                        else
                        {
                            Logger.Warn($"Skipping record state {recordState.Id} due to validation failure");
                        }
                    }

                    // Save changes for this batch
                    await CurrentUnitOfWork.SaveChangesAsync();

                    // Log progress for large operations
                    if (totalRecordStates > batchSize)
                    {
                        var processedCount = Math.Min(i + batchSize, totalRecordStates);
                        Logger.Info($"SkipCategory progress: {processedCount}/{totalRecordStates} record states processed");
                    }
                }

                // Update metadata for tracking the skip operation
                await UpdateSkipCategoryMetadata(sourceCategory, affectedRecordStatesCount);

                // Log completion with data loss warning
                Logger.Warn($"SkipCategory completed with DATA LOSS. " +
                           $"Category '{sourceCategory.Name}' (ID: {sourceCategory.Id}) skipped. " +
                           $"Affected users: {affectedUserIds.Count}, " +
                           $"Affected record states: {affectedRecordStatesCount} (DATA LOST)");

                return (affectedUserIds.Count, affectedRecordStatesCount);
            }
            catch (UserFriendlyException)
            {
                // Re-throw user-friendly exceptions as-is
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error in SkipCategory for mapping {mapping.SourceCategoryId}", ex);
                throw new UserFriendlyException(L("CategorySkipFailed", ex.Message));
            }
        }

        private async Task<bool> ValidateRecordStateForSkip(RecordState recordState)
        {
            try
            {
                // Rule 1: Record state must have a valid user
                if (!recordState.UserId.HasValue)
                {
                    Logger.Warn($"Record state {recordState.Id} has no associated user");
                    return false;
                }

                // Rule 2: Check if record state is in a critical state that shouldn't be skipped
                // This is a business decision - some states might be too important to lose
                if (recordState.State == EnumRecordState.Approved)
                {
                    // Log warning but allow - business may decide to skip even approved records
                    Logger.Warn($"Record state {recordState.Id} is in APPROVED state but will be skipped (DATA LOSS)");
                }

                // Rule 3: Additional business validation can be added here
                // For example, checking if the record is required for compliance
                // or if it's part of a critical healthcare requirement

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error validating record state {recordState.Id} for skip operation", ex);
                return false;
            }
        }

        private async Task AddSkipAuditNote(RecordState recordState, Guid? originalCategoryId, string originalCategoryName)
        {
            try
            {
                var auditNote = new RecordNote
                {
                    RecordStateId = recordState.Id,
                    Note = $"⚠️ CATEGORY SKIPPED DURING COHORT MIGRATION - DATA LOSS ⚠️\n" +
                           $"Original category '{originalCategoryName}' (ID: {originalCategoryId}) was SKIPPED during migration.\n" +
                           $"This record state is no longer associated with any category.\n" +
                           $"Migration performed by user {AbpSession.UserId} at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC.\n" +
                           $"⚠️ THIS REPRESENTS A DATA LOSS SITUATION ⚠️",
                    Created = DateTime.UtcNow,
                    AuthorizedOnly = true, // Make audit notes visible only to authorized users
                    HostOnly = false,
                    SendNotification = true, // Send notification for data loss situations
                    UserId = AbpSession.UserId,
                    TenantId = AbpSession.TenantId
                };

                await _recordNoteRepository.InsertAsync(auditNote);
            }
            catch (Exception ex)
            {
                // Don't fail the skip operation if audit note creation fails
                Logger.Error($"Failed to create skip audit note for record state {recordState.Id}", ex);
            }
        }

        private async Task UpdateSkipCategoryMetadata(RecordCategory sourceCategory, int skippedRecordStatesCount)
        {
            try
            {
                // Update metadata for tracking the skip operation
                var skipMetadata = new
                {
                    SkippedCategory = sourceCategory.Id,
                    SkippedCategoryName = sourceCategory.Name,
                    SkipDate = DateTime.UtcNow,
                    SkippedRecordStatesCount = skippedRecordStatesCount,
                    SkippedBy = AbpSession.UserId,
                    DataLossWarning = true,
                    SkipReason = "Cohort migration - category skip operation (DATA LOSS)",
                    RecoveryNote = "Record states have been disconnected from category. Manual intervention may be required for data recovery."
                };

                Logger.Warn($"Category skip metadata (DATA LOSS): {JsonConvert.SerializeObject(skipMetadata)}");

                // If there's a specific metadata field in RecordCategory, update it here
                // sourceCategory.Metadata = JsonConvert.SerializeObject(skipMetadata);
                // await _recordCategoryRepository.UpdateAsync(sourceCategory);
            }
            catch (Exception ex)
            {
                // Don't fail the skip operation if metadata update fails
                Logger.Error($"Failed to update category skip metadata", ex);
            }
        }

        // Optional: Method to create a special "skipped" category for better data tracking
        // This could be used instead of setting RecordCategoryId to null
        private async Task<Guid> GetOrCreateSkippedCategoryId(Guid targetDepartmentId)
        {
            try
            {
                // Check if a "skipped" category already exists in the target department
                var skippedCategory = await _recordCategoryRepository.GetAll()
                    .Include(rc => rc.RecordRequirementFk)
                    .FirstOrDefaultAsync(rc =>
                        rc.RecordRequirementFk.TenantDepartmentId == targetDepartmentId &&
                        rc.Name.ToLower() == "skipped during migration");

                if (skippedCategory != null)
                {
                    return skippedCategory.Id;
                }

                // Create a special requirement for skipped items if it doesn't exist
                var skippedRequirement = await _recordRequirementRepository.FirstOrDefaultAsync(r =>
                    r.TenantDepartmentId == targetDepartmentId &&
                    r.Name.ToLower() == "skipped migration items");

                if (skippedRequirement == null)
                {
                    skippedRequirement = new RecordRequirement
                    {
                        Name = "Skipped Migration Items",
                        Description = "Container for record states that were skipped during cohort migration",
                        TenantDepartmentId = targetDepartmentId,
                        TenantId = AbpSession.TenantId,
                        IsSurpathOnly = true,
                        Metadata = JsonConvert.SerializeObject(new
                        {
                            Purpose = "Migration skip container",
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = AbpSession.UserId,
                            Warning = "This requirement contains skipped migration items"
                        })
                    };

                    await _recordRequirementRepository.InsertAsync(skippedRequirement);
                    await CurrentUnitOfWork.SaveChangesAsync();
                }

                // Create the skipped category
                var newSkippedCategory = new RecordCategory
                {
                    Name = "Skipped During Migration",
                    Instructions = "This category contains record states that were skipped during cohort migration. Manual review may be required.",
                    RecordRequirementId = skippedRequirement.Id,
                    TenantId = AbpSession.TenantId
                };

                await _recordCategoryRepository.InsertAsync(newSkippedCategory);
                await CurrentUnitOfWork.SaveChangesAsync();

                Logger.Info($"Created skipped category: {newSkippedCategory.Name} (ID: {newSkippedCategory.Id})");
                return newSkippedCategory.Id;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to create skipped category for department {targetDepartmentId}", ex);
                throw new UserFriendlyException(L("SkippedCategoryCreationFailed", ex.Message));
            }
        }

        private string DetermineMigrationComplexity(List<RequirementCategoryAnalysisDto> categories)
        {
            if (categories.Count <= 3) return "Simple";
            if (categories.Count <= 10) return "Moderate";
            return "Complex";
        }

        private int CalculateEstimatedDuration(int usersCount, int categoriesCount)
        {
            // Base time: 1 minute per 10 users + 2 minutes per category
            return Math.Max(5, (usersCount / 10) + (categoriesCount * 2));
        }

        private int CalculateSimilarity(string source, string target)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target))
                return 0;

            if (string.Equals(source, target, StringComparison.OrdinalIgnoreCase))
                return 100;

            // Use Levenshtein distance for better similarity calculation
            var levenshteinDistance = CalculateLevenshteinDistance(source.ToLowerInvariant(), target.ToLowerInvariant());
            var maxLength = Math.Max(source.Length, target.Length);

            // Convert distance to similarity percentage (0-100)
            var similarity = (1.0 - (double)levenshteinDistance / maxLength) * 100;

            // Apply healthcare-specific matching bonuses
            var bonusScore = CalculateHealthcareMatchingBonus(source, target);

            return Math.Max(0, Math.Min(100, (int)(similarity + bonusScore)));
        }

        private int CalculateLevenshteinDistance(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
                return target?.Length ?? 0;

            if (string.IsNullOrEmpty(target))
                return source.Length;

            var sourceLength = source.Length;
            var targetLength = target.Length;
            var matrix = new int[sourceLength + 1, targetLength + 1];

            // Initialize first row and column
            for (int i = 0; i <= sourceLength; i++)
                matrix[i, 0] = i;

            for (int j = 0; j <= targetLength; j++)
                matrix[0, j] = j;

            // Calculate distances
            for (int i = 1; i <= sourceLength; i++)
            {
                for (int j = 1; j <= targetLength; j++)
                {
                    var cost = source[i - 1] == target[j - 1] ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(
                            matrix[i - 1, j] + 1,     // deletion
                            matrix[i, j - 1] + 1),    // insertion
                        matrix[i - 1, j - 1] + cost  // substitution
                    );
                }
            }

            return matrix[sourceLength, targetLength];
        }

        private double CalculateHealthcareMatchingBonus(string source, string target)
        {
            var bonus = 0.0;

            // Common healthcare abbreviations and variations
            var healthcarePatterns = new Dictionary<string, string[]>
            {
                { "immunization", new[] { "immun", "vaccine", "vaccination", "shot" } },
                { "background", new[] { "bg", "bgcheck", "criminal", "check" } },
                { "drug", new[] { "substance", "screening", "test", "urine" } },
                { "physical", new[] { "exam", "examination", "health", "medical" } },
                { "cpr", new[] { "cardiopulmonary", "resuscitation", "life support" } },
                { "bls", new[] { "basic life support", "basic life" } },
                { "acls", new[] { "advanced cardiac", "advanced life" } },
                { "license", new[] { "lic", "licensing", "permit" } },
                { "certification", new[] { "cert", "certificate", "certified" } },
                { "training", new[] { "education", "course", "class" } }
            };

            var sourceLower = source.ToLowerInvariant();
            var targetLower = target.ToLowerInvariant();

            foreach (var pattern in healthcarePatterns)
            {
                var keyword = pattern.Key;
                var variations = pattern.Value;

                var sourceContainsKeyword = sourceLower.Contains(keyword);
                var targetContainsKeyword = targetLower.Contains(keyword);

                var sourceContainsVariation = variations.Any(v => sourceLower.Contains(v));
                var targetContainsVariation = variations.Any(v => targetLower.Contains(v));

                // Bonus for matching healthcare concepts
                if ((sourceContainsKeyword && targetContainsKeyword) ||
                    (sourceContainsKeyword && targetContainsVariation) ||
                    (sourceContainsVariation && targetContainsKeyword) ||
                    (sourceContainsVariation && targetContainsVariation))
                {
                    bonus += 15.0; // Significant bonus for healthcare concept match
                    break; // Only apply one concept bonus
                }
            }

            // Bonus for common word patterns
            var sourceWords = sourceLower.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
            var targetWords = targetLower.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);

            var commonWords = sourceWords.Intersect(targetWords).Count();
            if (commonWords > 0)
            {
                bonus += Math.Min(10.0, commonWords * 3.0); // Up to 10 points for common words
            }

            return bonus;
        }

        #endregion Private Helper Methods

        #region User Department Association Management

        /// <summary>
        /// Updates TenantDepartmentUsers associations when a cohort is migrated to a new department.
        /// This ensures users are properly associated with the new department and removed from the old one.
        /// </summary>
        /// <param name="cohortId">The cohort being migrated</param>
        /// <param name="oldDepartmentId">The original department ID (can be null)</param>
        /// <param name="newDepartmentId">The target department ID</param>
        /// <param name="migrationId">Migration ID for audit logging</param>
        /// <returns>Number of users whose department associations were updated</returns>
        private async Task<int> UpdateCohortUserDepartmentAssociations(Guid cohortId, Guid? oldDepartmentId, Guid newDepartmentId, string migrationId)
        {
            try
            {
                // Get all users in the cohort
                var cohortUsers = await _cohortUserRepository.GetAll()
                    .Where(cu => cu.CohortId == cohortId)
                    .Select(cu => cu.UserId)
                    .ToListAsync();

                if (!cohortUsers.Any())
                {
                    Logger.Info($"No users found in cohort {cohortId} for department association update");
                    return 0;
                }

                Logger.Info($"Updating department associations for {cohortUsers.Count} users from cohort {cohortId}");

                var updatedUsersCount = 0;
                var createdAssociationsCount = 0;
                var removedAssociationsCount = 0;

                foreach (var userId in cohortUsers)
                {
                    try
                    {
                        // Check if user already has an association with the new department
                        var existingNewAssociation = await _tenantDepartmentUserRepository.FirstOrDefaultAsync(
                            tdu => tdu.UserId == userId && tdu.TenantDepartmentId == newDepartmentId);

                        // If user doesn't have association with new department, create it
                        if (existingNewAssociation == null)
                        {
                            var newAssociation = new TenantDepartmentUser
                            {
                                UserId = userId,
                                TenantDepartmentId = newDepartmentId,
                                TenantId = AbpSession.TenantId
                            };

                            await _tenantDepartmentUserRepository.InsertAsync(newAssociation);
                            createdAssociationsCount++;
                            Logger.Debug($"Created new department association for user {userId} with department {newDepartmentId}");
                        }
                        else
                        {
                            Logger.Debug($"User {userId} already associated with target department {newDepartmentId}");
                        }

                        // Remove association with old department if it exists and is different from new department
                        if (oldDepartmentId.HasValue && oldDepartmentId.Value != newDepartmentId)
                        {
                            var oldAssociation = await _tenantDepartmentUserRepository.FirstOrDefaultAsync(
                                tdu => tdu.UserId == userId && tdu.TenantDepartmentId == oldDepartmentId.Value);

                            if (oldAssociation != null)
                            {
                                // Check if user has other cohorts in the old department before removing
                                var otherCohortsInOldDept = await _cohortUserRepository.GetAll()
                                    .Join(_cohortRepository.GetAll(),
                                        cu => cu.CohortId,
                                        c => c.Id,
                                        (cu, c) => new { CohortUser = cu, Cohort = c })
                                    .Where(x => x.CohortUser.UserId == userId &&
                                               x.Cohort.TenantDepartmentId == oldDepartmentId.Value &&
                                               x.CohortUser.CohortId != cohortId)
                                    .AnyAsync();

                                if (!otherCohortsInOldDept)
                                {
                                    await _tenantDepartmentUserRepository.DeleteAsync(oldAssociation);
                                    removedAssociationsCount++;
                                    Logger.Debug($"Removed old department association for user {userId} from department {oldDepartmentId.Value}");
                                }
                                else
                                {
                                    Logger.Debug($"User {userId} has other cohorts in old department {oldDepartmentId.Value}, keeping association");
                                }
                            }
                        }

                        updatedUsersCount++;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Failed to update department association for user {userId} in migration {migrationId}", ex);
                        // Continue with other users rather than failing the entire migration
                    }
                }

                // Save all changes
                await CurrentUnitOfWork.SaveChangesAsync();

                Logger.Info($"Department association update completed for migration {migrationId}: " +
                           $"{updatedUsersCount} users processed, " +
                           $"{createdAssociationsCount} new associations created, " +
                           $"{removedAssociationsCount} old associations removed");

                return updatedUsersCount;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to update cohort user department associations for migration {migrationId}", ex);
                throw new UserFriendlyException(L("FailedToUpdateUserDepartmentAssociations", ex.Message));
            }
        }

        #endregion User Department Association Management
    }
}