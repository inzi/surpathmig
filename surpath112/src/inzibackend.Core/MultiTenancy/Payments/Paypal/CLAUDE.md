# PayPal Payment Gateway Documentation

## Overview
PayPal payment gateway integration for one-time subscription payments. Supports both sandbox and live environments with checkout order capture functionality.

## Contents

### Files

#### PayPalGatewayManager.cs
- **Purpose**: Manager service for PayPal payment processing
- **Key Features**:
  - Environment selection (sandbox/live)
  - Order capture and verification
  - Payment status validation
  - User-friendly error handling
- **Dependencies**: PayPalCheckoutSdk
- **Pattern**: Service base with transient lifetime

#### PayPalExecutePaymentRequestInput.cs
- **Purpose**: Input DTO for payment execution requests
- **Properties**: Contains OrderId for capturing completed PayPal orders

#### PayPalConfiguration.cs
- **Purpose**: Configuration for PayPal payment gateway
- **Key Features**:
  - Implements `IPaymentGatewayConfiguration` interface
  - Environment selection (sandbox/live)
  - Client credentials management
  - Demo account configuration
  - Funding options control
  - Does not support recurring payments
- **Configuration Keys**:
  - `Payment:PayPal:Environment` (sandbox/live)
  - `Payment:PayPal:ClientId`
  - `Payment:PayPal:ClientSecret`
  - `Payment:PayPal:DemoUsername`
  - `Payment:PayPal:DemoPassword`
  - `Payment:PayPal:IsActive`
  - `Payment:PayPal:DisabledFundings` (array)

### Key Components

- **PayPalGatewayManager**: Core payment processing service
- **PayPalPaymentGatewayConfiguration**: Configuration provider
- **PayPalExecutePaymentRequestInput**: Request model

### Dependencies

- **External Libraries**:
  - PayPalCheckoutSdk.Core (PayPal SDK)
  - ABP Framework (dependency injection, error handling)

- **Internal Dependencies**:
  - `inzibackendServiceBase` (base service class)
  - `IPaymentGatewayConfiguration` (configuration interface)
  - `IAppConfigurationAccessor` (configuration accessor)

## Architecture Notes

- **Pattern**: Manager pattern for payment operations
- **Status**: Fully implemented and production-ready
- **Integration**: Multi-tenant subscription payments (one-time only)
- **Lifecycle**: Transient dependency injection
- **Environments**: Supports both sandbox and production

## Business Logic

### Payment Flow
1. Frontend creates PayPal order via PayPal SDK
2. User completes payment in PayPal interface
3. Backend receives OrderId
4. `CaptureOrderAsync` captures and verifies payment
5. Payment status validated (must be "COMPLETED")
6. Returns PayPal payment ID for record-keeping

### Environment Selection
- **Sandbox**: For testing with test credentials
- **Live**: For production payments with real money
- Automatically selects environment based on configuration

### Error Handling
- Throws `UserFriendlyException` with localized message on payment failure
- Validates payment status before returning success

## Usage Across Codebase

Used by:
- Multi-tenant subscription payment controllers
- Payment processing workflows
- Subscription purchase flows

**Note**: PayPal does not support recurring/automatic payments in this implementation. Used for one-time payments only.

## Configuration

### Required Settings
```json
{
  "Payment": {
    "PayPal": {
      "Environment": "sandbox",
      "ClientId": "your-client-id",
      "ClientSecret": "your-client-secret",
      "IsActive": true,
      "DemoUsername": "demo@example.com",
      "DemoPassword": "demo-password",
      "DisabledFundings": ["credit", "card"]
    }
  }
}
```

### Disabled Fundings
Array of funding sources to disable in PayPal checkout (e.g., credit cards, specific payment methods)

## Security Considerations

- Client credentials stored in configuration (secure storage recommended)
- Payment verification on server-side
- Order capture requires valid OrderId
- Status validation prevents fraudulent completions
- Environment separation for testing vs production
- No sensitive data in DTO models