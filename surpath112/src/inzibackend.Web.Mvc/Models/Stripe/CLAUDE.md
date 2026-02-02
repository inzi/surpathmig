# Models/Stripe Documentation

## Overview
This folder contains view models for Stripe payment integration, providing an alternative payment gateway to PayPal for subscription purchases and upgrades.

## Files

### StripePurchaseViewModel.cs
**Purpose**: View model for Stripe checkout page

**Properties**:
- `EditionId`: Edition/plan being purchased
- `EditionDisplayName`: Human-readable plan name
- `PaymentPeriodType`: Monthly, Annual, etc.
- `Amount`: Payment amount in cents (Stripe convention)
- `Currency`: Payment currency (USD, EUR, etc.)
- `StripePublishableKey`: Public API key for client-side
- `SuccessUrl`: Redirect after successful payment
- `CancelUrl`: Redirect if payment cancelled
- `SessionId`: Stripe Checkout Session ID
- `Description`: Payment description for receipt

## Architecture Notes

### Payment Flow (Stripe Checkout)
1. User selects edition/plan to purchase
2. Server creates Stripe Checkout Session via API
3. `StripePurchaseViewModel` populated with session details
4. View redirects user to Stripe Checkout page (hosted by Stripe)
5. User enters payment info on Stripe's secure page
6. Stripe processes payment
7. Stripe redirects to `SuccessUrl` on success
8. Webhook confirms payment
9. Subscription activated

### Payment Flow (Stripe Elements)
Alternative implementation using embedded payment form:
1. View loads Stripe.js and Stripe Elements
2. Payment form rendered in-app
3. User enters card details (never touches server)
4. Stripe tokenizes card (creates PaymentMethod)
5. Token sent to server
6. Server charges PaymentMethod via API
7. Subscription activated on success

### Security
- `StripePublishableKey` safe for client-side use
- Secret key never exposed to browser
- PCI compliance maintained (card data never touches server)
- Webhook signature validation
- HTTPS required
- CSRF protection on all endpoints

### Integration Points
- Stripe Checkout API (hosted payment page)
- Stripe Elements API (embedded payment form)
- Stripe Webhooks for payment confirmations
- Stripe Customer API for recurring subscriptions
- Stripe Invoice API for billing

## Usage Across Codebase

### Controllers
- `StripeController.Purchase()`: Initiates checkout
- `StripeController.CreateCheckoutSession()`: Creates Stripe session
- `StripeController.PaymentCallback()`: Handles redirect after payment
- `StripeController.Webhook()`: Processes Stripe webhooks

### Views
- `/Views/Stripe/Purchase.cshtml`: Stripe checkout page
  - Loads Stripe.js SDK
  - Redirects to Stripe Checkout
  - Or renders Stripe Elements form (alternative)
  - Handles success/error callbacks

### Services
- `IStripePaymentService`: Stripe API wrapper
- `IPaymentManager`: Payment coordination
- `ISubscriptionAppService`: Subscription management
- Webhook event handlers

## Business Logic

### Checkout Session Creation
```csharp
// Server-side
var session = stripe.checkout.sessions.create({
    payment_method_types: ["card"],
    line_items: [{
        price_data: {
            currency: "usd",
            unit_amount: model.Amount, // cents
            product_data: {
                name: model.EditionDisplayName,
                description: "Annual Subscription"
            }
        },
        quantity: 1
    }],
    mode: "subscription", // or "payment" for one-time
    success_url: model.SuccessUrl,
    cancel_url: model.CancelUrl
});

model.SessionId = session.Id;
```

### Webhook Processing
Stripe sends webhooks for events:
- `checkout.session.completed`: Payment successful
- `invoice.payment_succeeded`: Recurring payment successful
- `invoice.payment_failed`: Payment failed (retry or cancel)
- `customer.subscription.updated`: Subscription changed
- `customer.subscription.deleted`: Subscription cancelled

Application responds by:
- Activating subscription
- Sending confirmation emails
- Updating tenant edition
- Logging for audit

### Subscription Management
- **New Subscription**: Create Stripe Customer and Subscription
- **Upgrade/Downgrade**: Update Subscription with proration
- **Renewal**: Automatic via Stripe (webhook notification)
- **Cancellation**: Cancel subscription, set expiry date
- **Failed Payment**: Retry logic, dunning emails

## Dependencies

### Internal
- `inzibackend.MultiTenancy.Payments`: Payment domain
- `inzibackend.Editions`: Plan management
- Configuration settings for Stripe keys

### External
- `Stripe.net` NuGet package: .NET SDK
- Stripe.js: Client-side JavaScript library
- Stripe Elements: Pre-built UI components

### Configuration
Required settings:
- `StripePublishableKey`: Public API key
- `StripeSecretKey`: Secret API key (server-only)
- `StripeWebhookSecret`: Webhook signing secret
- Environment: Test mode vs. Live mode

## Testing

### Test Mode
- Use test API keys (starts with `pk_test_` and `sk_test_`)
- Test card numbers:
  - Success: `4242 4242 4242 4242`
  - Decline: `4000 0000 0000 0002`
  - 3D Secure: `4000 0027 6000 3184`
- Webhook testing via Stripe CLI

### Test Scenarios
- Successful payment
- Declined card
- Expired card
- 3D Secure authentication
- Subscription creation
- Subscription upgrade/downgrade
- Recurring payment
- Failed renewal payment
- Refund processing

## Comparison with PayPal

| Feature | Stripe | PayPal |
|---------|--------|--------|
| **UI** | Hosted (Checkout) or Embedded (Elements) | Hosted (Smart Buttons) |
| **PCI Compliance** | Stripe handles | PayPal handles |
| **Recurring** | Native subscriptions | Subscriptions or billing agreements |
| **Fees** | 2.9% + $0.30 | Similar |
| **International** | 135+ currencies | 100+ currencies |
| **Developer Experience** | Excellent docs, modern API | Good docs, REST API |
| **Settlement** | 2-7 days | 1-3 days |

## Related Documentation
- [Models/Paypal/CLAUDE.md](../Paypal/CLAUDE.md): PayPal payment integration
- [Models/Payment/CLAUDE.md](../Payment/CLAUDE.md): General payment models
- [Controllers/CLAUDE.md](../../Controllers/CLAUDE.md): Payment controllers
- [inzibackend.Core.Shared/MultiTenancy/Payments/CLAUDE.md](../../../../inzibackend.Core.Shared/MultiTenancy/Payments/CLAUDE.md): Payment domain
- [Models/CLAUDE.md](../CLAUDE.md): Parent folder documentation