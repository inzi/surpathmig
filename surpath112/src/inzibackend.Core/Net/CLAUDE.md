# Net (Networking) Documentation

## Overview
Network communication services including email (SMTP) and SMS messaging. Provides abstraction over communication providers with configuration management.

## Contents

### Subfolders

#### Emailing/
- SMTP email sending
- Email templates
- Configuration management
- See [Emailing/CLAUDE.md](Emailing/CLAUDE.md)

#### Sms/
- Twilio SMS integration
- SMS configuration
- Message sending
- See [Sms/CLAUDE.md](Sms/CLAUDE.md)

### Key Components

- **Email Service**: SMTP-based email delivery
- **SMS Service**: Twilio-based SMS delivery
- **Configuration**: Provider settings management
- **Templates**: HTML email templates

### Dependencies

- **External Libraries**:
  - MailKit (email)
  - Twilio SDK (SMS)
  - ABP Framework

- **Internal Dependencies**:
  - Configuration system
  - Localization
  - Tenant management

## Architecture Notes

- **Pattern**: Provider pattern for communication services
- **Abstraction**: Interface-based for testability
- **Configuration**: External configuration for credentials
- **Multi-Tenancy**: Tenant-specific settings support

## Usage Across Codebase

Used for:
- User notifications
- Account verification
- Password reset
- Two-factor authentication
- System alerts
- Subscription communications
- Welcome messages

## Security Considerations

- Credentials stored securely
- Encrypted passwords
- Rate limiting
- Content validation
- PII handling in messages

## Extension Points

- Additional email providers (SendGrid, AWS SES)
- Additional SMS providers (AWS SNS, MessageBird)
- Message queuing
- Delivery tracking
- Retry logic
- Template management UI