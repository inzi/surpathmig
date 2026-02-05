# MultiTenancy/Payments Documentation

## Overview
Comprehensive payment infrastructure for multi-tenant SaaS subscriptions, supporting multiple payment gateways, billing cycles, and subscription models.

## Contents

### Files

#### CreatePaymentResponse.cs
- **Purpose**: Abstract base class for payment creation responses
- **Key Functionality**:
  - Abstract GetId() method for payment identifier retrieval
  - Base class for gateway-specific implementations
- **Usage**: Standardizes payment creation responses across different gateways

#### ExecutePaymentResponse.cs
- **Purpose**: Abstract base class for payment execution responses
- **Key Functionality**:
  - Abstract GetId() method for transaction identifier
  - Base class for gateway-specific execution results
- **Usage**: Standardizes payment execution responses

#### PaymentGatewayModel.cs
- **Purpose**: Model describing payment gateway capabilities
- **Key Properties**:
  - GatewayType: Identifies the payment provider (PayPal, Stripe)
  - SupportsRecurringPayments: Indicates recurring payment support
- **Usage**: Gateway capability discovery and selection

#### PaymentPeriodType.cs
- **Purpose**: Billing cycle definitions for subscriptions
- **Enum Values**:
  - Daily (1): Daily billing cycle
  - Weekly (7): Weekly billing cycle
  - Monthly (30): Monthly billing cycle
  - Annual (365): Annual billing cycle
- **Note**: Values represent days in period for calculations

#### SubscriptionPaymentGatewayType.cs
- **Purpose**: Supported payment gateway providers
- **Enum Values**:
  - Paypal (1): PayPal payment gateway
  - Stripe (2): Stripe payment gateway
- **Usage**: Gateway selection and routing

#### SubscriptionPaymentStatus.cs
- **Purpose**: Payment transaction status tracking
- **Enum Values**:
  - NotPaid (1): Payment pending
  - Paid (2): Payment successful
  - Failed (3): Payment failed
  - Cancelled (4): Payment cancelled
  - Completed (5): Payment fully processed
- **Usage**: Transaction lifecycle tracking

#### SubscriptionPaymentType.cs
- **Purpose**: Subscription billing model types
- **Enum Values**:
  - Manual (0): One-time manual payments
  - RecurringAutomatic (1): Auto-renewed subscriptions
  - RecurringManual (2): Manual renewal required
- **Usage**: Determines billing behavior

#### SubscriptionPaymentTypeExtensions.cs
- **Purpose**: Extension methods for payment type operations
- **Methods**:
  - IsRecurring(): Returns true for recurring payment types
- **Logic**: Any non-Manual type is considered recurring

#### SubscriptionStartType.cs
- **Purpose**: Initial subscription tier types
- **Enum Values**:
  - Free (1): Free tier subscription
  - Trial (2): Trial period subscription
  - Paid (3): Paid subscription from start
- **Usage**: Determines initial billing behavior

### Key Components
- Abstract response classes for gateway abstraction
- Comprehensive payment status lifecycle
- Flexible billing period definitions
- Multiple payment gateway support
- Extension methods for type checking

### Dependencies
- No external dependencies
- Part of the inzibackend.MultiTenancy.Payments namespace

## Architecture Notes
- Gateway-agnostic design with abstract base classes
- Supports both one-time and recurring payment models
- Period values in days enable date calculations
- Extensible for additional payment gateways
- Clear separation between payment creation and execution

## Business Logic
- **Payment Flow**: Create → Execute → Track Status
- **Recurring Logic**: Automatic renewals vs manual renewals
- **Trial Support**: Free trials before paid subscriptions
- **Gateway Selection**: Based on capabilities and merchant preferences
- **Billing Cycles**: Flexible periods from daily to annual

## Usage Across Codebase
These payment components are used in:
- Subscription management services
- Payment gateway integrations
- Billing and invoice generation
- Trial to paid conversions
- Recurring payment processors
- Financial reporting
- Tenant subscription workflows
- Payment notification handlers