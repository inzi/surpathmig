using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using inzibackend.Authorization.Users;

namespace inzibackend.Surpath
{
    public class HierarchicalPricingManager : DomainService
    {
        private readonly IRepository<TenantSurpathService, Guid> _tenantSurpathServiceRepository;
        private readonly IRepository<SurpathService, Guid> _surpathServiceRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;

        public HierarchicalPricingManager(
            IRepository<TenantSurpathService, Guid> tenantSurpathServiceRepository,
            IRepository<SurpathService, Guid> surpathServiceRepository,
            IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
            IRepository<Cohort, Guid> cohortRepository,
            IRepository<User, long> userRepository,
            IRepository<CohortUser, Guid> cohortUserRepository)
        {
            _tenantSurpathServiceRepository = tenantSurpathServiceRepository;
            _surpathServiceRepository = surpathServiceRepository;
            _tenantDepartmentRepository = tenantDepartmentRepository;
            _cohortRepository = cohortRepository;
            _userRepository = userRepository;
            _cohortUserRepository = cohortUserRepository;
        }

        [UnitOfWork]
        public async Task<HierarchicalPricing> GetHierarchicalPricingAsync(int tenantId, Guid? surpathServiceId = null)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var result = new HierarchicalPricing();

                // Get tenant info
                result.Tenant = new TenantPricing
                {
                    Id = tenantId,
                    Name = "Tenant " + tenantId // You might want to get the actual tenant name
                };

                // Base query for tenant surpath services
                var servicesQuery = _tenantSurpathServiceRepository.GetAll()
                    .Include(s => s.SurpathServiceFk)
                    .Where(s => s.TenantId == tenantId && !s.IsDeleted);

                if (surpathServiceId.HasValue)
                {
                    servicesQuery = servicesQuery.Where(s => s.SurpathServiceId == surpathServiceId);
                }

                var allTenantServices = await servicesQuery.ToListAsync();

                // Get tenant-level services (no department, cohort, or user assignment)
                var tenantLevelServices = allTenantServices
                    .Where(s => s.TenantDepartmentId == null && s.CohortId == null && s.UserId == null)
                    .ToList();

                result.Tenant.Services = MapToServicePrices(tenantLevelServices, null);

                // Get all departments for the tenant
                var departments = await _tenantDepartmentRepository.GetAll()
                    .Where(d => d.TenantId == tenantId && !d.IsDeleted)
                    .ToListAsync();

                foreach (var dept in departments)
                {
                    var deptPricing = new DepartmentPricing
                    {
                        Id = dept.Id,
                        Name = dept.Name
                    };

                    // Get department-level services
                    var deptServices = allTenantServices
                        .Where(s => s.TenantDepartmentId == dept.Id && s.CohortId == null && s.UserId == null)
                        .ToList();

                    deptPricing.Services = MapToServicePrices(deptServices, result.Tenant.Services);

                    // Get cohorts for this department
                    var deptCohorts = await _cohortRepository.GetAll()
                        .Where(c => c.TenantDepartmentId == dept.Id && !c.IsDeleted)
                        .ToListAsync();

                    foreach (var cohort in deptCohorts)
                    {
                        var cohortPricing = await BuildCohortPricing(cohort, allTenantServices, deptPricing.Services);
                        deptPricing.Cohorts.Add(cohortPricing);
                    }

                    result.Departments.Add(deptPricing);
                }

                // Get standalone cohorts (cohorts without department)
                var standaloneCohorts = await _cohortRepository.GetAll()
                    .Where(c => c.TenantId == tenantId && c.TenantDepartmentId == null && !c.IsDeleted)
                    .ToListAsync();

                foreach (var cohort in standaloneCohorts)
                {
                    var cohortPricing = await BuildCohortPricing(cohort, allTenantServices, result.Tenant.Services);
                    result.Cohorts.Add(cohortPricing);
                }

                return result;
            }
        }

        private async Task<CohortPricing> BuildCohortPricing(
            Cohort cohort, 
            List<TenantSurpathService> allTenantServices, 
            List<ServicePrice> parentServices)
        {
            var cohortPricing = new CohortPricing
            {
                Id = cohort.Id,
                Name = cohort.Name
            };

            // Get cohort-level services
            var cohortServices = allTenantServices
                .Where(s => s.CohortId == cohort.Id && s.UserId == null)
                .ToList();

            cohortPricing.Services = MapToServicePrices(cohortServices, parentServices);

            // Get users in this cohort
            var cohortUsers = await _cohortUserRepository.GetAll()
                .Include(cu => cu.UserFk)
                .Where(cu => cu.CohortId == cohort.Id && !cu.IsDeleted)
                .ToListAsync();

            foreach (var cohortUser in cohortUsers.Take(10)) // Limit to first 10 users for performance
            {
                if (cohortUser.UserFk != null)
                {
                    var userPricing = new UserPricing
                    {
                        Id = cohortUser.UserFk.Id,
                        Name = cohortUser.UserFk.Name,
                        Surname = cohortUser.UserFk.Surname,
                        UserName = cohortUser.UserFk.UserName,
                        EmailAddress = cohortUser.UserFk.EmailAddress
                    };

                    // Get user-level services
                    var userServices = allTenantServices
                        .Where(s => s.UserId == cohortUser.UserFk.Id)
                        .ToList();

                    userPricing.Services = MapToServicePrices(userServices, cohortPricing.Services);
                    cohortPricing.Users.Add(userPricing);
                }
            }

            return cohortPricing;
        }

        private List<ServicePrice> MapToServicePrices(
            List<TenantSurpathService> services, 
            List<ServicePrice> parentServices)
        {
            var result = new List<ServicePrice>();
            
            // Group by SurpathServiceId to handle multiple records for the same service
            var groupedServices = services.GroupBy(s => s.SurpathServiceId);

            foreach (var group in groupedServices)
            {
                var service = group.FirstOrDefault(s => s.IsPricingOverrideEnabled) ?? group.First();
                var surpathService = service.SurpathServiceFk;
                
                if (surpathService == null) continue;

                // Find parent service price if exists
                var parentService = parentServices?.FirstOrDefault(p => p.ServiceId == service.SurpathServiceId);
                
                // For tenant level (no parent), use the SurpathService price as base price
                // For other levels, use the parent's effective price
                var basePrice = parentService?.EffectivePrice ?? surpathService.Price;

                var isEffectivelyEnabled = service.IsPricingOverrideEnabled || service.IsInvoiced;
                var hasCustomPrice = service.Price > 0;
                var overridePrice = isEffectivelyEnabled && hasCustomPrice ? service.Price : (double?)null;
                var effectiveOverridePrice = overridePrice ?? basePrice;

                var servicePrice = new ServicePrice
                {
                    ServiceId = surpathService.Id,
                    ServiceName = surpathService.Name,
                    ServiceDescription = surpathService.Description,
                    BasePrice = basePrice,
                    OverridePrice = overridePrice,
                    EffectivePrice = effectiveOverridePrice,
                    IsInherited = !isEffectivelyEnabled,
                    TenantSurpathServiceId = service.Id,
                    IsEnabled = isEffectivelyEnabled,
                    IsInvoiced = service.IsInvoiced
                };

                result.Add(servicePrice);
            }

            // Add parent services that don't have overrides at this level
            if (parentServices != null)
            {
                foreach (var parentService in parentServices)
                {
                    if (!result.Any(r => r.ServiceId == parentService.ServiceId))
                    {
                        var inheritedPrice = new ServicePrice
                        {
                            ServiceId = parentService.ServiceId,
                            ServiceName = parentService.ServiceName,
                            ServiceDescription = parentService.ServiceDescription,
                            BasePrice = parentService.EffectivePrice,
                            OverridePrice = null,
                            EffectivePrice = parentService.EffectivePrice,
                            IsInherited = true,
                            TenantSurpathServiceId = null,
                            IsEnabled = parentService.IsEnabled,
                            IsInvoiced = parentService.IsInvoiced
                        };
                        result.Add(inheritedPrice);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets effective pricing for purchase modal during registration or renewal.
        /// This method explicitly disables tenant filter and queries with tenant ID to ensure
        /// it works correctly in both host and tenant contexts.
        /// </summary>
        [UnitOfWork]
        public async Task<HierarchicalPricing> GetPricingForPurchaseAsync(
            int tenantId, 
            Guid? deptId = null, 
            Guid? cohortId = null, 
            long? userId = null)
        {
            // Explicitly disable tenant filter to ensure we can query across tenants
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var result = new HierarchicalPricing();

                // Get tenant info
                result.Tenant = new TenantPricing
                {
                    Id = tenantId,
                    Name = "Tenant " + tenantId
                };

                // Get all tenant surpath services for the specific tenant (including disabled ones)
                var allTenantServices = await _tenantSurpathServiceRepository.GetAll()
                    .Include(s => s.SurpathServiceFk)
                    .Where(s => s.TenantId == tenantId && !s.IsDeleted )
                    .ToListAsync();

                // Get unique service IDs from tenant services
                var serviceIds = allTenantServices
                    .Where(s => s.SurpathServiceId.HasValue)
                    .Select(s => s.SurpathServiceId.Value)
                    .Distinct()
                    .ToList();

                // Get tenant-level services (no department, cohort, or user assignment)
                var tenantLevelServices = allTenantServices
                    .Where(s => s.TenantDepartmentId == null && s.CohortId == null && s.UserId == null)
                    .ToList();

                result.Tenant.Services = MapToServicePrices(tenantLevelServices, null);

                // If department ID is provided, get department services
                if (deptId.HasValue)
                {
                    var dept = await _tenantDepartmentRepository.GetAll()
                        .Where(d => d.Id == deptId.Value && d.TenantId == tenantId && !d.IsDeleted)
                        .FirstOrDefaultAsync();

                    if (dept != null)
                    {
                        var deptPricing = new DepartmentPricing
                        {
                            Id = dept.Id,
                            Name = dept.Name
                        };

                        var deptServices = allTenantServices
                            .Where(s => s.TenantDepartmentId == dept.Id && s.CohortId == null && s.UserId == null)
                            .ToList();

                        deptPricing.Services = MapToServicePrices(deptServices, result.Tenant.Services);
                        result.Departments.Add(deptPricing);

                        // If cohort ID is provided and belongs to this department
                        if (cohortId.HasValue)
                        {
                            var cohort = await _cohortRepository.GetAll()
                                .Where(c => c.Id == cohortId.Value && c.TenantDepartmentId == dept.Id && c.TenantId == tenantId && !c.IsDeleted)
                                .FirstOrDefaultAsync();

                            if (cohort != null)
                            {
                                var cohortPricing = await BuildCohortPricingForPurchase(cohort, allTenantServices, deptPricing.Services, userId);
                                deptPricing.Cohorts.Add(cohortPricing);
                            }
                        }
                    }
                }

                // If cohort ID is provided but no department (standalone cohort)
                if (cohortId.HasValue && !deptId.HasValue)
                {
                    var cohort = await _cohortRepository.GetAll()
                        .Where(c => c.Id == cohortId.Value && c.TenantDepartmentId == null && c.TenantId == tenantId && !c.IsDeleted)
                        .FirstOrDefaultAsync();

                    if (cohort != null)
                    {
                        var cohortPricing = await BuildCohortPricingForPurchase(cohort, allTenantServices, result.Tenant.Services, userId);
                        result.Cohorts.Add(cohortPricing);
                    }
                }

                // If user ID is provided (renewal scenario), include user-specific pricing
                if (userId.HasValue)
                {
                    // Find which cohort the user belongs to
                    var userCohort = await _cohortUserRepository.GetAll()
                        .Include(cu => cu.CohortFk)
                        .Where(cu => cu.UserId == userId.Value && cu.TenantId == tenantId && !cu.IsDeleted)
                        .FirstOrDefaultAsync();

                    if (userCohort != null && userCohort.CohortFk != null)
                    {
                        // Check if we already have this cohort in our result
                        CohortPricing existingCohort = null;
                        
                        if (userCohort.CohortFk.TenantDepartmentId.HasValue)
                        {
                            var dept = result.Departments.FirstOrDefault(d => d.Id == userCohort.CohortFk.TenantDepartmentId.Value);
                            existingCohort = dept?.Cohorts.FirstOrDefault(c => c.Id == userCohort.CohortId);
                        }
                        else
                        {
                            existingCohort = result.Cohorts.FirstOrDefault(c => c.Id == userCohort.CohortId);
                        }

                        // If cohort is not in result, add it
                        if (existingCohort == null)
                        {
                            var cohortPricing = await BuildCohortPricingForPurchase(userCohort.CohortFk, allTenantServices, result.Tenant.Services, userId);
                            
                            if (userCohort.CohortFk.TenantDepartmentId.HasValue)
                            {
                                // Add to department
                                var dept = result.Departments.FirstOrDefault(d => d.Id == userCohort.CohortFk.TenantDepartmentId.Value);
                                if (dept == null)
                                {
                                    var tenantDept = await _tenantDepartmentRepository.GetAll()
                                        .Where(d => d.Id == userCohort.CohortFk.TenantDepartmentId.Value && d.TenantId == tenantId)
                                        .FirstOrDefaultAsync();
                                    
                                    if (tenantDept != null)
                                    {
                                        dept = new DepartmentPricing
                                        {
                                            Id = tenantDept.Id,
                                            Name = tenantDept.Name
                                        };
                                        result.Departments.Add(dept);
                                    }
                                }
                                dept?.Cohorts.Add(cohortPricing);
                            }
                            else
                            {
                                result.Cohorts.Add(cohortPricing);
                            }
                        }
                    }
                }

                return result;
            }
        }

        private async Task<CohortPricing> BuildCohortPricingForPurchase(
            Cohort cohort,
            List<TenantSurpathService> allTenantServices,
            List<ServicePrice> parentServices,
            long? userId)
        {
            var cohortPricing = new CohortPricing
            {
                Id = cohort.Id,
                Name = cohort.Name
            };

            // Get cohort-level services
            var cohortServices = allTenantServices
                .Where(s => s.CohortId == cohort.Id && s.UserId == null)
                .ToList();

            cohortPricing.Services = MapToServicePrices(cohortServices, parentServices);

            // If user ID is provided, include user-specific pricing
            if (userId.HasValue)
            {
                var user = await _userRepository.GetAll()
                    .Where(u => u.Id == userId.Value && u.TenantId == cohort.TenantId)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    var userPricing = new UserPricing
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Surname = user.Surname,
                        UserName = user.UserName,
                        EmailAddress = user.EmailAddress
                    };

                    // Get user-level services
                    var userServices = allTenantServices
                        .Where(s => s.UserId == user.Id)
                        .ToList();

                    userPricing.Services = MapToServicePrices(userServices, cohortPricing.Services);
                    cohortPricing.Users.Add(userPricing);
                }
            }

            return cohortPricing;
        }

        [UnitOfWork]
        public async Task<double> GetEffectivePriceForServiceAsync(
            Guid serviceId, 
            int tenantId, 
            Guid? deptId, 
            Guid? cohortId, 
            long? userId)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                // Get hierarchical pricing for this specific service
                var hierarchicalPricing = await GetHierarchicalPricingAsync(tenantId, serviceId);

                // Find the most specific price based on the hierarchy
                double effectivePrice = 0;

                // Check tenant level first (base price)
                if (hierarchicalPricing.Tenant?.Services != null)
                {
                    var tenantService = hierarchicalPricing.Tenant.Services.FirstOrDefault(s => s.ServiceId == serviceId);
                    if (tenantService != null)
                    {
                        effectivePrice = tenantService.EffectivePrice;
                    }
                }

                // Check department level
                if (deptId.HasValue && hierarchicalPricing.Departments != null)
                {
                    var dept = hierarchicalPricing.Departments.FirstOrDefault(d => d.Id == deptId.Value);
                    if (dept?.Services != null)
                    {
                        var deptService = dept.Services.FirstOrDefault(s => s.ServiceId == serviceId);
                        if (deptService?.OverridePrice != null)
                        {
                            effectivePrice = deptService.OverridePrice.Value;
                        }
                    }
                }

                // Check cohort level
                if (cohortId.HasValue)
                {
                    // Check cohorts within departments
                    if (deptId.HasValue && hierarchicalPricing.Departments != null)
                    {
                        var dept = hierarchicalPricing.Departments.FirstOrDefault(d => d.Id == deptId.Value);
                        var cohort = dept?.Cohorts?.FirstOrDefault(c => c.Id == cohortId.Value);
                        if (cohort?.Services != null)
                        {
                            var cohortService = cohort.Services.FirstOrDefault(s => s.ServiceId == serviceId);
                            if (cohortService?.OverridePrice != null)
                            {
                                effectivePrice = cohortService.OverridePrice.Value;
                            }
                        }
                    }

                    // Check standalone cohorts
                    if (hierarchicalPricing.Cohorts != null)
                    {
                        var cohort = hierarchicalPricing.Cohorts.FirstOrDefault(c => c.Id == cohortId.Value);
                        if (cohort?.Services != null)
                        {
                            var cohortService = cohort.Services.FirstOrDefault(s => s.ServiceId == serviceId);
                            if (cohortService?.OverridePrice != null)
                            {
                                effectivePrice = cohortService.OverridePrice.Value;
                            }
                        }
                    }
                }

                // Check user level
                if (userId.HasValue)
                {
                    // Need to find which cohort the user belongs to
                    var userCohort = await _cohortUserRepository.GetAll()
                        .Where(cu => cu.UserId == userId.Value && !cu.IsDeleted)
                        .Select(cu => cu.CohortId)
                        .FirstOrDefaultAsync();

                    if (userCohort != Guid.Empty)
                    {
                        // Check in department cohorts
                        foreach (var dept in hierarchicalPricing.Departments ?? new List<DepartmentPricing>())
                        {
                            var cohort = dept.Cohorts?.FirstOrDefault(c => c.Id == userCohort);
                            var user = cohort?.Users?.FirstOrDefault(u => u.Id == userId.Value);
                            if (user?.Services != null)
                            {
                                var userService = user.Services.FirstOrDefault(s => s.ServiceId == serviceId);
                                if (userService?.OverridePrice != null)
                                {
                                    return userService.OverridePrice.Value;
                                }
                            }
                        }

                        // Check in standalone cohorts
                        var standaloneCohort = hierarchicalPricing.Cohorts?.FirstOrDefault(c => c.Id == userCohort);
                        var standaloneUser = standaloneCohort?.Users?.FirstOrDefault(u => u.Id == userId.Value);
                        if (standaloneUser?.Services != null)
                        {
                            var userService = standaloneUser.Services.FirstOrDefault(s => s.ServiceId == serviceId);
                            if (userService?.OverridePrice != null)
                            {
                                return userService.OverridePrice.Value;
                            }
                        }
                    }
                }

                return effectivePrice;
            }
        }

        /// <summary>
        /// Gets hierarchical pricing using the new unified structure
        /// </summary>
        [UnitOfWork]
        public async Task<HierarchicalPricingNode> GetHierarchicalPricingV2Async(int tenantId, Guid? surpathServiceId = null, bool includeDisabled = false)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                // Get tenant info
                var tenantNode = new HierarchicalPricingNode
                {
                    Id = tenantId.ToString(),
                    NodeType = "tenant",
                    Name = "Tenant " + tenantId // You might want to get the actual tenant name
                };

                // Get all tenant surpath services
                var servicesQuery = _tenantSurpathServiceRepository.GetAll()
                    .Include(s => s.SurpathServiceFk)
                    .Where(s => s.TenantId == tenantId && !s.IsDeleted);

                // Only filter by IsEnabled if not including disabled services
                if (!includeDisabled)
                {
                    // Filter removed - load all services to support hierarchy inheritance
                }

                if (surpathServiceId.HasValue)
                {
                    servicesQuery = servicesQuery.Where(s => s.SurpathServiceId == surpathServiceId);
                }

                var allTenantServices = await servicesQuery.ToListAsync();

                // Get tenant-level services
                var tenantLevelServices = allTenantServices
                    .Where(s => s.TenantDepartmentId == null && s.CohortId == null && s.UserId == null)
                    .ToList();

                tenantNode.Services = MapToTenantSurpathServiceInfos(tenantLevelServices);

                // Get all departments for the tenant
                var departments = await _tenantDepartmentRepository.GetAll()
                    .Where(d => d.TenantId == tenantId && !d.IsDeleted)
                    .ToListAsync();

                foreach (var dept in departments)
                {
                    var deptNode = new HierarchicalPricingNode
                    {
                        Id = dept.Id.ToString(),
                        NodeType = "department",
                        Name = dept.Name,
                        Description = dept.Description
                    };

                    // Get department-level services
                    var deptServices = allTenantServices
                        .Where(s => s.TenantDepartmentId == dept.Id && s.CohortId == null && s.UserId == null)
                        .ToList();

                    deptNode.Services = MapToTenantSurpathServiceInfos(deptServices);

                    // Get cohorts for this department
                    var deptCohorts = await _cohortRepository.GetAll()
                        .Where(c => c.TenantDepartmentId == dept.Id && !c.IsDeleted)
                        .ToListAsync();

                    foreach (var cohort in deptCohorts)
                    {
                        var cohortNode = await BuildCohortNodeV2(cohort, allTenantServices);
                        deptNode.Children.Add(cohortNode);
                    }

                    tenantNode.Children.Add(deptNode);
                }

                // Get standalone cohorts (cohorts without department)
                var standaloneCohorts = await _cohortRepository.GetAll()
                    .Where(c => c.TenantId == tenantId && c.TenantDepartmentId == null && !c.IsDeleted)
                    .ToListAsync();

                foreach (var cohort in standaloneCohorts)
                {
                    var cohortNode = await BuildCohortNodeV2(cohort, allTenantServices);
                    tenantNode.Children.Add(cohortNode);
                }

                return tenantNode;
            }
        }

        private async Task<HierarchicalPricingNode> BuildCohortNodeV2(
            Cohort cohort,
            List<TenantSurpathService> allTenantServices)
        {
            var cohortNode = new HierarchicalPricingNode
            {
                Id = cohort.Id.ToString(),
                NodeType = "cohort",
                Name = cohort.Name,
                Description = cohort.Description
            };

            // Get cohort-level services
            var cohortServices = allTenantServices
                .Where(s => s.CohortId == cohort.Id && s.UserId == null)
                .ToList();

            cohortNode.Services = MapToTenantSurpathServiceInfos(cohortServices);

            // Get users in this cohort
            var cohortUsers = await _cohortUserRepository.GetAll()
                .Include(cu => cu.UserFk)
                .Where(cu => cu.CohortId == cohort.Id && !cu.IsDeleted)
                .ToListAsync();

            foreach (var cohortUser in cohortUsers.Take(10)) // Limit to first 10 users for performance
            {
                if (cohortUser.UserFk != null)
                {
                    var userNode = new HierarchicalPricingNode
                    {
                        Id = cohortUser.UserFk.Id.ToString(),
                        NodeType = "user",
                        Name = cohortUser.UserFk.Name,
                        Surname = cohortUser.UserFk.Surname,
                        UserName = cohortUser.UserFk.UserName,
                        EmailAddress = cohortUser.UserFk.EmailAddress
                    };

                    // Get user-level services
                    var userServices = allTenantServices
                        .Where(s => s.UserId == cohortUser.UserFk.Id)
                        .ToList();

                    userNode.Services = MapToTenantSurpathServiceInfos(userServices);
                    cohortNode.Children.Add(userNode);
                }
            }

            return cohortNode;
        }

        private List<TenantSurpathServiceInfo> MapToTenantSurpathServiceInfos(List<TenantSurpathService> services)
        {
            var result = new List<TenantSurpathServiceInfo>();

            foreach (var service in services)
            {
                var info = new TenantSurpathServiceInfo
                {
                    Id = service.Id,
                    Name = service.Name ?? service.SurpathServiceFk?.Name,
                    Description = service.Description ?? service.SurpathServiceFk?.Description,
                    Price = service.Price,
                    IsEnabled = service.IsPricingOverrideEnabled || service.IsInvoiced,
                    SurpathServiceId = service.SurpathServiceId,
                    TenantDepartmentId = service.TenantDepartmentId,
                    CohortId = service.CohortId,
                    UserId = service.UserId,
                    // Add base price from SurpathService
                    BasePrice = service.SurpathServiceFk?.Price ?? 0,
                    IsInvoiced = service.IsInvoiced
                };

                result.Add(info);
            }

            return result;
        }

        /// <summary>
        /// Gets effective pricing for purchase modal during registration or renewal using V2 structure
        /// </summary>
        [UnitOfWork]
        public async Task<HierarchicalPricingNode> GetPricingForPurchaseV2Async(
            int tenantId,
            Guid? deptId = null,
            Guid? cohortId = null,
            long? userId = null)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                // Get tenant info
                var tenantNode = new HierarchicalPricingNode
                {
                    Id = tenantId.ToString(),
                    NodeType = "tenant",
                    Name = "Tenant " + tenantId
                };

                // Get all tenant surpath services (including disabled to capture hierarchy overrides)
                var allTenantServices = await _tenantSurpathServiceRepository.GetAll()
                    .Include(s => s.SurpathServiceFk)
                    .Where(s => s.TenantId == tenantId && !s.IsDeleted)
                    .ToListAsync();

                // Get tenant-level services
                var tenantLevelServices = allTenantServices
                    .Where(s => s.TenantDepartmentId == null && s.CohortId == null && s.UserId == null)
                    .ToList();

                tenantNode.Services = MapToTenantSurpathServiceInfos(tenantLevelServices);

                // If department ID is provided, get department node
                if (deptId.HasValue)
                {
                    var dept = await _tenantDepartmentRepository.GetAll()
                        .Where(d => d.Id == deptId.Value && d.TenantId == tenantId && !d.IsDeleted)
                        .FirstOrDefaultAsync();

                    if (dept != null)
                    {
                        var deptNode = new HierarchicalPricingNode
                        {
                            Id = dept.Id.ToString(),
                            NodeType = "department",
                            Name = dept.Name,
                            Description = dept.Description
                        };

                        var deptServices = allTenantServices
                            .Where(s => s.TenantDepartmentId == dept.Id && s.CohortId == null && s.UserId == null)
                            .ToList();

                        deptNode.Services = MapToTenantSurpathServiceInfos(deptServices);
                        tenantNode.Children.Add(deptNode);

                        // If cohort ID is provided and belongs to this department
                        if (cohortId.HasValue)
                        {
                            var cohort = await _cohortRepository.GetAll()
                                .Where(c => c.Id == cohortId.Value && c.TenantDepartmentId == dept.Id && c.TenantId == tenantId && !c.IsDeleted)
                                .FirstOrDefaultAsync();

                            if (cohort != null)
                            {
                                var cohortNode = await BuildCohortNodeForPurchaseV2(cohort, allTenantServices, userId);
                                deptNode.Children.Add(cohortNode);
                            }
                        }
                    }
                }

                // If cohort ID is provided but no department (standalone cohort)
                if (cohortId.HasValue && !deptId.HasValue)
                {
                    var cohort = await _cohortRepository.GetAll()
                        .Where(c => c.Id == cohortId.Value && c.TenantDepartmentId == null && c.TenantId == tenantId && !c.IsDeleted)
                        .FirstOrDefaultAsync();

                    if (cohort != null)
                    {
                        var cohortNode = await BuildCohortNodeForPurchaseV2(cohort, allTenantServices, userId);
                        tenantNode.Children.Add(cohortNode);
                    }
                }

                // If user ID is provided (renewal scenario), ensure we have their cohort
                if (userId.HasValue && !cohortId.HasValue)
                {
                    var userCohort = await _cohortUserRepository.GetAll()
                        .Include(cu => cu.CohortFk)
                        .Where(cu => cu.UserId == userId.Value && cu.TenantId == tenantId && !cu.IsDeleted)
                        .FirstOrDefaultAsync();

                    if (userCohort != null && userCohort.CohortFk != null)
                    {
                        // Build the full path to the user
                        if (userCohort.CohortFk.TenantDepartmentId.HasValue && !deptId.HasValue)
                        {
                            // Need to add department
                            var dept = await _tenantDepartmentRepository.GetAll()
                                .Where(d => d.Id == userCohort.CohortFk.TenantDepartmentId.Value && d.TenantId == tenantId)
                                .FirstOrDefaultAsync();

                            if (dept != null)
                            {
                                var deptNode = new HierarchicalPricingNode
                                {
                                    Id = dept.Id.ToString(),
                                    NodeType = "department",
                                    Name = dept.Name,
                                    Description = dept.Description
                                };

                                var deptServices = allTenantServices
                                    .Where(s => s.TenantDepartmentId == dept.Id && s.CohortId == null && s.UserId == null)
                                    .ToList();

                                deptNode.Services = MapToTenantSurpathServiceInfos(deptServices);
                                tenantNode.Children.Add(deptNode);

                                var cohortNode = await BuildCohortNodeForPurchaseV2(userCohort.CohortFk, allTenantServices, userId);
                                deptNode.Children.Add(cohortNode);
                            }
                        }
                        else
                        {
                            // Standalone cohort
                            var cohortNode = await BuildCohortNodeForPurchaseV2(userCohort.CohortFk, allTenantServices, userId);
                            tenantNode.Children.Add(cohortNode);
                        }
                    }
                }

                return tenantNode;
            }
        }

        private async Task<HierarchicalPricingNode> BuildCohortNodeForPurchaseV2(
            Cohort cohort,
            List<TenantSurpathService> allTenantServices,
            long? userId)
        {
            var cohortNode = new HierarchicalPricingNode
            {
                Id = cohort.Id.ToString(),
                NodeType = "cohort",
                Name = cohort.Name,
                Description = cohort.Description
            };

            // Get cohort-level services
            var cohortServices = allTenantServices
                .Where(s => s.CohortId == cohort.Id && s.UserId == null)
                .ToList();

            cohortNode.Services = MapToTenantSurpathServiceInfos(cohortServices);

            // If user ID is provided, include user node
            if (userId.HasValue)
            {
                var user = await _userRepository.GetAll()
                    .Where(u => u.Id == userId.Value && u.TenantId == cohort.TenantId)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    var userNode = new HierarchicalPricingNode
                    {
                        Id = user.Id.ToString(),
                        NodeType = "user",
                        Name = user.Name,
                        Surname = user.Surname,
                        UserName = user.UserName,
                        EmailAddress = user.EmailAddress
                    };

                    // Get user-level services
                    var userServices = allTenantServices
                        .Where(s => s.UserId == user.Id)
                        .ToList();

                    userNode.Services = MapToTenantSurpathServiceInfos(userServices);
                    cohortNode.Children.Add(userNode);
                }
            }

            return cohortNode;
        }

        private struct PricingResolution
        {
            public bool Found;
            public bool IsEnabled;
            public bool IsInvoiced;
            public double Price;
        }

        private PricingResolution ResolveServicePricingFromNode(
            HierarchicalPricingNode node,
            Guid surpathServiceId)
        {
            var resolution = new PricingResolution();

            var nodeService = node.Services?.FirstOrDefault(s => s.SurpathServiceId == surpathServiceId);
            if (nodeService != null)
            {
                resolution.Found = true;
                resolution.IsEnabled = nodeService.IsEnabled;
                resolution.IsInvoiced = nodeService.IsEnabled && nodeService.IsInvoiced;
                resolution.Price = nodeService.Price;
            }

            if (node.Children != null && node.Children.Any())
            {
                foreach (var child in node.Children)
                {
                    var childResolution = ResolveServicePricingFromNode(child, surpathServiceId);
                    if (!childResolution.Found)
                    {
                        continue;
                    }

                    // Prefer the most specific enabled configuration
                    if (!resolution.Found || childResolution.IsEnabled)
                    {
                        resolution = childResolution;
                    }
                }
            }

            return resolution;
        }

        /// <summary>
        /// Resolves the effective price and invoicing state for a service within the pricing hierarchy.
        /// </summary>
        public async Task<ServicePricingDecision> GetServicePricingDecisionAsync(
            Guid surpathServiceId,
            int tenantId,
            Guid? deptId = null,
            Guid? cohortId = null,
            long? userId = null)
        {
            var pricingNode = await GetPricingForPurchaseV2Async(tenantId, deptId, cohortId, userId);
            var resolution = ResolveServicePricingFromNode(pricingNode, surpathServiceId);

            // If the service was never encountered in the hierarchy, treat it as disabled
            if (!resolution.Found)
            {
                return new ServicePricingDecision
                {
                    SurpathServiceId = surpathServiceId,
                    EffectivePrice = 0,
                    IsEnabled = false,
                    IsInvoiced = false
                };
            }

            return new ServicePricingDecision
            {
                SurpathServiceId = surpathServiceId,
                EffectivePrice = resolution.Price,
                IsEnabled = resolution.IsEnabled,
                IsInvoiced = resolution.IsInvoiced
            };
        }

        /// <summary>
        /// Gets the effective amount a user should be charged for a service after applying invoicing rules.
        /// </summary>
        public async Task<double> GetEffectivePriceForServiceV2Async(
            Guid surpathServiceId,
            int tenantId,
            Guid? deptId = null,
            Guid? cohortId = null,
            long? userId = null)
        {
            var decision = await GetServicePricingDecisionAsync(surpathServiceId, tenantId, deptId, cohortId, userId);

            if (!decision.IsEnabled)
            {
                return 0;
            }

            return decision.PriceToCharge;
        }

        /// <summary>
        /// Determines if a user should be invoiced (bypassing payment) based on hierarchical pricing settings
        /// </summary>
        [UnitOfWork]
        public async Task<bool> GetIsInvoicedForUserAsync(
            int tenantId,
            Guid? deptId = null,
            Guid? cohortId = null,
            long? userId = null)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                // Get hierarchical pricing for this tenant
                var hierarchicalPricing = await GetHierarchicalPricingAsync(tenantId);

                // Check if any enabled service at the most specific level is marked as invoiced
                bool isInvoiced = false;

                // Check user level first (most specific)
                if (userId.HasValue)
                {
                    var userCohort = await _cohortUserRepository.GetAll()
                        .Where(cu => cu.UserId == userId.Value && !cu.IsDeleted)
                        .Select(cu => cu.CohortId)
                        .FirstOrDefaultAsync();

                    if (userCohort != Guid.Empty)
                    {
                        // Check in department cohorts
                        foreach (var dept in hierarchicalPricing.Departments ?? new List<DepartmentPricing>())
                        {
                            var cohort = dept.Cohorts?.FirstOrDefault(c => c.Id == userCohort);
                            var user = cohort?.Users?.FirstOrDefault(u => u.Id == userId.Value);
                            if (user?.Services != null)
                            {
                                var userInvoicedServices = user.Services.Where(s => s.IsEnabled && s.IsInvoiced).ToList();
                                if (userInvoicedServices.Any())
                                {
                                    return true; // User has invoiced services
                                }
                            }
                        }

                        // Check in standalone cohorts
                        var standaloneCohort = hierarchicalPricing.Cohorts?.FirstOrDefault(c => c.Id == userCohort);
                        var standaloneUser = standaloneCohort?.Users?.FirstOrDefault(u => u.Id == userId.Value);
                        if (standaloneUser?.Services != null)
                        {
                            var userInvoicedServices = standaloneUser.Services.Where(s => s.IsEnabled && s.IsInvoiced).ToList();
                            if (userInvoicedServices.Any())
                            {
                                return true; // User has invoiced services
                            }
                        }
                    }
                }

                // Check cohort level
                if (cohortId.HasValue)
                {
                    // Check cohorts within departments
                    if (deptId.HasValue && hierarchicalPricing.Departments != null)
                    {
                        var dept = hierarchicalPricing.Departments.FirstOrDefault(d => d.Id == deptId.Value);
                        var cohort = dept?.Cohorts?.FirstOrDefault(c => c.Id == cohortId.Value);
                        if (cohort?.Services != null)
                        {
                            var cohortInvoicedServices = cohort.Services.Where(s => s.IsEnabled && s.IsInvoiced).ToList();
                            if (cohortInvoicedServices.Any())
                            {
                                isInvoiced = true;
                            }
                        }
                    }

                    // Check standalone cohorts
                    if (!isInvoiced && hierarchicalPricing.Cohorts != null)
                    {
                        var cohort = hierarchicalPricing.Cohorts.FirstOrDefault(c => c.Id == cohortId.Value);
                        if (cohort?.Services != null)
                        {
                            var cohortInvoicedServices = cohort.Services.Where(s => s.IsEnabled && s.IsInvoiced).ToList();
                            if (cohortInvoicedServices.Any())
                            {
                                isInvoiced = true;
                            }
                        }
                    }
                }

                // Check department level
                if (!isInvoiced && deptId.HasValue && hierarchicalPricing.Departments != null)
                {
                    var dept = hierarchicalPricing.Departments.FirstOrDefault(d => d.Id == deptId.Value);
                    if (dept?.Services != null)
                    {
                        var deptInvoicedServices = dept.Services.Where(s => s.IsEnabled && s.IsInvoiced).ToList();
                        if (deptInvoicedServices.Any())
                        {
                            isInvoiced = true;
                        }
                    }
                }

                // Check tenant level (least specific)
                if (!isInvoiced && hierarchicalPricing.Tenant?.Services != null)
                {
                    var tenantInvoicedServices = hierarchicalPricing.Tenant.Services.Where(s => s.IsEnabled && s.IsInvoiced).ToList();
                    if (tenantInvoicedServices.Any())
                    {
                        isInvoiced = true;
                    }
                }

                return isInvoiced;
            }
        }

        /// <summary>
        /// Gets effective invoiced status for a specific service using hierarchical priority
        /// </summary>
        [UnitOfWork]
        public async Task<bool> GetIsInvoicedForServiceAsync(
            Guid serviceId,
            int tenantId,
            Guid? deptId = null,
            Guid? cohortId = null,
            long? userId = null)
        {
            var decision = await GetServicePricingDecisionAsync(serviceId, tenantId, deptId, cohortId, userId);
            return decision.IsEnabled && decision.IsInvoiced;
        }
    }
}
