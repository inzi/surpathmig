# User Export Service Documentation

## Overview
Excel export service for user data, converting user list DTOs into formatted Excel files for reporting, backup, and data analysis. Uses NPOI library for Excel generation with localized headers and timezone conversion.

## Contents

### Files

#### IUserListExcelExporter.cs
- **Purpose**: Interface defining user list Excel export contract
- **Method**:
  - `ExportToFile(List<UserListDto>)`: Converts user list to Excel file
- **Returns**: `FileDto` with download token for the generated Excel file

#### UserListExcelExporter.cs
- **Purpose**: NPOI-based implementation of user list Excel exporter
- **Inheritance**: Extends `NpoiExcelExporterBase` for Excel functionality
- **Key Features**:
  - Exports user data to .xlsx format
  - Localized column headers
  - Timezone-aware date conversion
  - Session-based timezone settings
  - Multi-column user information
- **Exported Columns**:
  1. Name (first name)
  2. Surname (last name)
  3. UserName (login name)
  4. PhoneNumber
  5. EmailAddress
  6. EmailConfirm (confirmation status)
  7. Roles (comma-separated role list)
  8. Active (account status)
  9. CreationTime (account created date)
- **Dependencies**:
  - `ITimeZoneConverter`: Converts UTC times to user's timezone
  - `IAbpSession`: Gets current user's timezone setting
  - `ITempFileCacheManager`: Stores generated Excel file temporarily

### Key Components
**Export Process:**
1. Create Excel workbook
2. Add "Users" worksheet
3. Add localized header row
4. Add data rows with user information
5. Apply timezone conversion to dates
6. Save to temporary cache
7. Return FileDto with access token

**Data Transformation:**
- Date/time fields converted to user's local timezone
- Role arrays joined into comma-separated strings
- Boolean values preserved for base class styling

### Dependencies
- **External**:
  - NPOI (Excel library)
  - ABP Framework (Session, Timezone, Localization)
- **Internal**:
  - `NpoiExcelExporterBase`: Base Excel export functionality
  - `UserListDto`: User data transfer object
  - `FileDto`: File download descriptor
  - `ITempFileCacheManager`: Temporary file storage

## Architecture Notes
- **Pattern**: Exporter pattern with interface abstraction
- **Library**: NPOI for Excel generation (open-source)
- **Localization**: All headers localized via L() method
- **Timezone Handling**: Respects user's timezone preference
- **File Management**: Uses temporary cache for download tokens

## Business Logic
### Export Workflow
1. **Accept User List**: Receives `List<UserListDto>` with user data
2. **Create Workbook**: Generate new Excel workbook
3. **Add Headers**: Create localized header row with 9 columns
4. **Add Data Rows**: Iterate through users and add data (implementation continues beyond shown code)
5. **Apply Styling**: Base class handles boolean to "Compliant"/"Not Compliant" conversion
6. **Save to Cache**: Store file with unique token
7. **Return Token**: Client uses token to download file

### Timezone Conversion
- Retrieves user's timezone setting from session
- Converts CreationTime from UTC to user's local time
- Ensures dates display correctly for user's location

### Localization
Headers are localized using the L() method:
- "Name", "Surname", "UserName"
- "PhoneNumber", "EmailAddress", "EmailConfirm"
- "Roles", "Active", "CreationTime"
- Supports multi-language deployments

## Usage Across Codebase
### Primary Consumers
- `UserAppService`: Main user management service
- User list export functionality in UI
- Admin reporting and data export features

### Typical Usage
```csharp
// In UserAppService
public async Task<FileDto> GetUsersToExcel(GetUsersInput input)
{
    var users = await GetUserListAsync(input);
    return _userListExcelExporter.ExportToFile(users);
}
```

### Related Components
- User list query services
- File download handlers
- Report generation systems

## Technical Considerations
### Performance
- Handles lists of arbitrary size
- NPOI is memory-efficient for large datasets
- Temporary file caching prevents memory bloat

### File Format
- Output: .xlsx (Excel 2007+ format)
- Compatible with Excel, LibreOffice, Google Sheets
- Preserves formatting and styling

### Localization Support
- All column headers localized
- Supports any language configured in the application
- Falls back to default language if translation missing

## Best Practices
- **Limit Export Size**: Consider pagination for very large user lists
- **Cache Management**: Temporary files should be cleaned up periodically
- **Timezone Awareness**: Always convert dates to user timezone
- **Column Selection**: Include only necessary columns for performance
- **Error Handling**: Handle missing or null data gracefully

## Extension Points
- Add more columns (last login, password expiration, etc.)
- Apply conditional formatting (inactive users in red)
- Add summary row with user counts
- Support CSV export in addition to Excel
- Add filtering options before export
- Include organization unit information