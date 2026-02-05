# Editions Documentation

## Overview
SaaS edition (subscription tier) management system defining feature sets, pricing, and subscription capabilities for multi-tenant plans.

## Contents

### Files

#### SubscribableEdition.cs
- **Purpose**: Extended edition entity with subscription pricing
- **Key Properties**:
  - Pricing by period (Monthly, Annual, etc.)
  - Trial period settings
  - Expiration behavior
  - Wait days after expiry
  - Recurring payment support
- **Pattern**: Rich domain model

#### EditionManager.cs
- **Purpose**: Business logic for edition operations
- **Key Features**:
  - Edition CRUD
  - Feature assignment
  - Pricing management
  - Edition validation
  - Upgrade/downgrade logic

#### FeatureValueStore.cs
- **Purpose**: Storage and retrieval of feature values per edition
- **Pattern**: Value store pattern from ABP

### Key Components

- **SubscribableEdition**: Subscription-enabled edition entity
- **EditionManager**: Edition business logic
- **FeatureValueStore**: Feature value persistence

### Dependencies

- **External Libraries**:
  - ABP Framework (editions module)
  - Abp.Application.Editions

- **Internal Dependencies**:
  - Feature system
  - Tenant management
  - Payment system

## Architecture Notes

- **Pattern**: Edition-Feature relationship (many-to-many)
- **Pricing**: Multiple payment periods supported
- **Features**: Toggle features per edition
- **Hierarchy**: Editions define capability tiers

## Business Logic

### Edition Types

#### Free Edition
- No pricing
- Limited features
- Trial or community tier

#### Paid Editions
- Monthly/Annual pricing
- Full feature sets
- Commercial tiers

### Subscription Lifecycle
1. Tenant selects edition
2. Payment processed (if paid)
3. Edition features activated
4. Trial period starts (if applicable)
5. Subscription expires
6. Grace period (if configured)
7. Downgrade or deactivate

### Feature Management
- Features enabled/disabled per edition
- Feature values customizable
- Inherited from edition definition
- Tenant cannot override (by default)

### Pricing Periods
- Daily
- Weekly
- Monthly (most common)
- Annual (discounted)

## Usage Across Codebase

Used by:
- Tenant registration
- Subscription purchase
- Feature checking
- Payment processing
- Edition upgrade/downgrade
- Trial management

## Extension Points

- Custom pricing models
- Additional payment periods
- Feature bundles
- Add-on features
- Enterprise custom editions