# Host Configuration DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for host-level (system-wide) configuration settings. These settings apply across all tenants and control global system behavior including security, billing, user management, and session policies. Host settings provide the foundation for multi-tenant SaaS operations.

## Contents

### Files

#### Main Configuration
- **HostSettingsEditDto.cs** - Root host settings composite:
  - Contains all host-level configuration sections
  - General, Security, UserManagement, Email, TenantManagement, Billing settings
  - Complete configuration snapshot for host admin

#### General Settings
- **GeneralSettingsEditDto.cs** - Basic system configuration:
  - Timezone settings
  - Default language
  - System-wide feature flags

#### Security Settings
- **SecuritySettingsEditDto.cs** - System security configuration:
  - Password complexity requirements
  - Login attempt limits
  - Security token settings
  - CORS configuration

- **TwoFactorLoginSettingsEditDto.cs** - 2FA configuration:
  - Enable/disable two-factor authentication
  - Available 2FA providers (SMS, Email, Google Authenticator)
  - Force 2FA for specific roles

- **UserLockOutSettingsEditDto.cs** - Account lockout policy:
  - Lockout duration
  - Maximum failed login attempts
  - Lockout enabled/disabled per tenant type

- **UserPasswordSettingsEditDto.cs** - Password policy:
  - Minimum length
  - Required character types (uppercase, lowercase, digits, symbols)
  - Password expiration period
  - Password history count

- **SessionTimeOutSettingsEditDto.cs** - Session management:
  - Session timeout duration
  - Remember me duration
  - Idle timeout settings

#### User Management
- **HostUserManagementSettingsEditDto.cs** - User management policies:
  - Email confirmation required
  - SMS verification required
  - Cookie consent required
  - Session timeout settings

#### Tenant Management
- **TenantManagementSettingsEditDto.cs** - Tenant administration settings:
  - Allow self-registration
  - Default edition for new tenants
  - Tenant isolation settings
  - Subdomain configuration

#### Billing Settings
- **HostBillingSettingsEditDto.cs** - SaaS billing configuration:
  - Payment gateway selection (Stripe, PayPal, Authorize.Net)
  - Billing cycle defaults
  - Trial period settings
  - Invoice configuration

- **PaymentPopupSettingsEditDto.cs** - Payment UI configuration:
  - Enable/disable payment popup
  - Popup appearance settings
  - Payment reminder timing

#### Other Settings
- **OtherSettingsEditDto.cs** - Miscellaneous host settings:
  - Logging levels
  - Feature flags
  - Maintenance mode
  - System-wide toggles

#### Utility DTOs
- **SendTestEmailInput.cs** - Test email functionality:
  - Email address to send test message
  - Used to verify SMTP configuration

### Key Components

#### Settings Hierarchy
- Host settings provide defaults for all tenants
- Tenants can override specific settings based on edition features
- Some settings are host-only (billing, tenant management)

#### Security Configuration
Comprehensive security controls:
- Password complexity enforcement
- Account lockout protection against brute force
- Two-factor authentication options
- Session timeout for security

### Dependencies
- **System.ComponentModel.DataAnnotations** - Validation attributes
- **inzibackend.Core.Shared** - Constants and enums

## Architecture Notes

### Composite Pattern
- HostSettingsEditDto aggregates all setting categories
- Allows atomic save of all host settings
- Simplifies settings backup/restore

### Validation Strategy
- Password settings validated for consistency
- Email configuration testable before save
- Billing settings require valid payment credentials

### Cache Invalidation
- Settings changes invalidate relevant caches
- Security settings affect authorization checks immediately
- Billing settings impact subscription calculations

## Business Logic

### Security Policy Enforcement
1. Host configures password complexity in UserPasswordSettingsEditDto
2. System enforces requirements for all tenants during registration/password change
3. Failed login attempts tracked per UserLockOutSettingsEditDto
4. Accounts locked after threshold reached

### Multi-Tenant Billing
1. Host configures payment gateways in HostBillingSettingsEditDto
2. Tenants subscribe to editions with payment
3. Payment popup shown based on PaymentPopupSettingsEditDto
4. System processes payments through configured gateway

### Session Management
1. SessionTimeOutSettingsEditDto defines timeout durations
2. User inactive beyond timeout = session expires
3. Remember me extends session per configuration
4. Security vs usability balance

### Tenant Registration
1. TenantManagementSettingsEditDto controls self-registration
2. New tenants get default edition
3. Email confirmation required based on settings
4. Subdomain assigned per configuration

## Usage Across Codebase
These DTOs are consumed by:
- **IHostSettingsAppService** - Host configuration management
- **Host Administration UI** - Settings management interface
- **Authentication Services** - Security policy enforcement
- **Session Management** - Timeout and lockout logic
- **Billing Services** - Payment processing configuration
- **Tenant Registration** - New tenant creation workflow
- **Email Services** - SMTP configuration

## Cross-Reference Impact
Changes to these DTOs affect:
- Host administration interfaces
- Security policy enforcement across all tenants
- Tenant registration process
- Billing and payment workflows
- Session timeout behavior
- Password validation rules
- Two-factor authentication setup
- Account lockout logic