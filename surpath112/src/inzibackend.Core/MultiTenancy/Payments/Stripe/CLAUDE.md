# Stripe Payment Gateway Documentation

## Overview
Full-featured Stripe payment gateway integration supporting both one-time and recurring subscription payments. Handles subscription management, plan creation, webhook events, and edition changes.

## Contents

### Files

#### StripeGatewayManager.cs
- **Purpose**: Comprehensive manager service for Stripe payment processing
- **Key Features**:
  - Subscription creation and updates
  - Plan and product management
  - Recurring payment handling
  - Edition change management
  - Invoice processing
  - Webhook event handling
  - Customer management
- **Pattern**: Service base implementing `ISupportsRecurringPayments`
- **Dependencies**: Stripe SDK, TenantManager, EditionManager

#### StripePaymentGatewayConfiguration.cs
- **Purpose**: Configuration for Stripe payment gateway
- **Key Features**:
  - API keys management (publishable, secret, webhook)
  - Base URL configuration
  - Payment method types configuration
  - Active status and recurring payment support
- **Configuration Keys**:
  - `Payment:Stripe:BaseUrl`
  - `Payment:Stripe:PublishableKey`
  - `Payment:Stripe:SecretKey`
  - `Payment:Stripe:WebhookSecret`
  - `Payment:Stripe:IsActive`
  - `Payment:Stripe:PaymentMethodTypes` (array)

#### StripeIdResponse.cs
- **Purpose**: Simple response DTO containing Stripe entity ID
- **Usage**: Return value for creation operations

### Key Components

- **StripeGatewayManager**: Core payment processing service (424 lines)
- **StripePaymentGatewayConfiguration**: Configuration provider
- **StripeIdResponse**: Response model

### Dependencies

- **External Libraries**:
  - Stripe.net SDK (main integration)
  - Stripe.Checkout (checkout sessions)
  - ABP Framework (dependency injection, threading)

- **Internal Dependencies**:
  - `TenantManager` (tenant operations)
  - `EditionManager` (edition/plan management)
  - `ISubscriptionPaymentRepository` (payment persistence)
  - `inzibackendServiceBase` (base service class)

## Architecture Notes

- **Pattern**: Manager pattern with event-driven architecture
- **Status**: Fully implemented and production-ready
- **Integration**: Primary payment gateway for the platform
- **Lifecycle**: Transient dependency injection
- **Supports Recurring**: Yes (automatic subscriptions)

## Business Logic

### Subscription Management

#### Creating Subscriptions
1. Get or create Stripe product
2. Get or create pricing plan for edition/period
3. Create Stripe checkout session
4. Customer completes payment
5. Subscription activated automatically

#### Updating Subscriptions
1. Detect edition change event
2. Create new pricing plan if needed
3. Update Stripe subscription
4. Handle proration (optional)
5. Record payment in database

#### Canceling Subscriptions
1. Cancel in Stripe
2. Update payment record status
3. Trigger cleanup events

### Plan Management

#### Plan ID Format
`{EditionName}_{PaymentPeriod}_{Currency}`
Example: `Premium_Monthly_USD`

#### Plan Creation
- Dynamically creates plans as needed
- Associates with product (app name)
- Supports multiple intervals: day, week, month, year
- Handles pricing in cents (Stripe format)

### Event Handling

#### RecurringPaymentsDisabledEventData
- Switches to manual invoice collection
- Sets payment due date
- Maintains subscription active status

#### RecurringPaymentsEnabledEventData
- Returns to automatic charging
- Re-enables auto-renewal

#### TenantEditionChangedEventData
- Handles upgrade/downgrade scenarios
- Creates new subscription payment record
- Updates Stripe subscription plan
- Cancels subscription if downgrade to free

#### Invoice Payment Succeeded (Webhook)
- Extends tenant subscription
- Records payment
- Updates tenant status
- Disables trial period

### Price Conversion
- **ConvertToStripePrice**: Decimal to cents (long)
- **ConvertFromStripePrice**: Cents to decimal
- All amounts in Stripe are in smallest currency unit (cents for USD)

### Customer Management
- Customers identified by tenant name (description field)
- Session-based customer updates
- Links customers to subscriptions

## Usage Across Codebase

Primary payment gateway used by:
- Subscription purchase controllers
- Webhook handlers
- Edition management
- Tenant subscription lifecycle
- Recurring payment processing
- Payment history tracking

## Configuration

### Required Settings
```json
{
  "Payment": {
    "Stripe": {
      "BaseUrl": "https://yourapp.com/",
      "PublishableKey": "pk_test_xxx",
      "SecretKey": "sk_test_xxx",
      "WebhookSecret": "whsec_xxx",
      "IsActive": true,
      "PaymentMethodTypes": ["card"]
    }
  }
}
```

### Payment Method Types
Array of allowed payment methods in checkout:
- `card` (credit/debit cards)
- `alipay`, `ideal`, `sepa_debit`, etc.

## Key Methods

### Public API
- `UpdateSubscription`: Change subscription plan
- `CancelSubscription`: Cancel recurring payments
- `DoesPlanExistAsync`: Check plan existence
- `GetOrCreatePlanAsync`: Ensure plan exists
- `GetOrCreateProductAsync`: Ensure product exists
- `GetOrCreatePlanForPayment`: Setup plan from payment record
- `UpdateCustomerDescriptionAsync`: Update customer metadata
- `HandleInvoicePaymentSucceededAsync`: Process webhook events

### Helper Methods
- `GetPlanInterval`: Convert payment period to Stripe interval
- `GetPlanId`: Generate unique plan identifier
- `ConvertToStripePrice` / `ConvertFromStripePrice`: Currency conversion
- `GetLastCompletedSubscriptionPayment`: Find active subscription

## Security Considerations

- API keys stored in configuration (use secure storage)
- Webhook signature verification via WebhookSecret
- Server-side payment validation
- Secure customer identification via tenant name
- Payment status verification before processing
- Unit of work transactions for data consistency
- Currency validation (inzibackendConsts.Currency)

## Error Handling

- Stripe SDK exceptions caught and logged
- Graceful handling of missing entities
- Null checks on optional values
- Event handler safeguards against invalid states

## Constants

- **ProductName**: "inzibackend" (Stripe product identifier)
- **StripeSessionIdSubscriptionPaymentExtensionDataKey**: "StripeSessionId" (metadata key)

## Performance Considerations

- Async/await throughout
- Caching of last completed payment
- Conditional plan creation (checks existence first)
- Minimal API calls via SDK