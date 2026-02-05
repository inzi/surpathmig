# Controllers Convention (ASP.NET Zero MVC + jQuery)

## Purpose
Controllers in ASP.NET Zero MVC + jQuery are **thin coordinators** that handle HTTP requests and return views or files. They delegate all business logic to application services.

## When to Use Controllers

Controllers are used for:
- ✅ **Initial page rendering** - Server-side Razor view rendering (Index actions)
- ✅ **Modal population** - Returning partial views for modals
- ✅ **File downloads** - Coordinating file downloads from temp cache
- ✅ **File uploads** - Accepting multipart/form-data uploads, saving to BinaryObjectManager, queuing background jobs
- ✅ **Simple redirects** - Routing between pages

## What Controllers Should NOT Do

Controllers should **NOT** contain:
- ❌ Business logic or calculations
- ❌ Data queries or transformations
- ❌ Authorization logic (use service layer)
- ❌ Complex data mapping
- ❌ Direct repository access (use services)
- ❌ Entity creation/updates (use services)

## Pattern: Thin Controllers

### ✅ Good Example - Modal Action
```csharp
[AbpMvcAuthorize(AppPermissions.Pages_Tenants_ImportUsers)]
public PartialViewResult ImportUsersModal(int tenantId)
{
    var viewModel = new ImportTenantUsersViewModel
    {
        TenantId = tenantId
    };

    return PartialView("_ImportUsersModal", viewModel);
}
```

**Why it's good**:
- Simple view model population
- Returns partial view
- Authorization checked
- No business logic

### ✅ Good Example - File Download
```csharp
[AbpMvcAuthorize(AppPermissions.Pages_Tenants_ImportUsers)]
public async Task<FileResult> ExportTenantUsers(int tenantId)
{
    // Call service for business logic
    var file = await _tenantAppService.GetTenantUsersToExcel(new EntityDto(tenantId));

    // Retrieve file from cache
    var fileBytes = _tempFileCacheManager.GetFile(file.FileToken, AbpSession.UserId, AbpSession.TenantId);
    if (fileBytes == null)
        throw new UserFriendlyException(L("RequestedFileDoesNotExists"));

    // Return file
    return File(fileBytes, file.FileType, file.FileName);
}
```

**Why it's good**:
- Delegates to service for data retrieval
- Only handles file download coordination
- Simple error handling
- Under 10 lines

### ✅ Good Example - File Upload with Background Job
```csharp
[AbpMvcAuthorize(AppPermissions.Pages_Tenants_ImportUsers)]
[HttpPost]
public async Task<JsonResult> ImportUsersFromExcel(int tenantId, IFormFile file)
{
    try
    {
        // Basic validation
        if (file == null || file.Length == 0)
            throw new UserFriendlyException(L("PleaseSelectAFile"));

        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        if (fileExtension != ".xlsx" && fileExtension != ".xls")
            throw new UserFriendlyException(L("InvalidFileTypeExcel"));

        // Read file bytes
        byte[] fileBytes;
        using (var stream = file.OpenReadStream())
        {
            fileBytes = new byte[stream.Length];
            await stream.ReadAsync(fileBytes, 0, (int)stream.Length);
        }

        // Save to binary storage
        var binaryObject = new BinaryObject(tenantId, fileBytes, $"Import_{Guid.NewGuid()}");
        await _binaryObjectManager.SaveAsync(binaryObject);

        // Queue background job (business logic happens here)
        await _backgroundJobManager.EnqueueAsync<ImportJob, ImportJobArgs>(
            new ImportJobArgs
            {
                TenantId = tenantId,
                BinaryObjectId = binaryObject.Id,
                User = new UserIdentifier(AbpSession.TenantId, AbpSession.UserId.Value)
            }
        );

        return Json(new { success = true });
    }
    catch (UserFriendlyException ex)
    {
        return Json(new { success = false, error = ex.Message });
    }
}
```

**Why it's good**:
- Validates file input
- Saves to storage
- Queues background job (where business logic lives)
- Simple error handling
- Returns JSON response

### ❌ Bad Example - Business Logic in Controller
```csharp
// WRONG - Too much logic in controller
public async Task<FileResult> ExportUsers(int tenantId)
{
    // ❌ Direct repository queries
    var users = await _userRepository.GetAll()
        .Where(u => u.TenantId == tenantId)
        .Include(u => u.Roles)
        .ToListAsync();

    // ❌ Complex data transformation
    var exportUsers = new List<UserExportDto>();
    foreach (var user in users)
    {
        var dept = await _deptRepository.FirstOrDefaultAsync(d => d.Id == user.DeptId);
        var roles = user.Roles.Select(r => r.Name).Distinct().ToList();

        exportUsers.Add(new UserExportDto
        {
            UserName = user.UserName,
            DeptName = dept?.Name,
            Roles = string.Join(",", roles)
        });
    }

    // ❌ Creating Excel in controller
    var file = _exporter.ExportToFile(exportUsers);
    return File(...);
}
```

**Why it's bad**:
- 30+ lines of business logic
- Direct repository access
- Data transformation in controller
- Not reusable (can't call from API or mobile)
- Not testable independently

### ✅ Refactored - Delegate to Service
```csharp
// CORRECT - Thin controller
public async Task<FileResult> ExportUsers(int tenantId)
{
    var file = await _userAppService.GetUsersToExcel(new GetUsersInput { TenantId = tenantId });

    var fileBytes = _tempFileCacheManager.GetFile(file.FileToken, AbpSession.UserId, AbpSession.TenantId);
    if (fileBytes == null)
        throw new UserFriendlyException(L("RequestedFileDoesNotExists"));

    return File(fileBytes, file.FileType, file.FileName);
}
```

## Controller Dependencies

Controllers should only inject:
- ✅ Application services (I[Feature]AppService)
- ✅ `ITempFileCacheManager` (for file downloads)
- ✅ `IBinaryObjectManager` (for file uploads)
- ✅ `IBackgroundJobManager` (for queueing jobs)
- ✅ Domain managers (TenantManager, UserManager) if needed for simple lookups
- ✅ Common services (ICommonLookupAppService, IEditionAppService)

Controllers should NOT inject:
- ❌ Repositories (use services instead)
- ❌ Exporters (services use them)
- ❌ Complex domain services (use app services)

## Reference Implementations

### Thin Controllers (Follow These)
- `src/inzibackend.Web.Mvc/Areas/App/Controllers/TenantsController.cs:173,184,202` - Import/Export actions
- `src/inzibackend.Web.Mvc/Areas/App/Controllers/UsersController.cs` - User management
- `src/inzibackend.Web.Core/Controllers/FileController.cs:139` - DownloadTempFile pattern

### What Goes in Controllers
```
Index Action → Query parameters → Return View with ViewModel
Modal Action → Load lookup data → Return PartialView
Export Action → Call service → Download file
Import Action → Validate file → Queue background job → Return JSON
```

## Authorization

Controllers use `[AbpMvcAuthorize]` attribute:
```csharp
[AbpMvcAuthorize(AppPermissions.Pages_Tenants_ImportUsers)]
public async Task<FileResult> ExportTenantUsers(int tenantId)
{
    // ...
}
```

But **primary authorization is at service layer** using `[AbpAuthorize]`. Controller authorization is secondary/defensive.

## Common Patterns

### Pattern 1: Index Action (Page Load)
```csharp
public async Task<ActionResult> Index()
{
    var viewModel = new IndexViewModel
    {
        // Simple lookup data only
        FilterOptions = await _commonLookupService.GetOptions()
    };
    return View(viewModel);
}
```

### Pattern 2: Create/Edit Modal
```csharp
[AbpMvcAuthorize(AppPermissions.Pages_[Entity]_Create)]
public async Task<PartialViewResult> CreateModal()
{
    var viewModel = new CreateViewModel
    {
        // Lookup data for dropdowns
        Departments = await _departmentService.GetDepartmentComboboxItems()
    };
    return PartialView("_CreateModal", viewModel);
}
```

### Pattern 3: File Download
```csharp
public async Task<FileResult> DownloadFile(Guid id)
{
    var file = await _service.GetFileById(id);
    var fileBytes = _tempFileCacheManager.GetFile(file.FileToken, AbpSession.UserId, AbpSession.TenantId);
    return File(fileBytes, file.FileType, file.FileName);
}
```

### Pattern 4: Background Job Queue
```csharp
[HttpPost]
public async Task<JsonResult> ProcessData(ProcessInput input)
{
    var binaryObject = await SaveInputToStorage(input);

    await _backgroundJobManager.EnqueueAsync<ProcessJob, ProcessJobArgs>(
        new ProcessJobArgs { BinaryObjectId = binaryObject.Id }
    );

    return Json(new { success = true });
}
```

## Verification Checklist

- [ ] Controller action is under 15 lines
- [ ] No direct repository access
- [ ] Business logic delegated to service
- [ ] Only coordination code in controller
- [ ] Authorization via `[AbpMvcAuthorize]`
- [ ] Returns appropriate type (View, PartialView, FileResult, JsonResult)
- [ ] Error handling with UserFriendlyException
- [ ] Uses localization L() for messages

## Reference Files
- `src/inzibackend.Web.Mvc/Areas/App/Controllers/TenantsController.cs`
- `src/inzibackend.Web.Mvc/Areas/App/Controllers/UsersController.cs`
- `src/inzibackend.Web.Core/Controllers/inzibackendControllerBase.cs` - Base class
- `src/inzibackend.Web.Core/Controllers/FileController.cs` - File operations
