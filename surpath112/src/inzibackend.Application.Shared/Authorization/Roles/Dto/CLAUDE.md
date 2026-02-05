# Roles DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for role management in the authorization system. Roles are collections of permissions that can be assigned to users to control their access to features and functionality. The system supports both static (system-defined) and dynamic (tenant-defined) roles with flexible permission assignments.

## Contents

### Files

#### List & Display DTOs
- **RoleListDto.cs** - Role list item with metadata:
  - Name - Internal role name (unique identifier)
  - DisplayName - User-facing role label
  - IsStatic - System role that cannot be deleted (e.g., Admin)
  - IsDefault - Automatically assigned to new users
  - CreationTime - Audit timestamp
  - Implements IHasCreationTime for audit tracking

#### Edit DTOs
- **RoleEditDto.cs** - Role creation/update data:
  - Id - Nullable for create vs update scenarios
  - DisplayName - Required user-friendly name
  - IsDefault - Flag for auto-assignment to new users
  - Validation: DisplayName is required

#### Input DTOs
- **GetRolesInput.cs** - Query parameters for role listing (filtering, paging, sorting)

- **CreateOrUpdateRoleInput.cs** - Complete role save operation:
  - Role details (RoleEditDto)
  - Permission assignments (list of permission names)
  - Handles both create and update in single operation

#### Output DTOs
- **GetRoleForEditOutput.cs** - Complete role edit context:
  - Role details for editing
  - All available permissions for assignment
  - Current permission grants for the role
  - Used to populate role editing forms

### Key Components

#### Role Types
- **Static Roles**: System-defined roles (Admin, User) that cannot be deleted
- **Dynamic Roles**: Tenant-created custom roles
- **Default Roles**: Roles automatically assigned to newly registered users

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO interfaces
- **Abp.Domain.Entities.Auditing** - Audit interfaces
- **System.ComponentModel.DataAnnotations** - Validation attributes

## Architecture Notes

### Role Management Pattern
- **Separation of Concerns**: Different DTOs for list, edit, create operations
- **Composite Operations**: CreateOrUpdateRoleInput combines role data with permissions
- **Edit Context**: GetRoleForEditOutput provides everything needed for role form
- **Validation**: Required fields enforced with data annotations

### Multi-Tenancy
- Each tenant has its own set of dynamic roles
- Static roles are shared across tenants
- Tenants cannot modify static roles

### Default Role Behavior
- Only one role should be marked as default per tenant
- New users automatically get the default role assigned
- Prevents new users from having no access

## Business Logic

### Role Lifecycle
1. **Creation**: CreateOrUpdateRoleInput with Id = null
2. **Permission Assignment**: Permissions sent as list of permission names
3. **User Assignment**: Users can have multiple roles
4. **Deletion**: Only dynamic roles can be deleted (IsStatic = false)

### Permission Inheritance
- Roles aggregate permissions (union of all granted permissions)
- Users with multiple roles get combined permissions
- No explicit permission hierarchy in roles

### Static Role Protection
- Admin role cannot be deleted or renamed
- Static roles provide baseline system functionality
- Ensures system always has administrative access

## Usage Across Codebase
These DTOs are consumed by:
- **IRoleAppService** - Role CRUD operations
- **Role Management UI** - Admin interface for role configuration
- **User Management** - Assigning roles to users
- **Authorization System** - Runtime permission checking
- **Tenant Setup** - Creating default roles for new tenants

## Cross-Reference Impact
Changes to these DTOs affect:
- Role management interfaces in admin panels
- User assignment workflows
- Permission configuration screens
- Multi-tenant role isolation
- Default user setup processes