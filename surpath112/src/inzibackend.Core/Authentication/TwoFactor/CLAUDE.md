# Two-Factor Authentication Documentation

## Overview
Two-factor authentication (2FA) infrastructure providing additional security layer for user authentication. Supports time-based one-time passwords (TOTP) via Google Authenticator and SMS-based codes.

## Contents

### Files

#### TwoFactorCodeCacheItem.cs
- **Purpose**: Cache model for storing temporary 2FA codes
- **Key Features**:
  - Serializable for distributed caching
  - 2-minute sliding expiration window
  - Simple code storage and retrieval
- **Cache Name**: "AppTwoFactorCodeCache"
- **Expiration**: 2 minutes (sliding)
- **Usage**: Temporary storage of SMS codes or backup codes

### Subfolders

#### Google/
- Google Authenticator integration
- TOTP generation and validation
- QR code generation for setup
- See [Google/CLAUDE.md](Google/CLAUDE.md)

### Key Components

- **TwoFactorCodeCacheItem**: Cache model for 2FA codes
- **Google Authenticator**: TOTP-based 2FA provider

### Dependencies

- **External Libraries**:
  - ABP Framework (caching)
  - System.ComponentModel.DataAnnotations

- **Internal Dependencies**:
  - ABP cache manager
  - User authentication system

## Architecture Notes

- **Pattern**: Cache-aside pattern for temporary codes
- **Serialization**: Supports distributed caching scenarios
- **Expiration**: Time-based automatic cleanup
- **Security**: Short-lived tokens

## Business Logic

### 2FA Flow
1. User initiates login with username/password
2. System generates or validates 2FA code
3. Code cached temporarily
4. User provides 2FA code
5. System validates against cached/generated code
6. Access granted on successful validation

### Code Lifecycle
- Generated on demand
- Cached with 2-minute expiration
- Sliding expiration extends on each access
- Auto-cleanup after expiration

### Google Authenticator Flow
1. User enables 2FA in profile
2. System generates secret key
3. QR code displayed for scanning
4. User scans with Google Authenticator app
5. App generates time-based codes
6. User enters code during login
7. System validates TOTP code

## Usage Across Codebase

Used by:
- Login process (Web.Core/Authentication)
- User profile settings
- Account security management
- SMS verification (for SMS-based 2FA)
- External authentication flows

## Security Considerations

### Code Storage
- Temporary caching only
- Short expiration window (2 minutes)
- No persistent storage of codes
- Sliding expiration prevents reuse

### Best Practices
- Codes single-use (for SMS codes)
- Time-based validation (for TOTP)
- Rate limiting on validation attempts
- Secure transmission (HTTPS)
- Encrypted cache storage (if distributed)

## Configuration

### Cache Settings
- Cache name: "AppTwoFactorCodeCache"
- Default expiration: 2 minutes
- Sliding expiration: Yes
- Distributed cache support: Yes (via ABP)

### 2FA Providers
- Google Authenticator (TOTP) - Primary
- SMS codes (via cache) - Backup
- Email codes (optional)

## Extension Points

- Additional 2FA providers
- Custom code generation logic
- Configurable expiration times
- Hardware token support
- Biometric authentication integration