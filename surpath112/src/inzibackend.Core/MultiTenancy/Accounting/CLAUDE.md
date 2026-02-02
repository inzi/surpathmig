# Accounting Documentation

## Overview
Invoice generation and management system for multi-tenant subscription billing. Provides invoice tracking, number generation, and tenant billing information storage.

## Contents

### Files

#### Invoice.cs
- **Purpose**: Entity representing an invoice for subscription payments
- **Table**: AppInvoices
- **Key Properties**:
  - `InvoiceNo`: Unique invoice number
  - `InvoiceDate`: Invoice generation date
  - `TenantLegalName`: Tenant's legal business name
  - `TenantAddress`: Billing address
  - `TenantTaxNo`: Tax identification number
- **Pattern**: Simple entity for invoice records

#### DefaultInvoiceNumberGenerator.cs
- **Purpose**: Service to generate unique invoice numbers
- **Key Features**: (inferred)
  - Sequential number generation
  - Tenant-specific numbering
  - Configurable format
  - Thread-safe generation

#### IInvoiceNumberGenerator.cs
- **Purpose**: Interface for invoice number generation
- **Pattern**: Strategy pattern for customizable numbering schemes

### Key Components

- **Invoice**: Domain entity for invoices
- **IInvoiceNumberGenerator**: Generation strategy interface
- **DefaultInvoiceNumberGenerator**: Default implementation

### Dependencies

- **External Libraries**:
  - ABP Framework (domain entities)
  - Entity Framework Core

- **Internal Dependencies**:
  - Multi-tenancy system
  - Subscription payment system
  - Tenant management

## Architecture Notes

- **Pattern**: Domain model with strategy pattern for generation
- **Storage**: Database persistence via EF Core
- **Extensibility**: Pluggable number generation strategies

## Business Logic

### Invoice Generation Flow
1. Subscription payment completed
2. Generate unique invoice number
3. Create invoice record with tenant details
4. Store invoice in database
5. Optional: Send invoice to tenant via email
6. Link invoice to payment record

### Invoice Number Generation
- Default format: Sequential with prefix
- Tenant-specific or global numbering
- Ensures uniqueness
- Thread-safe concurrent generation

### Invoice Information
- Captures tenant legal details at time of invoice
- Stores for historical accuracy
- Used for tax reporting
- Required for compliance

## Usage Across Codebase

Used by:
- Subscription payment processing
- Payment history display
- Tenant billing page
- Financial reporting
- Tax documentation
- Invoice download/printing

## Security Considerations

- Host-side only (tenant isolation)
- Invoice numbers must be unique and sequential
- Tenant information captured securely
- Access control on invoice viewing
- Audit trail for invoice generation

## Extension Points

- Custom invoice number formats
- Additional invoice fields (line items, taxes)
- PDF invoice generation
- Multi-currency support
- Invoice templates
- Automated invoice delivery