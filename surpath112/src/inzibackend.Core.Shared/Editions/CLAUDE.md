# Editions Documentation

## Overview
Defines payment types for the multi-tenant SaaS edition system, categorizing different payment scenarios for tenant subscriptions.

## Contents

### Files

#### EditionPaymentType.cs
- **Purpose**: Enumeration of payment scenarios for edition subscriptions
- **Enum Values**:
  - NewRegistration (0): Initial payment during tenant registration
  - BuyNow (1): Converting from trial to paid subscription
  - Upgrade (2): Moving to a higher-tier edition
  - Extend (3): Renewing current edition without changes
- **Usage**: Categorizes payment transactions for billing and subscription management

### Key Components
- Comprehensive XML documentation for each payment type
- Covers full subscription lifecycle scenarios
- Explicit integer values for database storage

### Dependencies
- No external dependencies
- Part of the inzibackend.Editions namespace

## Architecture Notes
- Supports flexible subscription models
- Clear distinction between different payment contexts
- Enables targeted pricing strategies per payment type
- Zero-based enumeration for compatibility

## Business Logic
- **NewRegistration**: Applied when new tenants sign up for paid editions
- **BuyNow**: Used when trial users convert to paying customers
- **Upgrade**: Enables tiered pricing with edition progression
- **Extend**: Supports subscription renewals at same tier

## Usage Across Codebase
This enum is used in:
- Payment processing services
- Subscription management workflows
- Billing and invoice generation
- Edition upgrade/downgrade logic
- Trial conversion tracking
- Financial reporting and analytics
- Tenant lifecycle management