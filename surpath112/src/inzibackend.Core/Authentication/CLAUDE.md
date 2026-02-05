# Authentication Documentation

## Overview
Authentication infrastructure providing secure user authentication with support for multi-factor authentication, external providers, and security token management.

## Contents

### Subfolders

#### TwoFactor/
- Two-factor authentication system
- TOTP and SMS-based 2FA
- See [TwoFactor/CLAUDE.md](TwoFactor/CLAUDE.md)
  - **Google/**: Google Authenticator integration
    - See [TwoFactor/Google/CLAUDE.md](TwoFactor/Google/CLAUDE.md)

### Key Components

- **Two-Factor Authentication**: Additional security layer
- **Code Caching**: Temporary 2FA code storage
- **Google Authenticator**: TOTP provider

### Dependencies

- **External Libraries**:
  - ABP Framework (authentication modules)
  - ASP.NET Core Identity
  - Google Authenticator libraries

- **Internal Dependencies**:
  - User management
  - Identity system
  - Security system
  - SMS service

## Architecture Notes

- **Pattern**: Provider pattern for authentication methods
- **Security**: Multi-layered authentication
- **Extensibility**: Pluggable authentication providers
- **Standards**: TOTP (RFC 6238), HOTP standards

## Business Logic

### Authentication Flow

#### Standard Authentication
1. User provides username/password
2. Credentials validated
3. User authenticated
4. Session created

#### With Two-Factor
1. User provides username/password
2. Credentials validated
3. 2FA required check
4. User provides 2FA code
5. Code validated
6. Full authentication granted

### Supported Methods

#### Primary Authentication
- Username/password (local)
- LDAP/Active Directory
- External providers (OAuth, SAML)

#### Two-Factor Authentication
- Google Authenticator (TOTP)
- SMS codes
- Email codes
- Backup codes

## Usage Across Codebase

Used by:
- Login process
- Account security settings
- User profile management
- Admin user setup
- API authentication
- Mobile app authentication

## Security Considerations

### Password Security
- Hashed with strong algorithm (BCrypt/PBKDF2)
- Salt per user
- Never stored in plain text
- Complexity requirements enforced

### Two-Factor Security
- Time-based codes (30-second window)
- Rate limiting on attempts
- Backup codes for device loss
- Secure QR code generation
- Secret key protection

### Session Security
- Secure cookies (HttpOnly, Secure, SameSite)
- Session timeout
- Concurrent session limits
- Security stamp validation

## Configuration

### Authentication Options
```json
{
  "Authentication": {
    "TwoFactor": {
      "IsEnabled": true,
      "RequiredForAll": false,
      "CodeValidityDuration": 120
    },
    "SessionTimeout": 1440
  }
}
```

## Extension Points

- Additional 2FA providers
- Hardware token support
- Biometric authentication
- Passwordless authentication
- Risk-based authentication
- Adaptive authentication