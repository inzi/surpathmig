# Authorize.Net Payment DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) specific to Authorize.Net payment gateway integration. Authorize.Net provides credit card processing as an alternative to Stripe, commonly used in the United States. These DTOs handle the Authorize.Net-specific payment flow including configuration, payment input, and result processing.

## Contents

### Files

- **AuthorizeNetConfigurationDto.cs** - Gateway configuration:
  - ApiLoginId - Authorize.Net API login ID
  - TransactionKey - Authorize.Net transaction key
  - IsActive - Gateway enabled/disabled
  - Environment - Sandbox vs Production
  - Used by host to configure Authorize.Net integration

- **AuthorizeNetPaymentInput.cs** - Payment request to Authorize.Net:
  - Amount - Payment amount
  - Currency - Currency code
  - Description - Payment description
  - Customer information
  - Credit card tokenization data (not raw card numbers)

- **AuthorizeNetResultOutput.cs** - Payment result from Authorize.Net:
  - Success - Payment succeeded/failed
  - TransactionId - Authorize.Net transaction ID
  - AuthorizationCode - Authorization code from processor
  - ErrorMessage - Error details if failed
  - ResponseCode - Detailed response code

### Key Components

#### Authorize.Net Integration
- Accept.js for PCI-compliant card tokenization
- Transaction API for payment processing
- Webhook support for async notifications
- Stored payment methods for recurring billing

#### Configuration
- Sandbox mode for testing
- Production mode for live payments
- API credentials securely stored
- Per-tenant configuration option

### Dependencies
- **Authorize.Net SDK** - Official SDK for integration
- **inzibackend.MultiTenancy.Payments.Dto** - Base payment DTOs

## Architecture Notes

### PCI Compliance
- Card numbers never touch server
- Accept.js tokenizes cards client-side
- Server receives only token (opaqueData)
- Authorize.Net stores sensitive data

### Security
- API credentials encrypted at rest
- HTTPS for all API calls
- Webhook signature verification
- Transaction fraud detection

### Error Handling
- Declined card handling
- Insufficient funds errors
- Card expiration errors
- Fraud detection responses

## Business Logic

### Authorize.Net Payment Flow
1. User enters card details in browser
2. Accept.js sends card to Authorize.Net, receives token
3. Token sent to server in AuthorizeNetPaymentInput
4. Server calls Authorize.Net API with token
5. Authorize.Net processes payment
6. Returns AuthorizeNetResultOutput with result
7. Subscription activated on success

### Configuration
- Host admin configures AuthorizeNetConfigurationDto
- API credentials from Authorize.Net merchant account
- Test in sandbox, switch to production when ready

### Recurring Billing
- Customer payment profile stored at Authorize.Net
- Automatic recurring billing for subscriptions
- Update payment method support
- Cancel subscription support

## Usage Across Codebase
These DTOs are consumed by:
- **IAuthorizeNetPaymentAppService** - Authorize.Net operations
- **Payment Gateway Services** - Payment processing
- **Configuration UI** - Gateway setup
- **Webhook Controllers** - Payment notifications
- **Subscription Services** - Recurring billing

## Cross-Reference Impact
Changes affect:
- Authorize.Net payment processing
- Gateway configuration interface
- Payment form integration
- Recurring billing setup
- Webhook processing