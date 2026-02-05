# Accounting DTOs Documentation

## Overview
Data Transfer Objects for invoice generation and management within the multi-tenant accounting system. These DTOs support the creation and display of invoices for tenant subscription payments.

## Contents

### Files

#### InvoiceDto.cs
- **Purpose**: Complete invoice representation for display and reporting
- **Key Properties**:
  - `Amount`: Invoice total amount
  - `EditionDisplayName`: Name of the subscribed edition
  - `InvoiceNo`: Unique invoice number
  - `InvoiceDate`: Date of invoice generation
  - `TenantLegalName`: Tenant's legal business name
  - `TenantAddress`: Multi-line tenant address
  - `TenantTaxNo`: Tenant's tax identification number
  - `HostLegalName`: SaaS host/provider legal name
  - `HostAddress`: Multi-line host address
- **Usage**: Displayed in invoice generation, PDF reports, and accounting records

#### CreateInvoiceDto.cs
- **Purpose**: Input DTO for invoice creation requests
- **Key Properties**:
  - `SubscriptionPaymentId`: Links invoice to a specific subscription payment transaction
- **Usage**: Passed to invoice creation service methods

### Key Components
**Invoice Generation Models:**
- Complete invoice details with billing and provider information
- Simple creation request model linked to subscription payments

### Dependencies
- **Framework**: Standard .NET types (System.Collections.Generic)
- **Internal**: None (pure DTO classes)

## Architecture Notes
- **Pattern**: Simple POCO DTOs for data transfer
- **Design**: Separation between input (CreateInvoiceDto) and output (InvoiceDto) models
- **Multi-line Addresses**: Uses `List<string>` for flexible address formatting
- **Immutable Data**: No business logic, purely data containers

## Business Logic
### Invoice Creation Flow
1. Client provides `SubscriptionPaymentId` via `CreateInvoiceDto`
2. Service retrieves payment and tenant details
3. Service generates invoice with complete billing information
4. Returns populated `InvoiceDto` with all required fields

### Invoice Data Requirements
- Both tenant and host legal details required
- Tax numbers included for compliance
- Edition information for clarity on what's being billed

## Usage Across Codebase
### Primary Consumers
- `IInvoiceAppService`: Invoice generation service
- Invoice PDF generation components
- Accounting reports and dashboards
- Subscription payment processing

### Data Flow
- Created from subscription payment records
- Displayed in admin and tenant dashboards
- Used in PDF generation for printable invoices
- Stored or referenced in accounting systems