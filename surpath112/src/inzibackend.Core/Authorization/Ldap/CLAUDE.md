# LDAP Authentication Documentation

## Overview
LDAP (Lightweight Directory Access Protocol) and Active Directory integration for external authentication. Allows users to authenticate using their corporate directory credentials.

## Contents

### Files

#### AppLdapAuthenticationSource.cs
- **Purpose**: Custom LDAP authentication source implementation
- **Base Class**: `LdapAuthenticationSource<Tenant, User>` from ABP Zero
- **Key Features**:
  - Corporate directory authentication
  - Domain user validation
  - Tenant-specific LDAP configuration
  - Automatic user creation on first login
- **Dependencies**: ABP Zero LDAP module

### Key Components

- **AppLdapAuthenticationSource**: LDAP integration implementation
- **ILdapSettings**: Configuration interface (from ABP)
- **IAbpZeroLdapModuleConfig**: Module configuration (from ABP)

### Dependencies

- **External Libraries**:
  - Abp.Zero.Ldap (ABP Zero LDAP module)
  - System.DirectoryServices.Protocols (LDAP client)

- **Internal Dependencies**:
  - `Tenant` entity
  - `User` entity
  - Multi-tenancy system
  - User manager

## Architecture Notes

- **Pattern**: Authentication source pattern from ABP
- **Multi-Tenancy**: Tenant-specific LDAP settings
- **User Provisioning**: Auto-create users on first login
- **Integration**: Pluggable authentication source

## Business Logic

### LDAP Authentication Flow
1. User enters username/password
2. System checks for LDAP configuration
3. Connects to LDAP server with provided credentials
4. Validates against directory
5. On success, checks if user exists locally
6. Creates user if first login
7. Grants access with local permissions

### User Provisioning
- Automatic user creation on first LDAP login
- Maps LDAP attributes to user properties
- Assigns default role
- Syncs email and name from directory
- No password stored locally

### Configuration
- Per-tenant LDAP settings
- LDAP server URL and port
- Domain name
- Service account credentials (optional)
- SSL/TLS settings
- User search base DN

## Usage Across Codebase

Used by:
- Login process
- External authentication providers
- User authentication pipeline
- Account controller

## Security Considerations

### Authentication
- Validates against external directory
- No local password storage for LDAP users
- Secure LDAP connections (LDAPS/StartTLS)
- Service account with minimal permissions

### User Management
- LDAP users cannot change password locally
- Email must match directory
- Disable local password features for LDAP users
- Regular sync with directory

### Best Practices
- Use LDAPS (LDAP over SSL) for production
- Restrict service account permissions
- Implement connection pooling
- Cache authentication results briefly
- Log authentication attempts
- Handle directory unavailability gracefully

## Configuration Example

Typical LDAP settings (configured per tenant):
```
Server: ldap://dc.example.com:389
Domain: EXAMPLE
Username Format: EXAMPLE\{0} or {0}@example.com
BaseDn: DC=example,DC=com
```

## Limitations

- Relies on external directory availability
- Network dependency
- May have latency in authentication
- Limited to Windows Active Directory or LDAP-compliant directories
- No offline authentication

## Extension Points

- Custom attribute mapping
- Additional user properties from LDAP
- Group/role mapping from directory
- Multi-domain support
- Federated authentication
- SAML integration alongside LDAP