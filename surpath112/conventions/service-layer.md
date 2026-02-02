# Service Layer Convention (ASP.NET Zero MVC + jQuery)

## Purpose
Application services contain all business logic and are exposed to clients via ABP's dynamic Web API layer. Controllers are thin coordinators only.

## When to Use
- All CRUD operations
- All business logic
- All data queries and transformations
- Any operation that needs authorization, validation, or audit logging

## Architecture Overview

```
jQuery Client (Browser)
        ↓
abp.services.app.[serviceName].[method]()  ← Service Proxy (auto-generated)
        ↓
Dynamic Web API Layer (ABP magic)  ← Handles JSON, auth, validation
        ↓
[ServiceName]AppService.[Method]()  ← Business logic here!
        ↓
Domain Layer / Repositories
```

## Key Components

### 1. Service Interface - `*.Application.Shared/[Feature]/I[Feature]AppService.cs`
```csharp
public interface ITenantAppService : IApplicationService
{
    Task<PagedResultDto<TenantListDto>> GetTenants(GetTenantsInput input);
    Task<FileDto> GetTenantUsersToExcel(EntityDto input);
    Task CreateTenant(CreateTenantInput input);
    // ... more methods
}
```

### 2. Service Implementation - `*.Application/[Feature]/[Feature]AppService.cs`
```csharp
[AbpAuthorize(AppPermissions.Pages_Tenants)]
public class TenantAppService : inzibackendAppServiceBase, ITenantAppService
{
    private readonly IRepository<Tenant, int> _tenantRepository;
    // ... dependencies

    public TenantAppService(IRepository<Tenant, int> tenantRepository, ...)
    {
        _tenantRepository = tenantRepository;
    }

    [AbpAuthorize(AppPermissions.Pages_Tenants_Export)]
    public async Task<FileDto> GetTenantUsersToExcel(EntityDto input)
    {
        // Business logic here
    }
}
```

### 3. Controller - `*.Web.Mvc/Areas/App/Controllers/[Feature]Controller.cs`
**Thin wrapper only!**
```csharp
[AbpMvcAuthorize(AppPermissions.Pages_Tenants_Export)]
public async Task<FileResult> Export(int id)
{
    var file = await _tenantAppService.GetTenantUsersToExcel(new EntityDto(id));
    var fileBytes = _tempFileCacheManager.GetFile(file.FileToken, ...);
    return File(fileBytes, file.FileType, file.FileName);
}
```

### 4. jQuery Client
**Uses service proxy, NOT controller!**
```javascript
var _tenantService = abp.services.app.tenant;

$('#ExportButton').click(function () {
    _tenantService.getTenantUsersToExcel({ id: tenantId })
        .done(function (result) {
            app.downloadTempFile(result);
        });
});
```

## What Goes Where

### Application Service (Business Logic)
✅ Data queries and transformations
✅ Business rule validation
✅ Entity creation/updates
✅ Complex calculations
✅ Authorization enforcement (`[AbpAuthorize]`)
✅ Calling other services
✅ Calling repositories
✅ Object mapping (Entity → DTO)

### Controller (Coordination Only)
✅ Receiving HTTP requests
✅ Calling application service
✅ Returning HTTP responses (Views, Files, JSON)
✅ Basic file upload handling (save to BinaryObjectManager)
✅ Very simple coordination logic only

### jQuery (Client-Side)
✅ UI interactions
✅ Form validation (client-side)
✅ Calling service proxies: `abp.services.app.*`
✅ Handling responses
✅ Displaying notifications

## Multi-Tenancy Filter Pattern

### When Host Queries Tenant Data
```csharp
public async Task<FileDto> GetTenantUsersToExcel(EntityDto input)
{
    var tenantId = input.Id;

    // Host (AbpSession.TenantId == null) querying tenant data
    if (AbpSession.TenantId == null)
    {
        CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
    }

    // Now can query tenant's data
    var users = await _userRepository.GetAll()
        .Where(u => u.TenantId == tenantId)
        .ToListAsync();

    // ...
}
```

### When Tenant Queries Own Data
```csharp
// No filter disabling needed - automatic filtering by current tenant
var users = await _userRepository.GetAll().ToListAsync();
// Only returns users for current tenant
```

## Service Proxy (Dynamic Web API)

### How It Works
1. ABP automatically exposes all public methods of `IApplicationService` implementations as Web API endpoints
2. Endpoints are available at: `/api/services/app/[ServiceName]/[MethodName]`
3. JavaScript proxy generated at: `abp.services.app.[serviceName].[methodName]()`
4. Proxy handles: JSON serialization, authorization headers, error handling, validation

### Example Mapping
```
C# Service: TenantAppService.GetTenantUsersToExcel(EntityDto input)
           ↓
Web API: POST /api/services/app/Tenant/GetTenantUsersToExcel
           ↓
JavaScript: abp.services.app.tenant.getTenantUsersToExcel({ id: 1 })
```

## Reference Implementations

### Standard Service Pattern
- `src/inzibackend.Application/Authorization/Users/UserAppService.cs` - Complete user management service
- `src/inzibackend.Application/MultiTenancy/TenantAppService.cs` - Tenant management service
- `src/inzibackend.Application/Surpath/CohortsAppService.cs` - Domain service example

### Controller Patterns (Thin Wrappers)
- `src/inzibackend.Web.Mvc/Areas/App/Controllers/TenantsController.cs` - Tenant management
- `src/inzibackend.Web.Mvc/Areas/App/Controllers/UsersController.cs` - User management

### jQuery Service Proxy Usage
- `src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/Tenants/_ImportUsersModal.js:11` - Export using service
- `src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/CohortUsers/Index.js:362` - Export using service

## Common Mistakes to Avoid

❌ **Business logic in controller**
❌ **jQuery calls controller endpoints directly** (bypasses service layer authorization)
❌ **Forgetting `[AbpAuthorize]` on service methods**
❌ **Not disabling tenant filter when host queries tenant data**
❌ **Using `GetAllListAsync()` when you need `.Include()` (use `.GetAll().Include().ToListAsync()` instead)**
❌ **N+1 queries** (use LINQ joins or bulk loads)

## Verification Checklist

- [ ] Interface defined in `*.Application.Shared`
- [ ] Implementation in `*.Application` with `[AbpAuthorize]`
- [ ] Controller is thin (< 10 lines per action)
- [ ] jQuery uses `abp.services.app.*` service proxy
- [ ] Tenant filter disabled if host querying tenant data
- [ ] No N+1 queries (use joins or bulk loading)
- [ ] Proper error handling with `UserFriendlyException`
- [ ] DTOs used for data transfer (never domain entities)

## Performance Tips
- Use `GetAll()` for LINQ queries, not `GetAllListAsync()` (allows `.Include()`)
- Bulk load related data (roles, departments) in dictionaries
- Use LINQ joins instead of nested queries
- Disable tracking with `.AsNoTracking()` for read-only queries
- Cache reference data when possible
