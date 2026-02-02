# ApiClient/Models Documentation

## Overview
Contains authentication-related model classes used for API client authentication flows. These models define the data structures for authentication requests and responses between the client application and the server.

## Contents

### Files

#### AbpAuthenticateModel.cs
- **Purpose**: Defines the authentication request model for user login
- **Key Properties**:
  - `UserNameOrEmailAddress`: User credential identifier
  - `Password`: User password
  - `TwoFactorVerificationCode`: Optional 2FA code
  - `RememberClient`: Flag for persistent authentication
  - `TwoFactorRememberClientToken`: Token for remembered 2FA devices
  - `SingleSignIn`: Optional SSO flag
  - `ReturnUrl`: Post-authentication redirect URL
- **Features**: 
  - Implements `ISingletonDependency` for DI container registration
  - Contains `IsTwoFactorVerification` helper property

#### AbpAuthenticateResultModel.cs
- **Purpose**: Defines the authentication response model containing tokens and user session information
- **Key Properties**:
  - `AccessToken`: JWT access token for API authorization
  - `EncryptedAccessToken`: Encrypted version for secure transmission
  - `RefreshToken`: Token for refreshing expired access tokens
  - `ExpireInSeconds`: Access token expiration time
  - `UserId`: Authenticated user identifier
  - `RequiresTwoFactorVerification`: Indicates if 2FA is needed
  - `TwoFactorAuthProviders`: Available 2FA provider options
  - `RefreshTokenExpireDate`: Refresh token expiration timestamp
- **Security Features**:
  - Password reset indicators (`ShouldResetPassword`, `PasswordResetCode`)
  - Two-factor authentication support

### Key Components
- **Authentication Models**: Core data structures for the authentication flow
- **Token Management**: Support for access, refresh, and encrypted tokens
- **Two-Factor Authentication**: Built-in support for 2FA workflows

### Dependencies
- `Abp.Dependency`: For dependency injection interfaces
- `System.Collections.Generic`: For collections support

## Architecture Notes
- Models follow the ABP (ASP.NET Boilerplate) framework conventions
- Clear separation between request (AbpAuthenticateModel) and response (AbpAuthenticateResultModel) models
- Designed for stateless JWT-based authentication
- Support for multiple authentication scenarios (password, 2FA, SSO)

## Business Logic
- Authentication flow supports username or email-based login
- Two-factor authentication is optional but fully integrated
- Refresh token mechanism for long-lived sessions
- Password reset workflow integrated into authentication response

## Usage Across Codebase
- Used by `AccessTokenManager` for token lifecycle management
- Consumed by proxy services for authentication operations
- Referenced by authentication HTTP handlers for API requests
- Part of the client-side authentication infrastructure