# Cohort Excel Operations Implementation

## Overview
This branch implements a dropdown-based Excel operations feature for both the Cohorts section and the Members (CohortUsers) section, replacing the single export buttons with more comprehensive solutions similar to the Users section. **Enhanced with tenant-specific status colors, automatic text contrast calculation, multi-sheet workbook generation, smart tab ordering, and robust sheet naming for Excel compliance.**

## 📋 Dropdown Options (Updated Structure)

### Cohorts Page (`/App/Cohorts`)
**Three Excel Operations Available:**
1. **"Cohorts"** - Basic cohorts export (calls `getCohortsToExcel`)
2. **"Cohorts Compliance Summary"** - Extended compliance report with colors (calls `getCohortUsersToExcel`) **with multi-sheet support**
3. **"Cohorts Immunization Report"** - Detailed immunization report with data-driven columns (calls `getCohortsImmunizationReportToExcel`) **with multi-sheet support**

### Members Page (`/App/CohortUsers`) 
**Two Excel Operations Available:**
1. **"Cohort Compliance Summary"** - Compliance report for single cohort with colors **or multi-sheet for multiple cohorts**
2. **"Cohort Immunization Report"** - Detailed immunization report for single cohort with data-driven columns **or multi-sheet for multiple cohorts**

## Features Implemented

### 1. Excel Operations Dropdown
- Replaced single "Export to Excel" button with "Excel Operations" dropdown in both:
  - **Cohorts page**: Main cohorts listing
  - **Members page**: Cohort members view (MasterDetailChild_Cohort_CohortUsers)
- Two export options available for both pages:
  - **Cohort Compliance Summary**: The existing basic export functionality **with tenant status colors and multi-sheet support**
  - **Cohort Immunization Report**: New detailed immunization report **with tenant status colors, data-driven columns, and multi-sheet support**

### 2. Multi-Sheet Workbook Generation 🆕
**NEW FEATURE**: When no specific cohort is selected (from Cohorts page), both compliance and immunization reports generate multi-sheet Excel workbooks:
- **Individual cohort sheets**: Each cohort gets its own tab with cohort name as sheet title and cohort header row
- **"All" sheet**: Combined data from all cohorts with cohort name in the first column for easy sorting/filtering
- **Smart sheet naming**: Uses exact cohort names when possible, with duplicate handling (adds -1, -2, etc. for conflicts)
- **Excel-compliant naming**: Removes invalid characters `[]*/?\:` and respects 31-character limit
- **Number prefix handling**: Cohort names starting with numbers get "(cohort) " prepended (e.g., "2023-2024 EMT" becomes "(cohort) 2023-2024 EMT")
- **Consistent formatting**: Same styling and color application across all sheets

### 3. Smart Tab Ordering & User Sorting 🆕
**NEW FEATURES**:
- **"All" tab first**: The combined "All" sheet always appears as the first tab for easy access
- **Alphabetical tab ordering**: Individual cohort tabs are sorted alphabetically by cohort name
- **User sorting by lastname**: All users within each sheet are sorted by lastname, then firstname for consistent ordering
- **Robust duplicate handling**: Multiple cohorts with identical names get unique suffixes (-1, -2, etc.)

### 4. Cohort Immunization Report
A comprehensive report that shows:
- **Cohort identifier**: "Cohort:" in column A, cohort name in column B (row 1)
- Each cohort member (name) starting from row 3, **sorted by lastname**
- For each immunization requirement that the user must upload:
  - Compliance status (Compliant/Not Compliant) **with tenant-specific background colors**
  - Administered date (from Record EffectiveDate if available) **- only if any user has meaningful administered dates**
  - Expiration date (from Record ExpirationDate if available) **- only if any user has meaningful expiration dates**
- **Data-driven column generation**: Only creates columns that contain actual data across all users
- **Intelligent column tracking**: Analyzes all data first to determine which columns have meaningful values
- **Simplified headers**: Category name for status, then simply "Administered Date" and "Expiration Date" for subsequent columns
- **Generate-first approach**: Creates all data rows first, then inserts cohort header above for consistent layout
- Automatically formats dates as MM/dd/yyyy
- **Automatic text color contrast calculation for optimal readability**

### 5. Tenant-Specific Status Colors 🎨
**NEW FEATURE**: Both exports now apply tenant-configured status colors:
- **Background colors**: Applied from `RecordStatus.HtmlColor` property based on compliance status
- **Text contrast**: Automatically calculated using W3C luminance formula to ensure readability
- **Fallback colors**: Default green (#67c777) for compliant, red (#ba323b) for non-compliant if no tenant config found
- **Smart color matching**: Uses `RecordStatus.ComplianceImpact` to determine appropriate tenant status colors
- **💡 NPOI 2.5.6 Compatibility**: Fixed color application using proper XSSFCellStyle and XSSFFont casting

## Files Modified

### Frontend (MVC Views & JavaScript)
- **src/inzibackend.Web.Mvc/Areas/App/Views/Cohorts/Index.cshtml**
  - Replaced single export button with Bootstrap dropdown
  - Added two dropdown options with unique IDs

- **src/inzibackend.Web.Mvc/Areas/App/Views/MasterDetailChild_Cohort_CohortUsers/Index.cshtml**
  - Replaced single export button with Bootstrap dropdown
  - Added two dropdown options with cohort-specific IDs

- **src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/Cohorts/Index.js**
  - Updated JavaScript handlers for two separate export functions
  - Added new handler for immunization report export

- **src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/MasterDetailChild_Cohort_CohortUsers/Index.js**
  - Updated JavaScript handlers for two separate export functions  
  - Added new handler for immunization report export
  - Maintains cohort-specific ID handling

### Backend (Application Layer)
- **src/inzibackend.Application.Shared/Surpath/ICohortsAppService.cs**
  - Added new method signature: `GetCohortsImmunizationReportToExcel`

- **src/inzibackend.Application.Shared/Surpath/ICohortUsersAppService.cs**
  - Added new method signature: `GetCohortUsersImmunizationReportToExcel`

- **src/inzibackend.Application/Surpath/CohortsAppService.cs**
  - Added RecordState repository injection to constructor
  - Implemented `GetCohortsImmunizationReportToExcel` method
  - Comprehensive logic to gather immunization data for cohort members
  - **Added status color extraction from RecordStatus.HtmlColor**

- **src/inzibackend.Application/Surpath/CohortUsersAppService.cs**
  - Implemented `GetCohortUsersImmunizationReportToExcel` method
  - Filters cohort users based on specific cohort ID
  - Reuses the same immunization data gathering logic
  - **Added `GetStatusColorForCompliance` method for tenant-specific colors**
  - **Enhanced CohortUserComplianceExcel class with status color properties**
  - **Added status color extraction from RecordStatus.HtmlColor**

### Data Transfer Objects
- **src/inzibackend.Application.Shared/Surpath/Dtos/CohortImmunizationReportDto.cs** (Modified)
  - Created DTOs for immunization report data structure
  - `CohortImmunizationReportDto`: Main report container
  - `ImmunizationRequirementDto`: Individual requirement data **with StatusColor property**
  - Shared between both Cohorts and CohortUsers services

### Excel Export Layer
- **src/inzibackend.Application/Surpath/Exporting/ICohortsExcelExporter.cs**
  - Added new method signature: `ExportImmunizationReportToFile`
  - **🆕 Added new method signature: `ExportImmunizationReportToFileMultiSheet`**

- **src/inzibackend.Application/Surpath/Exporting/ICohortUsersExcelExporter.cs**
  - Added new method signature: `ExportImmunizationReportToFile`
  - **🆕 Added new method signature: `ExportToFileFromObjectMultiSheet`**
  - **🆕 Added new method signature: `ExportImmunizationReportToFileMultiSheet`**

- **src/inzibackend.Application/Surpath/Exporting/CohortsExcelExporter.cs**
  - Implemented `ExportImmunizationReportToFile` method
  - Dynamic column generation based on requirements
  - Enhanced formatting and auto-sizing
  - **Added `ApplyComplianceStatusColor` method for NPOI cell styling**
  - **Added `GetContrastingTextColor` method for text contrast calculation**
  - **Manual row creation for proper color application**
  - **🆕 Implemented `ExportImmunizationReportToFileMultiSheet` method for multi-sheet generation**
  - **🆕 Added `CreateImmunizationSheet` helper method for consistent sheet creation**
  - **🆕 Enhanced `GetUniqueSheetName` method with number prefix handling and Excel compliance**
  - **🆕 Added user sorting by lastname in all sheet creation methods**

- **src/inzibackend.Application/Surpath/Exporting/CohortUsersExcelExporter.cs**
  - Implemented `ExportImmunizationReportToFile` method
  - Same dynamic column generation logic as CohortsExcelExporter
  - Consistent formatting across both exporters
  - **Added `ApplyComplianceStatusColor` method for NPOI cell styling**
  - **Added `GetContrastingTextColor` method for text contrast calculation**
  - **Replaced AddObjects with manual row creation for compliance summary**
  - **Enhanced to apply colors to both immunization and compliance reports**
  - **🆕 Implemented `ExportToFileFromObjectMultiSheet` method for multi-sheet compliance reports**
  - **🆕 Implemented `ExportImmunizationReportToFileMultiSheet` method for multi-sheet immunization reports**
  - **🆕 Added `CreateImmunizationSheet` helper method for consistent sheet creation**
  - **🆕 Enhanced `GetUniqueSheetName` method with number prefix handling and Excel compliance**
  - **🆕 Added user sorting by lastname in all sheet creation methods**

### Localization
- **src/inzibackend.Core/Localization/inzibackend/inzibackend.xml**
  - Added localization keys:
    - `CohortComplianceSummary`
    - `CohortImmunizationReport`

## Technical Implementation Details

### Data Gathering Strategy
Both immunization implementations use a sophisticated data-driven approach:
1. **Filter Data**: Apply filtering logic (cohorts vs specific cohort members)
2. **Get Members**: Retrieve all active users in scope
3. **Gather Requirements**: Use `SurpathManager.GetRequirementListFor()` to get immunization requirements
4. **Analyze Data**: Track which columns have meaningful data per category using `Dictionary<string, (bool hasStatus, bool hasAdministered, bool hasExpiration)>`
5. **Build Dynamic Structure**: Create column mapping based only on columns with actual data
6. **Sort Users**: Order all users by lastname, then firstname for consistent presentation
7. **Generate Data First**: Create all data rows with proper column alignment
8. **Insert Header**: Use `ShiftRows()` to insert cohort header and blank row above data for consistent layout
9. **💡 NEW: Extract Status Colors**: Get `RecordStatus.HtmlColor` for each compliance status
10. **🆕 NEW: Multi-Sheet Logic**: Detect multiple cohorts and generate individual sheets plus "All" sheet

### Multi-Sheet Generation Logic 🆕
**Detection**: 
- **Compliance Reports**: When `CohortId` parameter is not a valid GUID (`validguid = false`)
- **Immunization Reports**: When multiple distinct cohort names are present in the data

**Sheet Creation Order**:
- **"All" Sheet First**: Always created first for easy access to combined data
- **Individual Cohort Sheets**: Created in alphabetical order by cohort name
- **Consistent Formatting**: Same headers, colors, and styling across all sheets

**Safe Sheet Naming**: 
- **Number Prefix Handling**: Names starting with numbers get "(cohort) " prepended (e.g., "2023-2024 EMT" → "(cohort) 2023-2024 EMT")
- **Excel Compliance**: Removes/replaces invalid characters `[]*/?\:` and truncates to 31 character limit
- **Duplicate Resolution**: Tracks used sheet names and automatically adds suffixes (-1, -2, etc.) for identical cohort names
- **Name Preservation**: Preserves original name as much as possible while maintaining Excel compatibility

### User Sorting Strategy 🆕
**Consistent Ordering**: All exports now sort users by lastname, then firstname:
- **Single-sheet reports**: Users sorted before creating rows
- **Multi-sheet reports**: Users sorted both in "All" sheet and individual cohort sheets
- **Group-level sorting**: Cohort data within each group is also sorted by lastname
- **Performance**: Sorting done once per data set to minimize overhead

### Service Method Mapping
- **"Cohorts"** → `getCohortsToExcel`: Basic cohort information export (cohort-level data)
- **"Cohorts Compliance Summary"** → `getCohortUsersToExcel`: User compliance data across all cohorts with colors **and multi-sheet support**
- **"Cohorts Immunization Report"** → `getCohortsImmunizationReportToExcel`: Detailed immunization compliance with data-driven columns **and multi-sheet support**

### Excel Report Structure
- **Row 1**: "Cohort:" in column A, cohort name in column B  
- **Row 2**: Empty (spacing)
- **Row 3**: Header row with column names
- **Fixed Columns**: First Name, Last Name (sorted by lastname)
- **Dynamic Columns**: Auto-generated based on data analysis
  - Pattern per category: `{CategoryName}`, `Administered Date` (if data exists), `Expiration Date` (if data exists)
  - **Data-driven inclusion**: Columns only created if category has meaningful data (excludes DateTime.MinValue and empty strings)
  - **Simplified naming**: Category name for status, simple "Administered Date"/"Expiration Date" labels
  - **Column mapping**: Uses tuple-based mapping `(category, columnType)` for precise data placement
- **Auto-sizing**: All columns are automatically sized for optimal viewing
- **💡 NEW: Color Application (NPOI 2.5.6 Compatible)**: 
  - Compliance status cells get tenant-specific background colors
  - Text color automatically calculated for optimal contrast (black/white)
  - Uses W3C luminance formula: `(0.299*R + 0.587*G + 0.114*B) / 255`
  - **Fixed implementation**: Proper `XSSFCellStyle` casting and `SetFillForegroundColor()` method usage

### Status Color Implementation
- **Color Source**: `RecordStatus.HtmlColor` property from tenant-configured statuses
- **Color Matching**: Uses `RecordStatus.ComplianceImpact` enum to match compliance states
- **NPOI Integration**: 
  - **Fixed for NPOI 2.5.6**: Uses proper `XSSFCellStyle` and `XSSFFont` casting
  - **Proper method calls**: `SetFillForegroundColor()` and `SetColor()` for custom colors
  - `ColorTranslator.FromHtml()` for hex color parsing
  - Manual cell style creation and font styling
- **Fallback Strategy**: Default colors when tenant config not found
- **Error Handling**: Graceful degradation if color parsing fails

### Key Differences Between Implementations
- **Cohorts page**: Processes all cohorts and their members (broader scope)
- **Members page**: Processes specific cohort members only (targeted scope)
- **Filtering**: Members page includes cohort-specific filtering
- **Data source**: Both use same underlying data structures and logic
- **💡 Color consistency**: Both use identical color application methods
- **🆕 Sorting consistency**: Both use identical lastname-based sorting

### Error Handling
- Graceful handling of missing records
- Default values for non-compliant/missing data
- Safe navigation for nullable properties
- Consistent error handling across both implementations
- **💡 NEW: Color parsing error handling**: Continue without styling if color parsing fails
- **🆕 NEW: Sheet naming error handling**: Fallback to generic names if cohort name processing fails

## 🔧 Troubleshooting

### "Unknown Cohort" Issue
**Problem**: Users appear in "Unknown Cohort" tab instead of their actual cohort (e.g., "2023-2024 Dual Credit EMT Cohort")

**Root Cause**: The `CohortName` property in the data may be null or empty for some users, even though they belong to a valid cohort.

**Resolution**: 
1. **Sheet Naming**: Cohort names starting with numbers now get "(cohort) " prepended for Excel compliance
2. **Data Validation**: Check that the CohortName property is properly populated in the data source
3. **Fallback Logic**: "Unknown Cohort" only appears when CohortName is actually null/empty

**Example**: 
- **Before**: "2023-2024 EMT Cohort" → Excel error or "Unknown Cohort"
- **After**: "2023-2024 EMT Cohort" → "(cohort) 2023-2024 EMT Cohort" tab

### Tab Ordering Issues
**Expected Behavior**: 
- "All" tab appears first
- Individual cohort tabs appear in alphabetical order
- Users within each tab are sorted by lastname

## Database Dependencies
- **RecordState**: Latest compliance record for each user/category
- **Record**: Contains EffectiveDate (administered) and ExpirationDate
- **RecordStatus**: Determines compliance status via ComplianceImpact **and provides HtmlColor**
- **RecordRequirement & RecordCategory**: Defines what users must upload

## New Dependencies Added
- **System.Drawing**: For color parsing and luminance calculations
- **NPOI.SS.UserModel & NPOI.XSSF.UserModel**: Enhanced Excel styling capabilities **with NPOI 2.5.6 compatibility**

## Future Enhancements
- Additional export formats (PDF, CSV) with color preservation
- Filtering options for specific requirements
- Date range filtering for administered/expiration dates
- Bulk operations for compliance management
- Unified export service to reduce code duplication
- **Custom color themes per tenant department**
- **Advanced contrast ratio calculations (WCAG compliance)**
- **Color-blind accessibility options**
- **💡 LAYOUT: Option to show multiple cohorts in separate sheets within single workbook**
- **💡 DATA-DRIVEN: User-configurable column inclusion/exclusion settings with data analysis preview**
- **💡 DATA-DRIVEN: Option to show placeholder columns even when no data exists (toggle)**
- **💡 PERFORMANCE: Caching of data analysis results for repeated exports**
- **💡 ANALYTICS: Summary statistics on data completeness per category**
- **💡 VALIDATION: Pre-export data quality report highlighting missing or incomplete records**
- **🆕 MULTI-SHEET: User preference for single-sheet vs multi-sheet format selection**
- **🆕 MULTI-SHEET: Custom sheet naming templates (e.g., "Cohort: {Name}", "{Name} - {Date}")**
- **🆕 MULTI-SHEET: Advanced duplicate handling options (numbered suffixes, abbreviated names, etc.)**
- **🆕 MULTI-SHEET: Sheet ordering options (alphabetical, by size, by compliance rate)**
- **🆕 MULTI-SHEET: Cross-sheet summary/dashboard sheet with cohort comparison metrics**
- **🆕 MULTI-SHEET: Excel table formatting with sortable/filterable headers**
- **🆕 MULTI-SHEET: Conditional formatting based on compliance thresholds**
- **🆕 MULTI-SHEET: Export progress indicator for large multi-cohort datasets**
- **🆕 SORTING: User-configurable sort options (firstname, lastname, compliance status, etc.)**
- **🆕 SHEET-NAMING: Data validation to ensure cohort names are properly populated before export**

## Testing Notes
- Test with cohorts that have different requirement sets
- Verify dynamic column generation works correctly on both pages
- Test date formatting and null value handling
- Confirm permissions are respected (same as original exports)
- Test cohort-specific filtering on Members page
- Verify dropdown UI consistency between pages
- **💡 NEW: Test color application with various tenant status color configurations (NPOI 2.5.6)**
- **💡 NEW: Verify text contrast works with both light and dark background colors**
- **💡 NEW: Test fallback colors when tenant status config is missing**
- **💡 NEW: Verify Excel files open correctly in various spreadsheet applications**
- **💡 FIXED: Confirm colors display properly in Excel with NPOI 2.5.6 implementation**
- **💡 LAYOUT: Test cohort name appears correctly split across columns A and B in row 1**
- **💡 DATA-DRIVEN: Test column tracking correctly identifies which categories have meaningful data**
- **💡 DATA-DRIVEN: Test scenarios where some categories have no administered dates but others do**
- **💡 DATA-DRIVEN: Test scenarios where some categories have no expiration dates but others do**
- **💡 DATA-DRIVEN: Test mixed scenarios with partial data across different categories**
- **💡 HEADERS: Verify simplified column headers (category name, then "Administered Date", "Expiration Date")**
- **💡 HEADERS: Test that empty columns are completely excluded from the sheet**
- **💡 LAYOUT: Verify ShiftRows correctly positions cohort header above data without affecting column alignment**
- **💡 PERFORMANCE: Test with large datasets to ensure data analysis doesn't impact performance significantly**
- **🆕 MULTI-SHEET: Test multi-sheet generation with multiple cohorts from Cohorts page**
- **🆕 MULTI-SHEET: Test single-sheet generation when only one cohort is present**
- **🆕 MULTI-SHEET: Verify each cohort gets its own properly named sheet with correct data**
- **🆕 MULTI-SHEET: Test "All" sheet contains combined data with cohort names in first column**
- **🆕 MULTI-SHEET: Test safe sheet naming with cohorts that have special characters or long names**
- **🆕 MULTI-SHEET: Test duplicate cohort name handling (should add -1, -2, etc. suffixes)**
- **🆕 MULTI-SHEET: Test sheet names with exact cohort names when no conflicts exist**
- **🆕 MULTI-SHEET: Verify sheet order is consistent ("All" first, then alphabetical by cohort name)**
- **🆕 MULTI-SHEET: Test color application works correctly across all sheets**
- **🆕 MULTI-SHEET: Test auto-sizing works for all sheets in the workbook**
- **🆕 MULTI-SHEET: Test with cohorts that have different sets of requirements/categories**
- **🆕 SINGLE vs MULTI: Test that Members page (single cohort) still generates single-sheet reports**
- **🆕 SINGLE vs MULTI: Test that Members page with multiple cohorts generates multi-sheet reports**
- **🆕 NUMBER-PREFIX: Test cohorts starting with numbers (e.g., "2023-2024 EMT") get "(cohort) " prefix**
- **🆕 NUMBER-PREFIX: Verify "Unknown Cohort" issue is resolved for numeric cohort names**
- **🆕 SORTING: Test all users are sorted by lastname, then firstname in all reports**
- **🆕 SORTING: Test sorting works correctly in both single-sheet and multi-sheet formats**
- **🆕 TAB-ORDER: Verify "All" tab is always first tab in multi-sheet workbooks**
- **🆕 TAB-ORDER: Verify individual cohort tabs are in alphabetical order**

## Dependencies
- Uses existing AspNetZero/ABP framework patterns
- Leverages NPOI 2.5.6 for Excel generation with enhanced styling
- Follows established localization patterns
- Maintains existing security/permission model
- Shares DTOs and logic between Cohorts and CohortUsers services
- **💡 NEW: Requires System.Drawing for color operations**
- **💡 FIXED: Enhanced NPOI 2.5.6 usage for advanced cell styling with proper casting**

## Performance Considerations
- **Color lookup optimization**: Status colors retrieved once per compliance state
- **Bulk operations**: Color application done during row creation to minimize overhead
- **Memory efficiency**: Color objects created on-demand and disposed properly
- **Excel file size**: Styled cells may increase file size slightly but remain manageable
- **💡 NPOI 2.5.6 Optimization**: Proper object casting reduces overhead and ensures compatibility
- **💡 DATA-DRIVEN: Pre-analysis phase**: Initial data scan to determine column requirements adds minimal overhead
- **💡 COLUMN EFFICIENCY**: Only processing and creating columns with actual data reduces memory usage and file size
- **💡 LAYOUT OPTIMIZATION**: ShiftRows operation is efficient for small row counts and maintains cell references
- **🆕 SORTING OPTIMIZATION**: Single sort operation per data set reduces processing time
- **🆕 SHEET-NAMING OPTIMIZATION**: String operations for sheet name processing are minimal and cached
- **🆕 TAB-ORDER OPTIMIZATION**: "All" sheet created first eliminates need for sheet reordering operations
 