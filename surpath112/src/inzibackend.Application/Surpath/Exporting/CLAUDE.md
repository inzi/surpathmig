# Surpath Excel Exporters Documentation

## Overview
Comprehensive collection of 30+ Excel exporter services for all major Surpath entities. Each exporter converts domain DTOs to formatted Excel files using NPOI library, with localized headers, timezone conversion, and consistent formatting. Supports data export, reporting, and backup functionality.

## Contents

### Files

This folder contains 60+ files following a consistent pattern:
- **Interface files** (I*ExcelExporter.cs): Define export contracts
- **Implementation files** (*ExcelExporter.cs): NPOI-based implementations

### Entity Exporters

#### Core Compliance Entities
- **CohortsExcelExporter**: Student cohort groups with department associations
- **CohortUsersExcelExporter**: Individual students within cohorts
- **RecordsExcelExporter**: Compliance document records
- **RecordStatesExcelExporter**: Record approval states and statuses
- **RecordStatusesExcelExporter**: Available record statuses
- **RecordCategoriesExcelExporter**: Requirement categories
- **RecordRequirementsExcelExporter**: Specific requirement definitions
- **RecordCategoryRulesExcelExporter**: Compliance rules and logic
- **RecordNotesExcelExporter**: Annotations on records

#### Department & Organization
- **TenantDepartmentsExcelExporter**: Department definitions
- **TenantDepartmentUsersExcelExporter**: Department user assignments
- **DepartmentUsersExcelExporter**: Department membership
- **DeptCodesExcelExporter**: Department classification codes

#### Drug Testing
- **DrugsExcelExporter**: Drug types and testing parameters
- **DrugPanelsExcelExporter**: Drug testing panel configurations
- **DrugTestCategoriesExcelExporter**: Test categories
- **PanelsExcelExporter**: Testing panel definitions
- **TestCategoriesExcelExporter**: Test category management

#### Document Management
- **TenantDocumentsExcelExporter**: Document templates
- **TenantDocumentCategoriesExcelExporter**: Document categories
- **CodeTypesExcelExporter**: Document classification codes
- **ConfirmationValuesExcelExporter**: Confirmation value lists

#### Medical & Clinical
- **HospitalsExcelExporter**: Hospital/clinical site information
- **MedicalUnitsExcelExporter**: Medical unit/department data
- **RotationSlotsExcelExporter**: Clinical rotation schedules

#### Financial
- **LedgerEntriesExcelExporter**: Financial transaction headers
- **LedgerEntryDetailsExcelExporter**: Transaction line items
- **UserPurchasesExcelExporter**: User purchase history

#### Service Management
- **SurpathServicesExcelExporter**: Available service definitions
- **TenantSurpathServicesExcelExporter**: Tenant-specific service configs

#### Identity & Security
- **PIDTypesExcelExporter**: Personal identifier type exports
- **UserPidsExcelExporter**: User personal identifiers

### Implementation Pattern

All exporters follow this structure:
```csharp
public class EntityExcelExporter : NpoiExcelExporterBase, IEntityExcelExporter
{
    private readonly ITimeZoneConverter _timeZoneConverter;
    private readonly IAbpSession _abpSession;

    public FileDto ExportToFile(List<GetEntityForViewDto> entities)
    {
        return CreateExcelPackage(
            "EntityName.xlsx",
            excelPackage =>
            {
                var sheet = excelPackage.CreateSheet(L("EntityName"));
                AddHeader(sheet, L("Column1"), L("Column2"), ...);
                AddObjects(sheet, entities,
                    _ => _.Entity.Property1,
                    _ => _.Entity.Property2,
                    ...);
            });
    }
}
```

### Key Components

**Standard Features:**
- NPOI-based Excel generation (.xlsx format)
- Localized column headers via L() method
- Timezone-aware date conversion
- Consistent file naming (EntityName.xlsx)
- Temporary file caching with download tokens
- Property selector pattern for column mapping

**Inherited Capabilities (from NpoiExcelExporterBase):**
- Boolean to "Compliant"/"Not Compliant" conversion
- Automatic column width adjustment
- Cell styling and formatting
- Compliance color coding (green/red backgrounds)

### Dependencies
- **External**:
  - NPOI (Excel library)
  - ABP Framework (Session, Timezone, Localization)
- **Internal**:
  - `NpoiExcelExporterBase`: Base Excel functionality
  - `ITempFileCacheManager`: File caching
  - `ITimeZoneConverter`: Date conversion
  - `IAbpSession`: Current user session
  - Entity-specific DTOs (Get*ForViewDto classes)

## Architecture Notes
- **Pattern**: Exporter pattern with interface/implementation separation
- **Consistency**: All exporters follow identical structure
- **Localization**: Column headers localized for multi-language support
- **Timezone Handling**: Dates converted to user's timezone
- **File Management**: Temporary cache with token-based downloads

## Business Logic

### Export Process
1. Accept list of entity DTOs
2. Create Excel workbook with entity name as sheet name
3. Add localized header row
4. Map entity properties to columns via selectors
5. Apply base class formatting (booleans, dates, etc.)
6. Save to temporary cache
7. Return FileDto with download token

### Column Selection Strategy
- Include most important fields first
- Related entity names (foreign key lookups)
- Dates converted to user timezone
- Boolean values formatted as Compliant/Not Compliant
- Exclude binary data and large text fields

### Localization
Headers use L() method with resource keys:
- Entity name (e.g., "Cohort", "Record")
- Property name (e.g., "Name", "Description")
- Related entity (e.g., "TenantDepartment" + "Name")

## Usage Across Codebase

### Primary Consumers
- Application services (e.g., CohortsAppService)
- Export controller actions
- Report generation services
- Admin data export features

### Typical Usage Pattern
```csharp
// In CohortsAppService
public async Task<FileDto> GetCohortsToExcel(GetAllCohortsInput input)
{
    var cohorts = await GetAllCohorts(input);
    return _cohortsExcelExporter.ExportToFile(cohorts.Items.ToList());
}
```

### File Download Flow
1. User clicks "Export to Excel" button
2. Controller calls app service export method
3. Exporter generates Excel file
4. File cached with unique token
5. FileDto returned to client
6. Client downloads file using token
7. Temporary file cleaned up after download/timeout

## Technical Considerations

### Performance
- Handles lists of arbitrary size
- NPOI is memory-efficient
- Auto-size columns can be slow for very wide sheets
- Consider pagination for extremely large exports

### File Format
- Output: .xlsx (Excel 2007+ format)
- Compatible with Excel, LibreOffice, Google Sheets
- Cell formatting and styling preserved
- No macros or formulas (data only)

### Localization Support
- All exporters support localized column headers
- Date formatting respects user culture
- Timezone conversion for all date fields
- Supports any language configured in application

## Best Practices
- **Limit Row Count**: Consider pagination for exports > 10,000 rows
- **Column Selection**: Export only necessary columns
- **Performance Testing**: Test with realistic data volumes
- **Error Handling**: Handle null values in property selectors
- **Cache Cleanup**: Ensure temporary files are cleaned up
- **Timezone Awareness**: Always convert dates to user timezone

## Common Export Columns

### Most Entities Include
- ID (primary key)
- Name
- Description
- Created date/time
- Is Active/Is Deleted flags
- Related entity names (department, cohort, etc.)

### Compliance-Specific
- Compliance status (boolean → Compliant/Not Compliant)
- Expiration dates
- Approval status
- Record states

### Financial
- Amounts
- Transaction dates
- Payment status
- Reference numbers

## Extension Points
- Add filtering before export
- Support CSV format in addition to Excel
- Add summary rows with totals/counts
- Apply conditional formatting (colors, fonts)
- Include charts or pivot tables
- Support Excel templates with pre-formatted headers
- Add export scheduling (daily, weekly reports)
- Implement large dataset streaming for memory efficiency

## Maintenance Notes
- **Adding New Exporter**: Follow existing pattern exactly
- **Column Changes**: Update both header array and property selectors
- **Localization**: Add new resource keys for any new columns
- **Testing**: Test with empty lists, single items, and large datasets
- **Versioning**: If export format changes, consider version numbers in filename