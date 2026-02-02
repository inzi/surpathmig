# Surpath Payment and Pricing System Documentation

## Overview

The Surpath payment and pricing system is a comprehensive financial management platform that handles service pricing, payment processing, and financial tracking for educational institutions. This system supports hierarchical pricing structures, multiple payment methods, and detailed financial reporting.

## Core Architecture

### 1. Pricing Hierarchy

The system implements a sophisticated pricing hierarchy that allows for flexible cost structures:

```
System-Wide Services (Host Level)
├── Tenant-Level Services
│   ├── Department-Level Services
│   │   ├── Cohort-Level Services
│   │   │   └── Individual User Services
│   │   └── Organization Unit Services
│   └── Direct User Services
```

**Priority Order (Highest to Lowest):**
1. **Individual User Services** (`TenantSurpathService.UserId`)
2. **Cohort-Level Services** (`TenantSurpathService.CohortId`)
3. **Department-Level Services** (`TenantSurpathService.TenantDepartmentId`)
4. **Organization Unit Services** (`TenantSurpathService.OrganizationUnitId`)
5. **Tenant-Level Services** (no specific assignment)

### 2. Core Entities

#### SurpathService (System-Wide Template)
```csharp
public class SurpathService : FullAuditedEntity<Guid>, IMayHaveTenant
{
    public string Name { get; set; }                    // Service name
    public double Price { get; set; }                   // Base price
    public decimal Discount { get; set; }               // Default discount
    public string Description { get; set; }             // Service description
    public bool IsEnabledByDefault { get; set; }        // Auto-enable for tenants
    public string FeatureIdentifier { get; set; }       // Feature flag identifier
    
    // Optional scope assignments (for templates)
    public Guid? TenantDepartmentId { get; set; }
    public Guid? CohortId { get; set; }
    public long? UserId { get; set; }
    public Guid? RecordCategoryRuleId { get; set; }
}
```

#### TenantSurpathService (Tenant-Specific Implementation)
```csharp
public class TenantSurpathService : FullAuditedEntity<Guid>, IMayHaveTenant
{
    public int? TenantId { get; set; }                  // Tenant scope
    public string Name { get; set; }                    // Tenant-specific name
    public double Price { get; set; }                   // Tenant-specific price
    public string Description { get; set; }             // Tenant-specific description
    public bool IsEnabled { get; set; }                 // Enable/disable for tenant
    
    // Service template reference
    public Guid? SurpathServiceId { get; set; }
    
    // Hierarchical assignment (mutually exclusive)
    public Guid? TenantDepartmentId { get; set; }       // Department-specific
    public Guid? CohortId { get; set; }                 // Cohort-specific
    public long? UserId { get; set; }                   // User-specific
    public long? OrganizationUnitId { get; set; }       // OU-specific
    public Guid? CohortUserId { get; set; }             // Cohort user specific
    
    // Compliance integration
    public Guid? RecordCategoryRuleId { get; set; }     // Links to compliance requirements
}
```

## Pricing Calculation Logic

### 1. Service Discovery Algorithm

The system uses a sophisticated algorithm to determine which services apply to a user:

```csharp
// From PaymentController.PurchaseModal method
public async Task<PartialViewResult> PurchaseModal(Guid? deptId, Guid? cohortId)
{
    // 1. Get all enabled tenant services
    var tenantSurpathServices = await GetEnabledTenantServices();
    
    // 2. Categorize services by scope
    var unassigned = services.Where(s => s.TenantDepartmentId == null && 
                                         s.CohortId == null && 
                                         s.UserId == null).ToList();
    
    var deptServices = services.Where(s => s.TenantDepartmentId != null && 
                                          userDepartments.Contains(s.TenantDepartmentId.Value)).ToList();
    
    var cohortServices = services.Where(s => s.CohortId != null && 
                                            userCohorts.Contains(s.CohortId.Value)).ToList();
    
    var userServices = services.Where(s => s.UserId != null && 
                                          s.UserId == currentUserId).ToList();
    
    // 3. Apply priority hierarchy
    var prioritizedServices = allServices
        .GroupBy(s => s.SurpathServiceId)
        .Select(group => group
            .OrderByDescending(s => 
                s.UserId != null ? 4 :           // Highest priority
                s.CohortId != null ? 3 :
                s.TenantDepartmentId != null ? 2 :
                1                                // Lowest priority (tenant-wide)
            )
            .First()
        )
        .ToList();
    
    return prioritizedServices;
}
```

### 2. Cost Calculation

Total cost is calculated by summing all applicable service prices:

```csharp
decimal totalCost = applicableServices.Sum(s => (decimal)s.Price);
decimal amountDue = totalCost - amountPaid;
```

## Payment Processing Architecture

### 1. Payment Gateway Integration

The system integrates with **Authorize.Net** for credit card processing:

#### Configuration
```json
{
  "Payment": {
    "AuthorizeNet": {
      "IsActive": "true",
      "UseSandbox": "true|false",
      "ApiLoginID": "your_api_login_id",
      "ApiTransactionKey": "your_transaction_key",
      "PublicClientKey": "your_public_client_key"
    }
  }
}
```

#### Payment Flow
1. **Pre-Authorization**: `AuthNetManager.PreAuthCreditCardRequest()`
2. **Capture**: `AuthNetManager.CapturePreAuthCreditCardRequest()`
3. **Direct Charge**: `AuthNetManager.ChargeCreditCardRequest()`

### 2. Payment Processing Components

#### AuthNetManager
```csharp
public class AuthNetManager : inzibackendDomainServiceBase
{
    // Pre-authorize a credit card transaction
    public async Task<createTransactionResponse> PreAuthCreditCardRequest(
        AuthNetSubmit authNetSubmit, 
        decimal amount, 
        string chargeDescription)
    
    // Capture a pre-authorized transaction
    public async Task<createTransactionResponse> CapturePreAuthCreditCardRequest(
        AuthNetSubmit authNetSubmit, 
        AuthNetCaptureResultDto captureResult, 
        decimal amount, 
        string chargeDescription)
    
    // Direct charge (auth + capture)
    public async Task<bool> ChargeCreditCardRequest(
        AuthNetSubmit authNetSubmit, 
        long userId, 
        decimal amount, 
        string chargeDescription)
}
```

#### SurpathPayManager
```csharp
public class SurpathPayManager : inzibackendDomainServiceBase
{
    // Create financial records after successful payment
    public async Task CreateLedgerEntry(
        transactionResponse transactionResponse, 
        long userId, 
        int? tenantId, 
        decimal amount, 
        AuthNetSubmit authNetSubmit, 
        decimal totalPrice)
    
    // Check if user has paid for services
    public async Task<bool> UserIsPaid(long userId)
    
    // Get available services for tenant
    public async Task<List<TenantSurpathService>> GetSurpathServicesForTenant(int tenantId)
}
```

## Financial Tracking System

### 1. Ledger Architecture

The system maintains detailed financial records through a dual-ledger system:

#### LedgerEntry (Transaction Header)
```csharp
public class LedgerEntry : FullAuditedEntity<Guid>, IMayHaveTenant
{
    // Transaction identification
    public string AuthNetNetworkTransId { get; set; }    // Authorize.Net transaction ID
    public string AuthCode { get; set; }                 // Authorization code
    public string ReferenceTransactionId { get; set; }   // Reference transaction
    
    // Financial details
    public double Amount { get; set; }                   // Amount charged
    public double TotalPrice { get; set; }               // Total service cost
    public double AmountDue { get; set; }                // Remaining balance
    public long PaidAmount { get; set; }                 // Amount actually paid
    
    // Payment method details
    public string AccountNumber { get; set; }            // Masked card number
    public string CardLastFour { get; set; }             // Last 4 digits
    public string CardNameOnCard { get; set; }           // Cardholder name
    public string CardZipCode { get; set; }              // Billing ZIP
    
    // Transaction metadata
    public bool Settled { get; set; }                    // Settlement status
    public DateTime? ExpirationDate { get; set; }        // Service expiration
    public string TransactionMessage { get; set; }       // Full response JSON
    
    // User and tenant context
    public long UserId { get; set; }                     // User who paid
    public int? TenantId { get; set; }                   // Tenant context
    public Guid? CohortId { get; set; }                  // Associated cohort
}
```

#### LedgerEntryDetail (Line Items)
```csharp
public class LedgerEntryDetail : FullAuditedEntity<Guid>, IMayHaveTenant
{
    public Guid? LedgerEntryId { get; set; }             // Parent transaction
    
    // Service details
    public Guid SurpathServiceId { get; set; }           // Service template
    public Guid? TenantSurpathServiceId { get; set; }    // Tenant-specific service
    
    // Financial details
    public double Amount { get; set; }                   // Service price
    public double AmountPaid { get; set; }               // Amount paid for this service
    public decimal Discount { get; set; }               // Applied discount
    public double DiscountAmount { get; set; }           // Discount value
    
    // Payment tracking
    public DateTime? DatePaidOn { get; set; }            // Payment date
    public string Note { get; set; }                     // Additional notes
    public string MetaData { get; set; }                 // Additional data
    
    // User purchase reference
    public Guid? UserPurchaseId { get; set; }            // Links to UserPurchase
}
```

### 2. User Purchase Tracking

#### UserPurchase Entity
```csharp
public class UserPurchase : FullAuditedEntity<Guid>, IMayHaveTenant
{
    // Purchase identification
    public string Name { get; set; }                     // Purchase description
    public string Description { get; set; }              // Detailed description
    
    // Financial details
    public double Price { get; set; }                    // Original price
    public double AmountPaid { get; set; }               // Amount paid so far
    public double Balance => Price - AmountPaid;         // Remaining balance
    
    // Service details
    public Guid? SurpathServiceId { get; set; }          // Service purchased
    public Guid? TenantSurpathServiceId { get; set; }    // Tenant-specific service
    
    // Temporal details
    public DateTime PurchaseDate { get; set; }           // Purchase date
    public DateTime? ExpirationDate { get; set; }        // Service expiration
    public PaymentPeriodType PaymentPeriodType { get; set; } // Billing period
    public bool IsRecurring { get; set; }                // Recurring billing
    
    // Status tracking
    public PurchaseStatus Status { get; set; }           // Current status
    public string Notes { get; set; }                    // Additional notes
    
    // Context
    public long UserId { get; set; }                     // Purchaser
    public Guid? CohortId { get; set; }                  // Associated cohort
    public int? TenantId { get; set; }                   // Tenant context
}
```

## Payment Popup System

### 1. Configuration Hierarchy

The payment popup system allows fine-grained control over when payment prompts appear:

#### Global Settings (Host Level)
```csharp
AppSettings.PaymentPopup.EnableGlobalPaymentPopup = "true|false"
```

#### Tenant Settings
```csharp
public class TenantPaymentPopupSettingsEditDto
{
    public string EnablePaymentPopupForCohort { get; set; }      // Comma-separated cohort IDs
    public string EnablePaymentPopupForDepartment { get; set; }  // Comma-separated department IDs
}
```

### 2. Popup Trigger Logic

```javascript
// From purchaseHelper.js
jQuery(document).ready(function () {
    // Basic eligibility checks
    if (!app.session.tenant) return;
    if (!app.session.user || app.session.user.isPaid) return;
    if (!app.session.user.roles.includes("user")) return;
    
    // Global setting check
    var isGlobalEnabled = abp.setting.get(AppSettings.PaymentPopup.EnableGlobalPaymentPopup);
    if (isGlobalEnabled.toLowerCase() !== "true") return;
    
    // Tenant capability checks
    if (!app.session.tenant.isDonorPay) return;
    if (!app.session.surpathSettings.enableInSessionPayment) return;
    
    var shouldShowPopup = false;
    
    // Cohort-based popup logic
    if (app.session.user.isCohortUser) {
        var enabledCohorts = abp.setting.get(AppSettings.PaymentPopup.EnablePaymentPopupForCohort);
        
        if (!enabledCohorts || enabledCohorts === "") {
            shouldShowPopup = true; // Show to all cohort users if no specific config
        } else {
            var cohortList = enabledCohorts.split(',');
            if (cohortList.includes(app.session.user.cohortId.toString())) {
                shouldShowPopup = true;
            }
        }
    }
    
    // Department-based popup logic
    if (app.session.user.isDepartmentUser) {
        var enabledDepartments = abp.setting.get(AppSettings.PaymentPopup.EnablePaymentPopupForDepartment);
        
        if (!enabledDepartments || enabledDepartments === "") {
            shouldShowPopup = true; // Show to all department users if no specific config
        } else {
            var deptList = enabledDepartments.split(',');
            if (deptList.includes(app.session.user.departmentId.toString())) {
                shouldShowPopup = true;
            }
        }
    }
    
    if (shouldShowPopup) {
        // Show payment popup
        showPaymentModal();
    }
});
```

## User Interface Components

### 1. Payment Modal (`_PurchaseModal.cshtml`)

The payment modal is the primary interface for service purchases:

#### Features:
- **Service Selection**: Displays applicable services with prices
- **Payment Form**: Credit card input with Authorize.Net integration
- **Billing Address**: Separate billing address option
- **Real-time Validation**: Client-side validation before submission
- **Secure Processing**: Uses Authorize.Net Accept.js for PCI compliance

#### Key JavaScript Functions:
```javascript
// From _PurchaseModal.js
function sendPaymentDataToAnet() {
    var authData = {
        clientKey: PublicClientKey,
        apiLoginID: ApiLoginID
    };
    
    var cardData = {
        cardNumber: $('#cardNumber').val(),
        month: $('#expMonth').val(),
        year: $('#expYear').val(),
        cardCode: $('#cardCode').val(),
        zip: $('#billingZipCode').val(),
        fullName: $('#cardNameOnCard').val()
    };
    
    var secureData = {
        authData: authData,
        cardData: cardData
    };
    
    Accept.dispatchData(secureData, opaqueHandler);
}

function opaqueHandler(response) {
    if (response.messages.resultCode === "Ok") {
        // Process successful tokenization
        paymentFormUpdate(response.opaqueData);
    } else {
        // Handle tokenization errors
        showPaymentErrors(response.messages.message);
    }
}
```

### 2. Registration Integration

The payment system integrates seamlessly with user registration:

#### Registration Flow with Payment:
1. **User Registration Form**: Basic user information
2. **Department/Cohort Selection**: Determines applicable services
3. **Service Discovery**: System identifies required services and pricing
4. **Payment Processing**: If services require payment, show payment modal
5. **Account Activation**: Complete registration after successful payment

```csharp
// From AccountController.Register method
if (tenantIsDonorPay && hasRequiredServices) {
    // Calculate total service cost
    var totalPrice = applicableServices.Sum(s => s.Price);
    
    // Process payment
    var response = await _authNetManager.CapturePreAuthCreditCardRequest(
        model.AuthNetSubmit, 
        model.AuthNetCaptureResultDto, 
        totalPrice, 
        tenantName);
    
    if (response.messages.resultCode == messageTypeEnum.Ok) {
        // Create ledger entries
        await _surpathPayManager.CreateLedgerEntry(
            response.transactionResponse, 
            user.Id, 
            user.TenantId, 
            totalPrice, 
            model.AuthNetSubmit, 
            totalPrice);
        
        // Activate user account
        user.IsActive = true;
        user.IsPaid = true;
    } else {
        // Handle payment failure
        user.IsActive = false;
        throw new UserFriendlyException("Payment Failed");
    }
}
```

## Administrative Interfaces

### 1. Service Management

#### TenantSurpathServices Management
- **Location**: `/App/TenantSurpathServices`
- **Features**:
  - Create/Edit tenant-specific services
  - Set pricing for different scopes (tenant, department, cohort, user)
  - Enable/disable services
  - Bulk operations
  - Excel import/export

#### SurpathServices Management (Host Only)
- **Location**: `/App/SurpathServices`
- **Features**:
  - Manage system-wide service templates
  - Set default pricing and discounts
  - Configure feature flags
  - Define compliance integration points

### 2. Financial Reporting

#### LedgerEntries Management
- **Location**: `/App/LedgerEntries`
- **Features**:
  - View all payment transactions
  - Filter by date, user, tenant, amount
  - Export financial reports
  - Transaction details and audit trail

#### UserPurchases Management
- **Location**: `/App/UserPurchases`
- **Features**:
  - Track individual user purchases
  - Apply manual payments
  - Manage payment plans
  - Balance tracking and reporting

### 3. Payment Configuration

#### Tenant Settings
- **Location**: `/App/Settings` → Payment Popup Tab
- **Features**:
  - Configure cohort-specific payment popups
  - Configure department-specific payment popups
  - Set payment timing and triggers

#### Host Settings
- **Location**: Host admin panel
- **Features**:
  - Global payment popup enable/disable
  - Authorize.Net configuration
  - System-wide payment policies

## Security and Compliance

### 1. PCI Compliance

The system maintains PCI compliance through:

#### Secure Tokenization
- **Authorize.Net Accept.js**: Client-side tokenization
- **No Card Storage**: Card data never touches server
- **Secure Transmission**: HTTPS for all payment communications

#### Data Protection
```csharp
// Card data is immediately tokenized and masked
public class LedgerEntry {
    public string AccountNumber { get; set; }        // Masked (e.g., "XXXX1234")
    public string CardLastFour { get; set; }         // Last 4 digits only
    public string AuthNetTransHashSha2 { get; set; } // Secure hash
}
```

### 2. Authorization and Permissions

#### Permission Structure
```csharp
public static class AppPermissions {
    // Payment popup configuration
    public const string Pages_PaymentPopup_Configure = "Pages.PaymentPopup.Configure";
    public const string Pages_PaymentPopup_Configure_Global = "Pages.PaymentPopup.Configure.Global";
    public const string Pages_PaymentPopup_Configure_ForCohort = "Pages.PaymentPopup.Configure.ForCohort";
    public const string Pages_PaymentPopup_Configure_ForDepartment = "Pages.PaymentPopup.Configure.ForDepartment";
    
    // Financial management
    public const string Pages_LedgerEntries = "Pages.LedgerEntries";
    public const string Pages_LedgerEntries_Create = "Pages.LedgerEntries.Create";
    public const string Pages_LedgerEntries_Edit = "Pages.LedgerEntries.Edit";
    public const string Pages_LedgerEntries_Delete = "Pages.LedgerEntries.Delete";
    
    // Service management
    public const string Pages_TenantSurpathServices = "Pages.TenantSurpathServices";
    public const string Pages_SurpathServices = "Pages.SurpathServices"; // Host only
}
```

## Integration with Compliance System

### 1. Service-Requirement Mapping

Services are directly linked to compliance requirements:

```csharp
public class TenantSurpathService {
    public Guid? RecordCategoryRuleId { get; set; }  // Links to compliance requirement
}

// Compliance evaluation considers service purchases
public class SurpathComplianceEvaluator {
    public async Task<ComplianceValues> GetDetailedComplianceValuesForUser(long userId) {
        // Check if user has purchased required services
        var requiredServices = GetRequiredSurpathServices(userId);
        var purchasedServices = GetUserPurchasedServices(userId);
        
        // Service compliance affects overall compliance status
        var serviceCompliance = requiredServices.All(rs => 
            purchasedServices.Any(ps => ps.SurpathServiceId == rs.Id));
        
        return new ComplianceValues {
            ServiceCompliance = serviceCompliance,
            // ... other compliance factors
        };
    }
}
```

### 2. Automatic Service Discovery

The system automatically identifies required services based on compliance requirements:

```csharp
// From PurchaseModal method
var complianceInfo = await _surpathComplianceEvaluator.GetComplianceInfo(tenantId);
var requiredServices = SurpathOnlyRequirements.GetAllSurpathRequirementsFromComplianceInfo(complianceInfo);
var serviceIds = requiredServices.Select(s => s.RecordRequirement.SurpathServiceId).ToList();
var applicableServices = tenantServices.Where(t => serviceIds.Contains(t.SurpathServiceId)).ToList();
```

## Error Handling and Logging

### 1. Payment Error Handling

```csharp
public async Task<bool> ChargeCreditCardRequest(AuthNetSubmit authNetSubmit, long userId, decimal amount) {
    try {
        var response = await ProcessPayment(authNetSubmit, amount);
        
        if (response.messages.resultCode == messageTypeEnum.Ok) {
            if (response.transactionResponse.messages != null) {
                await _surpathPayManager.CreateLedgerEntry(response.transactionResponse, userId, tenantId, amount, authNetSubmit);
                return true;
            } else {
                var errorMsg = L("VerifyPaymentInformation") + "<br>" + 
                              response.transactionResponse.errors[0].errorText + "<br>" + 
                              response.transactionResponse.errors[0].errorCode;
                throw new UserFriendlyException(L("PaymentFailed"), errorMsg);
            }
        } else {
            Logger.Error($"Payment failed for user {userId}: {response.messages.message[0].text}");
            throw new UserFriendlyException(L("PaymentFailed"), response.messages.message[0].text);
        }
    } catch (Exception ex) {
        Logger.Error($"Payment processing error for user {userId}", ex);
        throw new UserFriendlyException(L("PaymentProcessingError"));
    }
}
```

### 2. Transaction Logging

All payment transactions are comprehensively logged:

```csharp
public async Task CreateLedgerEntry(transactionResponse transactionResponse, long userId, int? tenantId, decimal amount, AuthNetSubmit authNetSubmit) {
    var ledgerEntry = new LedgerEntry {
        // Store complete transaction response for audit trail
        TransactionMessage = JsonConvert.SerializeObject(transactionResponse),
        
        // Financial details
        Amount = (double)amount,
        TotalPrice = (double)totalPrice,
        AmountDue = (double)(totalPrice - amount),
        
        // Transaction identification
        AuthNetNetworkTransId = transactionResponse.transId,
        AuthCode = transactionResponse.authCode,
        ReferenceTransactionId = transactionResponse.refTransID,
        
        // Security and audit
        AuthNetTransHashSha2 = transactionResponse.transHashSha2,
        Settled = true,
        
        // Context
        UserId = userId,
        TenantId = tenantId,
        CreationTime = DateTime.Now
    };
    
    await _ledgerEntryRepository.InsertAsync(ledgerEntry);
}
```

## Performance Optimization

### 1. Service Discovery Optimization

```csharp
// Efficient service lookup with minimal database queries
public async Task<List<TenantSurpathServiceDto>> GetApplicableServices(long userId) {
    // Single query to get all user context
    var userContext = await (from u in _userRepository.GetAll()
                            join tdu in _tenantDepartmentUserRepository.GetAll() on u.Id equals tdu.UserId into tdus
                            join cu in _cohortUserRepository.GetAll() on u.Id equals cu.UserId into cus
                            where u.Id == userId
                            select new {
                                User = u,
                                Departments = tdus.Select(tdu => tdu.TenantDepartmentId).ToList(),
                                Cohorts = cus.Select(cu => cu.CohortId).ToList()
                            }).FirstOrDefaultAsync();
    
    // Single query to get all applicable services
    var services = await _tenantSurpathServiceRepository.GetAll()
        .Where(s => s.TenantId == userContext.User.TenantId && s.IsEnabled)
        .Where(s => s.UserId == null || s.UserId == userId)
        .Where(s => s.TenantDepartmentId == null || userContext.Departments.Contains(s.TenantDepartmentId.Value))
        .Where(s => s.CohortId == null || userContext.Cohorts.Contains(s.CohortId.Value))
        .ToListAsync();
    
    return ApplyPriorityHierarchy(services);
}
```

### 2. Caching Strategy

```csharp
// Cache frequently accessed pricing data
[CacheOutput(Duration = 300)] // 5 minutes
public async Task<List<TenantSurpathServiceDto>> GetTenantServices(int tenantId) {
    return await _tenantSurpathServiceRepository.GetAll()
        .Where(s => s.TenantId == tenantId && s.IsEnabled)
        .ProjectTo<TenantSurpathServiceDto>()
        .ToListAsync();
}
```

## Given-When-Then Scenarios

### Scenario 1: New User Registration with Payment

**Given**: A new user is registering for a cohort that requires paid services
**When**: The user completes registration and selects a cohort with required services
**Then**: 
- System identifies applicable services based on cohort membership
- Payment modal displays with total cost calculation
- User enters payment information securely via Authorize.Net
- Payment is processed and ledger entries are created
- User account is activated with appropriate service access

**Implementation**:
```csharp
[Test]
public async Task NewUserRegistration_WithPaidServices_ShouldProcessPaymentAndActivateAccount() {
    // Arrange
    var cohort = CreateTestCohort();
    var requiredService = CreateRequiredService(cohort.TenantDepartmentId);
    var registrationModel = CreateRegistrationModel(cohort.Id);
    
    // Act
    var result = await _accountController.Register(registrationModel);
    
    // Assert
    Assert.True(result.User.IsActive);
    Assert.True(result.User.IsPaid);
    Assert.NotNull(await _ledgerEntryRepository.FirstOrDefaultAsync(le => le.UserId == result.User.Id));
}
```

### Scenario 2: Hierarchical Service Pricing

**Given**: A tenant has services configured at multiple levels (tenant, department, cohort, user)
**When**: A user views available services for purchase
**Then**: 
- System applies priority hierarchy (user > cohort > department > tenant)
- Only the highest priority service for each service type is displayed
- Pricing reflects the most specific configuration available

**Implementation**:
```csharp
[Test]
public async Task ServicePricing_WithHierarchicalConfiguration_ShouldApplyCorrectPriority() {
    // Arrange
    var user = CreateTestUser();
    var cohort = CreateTestCohort();
    var department = CreateTestDepartment();
    
    var tenantService = CreateTenantService(price: 100); // Tenant level
    var deptService = CreateDepartmentService(department.Id, price: 80); // Department level
    var cohortService = CreateCohortService(cohort.Id, price: 60); // Cohort level
    var userService = CreateUserService(user.Id, price: 40); // User level
    
    // Act
    var applicableServices = await _paymentController.GetApplicableServices(user.Id);
    
    // Assert
    Assert.Single(applicableServices);
    Assert.Equal(40, applicableServices.First().Price); // User-specific price wins
}
```

### Scenario 3: Payment Popup Configuration

**Given**: A tenant administrator wants to configure payment popups for specific cohorts
**When**: The administrator updates payment popup settings
**Then**: 
- Only users in specified cohorts see payment popups
- Users in other cohorts do not see payment prompts
- Global settings can override tenant-specific configurations

**Implementation**:
```csharp
[Test]
public async Task PaymentPopup_WithCohortConfiguration_ShouldShowOnlyToSpecifiedCohorts() {
    // Arrange
    var cohort1 = CreateTestCohort();
    var cohort2 = CreateTestCohort();
    var user1 = CreateTestUser(cohort1.Id);
    var user2 = CreateTestUser(cohort2.Id);
    
    await _settingsManager.ChangeSettingForTenantAsync(
        tenantId, 
        AppSettings.PaymentPopup.EnablePaymentPopupForCohort, 
        cohort1.Id.ToString());
    
    // Act
    var shouldShowForUser1 = await _paymentPopupService.ShouldShowPopup(user1.Id);
    var shouldShowForUser2 = await _paymentPopupService.ShouldShowPopup(user2.Id);
    
    // Assert
    Assert.True(shouldShowForUser1);
    Assert.False(shouldShowForUser2);
}
```

### Scenario 4: Failed Payment Handling

**Given**: A user attempts to purchase services with invalid payment information
**When**: The payment processing fails due to declined card or invalid data
**Then**: 
- User receives clear error message indicating the failure reason
- No ledger entries are created for failed transactions
- User account remains inactive until successful payment
- Failed transaction details are logged for audit purposes

**Implementation**:
```csharp
[Test]
public async Task PaymentProcessing_WithInvalidCard_ShouldHandleFailureGracefully() {
    // Arrange
    var user = CreateTestUser();
    var invalidPaymentData = CreateInvalidPaymentData();
    
    // Act & Assert
    var exception = await Assert.ThrowsAsync<UserFriendlyException>(
        () => _authNetManager.ChargeCreditCardRequest(invalidPaymentData, user.Id, 100));
    
    Assert.Contains("Payment Failed", exception.Message);
    Assert.Null(await _ledgerEntryRepository.FirstOrDefaultAsync(le => le.UserId == user.Id));
    Assert.False(user.IsPaid);
}
```

### Scenario 5: Service Compliance Integration

**Given**: A user has compliance requirements that include paid services
**When**: The compliance system evaluates the user's status
**Then**: 
- Unpaid required services result in non-compliant status
- Paid services contribute to overall compliance score
- Service expiration affects ongoing compliance status

**Implementation**:
```csharp
[Test]
public async Task ComplianceEvaluation_WithPaidServices_ShouldReflectServiceStatus() {
    // Arrange
    var user = CreateTestUser();
    var requiredService = CreateRequiredService();
    var userPurchase = CreateUserPurchase(user.Id, requiredService.Id, isPaid: true);
    
    // Act
    var complianceValues = await _complianceEvaluator.GetDetailedComplianceValuesForUser(user.Id);
    
    // Assert
    Assert.True(complianceValues.ServiceCompliance);
    Assert.True(complianceValues.OverallCompliance);
}
```

### Scenario 6: Bulk Payment Processing

**Given**: An administrator needs to apply payments for multiple users
**When**: The administrator uses the bulk payment feature
**Then**: 
- Multiple user purchases are updated simultaneously
- Ledger entries are created for each payment
- User statuses are updated based on new payment amounts
- Audit trail captures all changes with administrator context

**Implementation**:
```csharp
[Test]
public async Task BulkPaymentProcessing_WithMultipleUsers_ShouldUpdateAllRecords() {
    // Arrange
    var users = CreateTestUsers(5);
    var purchases = users.Select(u => CreateUserPurchase(u.Id)).ToList();
    var bulkPaymentData = CreateBulkPaymentData(purchases);
    
    // Act
    await _userPurchaseService.ApplyBulkPayments(bulkPaymentData);
    
    // Assert
    foreach (var purchase in purchases) {
        var updatedPurchase = await _userPurchaseRepository.GetAsync(purchase.Id);
        Assert.True(updatedPurchase.AmountPaid > 0);
        Assert.NotNull(await _ledgerEntryRepository.FirstOrDefaultAsync(
            le => le.UserPurchaseId == purchase.Id));
    }
}
```

This comprehensive documentation provides a complete understanding of the Surpath payment and pricing system's architecture, implementation details, and operational procedures. The system's sophisticated hierarchy, secure payment processing, and integration with compliance requirements make it a robust solution for educational institution financial management. 