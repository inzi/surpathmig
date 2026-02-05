# Users Documentation

## Overview
Core user management domain layer implementing ASP.NET Core Identity with extensive customizations for multi-tenant SaaS operations. Includes user entity, managers, repositories, and supporting services for authentication, profile management, and password policies.

## Contents

### Core Files

#### User.cs
- **Purpose**: Primary user entity extending AbpUser
- **Key Features**:
  - Multi-tenant support
  - Extended profile fields
  - Google Authenticator integration
  - Password expiration tracking
  - Soft delete support

#### UserManager.cs
- **Purpose**: Extended ASP.NET Core Identity UserManager
- **Key Features**:
  - Custom password validation
  - User creation with tenant context
  - Permission management
  - Organization unit assignment

#### UserStore.cs
- **Purpose**: Custom implementation of IUserStore
- **Key Features**:
  - Repository-based persistence
  - Multi-tenant queries
  - Extended user operations

#### IUserRepository.cs
- **Purpose**: Custom repository interface for user operations
- **Key Methods**:
  - GetPasswordExpiredUserIds
  - UpdateUsersToChangePasswordOnNextLogin
  - Custom user queries

#### UserRegistrationManager.cs
- **Purpose**: Handles user registration logic
- **Key Features**:
  - Email/username validation
  - Tenant assignment
  - Welcome email sending
  - Default role assignment

### Supporting Services

#### UserClaimsPrincipalFactory.cs
- **Purpose**: Creates ClaimsPrincipal with custom claims
- **Key Features**: Tenant claims, custom user properties

#### UserPolicy.cs & IUserPolicy.cs
- **Purpose**: User account policies
- **Key Features**: Password requirements, account lockout rules

#### UserEmailer.cs & IUserEmailer.cs
- **Purpose**: User-related email notifications
- **Key Features**: Welcome emails, password resets, email confirmations

#### UserLinkManager.cs & IUserLinkManager.cs
- **Purpose**: Manages linked user accounts
- **Key Features**: Account linking, multi-account management

#### UserManagerExtensions.cs
- **Purpose**: Extension methods for UserManager
- **Key Features**: Convenience methods, query helpers

#### RecentPassword.cs
- **Purpose**: Tracks password history
- **Key Features**: Prevents password reuse

#### SwitchToLinkedAccountCacheItem.cs & Extensions
- **Purpose**: Caching for account switching
- **Key Features**: Fast account switching, session management

### Subfolders

#### Password/
- Password expiration services
- Password policy enforcement
- See [Password/CLAUDE.md](Password/CLAUDE.md)

#### Profile/
- Profile image management
- Gravatar integration
- See [Profile/CLAUDE.md](Profile/CLAUDE.md)

### Key Components

- **User**: Core user entity with extensive properties
- **UserManager**: Business logic for user operations
- **UserRegistrationManager**: Registration workflow
- **IUserRepository**: Data access interface

### Dependencies

- **External Libraries**:
  - Microsoft.AspNetCore.Identity
  - ABP Framework (Users, Authorization)

- **Internal Dependencies**:
  - Authorization system
  - Multi-tenancy
  - Organization units
  - Email services

## Architecture Notes

- **Pattern**: Repository and Domain Service patterns
- **Identity**: Extended ASP.NET Core Identity
- **Multi-tenancy**: Full tenant isolation
- **Caching**: Strategic caching for performance

## Business Logic

### User Lifecycle
1. Registration with email confirmation
2. Role and permission assignment
3. Organization unit placement
4. Profile completion
5. Password management
6. Account deactivation/deletion

### Security Features
- Two-factor authentication (Google Authenticator)
- Password expiration and history
- Account lockout policies
- Email confirmation requirements
- Session management

### Multi-tenancy
- Users belong to specific tenants
- Cross-tenant user linking
- Tenant-specific settings
- Isolated user data

## Usage Across Codebase

The User system is fundamental to:
- Authentication and authorization
- Tenant management
- Compliance tracking (via CohortUser)
- Audit trails
- Email notifications
- Permission checking
- Organization hierarchy

## Security Considerations

- Password policies enforced at domain level
- Two-factor authentication support
- Account lockout protection
- Email verification required
- Password history prevents reuse
- Secure password storage (hashed)
- Session invalidation on password change