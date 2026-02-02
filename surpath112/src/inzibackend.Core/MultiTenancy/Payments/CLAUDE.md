# Payments Documentation

## Overview
Comprehensive subscription payment system supporting multiple payment gateways (Stripe, PayPal, Authorize.Net) with recurring billing, upgrade/downgrade handling, and payment tracking.

## Contents

### Core Files

#### SubscriptionPayment.cs
- **Purpose**: Entity representing a subscription payment transaction
- **Table**: AppSubscriptionPayments
- **Key Properties**:
  - `Gateway`: Payment gateway type (Stripe/PayPal/AuthorizeNet)
  - `Amount`: Payment amount
  - `Status`: Payment status (NotPaid/Paid/Completed/Failed/Cancelled)
  - `EditionId`: Subscription tier
  - `TenantId`: Associated tenant
  - `DayCount`: Duration of subscription
  - `PaymentPeriodType`: Daily/Weekly/Monthly/Annual
  - `ExternalPaymentId`: Gateway's transaction ID
  - `IsRecurring`: Automatic renewal flag
  - `InvoiceNo`: Associated invoice number
  - `EditionPaymentType`: Upgrade/Extend/BuyNow
  - `SuccessUrl` / `ErrorUrl`: Redirect URLs
- **Status Methods**:
  - `SetAsPaid()`: Mark payment completed
  - `SetAsCompleted()`: Finalize payment
  - `SetAsCancelled()`: Cancel payment
  - `SetAsFailed()`: Mark as failed
- **Helper Methods**:
  - `GetPaymentPeriodType()`: Convert DayCount to enum
  - `IsProrationPayment()`: Check if upgrade proration

#### IPaymentGatewayConfiguration.cs
- **Purpose**: Interface for payment gateway configuration
- **Properties**:
  - `IsActive`: Gateway enabled status
  - `SupportsRecurringPayments`: Recurring capability
  - `GatewayType`: Gateway identifier

#### IPaymentGatewayStore.cs
- **Purpose**: Interface for gateway management
- **Methods**:
  - `GetActiveGateways()`: List enabled gateways

#### PaymentGatewayStore.cs
- **Purpose**: Implementation of gateway store
- **Key Features**:
  - Discovers all registered gateway configurations
  - Filters by active status
  - Returns gateway capabilities
  - Uses IoC resolution for dynamic discovery

#### ISupportsRecurringPayments.cs
- **Purpose**: Interface for gateways with recurring payment support
- **Event Handlers**:
  - `RecurringPaymentsEnabledEventData`: When enabled
  - `RecurringPaymentsDisabledEventData`: When disabled
  - `TenantEditionChangedEventData`: On tier change
  - `HandleInvoicePaymentSucceededAsync()`: Webhook handler

#### ISubscriptionPaymentRepository.cs
- **Purpose**: Repository interface for payment queries
- **Methods**: (inferred)
  - Find payments by gateway and external ID
  - Get last completed payment
  - Query by tenant and status

#### SubscriptionPaymentExtensionData.cs
- **Purpose**: Key-value extension data for payments
- **Use Case**: Store gateway-specific metadata

#### ISubscriptionPaymentExtensionDataRepository.cs
- **Purpose**: Repository for extension data

### Subfolders

#### Stripe/
- Full Stripe integration
- Subscription management
- Webhook handling
- See [Stripe/CLAUDE.md](Stripe/CLAUDE.md)

#### Paypal/
- PayPal checkout integration
- One-time payments
- See [Paypal/CLAUDE.md](Paypal/CLAUDE.md)

#### AuthorizeNet/
- Authorize.Net placeholder
- Not production-ready
- See [AuthorizeNet/CLAUDE.md](AuthorizeNet/CLAUDE.md)

### Key Components

- **SubscriptionPayment**: Payment entity with state machine
- **PaymentGatewayStore**: Gateway registry
- **Payment Configurations**: Per-gateway settings
- **Event Handlers**: Recurring payment lifecycle

### Dependencies

- **External Libraries**:
  - ABP Framework (domain entities, events)
  - Stripe.net SDK
  - PayPalCheckoutSdk
  - Entity Framework Core

- **Internal Dependencies**:
  - Edition management
  - Tenant management
  - Invoice generation
  - Multi-tenancy system

## Architecture Notes

- **Pattern**: Strategy pattern for multiple gateways
- **State Machine**: Payment status transitions
- **Events**: Event-driven for recurring payments
- **Extensibility**: Easy to add new gateways

## Business Logic

### Payment Lifecycle

#### One-Time Payment
1. Create payment record (Status: NotPaid)
2. User redirected to gateway
3. Payment processed
4. Webhook/callback sets status to Paid
5. System extends subscription
6. Status set to Completed

#### Recurring Payment
1. Initial payment created (IsRecurring: true)
2. Subscription created at gateway
3. Gateway charges automatically on renewal
4. Webhook notifies of successful charge
5. New payment record created
6. Subscription extended
7. Previous recurring payment marked non-recurring

### Payment Status Transitions
- **NotPaid** → **Paid**: Payment successful
- **NotPaid** → **Cancelled**: User cancelled
- **Paid** → **Completed**: Subscription activated
- **Any** → **Failed**: Payment error

### Upgrade/Downgrade Handling
- **Upgrade**: Proration charge, immediate effect
- **Downgrade**: Applied at end of current period
- **Recurring Update**: Gateway subscription updated
- **Edition Change Event**: Triggers gateway updates

### Day Count to Period Mapping
- 1 day → Daily
- 7 days → Weekly
- 30 days → Monthly
- 365 days → Annual

## Usage Across Codebase

Used by:
- Subscription purchase controllers
- Webhook endpoint handlers
- Payment history pages
- Tenant dashboard
- Edition upgrade/downgrade flows
- Subscription management
- Invoice generation

## Gateway Comparison

| Feature | Stripe | PayPal | Authorize.Net |
|---------|--------|--------|---------------|
| Status | Production | Production | Placeholder |
| Recurring | Yes | No | Not Implemented |
| Webhooks | Yes | No | Not Implemented |
| Proration | Yes | No | No |

## Security Considerations

### Payment Data
- No credit card storage
- Gateway handles PCI compliance
- External payment IDs only
- Secure webhook validation

### Status Transitions
- Protected status setters
- Validation before transitions
- Audit trail via FullAuditedEntity
- Unit of work transactions

### Gateway Security
- API keys in configuration
- Webhook signature verification
- HTTPS enforcement
- Timeout handling

## Extension Points

- Additional payment gateways
- Custom payment flows
- Promotional codes/discounts
- Refund handling
- Payment retry logic
- Failed payment recovery
- Dunning management
- Multi-currency support