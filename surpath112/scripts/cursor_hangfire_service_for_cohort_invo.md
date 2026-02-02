# Updated Implementation Plan for Invoice Generation with Hangfire

## 1. Enable Hangfire in the Application

First, we need to enable Hangfire in the application by modifying the following configurations:

1. Set `WebConsts.HangfireDashboardEnabled = true` in `src/inzibackend.Web.Core/Common/WebConsts.cs`.
2. Ensure the Hangfire configuration is uncommented in the appropriate startup class.

## 2. Create New Entity for Cohort User Invoices

Since the existing Invoice entity is related to subscription payments, we'll create a new entity for cohort user invoices in `src/inzibackend.Core/Surpath/Invoicing/CohortUserInvoice.cs`:

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using inzibackend.Authorization.Users;

namespace inzibackend.Surpath.Invoicing
{
    [Table("CohortUserInvoices")]
    [Audited]
    public class CohortUserInvoice : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public InvoiceStatus Status { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? PaidDate { get; set; }

        public virtual Guid CohortUserId { get; set; }

        [ForeignKey("CohortUserId")]
        public CohortUser CohortUserFk { get; set; }

        public virtual Guid? TenantDepartmentId { get; set; }

        [ForeignKey("TenantDepartmentId")]
        public TenantDepartment TenantDepartmentFk { get; set; }

        public virtual Guid? CohortId { get; set; }

        [ForeignKey("CohortId")]
        public Cohort CohortFk { get; set; }

        public virtual long UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }
    }

    public enum InvoiceStatus
    {
        Draft = 0,
        Sent = 1,
        Paid = 2,
        Overdue = 3,
        Cancelled = 4
    }
}
```

## 3. Create Price Configuration Entity

We'll need to store pricing rules based on tenant, department, cohort, and individual. Create a `src/inzibackend.Core/Surpath/Invoicing/PriceConfiguration.cs` entity:

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using inzibackend.Authorization.Users;

namespace inzibackend.Surpath.Invoicing
{
    [Table("PriceConfigurations")]
    [Audited]
    public class PriceConfiguration : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public decimal Price { get; set; }

        // Price configuration can be at tenant level
        public bool IsTenantLevel { get; set; } = false;

        // Or department level
        public virtual Guid? TenantDepartmentId { get; set; }

        [ForeignKey("TenantDepartmentId")]
        public TenantDepartment TenantDepartmentFk { get; set; }

        // Or cohort level
        public virtual Guid? CohortId { get; set; }

        [ForeignKey("CohortId")]
        public Cohort CohortFk { get; set; }

        // Or individual user level
        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        // Frequency of invoicing
        public InvoiceFrequency Frequency { get; set; }

        // Optional service or item description
        public string Description { get; set; }
    }

    public enum InvoiceFrequency
    {
        Monthly = 0,
        Quarterly = 1,
        Semiannual = 2,
        Annual = 3,
        OneTime = 4
    }
}
```

## 4. Update DbContext

Add the new entities to the DbContext in `src/inzibackend.EntityFrameworkCore/EntityFrameworkCore/inzibackendDbContext.cs`:

```csharp
public DbSet<CohortUserInvoice> CohortUserInvoices { get; set; }
public DbSet<PriceConfiguration> PriceConfigurations { get; set; }
```

## 5. Create DTOs

Create necessary DTOs in `src/inzibackend.Application.Shared/Surpath/Invoicing/Dtos/`:

```csharp
// CohortUserInvoiceDto.cs
public class CohortUserInvoiceDto : EntityDto<Guid>
{
    public string InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public InvoiceStatus Status { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public Guid CohortUserId { get; set; }
    public string CohortUserName { get; set; }
    public Guid? TenantDepartmentId { get; set; }
    public string TenantDepartmentName { get; set; }
    public Guid? CohortId { get; set; }
    public string CohortName { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; }
}

// PriceConfigurationDto.cs
public class PriceConfigurationDto : EntityDto<Guid>
{
    public decimal Price { get; set; }
    public bool IsTenantLevel { get; set; }
    public Guid? TenantDepartmentId { get; set; }
    public string TenantDepartmentName { get; set; }
    public Guid? CohortId { get; set; }
    public string CohortName { get; set; }
    public long? UserId { get; set; }
    public string UserName { get; set; }
    public InvoiceFrequency Frequency { get; set; }
    public string Description { get; set; }
}
```

## 6. Implement Domain Service

Create a domain service to encapsulate the price calculation logic in `src/inzibackend.Core/Surpath/Invoicing/InvoiceDomainService.cs`:

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

namespace inzibackend.Surpath.Invoicing
{
    public class InvoiceDomainService : DomainService, IInvoiceDomainService
    {
        private readonly IRepository<PriceConfiguration, Guid> _priceConfigRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        
        public InvoiceDomainService(
            IRepository<PriceConfiguration, Guid> priceConfigRepository,
            IRepository<CohortUser, Guid> cohortUserRepository,
            IRepository<Cohort, Guid> cohortRepository)
        {
            _priceConfigRepository = priceConfigRepository;
            _cohortUserRepository = cohortUserRepository;
            _cohortRepository = cohortRepository;
        }

        public async Task<decimal> CalculateInvoiceAmount(Guid cohortUserId)
        {
            var cohortUser = await _cohortUserRepository.GetAsync(cohortUserId);
            if (cohortUser == null)
                throw new ArgumentException("CohortUser not found", nameof(cohortUserId));

            // Try to get user-specific pricing first (highest priority)
            var userPricing = await _priceConfigRepository.FirstOrDefaultAsync(p => 
                p.UserId == cohortUser.UserId && 
                p.TenantId == cohortUser.TenantId);
            
            if (userPricing != null)
                return userPricing.Price;

            // Try to get cohort-specific pricing
            var cohortPricing = await _priceConfigRepository.FirstOrDefaultAsync(p => 
                p.CohortId == cohortUser.CohortId && 
                p.TenantId == cohortUser.TenantId);
            
            if (cohortPricing != null)
                return cohortPricing.Price;

            // Get the cohort to find its department
            var cohort = await _cohortRepository.GetAsync(cohortUser.CohortId.Value);
            
            // Try to get department-specific pricing
            var departmentPricing = await _priceConfigRepository.FirstOrDefaultAsync(p => 
                p.TenantDepartmentId == cohort.TenantDepartmentId && 
                p.TenantId == cohortUser.TenantId);
            
            if (departmentPricing != null)
                return departmentPricing.Price;

            // Fall back to tenant-level pricing
            var tenantPricing = await _priceConfigRepository.FirstOrDefaultAsync(p => 
                p.IsTenantLevel && 
                p.TenantId == cohortUser.TenantId);
            
            if (tenantPricing != null)
                return tenantPricing.Price;

            // If no pricing configuration exists, return default price (could be 0 or some minimal amount)
            return 0;
        }
    }

    public interface IInvoiceDomainService
    {
        Task<decimal> CalculateInvoiceAmount(Guid cohortUserId);
    }
}
```

## 7. Implement Application Service

Create an application service for invoice management in `src/inzibackend.Application/Surpath/Invoicing/CohortUserInvoiceAppService.cs`:

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using inzibackend.Authorization;
using inzibackend.Surpath.Invoicing.Dtos;

namespace inzibackend.Surpath.Invoicing
{
    [AbpAuthorize(AppPermissions.Pages_Invoices)]
    public class CohortUserInvoiceAppService : ApplicationService, ICohortUserInvoiceAppService
    {
        private readonly IRepository<CohortUserInvoice, Guid> _cohortUserInvoiceRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;
        private readonly IInvoiceDomainService _invoiceDomainService;
        private readonly IInvoiceNumberGenerator _invoiceNumberGenerator;
        
        public CohortUserInvoiceAppService(
            IRepository<CohortUserInvoice, Guid> cohortUserInvoiceRepository,
            IRepository<CohortUser, Guid> cohortUserRepository,
            IRepository<Cohort, Guid> cohortRepository,
            IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
            IInvoiceDomainService invoiceDomainService,
            IInvoiceNumberGenerator invoiceNumberGenerator)
        {
            _cohortUserInvoiceRepository = cohortUserInvoiceRepository;
            _cohortUserRepository = cohortUserRepository;
            _cohortRepository = cohortRepository;
            _tenantDepartmentRepository = tenantDepartmentRepository;
            _invoiceDomainService = invoiceDomainService;
            _invoiceNumberGenerator = invoiceNumberGenerator;
        }

        public async Task<CohortUserInvoiceDto> GetInvoiceById(EntityDto<Guid> input)
        {
            var invoice = await _cohortUserInvoiceRepository.GetAsync(input.Id);
            return ObjectMapper.Map<CohortUserInvoiceDto>(invoice);
        }

        [UnitOfWork]
        public async Task<CohortUserInvoiceDto> GenerateInvoiceForCohortUser(EntityDto<Guid> input)
        {
            var cohortUser = await _cohortUserRepository.GetAsync(input.Id);
            if (cohortUser == null)
                throw new ArgumentException("CohortUser not found", nameof(input.Id));
            
            var cohort = await _cohortRepository.GetAsync(cohortUser.CohortId.Value);
            var amount = await _invoiceDomainService.CalculateInvoiceAmount(cohortUser.Id);
            var invoiceNo = await _invoiceNumberGenerator.GetNewInvoiceNumber();
            
            var invoice = new CohortUserInvoice
            {
                InvoiceNumber = invoiceNo,
                InvoiceDate = Clock.Now,
                Amount = amount,
                Status = InvoiceStatus.Draft,
                DueDate = Clock.Now.AddDays(30), // 30-day payment term
                CohortUserId = cohortUser.Id,
                TenantDepartmentId = cohort.TenantDepartmentId,
                CohortId = cohortUser.CohortId,
                UserId = cohortUser.UserId,
                TenantId = cohortUser.TenantId,
                Description = $"Service fee for {cohort.Name}"
            };
            
            var invoiceId = await _cohortUserInvoiceRepository.InsertAndGetIdAsync(invoice);
            var newInvoice = await _cohortUserInvoiceRepository.GetAsync(invoiceId);
            
            return ObjectMapper.Map<CohortUserInvoiceDto>(newInvoice);
        }

        [UnitOfWork]
        public async Task<ListResultDto<CohortUserInvoiceDto>> GenerateInvoicesForCohort(EntityDto<Guid> input)
        {
            var cohort = await _cohortRepository.GetAsync(input.Id);
            if (cohort == null)
                throw new ArgumentException("Cohort not found", nameof(input.Id));
            
            var cohortUsers = await _cohortUserRepository.GetAllListAsync(cu => cu.CohortId == input.Id);
            var generatedInvoices = new List<CohortUserInvoiceDto>();
            
            foreach (var cohortUser in cohortUsers)
            {
                var invoiceDto = await GenerateInvoiceForCohortUser(new EntityDto<Guid>(cohortUser.Id));
                generatedInvoices.Add(invoiceDto);
            }
            
            return new ListResultDto<CohortUserInvoiceDto>(generatedInvoices);
        }

        [UnitOfWork]
        public async Task UpdateInvoiceStatus(UpdateInvoiceStatusDto input)
        {
            var invoice = await _cohortUserInvoiceRepository.GetAsync(input.Id);
            invoice.Status = input.Status;
            
            if (input.Status == InvoiceStatus.Paid)
                invoice.PaidDate = Clock.Now;
            
            await _cohortUserInvoiceRepository.UpdateAsync(invoice);
        }
    }

    public interface ICohortUserInvoiceAppService : IApplicationService
    {
        Task<CohortUserInvoiceDto> GetInvoiceById(EntityDto<Guid> input);
        Task<CohortUserInvoiceDto> GenerateInvoiceForCohortUser(EntityDto<Guid> input);
        Task<ListResultDto<CohortUserInvoiceDto>> GenerateInvoicesForCohort(EntityDto<Guid> input);
        Task UpdateInvoiceStatus(UpdateInvoiceStatusDto input);
    }

    public class UpdateInvoiceStatusDto : EntityDto<Guid>
    {
        public InvoiceStatus Status { get; set; }
    }
}
```

## 8. Create Hangfire Background Job Service

Create a Hangfire job service in `src/inzibackend.Application/Surpath/Jobs/InvoiceGenerationJob.cs`:

```csharp
using System;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Hangfire;
using inzibackend.Surpath.Invoicing;

namespace inzibackend.Surpath.Jobs
{
    public class InvoiceGenerationJob : BackgroundJob<InvoiceGenerationJobArgs>, ITransientDependency
    {
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly ICohortUserInvoiceAppService _invoiceAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        
        public InvoiceGenerationJob(
            IRepository<Cohort, Guid> cohortRepository,
            ICohortUserInvoiceAppService invoiceAppService,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _cohortRepository = cohortRepository;
            _invoiceAppService = invoiceAppService;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [AutomaticRetry(Attempts = 3)]
        public override void Execute(InvoiceGenerationJobArgs args)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                if (args.ForAllCohorts)
                {
                    GenerateInvoicesForAllCohorts().Wait();
                }
                else if (args.CohortId.HasValue)
                {
                    GenerateInvoicesForCohort(args.CohortId.Value).Wait();
                }
                
                uow.Complete();
            }
        }

        private async Task GenerateInvoicesForAllCohorts()
        {
            var cohorts = await _cohortRepository.GetAllListAsync();
            foreach (var cohort in cohorts)
            {
                await GenerateInvoicesForCohort(cohort.Id);
            }
        }

        private async Task GenerateInvoicesForCohort(Guid cohortId)
        {
            await _invoiceAppService.GenerateInvoicesForCohort(new Abp.Application.Services.Dto.EntityDto<Guid>(cohortId));
        }
        
        // Method to schedule this job to run monthly
        public static void ScheduleMonthlyInvoiceGeneration()
        {
            RecurringJob.AddOrUpdate<InvoiceGenerationJob>(
                "GenerateMonthlyInvoices", 
                job => job.Execute(new InvoiceGenerationJobArgs { ForAllCohorts = true }), 
                Cron.Monthly);
        }
        
        // Method to schedule this job to run on the first day of every month at 1:00 AM
        public static void SchedulePreciseMonthlyInvoiceGeneration()
        {
            RecurringJob.AddOrUpdate<InvoiceGenerationJob>(
                "GenerateMonthlyInvoices", 
                job => job.Execute(new InvoiceGenerationJobArgs { ForAllCohorts = true }), 
                "0 1 1 * *"); // At 01:00 on day-of-month 1
        }
    }
    
    [Serializable]
    public class InvoiceGenerationJobArgs
    {
        public Guid? CohortId { get; set; }
        public bool ForAllCohorts { get; set; }
    }
}
```

## 9. Configure Hangfire in Startup

Enable Hangfire in the web project and register the recurring job in `src/inzibackend.Web.Mvc/Startup/Startup.cs`:

```csharp
// In ConfigureServices method
services.AddHangfire(config =>
{
    config.UseSqlServerStorage(_appConfiguration.GetConnectionString("Default"));
});

// In Configure method, after app.UseHangfireServer();
if (WebConsts.HangfireDashboardEnabled)
{
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new AbpHangfireAuthorizationFilter("Pages_Administration_HangfireDashboard") }
    });
    
    // Schedule the invoice generation job to run monthly
    InvoiceGenerationJob.ScheduleMonthlyInvoiceGeneration();
}
```

## 10. Add Service Registration in Module Class

Add the domain service registration in the `inzibackendCoreModule.cs`:

```csharp
public override void Initialize()
{
    IocManager.RegisterAssemblyByConvention(typeof(inzibackendCoreModule).GetAssembly());
    IocManager.Register<IInvoiceDomainService, InvoiceDomainService>();
}
```

## 11. Create Database Migration

Generate and apply a migration to update the database schema:

```bash
# Generate migration
Add-Migration Added_CohortUserInvoice_And_PriceConfiguration

# Apply migration
Update-Database
```

## 12. Testing

Write unit tests for the invoice generation logic to ensure it works correctly:

```csharp
// Test for InvoiceDomainService to verify pricing calculations
[Fact]
public async Task CalculateInvoiceAmount_ShouldReturnCorrectPrice()
{
    // Arrange
    // Setup test data with various pricing configurations
    
    // Act
    var price = await _invoiceDomainService.CalculateInvoiceAmount(cohortUserId);
    
    // Assert
    Assert.Equal(expectedPrice, price);
}

// Test for CohortUserInvoiceAppService to verify invoice generation
[Fact]
public async Task GenerateInvoiceForCohortUser_ShouldCreateValidInvoice()
{
    // Arrange
    // Setup test data
    
    // Act
    var result = await _invoiceAppService.GenerateInvoiceForCohortUser(new EntityDto<Guid>(cohortUserId));
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedAmount, result.Amount);
    // Additional assertions
}
```

This comprehensive plan leverages the existing infrastructure while building a new invoicing subsystem specifically for cohort users. The tiered pricing logic ensures that the correct price is applied based on tenant, department, cohort, and individual settings. The Hangfire integration provides reliable, scheduled execution of the invoice generation process.

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using inzibackend.Authorization.Users;

namespace inzibackend.Surpath.Invoicing
{
    [Table("CohortUserInvoices")]
    [Audited]
    public class CohortUserInvoice : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public InvoiceStatus Status { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? PaidDate { get; set; }

        public virtual Guid CohortUserId { get; set; }

        [ForeignKey("CohortUserId")]
        public CohortUser CohortUserFk { get; set; }

        public virtual Guid? TenantDepartmentId { get; set; }

        [ForeignKey("TenantDepartmentId")]
        public TenantDepartment TenantDepartmentFk { get; set; }

        public virtual Guid? CohortId { get; set; }

        [ForeignKey("CohortId")]
        public Cohort CohortFk { get; set; }

        public virtual long UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }
    }

    public enum InvoiceStatus
    {
        Draft = 0,
        Sent = 1,
        Paid = 2,
        Overdue = 3,
        Cancelled = 4
    }
}
```

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using inzibackend.Authorization.Users;

namespace inzibackend.Surpath.Invoicing
{
    [Table("PriceConfigurations")]
    [Audited]
    public class PriceConfiguration : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public decimal Price { get; set; }

        // Price configuration can be at tenant level
        public bool IsTenantLevel { get; set; } = false;

        // Or department level
        public virtual Guid? TenantDepartmentId { get; set; }

        [ForeignKey("TenantDepartmentId")]
        public TenantDepartment TenantDepartmentFk { get; set; }

        // Or cohort level
        public virtual Guid? CohortId { get; set; }

        [ForeignKey("CohortId")]
        public Cohort CohortFk { get; set; }

        // Or individual user level
        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        // Frequency of invoicing
        public InvoiceFrequency Frequency { get; set; }

        // Optional service or item description
        public string Description { get; set; }
    }

    public enum InvoiceFrequency
    {
        Monthly = 0,
        Quarterly = 1,
        Semiannual = 2,
        Annual = 3,
        OneTime = 4
    }
}
```

```csharp
public DbSet<CohortUserInvoice> CohortUserInvoices { get; set; }
public DbSet<PriceConfiguration> PriceConfigurations { get; set; }
```

```csharp
// CohortUserInvoiceDto.cs
public class CohortUserInvoiceDto : EntityDto<Guid>
{
    public string InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public InvoiceStatus Status { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public Guid CohortUserId { get; set; }
    public string CohortUserName { get; set; }
    public Guid? TenantDepartmentId { get; set; }
    public string TenantDepartmentName { get; set; }
    public Guid? CohortId { get; set; }
    public string CohortName { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; }
}

// PriceConfigurationDto.cs
public class PriceConfigurationDto : EntityDto<Guid>
{
    public decimal Price { get; set; }
    public bool IsTenantLevel { get; set; }
    public Guid? TenantDepartmentId { get; set; }
    public string TenantDepartmentName { get; set; }
    public Guid? CohortId { get; set; }
    public string CohortName { get; set; }
    public long? UserId { get; set; }
    public string UserName { get; set; }
    public InvoiceFrequency Frequency { get; set; }
    public string Description { get; set; }
}
```

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

namespace inzibackend.Surpath.Invoicing
{
    public class InvoiceDomainService : DomainService, IInvoiceDomainService
    {
        private readonly IRepository<PriceConfiguration, Guid> _priceConfigRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        
        public InvoiceDomainService(
            IRepository<PriceConfiguration, Guid> priceConfigRepository,
            IRepository<CohortUser, Guid> cohortUserRepository,
            IRepository<Cohort, Guid> cohortRepository)
        {
            _priceConfigRepository = priceConfigRepository;
            _cohortUserRepository = cohortUserRepository;
            _cohortRepository = cohortRepository;
        }

        public async Task<decimal> CalculateInvoiceAmount(Guid cohortUserId)
        {
            var cohortUser = await _cohortUserRepository.GetAsync(cohortUserId);
            if (cohortUser == null)
                throw new ArgumentException("CohortUser not found", nameof(cohortUserId));

            // Try to get user-specific pricing first (highest priority)
            var userPricing = await _priceConfigRepository.FirstOrDefaultAsync(p => 
                p.UserId == cohortUser.UserId && 
                p.TenantId == cohortUser.TenantId);
            
            if (userPricing != null)
                return userPricing.Price;

            // Try to get cohort-specific pricing
            var cohortPricing = await _priceConfigRepository.FirstOrDefaultAsync(p => 
                p.CohortId == cohortUser.CohortId && 
                p.TenantId == cohortUser.TenantId);
            
            if (cohortPricing != null)
                return cohortPricing.Price;

            // Get the cohort to find its department
            var cohort = await _cohortRepository.GetAsync(cohortUser.CohortId.Value);
            
            // Try to get department-specific pricing
            var departmentPricing = await _priceConfigRepository.FirstOrDefaultAsync(p => 
                p.TenantDepartmentId == cohort.TenantDepartmentId && 
                p.TenantId == cohortUser.TenantId);
            
            if (departmentPricing != null)
                return departmentPricing.Price;

            // Fall back to tenant-level pricing
            var tenantPricing = await _priceConfigRepository.FirstOrDefaultAsync(p => 
                p.IsTenantLevel && 
                p.TenantId == cohortUser.TenantId);
            
            if (tenantPricing != null)
                return tenantPricing.Price;

            // If no pricing configuration exists, return default price (could be 0 or some minimal amount)
            return 0;
        }
    }

    public interface IInvoiceDomainService
    {
        Task<decimal> CalculateInvoiceAmount(Guid cohortUserId);
    }
}
```

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using inzibackend.Authorization;
using inzibackend.Surpath.Invoicing.Dtos;

namespace inzibackend.Surpath.Invoicing
{
    [AbpAuthorize(AppPermissions.Pages_Invoices)]
    public class CohortUserInvoiceAppService : ApplicationService, ICohortUserInvoiceAppService
    {
        private readonly IRepository<CohortUserInvoice, Guid> _cohortUserInvoiceRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;
        private readonly IInvoiceDomainService _invoiceDomainService;
        private readonly IInvoiceNumberGenerator _invoiceNumberGenerator;
        
        public CohortUserInvoiceAppService(
            IRepository<CohortUserInvoice, Guid> cohortUserInvoiceRepository,
            IRepository<CohortUser, Guid> cohortUserRepository,
            IRepository<Cohort, Guid> cohortRepository,
            IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
            IInvoiceDomainService invoiceDomainService,
            IInvoiceNumberGenerator invoiceNumberGenerator)
        {
            _cohortUserInvoiceRepository = cohortUserInvoiceRepository;
            _cohortUserRepository = cohortUserRepository;
            _cohortRepository = cohortRepository;
            _tenantDepartmentRepository = tenantDepartmentRepository;
            _invoiceDomainService = invoiceDomainService;
            _invoiceNumberGenerator = invoiceNumberGenerator;
        }

        public async Task<CohortUserInvoiceDto> GetInvoiceById(EntityDto<Guid> input)
        {
            var invoice = await _cohortUserInvoiceRepository.GetAsync(input.Id);
            return ObjectMapper.Map<CohortUserInvoiceDto>(invoice);
        }

        [UnitOfWork]
        public async Task<CohortUserInvoiceDto> GenerateInvoiceForCohortUser(EntityDto<Guid> input)
        {
            var cohortUser = await _cohortUserRepository.GetAsync(input.Id);
            if (cohortUser == null)
                throw new ArgumentException("CohortUser not found", nameof(input.Id));
            
            var cohort = await _cohortRepository.GetAsync(cohortUser.CohortId.Value);
            var amount = await _invoiceDomainService.CalculateInvoiceAmount(cohortUser.Id);
            var invoiceNo = await _invoiceNumberGenerator.GetNewInvoiceNumber();
            
            var invoice = new CohortUserInvoice
            {
                InvoiceNumber = invoiceNo,
                InvoiceDate = Clock.Now,
                Amount = amount,
                Status = InvoiceStatus.Draft,
                DueDate = Clock.Now.AddDays(30), // 30-day payment term
                CohortUserId = cohortUser.Id,
                TenantDepartmentId = cohort.TenantDepartmentId,
                CohortId = cohortUser.CohortId,
                UserId = cohortUser.UserId,
                TenantId = cohortUser.TenantId,
                Description = $"Service fee for {cohort.Name}"
            };
            
            var invoiceId = await _cohortUserInvoiceRepository.InsertAndGetIdAsync(invoice);
            var newInvoice = await _cohortUserInvoiceRepository.GetAsync(invoiceId);
            
            return ObjectMapper.Map<CohortUserInvoiceDto>(newInvoice);
        }

        [UnitOfWork]
        public async Task<ListResultDto<CohortUserInvoiceDto>> GenerateInvoicesForCohort(EntityDto<Guid> input)
        {
            var cohort = await _cohortRepository.GetAsync(input.Id);
            if (cohort == null)
                throw new ArgumentException("Cohort not found", nameof(input.Id));
            
            var cohortUsers = await _cohortUserRepository.GetAllListAsync(cu => cu.CohortId == input.Id);
            var generatedInvoices = new List<CohortUserInvoiceDto>();
            
            foreach (var cohortUser in cohortUsers)
            {
                var invoiceDto = await GenerateInvoiceForCohortUser(new EntityDto<Guid>(cohortUser.Id));
                generatedInvoices.Add(invoiceDto);
            }
            
            return new ListResultDto<CohortUserInvoiceDto>(generatedInvoices);
        }

        [UnitOfWork]
        public async Task UpdateInvoiceStatus(UpdateInvoiceStatusDto input)
        {
            var invoice = await _cohortUserInvoiceRepository.GetAsync(input.Id);
            invoice.Status = input.Status;
            
            if (input.Status == InvoiceStatus.Paid)
                invoice.PaidDate = Clock.Now;
            
            await _cohortUserInvoiceRepository.UpdateAsync(invoice);
        }
    }

    public interface ICohortUserInvoiceAppService : IApplicationService
    {
        Task<CohortUserInvoiceDto> GetInvoiceById(EntityDto<Guid> input);
        Task<CohortUserInvoiceDto> GenerateInvoiceForCohortUser(EntityDto<Guid> input);
        Task<ListResultDto<CohortUserInvoiceDto>> GenerateInvoicesForCohort(EntityDto<Guid> input);
        Task UpdateInvoiceStatus(UpdateInvoiceStatusDto input);
    }

    public class UpdateInvoiceStatusDto : EntityDto<Guid>
    {
        public InvoiceStatus Status { get; set; }
    }
}
```

```csharp
using System;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Hangfire;
using inzibackend.Surpath.Invoicing;

namespace inzibackend.Surpath.Jobs
{
    public class InvoiceGenerationJob : BackgroundJob<InvoiceGenerationJobArgs>, ITransientDependency
    {
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly ICohortUserInvoiceAppService _invoiceAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        
        public InvoiceGenerationJob(
            IRepository<Cohort, Guid> cohortRepository,
            ICohortUserInvoiceAppService invoiceAppService,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _cohortRepository = cohortRepository;
            _invoiceAppService = invoiceAppService;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [AutomaticRetry(Attempts = 3)]
        public override void Execute(InvoiceGenerationJobArgs args)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                if (args.ForAllCohorts)
                {
                    GenerateInvoicesForAllCohorts().Wait();
                }
                else if (args.CohortId.HasValue)
                {
                    GenerateInvoicesForCohort(args.CohortId.Value).Wait();
                }
                
                uow.Complete();
            }
        }

        private async Task GenerateInvoicesForAllCohorts()
        {
            var cohorts = await _cohortRepository.GetAllListAsync();
            foreach (var cohort in cohorts)
            {
                await GenerateInvoicesForCohort(cohort.Id);
            }
        }

        private async Task GenerateInvoicesForCohort(Guid cohortId)
        {
            await _invoiceAppService.GenerateInvoicesForCohort(new Abp.Application.Services.Dto.EntityDto<Guid>(cohortId));
        }
        
        // Method to schedule this job to run monthly
        public static void ScheduleMonthlyInvoiceGeneration()
        {
            RecurringJob.AddOrUpdate<InvoiceGenerationJob>(
                "GenerateMonthlyInvoices", 
                job => job.Execute(new InvoiceGenerationJobArgs { ForAllCohorts = true }), 
                Cron.Monthly);
        }
        
        // Method to schedule this job to run on the first day of every month at 1:00 AM
        public static void SchedulePreciseMonthlyInvoiceGeneration()
        {
            RecurringJob.AddOrUpdate<InvoiceGenerationJob>(
                "GenerateMonthlyInvoices", 
                job => job.Execute(new InvoiceGenerationJobArgs { ForAllCohorts = true }), 
                "0 1 1 * *"); // At 01:00 on day-of-month 1
        }
    }
    
    [Serializable]
    public class InvoiceGenerationJobArgs
    {
        public Guid? CohortId { get; set; }
        public bool ForAllCohorts { get; set; }
    }
}
```

```csharp
// In ConfigureServices method
services.AddHangfire(config =>
{
    config.UseSqlServerStorage(_appConfiguration.GetConnectionString("Default"));
});

// In Configure method, after app.UseHangfireServer();
if (WebConsts.HangfireDashboardEnabled)
{
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new AbpHangfireAuthorizationFilter("Pages_Administration_HangfireDashboard") }
    });
    
    // Schedule the invoice generation job to run monthly
    InvoiceGenerationJob.ScheduleMonthlyInvoiceGeneration();
}
```

```csharp
public override void Initialize()
{
    IocManager.RegisterAssemblyByConvention(typeof(inzibackendCoreModule).GetAssembly());
    IocManager.Register<IInvoiceDomainService, InvoiceDomainService>();
}
```

```shellscript
# Generate migration
Add-Migration Added_CohortUserInvoice_And_PriceConfiguration

# Apply migration
Update-Database
```

```csharp
// Test for InvoiceDomainService to verify pricing calculations
[Fact]
public async Task CalculateInvoiceAmount_ShouldReturnCorrectPrice()
{
    // Arrange
    // Setup test data with various pricing configurations
    
    // Act
    var price = await _invoiceDomainService.CalculateInvoiceAmount(cohortUserId);
    
    // Assert
    Assert.Equal(expectedPrice, price);
}

// Test for CohortUserInvoiceAppService to verify invoice generation
[Fact]
public async Task GenerateInvoiceForCohortUser_ShouldCreateValidInvoice()
{
    // Arrange
    // Setup test data
    
    // Act
    var result = await _invoiceAppService.GenerateInvoiceForCohortUser(new EntityDto<Guid>(cohortUserId));
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedAmount, result.Amount);
    // Additional assertions
}
```

