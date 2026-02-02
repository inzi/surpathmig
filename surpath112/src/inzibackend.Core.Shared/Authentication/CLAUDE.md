# Authentication Documentation

## Overview
Configuration classes for external authentication providers supporting social login and federated authentication scenarios. Enables users to authenticate using third-party identity providers.

## Contents

### Files

#### FacebookExternalLoginProviderSettings.cs
- **Purpose**: Configuration settings for Facebook OAuth authentication
- **Key Properties**:
  - AppId: Facebook application identifier
  - AppSecret: Facebook application secret
- **Validation**: IsValid() ensures both AppId and AppSecret are present

#### GoogleExternalLoginProviderSettings.cs
- **Purpose**: Configuration settings for Google OAuth authentication
- **Key Properties**:
  - ClientId: Google OAuth client identifier
  - ClientSecret: Google OAuth client secret
  - UserInfoEndpoint: Optional endpoint for user information retrieval
- **Validation**: IsValid() ensures ClientId and ClientSecret are present

#### JsonClaimMapDto.cs
- **Purpose**: DTO for mapping JSON claims to application claims
- **Key Properties**:
  - Claim: The claim type to map to
  - Key: The JSON key to extract the value from
- **Usage**: Maps external provider claims to internal application claims

#### MicrosoftExternalLoginProviderSettings.cs
- **Purpose**: Configuration settings for Microsoft Account authentication
- **Key Properties**:
  - ClientId: Microsoft application client ID
  - ClientSecret: Microsoft application client secret
- **Validation**: IsValid() ensures both credentials are present

#### OpenIdConnectExternalLoginProviderSettings.cs
- **Purpose**: Configuration for OpenID Connect providers
- **Key Properties**:
  - ClientId: OIDC client identifier
  - ClientSecret: OIDC client secret
  - Authority: OIDC authority URL (must use HTTPS)
  - LoginUrl: Custom login URL
  - ValidateIssuer: Whether to validate the token issuer
- **Validation**: Ensures ClientId or Authority is present; enforces HTTPS for Authority

#### TwitterExternalLoginProviderSettings.cs
- **Purpose**: Configuration settings for Twitter OAuth authentication
- **Key Properties**:
  - ConsumerKey: Twitter API consumer key
  - ConsumerSecret: Twitter API consumer secret
- **Validation**: IsValid() ensures both credentials are present

#### WsFederationExternalLoginProviderSettings.cs
- **Purpose**: Configuration for WS-Federation authentication providers
- **Key Properties**:
  - ClientId: WS-Fed client identifier
  - Tenant: Tenant identifier for multi-tenant scenarios
  - MetaDataAddress: Metadata endpoint URL
  - Wtrealm: WS-Fed realm identifier
  - Authority: Identity provider authority
- **Validation**: IsValid() ensures ClientId is present

### Key Components
- Common IsValid() pattern for configuration validation
- Support for OAuth 2.0, OpenID Connect, and WS-Federation protocols
- Claim mapping functionality for external identity integration

### Dependencies
- Abp.Extensions: For string validation utilities
- Abp.UI: For user-friendly exception handling
- System: For string comparison operations

## Architecture Notes
- Consistent validation pattern across all provider settings
- Security-focused with HTTPS enforcement for sensitive endpoints
- Flexible claim mapping for identity transformation
- Supports multiple authentication protocols and standards

## Business Logic
- Each provider requires specific credentials for authentication
- HTTPS is mandatory for OpenID Connect authority URLs
- Validation prevents misconfigured authentication providers
- Supports social login for improved user experience

## Usage Across Codebase
These settings are used by:
- Authentication middleware configuration
- External login provider setup
- OAuth/OIDC flow implementations
- User registration and login services
- Multi-tenant authentication scenarios