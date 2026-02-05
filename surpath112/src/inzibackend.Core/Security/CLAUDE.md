# Security Documentation

## Overview
Security infrastructure including password complexity management and security policy enforcement.

## Contents

### Files

#### PasswordComplexitySettingStore.cs
- **Purpose**: Stores and retrieves password complexity settings
- **Key Features**:
  - Minimum length requirements
  - Required character types (uppercase, lowercase, digits, special chars)
  - Configurable per tenant
  - Default complexity rules

#### IPasswordComplexitySettingStore.cs
- **Purpose**: Interface for password complexity settings

### Key Components

- **PasswordComplexitySettingStore**: Settings storage and retrieval
- **Password Complexity Rules**: Configurable security policies

### Dependencies

- **External Libraries**:
  - ABP Framework (settings system)
  - ASP.NET Core Identity

- **Internal Dependencies**:
  - Configuration system
  - Tenant management
  - User management

## Architecture Notes

- **Pattern**: Store pattern for settings management
- **Configuration**: Hierarchical (application → tenant)
- **Validation**: Enforced during password creation/change
- **Customization**: Tenant-specific rules supported

## Business Logic

### Password Complexity Rules

#### Default Requirements
- Minimum length: 8 characters
- Require uppercase letters: Yes
- Require lowercase letters: Yes
- Require digits: Yes
- Require non-alphanumeric: Yes (special characters)

#### Configurable Options
- Minimum length (6-128 characters)
- Character type requirements (on/off)
- Custom regex patterns
- Password history (prevent reuse)
- Password expiration period

### Validation Flow
1. User sets/changes password
2. Retrieve complexity settings for tenant
3. Validate against each rule
4. Return validation errors if fails
5. Accept password if passes all rules

### Settings Scope
- **Application Level**: Default for all tenants
- **Tenant Level**: Override application defaults
- **No User Level**: Same rules for all users in tenant

## Usage Across Codebase

Used by:
- User registration
- Password change
- Password reset
- Admin user creation
- Validation messages
- Client-side validation

## Security Best Practices

### Strong Passwords
- Longer is better (12+ recommended)
- Variety of character types
- No common patterns
- No personal information
- Password managers encouraged

### Validation Timing
- Server-side validation (required)
- Client-side validation (UX)
- Both must match
- Never trust client validation alone

### Error Messages
- Generic errors (don't reveal which rule failed for security)
- Detailed errors during input (UX)
- Log failed attempts
- Rate limiting on validation

## Extension Points

- Custom password validators
- Integration with password breach databases (Have I Been Pwned)
- Two-factor requirement for weak passwords
- Progressive complexity (more sensitive roles = stricter rules)
- Password strength meter
- Password generation helpers