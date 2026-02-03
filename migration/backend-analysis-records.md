# Backend Analysis: Records Module (WS 3.1.3)

**Analysis Date:** 2026-02-03
**Analyst:** Compliance Migration Developer
**Status:** Ready for Backend Validator Review

---

## Overview

The Records module manages document uploads with binary file storage. It's the foundation for compliance document tracking with file management, expiration dates, and category associations.

**Complexity:** HIGH - First module with binary file upload/download
**Backend LOC:** 504 lines (RecordsAppService)
**Key Dependencies:** BinaryObjectManager, TempFileCacheManager, FileController

---

## Backend Architecture

### AppService: RecordsAppService

**Location:** `surpath112/src/inzibackend.Application/Surpath/RecordsAppService.cs`
**Interface:** `IRecordsAppService` (9 methods)
**Permissions:** `AppPermissions.Pages_Records`

#### Methods Analysis

| Method | Purpose | HTTP Method | Complexity | Notes |
|--------|---------|-------------|------------|-------|
| `GetAll` | Paginated list with filters | GET | Medium | Joins TenantDocumentCategory, fetches file names |
| `GetRecordForView` | Single record view | GET | Low | Includes lookup, file name |
| `GetRecordForEdit` | Get for editing | GET | Low | Includes lookup, file name |
| `CreateOrEdit` | Create/Update record | POST | HIGH | Handles file upload via token |
| `Create` | Create new record | POST | HIGH | Creates BinaryObject, TenantDocument |
| `Update` | Update existing | PUT | HIGH | Updates BinaryObject |
| `Delete` | Delete record | DELETE | Low | Standard deletion |
| `GetRecordsToExcel` | Export to Excel | GET | Medium | Filtered export with row limit |
| `GetAllTenantDocumentCategoryForLookupTable` | Lookup modal | GET | Low | Paginated lookup |
| `RemovefiledataFile` | Remove file only | DELETE | Medium | Deletes from BinaryObjectManager |

#### Critical Business Logic

**File Upload Flow:**
```csharp
// 1. Frontend uploads file to RecordsController.UploadfiledataFile
// 2. Controller validates size/extension, stores in TempFileCacheManager
// 3. Returns fileToken (Guid) to frontend
// 4. Frontend passes fileToken in CreateOrEditRecordDto
// 5. AppService.CreateOrEdit calls GetBinaryObjectFromCache(fileToken)
// 6. Creates BinaryObject with tenant isolation
// 7. Stores file in file system via BinaryObjectManager
// 8. Record.filedata = BinaryObject.Id (reference)
```

**Key Code Snippet (Create method):**
```csharp
var record = ObjectMapper.Map<Record>(input);
var storedFile = await GetBinaryObjectFromCache(input.filedataToken);
record.filedata = storedFile.Id;
record.filename = input.filename;
record.metadata = storedFile.Metadata;
record.physicalfilepath = storedFile.FileName;
record.BinaryObjId = storedFile.Id;
await _recordRepository.InsertAsync(record);
RecordCreateUpdatePostProcess(record); // Creates TenantDocument
```

**Post-Processing:**
- Creates `TenantDocument` entity when `TenantDocumentCategoryId` is set
- Links Record → TenantDocument for document tracking

**Tenant Isolation:**
- BinaryObject created with `AbpSession.TenantId`
- Files stored in: `{SurpathRecordsRootFolder}/{tenantId}/{userId}/`
- Folder auto-created via `GetDestFolder()`

---

### Controllers

#### RecordsController (MVC)

**Location:** `surpath112/src/inzibackend.Web.Mvc/Areas/App/Controllers/RecordsController.cs`

**Key Method:** `UploadfiledataFile()`
```csharp
[AbpMvcAuthorize(AppPermissions.Pages_RecordStates)]
public FileUploadCacheOutput UploadfiledataFile()
{
    // 1. Validate file count
    if (Request.Form.Files.Count == 0) throw new UserFriendlyException(L("NoFileFoundError"));

    // 2. Validate file size
    var file = Request.Form.Files.First();
    if (file.Length > SurpathSettings.MaxfiledataLength)
        throw new UserFriendlyException(L("Warn_File_SizeLimit") + ...);

    // 3. Validate file extension
    var fileType = Path.GetExtension(file.FileName).Substring(1);
    if (!filedataAllowedFileTypes.Contains(fileType))
        throw new UserFriendlyException(L("FileNotInAllowedFileTypes") + ...);

    // 4. Store in temporary cache
    var fileToken = Guid.NewGuid().ToString("N");
    _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));

    // 5. Return token to frontend
    return new FileUploadCacheOutput(fileToken);
}
```

**File Settings:**
- `SurpathSettings.MaxfiledataLength` - Max file size
- `SurpathSettings.AllowedFileExtensionsArray` - Allowed types
- Defined in SurpathSettings class

#### FileController (Web.Core)

**Location:** `surpath112/src/inzibackend.Web.Core/Controllers/FileController.cs`

**Download Method:** `DownloadBinaryFile(Guid id, string contentType, string fileName)`
- Checks user access via `SurpathManager.UserAccessToFile`
- Retrieves file from `BinaryObjectManager`
- Returns file with proper content type and filename
- Supports dummy documents in development

**View Method:** `ViewBinaryFile(Guid id, string contentType, string fileName)`
- Same as download but sets `Content-Disposition: inline`
- For in-browser viewing (PDFs, images)

---

## DTOs

### Core DTOs

**RecordDto** (View)
```csharp
public class RecordDto : EntityDto<Guid>
{
    public Guid? filedata { get; set; }           // BinaryObject ID
    public string filedataFileName { get; set; }   // Display name (computed)
    public string filename { get; set; }           // Original filename
    public string physicalfilepath { get; set; }   // File system path
    public string metadata { get; set; }           // JSON metadata
    public Guid BinaryObjId { get; set; }         // BinaryObject reference
    public DateTime? DateUploaded { get; set; }
    public DateTime? DateLastUpdated { get; set; }
    public bool InstructionsConfirmed { get; set; } // Checkbox
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public Guid? TenantDocumentCategoryId { get; set; }
}
```

**CreateOrEditRecordDto** (Save)
```csharp
public class CreateOrEditRecordDto : EntityDto<Guid?>
{
    public Guid? filedata { get; set; }
    public string filedataToken { get; set; }  // ⚠️ Critical: File upload token
    public string filename { get; set; }
    // ... same fields as RecordDto
}
```

**GetRecordForViewDto** (List)
```csharp
public class GetRecordForViewDto
{
    public RecordDto Record { get; set; }
    public string TenantDocumentCategoryName { get; set; } // Lookup name
}
```

**GetRecordForEditOutput** (Edit)
```csharp
public class GetRecordForEditOutput
{
    public CreateOrEditRecordDto Record { get; set; }
    public string TenantDocumentCategoryName { get; set; }
    public string filedataFileName { get; set; } // Current file name
}
```

### Input DTOs

**GetAllRecordsInput** (List filters)
```csharp
public class GetAllRecordsInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
    public string filenameFilter { get; set; }
    public string physicalfilepathFilter { get; set; }
    public string metadataFilter { get; set; }
    public Guid? BinaryObjIdFilter { get; set; }
    public DateTime? MinDateUploadedFilter { get; set; }
    public DateTime? MaxDateUploadedFilter { get; set; }
    public DateTime? MinDateLastUpdatedFilter { get; set; }
    public DateTime? MaxDateLastUpdatedFilter { get; set; }
    public int? InstructionsConfirmedFilter { get; set; }
    public DateTime? MinEffectiveDateFilter { get; set; }
    public DateTime? MaxEffectiveDateFilter { get; set; }
    public DateTime? MinExpirationDateFilter { get; set; }
    public DateTime? MaxExpirationDateFilter { get; set; }
    public string TenantDocumentCategoryNameFilter { get; set; }
}
```

---

## Domain Entity

**Record Entity**

**Location:** `surpath112/src/inzibackend.Core/Surpath/Record.cs`

```csharp
[Table("Records")]
[Audited]
public class Record : FullAuditedEntity<Guid>, IMayHaveTenant
{
    public int? TenantId { get; set; }

    // File fields
    public virtual Guid? filedata { get; set; }        // BinaryObject ID
    public virtual string filename { get; set; }       // Display name
    public virtual string physicalfilepath { get; set; } // File path
    public virtual string metadata { get; set; }       // JSON metadata
    public virtual Guid BinaryObjId { get; set; }      // BinaryObject reference

    // Timestamps
    public virtual DateTime? DateUploaded { get; set; }
    public virtual DateTime? DateLastUpdated { get; set; }

    // Flags
    public virtual bool InstructionsConfirmed { get; set; }

    // Dates
    public virtual DateTime? EffectiveDate { get; set; }
    public virtual DateTime? ExpirationDate { get; set; }

    // Foreign key
    public virtual Guid? TenantDocumentCategoryId { get; set; }

    [ForeignKey("TenantDocumentCategoryId")]
    public TenantDocumentCategory TenantDocumentCategoryFk { get; set; }
}
```

**Audit Trail:**
- Inherits `FullAuditedEntity<Guid>` (CreatedBy, CreatedTime, LastModifiedBy, etc.)
- `[Audited]` attribute enables change tracking

---

## Dependencies

### External Module Dependencies

1. **TenantDocumentCategory** (WS 3.2 - Not yet migrated)
   - `Record.TenantDocumentCategoryId` → `TenantDocumentCategory.Id`
   - Lookup modal for category selection
   - **Migration Strategy:** Use placeholder lookup modal until WS 3.2 complete

2. **BinaryObjectManager** (Framework - Already exists)
   - Interface: `IBinaryObjectManager`
   - Location: `inzibackend.Storage`
   - Used for file storage/retrieval

3. **TempFileCacheManager** (Framework - Already exists)
   - Interface: `ITempFileCacheManager`
   - Temporary file storage before commit

4. **TenantDocument** (Created in post-processing)
   - Links Record to TenantDocumentCategory
   - Auto-created by `RecordCreateUpdatePostProcess`

---

## Frontend Implementation (Surpath112)

### Views

1. **Index.cshtml** - List view
2. **_CreateOrEditModal.cshtml** - Create/edit modal
3. **_ViewRecordModal.cshtml** - View details modal
4. **_RecordTenantDocumentCategoryLookupTableModal.cshtml** - Category lookup

### JavaScript (jQuery)

**_CreateOrEditModal.js** - Key patterns:
```javascript
// File upload on change
$('#Record_filedata').change(function () {
    var file = $(this)[0].files[0];
    var formData = new FormData();
    formData.append('file', file);

    $.ajax({
        url: '/App/Records/UploadfiledataFile',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
    }).done(function (resp) {
        _filedataToken = resp.result.fileToken; // Store token
    });
});

// Save with file token
this.save = function () {
    var record = _$recordInformationForm.serializeFormToObject();
    record.filedataToken = _filedataToken; // Pass token
    _recordsService.createOrEdit(record);
};

// Remove file
$('#Record_filedata_Remove').click(function () {
    _recordsService.removefiledataFile({ id: Record.id });
});
```

**File Download Link:**
```html
<a href="/File/DownloadBinaryFile?id=@Model.Record.filedata">
    @Model.filedataFileName
</a>
```

---

## React Migration Plan

### Component Structure

```
surpath200/react/src/pages/compliance/records/
├── index.tsx                          # Main list page
├── components/
│   ├── CreateOrEditRecordModal.tsx   # Create/edit modal
│   ├── ViewRecordModal.tsx           # View details modal
│   ├── RecordTable.tsx               # Ant Design Table
│   ├── RecordFilters.tsx             # Filter form
│   └── TenantDocumentCategoryLookup.tsx # Category lookup (placeholder)
└── README.md                          # Module documentation
```

### File Upload Implementation

**Pattern (based on ChangeProfilePictureModal.tsx):**

```tsx
// State
const [fileToken, setFileToken] = useState<string>('');
const [uploading, setUploading] = useState(false);

// Ant Design Upload config
const uploadProps: UploadProps = {
    name: 'file',
    action: `${AppConsts.remoteServiceBaseUrl}/App/Records/UploadfiledataFile`,
    headers: { Authorization: `Bearer ${token}` },
    showUploadList: false,
    accept: SurpathSettings.AllowedFileExtensions,
    beforeUpload: (file) => {
        // Validate size
        if (file.size > SurpathSettings.MaxfiledataLength) {
            message.error('File too large');
            return Upload.LIST_IGNORE;
        }
        setUploading(true);
        return true; // Proceed with upload
    },
    onChange: (info) => {
        if (info.file.status === 'done') {
            setFileToken(info.file.response.result.fileToken);
            form.setFieldValue('filename', info.file.name);
            setUploading(false);
        } else if (info.file.status === 'error') {
            message.error('Upload failed');
            setUploading(false);
        }
    }
};

// Save with fileToken
const handleSave = async (values: CreateOrEditRecordDto) => {
    values.filedataToken = fileToken; // Pass token
    await recordsService.createOrEdit(values);
};
```

**File Download Link:**
```tsx
{record.filedata && (
    <a
        href={`/File/DownloadBinaryFile?id=${record.filedata}`}
        target="_blank"
        rel="noopener noreferrer"
    >
        {record.filedataFileName || 'Download'}
    </a>
)}
```

---

## Testing Requirements

### Unit Tests (Backend)
- ✅ Already exist in `surpath112/test/inzibackend.Tests/Surpath/RecordsAppServiceTest.cs`

### A/B Testing Scenarios

1. **File Upload**
   - Upload various file types (PDF, JPEG, PNG)
   - Validate size limits
   - Validate extension restrictions
   - Verify file stored correctly

2. **File Download**
   - Download file via link
   - View file inline (PDFs)
   - Verify permissions enforced

3. **CRUD Operations**
   - Create record with file
   - Edit record, replace file
   - Delete record (file preserved)
   - Remove file only (RemovefiledataFile)

4. **Filters**
   - Filter by filename
   - Filter by date ranges
   - Filter by InstructionsConfirmed
   - Filter by TenantDocumentCategory

5. **Expiration Tracking**
   - Set effective date
   - Set expiration date
   - Visual indicators for expired documents

---

## Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| File upload fails silently | HIGH | Show upload progress, error messages |
| Large files timeout | MEDIUM | Frontend validation, show progress bar |
| TenantDocumentCategory not migrated | HIGH | Use placeholder lookup, defer functionality to WS 3.2 |
| File permissions not enforced | CRITICAL | Use existing FileController.CheckAccessToFile |
| BinaryObject cleanup on delete | MEDIUM | Document behavior (files preserved) |

---

## Backend Validation Checklist

- [ ] All 9 AppService methods mapped correctly
- [ ] File upload flow understood (TempFileCache → BinaryObject)
- [ ] File download endpoints identified (DownloadBinaryFile, ViewBinaryFile)
- [ ] Permission attributes correct (`AppPermissions.Pages_Records`)
- [ ] DTOs match entity structure
- [ ] Tenant isolation verified
- [ ] TenantDocumentCategory dependency documented
- [ ] Post-processing logic (TenantDocument creation) understood
- [ ] File validation rules documented (size, extensions)
- [ ] Audit trail confirmed (FullAuditedEntity)

---

## Next Steps

1. **Backend Validator:** Review this analysis
2. **Compliance Migration Developer:** Implement React components after approval
3. **QA Engineer:** Execute A/B testing scenarios
4. **Documentation Specialist:** Update any missing patterns

---

## Questions for Backend Validator

1. Is the file upload flow (TempFileCache → BinaryObject) correctly understood?
2. Should we preserve existing file when updating a Record without changing the file?
3. How should we handle TenantDocumentCategory lookup before WS 3.2 is complete?
4. Are there any additional permissions needed beyond `Pages_Records`?
5. Should file deletion (RemovefiledataFile) also delete the BinaryObject, or just unlink it?

---

**Analysis Complete - Ready for Backend Validation** ✅
