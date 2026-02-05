# Sessions Documentation

## Overview
Session management service interface and DTOs for handling user login information, tenant context, application metadata, and authentication token management in the multi-tenant SaaS application.

## Contents

### Files

#### Service Interface
- **ISessionAppService.cs** - Session service interface providing:
  - Current login information retrieval
  - User sign-in token updates for security

### Key Components
- **ISessionAppService** - Core session management service
  - `GetCurrentLoginInformations()` - Retrieves comprehensive session data
  - `UpdateUserSignInToken()` - Refreshes security tokens

### Dependencies
- Abp.Application.Services - ABP framework service base
- Sessions.Dto - Session-related DTOs

## Subfolders

### Dto
Contains session-related Data Transfer Objects:

#### Core Session DTOs
- **GetCurrentLoginInformationsOutput.cs** - Comprehensive session information including user, tenant, and application data
- **UserLoginInfoDto.cs** - Current user information including roles and permissions
- **TenantLoginInfoDto.cs** - Tenant context information
- **ApplicationInfoDto.cs** - Application metadata and configuration

#### Security
- **UpdateUserSignInTokenOutput.cs** - Result of token refresh operations

#### Subscription & Licensing
- **EditionInfoDto.cs** - Edition/plan information for the tenant
- **SubscriptionPaymentInfoDto.cs** - Payment and subscription status

## Architecture Notes
- Session management follows ABP's multi-tenant session pattern
- Stateless design with JWT tokens for authentication
- Session information aggregates user, tenant, and application context
- Token refresh mechanism for security without re-authentication

## Business Logic
- **Session Context**: Provides complete context for the current user including tenant, roles, and permissions
- **Multi-Tenancy**: Session aware of current tenant context for data isolation
- **Edition Features**: Session includes edition/plan information for feature toggling
- **Token Management**: Automatic token refresh for seamless user experience
- **Security**: Sign-in tokens updated periodically for security

## Usage Across Codebase
This session service is used by:
- Web.Mvc controllers for user context
- Authorization filters and middleware
- Client-side JavaScript for user information
- Feature availability checks
- Tenant-specific configurations
- Navigation and menu generation based on permissions