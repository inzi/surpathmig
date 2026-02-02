# EPPlus Excel Infrastructure Documentation

## Overview
EPPlus-based Excel import/export infrastructure (COMMENTED OUT/DISABLED). These base classes provided Excel functionality using the EPPlus library but are no longer active due to EPPlus v5+ licensing changes. The code is preserved for reference but all functionality has been migrated to NPOI.

## Contents

### Files

#### EpPlusExcelExporterBase.cs
- **Status**: COMPLETELY COMMENTED OUT
- **Original Purpose**: Abstract base class for Excel export using EPPlus library
- **License Note**: EPPlus v5+ changed to dual licensing (Polyform Non Commercial/Commercial)
- **Original Features** (when active):
  - Export to .xlsx format
  - Header creation with bold formatting
  - Bulk object export with property selectors
  - Temporary file caching
- **Current State**: Entire implementation commented out, retained for historical reference

#### EpPlusExcelImporterBase.cs
- **Status**: COMPLETELY COMMENTED OUT
- **Original Purpose**: Abstract base class for Excel import using EPPlus library
- **Original Features** (when active):
  - Generic type parameter for entity import
  - Multi-worksheet processing
  - Row-by-row parsing with error tolerance
  - Header row skipping
- **Current State**: Entire implementation commented out

### Key Components
**ALL COMPONENTS ARE DISABLED**
- Export functionality: Migrated to NPOI
- Import functionality: Migrated to NPOI

### Dependencies
- **Former External**: EPPlus library (OfficeOpenXml namespace)
- **Former Internal**:
  - `ITempFileCacheManager`
  - `FileDto`
  - `inzibackendServiceBase`
- **Current**: None (files contain only comments)

## Architecture Notes
- **Why Disabled**: EPPlus licensing change from LGPL to commercial/non-commercial dual license
- **Migration Path**: All Excel functionality moved to NPOI library (open-source Apache 2.0)
- **File Retention**: Code kept for reference and potential future use if licensing changes
- **Version Affected**: EPPlus v5 and later require commercial license for business use
- **Previous Versions**: EPPlus v4 and earlier were LGPL licensed

## Business Logic
**NONE - ALL CODE COMMENTED OUT**

Original functionality has been reimplemented in:
- `NpoiExcelExporterBase.cs` - Export functionality
- `NpoiExcelImporterBase.cs` - Import functionality

## Usage Across Codebase
**NO CURRENT USAGE**
- All consumers have been migrated to NPOI equivalents
- No active references to these classes in the codebase

## Migration Notes
### If EPPlus Becomes Viable Again
1. Uncomment the code in both files
2. Restore EPPlus NuGet package reference
3. Update services to derive from EPPlus base classes instead of NPOI
4. Test import/export functionality
5. Consider performance differences between libraries

### Feature Parity with NPOI
The NPOI implementation provides equivalent or better functionality:
- Multi-format support (.xls and .xlsx)
- Rich cell styling and compliance color coding
- Better formatting control
- Open-source licensing without restrictions

## Technical Considerations
### Why These Files Still Exist
- **Historical Reference**: Shows original implementation approach
- **License Monitoring**: If EPPlus reverts to open-source, code is ready
- **Documentation**: Helps understand why NPOI was chosen
- **Migration Guide**: Shows differences between libraries

### Key Differences from NPOI
- **EPPlus**: Cleaner API, more modern syntax
- **NPOI**: More verbose but more powerful styling options
- **Licensing**: NPOI is freely available for commercial use
- **Community**: Both have active communities

## Recommendations
- **Do Not Use**: These classes are disabled and should not be activated without resolving licensing
- **Use NPOI Instead**: All Excel functionality should use NPOI base classes
- **Consider Removal**: If no plans to use EPPlus, these files could be deleted
- **Keep Updated**: If keeping for reference, note when license terms change