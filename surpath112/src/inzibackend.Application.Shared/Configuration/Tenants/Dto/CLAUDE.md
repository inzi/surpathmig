# Tenant Configuration DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for tenant-specific configuration settings. These settings allow each tenant to customize their instance of the application including billing preferences, email configuration, LDAP integration, user management policies, and other tenant-level options. Tenant settings override or supplement host defaults based on edition features.

## Contents

### Files

#### Main Configuration
- **TenantSettingsEditDto.cs** - Root tenant settings composite:
  - Contains all tenant-configurable sections
  - General, Email, UserManagement, Security, Billing, Other settings
  - Complete tenant configuration snapshot

#### Email Configuration
- **TenantEmailSettingsEditDto.cs** - Tenant email settings:
  - SMTP server configuration (or use host defaults)
  - Default sender information
  - Email signature
  - Per-tenant email customization

#### User Management
- **TenantUserManagementSettingsEditDto.cs** - User policies:
  - Email confirmation required
  - SMS verification required
  - Session timeout overrides
  - User registration settings

#### LDAP Integration
- **LdapSettingsEditDto.cs** - Active Directory/LDAP configuration:
  - LDAP server connection (host, port, credentials)
  - Base DN for user search
  - User filter queries
  - Attribute mappings (username, email, name)
  - Enable/disable LDAP authentication
  - Allows enterprise single sign-on

#### Billing Settings
- **TenantBillingSettingsEditDto.cs** - Tenant billing preferences:
  - Tax ID / VAT number
  - Billing address
  - Invoice preferences
  - Payment method selection

- **TenantPaymentPopupSettingsEditDto.cs** - Payment reminder settings:
  - Show/hide payment reminders
  - Reminder frequency
  - Grace period configuration

#### Other Settings
- **TenantOtherSettingsEditDto.cs** - Miscellaneous tenant settings:
  - Feature flags specific to tenant
  - Custom configuration options
  - Tenant-specific toggles

### Key Components

#### Settings Inheritance
- Tenants inherit host defaults
- Can override based on edition permissions
- Some settings tenant-only (LDAP, billing address)
- Other settings may be locked by host

#### LDAP Integration
Enables enterprise authentication:
- Connect to organizational Active Directory
- Import user accounts from LDAP
- Synchronize user attributes
- Single sign-on for corporate users

### Dependencies
- **System.ComponentModel.DataAnnotations** - Validation attributes
- **inzibackend.Core.Shared** - Constants and shared types

## Architecture Notes

### Tenant Isolation
- Each tenant's settings independent
- LDAP credentials tenant-specific
- Email configuration tenant-scoped
- Billing information isolated

### Edition-Based Access
- Premium editions may unlock additional settings
- Basic editions may have settings locked to host defaults
- Feature flags control setting visibility

### Security Considerations
- LDAP passwords encrypted at rest
- Email credentials stored securely
- Billing information PII-protected
- Tenant cannot access other tenants' settings

## Business Logic

### LDAP Authentication Workflow
1. Tenant configures LdapSettingsEditDto with AD server info
2. Enables LDAP authentication
3. User logs in with corporate credentials
4. System authenticates against LDAP server
5. User account created/updated from LDAP attributes
6. User logged in with LDAP identity

### Tenant Email Configuration
1. Tenant enters SMTP settings in TenantEmailSettingsEditDto
2. Sends test email to verify
3. All tenant emails use custom SMTP
4. Falls back to host SMTP if not configured

### Billing Customization
1. Tenant enters tax ID in TenantBillingSettingsEditDto
2. Invoices include tax information
3. Payment popup settings control reminder frequency
4. Tenant manages own payment methods

### Settings Override Priority
1. Tenant-specific settings (highest priority)
2. Host defaults
3. System defaults (lowest priority)

## Usage Across Codebase
These DTOs are consumed by:
- **ITenantSettingsAppService** - Tenant configuration management
- **Tenant Administration UI** - Settings screens for tenant admins
- **LDAP Authentication** - Enterprise login integration
- **Email Services** - Tenant-specific SMTP configuration
- **Billing Services** - Invoice generation with tenant details
- **User Management** - Tenant-specific user policies

## Cross-Reference Impact
Changes to these DTOs affect:
- Tenant admin configuration interfaces
- LDAP authentication workflow
- Email sending with tenant SMTP
- Invoice generation and billing
- User management policies per tenant
- Settings inheritance from host
- Edition feature unlocking