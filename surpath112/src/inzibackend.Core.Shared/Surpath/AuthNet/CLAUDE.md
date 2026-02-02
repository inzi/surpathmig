# Surpath/AuthNet Documentation

## Overview
Authorize.Net payment gateway integration models for processing credit card transactions and managing payment data for Surpath services.

## Contents

### Files

#### AuthNetSubmit.cs
- **Purpose**: DTOs and models for Authorize.Net payment processing
- **Key Classes**:
  - **AuthNetCaptureResultDto**: Captures transaction results
    - TransactionId: Unique transaction identifier
    - AuthCode: Authorization code from processor
  - **AuthNetPreAuthDto**: Pre-authorization request model
    - authNetSubmit: Payment submission details
    - amount: Transaction amount
  - **AuthNetSubmit**: Main payment submission model
    - Payment token data (dataValue, dataDescriptor)
    - Cardholder information
    - Billing address details
    - Service items being purchased
    - Transaction results
  - **AuthNetItems**: Line items for transactions
    - Product details (name, price, quantity)
    - Calculated totals and amounts paid

### Key Components
- Tokenized payment processing (dataValue/dataDescriptor)
- Support for multiple service items per transaction
- Billing address management
- Transaction tracking and authorization codes
- Pre-authorization and capture workflow

### Dependencies
- Abp.Application.Services.Dto: For DTO base classes
- System.Collections.Generic: For list operations
- Part of the inzibackend.Surpath namespace

## Architecture Notes
- Uses Authorize.Net's Accept.js tokenization for PCI compliance
- Stores only tokenized card data, not raw card numbers
- Supports itemized transactions for detailed reporting
- Separate billing address option for flexibility
- Transaction state tracking through IDs and auth codes

## Business Logic
- **Payment Flow**: Tokenize → Pre-authorize → Capture
- **Service Association**: Links payments to TenantSurpathServiceIds
- **Security**: No raw credit card data stored (only last four digits)
- **Itemization**: Detailed line items for accounting and receipts
- **Address Handling**: Supports different billing addresses

## Usage Across Codebase
These payment models are used in:
- Payment processing services
- Shopping cart checkout flows
- Service purchase workflows
- Payment history tracking
- Financial reporting
- Receipt generation
- Refund processing
- Payment gateway integration layer