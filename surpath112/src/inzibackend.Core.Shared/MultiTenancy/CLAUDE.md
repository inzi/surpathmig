# MultiTenancy Documentation

## Overview
Core multi-tenancy infrastructure providing tenant configuration constants and comprehensive payment/subscription management for the SaaS platform.

## Contents

### Files

#### TenantConsts.cs
- **Purpose**: Constants for tenant validation and configuration
- **Key Constants**:
  - TenancyNameRegex: Pattern for valid tenant names (alphanumeric with underscore/hyphen)
  - DefaultTenantName: "Default" - name for the default tenant
  - MaxNameLength: 128 characters maximum for tenant names
  - DefaultTenantId: 1 - ID of the default tenant
- **Validation Rules**: Names must start with a letter, followed by letters, numbers, underscores, or hyphens

### Key Components
- Tenant naming validation patterns
- Default tenant configuration
- Comprehensive payment system infrastructure
- Subscription management models

### Dependencies
- No external dependencies at this level
- Part of the inzibackend.MultiTenancy namespace

## Subfolders

### Payments
Comprehensive payment and subscription management system:
- **Payment Responses**: Abstract base classes for payment operations
- **Gateway Support**: PayPal and Stripe integration models
- **Billing Cycles**: Daily, Weekly, Monthly, Annual periods
- **Payment Types**: Manual, Recurring Automatic, Recurring Manual
- **Subscription States**: NotPaid, Paid, Failed, Cancelled, Completed
- **Subscription Tiers**: Free, Trial, Paid
- **Gateway Capabilities**: Model for gateway feature discovery

## Architecture Notes
- Multi-tenant isolation at the core level
- Regex-based validation for tenant names
- Default tenant (ID: 1) for system operations
- Extensible payment gateway architecture
- Clear separation of payment creation and execution
- Support for complex subscription scenarios

## Business Logic
- **Tenant Names**: Must be URL-friendly (no spaces or special characters)
- **Default Tenant**: Used for host-level operations
- **Payment Flow**: Create → Execute → Track lifecycle
- **Subscription Models**: Supports trials, upgrades, and renewals
- **Recurring Payments**: Both automatic and manual renewal options

## Usage Across Codebase
Multi-tenancy infrastructure is used in:
- Tenant registration and provisioning
- URL routing and subdomain handling
- Data isolation and filtering
- Subscription management workflows
- Payment processing pipelines
- Billing and invoicing systems
- Financial reporting per tenant
- Trial to paid conversion flows
- License management