# SurpathPayManager Documentation

## Overview
Domain service for managing payment-related operations in the Surpath system, including ledger entries, payment processing through Authorize.Net, and financial tracking for compliance services.

## Contents

### Files

#### SurpathPayManager.cs
- **Purpose**: Central payment management service for Surpath services
- **Key Functionality**:
  - Ledger entry management
  - Payment transaction processing
  - Integration with Authorize.Net payment gateway
  - Financial record keeping for compliance services
- **Dependencies**:
  - Multiple entity repositories for comprehensive payment tracking
  - Authorize.Net API for payment processing
  - Feature checking for service availability

### Key Components

- **SurpathPayManager**: Main domain service for payment operations

### Dependencies

- **External Libraries**:
  - AuthorizeNet.Api (Payment gateway integration)
  - Newtonsoft.Json (JSON serialization)
  - ABP Framework (Domain services, repositories, features)

- **Internal Dependencies**:
  - Multiple Surpath entities (Cohort, CohortUser, RecordState, etc.)
  - Ledger entities (LedgerEntry, LedgerEntryDetail)
  - User and tenant repositories
  - Feature checking services

## Architecture Notes

- **Pattern**: Domain Service pattern with repository injection
- **Payment Gateway**: Authorize.Net integration
- **Transaction Support**: Uses System.Transactions for ACID compliance
- **Multi-tenancy**: Full tenant isolation for payments

## Business Logic

### Payment Processing Flow
1. Create ledger entries for transactions
2. Process payments through Authorize.Net
3. Update ledger with transaction results
4. Track payment details for reporting

### Financial Tracking
- Ledger entries for audit trail
- Detailed transaction records
- Service-specific payment tracking
- User purchase history

## Usage Across Codebase

This service is used for:
- Processing payments for drug tests and background checks
- Managing donor-pay vs institution-pay scenarios
- Creating financial reports
- Tracking payment history
- Reconciling transactions

## Security Considerations

- Payment data isolation by tenant
- Secure payment gateway integration
- Transaction logging for audit
- Feature-based access control