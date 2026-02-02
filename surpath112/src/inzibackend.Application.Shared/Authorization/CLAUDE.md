# Authorization Documentation

## Overview
This folder contains service interfaces and DTOs for the authorization system including user management, role management, permissions, account operations, and user delegation. The authorization system provides comprehensive identity and access management for the multi-tenant application.

## Contents

### Files

The folder contains service interfaces for core authorization functionality. See subfolders for complete functionality breakdown.

### Subfolders

#### Accounts
Account management including registration, activation, password management, and account linking.
[Full details in Accounts/CLAUDE.md](Accounts/CLAUDE.md)

#### Permissions
Permission definition and management for fine-grained access control.
[Full details in Permissions/Dto/CLAUDE.md](Permissions/Dto/CLAUDE.md)

#### Roles
Role-based access control including role CRUD and permission assignment.
[Full details in Roles/Dto/CLAUDE.md](Roles/Dto/CLAUDE.md)

#### Users
Comprehensive user management including profiles, delegation, and bulk operations.
Subfolders:
- **Users/Dto** - User management DTOs [Details](Users/Dto/CLAUDE.md)
- **Users/Profile** - User profile self-service [Details](Users/Profile/Dto/CLAUDE.md)
- **Users/Delegation** - User delegation system [Details](Users/Delegation/Dto/CLAUDE.md)

## Architecture Notes

### Authorization Model
- **Users**: Individual authenticated accounts
- **Roles**: Collections of permissions
- **Permissions**: Fine-grained access controls
- **Organization Units**: Hierarchical permission scoping

### Multi-Tenant Authorization
- Users belong to specific tenants
- Roles tenant-specific or shared
- Permissions evaluated within tenant context
- Cross-tenant operations require special handling

### Security Features
- Two-factor authentication
- Password complexity enforcement
- Account lockout protection
- Email/SMS verification
- Impersonation with audit trail
- User delegation for temporary access

## Business Logic

### User Lifecycle
1. Registration → Email verification → Activation
2. Role assignment → Permission inheritance
3. Profile management → Security settings
4. Delegation → Impersonation → Account linking
5. Deactivation → Deletion

### Permission Checking
- Runtime permission evaluation
- Role-based access control (RBAC)
- User-specific permission grants
- Organization unit permissions
- Multi-factor authentication requirements

## Usage Across Codebase
These interfaces support:
- Authentication workflows
- Authorization attribute checking
- User management interfaces
- Role administration
- Permission configuration
- Account self-service features

## Cross-Reference Impact
Changes affect:
- Login and authentication
- Permission checking throughout application
- User management interfaces
- Role configuration screens
- Account registration flows
- Security settings