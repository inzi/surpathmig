# Cache Documentation

## Overview
This folder contains caching infrastructure for SMS verification codes used in user profile operations. It provides a typed cache implementation for storing and retrieving SMS verification codes temporarily.

## Contents

### Files

#### SmsVerificationCodeCacheItem.cs
- **Purpose**: Defines the cache item structure for storing SMS verification codes
- **Key Features**:
  - Serializable class for distributed caching support
  - Contains a single `Code` property to store the verification code
  - Defines cache name constant `AppSmsVerificationCodeCache`
- **Usage**: Used as the value type in the SMS verification code cache

#### SmsVerificationCodeCacheExtensions.cs
- **Purpose**: Provides extension methods for accessing the SMS verification code cache
- **Key Features**:
  - Extension method `GetSmsVerificationCodeCache` for `ICacheManager`
  - Returns a typed cache instance for SMS verification codes
  - Simplifies cache access throughout the application

### Key Components

**Classes:**
- `SmsVerificationCodeCacheItem`: Serializable cache item for SMS codes
- `SmsVerificationCodeCacheExtensions`: Static class with cache access extensions

**Constants:**
- `CacheName = "AppSmsVerificationCodeCache"`: Unique identifier for the SMS verification cache

### Dependencies
- **External:**
  - `Abp.Runtime.Caching`: ABP framework caching infrastructure
  - `System`: Core .NET framework

- **Internal:**
  - No direct internal dependencies

## Architecture Notes
- **Pattern**: Extension method pattern for cache access
- **Caching Strategy**: Typed caching using ABP's `ITypedCache<TKey, TValue>`
- **Serialization**: Cache items are marked as `[Serializable]` for distributed cache support
- **Key Type**: Uses `string` as the cache key type (likely phone number or user identifier)

## Business Logic
- SMS verification codes are temporarily stored in cache
- Codes are associated with string keys (likely phone numbers or user IDs)
- Cache provides temporary storage for verification codes during the SMS verification process
- Supports two-factor authentication and phone number verification workflows

## Usage Across Codebase
This cache infrastructure is likely used in:
- User profile update operations requiring SMS verification
- Two-factor authentication setup
- Phone number change/verification processes
- Password reset via SMS workflows

## Security Considerations
- Verification codes are stored temporarily in memory/cache
- Cache should have appropriate expiration times
- Codes should be removed after successful verification or expiration