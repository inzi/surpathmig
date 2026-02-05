# Authentication Documentation

## Overview
This folder contains the complete authentication infrastructure for the web application, including JWT bearer token authentication, external authentication provider integration, and two-factor authentication support. It provides a comprehensive security layer for both API and web-based authentication scenarios.

## Contents

### Files
*No files directly in this folder - functionality organized in subfolders*

### Key Components
- **JWT Bearer Authentication**: Complete JWT token infrastructure
- **External Authentication**: OAuth and WS-Federation provider support
- **Two-Factor Authentication**: 2FA code caching and management

### Dependencies
- Microsoft.AspNetCore.Authentication (ASP.NET Core auth framework)
- Microsoft.IdentityModel.Tokens (Token handling)
- Abp.AspNetZeroCore.Web (ABP Zero authentication base)

## Subfolders

### External
Manages external authentication providers (OAuth, WS-Federation, social logins)
- **Factory Pattern**: Dynamic provider selection
- **Claims Processing**: Extracts user info from external providers
- **Provider Support**: Google, Microsoft, Facebook, WS-Federation
- **Business Value**: Enables SSO and reduces password fatigue

### JwtBearer
Implements JWT bearer token authentication for APIs
- **Token Management**: Creation, validation, and refresh
- **Security Stamps**: Prevents token reuse after security changes
- **Async Processing**: High-performance token validation
- **Multi-tenant Support**: Tenant-isolated tokens
- **Business Value**: Secure, scalable API authentication

### TwoFactor
Provides two-factor authentication infrastructure
- **Code Caching**: Temporary storage of 2FA codes
- **Multiple Methods**: SMS, email, authenticator app support
- **Security**: Time-based expiration and single-use codes
- **Business Value**: Enhanced account security

## Architecture Notes
- **Layered Security**: Multiple authentication methods for different scenarios
- **Provider Agnostic**: Easily add new authentication providers
- **Performance Optimized**: Caching and async operations throughout
- **Multi-tenant Ready**: Full tenant isolation in all auth flows
- **Standards Compliant**: OAuth 2.0, OpenID Connect, JWT standards

## Business Logic
- **Authentication Flow**:
  1. User provides credentials (internal or external)
  2. System validates and generates JWT token
  3. Optional 2FA challenge if enabled
  4. Token used for subsequent API calls
  5. Refresh token extends session without re-authentication

- **Security Features**:
  - Password complexity requirements
  - Account lockout on failed attempts
  - Security stamp invalidation
  - Token expiration and refresh
  - Multi-factor authentication

## Security Considerations
- Tokens signed with symmetric keys (HMAC-SHA256)
- Security stamps prevent token reuse after password changes
- Refresh tokens enable revocation without affecting all sessions
- External auth reduces password exposure
- 2FA significantly increases account security

## Usage Across Codebase
- **Controllers**: All API controllers use JWT authentication
- **TokenAuthController**: Main authentication endpoint
- **External Integrations**: Mobile apps, SPAs, third-party services
- **Background Jobs**: Authenticated job execution
- **SignalR Hubs**: Real-time authenticated connections
- **Admin Features**: Tenant and user management

## Configuration
Key settings in appsettings.json:
- `Authentication:JwtBearer:SecurityKey`: Token signing key
- `Authentication:JwtBearer:Issuer`: Token issuer
- `Authentication:JwtBearer:Audience`: Token audience
- `Authentication:TwoFactor:IsEnabled`: 2FA enablement
- External provider settings (client IDs, secrets)

## Extension Points
- Custom authentication providers via IExternalLoginInfoManager
- Custom token validators via IAsyncSecurityTokenValidator
- Custom 2FA providers
- Custom claims transformation