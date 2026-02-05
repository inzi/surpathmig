# Dto Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) used for bulk user import operations. These DTOs facilitate the import of users from external sources like Excel files or CSV data.

## Contents

### Files

#### ImportUserDto.cs
- **Purpose**: DTO for representing user data during bulk import operations
- **Key Properties**:
  - `Name`: User's first name
  - `Surname`: User's last name
  - `UserName`: Login username
  - `EmailAddress`: User's email address
  - `PhoneNumber`: User's phone number
  - `Password`: Initial password for the user
  - `AssignedRoleNames`: Array of role names to assign to the user
  - `Exception`: Error message if validation fails during import
- **Key Methods**:
  - `CanBeImported()`: Returns true if no exceptions occurred during validation

### Key Components

**Classes:**
- `ImportUserDto`: Main DTO for user import data

**Properties:**
- User identification fields (Name, Surname, UserName)
- Contact information (EmailAddress, PhoneNumber)
- Security (Password, AssignedRoleNames)
- Validation tracking (Exception)

### Dependencies
- **External:**
  - None (uses only .NET base types)

- **Internal:**
  - No direct internal dependencies

## Architecture Notes
- **Pattern**: Data Transfer Object (DTO) pattern
- **Validation**: Self-contained validation tracking via Exception property
- **Role Assignment**: Supports multiple role assignment through array
- **Import Workflow**: Designed for batch processing with error tracking per record

## Business Logic
- Each import record can track its own validation errors
- Users can be assigned multiple roles during import
- Password can be set during import (likely with option to force change on first login)
- Import process can continue even if individual records fail validation
- The `CanBeImported()` method provides a simple way to filter valid records

## Usage Across Codebase
This DTO is likely used in:
- Excel/CSV import services
- Bulk user creation endpoints
- User migration tools
- Tenant onboarding processes
- Administrative user management interfaces

## Data Flow
1. External data source (Excel/CSV) is parsed into `ImportUserDto` objects
2. Each DTO is validated, with errors stored in the `Exception` property
3. Valid DTOs (where `CanBeImported()` returns true) are processed
4. Users are created with specified roles and credentials
5. Import results are reported back with success/failure per record

## Security Considerations
- Passwords are handled in plain text within the DTO (should be hashed before storage)
- Role assignment should be validated against tenant permissions
- Bulk import operations should be restricted to administrative users
- Import should validate email uniqueness and username availability