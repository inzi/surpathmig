# SMS Service Documentation

## Overview
SMS messaging service using Twilio for sending text messages. Supports two-factor authentication codes, notifications, and user communications.

## Contents

### Files

#### ISmsSender.cs
- **Purpose**: Interface for SMS sending operations
- **Method**: `Task SendAsync(string number, string message)` - Send SMS to phone number

#### SmsSender.cs
- **Purpose**: Default SMS sender implementation
- **Pattern**: Wrapper/adapter pattern

#### TwilioSmsSender.cs
- **Purpose**: Twilio-based SMS implementation
- **Key Features**:
  - Integrates with Twilio REST API
  - Supports international phone numbers
  - Async message sending
  - Automatic client initialization
- **Dependencies**: Twilio SDK

#### TwilioSmsSenderConfiguration.cs
- **Purpose**: Configuration for Twilio service
- **Properties**:
  - AccountSid: Twilio account identifier
  - AuthToken: Authentication token
  - SenderNumber: Phone number for outgoing messages

### Key Components

- **ISmsSender**: Service interface
- **TwilioSmsSender**: Twilio implementation (transient)
- **TwilioSmsSenderConfiguration**: Configuration provider

### Dependencies

- **External Libraries**:
  - Twilio SDK (REST API client)
  - ABP Framework (dependency injection)

- **Internal Dependencies**:
  - Configuration system
  - Identity system (for 2FA codes)

## Architecture Notes

- **Pattern**: Strategy pattern for multiple SMS providers
- **Lifecycle**: Transient dependency (creates new Twilio client per send)
- **Extensibility**: Easy to add alternative providers (AWS SNS, MessageBird)

## Business Logic

### SMS Sending Flow
1. Service receives phone number and message
2. Twilio client initialized with credentials
3. Message created via Twilio API
4. Delivery status available via Twilio callback (optional)
5. Async operation completes

### Phone Number Format
- Supports E.164 format (+1234567890)
- International numbers supported
- Twilio handles number validation

### Message Constraints
- Maximum length varies by region (typically 160 chars for SMS)
- Longer messages automatically split
- Unicode support for international characters

## Usage Across Codebase

Used by:
- Two-factor authentication (sending codes)
- Phone number verification
- Password reset via SMS
- User notifications (optional)
- Emergency alerts
- Appointment reminders

## Security Considerations

### Credentials
- AccountSid and AuthToken stored in configuration
- Use secure storage (Azure Key Vault recommended)
- Never log tokens or credentials

### Privacy
- Phone numbers are PII
- No SMS content logging
- Comply with SMS regulations (TCPA, GDPR)
- User opt-in required
- Provide opt-out mechanism

### Rate Limiting
- Implement sending limits
- Prevent SMS bombing
- Track send frequency per user
- Monitor costs

## Configuration

### App Settings
```json
{
  "Twilio": {
    "AccountSid": "ACxxxxxxxxxxxxxxxxxxxxxx",
    "AuthToken": "your_auth_token",
    "SenderNumber": "+15551234567"
  }
}
```

### Twilio Account Setup
1. Create Twilio account
2. Purchase phone number
3. Get Account SID and Auth Token
4. Configure webhook endpoints (optional)

## Cost Considerations

- Per-message pricing
- International rates vary
- Monitor usage via Twilio dashboard
- Set spending limits
- Consider message consolidation

## Extension Points

- Alternative SMS providers (failover)
- SMS queue for batch sending
- Delivery status tracking
- SMS templates
- Shortcode support
- MMS support (images)
- Two-way messaging