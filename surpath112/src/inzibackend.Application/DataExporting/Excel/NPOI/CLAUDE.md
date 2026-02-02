# NPOI Excel Infrastructure Documentation

## Overview
NPOI-based Excel import/export infrastructure providing base classes for reading and writing Excel files (.xls and .xlsx formats). NPOI is used as the primary Excel library for the application, offering comprehensive styling capabilities including compliance status color coding.

## Contents

### Files

#### NpoiExcelImporterBase.cs
- **Purpose**: Abstract base class for importing Excel files into typed entities
- **Key Features**:
  - Generic type parameter for entity type
  - Support for both old (.xls/HSSF) and new (.xlsx/XSSF) Excel formats
  - Processes all sheets in a workbook
  - Automatically skips header rows
  - Error-tolerant row processing (ignores failed rows)
- **Key Method**:
  - `ProcessExcelFile<TEntity>()`: Main import method accepting file bytes and row processor function
- **Usage Pattern**: Derive from this class and provide row processing logic via delegate

#### NpoiExcelExporterBase.cs
- **Purpose**: Abstract base class for exporting data to Excel files with rich formatting
- **Key Features**:
  - Inherits from `inzibackendServiceBase` for localization and common services
  - Temporary file caching via `ITempFileCacheManager`
  - Compliance status color coding (compliant=light green, non-compliant=red)
  - Automatic boolean to "Compliant"/"Not Compliant" text conversion
  - Header creation with bold formatting
  - Auto-sizing columns for better readability
  - Date format management with caching
- **Cell Style Management**:
  - `GetDateCellStyle()`: Cached date cell styles
  - `GetDateDataFormat()`: Cached date formats for performance
  - Custom compliance coloring:
    - Compliant: RGB(170, 220, 200) - light green
    - Non-Compliant: RGB(255, 102, 102) - light red
- **Key Methods**:
  - `CreateExcelPackage()`: Creates Excel file with custom content generator
  - `AddHeader()`: Adds bold header row
  - `AddObjects<T>()`: Bulk adds data rows with automatic boolean formatting
  - `SetCellDataFormat()`: Applies custom date formatting
  - `Save()`: Writes workbook to temporary file cache
- **Special Handling**:
  - Boolean values automatically converted to styled "Compliant"/"Not Compliant" cells
  - Contains commented-out experimental styling code for reference
  - Explicit garbage collection after column auto-sizing

### Key Components
**Import Infrastructure:**
- Generic base class for strongly-typed imports
- Multi-sheet processing support
- Error-resilient parsing

**Export Infrastructure:**
- Rich formatting capabilities
- Compliance-aware cell styling
- Performance-optimized caching
- Temporary file management

### Dependencies
- **External Libraries**:
  - NPOI (SS.UserModel, XSSF, HSSF) - Apache POI .NET port
  - System.Drawing (for Color types)
- **Internal**:
  - `inzibackendServiceBase`: Base service functionality
  - `ITempFileCacheManager`: Temporary file storage (Abp.AspNetZeroCore.Net)
  - `FileDto`: File representation (inzibackend.Dto)
  - `Storage`: File storage abstractions (inzibackend.Storage)

## Architecture Notes
- **Library Choice**: NPOI chosen over EPPlus (which switched to commercial licensing)
- **Pattern**: Template Method pattern - base classes provide structure, derived classes implement specifics
- **File Format Support**: Both legacy (.xls) and modern (.xlsx) Excel formats
- **Performance**: Cell style caching prevents redundant object creation
- **Error Handling**: Import process silently ignores rows that fail to parse

## Business Logic
### Import Process
1. Accept Excel file as byte array
2. Detect format and create appropriate workbook (HSSF or XSSF)
3. Iterate through all sheets
4. For each sheet, skip header row (row 0)
5. Call custom row processor for each data row
6. Aggregate entities from all sheets
7. Return typed entity list

### Export Process
1. Create XSSFWorkbook instance
2. Apply creator action to populate sheets
3. Add headers with bold styling
4. Add data rows with property selectors
5. Apply compliance color coding for boolean values
6. Auto-size columns for readability
7. Save to temporary file cache
8. Return FileDto with access token

### Compliance Coloring Logic
- Detects boolean values in cell content (case-insensitive)
- True values: Green background + "Compliant" text
- False values: Red background + "Not Compliant" text
- Improves visual scanning of compliance reports

## Usage Across Codebase
### Primary Consumers
- User import/export services
- Audit log exporters
- Compliance report exporters
- Any service requiring Excel file generation

### Typical Implementation Pattern
```csharp
// Import
public class UserImporter : NpoiExcelImporterBase<UserDto>
{
    public List<UserDto> ImportUsers(byte[] fileBytes)
    {
        return ProcessExcelFile(fileBytes, (sheet, row) => {
            // Extract data from row and return UserDto
        });
    }
}

// Export
public class ComplianceExporter : NpoiExcelExporterBase
{
    public FileDto ExportCompliance(List<ComplianceDto> data)
    {
        return CreateExcelPackage("Compliance.xlsx", workbook => {
            var sheet = workbook.CreateSheet("Compliance");
            AddHeader(sheet, "Name", "Status", "Date");
            AddObjects(sheet, data,
                d => d.Name,
                d => d.IsCompliant,
                d => d.Date);
        });
    }
}
```

## Technical Considerations
### Why NPOI Over EPPlus
- EPPlus v5+ switched to dual licensing (commercial required for business use)
- NPOI remains open-source (Apache 2.0 license)
- Both files exist but EPPlus code is commented out

### Performance Optimizations
- Cell style caching reduces object creation overhead
- Workbook-level style management prevents duplication
- Explicit GC collection after intensive operations (column sizing)
- Stream-based file handling for memory efficiency

### Memory Management
- Uses `using` statements for proper stream disposal
- Clears cached styles when workbook changes
- Explicit garbage collection hints for large datasets