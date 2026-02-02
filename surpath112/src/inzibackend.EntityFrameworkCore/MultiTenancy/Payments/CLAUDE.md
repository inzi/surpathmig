# Payments Documentation

## Overview
Custom repository implementations for subscription payment management in the multi-tenant SaaS application. Provides specialized data access for payment processing, subscription tracking, and payment extension data storage.

## Contents

### Files

#### SubscriptionPaymentRepository.cs
- **Purpose**: Custom repository for subscription payment entities
- **Key Functionality**:
  - Retrieves payments by gateway and payment ID
  - Finds last completed or any last payment for a tenant
  - Supports filtering by gateway type and recurring status
  - Implements `ISubscriptionPaymentRepository` interface

#### SubscriptionPaymentExtensionDataRepository.cs
- **Purpose**: Repository for storing additional payment metadata
- **Key Functionality**:
  - Manages key-value extension data for payments
  - Supports soft delete through query filters
  - Implements `ISubscriptionPaymentExtensionDataRepository`

### Key Components

**SubscriptionPaymentRepository Methods**

`GetByGatewayAndPaymentIdAsync(gateway, paymentId)`
- Finds unique payment by external payment ID and gateway
- Used for payment verification and duplicate prevention

`GetLastCompletedPaymentOrDefaultAsync(tenantId, gateway?, isRecurring?)`
- Retrieves most recent successful payment for a tenant
- Optional filtering by gateway and recurring status
- Orders by ID descending for latest payment

`GetLastPaymentOrDefaultAsync(tenantId, gateway?, isRecurring?)`
- Similar to above but includes all payment statuses
- Used for payment history and retry scenarios

**SubscriptionPaymentExtensionDataRepository**
- Base repository implementation without custom methods
- Relies on built-in query filters for soft delete
- Stores arbitrary payment metadata as key-value pairs

### Dependencies
- Abp.EntityFrameworkCore
- Abp.Linq.Extensions (for WhereIf conditional filtering)
- Microsoft.EntityFrameworkCore
- Base repository infrastructure

## Architecture Notes

**Query Patterns**
- Async/await throughout for scalability
- Conditional filtering using WhereIf extension
- Explicit ToListAsync() before ordering (performance consideration)

**Multi-Tenancy**
- Tenant-specific payment isolation
- Payment history per tenant
- Supports SaaS billing scenarios

**Data Model**
- SubscriptionPayment: Core payment record
- SubscriptionPaymentExtensionData: Flexible metadata storage
- Supports multiple payment gateways

## Business Logic

**Payment Lookup Strategy**
1. External payment ID + Gateway = unique identifier
2. Most recent payments determined by ID (not date)
3. Completed vs all payments for different use cases

**Extension Data Pattern**
- Key-value pairs for gateway-specific data
- Soft delete support for data retention
- Flexible schema for various payment providers

## Usage Across Codebase

**Primary Consumers**
- Payment processing services
- Subscription management features
- Billing and invoice generation
- Payment gateway integrations

**Integration Scenarios**
- Webhook handlers for payment gateways
- Subscription renewal processes
- Payment history displays
- Retry logic for failed payments

**Related Entities**
- Tenant: Multi-tenant isolation
- SubscriptionPayment: Payment records
- SubscriptionPaymentExtensionData: Payment metadata
- Invoice: Related billing documents

**Payment Gateways Supported**
- Multiple gateway types via enum
- Gateway-specific data in extension table
- Unified interface regardless of gateway