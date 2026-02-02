# Identity Documentation

## Overview
ASP.NET Core Identity integration and customization for authentication, security stamps, and sign-in management.

## Contents

### Files

#### SignInManager.cs
- **Purpose**: Custom sign-in manager extending ASP.NET Core Identity
- **Key Features**:
  - Custom sign-in logic
  - Two-factor authentication integration
  - External login support
  - Tenant-aware sign-in
  - Lockout handling

#### SecurityStampValidator.cs / SecurityStampValidatorCallback.cs
- **Purpose**: Security stamp validation for session invalidation
- **Key Features**:
  - Validates security stamp on each request
  - Invalidates sessions on password change
  - Handles concurrent login scenarios
  - Automatic sign-out on security changes

#### IdentityRegistrar.cs
- **Purpose**: Registers ASP.NET Core Identity services
- **Key Features**:
  - Service configuration
  - Password policy setup
  - Lockout configuration
  - Token provider registration

#### IdentityExtensions.cs
- **Purpose**: Extension methods for Identity operations
- **Methods**: Helper methods for common identity tasks

### Key Components

- **SignInManager**: Authentication and sign-in operations
- **SecurityStampValidator**: Session security validation
- **IdentityRegistrar**: Service registration
- **IdentityExtensions**: Helper methods

### Dependencies

- **External Libraries**:
  - ASP.NET Core Identity
  - Microsoft.AspNetCore.Identity

- **Internal Dependencies**:
  - User management
  - Tenant management
  - Two-factor authentication
  - External authentication

## Architecture Notes

- **Pattern**: Extension of ASP.NET Core Identity
- **Customization**: Tenant-aware identity
- **Security**: Security stamp validation
- **Integration**: Seamless ABP integration

## Business Logic

### Sign-In Flow
1. User provides credentials
2. Validate username/password
3. Check lockout status
4. Validate security stamp
5. Check two-factor requirement
6. Issue authentication cookie
7. Redirect to return URL

### Security Stamp
- Generated on user creation
- Changed on password update
- Changed on security-sensitive actions
- Validates on each request
- Invalidates all sessions when changed

### Lockout
- Failed login attempts tracked
- Lockout after threshold (default: 5)
- Lockout duration configurable
- Admin can unlock manually

### Two-Factor Integration
- Check if 2FA enabled for user
- Require 2FA code after password
- Store temporary authentication
- Validate 2FA code
- Complete sign-in

## Usage Across Codebase

Used by:
- Login controllers
- Authentication middleware
- Account management
- Password reset
- External authentication
- API authentication

## Security Considerations

### Security Stamp
- Critical for session security
- Change on all security updates
- Prevents session fixation
- Forces re-authentication

### Password Policy
- Minimum length
- Required character types
- Complexity requirements
- Password history (optional)

### Lockout Protection
- Brute force prevention
- Configurable thresholds
- Auto-unlock after duration
- Admin override capability

## Configuration

### Identity Options
```csharp
options.Password.RequireDigit = true;
options.Password.RequiredLength = 8;
options.Password.RequireNonAlphanumeric = true;
options.Lockout.MaxFailedAccessAttempts = 5;
options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
```

## Extension Points

- Custom password validators
- Additional security checks
- Custom sign-in logic
- External authentication providers
- Biometric authentication
- Hardware tokens