# MultiTenancy Documentation

## Overview
Multi-tenancy management service interfaces and DTOs for the SaaS platform. Handles tenant creation, configuration, feature management, subscriptions, payments, and tenant isolation in the school compliance tracking system.

## Contents

### Files (Service Interfaces)

#### Core Tenant Management
- **ITenantAppService.cs** - Primary tenant management service:
  - CRUD operations for tenants
  - Feature management per tenant
  - Tenant admin unlocking
  - Tenant creation wizard for onboarding

- **ITenantRegistrationAppService.cs** - Self-service tenant registration:
  - New tenant signup
  - Trial activation
  - Registration validation

- **ISubscriptionAppService.cs** - Subscription and billing management:
  - Plan selection and upgrades
  - Payment processing
  - Subscription status tracking

### Key Components
- Tenant isolation at database level
- Feature toggling per tenant
- Edition-based feature sets
- Payment gateway integrations
- Host dashboard for SaaS management

### Dependencies
- Abp.Application.Services - ABP multi-tenancy framework
- MultiTenancy.Dto - Tenant management DTOs
- Payment provider integrations

## Subfolders

### Dto
Core tenant management DTOs including:
- Tenant creation and editing
- Feature configuration
- Subscription management
- Registration workflows

### Payments
Payment processing infrastructure:

#### AuthorizeNet
Authorize.Net payment gateway integration

#### PayPal
PayPal payment processing

#### Stripe
Stripe payment gateway integration

#### Dto
Common payment DTOs and interfaces

### HostDashboard
Host-side dashboard for SaaS management:
- Tenant statistics
- Revenue tracking
- System health monitoring
- Usage analytics

## Architecture Notes
- Database-per-tenant or shared database with row-level security
- Feature flags controlled at tenant level
- Edition system for tiered pricing
- Multiple payment gateway support
- Tenant data completely isolated
- Host has super-admin capabilities

## Business Logic
- **Tenant Creation**: Wizard-based onboarding with initial configuration
- **Feature Management**: Features enabled based on edition/subscription
- **Trial Management**: Time-limited trials with automatic conversion
- **Payment Processing**: Multiple gateway support with fallback
- **Tenant Isolation**: Complete data isolation between tenants
- **Admin Management**: Tenant admin accounts with unlock capabilities
- **Subscription Tiers**: Different feature sets per pricing tier

## Usage Across Codebase
Multi-tenancy services are fundamental to:
- All application services for tenant context
- Data access layer for tenant filtering
- Authentication for tenant resolution
- UI for tenant-specific branding
- Background jobs for tenant-specific processing
- API endpoints for tenant isolation
- Reporting for tenant-specific data