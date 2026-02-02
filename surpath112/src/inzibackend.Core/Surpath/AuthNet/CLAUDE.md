# AuthNet Documentation

## Overview
Authorize.Net payment gateway integration manager for processing credit card transactions in the Surpath system. Handles both sandbox and production payment processing with secure tokenization.

## Contents

### Files

#### AuthNetManager.cs
- **Purpose**: Domain service for Authorize.Net payment processing
- **Key Functionality**:
  - Credit card charge processing using secure tokens
  - Support for both sandbox and production environments
  - Integration with SurpathPayManager for ledger tracking
  - Secure handling of payment credentials
- **Configuration**:
  - ApiLoginID: Merchant account identifier
  - ApiTransactionKey: Authentication key
  - PublicClientKey: Client-side tokenization key
  - UseSandbox: Environment toggle
  - IsActive: Feature toggle

### Key Components

- **AuthNetManager**: Main service for payment gateway operations
- **ChargeCreditCardRequest**: Primary method for processing payments

### Dependencies

- **External Libraries**:
  - AuthorizeNet.Api (Payment gateway SDK)
  - Newtonsoft.Json (JSON processing)
  - Microsoft.Extensions.Configuration

- **Internal Dependencies**:
  - UserManager (User context)
  - SurpathPayManager (Ledger integration)
  - IAppConfigurationAccessor (Configuration access)

## Architecture Notes

- **Pattern**: Domain Service with configuration injection
- **Security**: Uses opaque data tokens (no raw card data)
- **Environment Support**: Configurable sandbox/production modes
- **Logging**: Debug logging for transaction tracking

## Business Logic

### Payment Processing Flow
1. Receive tokenized payment data (opaque data)
2. Create charge request with user and amount
3. Process through Authorize.Net API
4. Update ledger through SurpathPayManager
5. Return success/failure status

### Security Features
- Never handles raw credit card numbers
- Uses secure tokens (dataDescriptor/dataValue)
- Configuration-based credentials
- Environment-specific endpoints

## Usage Across Codebase

This service is used for:
- Processing payments for drug tests
- Processing payments for background checks
- Donor-pay transactions
- Any credit card payment processing

## Configuration Requirements

```json
"AuthorizeNet": {
  "IsActive": "true",
  "UseSandbox": "false",
  "ApiLoginID": "YOUR_API_LOGIN",
  "ApiTransactionKey": "YOUR_TRANSACTION_KEY",
  "PublicClientKey": "YOUR_PUBLIC_KEY"
}
```

## Security Considerations

- Credentials stored in configuration, not code
- PCI compliance through tokenization
- Separate sandbox/production credentials
- Transaction logging for audit trail