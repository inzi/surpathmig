# Payments DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for the subscription payment system. These DTOs handle payment processing, subscription management, payment history, and integration with multiple payment gateways (Stripe, PayPal, Authorize.Net). The payment system enables SaaS monetization through subscription billing.

## Contents

### Files

#### Payment Operations
- **CreatePaymentDto.cs** - Initiate payment:
  - SubscriptionPaymentGateway - Gateway to use (Stripe, PayPal, Authorize.Net)
  - EditionId - Edition being subscribed to
  - PaymentPeriodType - Billing cycle (Monthly, Annual)
  - SuccessUrl, ErrorUrl - Redirect URLs after payment
  - Creates payment session with gateway

- **CancelPaymentDto.cs** - Cancel ongoing payment:
  - Gateway - Which gateway to cancel with
  - PaymentId - Payment session to cancel

#### Payment Information
- **PaymentInfoDto.cs** - Payment record details:
  - Edition information
  - Amount and currency
  - Payment gateway
  - Payment status (Pending, Completed, Failed, Cancelled)
  - Invoice number
  - Payment date

- **SubscriptionPaymentDto.cs** - Subscription payment details:
  - Extends PaymentInfoDto
  - Subscription period (start, end dates)
  - Tenant information
  - Payment gateway details

- **SubscriptionPaymentListDto.cs** - Payment list item:
  - Simplified payment info for lists
  - Status, amount, date
  - Invoice download link

#### Gateway Integration
- **GetActiveGatewaysInput.cs** - Query configured gateways:
  - Returns list of enabled payment gateways
  - Used for payment method selection

- **StripePaymentResultInput.cs** - Stripe payment callback:
  - SessionId from Stripe
  - Payment status
  - Used for payment completion handling

#### Payment History
- **GetPaymentHistoryInput.cs** - Query payment history:
  - Date range filtering
  - Status filtering (All, Completed, Failed, Pending)
  - Paging and sorting support
  - Returns list of SubscriptionPaymentListDto

### Key Components

#### Payment Gateways
- **Stripe** - Credit card processing (primary)
- **PayPal** - PayPal account payments
- **Authorize.Net** - Credit card alternative
- Gateway-specific DTOs in subfolders

#### Payment Lifecycle
1. **Initiate**: CreatePaymentDto starts payment
2. **Gateway**: User redirected to payment gateway
3. **Complete**: Gateway calls back with result
4. **Record**: Payment recorded in system
5. **Subscription**: Tenant subscription activated

#### Subscription Types
- **Monthly**: Recurring monthly billing
- **Annual**: Yearly billing (typically discounted)
- **Trial**: Free trial period before billing
- **One-Time**: Non-recurring payments

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- **inzibackend.MultiTenancy.Payments.{Gateway}.Dto** - Gateway-specific DTOs
- **inzibackend.Dto** - Paging base DTOs

## Architecture Notes

### Multi-Gateway Support
- Abstract payment interface
- Gateway-specific implementations
- Fallback gateways for redundancy
- Per-tenant gateway configuration

### Security Considerations
- **PCI Compliance**: No credit card storage in system
- **Gateway Tokens**: Use gateway-provided tokens
- **HTTPS Only**: All payment operations over SSL
- **Webhook Validation**: Verify gateway callbacks

### Payment State Management
- Payments tracked through multiple states
- Idempotency for duplicate payment prevention
- Retry logic for failed payments
- Refund support for cancellations

### Webhook Handling
- Async payment completion via webhooks
- Verify webhook signatures
- Idempotent webhook processing
- Handle out-of-order webhooks

## Business Logic

### Payment Workflow
1. **Select Edition**: Tenant chooses pricing tier
2. **Create Payment**: System calls CreatePaymentDto with edition and period
3. **Gateway Redirect**: User sent to payment gateway (Stripe, PayPal, etc.)
4. **Payment Processing**: User enters payment details at gateway
5. **Callback**: Gateway calls back with StripePaymentResultInput (or equivalent)
6. **Subscription Activation**: System activates tenant subscription
7. **Invoice Generation**: System generates invoice
8. **Confirmation**: Tenant receives confirmation email

### Subscription Renewal
1. Gateway automatically charges renewal
2. Webhook notifies system of renewal payment
3. System extends subscription period
4. Invoice generated for renewal
5. Tenant notified of successful renewal

### Failed Payment Handling
1. Gateway attempts payment
2. Payment fails (insufficient funds, card expired, etc.)
3. Webhook notifies system of failure
4. System sends dunning email to tenant
5. Retries payment after configured interval
6. After X failures, subscription suspended

### Payment History
- Tenants view payment history via GetPaymentHistoryInput
- Download invoices for accounting
- Dispute resolution reference
- Tax records

## Usage Across Codebase
These DTOs are consumed by:
- **IPaymentAppService** - Payment operations
- **Tenant Subscription UI** - Edition purchase and upgrade
- **Payment Gateway Services** - Gateway integration
- **Webhook Controllers** - Payment completion handling
- **Billing Services** - Invoice generation
- **Email Services** - Payment confirmation emails

## Cross-Reference Impact
Changes to these DTOs affect:
- Subscription purchase flows
- Payment gateway integrations
- Payment history displays
- Invoice generation
- Webhook processing
- Subscription activation logic
- Payment retry mechanisms
- Dunning email workflows