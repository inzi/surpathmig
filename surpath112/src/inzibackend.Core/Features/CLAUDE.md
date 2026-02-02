# Features Documentation

## Overview
Feature flag system enabling/disabling functionality based on edition, tenant configuration, or runtime conditions. Core to SaaS multi-tenancy model.

## Contents

### Files

#### AppFeatures.cs
- **Purpose**: Static feature name constants
- **Key Features**:
  - Chat
  - Tenant management
  - Edition management
  - Background jobs
  - Email sending
  - Custom features per edition

#### AppFeatureProvider.cs
- **Purpose**: ABP feature provider defining all features
- **Features**:
  - Feature definitions
  - Default values
  - Edition associations
  - Input types (checkbox, text, number)
  - Scopes (Edition, Tenant)

#### FeatureMetadata.cs
- **Purpose**: Additional metadata for features
- **Properties**: Display info, descriptions, icons

#### FeatureExtensions.cs
- **Purpose**: Extension methods for feature checking
- **Methods**: Simplified feature access helpers

### Key Components

- **AppFeatures**: Feature name constants (30+ features)
- **AppFeatureProvider**: Feature definitions
- **FeatureMetadata**: Feature metadata
- **FeatureExtensions**: Helper methods

### Dependencies

- **External Libraries**:
  - ABP Framework (features module)
  - Abp.Application.Features

- **Internal Dependencies**:
  - Edition system
  - Tenant management
  - Permission system

## Architecture Notes

- **Pattern**: Provider pattern from ABP
- **Hierarchy**: Edition → Tenant → Runtime checking
- **Storage**: Feature values in database
- **Caching**: Feature checks cached for performance

## Business Logic

### Feature Scopes

#### Edition Features
- Defined at edition level
- All tenants in edition inherit
- Cannot be changed by tenant
- Examples: Chat, Max user count

#### Tenant Features
- Tenant-specific overrides
- Can differ from edition defaults
- Admin configurable
- Examples: Custom integrations

### Feature Types

#### Boolean Features
- Enabled/disabled (true/false)
- Most common type
- Example: Chat enabled

#### Value Features
- Numeric limits
- Text values
- Example: Max user count = 100

### Feature Checking
```csharp
if (await FeatureChecker.IsEnabledAsync("Chat"))
{
    // Chat functionality
}

var maxUsers = await FeatureChecker.GetValueAsync<int>("MaxUserCount");
```

## Common Features

### Core Features
- **Chat**: Real-time messaging
- **MultiTenancy**: Tenant isolation
- **Mobile**: Mobile app support

### Surpath Features
- Drug testing enabled
- Background checks enabled
- Compliance tracking
- Document management

### Administrative
- Max user count
- Max OU count
- Storage quota
- API rate limits

## Usage Across Codebase

Used by:
- Controllers (feature authorization)
- Services (conditional logic)
- UI (show/hide elements)
- Navigation (menu filtering)
- API endpoints
- Background jobs

## Security Considerations

- Feature checks for authorization
- Cannot bypass via API
- Tenant isolation enforced
- Edition limits respected

## Extension Points

- Custom features per domain
- Dynamic feature values
- Feature dependencies
- Feature bundles
- A/B testing via features