# Email Service Documentation

## Overview
Email infrastructure providing SMTP email sending with HTML templates, configuration management, and multi-tenant support. Uses MailKit for reliable email delivery.

## Contents

### Files

#### EmailTemplateProvider.cs
- **Purpose**: Loads and manages email templates with tenant-specific branding
- **Key Features**:
  - Caches templates per tenant
  - Embedded resource loading (default.html)
  - Dynamic logo URL injection
  - Year placeholder replacement
  - Singleton for performance
- **Template Variables**: {EMAIL_LOGO_URL}, {THIS_YEAR}

#### IEmailTemplateProvider.cs
- **Purpose**: Interface for template management
- **Method**: `GetDefaultTemplate(int? tenantId)` - Returns HTML template

#### inzibackendSmtpEmailSenderConfiguration.cs
- **Purpose**: Custom SMTP configuration with encrypted password support
- **Key Features**:
  - Extends ABP's SmtpEmailSenderConfiguration
  - Decrypts stored SMTP password using SimpleStringCipher
  - Reads from app settings

#### inzibackendMailKitSmtpBuilder.cs
- **Purpose**: Custom MailKit SMTP client builder
- **Features**: Advanced SMTP configuration options

#### EmailSettingsChecker.cs
- **Purpose**: Validates email configuration settings
- **Use**: Ensures SMTP settings are correct before sending

#### IEmailSettingChecker.cs
- **Purpose**: Interface for settings validation

### Subfolder

#### EmailTemplates/
- HTML email templates
- See [EmailTemplates/CLAUDE.md](EmailTemplates/CLAUDE.md)

### Key Components

- **EmailTemplateProvider**: Template management (singleton)
- **SMTP Configuration**: Encrypted credentials
- **Settings Checker**: Configuration validation

### Dependencies

- **External Libraries**:
  - MailKit (SMTP client)
  - ABP Framework (email, configuration)
  - System.Net.Mail

- **Internal Dependencies**:
  - IWebUrlService (URL generation)
  - ITenantCache (tenant information)
  - Configuration system

## Architecture Notes

- **Pattern**: Provider pattern for templates, configuration pattern for SMTP
- **Caching**: Template caching per tenant (singleton)
- **Security**: Encrypted password storage
- **Multi-Tenancy**: Tenant-specific logos and templates

## Business Logic

### Email Sending Flow
1. Load template for tenant
2. Replace template variables
3. Inject dynamic content (EMAIL_BODY)
4. Configure SMTP client with encrypted password
5. Send email via MailKit
6. Handle delivery status

### Template Management
- Templates loaded from embedded resources
- Cached per tenant for performance
- Logo URLs generated dynamically
- Current year injected automatically

### SMTP Configuration
- Host, port, credentials from settings
- Password encrypted in storage
- Decrypted at runtime
- SSL/TLS support

## Usage Across Codebase

Used by:
- User registration (welcome emails)
- Password reset
- Email verification
- Subscription notifications
- Payment confirmations
- System notifications
- Friendship invitations
- Admin notifications

## Security Considerations

- SMTP password encrypted at rest
- SSL/TLS enforced for email transmission
- Template injection protection
- HTML encoding of user content
- No sensitive data in templates

## Configuration

### App Settings
```json
{
  "Smtp": {
    "Host": "smtp.example.com",
    "Port": 587,
    "Username": "noreply@example.com",
    "Password": "[encrypted]",
    "EnableSsl": true,
    "UseDefaultCredentials": false
  }
}
```

## Extension Points

- Custom email templates per tenant
- Additional template providers
- Alternative email providers (SendGrid, AWS SES)
- Email queue for bulk sending
- Retry logic for failed emails
- Email analytics