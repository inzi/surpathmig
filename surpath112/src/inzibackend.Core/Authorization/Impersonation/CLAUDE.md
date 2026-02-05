# Impersonation Documentation

## Overview
User impersonation system allowing administrators to act as other users for support and troubleshooting. Includes secure caching and identity management with full audit trail.

## Contents

### Files

#### IImpersonationManager.cs
- **Purpose**: Interface for impersonation operations
- **Key Methods**: Begin/end impersonation, validation

#### ImpersonationManager.cs
- **Purpose**: Core impersonation logic implementation
- **Key Features**: Permission checking, session management, audit logging

#### ImpersonationCacheItem.cs
- **Purpose**: Cached impersonation session data
- **Key Data**: Impersonator ID, target user ID, tenant information

#### ImpersonationCacheManagerExtensions.cs
- **Purpose**: Cache management extension methods
- **Key Features**: Cache operations, session retrieval

#### UserAndIdentity.cs
- **Purpose**: Combined user and identity information
- **Key Properties**: User entity, claim identity

### Key Components

- **ImpersonationManager**: Core business logic
- **ImpersonationCacheItem**: Session storage
- **UserAndIdentity**: Identity wrapper

### Dependencies

- **External Libraries**:
  - ABP Framework (Caching, authorization)
  - ASP.NET Core Identity

- **Internal Dependencies**:
  - User management
  - Permission system
  - Tenant system

## Architecture Notes

- **Security**: Permission-based access control
- **Caching**: Session-based impersonation tracking
- **Auditing**: Full trail of impersonation activities

## Business Logic

- Admin-only impersonation rights
- Cross-tenant impersonation support
- Automatic session cleanup
- Identity preservation

## Usage Across Codebase

Used for:
- Customer support operations
- Troubleshooting user issues
- Testing user experiences
- Administrative oversight