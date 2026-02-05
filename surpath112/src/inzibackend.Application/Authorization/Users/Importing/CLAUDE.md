# Importing Documentation

## Overview
This folder contains the complete infrastructure for bulk user import functionality, allowing administrators to import users from Excel files. It handles the entire import workflow including reading Excel data, validating users, creating accounts, and reporting errors.

## Contents

### Files

#### IUserListExcelDataReader.cs
- **Purpose**: Interface defining the contract for reading user data from Excel files
- **Key Methods**:
  - `GetUsersFromExcel(byte[] fileBytes)`: Converts Excel file bytes into list of ImportUserDto
- **Dependency Injection**: Registered as transient dependency

#### UserListExcelDataReader.cs
- **Purpose**: Implementation of Excel reading logic using NPOI library
- **Key Features**:
  - Inherits from `NpoiExcelImporterBase<ImportUserDto>`
  - Processes Excel rows into ImportUserDto objects
  - Validates required fields (UserName, Name, Surname, EmailAddress, Password)
  - Handles optional fields (PhoneNumber)
  - Parses comma-separated role assignments
  - Provides localized error messages for invalid fields
- **Excel Format**:
  - Column 0: UserName (required)
  - Column 1: Name (required)
  - Column 2: Surname (required)
  - Column 3: EmailAddress (required)
  - Column 4: PhoneNumber (optional)
  - Column 5: Password (required)
  - Column 6: AssignedRoleNames (comma-separated list)

#### ImportUsersToExcelJob.cs
- **Purpose**: Background job for processing user import operations
- **Key Features**:
  - Executes as async background job
  - Reads Excel file from binary storage
  - Creates users with proper tenant context
  - Validates passwords against configured policies
  - Assigns roles based on display names
  - Handles errors gracefully per user
  - Sends notifications on completion
  - Exports invalid users to Excel for review
- **Workflow**:
  1. Retrieves Excel file from binary storage
  2. Parses users from Excel
  3. Validates each user individually
  4. Creates users in separate units of work
  5. Collects failed imports
  6. Generates error report if needed
  7. Sends appropriate notification

#### IInvalidUserExporter.cs
- **Purpose**: Interface for exporting failed user imports
- **Key Methods**:
  - `ExportToFile(List<ImportUserDto> userListDtos)`: Creates Excel file with invalid users

#### InvalidUserExporter.cs
- **Purpose**: Exports failed user imports to Excel for review
- **Key Features**:
  - Inherits from `NpoiExcelExporterBase`
  - Creates Excel file with all user fields plus error reason
  - Auto-sizes columns for readability
  - Includes localized column headers
- **Export Columns**:
  - UserName, Name, Surname, EmailAddress
  - PhoneNumber, Password, Roles
  - Refuse Reason (exception message)

### Key Components

**Services:**
- `UserListExcelDataReader`: Excel parsing implementation
- `ImportUsersToExcelJob`: Background job processor
- `InvalidUserExporter`: Error report generator

**Interfaces:**
- `IUserListExcelDataReader`: Excel reading contract
- `IInvalidUserExporter`: Error export contract

### Dependencies
- **External:**
  - `NPOI`: Excel file manipulation
  - `Abp.BackgroundJobs`: Background job infrastructure
  - `Microsoft.AspNetCore.Identity`: Password validation and hashing

- **Internal:**
  - `inzibackend.Authorization.Roles`: Role management
  - `inzibackend.Storage`: Binary file storage
  - `inzibackend.Notifications`: User notifications
  - `inzibackend.DataExporting.Excel.NPOI`: Excel export base classes
  - Dto subfolder: ImportUserDto

## Subfolders

### Dto
Contains `ImportUserDto` which defines the structure for user import records, including validation state tracking and role assignments.

## Architecture Notes
- **Pattern**: Background job pattern for long-running imports
- **Excel Processing**: Uses NPOI library for cross-platform Excel support
- **Error Handling**: Per-user error isolation prevents single failure from stopping entire import
- **Unit of Work**: Each user creation in separate UoW for transaction isolation
- **Validation**: Multi-level validation (Excel parsing, business rules, password policies)
- **Notifications**: Async notifications keep users informed of import status

## Business Logic

### Import Process
1. **File Upload**: Admin uploads Excel file through UI
2. **Job Queuing**: Import job queued with file reference
3. **Excel Parsing**: File read and converted to DTOs
4. **Validation**: Each user validated for:
   - Required field presence
   - Password complexity requirements
   - User count limits per tenant
   - Role validity
5. **User Creation**: Valid users created with:
   - Hashed passwords
   - Assigned roles (matched by display name)
   - Proper tenant association
6. **Error Reporting**: Failed users exported to Excel with reasons
7. **Notification**: Admin notified of results

### Role Assignment
- Roles matched by display name (case-insensitive)
- Multiple roles supported via comma separation
- Invalid role names don't fail entire user import

### Password Handling
- Validated against all configured password validators
- Hashed using ASP.NET Core Identity hasher
- Empty passwords allowed (user must set on first login)

## Usage Across Codebase
This import infrastructure is used by:
- Admin user management interfaces
- Tenant onboarding workflows
- Migration tools from other systems
- Bulk user provisioning APIs
- School enrollment systems

## Security Considerations
- Import restricted to authorized administrators
- Passwords validated against security policies
- User count limits enforced per tenant
- Tenant isolation maintained throughout process
- Binary files cleaned up after processing
- Sensitive data (passwords) not included in error exports
- Background job ensures UI remains responsive during large imports

## Performance Considerations
- Background job prevents UI blocking
- Individual UoW per user prevents transaction size issues
- Failed users don't stop successful imports
- Excel parsing optimized with NPOI streaming
- Notifications prevent need for polling