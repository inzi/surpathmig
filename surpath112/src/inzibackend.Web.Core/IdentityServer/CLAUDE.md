# IdentityServer Documentation

## Overview
This folder contains IdentityServer 4 configuration for OAuth 2.0 and OpenID Connect provider functionality. It defines API resources, scopes, identity resources, and client applications that can authenticate against the system.

## Contents

### Files

#### IdentityServerConfig.cs
- **Purpose**: Configuration factory for IdentityServer 4 resources and clients
- **Key Methods**:
  - **GetApiResources()**: Defines API resources protected by IdentityServer
  - **GetApiScopes()**: Defines API scopes for fine-grained permissions
  - **GetIdentityResources()**: Defines identity resources (OpenID Connect standard claims)
  - **GetClients()**: Loads client applications from configuration
- **Configuration Source**: Reads clients from appsettings.json under "IdentityServer:Clients"

#### IdentityServerRegistrar.cs
- **Purpose**: Registration helper for IdentityServer services (not shown but referenced)
- **Functionality**: Registers IdentityServer in DI container during startup

### Key Components
- **API Resources**: Backend APIs protected by tokens
- **API Scopes**: Permissions clients can request
- **Identity Resources**: User claims available via OpenID Connect
- **Clients**: Applications authorized to request tokens

### Dependencies
- **IdentityServer4**: OAuth/OpenID Connect server
- **IdentityServer4.Models**: IdentityServer configuration models
- **Microsoft.Extensions.Configuration**: Configuration access

## Architecture Notes

### OAuth 2.0 / OpenID Connect Provider
IdentityServer allows the application to act as an authorization server:
- Issue access tokens for API access
- Issue identity tokens for user authentication
- Support OAuth flows (authorization code, client credentials, etc.)
- Provide user consent screens
- Enable single sign-on (SSO)

### Resource/Scope Model
```
API Resource: "default-api"
  └── Scope: "default-api" (all functionality)
```

Current configuration uses a single broad scope. In production, consider:
```
API Resource: "compliance-api"
  ├── Scope: "compliance.read"
  ├── Scope: "compliance.write"
  └── Scope: "compliance.admin"
```

### Identity Resources
Standard OpenID Connect claims:
- **OpenId**: Required for OpenID Connect (sub claim)
- **Profile**: User profile information (name, etc.)
- **Email**: User email address
- **Phone**: User phone number

## Business Logic

### API Resource Configuration
```csharp
new ApiResource("default-api", "Default (all) API")
{
    Description = "AllFunctionalityYouHaveInTheApplication",
    ApiSecrets = {new Secret("secret")},
    Scopes = { "default-api" }
}
```
- **Name**: "default-api" - Resource identifier
- **Display Name**: "Default (all) API" - Shown to users
- **API Secret**: Validates API is authorized to accept tokens
- **Scopes**: List of scopes this resource supports

### Client Configuration (from appsettings.json)
```json
{
  "IdentityServer": {
    "Clients": [
      {
        "ClientId": "mobile-app",
        "ClientName": "Mobile Application",
        "AllowedGrantTypes": ["authorization_code", "refresh_token"],
        "RequireConsent": false,
        "AllowOfflineAccess": true,
        "ClientSecrets": [
          { "Value": "secret-hash-here" }
        ],
        "AllowedScopes": ["openid", "profile", "email", "default-api"],
        "RedirectUris": ["com.yourapp://callback"],
        "PostLogoutRedirectUris": ["com.yourapp://logout"]
      }
    ]
  }
}
```

### Client Properties
- **ClientId**: Unique identifier for the client application
- **ClientName**: Display name for consent screen
- **AllowedGrantTypes**: OAuth flows the client can use
  - `authorization_code`: Standard web app flow
  - `client_credentials`: Service-to-service auth
  - `password`: Direct username/password (not recommended)
  - `refresh_token`: Allow token refresh
- **RequireConsent**: Show user consent screen (usually false for first-party apps)
- **AllowOfflineAccess**: Allow refresh tokens for long-lived sessions
- **ClientSecrets**: Authenticate the client (hashed with Sha256)
- **AllowedScopes**: What scopes client can request
- **RedirectUris**: Valid callback URLs after authentication
- **PostLogoutRedirectUris**: Valid URLs after logout

## Usage Across Codebase

### Consumed By
- **Startup.cs**: IdentityServer registered during ConfigureServices
- **Token Authentication**: API validates tokens issued by IdentityServer
- **Mobile Apps**: Request tokens using configured client credentials
- **Third-party Integrations**: External systems can authenticate via OAuth

### Integration Points
```csharp
// Startup.ConfigureServices
services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
    .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
    .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
    .AddInMemoryClients(IdentityServerConfig.GetClients(configuration))
    .AddAbpIdentityServer();
```

### Token Validation
APIs validate tokens against IdentityServer:
```csharp
services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.Authority = "https://yourapp.com"; // IdentityServer URL
        options.Audience = "default-api";
    });
```

## Security Considerations

### Client Secrets
```csharp
ClientSecrets = { new Secret("secret".Sha256()) }
```
- ⚠️ Example uses weak secret "secret"
- **Production**: Use strong, randomly generated secrets
- **Storage**: Store secrets in configuration (encrypted)
- **Rotation**: Support secret rotation without downtime

### API Secrets
```csharp
ApiSecrets = {new Secret("secret")}
```
- Validates API resource can accept tokens
- Less critical than client secrets (internal validation)
- Still should be strong in production

### Redirect URI Validation
- **Strict Matching**: Exact match required for security
- **HTTPS Only**: Production should use HTTPS redirects
- **Wildcards**: Not supported (security risk)

### Consent Screen
- **RequireConsent**: false for first-party apps (seamless UX)
- **RequireConsent**: true for third-party apps (user explicitly grants access)

## OAuth 2.0 Grant Types

### Authorization Code (Recommended)
```json
"AllowedGrantTypes": ["authorization_code"]
```
- User redirected to login page
- User authenticates and consents
- Authorization code returned to client
- Client exchanges code for token (with client secret)
- Most secure for user-facing applications

### Client Credentials
```json
"AllowedGrantTypes": ["client_credentials"]
```
- Service-to-service authentication
- No user involved
- Client uses client_id and client_secret
- For backend services and scheduled jobs

### Resource Owner Password (Not Recommended)
```json
"AllowedGrantTypes": ["password"]
```
- Client directly handles username/password
- No redirect to login page
- Only for highly trusted clients
- Use authorization_code instead when possible

### Refresh Token
```json
"AllowedGrantTypes": ["refresh_token"],
"AllowOfflineAccess": true
```
- Allows token refresh without re-authentication
- Enables long-lived sessions
- Mobile apps can work offline then refresh token

## Performance Considerations
- In-memory stores (IdentityResources, Clients) cached automatically
- Token validation cached by JWT middleware
- Minimal overhead per request
- Production: Consider persistent stores for scale

## Extensibility

### Adding a New Client
1. Add client configuration to appsettings.json:
```json
{
  "ClientId": "my-spa",
  "ClientName": "My SPA Application",
  "AllowedGrantTypes": ["authorization_code"],
  "RequirePkce": true,
  "RequireClientSecret": false,
  "AllowedScopes": ["openid", "profile", "default-api"],
  "RedirectUris": ["https://myapp.com/callback"],
  "PostLogoutRedirectUris": ["https://myapp.com"]
}
```

2. Client can now authenticate against IdentityServer

### Adding New API Scopes
```csharp
public static IEnumerable<ApiScope> GetApiScopes()
{
    return new List<ApiScope>
    {
        new ApiScope("default-api", "Default (all) API"),
        new ApiScope("compliance.read", "Read compliance data"),
        new ApiScope("compliance.write", "Write compliance data"),
        new ApiScope("admin", "Administrative access")
    };
}
```

### Adding New Identity Resources
```csharp
new IdentityResource(
    name: "organization",
    displayName: "Organization",
    claimTypes: new[] { "org_id", "org_name" })
```

## Deployment Notes

### Development Environment
- DeveloperSigningCredential used (generates temporary key)
- Secrets can be simple values
- HTTPS not required (but recommended)

### Production Environment
- **Signing Certificate**: Use real certificate (not DeveloperSigningCredential)
- **Secrets Management**: Use Azure Key Vault or similar
- **HTTPS Required**: All redirect URIs must use HTTPS
- **Persistent Stores**: Consider database-backed stores for clients
- **Monitoring**: Log token issuance and validation failures

### Configuration Best Practices
- Store client secrets encrypted in configuration
- Use environment-specific appsettings files
- Never commit production secrets to source control
- Rotate secrets regularly
- Monitor for suspicious token requests