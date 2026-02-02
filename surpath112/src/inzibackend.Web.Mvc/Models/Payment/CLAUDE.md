# Models/Payment Documentation

## Overview
This folder contains shared view models for payment-related functionality, providing common models used across different payment gateways (Stripe, PayPal) and payment scenarios.

## Files

### PaymentInfoViewModel.cs
**Purpose**: Displays current payment/subscription information

**Properties**:
- `Edition`: Current subscription plan details
  - `DisplayName`: Plan name
  - `Features`: List of included features
- `Subscription`: Subscription status
  - `StartDate`: Subscription start
  - `EndDate`: Expiry or renewal date
  - `IsActive`: Current status
  - `IsInTrialPeriod`: Free trial indicator
- `BillingAddress`: Saved billing information (optional)
- `PaymentMethod`: Last 4 digits of card or PayPal account

**Usage**: Subscription management page, billing section

### CreatePaymentModel.cs
**Purpose**: Model for initiating a payment

**Properties**:
- `EditionId`: Plan being purchased
- `PaymentGateway`: Stripe, PayPal, or other
- `PaymentPeriodType`: Monthly, Annual
- `Amount`: Payment amount
- `SuccessUrl`: Redirect after success
- `CancelUrl`: Redirect if cancelled

**Usage**: Payment gateway selection and initiation

### PaymentResultViewModel.cs
**Purpose**: Shows payment result (success or failure)

**Properties**:
- `IsSuccess`: Whether payment succeeded
- `TransactionId`: Payment gateway transaction ID
- `OrderId`: Internal order ID
- `Amount`: Amount charged
- `Currency`: Payment currency
- `EditionName`: Plan purchased
- `ErrorMessage`: Error details if failed
- `Receipt`: Receipt data (invoice number, date, etc.)

**Usage**: Post-payment confirmation page

### UpgradeEditionViewModel.cs
**Purpose**: Model for subscription upgrade/downgrade

**Properties**:
- `CurrentEditionId`: Current plan
- `TargetEditionId`: Desired plan
- `IsUpgrade`: Whether upgrade (vs. downgrade)
- `ProrationAmount`: Credit or charge for plan change
- `EffectiveDate`: When change takes effect (immediate or next billing)
- `Features`: Feature comparison between plans

**Usage**: Subscription upgrade/downgrade flow

### PaymentGatewayModel.cs
**Purpose**: Configuration for payment gateway selection

**Properties**:
- `Gateway`: Enum (Stripe, PayPal, Manual)
- `DisplayName`: Human-readable name
- `IsActive`: Whether gateway is enabled
- `SupportedCurrencies`: List of supported currencies
- `MinimumAmount`: Minimum payment amount
- `SupportsRecurring`: Whether supports subscriptions

**Usage**: Payment gateway selection page

## Architecture Notes

### Gateway Abstraction
These models provide gateway-agnostic interface:
- Controller receives `CreatePaymentModel`
- Based on `PaymentGateway` property, routes to appropriate handler:
  - `StripePaymentService`
  - `PayPalPaymentService`
  - `ManualPaymentService` (bank transfer, invoice)
- Each service returns `PaymentResultViewModel`

### Payment Flow
```
User selects edition
    ↓
CreatePaymentModel populated
    ↓
User selects payment gateway
    ↓
Gateway-specific flow initiated
    ↓
Payment processed
    ↓
PaymentResultViewModel returned
    ↓
Subscription activated
    ↓
Confirmation shown
```

### Multi-Currency Support
- Amounts stored in database in USD
- Display converted to user's currency
- Payment gateway handles currency conversion
- Exchange rates tracked for reporting

## Usage Across Codebase

### Controllers
- `PaymentController.CreatePayment()`: Initiates payment
- `PaymentController.PaymentCallback()`: Processes gateway callback
- `PaymentController.PaymentInfo()`: Shows subscription info
- `SubscriptionController.Upgrade()`: Handles plan changes

### Views
- `/Views/Payment/CreatePayment.cshtml`: Payment gateway selection
- `/Views/Payment/PaymentResult.cshtml`: Success/failure page
- `/Views/Subscription/Info.cshtml`: Current subscription info
- `/Views/Subscription/Upgrade.cshtml`: Plan upgrade page

### Services
- `IPaymentManager`: Coordinates payment processing
- `IStripePaymentService`: Stripe implementation
- `IPayPalPaymentService`: PayPal implementation
- `ISubscriptionAppService`: Subscription management

## Business Logic

### Proration Calculation
When upgrading/downgrading mid-cycle:
1. Calculate unused time on current plan
2. Calculate credit amount (if upgrade) or charge (if downgrade)
3. Apply proration to new plan's first payment
4. Example:
   - Current: $10/month, 15 days remaining
   - Target: $20/month
   - Credit: $5 (50% of month unused)
   - First payment for new plan: $20 - $5 = $15

### Subscription Lifecycle
- **Trial**: Free period, auto-converts to paid
- **Active**: Paid subscription, features enabled
- **Past Due**: Payment failed, grace period
- **Cancelled**: No longer renewing, but active until expiry
- **Expired**: Subscription ended, features disabled
- **Suspended**: Manually suspended by admin

### Payment Method Management
- Save card/PayPal for recurring payments
- PCI compliance (tokenization, no raw card data)
- Allow multiple payment methods
- Set default payment method
- Update payment method without losing subscription

## Dependencies

### Internal
- `inzibackend.MultiTenancy.Payments`: Payment domain logic
- `inzibackend.Editions`: Plan management
- Gateway-specific service interfaces

### External
- Payment gateway SDKs (Stripe, PayPal)
- Currency conversion APIs (optional)
- Receipt generation libraries

## Security Considerations
- Never store raw card numbers
- Use payment gateway tokenization
- PCI DSS compliance via gateway
- HTTPS for all payment pages
- Validate payment amounts server-side (prevent tampering)
- Webhook signature validation
- Rate limiting on payment endpoints

## Testing
- Use gateway test/sandbox modes
- Test cards provided by gateways
- Test all payment scenarios:
  - Successful payment
  - Declined payment
  - Network errors
  - Webhook delivery
  - Proration calculations
  - Upgrade/downgrade flows

## Related Documentation
- [Models/Stripe/CLAUDE.md](../Stripe/CLAUDE.md): Stripe-specific models
- [Models/Paypal/CLAUDE.md](../Paypal/CLAUDE.md): PayPal-specific models
- [Controllers/CLAUDE.md](../../Controllers/CLAUDE.md): Payment controllers
- [inzibackend.Core.Shared/MultiTenancy/Payments/CLAUDE.md](../../../../inzibackend.Core.Shared/MultiTenancy/Payments/CLAUDE.md): Payment domain models
- [Models/CLAUDE.md](../CLAUDE.md): Parent folder documentation