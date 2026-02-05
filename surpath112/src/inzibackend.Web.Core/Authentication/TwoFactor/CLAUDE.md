# Two-Factor Authentication Documentation

## Overview
This folder contains the two-factor authentication (2FA) infrastructure, specifically the caching mechanism for temporary 2FA codes. It provides secure temporary storage of authentication codes during the two-factor authentication process.

## Contents

### Files

#### TwoFactorCodeCacheExtensions.cs
- **Purpose**: Extension methods for managing two-factor authentication codes in cache
- **Key Features**:
  - Cache-based temporary code storage
  - Tenant-isolated code management
  - Automatic expiration of codes
  - Thread-safe operations

### Key Components
- **TwoFactorCodeCacheItem**: Cache item model for 2FA codes (defined in Application layer)
- **Cache Extensions**: Helper methods for cache operations
- **Expiration Management**: Automatic code expiration after configured time

### Dependencies
- Abp.Runtime.Caching (for distributed caching)
- System.Threading (for async operations)

## Architecture Notes
- Uses distributed cache for scalability across multiple servers
- Implements sliding expiration for security
- Cache keys include tenant ID for multi-tenant isolation
- Extension methods provide clean API for cache operations

## Business Logic
- 2FA codes are temporarily stored during authentication flow
- Codes expire after a configurable time period (default: 3 minutes)
- Each code is unique per user and tenant
- Codes are removed from cache after successful use

## Security Considerations
- Codes stored in memory/cache only, never persisted to database
- Automatic expiration prevents code reuse attacks
- Tenant isolation prevents cross-tenant access
- Single-use design prevents replay attacks

## Usage Across Codebase
This two-factor caching system is used by:
- TokenAuthController for 2FA login flows
- SMS and email 2FA code verification
- Google Authenticator integration
- Account security management endpoints