# Models/Paypal Documentation

## Overview
This folder contains view models for PayPal payment integration, specifically for subscription purchases and upgrades within the multi-tenant SaaS application.

## Files

### PayPalPurchaseViewModel.cs
**Purpose**: View model for PayPal checkout page

**Properties**:
- `EditionId`: Edition/plan being purchased
- `EditionDisplayName`: Human-readable plan name
- `PaymentPeriodType`: Monthly, Annual, etc.
- `Amount`: Payment amount in USD
- `Currency`: Payment currency (default USD)
- `SuccessUrl`: Redirect after successful payment
- `CancelUrl`: Redirect if payment cancelled
- `ErrorUrl`: Redirect on payment error
- `OrderId`: Unique order identifier
- `PayPalOrderId`: PayPal's order ID after creation

## Architecture Notes

### Payment Flow
1. User selects edition/plan to purchase
2. `PayPalPurchaseViewModel` populated with order details
3. View renders PayPal Smart Payment Buttons
4. User completes payment on PayPal
5. PayPal redirects to `SuccessUrl` with transaction details
6. Controller processes payment and activates subscription
7. User redirected to confirmation page

### Security
- Order IDs validated on callback
- Amount verification prevents tampering
- PayPal IPN/webhook validation
- HTTPS required for all payment pages
- No sensitive data in view model (e.g., PayPal API keys)

### Integration Points
- PayPal REST API for order creation
- PayPal Smart Payment Buttons SDK (client-side)
- Webhook handlers for payment notifications
- Subscription activation service

## Usage Across Codebase

### Controllers
- `PayPalController.Purchase()`: Initiates payment flow
- `PayPalController.PaymentCallback()`: Handles successful payment
- `PayPalController.Webhook()`: Processes PayPal webhooks

### Views
- `/Views/PayPal/Purchase.cshtml`: PayPal checkout page
  - Renders view model data
  - Loads PayPal JavaScript SDK
  - Configures Smart Payment Buttons
  - Handles client-side callbacks

### Services
- `IPaymentManager`: Payment processing coordination
- `ISubscriptionAppService`: Subscription management
- `IPayPalApiService`: PayPal API wrapper
- `ITenantManager`: Tenant upgrade/subscription updates

## Business Logic

### Edition Selection
- User browses available editions (pricing tiers)
- Selects desired plan and payment period
- System creates pending order
- Generates `PayPalPurchaseViewModel` with order details

### Payment Processing
1. PayPal order created via API
2. User authenticates with PayPal
3. User approves payment
4. PayPal captures payment
5. Webhook notification sent to application
6. Subscription activated immediately
7. Confirmation email sent to user

### Error Handling
- Payment declined: Show error, allow retry
- Network error: Queue for retry via webhook
- Invalid order: Log and notify administrator
- Duplicate payment: Detect and refund/credit

### Subscription Activation
- Tenant edition upgraded immediately
- Features enabled based on new edition
- Expiry date set based on payment period
- Receipt email sent
- Admin notified of new subscription

## Dependencies

### Internal
- `inzibackend.MultiTenancy.Payments`: Payment domain logic
- `inzibackend.Editions`: Edition/plan management
- Tenant management services

### External
- PayPal REST API SDK
- PayPal Smart Payment Buttons (JavaScript)
- PayPal Webhooks for notifications

### Configuration
- PayPal ClientId (from settings)
- PayPal Secret (from settings)
- PayPal mode: Sandbox vs. Production
- Webhook URL for callbacks

## Testing Considerations

### Sandbox Testing
- Use PayPal Sandbox accounts
- Test credentials in development settings
- Webhook URL must be publicly accessible (ngrok for local dev)
- Test various scenarios: success, cancel, decline, error

### Test Cases
- Successful payment flow
- Payment cancellation
- Payment declined (insufficient funds)
- Network timeout during payment
- Duplicate payment attempts
- Webhook delivery failures
- Subscription already active
- Invalid edition selection

## Related Documentation
- [Models/Stripe/CLAUDE.md](../Stripe/CLAUDE.md): Stripe payment integration (alternative)
- [Models/Payment/CLAUDE.md](../Payment/CLAUDE.md): General payment models
- [Controllers/CLAUDE.md](../../Controllers/CLAUDE.md): Payment controllers
- [inzibackend.Core.Shared/MultiTenancy/Payments/CLAUDE.md](../../../../inzibackend.Core.Shared/MultiTenancy/Payments/CLAUDE.md): Payment domain models
- [Models/CLAUDE.md](../CLAUDE.md): Parent folder documentation