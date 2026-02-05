# Authorization/Accounts Documentation

## Overview
Contains proxy service implementations for account management operations. These services handle user registration, password management, tenant resolution, email activation, and impersonation features.

## Contents

### Files

#### ProxyAccountAppService.cs
- **Purpose**: Client proxy for IAccountAppService, handling account-related operations
- **Key Methods**:
  - `IsTenantAvailable`: Checks if a tenant name is available
  - `ResolveTenantId`: Resolves tenant ID from tenant name
  - `Register`: New user registration
  - `SendPasswordResetCode`: Initiates password reset flow
  - `ResetPassword`: Completes password reset with code
  - `SendEmailActivationLink`: Sends email verification link
  - `ActivateEmail`: Activates user email with verification code
  - `ImpersonateUser`: Admin impersonates another user
  - `ImpersonateTenant`: Admin impersonates a tenant
  - `BackToImpersonator`: Returns to original user from impersonation
  - `SwitchToLinkedAccount`: Switches between linked accounts
  - `DelegatedImpersonate`: Performs delegated impersonation
- **Authentication**: Most methods use PostAnonymousAsync for public access
- **Interface**: Implements IAccountAppService

#### ProxyTokenAuthControllerService.cs
- **Purpose**: Client proxy for token-based authentication controller
- **Features**: Handles JWT token authentication operations
- **Usage**: Alternative authentication path via controller endpoints

### Key Components
- **Account Management**: User registration and activation
- **Password Recovery**: Complete password reset workflow
- **Multi-tenancy**: Tenant resolution and validation
- **Impersonation**: Administrative user/tenant impersonation
- **Email Verification**: Email activation workflow

### Dependencies
- `inzibackend.Authorization.Accounts.Dto`: Account-specific DTOs
- `ProxyAppServiceBase`: Base class for proxy services

## Architecture Notes
- **Anonymous Access**: Most endpoints allow anonymous access for registration/login
- **Impersonation Security**: Impersonation requires authentication
- **Multi-tenant Support**: Built-in tenant resolution and validation
- **Email Workflow**: Complete email verification lifecycle
- **Token Authentication**: Supports JWT-based authentication

## Business Logic
- **Registration Flow**:
  1. Check tenant availability
  2. Register new user
  3. Send email activation link
  4. Activate email with code
- **Password Reset Flow**:
  1. Send password reset code
  2. Verify code and reset password
- **Impersonation**:
  - Admin can impersonate users or tenants
  - Maintains impersonation chain
  - Can return to original context
- **Linked Accounts**: Support for switching between related accounts

## Usage Across Codebase
- Used by login/registration UI components
- Referenced in password recovery screens
- Consumed by admin dashboard for impersonation
- Integrated with tenant selection interfaces
- Part of the authentication infrastructure

## Security Considerations
- Anonymous endpoints for public operations
- Authenticated endpoints for sensitive operations
- Impersonation requires elevated permissions
- Email verification prevents unauthorized registrations
- Password reset uses secure token mechanism