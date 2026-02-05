# Editions DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for SaaS edition (pricing tier) management. Editions define feature sets, pricing plans, and capabilities available to tenants. The system supports multiple pricing tiers (Free, Basic, Premium, Enterprise, etc.) with different feature restrictions and subscription models.

## Contents

### Files

#### List & Display DTOs
- **EditionListDto.cs** - Edition list item with key information
- **EditionSelectDto.cs** - Edition for dropdown selection
- **SubscribableEditionComboboxItemDto.cs** - Edition with subscription info for selection
- **EditionWithFeaturesDto.cs** - Complete edition with all feature definitions

#### Edit DTOs
- **EditionEditDto.cs** - Edition details for editing
- **EditionCreateDto.cs** - New edition creation
- **UpdateEditionDto.cs** - Update existing edition
- **CreateOrUpdateEditionDto.cs** - Unified create/update operation with features

#### Feature DTOs
- **FlatFeatureDto.cs** - Feature definition:
  - Name - Feature identifier
  - DefaultValue - Default feature value
  - DisplayName - User-friendly name
  - Description - Feature explanation
  - InputType - Feature value type (checkbox, number, text)

- **FlatFeatureSelectDto.cs** - Feature with current value for selection
- **FeatureInputTypeDto.cs** - Feature input type definition (checkbox, single-line text, etc.)

#### Input DTOs
- **MoveTenantsToAnotherEditionDto.cs** - Bulk tenant edition change:
  - SourceEditionId - Current edition
  - TargetEditionId - New edition
  - TenantIds - Tenants to migrate

#### Output DTOs
- **GetEditionEditOutput.cs** - Complete edition edit context:
  - Edition details
  - All available features
  - Current feature values
  - Feature permissions

#### Utility DTOs
- **LocalizableComboboxItemDto.cs** - Localized dropdown item
- **LocalizableComboboxItemSourceDto.cs** - Localized dropdown source

### Key Components

#### Edition Features
Features control access to:
- Maximum user count
- Storage space limits
- Module access (Chat, Audit, Dashboard)
- API call limits
- Support level
- Custom features per business needs

#### Pricing Models
- Free - Limited features, no cost
- Trial - Full features, time-limited
- Paid - Recurring subscription (monthly/annual)
- Custom - Enterprise negotiated pricing

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- **System.ComponentModel.DataAnnotations** - Validation

## Architecture Notes

### Feature-Based Access Control
- Features toggle functionality at runtime
- No code changes needed for new tiers
- Feature values can be boolean, numeric, or text
- Hierarchical feature relationships

### Multi-Tenant Isolation
- Each tenant subscribed to one edition
- Edition change affects feature availability immediately
- Downgrade may restrict access to previously available features
- Upgrade unlocks new capabilities

### Default Edition
- System has one default edition for new tenant signups
- Provides baseline feature set
- Can be changed per tenant after creation

## Business Logic

### Edition Lifecycle
1. **Create Edition**: Admin defines new pricing tier with features
2. **Configure Features**: Set feature values (max users, storage, modules)
3. **Set Pricing**: Define subscription cost and billing cycle
4. **Tenant Subscribe**: Tenant selects edition during signup or upgrade
5. **Feature Enforcement**: System checks features before allowing operations
6. **Edition Migration**: Move tenants between editions (upgrade/downgrade)

### Feature Checking
```csharp
if (await FeatureChecker.IsEnabledAsync("App.ChatFeature"))
{
    // Chat functionality available
}

var maxUserCount = await FeatureChecker.GetValueAsync("App.MaxUserCount");
if (currentUserCount >= int.Parse(maxUserCount))
{
    // User limit reached
}
```

### Tenant Edition Change
1. Admin initiates MoveTenantsToAnotherEditionDto
2. System validates target edition
3. Features updated for affected tenants
4. Billing adjusted prorated
5. Tenants notified of change

## Usage Across Codebase
These DTOs are consumed by:
- **IEditionAppService** - Edition CRUD operations
- **Tenant Registration** - Edition selection during signup
- **Subscription Management** - Edition upgrades/downgrades
- **Feature Checking** - Runtime feature availability
- **Billing Services** - Pricing and invoicing
- **Admin Dashboard** - Edition management interface

## Cross-Reference Impact
Changes to these DTOs affect:
- Edition management interfaces
- Tenant registration and subscription flows
- Feature availability checking throughout application
- Billing and payment systems
- Tenant migration tools
- SaaS pricing page
- Feature restriction enforcement