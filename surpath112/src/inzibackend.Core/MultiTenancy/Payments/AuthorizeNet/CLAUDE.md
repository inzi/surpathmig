# AuthorizeNet Payment Gateway Documentation

## Overview
Authorize.Net payment gateway integration for subscription payments. Currently contains placeholder implementations with most functionality not yet implemented.

## Contents

### Files

#### AuthorizeNetManager.cs
- **Purpose**: Manager service for Authorize.Net payment processing
- **Status**: Placeholder implementation (not active)
- **Key Features**:
  - Implements `ISupportsRecurringPayments` interface
  - Event handlers stubbed out (not implemented)
  - `HandleInvoicePaymentSucceededAsync()` method contains commented-out Stripe-like logic
- **Pattern**: Service base with transient lifetime

#### AuthorizeNetConfiguration.cs
- **Purpose**: Configuration for Authorize.Net payment gateway
- **Status**: Incomplete implementation
- **Key Features**:
  - Implements `IPaymentGatewayConfiguration` interface
  - Properties throw `NotImplementedException`
  - Only WebhookSecret property implemented (reads from configuration)
- **Configuration Key**: `Payment:AuthorizeNet:WebhookSecret`

### Key Components

- **AuthorizeNetManager**: Payment processing service (inactive)
- **AuthorizeNetConfiguration**: Configuration provider (partial)

### Dependencies

- **External Libraries**:
  - ABP Framework (dependency injection, events)

- **Internal Dependencies**:
  - `inzibackendServiceBase` (base service class)
  - `ISupportsRecurringPayments` (payment interface)
  - `IPaymentGatewayConfiguration` (configuration interface)

## Architecture Notes

- **Pattern**: Manager pattern for payment operations
- **Status**: Not production-ready
- **Integration**: Intended for multi-tenant subscription payments
- **Lifecycle**: Transient dependency injection

## Business Logic

### Intended Functionality (Not Implemented)
1. Process subscription payments
2. Handle recurring billing
3. Respond to payment events
4. Manage customer subscriptions

### Event Handlers (Stubbed)
- `RecurringPaymentsDisabledEventData`: When recurring payments are turned off
- `RecurringPaymentsEnabledEventData`: When recurring payments are turned on
- `TenantEditionChangedEventData`: When tenant changes subscription tier
- Invoice payment succeeded event

## Usage Across Codebase

**Status**: Currently not used in production. The codebase primarily uses:
- Stripe (fully implemented)
- PayPal (functional)
- Authorize.Net (placeholder only)

## Implementation Notes

- Contains commented-out code similar to Stripe implementation
- Would need full implementation before activation
- Configuration properties need to be implemented
- Event handlers need business logic
- Gateway registration and activation required

## Security Considerations

- Webhook secret configuration available
- Full implementation would require:
  - API credential handling
  - Transaction security
  - PCI compliance
  - Webhook signature verification