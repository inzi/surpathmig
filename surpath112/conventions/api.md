# Dynamic Web API Pattern (ASP.NET Zero MVC + jQuery)

## Purpose
ASP.NET Zero automatically exposes application services as Web API endpoints via ABP's dynamic API layer. jQuery clients call these APIs using auto-generated service proxies.

**Note**: This is specific to ASP.NET Zero MVC + jQuery. Angular version uses the same services but different client-side integration.

## Architecture Overview

```
jQuery Client (Browser)
        ↓
abp.services.app.[serviceName].[methodName]()  ← Auto-generated proxy
        ↓
POST /api/services/app/[ServiceName]/[MethodName]  ← Dynamic Web API (ABP magic)
        ↓
[ServiceName]AppService.[Method]()  ← Your business logic
        ↓ [AbpAuthorize] ← Authorization enforced here
Domain Layer / Repositories
```

## How It Works

### 1. Application Service (Backend)
**Location**: `*.Application/[Feature]/[Feature]AppService.cs`

```csharp
[AbpAuthorize(AppPermissions.Pages_Tenants)]
public class TenantAppService : inzibackendAppServiceBase, ITenantAppService
{
    [AbpAuthorize(AppPermissions.Pages_Tenants_Export)]
    public async Task<FileDto> GetTenantUsersToExcel(EntityDto input)
    {
        // Business logic here
        var users = await QueryUsers(input.Id);
        return _exporter.ExportToFile(users);
    }
}
```

### 2. Dynamic Web API (Automatic)
ABP automatically creates this endpoint - **no manual API controller needed**:
```
POST /api/services/app/Tenant/GetTenantUsersToExcel
Content-Type: application/json

{ "id": 5 }
```

### 3. JavaScript Service Proxy (Auto-Generated)
**Location**: Client-side JavaScript

```javascript
var _tenantService = abp.services.app.tenant;

// Calls the dynamic API endpoint
_tenantService.getTenantUsersToExcel({ id: 5 })
    .done(function (result) {
        app.downloadTempFile(result);
    })
    .fail(function (error) {
        abp.message.error(error.message);
    });
```

## Naming Conventions

### C# Service to JavaScript Proxy Mapping

| C# | JavaScript |
|----|------------|
| `TenantAppService` | `abp.services.app.tenant` |
| `UserAppService` | `abp.services.app.user` |
| `CohortUsersAppService` | `abp.services.app.cohortUsers` |
| `GetTenantUsersToExcel(input)` | `getTenantUsersToExcel(input)` |
| `CreateOrUpdate(input)` | `createOrUpdate(input)` |

**Rules**:
- Class name `[Name]AppService` → `abp.services.app.[name]` (camelCase)
- Method name `MethodName` → `methodName` (camelCase)
- Remove "AppService" suffix
- Preserve method names otherwise

## What the Dynamic API Provides

The dynamic Web API layer automatically handles:
- ✅ **JSON serialization/deserialization**
- ✅ **Authorization** (via `[AbpAuthorize]` attributes)
- ✅ **Validation** (via data annotations on DTOs)
- ✅ **Localization** (error messages, validation)
- ✅ **Exception handling** (converts to HTTP status codes)
- ✅ **Audit logging** (tracks API calls)
- ✅ **Unit of Work** (transaction management)
- ✅ **Multi-tenancy** (tenant resolution)

## When to Use Direct Controllers vs. Services

### Use Application Services (Dynamic API) ✅
- **CRUD operations**: Create, read, update, delete
- **Data queries**: List users, get details, search
- **Business operations**: Calculate, process, transform
- **File exports**: Excel/PDF generation (return FileDto)
- **Any operation needing authorization, validation, or audit logging**

### Use MVC Controllers Directly ⚠️
- **File uploads**: MultiPart/form-data (can't go through service proxy easily)
- **Page rendering**: Initial page load with server-side Razor
- **Redirects**: Simple navigation between pages
- **Static file serving**: Images, CSS, JS (though usually wwwroot)
- **Legacy integrations**: When dynamic API doesn't fit

## jQuery Service Proxy Patterns

### Pattern 1: Simple Query
```javascript
var _service = abp.services.app.tenant;

_service.getTenants({
    filter: 'test',
    maxResultCount: 10
})
.done(function (result) {
    console.log('Total:', result.totalCount);
    console.log('Items:', result.items);
});
```

### Pattern 2: Create/Update with Modal
```javascript
var _service = abp.services.app.tenant;

this.save = function () {
    var formData = _$form.serializeFormToObject();

    _modalManager.setBusy(true);

    _service.createOrUpdateTenant(formData)
        .done(function () {
            abp.notify.info(app.localize('SavedSuccessfully'));
            _modalManager.close();
        })
        .always(function () {
            _modalManager.setBusy(false);
        });
};
```

### Pattern 3: File Export
```javascript
$('#ExportButton').click(function () {
    _service.getUsersToExcel({
        filter: $('#FilterInput').val()
    })
    .done(function (result) {
        app.downloadTempFile(result);  // Standard helper
    });
});
```

### Pattern 4: Delete with Confirmation
```javascript
function deleteEntity(entityId) {
    abp.message.confirm(
        app.localize('EntityDeleteWarning'),
        app.localize('AreYouSure'),
        function (isConfirmed) {
            if (isConfirmed) {
                _service.delete({ id: entityId })
                    .done(function () {
                        abp.notify.success(app.localize('SuccessfullyDeleted'));
                        _dataTable.ajax.reload();
                    });
            }
        }
    );
}
```

## Error Handling

### Service Layer Exceptions
```csharp
// In AppService
if (someCondition)
    throw new UserFriendlyException("User-friendly message");

// Generic exception
throw new Exception("Internal error");
```

### jQuery Handling
```javascript
_service.methodName(input)
    .done(function (result) {
        // Success
    })
    .fail(function (error) {
        // UserFriendlyException shows user-friendly message
        abp.message.error(error.message);
    });
```

## Common Mistakes to Avoid

### ❌ Calling Controllers from jQuery
```javascript
// WRONG - Bypasses service layer authorization/validation
$.ajax({
    url: '/App/Tenants/ExportUsers',
    type: 'GET',
    data: { tenantId: 5 }
});
```

### ✅ Use Service Proxy
```javascript
// CORRECT - Goes through service layer
abp.services.app.tenant.getTenantUsersToExcel({ id: 5 })
    .done(function (result) {
        app.downloadTempFile(result);
    });
```

### ❌ File Upload via Service Proxy
```javascript
// WRONG - Service proxy doesn't handle FormData well
var formData = new FormData();
formData.append('file', file);
_service.importUsers(formData);  // Won't work correctly
```

### ✅ File Upload via Controller
```javascript
// CORRECT - Use direct AJAX for file uploads
abp.ajax({
    url: abp.appPath + 'App/Tenants/ImportUsersFromExcel',
    type: 'POST',
    data: formData,
    processData: false,
    contentType: false
});
```

## Benefits of This Pattern

1. **Single Business Logic**: Same services for MVC, Angular, Mobile, API clients
2. **Authorization Enforced**: Can't bypass security by calling different endpoint
3. **Consistent Validation**: Same validation rules everywhere
4. **Testable**: Service layer can be unit tested
5. **Maintainable**: Business logic in one place
6. **Audit Trail**: All operations logged automatically
7. **Localization**: Error messages localized automatically

## Reference Implementations

### Application Services
- `src/inzibackend.Application/MultiTenancy/TenantAppService.cs:849` - GetTenantUsersToExcel
- `src/inzibackend.Application/Authorization/Users/UserAppService.cs:129` - GetUsersToExcel
- `src/inzibackend.Application/Surpath/CohortUsersAppService.cs:523` - GetCohortUsersToExcel

### jQuery Service Proxy Usage
- `src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/Tenants/_ImportUsersModal.js:11` - Service proxy export
- `src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/CohortUsers/Index.js:362` - Export to Excel
- `src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/Tenants/Index.js:3` - Service variable declaration

### Controllers (Thin Wrappers)
- `src/inzibackend.Web.Mvc/Areas/App/Controllers/TenantsController.cs:202` - ExportTenantUsers (7 lines)
- `src/inzibackend.Web.Core/Controllers/FileController.cs:139` - DownloadTempFile pattern

## Verification Checklist

- [ ] Service method has `[AbpAuthorize]` attribute
- [ ] jQuery uses `abp.services.app.*` not direct controller calls
- [ ] File exports use `app.downloadTempFile(result)`
- [ ] File uploads use direct AJAX (not service proxy)
- [ ] Business logic is in service, not controller
- [ ] Service returns DTOs, not entities
- [ ] Error messages use localization

## Notes

- The dynamic API layer is ABP/ASP.NET Zero "magic" - it just works
- No need to manually create API controllers for most operations
- Service proxy is generated at runtime (no build step)
- Works identically for both MVC and Angular clients
- All ABP features (auth, validation, UoW, etc.) work automatically