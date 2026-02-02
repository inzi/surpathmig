# Password Management Documentation

## Overview
Password expiration and management services for enforcing password security policies across the multi-tenant application. This module handles automatic password expiration and forces users to change passwords based on configurable policies.

## Contents

### Files

#### IPasswordExpirationService.cs
- **Purpose**: Interface defining the contract for password expiration services
- **Key Methods**:
  - ForcePasswordExpiredUsersToChangeTheirPassword(): Main method for enforcing password expiration

#### PasswordExpirationService.cs
- **Purpose**: Implementation of password expiration logic for both host and tenant users
- **Key Functionality**:
  - Checks application-wide password expiration settings
  - Processes both host users and all tenant users
  - Batch updates users whose passwords have expired
  - Forces password change on next login for expired passwords
- **Technical Details**:
  - Processes users in batches of 1000 for performance
  - Uses UTC time for consistent expiration checking
  - Respects multi-tenant boundaries

### Key Components

- **IPasswordExpirationService**: Service interface for password expiration
- **PasswordExpirationService**: Core implementation with multi-tenant support

### Dependencies

- **External Libraries**:
  - ABP Framework (Domain Services, Settings, Timing)
  - Entity Framework Core (via repositories)

- **Internal Dependencies**:
  - inzibackend.Configuration (AppSettings)
  - inzibackend.MultiTenancy (Tenant)
  - inzibackend.Authorization.Users (User, RecentPassword, IUserRepository)
  - inzibackendDomainServiceBase

## Architecture Notes

- **Pattern**: Domain Service pattern
- **Multi-tenancy**: Full support with explicit tenant context switching
- **Performance**: Batch processing to handle large user sets efficiently
- **Configuration**: Settings-driven behavior (can be enabled/disabled)
- **Time Handling**: Uses ABP Clock abstraction for testability

## Business Logic

### Password Expiration Flow
1. Check if password expiration is enabled globally
2. Calculate expiration date based on configured day count
3. Process host users first
4. Iterate through all tenants and process their users
5. Update expired users in batches to force password change

### Batch Processing
- Users are processed in groups of 1000
- Prevents memory issues with large user bases
- Maintains transaction boundaries for reliability

### Settings Used
- `AppSettings.UserManagement.Password.EnablePasswordExpiration`: Master switch
- `AppSettings.UserManagement.Password.PasswordExpirationDayCount`: Days until expiration

## Usage Across Codebase

This service is typically used by:
- Background jobs or scheduled tasks for regular password expiration checks
- Administrative tools for enforcing security policies
- Login processes that check for required password changes
- User management interfaces showing password status

## Security Considerations

- Passwords are never exposed or logged
- Expiration is enforced at login time
- Multi-tenant isolation is maintained
- Batch processing prevents timing attacks