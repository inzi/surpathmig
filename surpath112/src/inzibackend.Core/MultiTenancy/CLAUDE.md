# MultiTenancy Documentation

## Overview
Core multi-tenant architecture implementation providing complete tenant isolation, subscription management, payment processing, and automated tenant lifecycle management for the SaaS platform.

## Contents

### Core Files

#### Tenant.cs
- **Purpose**: Extended tenant entity with custom properties
- **Key Features**:
  - Subscription management
  - Payment settings (IsDonorPay)
  - Custom branding options
  - Edition assignments

#### TenantManager.cs
- **Purpose**: Business logic for tenant operations
- **Key Features**:
  - Tenant creation and setup
  - Edition management
  - Subscription handling
  - Default data initialization

### Event Data Classes

#### TenantEditionChangedEventData.cs
- **Purpose**: Event data for edition changes
- **Usage**: Triggered when tenant changes subscription tier

#### RecurringPaymentsEnabledEventData.cs
- **Purpose**: Event for enabling recurring payments
- **Usage**: Subscription automation

#### RecurringPaymentsDisabledEventData.cs
- **Purpose**: Event for disabling recurring payments
- **Usage**: Payment method changes

#### EndSubscriptionResult.cs
- **Purpose**: Result object for subscription termination
- **Contains**: Success status, termination details

### Background Workers

#### SubscriptionExpirationCheckWorker.cs
- **Purpose**: Periodic check for expiring subscriptions
- **Key Features**:
  - Automated expiration monitoring
  - Grace period handling
  - Notification triggering

#### SubscriptionExpireEmailNotifierWorker.cs
- **Purpose**: Sends expiration warning emails
- **Key Features**:
  - Scheduled notifications
  - Multiple warning stages
  - Template-based emails

### Subfolders

#### Accounting/
- Invoice management
- Payment tracking
- Billing history

#### Demo/
- Demo tenant creation
- Sample data generation

#### Payments/
- Payment gateway integrations
- Stripe, PayPal, Authorize.Net support
- Transaction processing

### Key Components

- **Tenant**: Core tenant entity
- **TenantManager**: Tenant business logic
- **Background Workers**: Automated maintenance

### Dependencies

- **External Libraries**:
  - ABP Framework (Multi-tenancy module)
  - Payment gateway SDKs

- **Internal Dependencies**:
  - Edition management
  - User system
  - Email notifications

## Architecture Notes

- **Pattern**: Domain-driven with managers
- **Isolation**: Complete data isolation
- **Automation**: Background workers for maintenance
- **Events**: Event-driven architecture

## Business Logic

### Tenant Lifecycle
1. Registration and creation
2. Trial period initialization
3. Subscription activation
4. Usage monitoring
5. Renewal/expiration
6. Data archival/deletion

### Subscription Management
- Edition-based features
- Trial periods
- Grace periods
- Automatic renewals
- Payment processing

### Custom Properties
- **IsDonorPay**: Controls payment model
- **SubscriptionEndDate**: Tracks active subscription
- **IsInTrialPeriod**: Trial status
- **CustomCssId**: Branding support

## Usage Across Codebase

Multi-tenancy affects:
- Data access (all queries)
- User authentication
- Permission scoping
- Feature availability
- Payment processing
- Email communications
- Background jobs
- File storage

## Security Considerations

- Complete data isolation
- Tenant context validation
- Cross-tenant access prevention
- Secure payment handling
- Audit logging per tenant