# User Management DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for comprehensive user management functionality. With 20 files, it represents the core user administration system including user CRUD operations, role assignments, permission management, login tracking, account linking, and bulk import capabilities. These DTOs support the full user lifecycle from creation through profile management.

## Contents

### Files

#### List & Display DTOs
- **UserListDto.cs** - User list item with essential information:
  - Basic profile (Name, Surname, UserName, EmailAddress, PhoneNumber)
  - ProfilePictureId for avatar display
  - IsEmailConfirmed verification status
  - Roles collection (UserListRoleDto)
  - IsActive and CreationTime for status tracking
  - Implements IPassivable and IHasCreationTime

- **UserListRoleDto.cs** - Role information in user lists
- **LinkedUserDto.cs** - Linked account information for account switching
- **UserLoginAttemptDto.cs** - Login attempt history for security auditing

#### Edit DTOs
- **UserEditDto.cs** - Complete user profile for admin editing:
  - Full name fields (Name, Middlename, Surname)
  - Account credentials (UserName, EmailAddress, Password)
  - Contact information (PhoneNumber, full address fields)
  - Security settings (IsActive, ShouldChangePasswordOnNextLogin, IsTwoFactorEnabled, IsLockoutEnabled)
  - Personal information (DateOfBirth, Address, City, State, Zip, SuiteApt)
  - IsAlwaysDonor flag for donation tracking
  - Password field marked [DisableAuditing] for security
  - Id nullable for create/update distinction
  - Mapped in CustomDtoMapper

#### Input DTOs
- **GetUsersInput.cs** - User query parameters:
  - Filter - Text search across user fields
  - Permissions - Filter by permission holdings
  - Role - Filter by role membership
  - OnlyLockedUsers - Show only locked accounts
  - Implements IShouldNormalize with default sorting "Name,Surname"
  - Implements IGetUsersInput interface

- **GetUsersToExcelInput.cs** - Export parameters for Excel generation
- **GetLinkedUsersInput.cs** - Query linked accounts
- **GetLoginAttemptsInput.cs** - Login history query (implements IGetLoginAttemptsInput)
- **CreateOrUpdateUserInput.cs** - User save operation with roles and optional organization units
- **LinkToUserInput.cs** - Link current account to another user account
- **UnlinkUserInput.cs** - Remove account link
- **ChangeUserLanguageDto.cs** - Update user's language preference
- **UpdateUserPermissionsInput.cs** - Manage user-specific permission grants

#### Output DTOs
- **GetUserForEditOutput.cs** - Complete user edit context:
  - User details (UserEditDto)
  - Profile picture
  - Assigned roles
  - All memberships
  - Available roles for assignment

- **GetUserPermissionsForEditOutput.cs** - Permission management context:
  - Current user permissions
  - All available permissions
  - Permission hierarchy

#### Bulk Operations
- **ImportUsersFromExcelJobArgs.cs** - Background job parameters for bulk user import from Excel files
- **IGetUsersInput.cs** - Interface for standardized user query inputs
- **IGetLoginAttemptsInput.cs** - Interface for login attempt queries

#### Role Association
- **UserRoleDto.cs** - User-role assignment representation

### Key Components

#### User Profile Structure
The system maintains comprehensive user profiles including:
- Identity information (Name, UserName, Email)
- Physical address (Street, Suite/Apt, City, State, Zip)
- Contact details (PhoneNumber with confirmation status)
- Security settings (2FA, lockout, password expiration)
- Personal data (DateOfBirth for compliance requirements)
- Behavioral flags (IsAlwaysDonor for donation patterns)

#### Account Linking
Users can link multiple accounts for:
- Switching between tenant contexts
- Connecting external logins
- Managing multiple identities

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- **Abp.Authorization.Users** - User base classes and constants
- **Abp.Domain.Entities** - Entity interfaces (IPassivable)
- **Abp.Auditing** - [DisableAuditing] attribute
- **System.ComponentModel.DataAnnotations** - Validation attributes
- **inzibackend.Dto** - Base paging DTOs

## Architecture Notes

### Validation Strategy
- Required fields enforced with [Required] attribute
- String length limits from AbpUserBase constants
- Email validation with [EmailAddress] attribute
- Password field excluded from auditing for security
- Empty password in edit = "don't change password"

### Security Patterns
- **Password Handling**: DisableAuditing prevents password logging
- **Login Tracking**: Login attempts recorded for security analysis
- **Account Locking**: IsLockoutEnabled support for security
- **Two-Factor Authentication**: IsTwoFactorEnabled flag
- **Email Confirmation**: IsEmailConfirmed verification status

### Multi-Tenancy
- Users belong to specific tenants
- Account linking allows cross-tenant user management
- Tenant isolation enforced at service level

### Performance Optimization
- UserEditDto mapped in CustomDtoMapper for efficient queries
- Paging and sorting support for large user lists
- Excel export for bulk data extraction
- Background jobs for bulk imports

## Business Logic

### User Lifecycle
1. **Registration/Creation**: CreateOrUpdateUserInput with Id = null
2. **Email Verification**: IsEmailConfirmed flag workflow
3. **Profile Management**: Users or admins edit via UserEditDto
4. **Role Assignment**: Users get multiple roles for access control
5. **Account Linking**: Connect multiple identities
6. **Deactivation**: IsActive flag for soft deletion
7. **Password Management**: ShouldChangePasswordOnNextLogin for security

### Permission Model
- **Role-Based**: Users inherit permissions from roles
- **User-Specific**: Additional permissions granted directly to users
- **Combined**: Runtime permissions = role permissions + user permissions

### Login Security
- Login attempts tracked for security monitoring
- Account lockout after failed attempts
- Two-factor authentication support
- Email and phone confirmation workflows

### Bulk Operations
- Excel import for onboarding large user groups
- Background job processing for performance
- Validation during import process
- Error reporting for failed imports

## Usage Across Codebase
These DTOs are consumed by:
- **IUserAppService** - User CRUD and management operations
- **User Management UI** - Admin interfaces for user administration
- **Profile Management** - Self-service user profile editing
- **Authentication System** - Login and security features
- **Bulk Import Services** - Background job processing
- **Role Management** - User-role assignment workflows
- **Reporting Systems** - User data exports and analytics

## Cross-Reference Impact
Changes to these DTOs affect:
- User management interfaces throughout the application
- Authentication and authorization systems
- Profile editing screens
- User import/export functionality
- Role assignment workflows
- Login security features
- Multi-tenant user isolation
- Account linking features
- Permission management interfaces