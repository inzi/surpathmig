# Purchase Application Service Documentation

## Overview
Comprehensive application service managing the purchase and payment processing for compliance services. Integrates with Authorize.Net for payment processing, manages ledger entries for financial tracking, and coordinates with compliance evaluation to grant services upon successful payment.

## Contents

### Files

#### PurchaseAppService.cs
- **Purpose**: Complete purchase and payment processing service
- **Authorization**: `[AbpAuthorize]` - Requires authentication
- **Key Features**:
  - Payment processing via Authorize.Net
  - Service purchase for cohort users
  - Ledger entry creation and management
  - Transaction tracking and de-duplication
  - Hierarchical pricing support
  - Role-based pricing (e.g., student vs. professional)
  - Integration with compliance evaluation
  - Multi-tenant support
- **Major Dependencies** (20+ injected services):
  - `IRepository<CohortUser>`: Student/user management
  - `IRepository<LedgerEntry>`: Financial transaction records
  - `IRepository<TenantSurpathService>`: Available services
  - `SurpathPayManager`: Payment processing coordination
  - `AuthNetManager`: Authorize.Net integration
  - `ISurpathComplianceEvaluator`: Post-purchase compliance update
  - `HierarchicalPricingManager`: Dynamic pricing calculation
  - `IRepository<ProcessedTransactions>`: Duplicate transaction prevention
  - Multiple user, role, and tenant repositories

### Key Components

**Purchase Workflow:**
1. User selects service to purchase
2. Service price calculated based on user role and tenant
3. Payment processed through Authorize.Net
4. Ledger entries created for financial tracking
5. Service granted to cohort user
6. Compliance status recalculated
7. Transaction recorded to prevent duplicates

**Payment Processing:**
- Credit card processing via Authorize.Net API
- Payment profile management
- Transaction authorization and capture
- Refund processing
- Failed transaction handling

**Financial Tracking:**
- `LedgerEntry`: Top-level transaction record
- `LedgerEntryDetail`: Line items for each purchased service
- Double-entry bookkeeping principles
- Tenant-level financial reporting

**Hierarchical Pricing:**
- Prices configured at multiple levels (host, tenant, department)
- Role-based pricing (students, professionals, etc.)
- Fallback to higher-level pricing if not configured
- Volume/bulk discount support (implied)

### Dependencies
- **External**:
  - ABP Framework (Authorization, Repositories, UnitOfWork)
  - AuthorizeNet.Api (payment gateway)
  - Entity Framework Core
  - System.Transactions (distributed transactions)
  - Newtonsoft.Json (data serialization)
- **Internal**:
  - `SurpathPayManager`: Payment orchestration (Core layer)
  - `AuthNetManager`: Authorize.Net wrapper (Core layer)
  - `HierarchicalPricingManager`: Pricing engine (Core layer)
  - `SurpathManager`: General Surpath operations
  - `ISurpathComplianceEvaluator`: Compliance recalculation
  - Surpath domain entities (CohortUser, LedgerEntry, TenantSurpathService, etc.)

## Architecture Notes
- **Pattern**: Application Service coordinating multiple domain services
- **Payment Gateway**: Authorize.Net integration for PCI compliance
- **Transaction Safety**: Unit of Work with transaction scope
- **Idempotency**: ProcessedTransactions table prevents duplicate charges
- **Pricing Strategy**: Hierarchical with role-based overrides

## Business Logic

### Service Purchase Flow
1. **User Selection**: User or admin selects service for cohort user
2. **Price Calculation**: Hierarchical pricing manager determines price
3. **Payment Collection**: Process payment via Authorize.Net
4. **Ledger Creation**: Create LedgerEntry with LedgerEntryDetail items
5. **Service Grant**: Associate service with cohort user
6. **Compliance Update**: Trigger compliance recalculation
7. **Notification**: Notify user of successful purchase (implied)

### Payment Processing Steps
1. **Validation**: Verify service availability and pricing
2. **Authorization**: Authorize payment with Authorize.Net
3. **Transaction Check**: Verify not already processed (de-duplication)
4. **Capture**: Capture authorized payment
5. **Recording**: Save transaction details
6. **Failure Handling**: Void authorization on subsequent failure

### Ledger Entry Structure
- **LedgerEntry**: Header record with total amount, date, user
- **LedgerEntryDetail**: One per service purchased with quantity and unit price
- **Tenant Scoped**: All entries tied to tenant
- **Audit Trail**: Immutable financial record

### Hierarchical Pricing Resolution
1. Check for user-specific pricing (e.g., student role)
2. Check department-level pricing
3. Check tenant-level pricing
4. Fall back to host default pricing
5. Apply any active discounts or promotions

## Usage Across Codebase

### Primary Consumers
- **Student Portal**: Users purchase required services (background checks, drug tests)
- **Admin Interface**: Staff process purchases on behalf of users
- **Compliance Dashboard**: Display service availability and purchase options
- **Billing Reports**: Financial reporting and reconciliation

### Related Systems
- **Authorize.Net**: Payment gateway for card processing
- **Compliance System**: Grants compliance upon service purchase
- **Notification System**: Email receipts and confirmations
- **Reporting**: Financial and compliance reports
- **User Management**: Cohort user service associations

## Security Considerations
- **PCI Compliance**: Never store credit card numbers (use Authorize.Net tokens)
- **Authorization**: All purchase operations require authentication
- **Tenant Isolation**: Users can only purchase for their tenant
- **Transaction Verification**: Prevent duplicate charges
- **Audit Logging**: All transactions logged for compliance
- **Role-based Pricing**: Prevent price tampering

## Financial Compliance
- **Ledger Integrity**: Immutable financial records
- **Transaction Tracking**: Every payment recorded with unique ID
- **Reconciliation**: Ledger entries match payment gateway transactions
- **Refund Support**: Ability to process refunds and voids
- **Tax Handling**: May include tax calculation (implementation-specific)

## Error Handling
- **Payment Failures**: Transaction rolled back, user notified
- **Network Issues**: Retry logic or manual processing queue
- **Duplicate Prevention**: ProcessedTransactions table prevents re-charging
- **Void on Error**: Authorization voided if subsequent steps fail

## Performance Considerations
- **Transaction Scope**: Keep database transactions short
- **Async Processing**: Payment processing can be async for large volumes
- **Caching**: Service and pricing data cached
- **Bulk Purchases**: Support multiple services in single transaction

## Integration Points
### Authorize.Net
- Customer profiles
- Payment profiles
- Transaction authorization and capture
- Recurring billing (if applicable)
- Reporting and reconciliation

### Compliance System
- Service grants trigger compliance recalculation
- Purchased services immediately affect compliance status
- Background jobs may verify service completion

## Best Practices
- **Always Use Transactions**: Wrap purchase operations in Unit of Work
- **Validate Before Charging**: Check service availability before payment
- **Store Transaction IDs**: Keep Authorize.Net transaction IDs for reference
- **Send Receipts**: Email receipt to user with transaction details
- **Monitor Failed Payments**: Alert admins to payment issues
- **Reconcile Daily**: Match ledger entries to gateway transactions

## Extension Points
- Add subscription/recurring payment support
- Implement promotional codes and discounts
- Add payment plans for expensive services
- Support multiple payment methods (ACH, PayPal, etc.)
- Integrate with accounting systems (QuickBooks, etc.)
- Add refund request workflow
- Implement chargebackhandling