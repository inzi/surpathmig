# Sample Files Convention (ASP.NET Zero MVC + jQuery)

## Purpose
Provide downloadable Excel sample/template files for bulk import features to help users understand the expected format and column order.

## When to Use
- Any Excel import feature
- Bulk data upload functionality
- When users need to see the exact column structure expected
- To reduce import errors by providing a template

## Key Components

### File Locations
- **Storage**: `src/inzibackend.Web.Mvc/wwwroot/assets/SampleFiles/`
- **Naming**: `[Feature]SampleFile.xlsx` (e.g., `BulkImportSampleFile.xlsx`, `ImportUsersSampleFile.xlsx`)
- **Access**: Via `/assets/SampleFiles/[FileName].xlsx` URL path

### View Integration
Sample file download links appear in:
1. **Import modals** - Below the file upload input
2. **Index pages** - In dropdown menus or help sections
3. **Help text** - Inline with instructions

## Pattern: Adding Sample File Link to Modal

### Step 1: Create/Update Excel Sample File
**Location**: `src/inzibackend.Web.Mvc/wwwroot/assets/SampleFiles/[Feature]SampleFile.xlsx`

**Contents**:
- Row 1: Column headers (matching localization keys)
- Rows 2-3: Example data showing correct format
- Include examples of optional fields (blank cells)
- Show date formats, comma-separated values, etc.

**Example for Bulk Import**:
```
UserName | Name  | Surname | EmailAddress      | ... | DepartmentName | CohortName
---------|-------|---------|-------------------|-----|----------------|------------
jdoe     | John  | Doe     | jdoe@example.com  | ... | Engineering    | Fall 2024
asmith   | Alice | Smith   | asmith@example.com| ... |                | Spring 2025
```

### Step 2: Add Localization String
**File**: `src/inzibackend.Core/Localization/inzibackend/inzibackend.xml`

Already exists (reuse):
```xml
<text name="ImportToExcelSampleFileDownloadInfo">{0} to download sample import file.</text>
<text name="ClickHere">Click here</text>
```

### Step 3: Add Link to Modal View
**File**: `src/inzibackend.Web.Mvc/Areas/App/Views/[Feature]/_ImportModal.cshtml`

Add below the file input field:
```html
<div class="form-group">
    <label>@L("ExcelFile")</label>
    <input id="ImportFileInput" type="file" accept=".xlsx,.xls" class="form-control" />
    <span class="form-text text-muted">@L("ExcelFileUpload_Hint")</span>
    <small class="form-text text-muted">
        @Html.Raw(L("ImportToExcelSampleFileDownloadInfo",
            "<a href='" + ApplicationPath + "assets/SampleFiles/[YourFile]SampleFile.xlsx'>" +
            L("ClickHere") +
            "</a>"))
    </small>
</div>
```

## Code Examples

### Example 1: Tenant User Import Modal
**File**: `src/inzibackend.Web.Mvc/Areas/App/Views/Tenants/_ImportUsersModal.cshtml:21-23`

```html
<small class="form-text text-muted">
    @Html.Raw(L("ImportToExcelSampleFileDownloadInfo",
        "<a href='" + ApplicationPath + "assets/SampleFiles/BulkImportSampleFile.xlsx'>" +
        L("ClickHere") +
        "</a>"))
</small>
```

### Example 2: User Administration Import
**File**: `src/inzibackend.Web.Mvc/Areas/App/Views/Users/Index.cshtml:41`

```html
<span class="dropdown-item-text text-muted pl-3">
    <small class="pl-2">
        @Html.Raw(L("ImportToExcelSampleFileDownloadInfo",
            "<a href='" + ApplicationPath + "assets/SampleFiles/ImportUsersSampleFile.xlsx'>" +
            L("ClickHere") +
            "</a>"))
    </small>
</span>
```

## Key Points

### URL Path Structure
```
ApplicationPath + "assets/SampleFiles/[FileName].xlsx"
```

- `ApplicationPath` - Razor variable for app root path
- `assets/` - Static files folder (maps to wwwroot/assets)
- `SampleFiles/` - Subfolder for sample/template files
- `[FileName].xlsx` - Your sample file name

### Localization Pattern
```csharp
@Html.Raw(L("ImportToExcelSampleFileDownloadInfo",
    "<a href='...'>" + L("ClickHere") + "</a>"))
```

- Uses `@Html.Raw()` to render HTML anchor tag
- First parameter: Localization key with `{0}` placeholder
- Second parameter: HTML anchor tag with link
- Inner text uses `L("ClickHere")` for localization

### File Placement
```
wwwroot/
  assets/
    SampleFiles/
      ImportUsersSampleFile.xlsx        ← User admin import
      BulkImportSampleFile.xlsx         ← Tenant user bulk import
      [YourFeature]SampleFile.xlsx      ← Your new feature
```

## Creating Sample Files

### Column Headers
Headers should use **localized names** matching your Excel exporter headers:
```csharp
// From TenantUserExporter.cs
AddHeader(
    sheet,
    L("UserName"),        // Column 0
    L("Name"),            // Column 1
    L("Surname"),         // Column 2
    // ... matches exactly
);
```

### Sample Data Rows
Include 2-3 example rows showing:
- ✅ Required fields populated
- ✅ Optional fields (some blank, some filled)
- ✅ Comma-separated values (roles: "Admin,User")
- ✅ Date formats (MM/dd/yyyy)
- ✅ Valid example data (realistic usernames, emails)

### Data Types to Demonstrate
- **Text**: Names, addresses
- **Email**: Valid format examples
- **Phone**: Format examples (with/without formatting)
- **Date**: MM/dd/yyyy format
- **Comma-separated**: Roles, tags, etc.
- **Optional**: Some cells blank
- **Lookup values**: Department/cohort names that exist in system

## Reference Implementations

### Sample Files
- `src/inzibackend.Web.Mvc/wwwroot/assets/SampleFiles/ImportUsersSampleFile.xlsx` - User import
- `src/inzibackend.Web.Mvc/wwwroot/assets/SampleFiles/BulkImportSampleFile.xlsx` - Tenant user bulk import

### View Usage
- `src/inzibackend.Web.Mvc/Areas/App/Views/Users/Index.cshtml:41` - Dropdown menu
- `src/inzibackend.Web.Mvc/Areas/App/Views/Tenants/_ImportUsersModal.cshtml:21-23` - Import modal

### Localization
- `src/inzibackend.Core/Localization/inzibackend/inzibackend.xml:1621` - ImportToExcelSampleFileDownloadInfo

## Common Mistakes to Avoid

### ❌ Wrong Column Order
```
Sample file has columns: Name, UserName, Email
Import expects: UserName, Name, Email
Result: Import fails with validation errors
```

### ✅ Match Import Column Order Exactly
```
Sample file columns: UserName, Name, Surname, Email, ...
Import reader columns: 0=UserName, 1=Name, 2=Surname, 3=Email, ...
Export headers: UserName, Name, Surname, Email, ...
Result: Perfect match, imports work first try
```

### ❌ Invalid Example Data
```
Email: test@test
Phone: 1234567890 (no area code separator)
Date: 1/1/2025 (ambiguous format)
```

### ✅ Realistic Example Data
```
Email: john.doe@example.com
Phone: (555) 123-4567 or 555-123-4567
Date: 01/15/2025 (MM/dd/yyyy)
Roles: Admin,User (comma-separated, no spaces)
```

### ❌ Hard-Coded Lookup Values
```
Department: Engineering
Cohort: Fall 2024
Result: Fails if tenant doesn't have these exact names
```

### ✅ Generic Example Names
```
Department: [DepartmentName]
Cohort: [CohortName]
Result: User understands to replace with their own values
```

Or use realistic names with a note:
```
Department: Engineering (replace with your department name)
Cohort: Fall 2024 (replace with your cohort name)
```

### ❌ Missing Optional Field Examples
```
All optional fields either all filled or all blank
Result: User doesn't know which fields are optional
```

### ✅ Show Optional Field Patterns
```
Row 1: All fields filled (shows full capability)
Row 2: Only required fields (shows minimum needed)
Row 3: Mix of optional fields (shows flexibility)
```

## Verification Checklist

### Sample File
- [ ] Created in `wwwroot/assets/SampleFiles/` directory
- [ ] Named `[Feature]SampleFile.xlsx`
- [ ] Column headers match exporter headers exactly
- [ ] Column order matches import reader exactly
- [ ] Contains 2-3 sample rows
- [ ] Shows required vs. optional fields clearly
- [ ] Uses realistic example data
- [ ] Demonstrates all data types (dates, comma-separated, etc.)
- [ ] File is committed to repository

### View Integration
- [ ] Link added to import modal below file input
- [ ] Uses `@Html.Raw()` for HTML rendering
- [ ] Uses `L("ImportToExcelSampleFileDownloadInfo", ...)` pattern
- [ ] Uses `ApplicationPath` for relative URL
- [ ] Inner link text uses `L("ClickHere")`
- [ ] Link tested in browser (downloads file)

### User Experience
- [ ] Link is visually clear and clickable
- [ ] Download starts immediately (no intermediate page)
- [ ] File opens correctly in Excel
- [ ] Sample data is helpful and realistic
- [ ] Instructions indicate what to replace

## Best Practices

### Sample File Naming
- Use descriptive names: `BulkImportSampleFile.xlsx` not `sample.xlsx`
- Include feature name for clarity
- Keep consistent with other sample files in the project

### Sample Data Quality
- Use realistic but fake data (john.doe@example.com, not production emails)
- Show variety (different name lengths, formats)
- Demonstrate edge cases if relevant (optional fields, special characters)
- Keep data professional and appropriate

### Maintenance
- Update sample files when column structure changes
- Keep in sync with import/export logic
- Test sample file actually imports successfully
- Document any special setup needed (departments must exist, etc.)

### Documentation in Modal
Place sample file link:
- ✅ Below file input (users see it before uploading)
- ✅ In help text or hint area
- ✅ With clear description ("download sample file", "see template", etc.)

## Notes

- Sample files are static resources (no server-side processing)
- They're committed to git (part of the project)
- Excel format: `.xlsx` (Excel 2007+) using NPOI library
- Users can download, edit, and re-upload
- Sample files should import successfully without modification (if minimal setup done)
