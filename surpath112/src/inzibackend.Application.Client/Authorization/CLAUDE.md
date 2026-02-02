# Authorization Documentation

## Overview
Contains all authorization-related proxy services including account management, user administration, and profile management. This folder provides client-side access to the complete authorization and user management infrastructure.

## Contents

### Key Components
- **Account Services**: Registration, password management, tenant resolution
- **User Services**: User CRUD operations and permission management
- **Profile Services**: User profile and authentication settings

### Dependencies
- `ProxyAppServiceBase`: Base class for all proxy services
- `inzibackend.Authorization.*Dto`: Authorization-related DTOs
- `ApiClient`: Core HTTP client for API communication

## Subfolders

### Accounts
Handles account-level operations including:
- User registration and email activation
- Password reset workflows
- Tenant resolution and validation
- User and tenant impersonation
- Linked account management

### Users
Manages user administration including:
- User CRUD operations
- Permission management
- User data export
- Account locking/unlocking

### Users/Profile
Provides profile management functionality:
- Profile editing and updates
- Password changes
- Profile picture management
- Two-factor authentication setup
- Language preferences
- GDPR data collection

## Architecture Notes
- **Proxy Pattern**: All services implement remote proxy pattern
- **Async/Await**: Fully asynchronous operations
- **Multi-tenancy**: Full multi-tenant support across all services
- **Security Layers**: Appropriate authentication for each endpoint

## Business Logic
- **User Lifecycle**: Complete user journey from registration to deletion
- **Authentication**: Multiple authentication methods (password, 2FA, impersonation)
- **Authorization**: Fine-grained permission management
- **Account Security**: Password policies, account locking, email verification

## Usage Across Codebase
- Core authentication infrastructure for client applications
- Used by all user-facing UI components
- Referenced in admin interfaces for user management
- Integrated with security and permission systems

## Security Architecture
- **Public Endpoints**: Registration, password reset (anonymous access)
- **Protected Endpoints**: User management, profile updates (requires auth)
- **Admin Endpoints**: Impersonation, permission management (elevated privileges)

## Cross-References
- **ApiClient**: Uses core API client for all HTTP operations
- **Application.Shared**: References shared DTOs and interfaces
- **UI Components**: Consumed by login, registration, and profile screens
- **Admin Dashboard**: Powers user management interfaces