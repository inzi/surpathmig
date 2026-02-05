# Authorization Documentation

## Overview
Comprehensive authorization system built on ABP Framework and ASP.NET Core Identity, providing role-based and permission-based access control with support for multi-tenancy, user delegation, impersonation, and LDAP integration.

## Contents

### Core Files

#### AppAuthorizationProvider.cs
- **Purpose**: Defines all application permissions
- **Key Features**:
  - Hierarchical permission structure
  - Multi-tenant permission scoping
  - Feature-dependent permissions

#### AppPermissions.cs
- **Purpose**: Static permission name constants
- **Key Permissions**:
  - Pages and navigation permissions
  - CRUD operation permissions
  - Surpath-specific permissions
  - Administrative permissions

#### PermissionChecker.cs
- **Purpose**: Custom permission checking logic
- **Key Features**:
  - User-based permission checks
  - Role-based permission aggregation
  - Caching for performance

### Subfolders

#### Users/
- User entity and management
- Registration and authentication
- Password policies
- Profile management
- See [Users/CLAUDE.md](Users/CLAUDE.md)

#### Roles/
- Role definitions and management
- Static and dynamic roles
- Role-permission mappings
- See [Roles/CLAUDE.md](Roles/CLAUDE.md)

#### Delegation/
- User delegation system
- Temporary permission grants
- Time-bound delegations
- See [Delegation/CLAUDE.md](Delegation/CLAUDE.md)

#### Impersonation/
- User impersonation for support
- Secure session management
- Audit trail
- See [Impersonation/CLAUDE.md](Impersonation/CLAUDE.md)

#### Ldap/
- LDAP/Active Directory integration
- External authentication
- User synchronization

### Key Components

- **AppAuthorizationProvider**: Permission definition registry
- **AppPermissions**: Permission name constants
- **PermissionChecker**: Runtime permission validation

### Permission Structure

#### Hierarchy
- **Pages**: Top-level navigation permissions
- **Administration**: System administration
- **Surpath**: Compliance-specific permissions
  - Drug testing permissions
  - Background check permissions
  - Document management permissions
  - Compliance review permissions

#### Permission Types
- **View**: Read-only access
- **Create**: Add new records
- **Edit**: Modify existing records
- **Delete**: Remove records
- **Download**: File access permissions

### Dependencies

- **External Libraries**:
  - ABP Framework (Authorization module)
  - ASP.NET Core Identity
  - System.DirectoryServices (LDAP)

- **Internal Dependencies**:
  - User management
  - Role management
  - Multi-tenancy
  - Feature management

## Architecture Notes

- **Pattern**: Provider pattern for permission registration
- **Caching**: Permission results cached for performance
- **Extensibility**: Easy to add new permissions
- **Multi-tenancy**: Tenant-specific permission scopes

## Business Logic

### Permission Checking Flow
1. User authentication verification
2. Role permission aggregation
3. Direct user permissions
4. Delegation permissions
5. Feature-based filtering
6. Result caching

### Special Permissions

#### Surpath Permissions
- **Drug Screen Download**: Access to drug test results
- **Background Check Download**: Access to background check reports
- **Compliance Review**: Administrative compliance oversight
- **CohortUser Management**: Student/user assignment

#### Administrative
- **Host Dashboard**: Super-admin access
- **Tenant Management**: Multi-tenant administration
- **User Management**: User CRUD operations
- **Role Management**: Permission assignment

## Usage Across Codebase

The authorization system is used throughout:
- Controller action authorization
- Service method security
- UI element visibility
- Navigation menu generation
- File download access
- API endpoint protection
- Report generation
- Workflow approvals

## Security Considerations

- Principle of least privilege
- Defense in depth
- Permission inheritance
- Audit logging
- Cache invalidation on changes
- Secure delegation
- Impersonation controls
- LDAP security

## Configuration

### Static Roles
- Admin: Full system access
- User: Basic user permissions
- Custom roles: Defined per tenant

### Dynamic Permissions
- Created at runtime
- Tenant-specific
- Feature-dependent
- User-assignable