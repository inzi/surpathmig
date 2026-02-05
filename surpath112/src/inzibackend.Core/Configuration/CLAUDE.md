# Configuration Documentation

## Overview
Application configuration system providing strongly-typed settings, Azure Key Vault integration, and hierarchical configuration management for host and tenant-specific settings.

## Contents

### Files

#### AppSettings.cs
- **Purpose**: Strongly-typed setting name constants
- **Key Sections**:
  - `HostManagement`: Billing information
  - `DashboardCustomization`: Dashboard configurations
  - `UiManagement`: Theme, layout, header, footer settings
  - `TenantManagement`: Registration, billing, subscription settings
  - `PaymentPopup`: Payment modal settings
  - `UserManagement`: User registration, email confirmation
  - `Security`: Password complexity, lockout settings
  - `Surpath`: Business domain settings
- **Pattern**: Static nested classes for organization

#### AppSettingProvider.cs
- **Purpose**: ABP setting provider defining all application settings
- **Features**:
  - Default values
  - Scopes (Application, Tenant, User)
  - Setting inheritance
  - Client visibility flags

#### IAppConfigurationAccessor.cs
- **Purpose**: Interface for accessing configuration root
- **Method**: `Configuration` property returns IConfigurationRoot

#### DefaultAppConfigurationAccessor.cs
- **Purpose**: Default implementation using AppConfigurations
- **Pattern**: Singleton dependency

#### AppConfigurations.cs
- **Purpose**: Static builder for configuration from appsettings.json
- **Features**:
  - Loads appsettings.json and environment-specific files
  - Azure Key Vault integration
  - Environment-based configuration

#### IAppConfigurationWriter.cs / DefaultAppConfigurationWriter.cs
- **Purpose**: Write configuration changes back to appsettings.json
- **Use**: Dynamic configuration updates (use with caution)

#### AzureKeyVaultConfiguration.cs
- **Purpose**: Configuration model for Azure Key Vault settings
- **Properties**: IsEnabled, KeyVaultName, ClientId, ClientSecret, TenantId

#### AppAzureKeyVaultConfigurer.cs
- **Purpose**: Configures Azure Key Vault integration
- **Features**: Loads secrets from Key Vault when enabled

#### HostingEnvironmentExtensions.cs
- **Purpose**: Extension methods for IHostingEnvironment
- **Methods**: Environment-specific helpers

### Key Components

- **AppSettings**: Setting name constants (200+ settings)
- **AppSettingProvider**: Setting definitions and defaults
- **AppConfigurations**: Configuration builder
- **Azure Key Vault Integration**: Secure secret storage

### Dependencies

- **External Libraries**:
  - Microsoft.Extensions.Configuration
  - Azure.Security.KeyVault.Secrets
  - ABP Framework (settings module)

- **Internal Dependencies**:
  - Used by all layers requiring configuration

## Architecture Notes

- **Pattern**: Options pattern, provider pattern
- **Hierarchy**: Application → Tenant → User settings
- **Extensibility**: Easy to add new settings
- **Security**: Sensitive settings in Key Vault

## Business Logic

### Setting Scopes

#### Application Level
- System-wide defaults
- Host configuration
- Feature flags

#### Tenant Level
- Tenant-specific overrides
- Branding settings
- Billing information
- Feature customization

#### User Level
- UI preferences
- Notification settings
- Personal defaults

### Configuration Loading Order
1. appsettings.json (base)
2. appsettings.{Environment}.json (environment-specific)
3. Azure Key Vault secrets (if enabled)
4. Environment variables
5. Command-line arguments

### Setting Inheritance
- User settings inherit from tenant
- Tenant settings inherit from application
- Lower levels can override higher levels

## Major Setting Categories

### UI Management
- Layout type (1-8 different layouts)
- Dark mode toggle
- Theme selection
- Header/footer configuration
- Sidebar behavior

### Security
- Password complexity rules
- Max failed login attempts
- User lockout duration
- Two-factor authentication
- SMS/email verification

### Tenant Management
- Self-registration enabled
- Default edition assignment
- Subscription expiry notifications
- Captcha requirements

### Surpath Business Settings
- Compliance rules
- Service configurations
- Payment settings
- Document storage

### Payment Popup
- Global payment modal
- Cohort-specific popups
- Department-level configuration

## Usage Across Codebase

Used by:
- All application layers
- Controllers (security settings)
- Services (business rules)
- UI (theme, layout)
- Background jobs (intervals)
- Email service (SMTP config)
- Payment gateways

## Security Considerations

### Sensitive Settings
- Database connection strings → Key Vault
- API keys → Key Vault
- SMTP password → Encrypted
- Payment gateway credentials → Key Vault

### Access Control
- Tenant admins can modify tenant settings
- Host admin controls application settings
- Users can modify own settings only

### Validation
- Setting value validation
- Type safety via strongly-typed access
- Range constraints where applicable

## Configuration Example

### appsettings.json
```json
{
  "App": {
    "TenantManagement": {
      "AllowSelfRegistration": true,
      "DefaultEdition": "Standard"
    },
    "UiManagement": {
      "Theme": "default",
      "DarkMode": false
    }
  },
  "AzureKeyVault": {
    "IsEnabled": true,
    "KeyVaultName": "mykeyvault",
    "TenantId": "...",
    "ClientId": "...",
    "ClientSecret": "..."
  }
}
```

## Extension Points

- Custom setting providers
- Additional configuration sources
- Setting change notifications
- Configuration validation
- Setting migrations
- Configuration UI
- Bulk setting updates