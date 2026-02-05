# MultiTenancy Documentation

## Overview
Contains proxy services for multi-tenant management operations. Provides client-side access to tenant administration functionality including CRUD operations, feature management, and tenant-specific settings.

## Contents

### Files

#### ProxyTenantAppService.cs
- **Purpose**: Client proxy for ITenantAppService, handling comprehensive tenant management
- **Key Methods**:
  - `GetTenants`: Retrieves paginated list of tenants with filtering
  - `CreateTenant`: Creates new tenant
  - `GetTenantForEdit`: Retrieves tenant details for editing
  - `UpdateTenant`: Updates existing tenant information
  - `DeleteTenant`: Removes tenant from system
  - `GetTenantFeaturesForEdit`: Gets tenant-specific feature settings
  - `UpdateTenantFeatures`: Updates tenant feature configuration
  - `ResetTenantSpecificFeatures`: Resets features to defaults
  - `UnlockTenantAdmin`: Unlocks tenant administrator account
- **Base Class**: Inherits from ProxyAppServiceBase
- **Interface**: Implements ITenantAppService

### Key Components
- **Tenant Management**: Complete CRUD operations for tenants
- **Feature Management**: Tenant-specific feature configuration
- **Admin Control**: Tenant administrator account management

### Dependencies
- `Abp.Application.Services.Dto`: ABP framework DTOs
- `inzibackend.MultiTenancy.Dto`: Tenant-specific DTOs
- `ProxyAppServiceBase`: Base proxy service class

## Architecture Notes
- **Multi-tenant Architecture**: Core support for SaaS multi-tenancy
- **Feature Toggling**: Per-tenant feature management
- **Isolation**: Each tenant operates in isolation
- **Admin Hierarchy**: Separate admin per tenant

## Business Logic
- **Tenant Lifecycle**: Complete tenant journey from creation to deletion
- **Feature Management**:
  - Tenant-specific feature overrides
  - Reset to edition defaults
  - Feature inheritance from editions
- **Admin Management**: Each tenant has dedicated admin account
- **Data Isolation**: Ensures complete data separation between tenants

## Usage Across Codebase
- Used by host admin UI for tenant management
- Referenced in tenant selection interfaces
- Consumed by subscription management features
- Integrated with billing and licensing systems
- Part of the SaaS infrastructure

## Security Considerations
- Host-only operations for tenant management
- Tenant isolation enforced at all levels
- Admin account security per tenant
- Feature restrictions based on subscription

## Cross-References
- **Edition Services**: Works with edition management for feature inheritance
- **User Services**: Coordinates with user management for tenant users
- **Authentication**: Integrates with multi-tenant authentication
- **Subscription**: Connected to billing and subscription management