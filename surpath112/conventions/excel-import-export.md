# Excel Import/Export Convention (ASP.NET Zero MVC + jQuery)

## Purpose
Standard pattern for bulk data import/export functionality using NPOI Excel library with background job processing and proper service layer separation.

**Key Classes**:
- **Exporting**: Uses `NpoiExcelExporterBase` class
- **Importing**: Uses `NpoiExcelImporterBase` class
- **Background Processing**: Uses `AsyncBackgroundJob<TArgs>` for imports

**Reference Examples**:
- `UserListExcelExporter` - Standard user export pattern
- `UserListExcelDataReader` - Standard Excel reading pattern
- `ImportUsersFromExcelJobArgs` - Background job arguments pattern

## When to Use
- Bulk user/entity imports from Excel files
- Data export to Excel for editing and re-import
- Any feature requiring Excel file upload with background processing
- Error reporting with invalid record exports

## Key Components

### Required Files (per entity)

1. **Job Arguments DTO** - `*.Application.Shared/[Feature]/Dto/Import[Entity]FromExcelJobArgs.cs`
   - Properties: TenantId (or other context), BinaryObjectId, User (UserIdentifier)

2. **Import DTO** - `*.Application/[Feature]/Importing/Dto/Import[Entity]Dto.cs`
   - All entity fields
   - Exception property for error tracking
   - CanBeImported() method

3. **Export DTO** - `*.Application/[Feature]/Importing/Dto/[Entity]ExportDto.cs`
   - Same fields as Import DTO (for symmetry)
   - Used for exporting existing data

4. **Excel Reader Interface** - `*.Application/[Feature]/Importing/I[Entity]ListExcelDataReader.cs`
   - Single method: `List<Import[Entity]Dto> Get[Entity]sFromExcel(byte[] fileBytes)`

5. **Excel Reader Implementation** - `*.Application/[Feature]/Importing/[Entity]ListExcelDataReader.cs`
   - Extends `NpoiExcelImporterBase<Import[Entity]Dto>`
   - Implements column reading logic with validation

6. **Exporter Interface** - `*.Application/[Feature]/Importing/I[Entity]Exporter.cs`
   - Single method: `FileDto ExportToFile(List<[Entity]ExportDto> items)`

7. **Exporter Implementation** - `*.Application/[Feature]/Importing/[Entity]Exporter.cs`
   - Extends `NpoiExcelExporterBase`
   - Uses `CreateExcelPackage`, `AddHeader`, `AddObjects`
   - Auto-sizes columns

8. **Invalid Exporter Interface** - `*.Application/[Feature]/Importing/IInvalid[Entity]Exporter.cs`
   - For exporting failed imports with error messages

9. **Invalid Exporter Implementation** - `*.Application/[Feature]/Importing/Invalid[Entity]Exporter.cs`
   - Includes all data columns + "Refuse Reason" column

10. **Background Job** - `*.Application/[Feature]/Importing/Import[Entity]ToExcelJob.cs`
    - Extends `AsyncBackgroundJob<Import[Entity]FromExcelJobArgs>`
    - Implements `ITransientDependency`

11. **App Service Method** - Add to existing `[Feature]AppService.cs`
    - `Task<FileDto> Get[Entity]sToExcel(GetAll[Entity]Input input)`
    - Includes `[AbpAuthorize]` attribute

12. **Controller Actions** - Add to existing `[Feature]Controller.cs`
    - Import modal action (returns PartialView)
    - Import processing action (file upload → BinaryObject → background job)
    - Export action (calls service → download file)

13. **View Model** - `*.Web.Mvc/Areas/App/Models/[Feature]/Import[Entity]ViewModel.cs`

14. **Modal View** - `*.Web.Mvc/Areas/App/Views/[Feature]/_ImportModal.cshtml`

15. **Modal JavaScript** - `*.Web.Mvc/wwwroot/view-resources/Areas/App/Views/[Feature]/_ImportModal.js`

## Pattern Structure

### Import Flow
```
User uploads Excel → Controller validates → Save to BinaryObjectManager
                                         ↓
                     Queue background job with BinaryObjectId
                                         ↓
         Background job: Retrieve file → Parse Excel → Validate each row
                                         ↓
              Create entities (separate UoW per entity for isolation)
                                         ↓
        Export invalid rows → Send notification with download link
```

### Export Flow
```
jQuery: abp.services.app.[service].get[Entity]sToExcel()
                    ↓
    App Service: Query entities → Build DTOs → Call Exporter
                    ↓
        Exporter: CreateExcelPackage → AddHeader → AddObjects
                    ↓
            Return FileDto with token
                    ↓
jQuery: app.downloadTempFile(result) → FileController.DownloadTempFile
```

## Code Examples

### Interface
```csharp
public interface ITenantUserExporter
{
    FileDto ExportToFile(List<TenantUserExportDto> users);
}
```

### Exporter Implementation
```csharp
public class TenantUserExporter : NpoiExcelExporterBase, ITenantUserExporter
{
    public TenantUserExporter(ITempFileCacheManager tempFileCacheManager)
        : base(tempFileCacheManager)
    {
    }

    public FileDto ExportToFile(List<TenantUserExportDto> users)
    {
        return CreateExcelPackage(
            "TenantUsers.xlsx",
            excelPackage =>
            {
                var sheet = excelPackage.CreateSheet(L("Users"));

                AddHeader(
                    sheet,
                    L("UserName"),
                    L("Name")
                    // ... more columns
                );

                AddObjects(
                    sheet, users,
                    _ => _.UserName,
                    _ => _.Name
                    // ... more fields (SAME ORDER as header)
                );

                // Auto-size all columns
                for (var i = 0; i < columnCount; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
            });
    }
}
```

### Excel Reader Implementation
```csharp
public class TenantUserListExcelDataReader : NpoiExcelImporterBase<ImportTenantUserDto>, ITenantUserListExcelDataReader
{
    private readonly ILocalizationSource _localizationSource;

    public TenantUserListExcelDataReader(ILocalizationManager localizationManager)
    {
        _localizationSource = localizationManager.GetSource(inzibackendConsts.LocalizationSourceName);
    }

    public List<ImportTenantUserDto> GetTenantUsersFromExcel(byte[] fileBytes)
    {
        return ProcessExcelFile(fileBytes, ProcessExcelRow);
    }

    private ImportTenantUserDto ProcessExcelRow(ISheet worksheet, int row)
    {
        if (IsRowEmpty(worksheet, row))
            return null;

        var exceptionMessage = new StringBuilder();
        var user = new ImportTenantUserDto();

        try
        {
            user.UserName = GetRequiredValueFromRowOrNull(worksheet, row, 0, nameof(user.UserName), exceptionMessage);
            user.Name = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(user.Name), exceptionMessage);
            // ... more columns

            if (exceptionMessage.Length > 0)
                user.Exception = exceptionMessage.ToString();
        }
        catch (Exception exception)
        {
            user.Exception = exception.Message;
        }

        return user;
    }
}
```

### Background Job
```csharp
public class ImportTenantUsersToExcelJob : AsyncBackgroundJob<ImportTenantUsersFromExcelJobArgs>, ITransientDependency
{
    private readonly ITenantUserListExcelDataReader _excelDataReader;
    private readonly IInvalidTenantUserExporter _invalidExporter;
    private readonly IBinaryObjectManager _binaryObjectManager;
    private readonly IAppNotifier _appNotifier;
    // ... other dependencies

    public override async Task ExecuteAsync(ImportTenantUsersFromExcelJobArgs args)
    {
        var entities = await GetEntityListFromExcelOrNullAsync(args);
        if (entities == null || !entities.Any())
        {
            await SendInvalidExcelNotificationAsync(args);
            return;
        }

        await CreateEntitiesAsync(args, entities);
    }

    private async Task CreateEntitiesAsync(ImportTenantUsersFromExcelJobArgs args, List<ImportTenantUserDto> entities)
    {
        var invalidEntities = new List<ImportTenantUserDto>();

        foreach (var entity in entities)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(args.TenantId))
                {
                    if (entity.CanBeImported())
                    {
                        try
                        {
                            await CreateEntityAsync(entity);
                        }
                        catch (Exception exception)
                        {
                            entity.Exception = exception.Message;
                            invalidEntities.Add(entity);
                        }
                    }
                    else
                    {
                        invalidEntities.Add(entity);
                    }
                }
                await uow.CompleteAsync();
            }
        }

        await ProcessImportResultAsync(args, invalidEntities);
    }
}
```

### App Service Export Method
```csharp
[AbpAuthorize(AppPermissions.Pages_[Feature]_Export)]
public async Task<FileDto> Get[Entity]sToExcel(GetAll[Entity]Input input)
{
    // Disable tenant filter if host querying tenant data
    if (AbpSession.TenantId == null)
        CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

    // Efficient query with joins (avoid N+1)
    var query = from entity in _repository.GetAll()
                // ... joins for related data
                select new { ... };

    var results = await query.ToListAsync();

    // Map to export DTOs
    var exportDtos = results.Select(r => new ExportDto { ... }).ToList();

    return _exporter.ExportToFile(exportDtos);
}
```

### Controller Actions
```csharp
// Import Modal
[AbpMvcAuthorize(AppPermissions.Pages_[Feature]_Import)]
public PartialViewResult ImportModal(int id)
{
    return PartialView("_ImportModal", new ImportViewModel { Id = id });
}

// Import Upload
[AbpMvcAuthorize(AppPermissions.Pages_[Feature]_Import)]
[HttpPost]
public async Task<JsonResult> ImportFromExcel(int id, IFormFile file)
{
    // Validate file
    var fileBytes = /* read file */;
    var binaryObject = new BinaryObject(tenantId, fileBytes, "Import_" + Guid.NewGuid());
    await _binaryObjectManager.SaveAsync(binaryObject);

    // Queue background job
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

// Export Download
[AbpMvcAuthorize(AppPermissions.Pages_[Feature]_Export)]
public async Task<FileResult> Export(int id)
{
    var file = await _appService.Get[Entity]sToExcel(new EntityDto(id));

    var fileBytes = _tempFileCacheManager.GetFile(file.FileToken, AbpSession.UserId, AbpSession.TenantId);
    if (fileBytes == null)
        throw new UserFriendlyException(L("RequestedFileDoesNotExists"));

    return File(fileBytes, file.FileType, file.FileName);
}
```

### jQuery Service Proxy Usage
```javascript
var _service = abp.services.app.[serviceName];

// Export button
$('#ExportButton').click(function () {
    _service
        .get[Entity]sToExcel({ id: _id })
        .done(function (result) {
            app.downloadTempFile(result);
        });
});

// Import button
$('#ImportButton').click(function () {
    var fileInput = $('#FileInput')[0];
    if (!fileInput.files || !fileInput.files.length) {
        abp.message.warn(app.localize('PleaseSelectAFile'));
        return;
    }

    var formData = new FormData();
    formData.append('id', _id);
    formData.append('file', fileInput.files[0]);

    _modalManager.setBusy(true);

    abp.ajax({
        url: abp.appPath + 'App/[Controller]/ImportFromExcel',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false
    }).done(function (result) {
        if (result.success) {
            abp.notify.info(app.localize('ImportProcessStart'));
            _modalManager.close();
        }
    }).always(function () {
        _modalManager.setBusy(false);
    });
});
```

## Reference Implementations

### User Import/Export (Standard Pattern)
- **Reader**: `src/inzibackend.Application/Authorization/Users/Importing/UserListExcelDataReader.cs`
- **Exporter**: `src/inzibackend.Application/Authorization/Users/Exporting/UserListExcelExporter.cs`
- **Invalid Exporter**: `src/inzibackend.Application/Authorization/Users/Importing/InvalidUserExporter.cs`
- **Background Job**: `src/inzibackend.Application/Authorization/Users/Importing/ImportUsersToExcelJob.cs`
- **Service Method**: `src/inzibackend.Application/Authorization/Users/UserAppService.cs:129` - `GetUsersToExcel()`

### Tenant User Import/Export (Extended Pattern with Associations)
- **Reader**: `src/inzibackend.Application/MultiTenancy/Importing/TenantUserListExcelDataReader.cs`
- **Exporter**: `src/inzibackend.Application/MultiTenancy/Importing/TenantUserExporter.cs`
- **Invalid Exporter**: `src/inzibackend.Application/MultiTenancy/Importing/InvalidTenantUserExporter.cs`
- **Background Job**: `src/inzibackend.Application/MultiTenancy/Importing/ImportTenantUsersToExcelJob.cs`
- **Service Method**: `src/inzibackend.Application/MultiTenancy/TenantAppService.cs:849` - `GetTenantUsersToExcel()`
- **Controller**: `src/inzibackend.Web.Mvc/Areas/App/Controllers/TenantsController.cs:173,184,202`
- **Modal**: `src/inzibackend.Web.Mvc/Areas/App/Views/Tenants/_ImportUsersModal.cshtml`
- **JavaScript**: `src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/Tenants/_ImportUsersModal.js`

### Cohort User Export
- **Service Method**: `src/inzibackend.Application/Surpath/CohortUsersAppService.cs:523` - `GetCohortUsersToExcel()`
- **Exporter**: `src/inzibackend.Application/Surpath/Exporting/CohortUsersExcelExporter.cs`
- **JavaScript**: `src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/CohortUsers/Index.js:361`

## Common Mistakes to Avoid

### ❌ Controller Contains Business Logic
```csharp
// WRONG - Business logic in controller
public async Task<FileResult> Export(int id)
{
    var users = await _userRepository.GetAll().Where(...).ToListAsync();
    // ... complex mapping logic
    var file = _exporter.ExportToFile(mapped);
    return File(...);
}
```

### ✅ Thin Controller, Logic in Service
```csharp
// CORRECT - Controller delegates to service
public async Task<FileResult> Export(int id)
{
    var file = await _appService.Get[Entity]sToExcel(new EntityDto(id));
    var fileBytes = _tempFileCacheManager.GetFile(file.FileToken, ...);
    return File(fileBytes, file.FileType, file.FileName);
}
```

### ❌ jQuery Calls Controller Directly
```javascript
// WRONG - Direct controller call bypasses service layer security
$('#ExportButton').click(function () {
    window.location.href = '/App/Controller/Export?id=' + id;
});
```

### ✅ jQuery Uses Service Proxy
```javascript
// CORRECT - Service proxy applies authorization, validation, etc.
$('#ExportButton').click(function () {
    _service.get[Entity]sToExcel({ id: id })
        .done(function (result) {
            app.downloadTempFile(result);
        });
});
```

### ❌ Mismatched Column Order
```csharp
// WRONG - Import expects column 0 = Name, Export puts Name in column 1
// Import Reader
user.Name = GetRequiredValueFromRowOrNull(worksheet, row, 0, ...);
user.UserName = GetRequiredValueFromRowOrNull(worksheet, row, 1, ...);

// Export
AddHeader(sheet, L("UserName"), L("Name"), ...);
AddObjects(sheet, users, _ => _.UserName, _ => _.Name, ...);
```

### ✅ Identical Column Order
```csharp
// CORRECT - Same order in import and export
// Import Reader (columns 0, 1, 2)
user.UserName = GetRequiredValueFromRowOrNull(worksheet, row, 0, ...);
user.Name = GetRequiredValueFromRowOrNull(worksheet, row, 1, ...);
user.Surname = GetRequiredValueFromRowOrNull(worksheet, row, 2, ...);

// Export (columns 0, 1, 2)
AddHeader(sheet, L("UserName"), L("Name"), L("Surname"), ...);
AddObjects(sheet, users, _ => _.UserName, _ => _.Name, _ => _.Surname, ...);
```

### ❌ N+1 Query Problem
```csharp
// WRONG - Queries department for each user (N+1 problem)
foreach (var user in users)
{
    var dept = await _deptRepository.FirstOrDefaultAsync(d => d.Id == user.DeptId);
    exportDto.DeptName = dept?.Name;
}
```

### ✅ Efficient Query with Joins
```csharp
// CORRECT - Single query with LEFT JOIN
var query = from user in _userRepository.GetAll()
            join dept in _deptRepository.GetAll()
                on user.DeptId equals dept.Id into deptJoin
            from d in deptJoin.DefaultIfEmpty()
            select new { User = user, DeptName = d.Name };
```

### ❌ Missing Tenant Filter Disable
```csharp
// WRONG - Host can't query tenant data
public async Task<FileDto> GetTenantUsersToExcel(EntityDto input)
{
    var users = await _userRepository.GetAll()
        .Where(u => u.TenantId == input.Id)
        .ToListAsync();
    // Returns empty - tenant filter blocks host from seeing tenant data
}
```

### ✅ Disable Filter for Host Context
```csharp
// CORRECT - Disable tenant filter when host queries tenant data
public async Task<FileDto> GetTenantUsersToExcel(EntityDto input)
{
    if (AbpSession.TenantId == null)
        CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

    var users = await _userRepository.GetAll()
        .Where(u => u.TenantId == input.Id)
        .ToListAsync();
    // Works correctly
}
```

### ❌ Duplicate Data in Export
```csharp
// WRONG - User has duplicate roles: "Admin,User,User,User..."
var roleNames = user.Roles.Select(r => r.RoleName);
exportDto.Roles = string.Join(",", roleNames);
```

### ✅ Distinct Values
```csharp
// CORRECT - Remove duplicates
var roleNames = user.Roles.Select(r => r.RoleName).Distinct();
exportDto.Roles = string.Join(",", roleNames);
```

## Verification Checklist

### Import Implementation
- [ ] Import DTO extends base class or has Exception property and CanBeImported()
- [ ] Excel reader extends `NpoiExcelImporterBase<T>`
- [ ] Background job extends `AsyncBackgroundJob<TArgs>`
- [ ] Job args include TenantId/context, BinaryObjectId, User
- [ ] Each entity created in separate UoW (fault isolation)
- [ ] Invalid entities exported with error messages
- [ ] Notification sent on completion
- [ ] Column order documented

### Export Implementation
- [ ] Exporter extends `NpoiExcelExporterBase`
- [ ] Interface has single method: `FileDto ExportToFile(List<TDto>)`
- [ ] Uses `CreateExcelPackage`, `AddHeader`, `AddObjects`
- [ ] Auto-sizes columns (except very wide columns)
- [ ] App service method has `[AbpAuthorize]` attribute
- [ ] App service disables tenant filter if needed (host context)
- [ ] Controller is thin (calls service, downloads file)
- [ ] jQuery uses service proxy (`abp.services.app.*`)
- [ ] Export column order matches import column order exactly

### Security & Permissions
- [ ] Permission defined in `AppPermissions.cs`
- [ ] Permission registered in `AppAuthorizationProvider.cs` with correct `multiTenancySides`
- [ ] Service method has `[AbpAuthorize]` attribute
- [ ] Controller actions have `[AbpMvcAuthorize]` attribute
- [ ] jQuery checks permission: `abp.auth.hasPermission('...')`

### User Experience
- [ ] Import modal has export button (download existing data)
- [ ] Import modal has file input with accept=".xlsx,.xls"
- [ ] Clear instructions in modal description
- [ ] Success notification when job queued
- [ ] Error notification with download link for invalid records
- [ ] Localization strings added for all UI text

## Performance Considerations
- Use LINQ joins instead of nested queries (avoid N+1)
- Load related data dictionaries once, not per-entity
- Background job for imports (don't block UI)
- Separate UoW per entity (prevents transaction size issues)
- Auto-size only relevant columns (skip very wide text columns)

## Multi-Tenancy Considerations
- Host querying tenant data: Disable `AbpDataFilters.MayHaveTenant`
- Tenant querying own data: Filter applied automatically
- Background job runs in correct tenant context via `SetTenantId()`
- BinaryObject saved with correct tenantId
- All repositories respect tenant isolation

## Notes
- Excel library: NPOI (cross-platform, no COM interop)
- File format: .xlsx (Excel 2007+)
- File download: Token-based via `ITempFileCacheManager` (secure, no direct paths)
- Authorization: Enforced at service layer, checked at UI layer
- Validation: Excel parsing errors vs. business rule errors (separate handling)
