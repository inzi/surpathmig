# PayPal Payment DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for PayPal payment gateway integration. PayPal enables users to pay with their PayPal account balance, linked bank account, or credit/debit card through PayPal's interface. This provides an alternative payment method for users who prefer PayPal over direct credit card processing.

## Contents

### Files

- **PayPalConfigurationDto.cs** - PayPal gateway configuration:
  - ClientId - PayPal REST API client ID
  - ClientSecret - PayPal REST API secret
  - IsActive - Gateway enabled/disabled
  - Environment - Sandbox vs Live
  - DemoUsername, DemoPassword - Test credentials for sandbox
  - Used by host to configure PayPal integration

### Key Components

#### PayPal Integration
- PayPal REST API for payment processing
- PayPal Checkout for user payment flow
- Instant Payment Notification (IPN) for webhooks
- Reference transactions for subscriptions

#### Configuration
- Sandbox mode for testing with demo accounts
- Production mode for live payments
- REST API credentials from PayPal developer portal
- IPN URL configuration for callbacks

### Dependencies
- **PayPal SDK** - Official SDK or REST API client
- **inzibackend.MultiTenancy.Payments.Dto** - Base payment DTOs

## Architecture Notes

### PayPal Checkout Flow
- Modal/redirect to PayPal for payment
- User logs into PayPal account
- Authorizes payment
- Redirected back to application
- IPN confirms payment completion

### Security
- OAuth 2.0 for API authentication
- API credentials encrypted
- HTTPS for all communications
- IPN signature verification

### User Experience
- Recognizable PayPal branding
- One-Click with PayPal account
- No card entry required
- Trust factor for users

## Business Logic

### PayPal Payment Workflow
1. User selects PayPal payment method
2. System creates PayPal payment session
3. User redirected to PayPal
4. User logs in and approves payment
5. PayPal redirects back with token
6. System completes payment with token
7. IPN confirms payment asynchronously
8. Subscription activated

### Recurring Billing
- Billing agreements for subscriptions
- Automatic monthly/annual charges
- User can cancel in PayPal account
- System handles billing agreement callbacks

### Sandbox Testing
- Test with PayPal sandbox accounts
- DemoUsername/DemoPassword for quick testing
- Full payment flow without real money
- Switch to production when ready

## Usage Across Codebase
These DTOs are consumed by:
- **IPayPalPaymentAppService** - PayPal operations
- **Payment Gateway Services** - Payment processing
- **Configuration UI** - Gateway setup
- **IPN Controllers** - Payment notifications
- **Subscription Services** - Recurring billing

## Cross-Reference Impact
Changes affect:
- PayPal payment processing
- Gateway configuration interface
- PayPal button integration
- IPN webhook processing
- Recurring billing with PayPal