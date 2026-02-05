# Authorization/Users Documentation

## Overview
Contains proxy service implementations for user management operations. This folder provides client-side access to user administration functionality including CRUD operations, permission management, and user account control.

## Contents

### Files

#### ProxyUserAppService.cs
- **Purpose**: Client proxy for IUserAppService, handling comprehensive user management
- **Key Methods**:
  - `GetUsers`: Retrieves paginated list of users with filtering
  - `GetUsersToExcel`: Exports user data to Excel format
  - `GetUserForEdit`: Retrieves user details for editing
  - `GetUserPermissionsForEdit`: Fetches user-specific permissions
  - `ResetUserSpecificPermissions`: Resets custom permissions to defaults
  - `UpdateUserPermissions`: Updates user permission assignments
  - `CreateOrUpdateUser`: Creates new or updates existing users
  - `DeleteUser`: Removes user from system
  - `UnlockUser`: Unlocks locked user accounts
- **Base Class**: Inherits from ProxyAppServiceBase
- **Interface**: Implements IUserAppService

### Key Components
- **User Management**: Complete CRUD operations for user accounts
- **Permission System**: Granular permission management per user
- **Data Export**: Excel export functionality for user lists
- **Account Control**: Lock/unlock user accounts

### Dependencies
- `Abp.Application.Services.Dto`: ABP framework DTOs (PagedResultDto, EntityDto)
- `inzibackend.Authorization.Users.Dto`: User-specific DTOs
- `inzibackend.Dto`: General application DTOs (FileDto)
- `ProxyAppServiceBase`: Base proxy service class

## Subfolders

### Profile
Contains profile-specific proxy services for user profile management, including profile editing, password changes, profile pictures, and authentication settings.

## Architecture Notes
- **Proxy Pattern**: Implements remote proxy for server-side user services
- **Async Operations**: All methods use async/await for non-blocking I/O
- **DTO Pattern**: Uses Data Transfer Objects for all data exchange
- **RESTful Mapping**: Methods map to RESTful API endpoints
- **Paging Support**: Built-in pagination for user lists

## Business Logic
- **User Lifecycle**: Supports full user lifecycle from creation to deletion
- **Permission Management**: 
  - User-specific permissions override role-based permissions
  - Reset functionality to revert to role defaults
- **Account Security**: Account locking/unlocking for security purposes
- **Data Export**: Allows administrative export of user data
- **Null-Safe Operations**: Uses NullableIdDto for optional ID parameters

## Usage Across Codebase
- Used by admin UI for user management screens
- Referenced in permission management interfaces
- Consumed by user import/export features
- Integrated with role management for permission inheritance
- Part of the administrative dashboard functionality

## Cross-References
- **Related Services**: Works with role and permission services
- **Profile Services**: Profile subfolder extends user functionality
- **Authentication**: Coordinates with authentication services for account control
- **Tenant Management**: Respects multi-tenant boundaries for user operations