# Authorization/Accounts/Dto Documentation

## Overview
Data Transfer Objects (DTOs) for account management operations including user registration, authentication, password management, email activation, tenant resolution, and user impersonation functionality.

## Contents

### Files

#### Registration and Account Creation
- **RegisterInput.cs** - User registration input DTO with comprehensive user profile fields including name, email, address, date of birth, phone number. Implements validation logic to ensure username is not an email unless it matches the primary email.
- **RegisterOutput.cs** - Response DTO for registration operations containing success status and registration result data.

#### Authentication and Token Management
- **RefreshTokenResult.cs** - DTO for token refresh operations, containing new access token and refresh token information.

#### Password Management
- **SendPasswordResetCodeInput.cs** - Input DTO for requesting password reset codes with email/username.
- **ResetPasswordInput.cs** - Input DTO for resetting password with reset code and new password.
- **ResetPasswordOutput.cs** - Response DTO for password reset operations.

#### Email Activation
- **SendEmailActivationLinkInput.cs** - Input DTO for requesting email activation links.
- **ActivateEmailInput.cs** - Input DTO for email activation with confirmation code.

#### Tenant Management
- **IsTenantAvailableInput.cs** - Input DTO for checking tenant availability by tenancy name.
- **IsTenantAvailableOutput.cs** - Output DTO containing tenant availability status and tenant ID.
- **TenantAvailabilityState.cs** - Enumeration defining possible tenant availability states.
- **ResolveTenantInput.cs** - Input DTO for resolving tenant information.
- **CurrentTenantInfoDto.cs** - DTO containing current tenant information for session context.

#### Impersonation
- **ImpersonateUserInput.cs** - Input DTO for impersonating a specific user with user ID and tenant ID.
- **ImpersonateTenantInput.cs** - Input DTO for impersonating a tenant administrator.
- **DelegatedImpersonateInput.cs** - Input DTO for delegated impersonation scenarios.
- **ImpersonateOutput.cs** - Output DTO containing impersonation token and tenancy name for impersonation sessions.

#### Account Switching
- **SwitchToLinkedAccountInput.cs** - Input DTO for switching between linked accounts.
- **SwitchToLinkedAccountOutput.cs** - Output DTO containing switch result and target account information.

### Key Components
- Validation attributes from System.ComponentModel.DataAnnotations
- ABP framework base classes (AbpUserBase) for field length constraints
- Custom validation logic implementing IValidatableObject
- Auditing control with [DisableAuditing] attribute for sensitive fields

### Dependencies
- Abp.Auditing - For audit control attributes
- Abp.Authorization.Users - For user base constants
- System.ComponentModel.DataAnnotations - For validation attributes
- inzibackend.Validation - Custom validation helpers
- inzibackend.Surpath.Dtos - Domain-specific DTOs

## Architecture Notes
- All DTOs follow a clear Input/Output naming convention for request/response pairs
- Sensitive fields like passwords are marked with [DisableAuditing] to prevent logging
- Validation is implemented both through data annotations and custom IValidatableObject implementation
- DTOs are designed to support multi-tenant architecture with tenant resolution and switching

## Business Logic
- Username validation ensures usernames cannot be email addresses unless they match the user's primary email
- Phone numbers must be exactly 10 digits as per business requirements
- Registration includes comprehensive user profile information including physical address
- Impersonation supports both user-level and tenant-level scenarios
- Account switching enables users to navigate between linked accounts

## Usage Across Codebase
These DTOs are primarily consumed by:
- IAccountAppService interface for defining account management operations
- AccountAppService implementation in the Application layer
- Account controllers in the Web.Mvc layer for handling HTTP requests
- Authentication and authorization middleware components