# Google Authentication Documentation

## Overview
Implementation of Google Authenticator-based two-factor authentication (2FA) for the inzibackend application. This module provides TOTP (Time-based One-Time Password) functionality using the Google Authenticator standard.

## Contents

### Files

#### GoogleAuthenticatorProvider.cs
- **Purpose**: Implements IUserTwoFactorTokenProvider interface for ASP.NET Core Identity integration
- **Key Functionality**:
  - Generates QR codes for Google Authenticator setup
  - Validates TOTP tokens entered by users
  - Checks if Google Authenticator is enabled for a user
  - Integrates with UserManager for 2FA workflow

#### GoogleTwoFactorAuthenticateService.cs
- **Purpose**: Core service implementing Google Authenticator TOTP algorithm
- **Key Functionality**:
  - Generates setup codes and QR code URLs using Google Charts API
  - Implements Base32 encoding for secret keys
  - Validates TOTP pins with configurable time drift tolerance (default: 5 minutes)
  - Generates time-based codes using HMACSHA1 algorithm
  - Based on RFC 6238 TOTP standard
- **Technical Details**:
  - 30-second time step for token generation
  - 6-digit numeric codes by default
  - Supports clock drift tolerance for user convenience

#### GoogleAuthenticatorSetupCode.cs
- **Purpose**: Data transfer object for Google Authenticator setup information
- **Properties**:
  - Account: User account identifier
  - AccountSecretKey: The shared secret key
  - ManualEntryKey: Base32 encoded key for manual entry
  - QrCodeSetupImageUrl: URL to Google Charts API for QR code generation

### Key Components

- **GoogleAuthenticatorProvider**: Main provider class for Identity integration
- **GoogleTwoFactorAuthenticateService**: Core TOTP implementation service
- **GoogleAuthenticatorSetupCode**: DTO for setup information

### Dependencies

- **External Libraries**:
  - System.Security.Cryptography (HMACSHA1)
  - Microsoft.AspNetCore.Identity
  - Abp.Dependency (for dependency injection)
  - Abp.UI (for user-friendly exceptions)

- **Internal Dependencies**:
  - inzibackend.Authorization.Users.User
  - inzibackendServiceBase

## Architecture Notes

- **Pattern**: Provider pattern for Identity integration
- **Security**:
  - Uses HMACSHA1 for secure token generation
  - Implements time-based tokens that expire every 30 seconds
  - Supports clock drift tolerance to handle time synchronization issues
- **Integration**: Seamlessly integrates with ASP.NET Core Identity's two-factor authentication system
- **External Service**: Uses Google Charts API for QR code generation (configurable HTTP/HTTPS)

## Business Logic

- Users must have GoogleAuthenticatorKey set to use Google Authenticator
- The system generates a QR code containing the TOTP URI for easy setup
- Token validation allows for time drift to accommodate clock differences
- Manual entry key provided for users who cannot scan QR codes

## Usage Across Codebase

This component is used by:
- User authentication flows requiring two-factor authentication
- User profile management for enabling/disabling Google Authenticator
- Login processes where 2FA is enabled