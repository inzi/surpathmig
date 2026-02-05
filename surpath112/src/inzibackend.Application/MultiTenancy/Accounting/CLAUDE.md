# Multi-Tenancy Accounting Services Documentation

## Overview
Financial accounting services for subscription and payment tracking. Generates invoices, tracks payments, manages subscription accounting, and provides financial reporting for the SaaS platform.

## Contents
- Invoice generation from subscription payments
- Payment recording and reconciliation
- Financial reporting per tenant
- Revenue recognition tracking
- Tax calculation (if applicable)

### Subfolders
- **Dto**: Invoice DTOs (InvoiceDto, CreateInvoiceDto) for data transfer

## Key Features
- Automated invoice generation on payment
- PDF invoice creation
- Payment history tracking
- Revenue reporting
- Integration with payment services

## Business Logic
Creates invoices when subscription payments are successful, tracks all financial transactions, and provides reporting for host administrators to monitor platform revenue.

## Usage
Invoked by payment services after successful transactions, and by reporting/admin interfaces for financial oversight.