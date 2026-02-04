# ABP Dynamic Web API

[2026-02-02|62b86a8]

## What It Is

ABP (ASP.NET Boilerplate) automatically exposes all public methods of `IApplicationService` implementations as Web API endpoints **without** manually creating `ApiController` classes.

## How It Works

### 1. Service Definition
```csharp
// Interface in *.Application.Shared
public interface ITenantAppService : IApplicationService
{
    Task<PagedResultDto<TenantListDto>> GetTenants(GetTenantsInput input);
}

// Implementation in *.Application
public class TenantAppService : inzibackendAppServiceBase, ITenantAppService
{
    public async Task<PagedResultDto<TenantListDto>> GetTenants(GetTenantsInput input)
    {
        // Business logic here
    }
}
```

### 2. Automatic Endpoint Generation

ABP creates: `POST /api/services/app/Tenant/GetTenants`

- HTTP Method: POST (always, even for queries)
- Route pattern: `/api/services/app/{ServiceName}/{MethodName}`
- Service name derived from class name (removes "AppService" suffix)

### 3. JavaScript Proxy

ABP generates client-side proxies:

```javascript
abp.services.app.tenant.getTenants({ filter: '' })
    .done(function(result) {
        // Handle response
    });
```

Naming convention:
- C# PascalCase → JavaScript camelCase
- `GetTenants()` → `getTenants()`

## Critical Rules

**Controllers are NOT needed** for most business operations:
- ✅ jQuery calls service proxy (`abp.services.app.*`)
- ❌ jQuery should NOT call controller endpoints directly (bypasses service layer authorization)

**Controllers only for**:
- Initial page rendering (MVC views)
- File downloads/uploads that need `FileResult`
- Non-JSON responses
- Thin coordination (< 10 lines)

## Why This Matters

**Wrong Pattern** (bypasses authorization):
```javascript
$.ajax({ url: '/App/Tenants/Export', type: 'GET' });
```

**Correct Pattern**:
```javascript
abp.services.app.tenant.getTenantUsersToExcel({ id: tenantId })
    .done(function(result) {
        app.downloadTempFile(result);
    });
```

## What It Handles Automatically

- JSON serialization/deserialization
- Authorization headers
- Validation
- Error handling
- Audit logging
- Multi-tenancy context

## Related

- (see: service-proxy-pattern) How to call from jQuery
- (see: clean-architecture-layers) Why services are separated from controllers
- Source: `surpath112/conventions/api.md`
