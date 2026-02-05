# Configuration DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for application configuration management. These DTOs handle system-wide settings including email configuration, theme customization, external authentication providers, and other tenant-specific or host-level settings. The configuration system enables runtime customization without code changes.

## Contents

### Files

#### Email Configuration
- **EmailSettingsEditDto.cs** - Email server settings:
  - SMTP server configuration (host, port, credentials)
  - Default sender information
  - SSL/TLS settings
  - Test email functionality
  - Per-tenant email configuration support

#### Theme Customization
- **ThemeSettingsDto.cs** - Complete UI theme configuration:
  - Theme - Selected theme name (from 14 available themes in AppConsts)
  - Layout - ThemeLayoutSettingsDto for page structure
  - Header - ThemeHeaderSettingsDto for header appearance
  - SubHeader - ThemeSubHeaderSettingsDto for breadcrumb area
  - Menu - ThemeMenuSettingsDto for navigation menu
  - Footer - ThemeFooterSettingsDto for footer appearance
  - All sub-settings initialized with defaults

- **ThemeLayoutSettingsDto.cs** - Page layout configuration:
  - Container type (fixed/fluid)
  - Layout positioning
  - Content width

- **ThemeHeaderSettingsDto.cs** - Header customization:
  - Fixed/scrollable header
  - Header skin/color scheme
  - Mobile header behavior

- **ThemeSubHeaderSettingsDto.cs** - Breadcrumb area configuration:
  - Show/hide sub-header
  - Sub-header style
  - Breadcrumb display options

- **ThemeMenuSettingsDto.cs** - Navigation menu settings:
  - Fixed/scrollable menu
  - Menu position (left/top)
  - Menu skin/color
  - Collapsed/expanded default state

- **ThemeFooterSettingsDto.cs** - Footer customization:
  - Fixed/static footer
  - Footer content and styling

#### External Authentication
- **ExternalLoginProviderSettingsEditDto.cs** - Single provider configuration:
  - Provider name (Google, Facebook, Microsoft, etc.)
  - Client ID and Client Secret
  - Additional provider-specific settings
  - Enabled/disabled flag

- **ExternalLoginSettingsDto.cs** - All external login providers:
  - Collection of ExternalLoginProviderSettingsEditDto
  - Used for bulk provider configuration management

### Key Components

#### Multi-Tenant Configuration
- Tenants can customize themes independently
- Each tenant configures own email settings
- External login providers configurable per tenant or host-level
- Settings inheritance from host to tenant

#### Theme System
The theme system provides:
- 14 pre-built themes (Material, Default, Light, Dark, etc.)
- Per-component customization (Header, Menu, Footer)
- Responsive behavior configuration
- Brand color customization

### Dependencies
- None for theme DTOs (pure configuration)
- Email settings may reference SMTP libraries
- External login DTOs used by OAuth middleware

## Architecture Notes

### Nested Configuration Pattern
- **Composite Design**: ThemeSettingsDto composes multiple sub-setting DTOs
- **Default Initialization**: Sub-DTOs initialized with sensible defaults
- **Hierarchical**: Settings organized by UI component

### Security Considerations
- **Client Secrets**: ExternalLoginProviderSettingsEditDto stores OAuth secrets
- **Email Credentials**: SMTP password stored securely
- **Tenant Isolation**: Settings scoped to tenant context

### Runtime Configuration
- Settings changeable without application restart
- Cache invalidation on settings change
- UI reflects changes immediately

### Validation Strategy
- Email settings validated before save
- Test email functionality to verify SMTP configuration
- OAuth provider settings verified during authentication flow

## Business Logic

### Theme Customization Workflow
1. Admin navigates to theme settings
2. Retrieves current ThemeSettingsDto
3. Modifies theme name or component settings
4. Saves changes
5. System invalidates UI cache
6. Users see new theme on next page load

### Email Configuration Workflow
1. Admin configures SMTP settings in EmailSettingsEditDto
2. Sends test email to verify configuration
3. System uses settings for all outbound emails
4. Tenant-specific settings override host defaults

### External Login Setup
1. Admin registers app with OAuth provider (Google, Facebook, etc.)
2. Receives Client ID and Client Secret
3. Enters credentials in ExternalLoginProviderSettingsEditDto
4. Enables provider
5. Users see provider button on login screen
6. OAuth flow uses configured credentials

### Configuration Hierarchy
- **Host Settings**: Default for all tenants
- **Tenant Override**: Tenant-specific customization
- **User Preferences**: User-level settings (stored elsewhere)

## Usage Across Codebase
These DTOs are consumed by:
- **IConfigurationAppService** - Configuration CRUD operations
- **Host/Tenant Settings UI** - Admin configuration interfaces
- **Email Services** - SMTP client configuration
- **Theme Middleware** - UI theme application
- **OAuth Middleware** - External authentication providers
- **View Rendering** - Theme-based view selection

## Cross-Reference Impact
Changes to these DTOs affect:
- Configuration management interfaces
- Theme customization UI
- Email sending functionality
- External login provider setup
- Settings storage and retrieval
- Multi-tenant configuration isolation
- UI rendering and styling
- OAuth authentication flows