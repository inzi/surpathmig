# Token Authentication Models Documentation

## Overview
This folder contains the data transfer objects (DTOs) and view models used for token-based authentication flows. These models define the contracts for authentication requests and responses, including standard login, external authentication, impersonation, and two-factor authentication.

## Contents

### Files

#### AuthenticateModel.cs
- **Purpose**: Input model for standard authentication requests
- **Key Properties**:
  - `UserNameOrEmailAddress`: User identifier
  - `Password`: User password
  - `RememberClient`: Remember device for 2FA
  - `TwoFactorCode`: Optional 2FA code
  - `TwoFactorProvider`: 2FA provider type

#### AuthenticateResultModel.cs
- **Purpose**: Response model for successful authentication
- **Key Properties**:
  - `AccessToken`: JWT access token
  - `RefreshToken`: Refresh token for renewal
  - `ExpireInSeconds`: Token expiration time
  - `UserId`: Authenticated user ID
  - `RequiresTwoFactor`: Whether 2FA is needed

#### ExternalAuthenticateModel.cs
- **Purpose**: Input model for external provider authentication
- **Properties**:
  - `AuthProvider`: External provider name (Google, Microsoft, etc.)
  - `ProviderKey`: User's ID from external provider
  - `ProviderAccessCode`: OAuth access token

#### ExternalAuthenticateResultModel.cs
- **Purpose**: Response for external authentication
- **Properties**:
  - `AccessToken`: JWT token if successful
  - `WaitingForActivation`: If account needs activation
  - `ReturnUrl`: Post-authentication redirect URL

#### ExternalLoginProviderInfoModel.cs
- **Purpose**: Information about available external login providers
- **Properties**:
  - `Name`: Provider display name
  - `ClientId`: OAuth client ID
  - `AdditionalParams`: Provider-specific parameters

#### ImpersonateModel.cs
- **Purpose**: Request model for user impersonation
- **Properties**:
  - `TenantId`: Target tenant
  - `UserId`: User to impersonate
  - `ImpersonationToken`: Security token

#### ImpersonateResultModel.cs
- **Purpose**: Response for impersonation requests
- **Properties**:
  - `AccessToken`: Impersonation JWT token
  - `ImpersonatedTenantId`: Target tenant ID
  - `ImpersonatedUserId`: Target user ID

#### ImpersonatedAuthenticateResultModel.cs
- **Purpose**: Result of authentication while impersonating
- **Properties**:
  - Extends AuthenticateResultModel
  - `ImpersonationToken`: Token to return to original user

#### SendTwoFactorAuthCodeModel.cs
- **Purpose**: Model for sending 2FA codes
- **Properties**:
  - `UserId`: Target user
  - `Provider`: SMS, Email, or Authenticator
  - `Code`: Generated 2FA code

#### SwitchedAccountAuthenticateResultModel.cs
- **Purpose**: Result when switching between linked accounts
- **Properties**:
  - `AccessToken`: New account's token
  - `SwitchedTenantId`: New tenant context
  - `SwitchedUserId`: New user context

### Key Components
- **Authentication Models**: Standard login flow DTOs
- **External Auth Models**: OAuth/SSO integration models
- **Impersonation Models**: Admin user switching
- **2FA Models**: Multi-factor authentication flow

### Dependencies
- System.ComponentModel.DataAnnotations (validation)
- Newtonsoft.Json (JSON serialization)

## Architecture Notes
- Models follow single responsibility principle
- Validation attributes ensure data integrity
- Nullable properties for optional features
- Inheritance used for common properties

## Business Logic
- **Authentication Flow Models**:
  1. AuthenticateModel → TokenAuthController
  2. Validation and processing
  3. AuthenticateResultModel returned
  4. Client stores tokens

- **External Auth Flow**:
  1. ExternalAuthenticateModel with provider token
  2. Provider validation
  3. User creation/mapping
  4. Standard token response

## Security Considerations
- Passwords never stored in models
- Tokens have expiration times
- Impersonation requires special permissions
- 2FA codes are time-limited

## Usage Across Codebase
- TokenAuthController uses all models
- Mobile apps consume these contracts
- SPAs use for authentication
- Integration tests validate flows