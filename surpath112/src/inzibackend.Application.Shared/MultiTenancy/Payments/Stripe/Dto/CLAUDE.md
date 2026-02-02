# Stripe Payment DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for Stripe payment gateway integration. Stripe is the primary payment processor providing credit card processing, subscription management, and comprehensive payment infrastructure. These DTOs handle Stripe-specific payment flows including checkout sessions, payment confirmation, and webhook processing.

## Contents

### Files

#### Configuration
- **StripeConfigurationDto.cs** - Stripe gateway configuration:
  - PublishableKey - Stripe publishable key (client-side)
  - SecretKey - Stripe secret key (server-side)
  - IsActive - Gateway enabled/disabled
  - PaymentMethodTypes - Accepted payment methods (card, etc.)
  - WebhookSecret - Webhook signature verification
  - Used by host to configure Stripe integration

#### Payment Operations
- **StripeCreatePaymentSessionInput.cs** - Create Stripe Checkout session:
  - Amount - Payment amount in cents
  - Currency - Currency code (usd, eur, etc.)
  - SuccessUrl - Redirect after successful payment
  - CancelUrl - Redirect if user cancels
  - EditionId - Subscription edition
  - PaymentPeriodType - Monthly/Annual billing
  - Creates Stripe Checkout Session and returns session ID

- **StripeGetPaymentInput.cs** - Retrieve payment details:
  - StripeSessionId - Checkout session ID
  - Used to query payment status

- **StripeConfirmPaymentInput.cs** - Confirm payment completion:
  - StripeSessionId - Session to confirm
  - Called after user completes payment

- **StripePaymentResultOutput.cs** - Payment result:
  - Success - Payment succeeded
  - SessionId - Stripe session ID
  - PaymentIntentId - Stripe PaymentIntent ID
  - CustomerId - Stripe customer ID
  - SubscriptionId - Stripe subscription ID (for recurring)
  - InvoiceId - Stripe invoice ID
  - ErrorMessage - Error details if failed

### Key Components

#### Stripe Checkout
- Hosted checkout page (redirect or embedded)
- PCI-compliant card collection
- Multiple payment methods (cards, Apple Pay, Google Pay)
- Automatic receipt emails
- Mobile-optimized

#### Stripe Subscriptions
- Recurring billing automation
- Prorated upgrades/downgrades
- Trial periods
- Usage-based billing
- Invoice generation

#### Webhooks
- Async payment confirmation
- Subscription lifecycle events
- Failed payment notifications
- Customer updates

### Dependencies
- **Stripe .NET SDK** - Official Stripe library
- **inzibackend.MultiTenancy.Payments.Dto** - Base payment DTOs

## Architecture Notes

### PCI Compliance
- Stripe.js handles card tokenization
- No card data touches server
- PCI-DSS Level 1 certified
- SCA (Strong Customer Authentication) support

### Security
- Publishable key for client (safe to expose)
- Secret key for server (encrypted storage)
- Webhook signatures verified
- Customer data encrypted at Stripe

### Idempotency
- Idempotency keys prevent duplicate charges
- Safe to retry failed API calls
- Webhook events deduplicated

### Error Handling
- Card declined errors
- Network errors with retry logic
- 3D Secure authentication
- Fraud detection responses

## Business Logic

### Stripe Payment Workflow
1. **Create Session**: StripeCreatePaymentSessionInput creates Checkout session
2. **Redirect**: User redirected to Stripe Checkout
3. **Payment**: User enters card details at Stripe
4. **Return**: User redirected to SuccessUrl after payment
5. **Confirm**: StripeConfirmPaymentInput confirms completion
6. **Webhook**: Stripe sends webhook for async confirmation
7. **Activate**: Subscription activated after confirmation
8. **Result**: StripePaymentResultOutput contains payment details

### Subscription Management
- **Create**: New subscription during initial payment
- **Renew**: Automatic renewal via Stripe subscriptions
- **Upgrade**: Change subscription with proration
- **Cancel**: Cancel subscription (immediate or end of period)
- **Failed Payment**: Retry logic and dunning

### Webhook Events
- `checkout.session.completed` - Payment succeeded
- `invoice.payment_succeeded` - Subscription renewal
- `invoice.payment_failed` - Payment failed
- `customer.subscription.deleted` - Subscription cancelled
- `customer.subscription.updated` - Subscription changed

### Testing
- Test mode with test cards
- Simulate successful/failed payments
- Test webhook events locally
- Verify before production

## Usage Across Codebase
These DTOs are consumed by:
- **IStripePaymentAppService** - Stripe payment operations
- **Payment Gateway Services** - Payment processing
- **Subscription Services** - Recurring billing
- **Webhook Controllers** - Stripe webhook handling
- **Configuration UI** - Stripe setup
- **Invoice Services** - Stripe invoice retrieval

## Cross-Reference Impact
Changes to these DTOs affect:
- Stripe payment processing flow
- Checkout session creation
- Payment confirmation logic
- Webhook processing
- Subscription management
- Invoice generation
- Failed payment handling
- Gateway configuration interface