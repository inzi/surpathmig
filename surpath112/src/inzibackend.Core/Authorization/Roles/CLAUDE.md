# Roles Documentation

## Overview
Role management system providing role-based access control (RBAC) with hierarchical permission assignment, organizational unit assignment, and static/dynamic role support.

## Contents

### Files

#### Role.cs
- **Purpose**: Core role entity representing a role in the system
- **Base Class**: `AbpRole<User>` from ABP Framework
- **Key Features**:
  - Multi-tenant role support
  - Display name and internal name
  - Static and dynamic roles
  - Permission assignments
  - Organizational unit association
- **Constructors**:
  - Default (parameterless)
  - With tenantId and displayName
  - With tenantId, name, and displayName

#### RoleManager.cs
- **Purpose**: Business logic for role operations
- **Base Class**: `AbpRoleManager<Role, User>` from ABP Framework
- **Key Features**:
  - Role CRUD operations
  - Permission management
  - Organizational unit assignments
  - Role validation
  - Permission inheritance
  - Protected admin role permissions
- **Key Methods**:
  - `SetGrantedPermissionsAsync`: Assign permissions to role
  - `GetRoleByIdAsync`: Retrieve role by ID
  - `CheckPermissionsToUpdate`: Validate permission changes
- **Business Rules**:
  - Admin role cannot have critical permissions removed
  - Prevents removal of user management permissions from Admin

#### RoleStore.cs
- **Purpose**: Data access layer for role operations
- **Base Class**: `AbpRoleStore<Role, User>` from ABP Framework
- **Functionality**: Repository pattern implementation for roles

#### AppRoleConfig.cs
- **Purpose**: Role configuration and setup
- **Key Features**: (inferred)
  - Static role definitions
  - Default permission assignments
  - Role creation on startup

### Key Components

- **Role**: Domain entity for roles
- **RoleManager**: Business logic layer
- **RoleStore**: Data access layer
- **AppRoleConfig**: Configuration provider

### Dependencies

- **External Libraries**:
  - ABP Framework (Authorization module)
  - ASP.NET Core Identity
  - Entity Framework Core

- **Internal Dependencies**:
  - `User` entity
  - Permission system
  - Organizational units
  - Localization system

## Architecture Notes

- **Pattern**: Repository and manager pattern from ABP
- **Multi-Tenancy**: Roles scoped to tenant
- **Inheritance**: Permission inheritance via ABP
- **Validation**: Business rule enforcement in manager

## Business Logic

### Role Types

#### Static Roles
Predefined system roles:
- **Admin**: Full system access (tenant admin)
- **User**: Standard user permissions
- **Host Admin**: Super admin (host level)

#### Dynamic Roles
- Created by administrators
- Custom permission sets
- Tenant-specific
- Can be modified and deleted

### Permission Assignment
1. Permissions assigned to roles
2. Users assigned to roles
3. Users inherit role permissions
4. Direct user permissions can override
5. Multiple role membership (additive)

### Role Protection
- Admin role has special protections
- Cannot remove critical permissions:
  - `Pages_Administration_Roles_Edit`
  - `Pages_Administration_Users_ChangePermissions`
- Prevents system lockout scenarios

### Organizational Units
- Roles can be assigned to OUs
- Users inherit OU role assignments
- Hierarchical permission structure
- Department/team-based access control

## Usage Across Codebase

Used by:
- User management (role assignment)
- Authorization checks
- Permission evaluation
- Navigation menu rendering
- UI element visibility
- Administrative interfaces
- Tenant setup and initialization

## Security Considerations

### Role Protection
- Admin role cannot be deleted
- Critical permissions protected
- Permission removal validation
- Audit logging of role changes

### Permission Management
- Principle of least privilege
- Granular permission assignment
- Role-based segregation of duties
- Regular permission reviews

### Best Practices
- Minimize admin role members
- Use dynamic roles for custom needs
- Document role purposes
- Regular audit of role assignments
- Remove unused roles
- Test permission changes in non-production

## Configuration

### Static Roles Created on Startup
- Host Admin (host side)
- Admin (tenant side)
- User (tenant side)

### Default Permissions
- Assigned during tenant initialization
- Defined in seed data
- Customizable per tenant

## Common Operations

### Creating a Role
1. Define role name and display name
2. Assign permissions
3. Optionally assign to OUs
4. Save to database
5. Cache permissions

### Assigning Users to Roles
1. Validate user exists
2. Check role exists
3. Create user-role relationship
4. Invalidate permission cache
5. Log assignment

### Modifying Role Permissions
1. Load existing role
2. Validate permission changes
3. Check for protected permissions
4. Update permission grants
5. Invalidate caches
6. Notify affected users (optional)

## Extension Points

- Custom role validation rules
- Additional role properties
- Role inheritance (custom)
- Dynamic permission calculation
- Role templates
- Role approval workflows