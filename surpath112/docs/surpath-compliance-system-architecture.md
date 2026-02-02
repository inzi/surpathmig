# Surpath Compliance System Architecture

## Overview

The Surpath system is a comprehensive compliance management platform designed for educational institutions (primarily nursing schools) to track student compliance with various requirements including immunizations, background checks, drug tests, and other documentation. This document provides a detailed explanation of the system's architecture, entity relationships, and compliance logic.

## Core Entity Relationships

### 1. Tenant Hierarchy

```
Tenant (abptenants)
├── TenantDepartments
│   ├── Cohorts
│   │   └── CohortUsers
│   ├── TenantDepartmentUsers (direct department membership)
│   ├── RecordRequirements (department-scoped)
│   └── RecordStatuses (department-scoped)
├── RecordRequirements (tenant-wide)
└── RecordStatuses (tenant-wide)
```

**Key Relationships:**
- **Tenant**: The top-level organization (e.g., "Navarro College")
- **TenantDepartment**: Academic departments within a tenant (e.g., "ADN", "LVN-RN Bridge")
- **Cohort**: Student groups within departments (e.g., "LVN-RN Bridge (Grads 2026)-COR")
- **Users**: Students and staff who belong to cohorts or departments

### 2. Requirements and Categories Structure

```
RecordRequirement
├── RecordCategory (1:many)
│   ├── RecordCategoryRule (1:1)
│   └── RecordStates (many)
│       ├── RecordStatus (1:1)
│       └── Record (0:1) - actual uploaded document
└── Scoping (one of):
    ├── TenantDepartmentId (department-wide)
    ├── CohortId (cohort-specific)
    └── NULL (tenant-wide)
```

**Key Relationships:**
- **RecordRequirement**: Defines what type of document/compliance is needed
- **RecordCategory**: Specific categories within a requirement (e.g., "MMR" under "Immunizations")
- **RecordCategoryRule**: Rules governing the category (required, expiration, notifications)
- **RecordState**: Tracks a user's compliance status for a specific category
- **RecordStatus**: The actual status (e.g., "Compliant", "Pending", "Expired")
- **Record**: The uploaded document/file (optional for some requirements)

### 3. User Membership and Scoping

```
User
├── CohortUsers (cohort membership)
│   └── Cohort
│       └── TenantDepartment
├── TenantDepartmentUsers (direct department membership)
│   └── TenantDepartment
└── RecordStates (compliance tracking)
    ├── RecordCategory
    ├── RecordStatus
    └── Record (optional)
```

## Database Schema Details

### Core Tables

#### 1. **abptenants**
- **Purpose**: Top-level organizations
- **Key Fields**: `Id`, `Name`, `TenancyName`

#### 2. **tenantdepartments**
- **Purpose**: Academic departments within tenants
- **Key Fields**: `Id`, `TenantId`, `Name`, `Description`, `Active`
- **Relationships**: 
  - Belongs to: `abptenants`
  - Has many: `cohorts`, `recordrequirements`, `recordstatuses`

#### 3. **cohorts**
- **Purpose**: Student groups within departments
- **Key Fields**: `Id`, `TenantId`, `TenantDepartmentId`, `Name`, `Description`, `DefaultCohort`
- **Relationships**:
  - Belongs to: `tenantdepartments`
  - Has many: `cohortusers`, `recordrequirements` (cohort-specific)

#### 4. **cohortusers**
- **Purpose**: Links users to cohorts
- **Key Fields**: `Id`, `TenantId`, `CohortId`, `UserId`
- **Relationships**:
  - Belongs to: `cohorts`, `abpusers`

#### 5. **tenantdepartmentusers**
- **Purpose**: Direct department membership (bypassing cohorts)
- **Key Fields**: `Id`, `TenantId`, `TenantDepartmentId`, `UserId`
- **Relationships**:
  - Belongs to: `tenantdepartments`, `abpusers`

#### 6. **recordrequirements**
- **Purpose**: Defines types of requirements/documents needed
- **Key Fields**: 
  - `Id`, `TenantId`, `Name`, `Description`
  - `CohortId` (NULL = not cohort-specific)
  - `TenantDepartmentId` (NULL = not department-specific)
  - `IsSurpathOnly` (true for drug tests, background checks)
  - `SurpathServiceId`, `TenantSurpathServiceId`
- **Scoping Logic**:
  - `CohortId = NULL, TenantDepartmentId = NULL`: Applies to all users in tenant
  - `CohortId = NULL, TenantDepartmentId = X`: Applies to all users in department X
  - `CohortId = Y, TenantDepartmentId = X`: Applies to users in cohort Y within department X
  - `CohortId = Y, TenantDepartmentId = NULL`: Applies to users in cohort Y (rare)

#### 7. **recordcategories**
- **Purpose**: Specific categories within requirements
- **Key Fields**: `Id`, `TenantId`, `RecordRequirementId`, `Name`, `RecordCategoryRuleId`
- **Relationships**:
  - Belongs to: `recordrequirements`, `recordcategoryrules`
  - Has many: `recordstates`

#### 8. **recordcategoryrules**
- **Purpose**: Rules governing categories
- **Key Fields**: 
  - `Id`, `TenantId`, `Name`, `Description`
  - `Required` (boolean - determines if category affects compliance)
  - `IsSurpathOnly` (boolean)
  - `Expires`, `ExpireInDays`
  - `Notify`, `WarnDaysBeforeFirst`, `WarnDaysBeforeSecond`, `WarnDaysBeforeFinal`

#### 9. **recordstates**
- **Purpose**: Tracks user compliance for specific categories
- **Key Fields**: 
  - `Id`, `TenantId`, `UserId`, `RecordCategoryId`
  - `RecordStatusId` (current status)
  - `RecordId` (uploaded document, can be NULL)
  - `State`, `Notes`
- **Relationships**:
  - Belongs to: `abpusers`, `recordcategories`, `recordstatuses`, `records`

#### 10. **recordstatuses**
- **Purpose**: Defines possible statuses for record states
- **Key Fields**: 
  - `Id`, `TenantId`, `TenantDepartmentId`, `StatusName`
  - `ComplianceImpact` (enum: Compliant=1, NotCompliant=0, etc.)
  - `IsDefault`, `IsSurpathServiceStatus`
  - `HtmlColor`, `CSSClass` (for UI display)
- **Scoping**: Can be tenant-wide or department-specific

#### 11. **records**
- **Purpose**: Actual uploaded documents/files
- **Key Fields**: `Id`, `TenantId`, `filename`, `BinaryObjId`, `TenantDocumentCategoryId`
- **Relationships**: Has many: `recordstates`

## Compliance Logic

### 1. Requirement Determination Logic

The system determines which requirements apply to a user through the `GetComplianceInfo` method in `SurpathComplianceEvaluator.cs`:

```csharp
// Step 1: Get user's department and cohort memberships
var userMemberships = GetUserDeptAndCohortMembership(userId);
var userDeptList = userMemberships.Select(um => um.TenantDepartmentId).Distinct();
var userCohort = userMemberships.Where(um => um.CohortId != null).FirstOrDefault();

// Step 2: Apply requirements in order of specificity
var requirementsForUser = new List<TenantRequirement>();

// 2a. Tenant-wide requirements (apply to all users)
requirementsForUser.AddRange(
    tenantRequirements.Where(r => 
        r.RecordRequirement.CohortId == null && 
        r.RecordRequirement.TenantDepartmentId == null)
);

// 2b. Department-wide requirements
requirementsForUser.AddRange(
    tenantRequirements.Where(r => 
        r.RecordRequirement.CohortId == null && 
        r.RecordRequirement.TenantDepartmentId != null && 
        userDeptList.Contains(r.RecordRequirement.TenantDepartmentId))
);

// 2c. Cohort-specific requirements within departments
requirementsForUser.AddRange(
    tenantRequirements.Where(r => 
        r.RecordRequirement.CohortId != null && 
        r.RecordRequirement.TenantDepartmentId != null && 
        userDeptList.Contains(r.RecordRequirement.TenantDepartmentId) && 
        userCohort?.CohortId == r.RecordRequirement.CohortId)
);

// 2d. Cohort-specific requirements (department-agnostic)
requirementsForUser.AddRange(
    tenantRequirements.Where(r => 
        r.RecordRequirement.CohortId != null && 
        r.RecordRequirement.TenantDepartmentId == null && 
        userCohort?.CohortId == r.RecordRequirement.CohortId)
);
```

### 2. Compliance Evaluation Logic

The system evaluates compliance through the `GetDetailedComplianceValuesForUser` method:

#### A. **Drug Test Compliance**
```csharp
// Find Surpath-only requirements containing "drug"
var drugRequirement = requirements.FirstOrDefault(r => 
    r.RecordRequirement.IsSurpathOnly && 
    r.RecordRequirement.Name.ToLower().Contains("drug"));

if (drugRequirement != null) {
    var userRecord = userRecords.FirstOrDefault(r => 
        r.RecordRequirement.Id == drugRequirement.RecordRequirement.Id);
    
    complianceValues.Drug = userRecord?.RecordStatus.ComplianceImpact == EnumStatusComplianceImpact.Compliant;
}
```

#### B. **Background Check Compliance**
```csharp
// Find Surpath-only requirements containing "background"
var backgroundRequirement = requirements.FirstOrDefault(r => 
    r.RecordRequirement.IsSurpathOnly && 
    r.RecordRequirement.Name.ToLower().Contains("background"));

if (backgroundRequirement != null) {
    var userRecord = userRecords.FirstOrDefault(r => 
        r.RecordRequirement.Id == backgroundRequirement.RecordRequirement.Id);
    
    complianceValues.Background = userRecord?.RecordStatus.ComplianceImpact == EnumStatusComplianceImpact.Compliant;
}
```

#### C. **Immunization Compliance**
```csharp
// Get all non-Surpath requirements that are marked as Required
var nonSurpathRequiredCategoryIds = requirements
    .Where(r => !r.RecordRequirement.IsSurpathOnly && r.RecordCategoryRule?.Required == true)
    .Select(r => r.RecordCategory.Id)
    .ToList();

// Get all categories where user has compliant status
var compliantUserCategoryIds = userRecords
    .Where(r => r.RecordStatus.ComplianceImpact == EnumStatusComplianceImpact.Compliant)
    .Select(r => r.RecordCategory.Id)
    .ToList();

// User is immunization compliant if ALL required categories are compliant
complianceValues.Immunization = !nonSurpathRequiredCategoryIds.Except(compliantUserCategoryIds).Any();
```

#### D. **Overall Compliance**
```csharp
// Get ALL required categories (both Surpath and non-Surpath)
var allRequiredCategoryIds = requirements
    .Where(r => r.RecordCategoryRule?.Required == true)
    .Select(r => r.RecordCategory.Id)
    .ToList();

var allCompliantCategoryIds = userRecords
    .Where(r => r.RecordStatus.ComplianceImpact == EnumStatusComplianceImpact.Compliant)
    .Select(r => r.RecordCategory.Id)
    .ToList();

var overallCompliance = !allRequiredCategoryIds.Except(allCompliantCategoryIds).Any();

complianceValues.InCompliance = 
    complianceValues.Drug && 
    complianceValues.Background && 
    complianceValues.Immunization && 
    overallCompliance;
```

### 3. Record State Management

#### A. **Record State Creation**
When a user is assigned to a cohort or department, the system automatically creates `recordstates` entries for all applicable requirements:

```sql
-- Example: Create recordstates for new department requirements
INSERT INTO recordstates (Id, TenantId, UserId, RecordCategoryId, RecordStatusId, State, CreationTime, CreatorUserId, IsDeleted)
SELECT 
    UUID(),
    @tenant_id,
    cu.UserId,
    rc.Id,
    @default_status_id,  -- Usually "Pending" or "Not Compliant"
    0,
    NOW(),
    1,
    0
FROM recordrequirements rr
JOIN recordcategories rc ON rr.Id = rc.RecordRequirementId
CROSS JOIN cohortusers cu
WHERE rr.TenantDepartmentId = @target_dept_id
  AND cu.CohortId IN (@cohort_ids)
  AND NOT EXISTS (
    SELECT 1 FROM recordstates rs 
    WHERE rs.UserId = cu.UserId 
      AND rs.RecordCategoryId = rc.Id 
      AND rs.IsDeleted = 0
  );
```

#### B. **Status Updates**
Record states are updated when:
1. **Documents are uploaded**: Status changes from "Pending" to "Under Review"
2. **Documents are reviewed**: Status changes to "Compliant", "Non-Compliant", or "Needs Revision"
3. **Surpath services complete**: Drug tests and background checks update automatically
4. **Administrative actions**: Manual status changes by administrators

### 4. Surpath Services Integration

#### A. **Surpath-Only Requirements**
Special requirements managed by external Surpath services:
- **Drug Tests**: `IsSurpathOnly = true`, name contains "drug"
- **Background Checks**: `IsSurpathOnly = true`, name contains "background"

#### B. **Service Linking**
```
RecordRequirement
├── SurpathServiceId (global service definition)
└── TenantSurpathServiceId (tenant-specific service configuration)
    ├── IsEnabled (can be disabled per tenant)
    ├── Price (tenant-specific pricing)
    └── RecordCategoryRuleId (links to compliance rules)
```

## Migration and Data Consistency

### 1. Cohort Migration Process

When moving cohorts between departments (as seen in the `moveuserstocohort.sql` script):

#### A. **Update Cohort Assignment**
```sql
UPDATE cohorts 
SET TenantDepartmentId = @target_dept_id
WHERE Id IN (@cohort_ids);
```

#### B. **Map Existing Record States**
```sql
-- Map equivalent categories between departments
UPDATE recordstates rs
JOIN cohortusers cu ON rs.UserId = cu.UserId
SET rs.RecordCategoryId = @new_category_id
WHERE cu.CohortId IN (@cohort_ids)
  AND rs.RecordCategoryId = @old_category_id;
```

#### C. **Create Missing Record States**
```sql
-- Create recordstates for new department requirements
-- CRITICAL: Use compliant status for migrated users to maintain compliance
INSERT INTO recordstates (...)
SELECT ..., 
  CASE 
    WHEN @grandfathered_status_id IS NOT NULL THEN @grandfathered_status_id
    ELSE @default_status_id
  END as RecordStatusId
FROM recordrequirements rr
JOIN recordcategories rc ON rr.Id = rc.RecordRequirementId
WHERE rr.TenantDepartmentId = @target_dept_id
  AND NOT EXISTS (existing recordstate check);
```

#### D. **Clean Up Obsolete Requirements**
```sql
-- Soft delete old cohort-specific requirements
UPDATE recordrequirements 
SET IsDeleted = 1
WHERE Id IN (@obsolete_requirement_ids);
```

### 2. Data Integrity Considerations

#### A. **Foreign Key Relationships**
- `recordstates.RecordCategoryId` → `recordcategories.Id`
- `recordcategories.RecordRequirementId` → `recordrequirements.Id`
- `recordstates.RecordStatusId` → `recordstatuses.Id`

#### B. **Soft Deletion**
All entities use soft deletion (`IsDeleted` flag) to maintain referential integrity and audit trails.

#### C. **Tenant Isolation**
All entities include `TenantId` for multi-tenant data isolation.

## Surpath Services Architecture

### 1. Service Hierarchy and Scoping

Surpath services operate on a two-tier architecture that enables both system-wide standardization and tenant-specific customization:

```
SurpathService (System-Wide Template)
├── Global service definitions (Drug Test, Background Check)
├── Default pricing and configuration
├── Feature identifiers for system integration
└── TenantSurpathService (Tenant-Specific Implementation)
    ├── Tenant-customized pricing
    ├── Department/Cohort/User-specific scoping
    ├── Enable/disable per tenant
    └── Links to RecordCategoryRules for compliance
```

#### A. **SurpathService (System-Wide)**
- **Purpose**: Global service templates managed by system administrators
- **Scope**: Available across all tenants but not directly used
- **Key Fields**:
  - `Name`, `Description`: Service identification
  - `Price`, `Discount`: Default pricing structure
  - `IsEnabledByDefault`: Whether new tenants get this service automatically
  - `FeatureIdentifier`: Links to external service providers (e.g., "DRUG_TEST_PROVIDER_A")
  - `RecordCategoryRuleId`: Links to compliance rules template

#### B. **TenantSurpathService (Tenant-Specific)**
- **Purpose**: Tenant-customized implementations of system services
- **Scope**: Specific to individual tenants with granular targeting
- **Key Fields**:
  - `SurpathServiceId`: References the system-wide template
  - `IsEnabled`: Tenant can disable services
  - `Price`: Tenant-specific pricing (overrides system default)
  - `Name`, `Description`: Tenant can customize naming
  - **Scoping Fields** (hierarchical priority):
    - `UserId`: Individual user-specific service
    - `CohortUserId`: Specific cohort user assignment
    - `CohortId`: Cohort-wide service
    - `TenantDepartmentId`: Department-wide service
    - `OrganizationUnitId`: Organization unit-specific
    - `NULL` (all fields): Tenant-wide service

### 2. Service-to-Requirement Integration

Surpath services integrate with the compliance system through a sophisticated linking mechanism:

```
SurpathService
├── FeatureIdentifier → External Provider Integration
└── TenantSurpathService
    ├── RecordCategoryRuleId → Compliance Rules
    └── RecordRequirement
        ├── SurpathServiceId (system reference)
        ├── TenantSurpathServiceId (tenant implementation)
        ├── IsSurpathOnly = true
        └── RecordCategory
            └── RecordState (user compliance tracking)
```

#### A. **Service Linking Process**
1. **System Admin** creates `SurpathService` with `FeatureIdentifier`
2. **Tenant Admin** creates `TenantSurpathService` linked to system service
3. **Requirement Creation** links `RecordRequirement` to both service levels
4. **Category Generation** creates `RecordCategory` for the requirement
5. **User Assignment** creates `RecordState` for each applicable user

#### B. **Compliance Integration**
```csharp
// In SurpathComplianceEvaluator.cs
var drugRequirement = requirements.FirstOrDefault(r => 
    r.RecordRequirement.IsSurpathOnly && 
    r.RecordRequirement.Name.ToLower().Contains("drug") &&
    r.RecordRequirement.TenantSurpathServiceId != null);

if (drugRequirement != null) {
    var tenantService = tenantSurpathServices.FirstOrDefault(ts => 
        ts.Id == drugRequirement.RecordRequirement.TenantSurpathServiceId);
    
    // Service-specific compliance logic
    complianceValues.Drug = EvaluateServiceCompliance(drugRequirement, tenantService);
}
```

### 3. Current Service Implementations

#### A. **Drug Testing Service**
- **System Service**: Global "Drug Test" template
- **Provider Integration**: `FeatureIdentifier` links to external testing provider
- **Compliance Logic**: 
  - `IsSurpathOnly = true`
  - Automatic status updates via webhook/API integration
  - Results: "Compliant" (passed), "Non-Compliant" (failed), "Pending" (ordered)
- **Pricing**: Tenant-customizable, typically $30-50 per test

#### B. **Background Check Service**
- **System Service**: Global "Background Check" template  
- **Provider Integration**: `FeatureIdentifier` links to background check provider
- **Compliance Logic**:
  - `IsSurpathOnly = true`
  - Automatic status updates via external service integration
  - Results: "Compliant" (clear), "Non-Compliant" (issues found), "Pending" (in progress)
- **Pricing**: Tenant-customizable, typically $15-25 per check

#### C. **Immunization Service (Future)**
- **Current State**: Not yet migrated to Surpath service model
- **Current Implementation**: Document-based requirements with manual review
- **Migration Plan**: 
  - Create system-wide "Immunization Verification" service
  - Link to external immunization verification providers
  - Maintain document upload capability as fallback
  - Automatic compliance verification where possible

### 4. Service Scoping and Priority Logic

The system applies services to users based on hierarchical scoping with priority resolution:

```csharp
// Service Priority Resolution (highest to lowest priority)
public TenantSurpathService GetApplicableService(long userId, Guid serviceId) {
    var userServices = GetServicesForUser(userId, serviceId);
    
    // 1. User-specific service (highest priority)
    var userSpecific = userServices.FirstOrDefault(s => s.UserId == userId);
    if (userSpecific != null) return userSpecific;
    
    // 2. Cohort user-specific service
    var cohortUserSpecific = userServices.FirstOrDefault(s => 
        s.CohortUserId != null && UserInCohortUser(userId, s.CohortUserId.Value));
    if (cohortUserSpecific != null) return cohortUserSpecific;
    
    // 3. Cohort-wide service
    var cohortWide = userServices.FirstOrDefault(s => 
        s.CohortId != null && UserInCohort(userId, s.CohortId.Value));
    if (cohortWide != null) return cohortWide;
    
    // 4. Department-wide service
    var departmentWide = userServices.FirstOrDefault(s => 
        s.TenantDepartmentId != null && UserInDepartment(userId, s.TenantDepartmentId.Value));
    if (departmentWide != null) return departmentWide;
    
    // 5. Organization unit service
    var orgUnitService = userServices.FirstOrDefault(s => 
        s.OrganizationUnitId != null && UserInOrgUnit(userId, s.OrganizationUnitId.Value));
    if (orgUnitService != null) return orgUnitService;
    
    // 6. Tenant-wide service (lowest priority)
    return userServices.FirstOrDefault(s => 
        s.UserId == null && s.CohortId == null && s.TenantDepartmentId == null);
}
```

### 5. Service Lifecycle Management

#### A. **Service Provisioning**
```csharp
// When a user is assigned to a cohort/department
public async Task ProvisionServicesForUser(long userId, Guid? cohortId, Guid? departmentId) {
    // 1. Get applicable services based on user's new assignments
    var applicableServices = await GetApplicableServices(userId, cohortId, departmentId);
    
    // 2. Create record requirements for Surpath-only services
    foreach (var service in applicableServices.Where(s => s.IsEnabled)) {
        await CreateServiceRequirement(userId, service);
    }
    
    // 3. Create initial record states with "Not Ordered" status
    foreach (var requirement in serviceRequirements) {
        await CreateInitialRecordState(userId, requirement, "Not Ordered");
    }
}

private async Task CreateServiceRequirement(long userId, TenantSurpathService service) {
    var requirement = new RecordRequirement {
        Name = service.Name,
        Description = service.Description,
        IsSurpathOnly = true,
        SurpathServiceId = service.SurpathServiceId,
        TenantSurpathServiceId = service.Id,
        TenantId = service.TenantId
    };
    
    await _recordRequirementRepository.InsertAsync(requirement);
    
    // Create associated category and rule
    await CreateServiceCategory(requirement, service);
}
```

#### B. **Service Ordering and Payment**
```csharp
// In PaymentController.cs - Service ordering logic
public async Task<IActionResult> ProcessServicePayment(List<Guid> tenantSurpathServiceIds) {
    var services = await GetTenantSurpathServices(tenantSurpathServiceIds);
    var totalAmount = services.Sum(s => s.Price);
    
    // 1. Process payment
    var paymentResult = await _paymentProcessor.ProcessPayment(totalAmount);
    
    if (paymentResult.IsSuccessful) {
        // 2. Create user purchases
        foreach (var service in services) {
            await CreateUserPurchase(AbpSession.UserId.Value, service, paymentResult);
        }
        
        // 3. Update record states to "Ordered" status
        await UpdateServiceRecordStates(AbpSession.UserId.Value, services, "Ordered");
        
        // 4. Trigger external service orders
        await TriggerExternalServiceOrders(AbpSession.UserId.Value, services);
    }
    
    return Json(new { success = paymentResult.IsSuccessful });
}
```

#### C. **External Service Integration**
```csharp
// Service provider webhook handling
[HttpPost]
public async Task<IActionResult> ProcessServiceResult([FromBody] ServiceResultDto result) {
    // 1. Validate webhook authenticity
    if (!await ValidateWebhookSignature(result)) {
        return Unauthorized();
    }
    
    // 2. Find associated record state
    var recordState = await FindRecordStateByExternalId(result.ExternalOrderId);
    if (recordState == null) return NotFound();
    
    // 3. Update compliance status based on result
    var newStatus = result.IsPassed ? "Compliant" : "Non-Compliant";
    await UpdateRecordStateStatus(recordState, newStatus, result.ResultDetails);
    
    // 4. Trigger compliance recalculation
    await _complianceEvaluator.RecalculateUserCompliance(recordState.UserId.Value);
    
    // 5. Send notifications
    await _notificationService.NotifyServiceCompletion(recordState.UserId.Value, result);
    
    return Ok();
}
```

### 6. Service Configuration and Management

#### A. **System Administrator Functions**
- **Create Global Services**: Define new service types available to all tenants
- **Set Default Pricing**: Establish baseline pricing that tenants can override
- **Configure Providers**: Link services to external provider APIs
- **Feature Management**: Enable/disable services system-wide

#### B. **Tenant Administrator Functions**
- **Enable/Disable Services**: Control which services are available to their users
- **Customize Pricing**: Set tenant-specific pricing for services
- **Scope Services**: Target services to specific departments, cohorts, or users
- **Configure Rules**: Link services to compliance rules and requirements

#### C. **Service Pricing Strategy**
```csharp
public class ServicePricingCalculator {
    public double CalculateServicePrice(TenantSurpathService tenantService, SurpathService systemService) {
        // 1. Start with tenant-specific price if set
        if (tenantService.Price > 0) {
            return tenantService.Price;
        }
        
        // 2. Fall back to system default price
        var basePrice = systemService.Price;
        
        // 3. Apply system-wide discount if applicable
        if (systemService.Discount > 0) {
            basePrice = basePrice * (1 - (double)systemService.Discount);
        }
        
        return basePrice;
    }
}
```

### 7. Migration Path for Immunization Services

#### A. **Current State Analysis**
- **Immunizations**: Currently document-based with manual review
- **Categories**: MMR, Hepatitis B, TB Test, etc.
- **Process**: Upload → Review → Approve/Reject
- **Compliance**: Based on document approval status

#### B. **Proposed Migration Strategy**
1. **Create System Service**: "Immunization Verification Service"
2. **Provider Integration**: Link to immunization verification APIs
3. **Hybrid Approach**: 
   - Automatic verification where possible
   - Document upload fallback for complex cases
   - Manual review override capability
4. **Gradual Rollout**: Department-by-department migration
5. **Compliance Preservation**: Maintain existing compliance during transition

#### C. **Implementation Phases**
```csharp
// Phase 1: Service Creation
var immunizationService = new SurpathService {
    Name = "Immunization Verification",
    Description = "Automated immunization record verification",
    FeatureIdentifier = "IMMUNIZATION_VERIFY_API",
    IsEnabledByDefault = false, // Opt-in during migration
    Price = 5.00 // Lower cost due to automation
};

// Phase 2: Tenant Opt-in
var tenantService = new TenantSurpathService {
    SurpathServiceId = immunizationService.Id,
    TenantId = tenantId,
    IsEnabled = true,
    Price = 3.00, // Tenant-specific pricing
    Name = "Immunization Verification"
};

// Phase 3: Requirement Migration
foreach (var immunizationRequirement in existingImmunizationRequirements) {
    immunizationRequirement.IsSurpathOnly = true;
    immunizationRequirement.TenantSurpathServiceId = tenantService.Id;
    // Preserve existing record states and compliance
}
```

## Performance Considerations

### 1. Bulk Compliance Evaluation

The `GetBulkComplianceValuesForUsers` method optimizes performance for large user sets:

```csharp
// 1. Batch load all shared tenant-level data
var tenantRequirements = await GetTenantRequirements(tenantId);
var userMemberships = await GetAllUserMemberships(userIds);
var userRecords = await GetAllUserRecords(userIds);

// 2. Process compliance for each user in memory
foreach (var userId in userIds) {
    var userSpecificMemberships = userMemberships.Where(m => m.UserId == userId);
    var userSpecificRecords = userRecords.Where(r => r.UserId == userId);
    
    // Apply compliance logic without additional database calls
    var compliance = EvaluateCompliance(userSpecificMemberships, userSpecificRecords, tenantRequirements);
    results[userId] = compliance;
}
```

### 2. Caching Strategy

- **Tenant Requirements**: Cached per tenant to avoid repeated queries
- **User Memberships**: Cached during session for frequent compliance checks
- **Record Statuses**: Cached as they change infrequently
- **Service Configurations**: Cached per tenant to optimize service resolution

### 3. Database Indexing

Key indexes for performance:
```sql
-- User-based queries
CREATE INDEX IX_RecordStates_UserId_TenantId ON recordstates(UserId, TenantId);
CREATE INDEX IX_CohortUsers_UserId ON cohortusers(UserId);
CREATE INDEX IX_TenantDepartmentUsers_UserId ON tenantdepartmentusers(UserId);

-- Requirement queries
CREATE INDEX IX_RecordRequirements_TenantDepartmentId ON recordrequirements(TenantDepartmentId);
CREATE INDEX IX_RecordRequirements_CohortId ON recordrequirements(CohortId);
CREATE INDEX IX_RecordRequirements_SurpathServiceId ON recordrequirements(SurpathServiceId);
CREATE INDEX IX_RecordRequirements_TenantSurpathServiceId ON recordrequirements(TenantSurpathServiceId);

-- Service queries
CREATE INDEX IX_TenantSurpathServices_SurpathServiceId ON tenantsurpathservices(SurpathServiceId);
CREATE INDEX IX_TenantSurpathServices_TenantId_IsEnabled ON tenantsurpathservices(TenantId, IsEnabled);
CREATE INDEX IX_TenantSurpathServices_CohortId ON tenantsurpathservices(CohortId);
CREATE INDEX IX_TenantSurpathServices_TenantDepartmentId ON tenantsurpathservices(TenantDepartmentId);

-- Compliance queries
CREATE INDEX IX_RecordStates_RecordCategoryId_UserId ON recordstates(RecordCategoryId, UserId);
CREATE INDEX IX_RecordStatuses_ComplianceImpact ON recordstatuses(ComplianceImpact);
```

---

## Given-When-Then Compliance Scenarios

### Scenario 1: New Student Enrollment

**Given**: A new student is enrolled in the "LVN-RN Bridge (Grads 2026)-COR" cohort
**When**: The student is added to the cohort via `CohortUsersAppService.CreateOrEdit`
**Then**: 
- Record states are automatically created for all department and cohort requirements
- Initial status is set to "Pending" for document-based requirements
- Initial status is set to "Not Ordered" for Surpath services (drug test, background check)
- Student appears as "Not Compliant" until requirements are fulfilled

**Implementation**:
```csharp
// In SurpathManager or similar service
public async Task AddUserToDefaultCohort(long userId, int tenantId) {
    // 1. Create cohort user relationship
    var cohortUser = new CohortUser { UserId = userId, CohortId = defaultCohortId };
    await _cohortUserRepository.InsertAsync(cohortUser);
    
    // 2. Get all requirements for this cohort
    var requirements = await GetRequirementsForCohort(defaultCohortId);
    
    // 3. Create record states for each requirement category
    foreach (var requirement in requirements) {
        foreach (var category in requirement.Categories) {
            var recordState = new RecordState {
                UserId = userId,
                RecordCategoryId = category.Id,
                RecordStatusId = GetDefaultStatusId(category),
                State = 0,
                TenantId = tenantId
            };
            await _recordStateRepository.InsertAsync(recordState);
        }
    }
}
```

### Scenario 2: Document Upload and Review

**Given**: A student has uploaded an MMR immunization record
**When**: An administrator reviews and approves the document
**Then**: 
- Record state status changes from "Under Review" to "Compliant"
- Student's immunization compliance is recalculated
- If all immunization requirements are now compliant, immunization status becomes true
- Overall compliance is recalculated

**Implementation**:
```csharp
// In RecordStatesAppService
public async Task UpdateRecordStatus(Guid recordStateId, Guid newStatusId, string notes) {
    var recordState = await _recordStateRepository.GetAsync(recordStateId);
    var newStatus = await _recordStatusRepository.GetAsync(newStatusId);
    
    recordState.RecordStatusId = newStatusId;
    recordState.Notes = notes;
    recordState.LastModificationTime = DateTime.Now;
    
    await _recordStateRepository.UpdateAsync(recordState);
    
    // Trigger compliance recalculation
    var compliance = await _complianceEvaluator.GetDetailedComplianceValuesForUser(recordState.UserId.Value);
    
    // Optionally notify user of status change
    if (newStatus.ComplianceImpact == EnumStatusComplianceImpact.Compliant) {
        await _notificationService.NotifyUserOfCompliance(recordState.UserId.Value, recordState.RecordCategoryId);
    }
}
```

### Scenario 3: Surpath Service Completion

**Given**: A student has ordered a drug test through the Surpath service
**When**: The drug test results are received and processed
**Then**: 
- Record state status automatically updates to "Compliant" or "Non-Compliant" based on results
- Student's drug test compliance is immediately updated
- Overall compliance is recalculated
- Student and administrators are notified of the result

**Implementation**:
```csharp
// In SurpathServiceWebhookHandler
public async Task ProcessDrugTestResult(DrugTestResultDto result) {
    // 1. Find the user's drug test record state
    var recordState = await _recordStateRepository.FirstOrDefaultAsync(rs => 
        rs.UserId == result.UserId && 
        rs.RecordCategory.RecordRequirement.IsSurpathOnly == true &&
        rs.RecordCategory.RecordRequirement.Name.Contains("Drug"));
    
    if (recordState != null) {
        // 2. Update status based on test result
        var statusId = result.IsPassed ? 
            await GetCompliantStatusId(recordState.TenantId) : 
            await GetNonCompliantStatusId(recordState.TenantId);
        
        recordState.RecordStatusId = statusId;
        recordState.LastModificationTime = DateTime.Now;
        recordState.Notes = $"Test completed on {result.TestDate}. Result: {result.Result}";
        
        await _recordStateRepository.UpdateAsync(recordState);
        
        // 3. Notify stakeholders
        await _notificationService.NotifyDrugTestCompletion(result.UserId, result.IsPassed);
    }
}
```

### Scenario 4: Cohort Migration Between Departments

**Given**: LVN-RN Bridge cohorts need to move from ADN department to LVN-RN Bridge department
**When**: The migration script `moveuserstocohort.sql` is executed
**Then**: 
- Cohort department assignment is updated
- Existing record states are mapped to equivalent categories in the new department
- New record states are created for additional requirements in the target department
- **Critical**: New record states use "Compliant" status to maintain existing compliance
- Users who were compliant before migration remain compliant after migration

**Implementation** (SQL):
```sql
-- 1. Update cohort department
UPDATE cohorts SET TenantDepartmentId = @target_dept_id WHERE Id IN (@cohort_ids);

-- 2. Map existing record states
UPDATE recordstates rs
JOIN cohortusers cu ON rs.UserId = cu.UserId
SET rs.RecordCategoryId = @new_category_id
WHERE cu.CohortId IN (@cohort_ids) AND rs.RecordCategoryId = @old_category_id;

-- 3. Create missing record states with grandfathered compliance
INSERT INTO recordstates (Id, TenantId, UserId, RecordCategoryId, RecordStatusId, ...)
SELECT UUID(), @tenant_id, cu.UserId, rc.Id, @compliant_status_id, ...
FROM recordrequirements rr
JOIN recordcategories rc ON rr.Id = rc.RecordRequirementId
CROSS JOIN cohortusers cu
WHERE rr.TenantDepartmentId = @target_dept_id
  AND cu.CohortId IN (@cohort_ids)
  AND NOT EXISTS (SELECT 1 FROM recordstates rs WHERE rs.UserId = cu.UserId AND rs.RecordCategoryId = rc.Id);
```

### Scenario 5: Requirement Rule Changes

**Given**: A department decides that "Hepatitis B Titer" is no longer required for existing students
**When**: An administrator updates the record category rule to set `Required = false`
**Then**: 
- Existing record states remain unchanged (for audit purposes)
- Compliance calculations no longer consider this category as required
- Students who were non-compliant due to this requirement may become compliant
- New students still see the requirement but it doesn't affect their compliance

**Implementation**:
```csharp
// In RecordCategoryRulesAppService
public async Task UpdateRequirementRule(Guid ruleId, bool isRequired) {
    var rule = await _recordCategoryRuleRepository.GetAsync(ruleId);
    rule.Required = isRequired;
    rule.LastModificationTime = DateTime.Now;
    
    await _recordCategoryRuleRepository.UpdateAsync(rule);
    
    // Recalculate compliance for all affected users
    var affectedUsers = await GetUsersAffectedByRule(ruleId);
    foreach (var userId in affectedUsers) {
        var compliance = await _complianceEvaluator.GetDetailedComplianceValuesForUser(userId);
        // Compliance is automatically recalculated based on current rules
        
        // Optionally notify users if they became compliant
        if (compliance.InCompliance) {
            await _notificationService.NotifyComplianceStatusChange(userId, true);
        }
    }
}
```

### Scenario 6: Bulk Compliance Reporting

**Given**: An administrator needs to generate a compliance report for all students in a cohort
**When**: The report is requested via `CohortUsersAppService.GetAll`
**Then**: 
- Bulk compliance evaluation is performed for efficiency
- Each student's compliance status is calculated for: Drug, Background, Immunization, Overall
- Results are returned with detailed breakdown of compliant/non-compliant categories
- Report can be exported to Excel with full compliance details

**Implementation**:
```csharp
// In CohortUsersAppService.GetAll
public async Task<PagedResultDto<GetCohortUserForViewDto>> GetAll(GetAllCohortUsersInput input) {
    // 1. Get filtered cohort users
    var cohortUsers = await GetFilteredCohortUsers(input);
    
    // 2. Extract user IDs for bulk processing
    var userIds = cohortUsers.Select(cu => cu.UserId).ToList();
    var tenantId = AbpSession.TenantId.Value;
    
    // 3. Bulk evaluate compliance (optimized for performance)
    var bulkComplianceValues = await _surpathComplianceEvaluator
        .GetBulkComplianceValuesForUsers(userIds, tenantId);
    
    // 4. Build result DTOs
    var results = new List<GetCohortUserForViewDto>();
    foreach (var cohortUser in cohortUsers) {
        var result = new GetCohortUserForViewDto {
            CohortUser = MapToCohortUserDto(cohortUser),
            UserEditDto = MapToUserEditDto(cohortUser.User),
            ComplianceValues = bulkComplianceValues.ContainsKey(cohortUser.UserId) 
                ? bulkComplianceValues[cohortUser.UserId] 
                : new ComplianceValues { InCompliance = false }
        };
        results.Add(result);
    }
    
    return new PagedResultDto<GetCohortUserForViewDto>(totalCount, results);
}
```

### Scenario 7: Expiring Requirements Notification

**Given**: A student's immunization record is set to expire in 30 days
**When**: The nightly notification job runs
**Then**: 
- System identifies records approaching expiration based on `RecordCategoryRule.ExpireInDays`
- Notifications are sent based on `WarnDaysBeforeFirst`, `WarnDaysBeforeSecond`, `WarnDaysBeforeFinal`
- Student receives email/in-app notification about upcoming expiration
- Administrators receive summary of students with expiring requirements

**Implementation**:
```csharp
// In ExpirationNotificationJob
public async Task Execute() {
    var today = DateTime.Today;
    
    // Find all record states with expiring requirements
    var expiringRecords = await (
        from rs in _recordStateRepository.GetAll()
        join rc in _recordCategoryRepository.GetAll() on rs.RecordCategoryId equals rc.Id
        join rcr in _recordCategoryRuleRepository.GetAll() on rc.RecordCategoryRuleId equals rcr.Id
        join rec in _recordRepository.GetAll() on rs.RecordId equals rec.Id
        where rcr.Expires == true 
          && rcr.ExpireInDays.HasValue
          && rec.CreationTime.AddDays(rcr.ExpireInDays.Value) > today
          && rec.CreationTime.AddDays(rcr.ExpireInDays.Value) <= today.AddDays(rcr.WarnDaysBeforeFirst ?? 30)
        select new {
            RecordState = rs,
            Rule = rcr,
            ExpirationDate = rec.CreationTime.AddDays(rcr.ExpireInDays.Value),
            DaysUntilExpiration = (rec.CreationTime.AddDays(rcr.ExpireInDays.Value) - today).Days
        }
    ).ToListAsync();
    
    // Group by user and send notifications
    var userGroups = expiringRecords.GroupBy(er => er.RecordState.UserId);
    foreach (var userGroup in userGroups) {
        await _notificationService.SendExpirationWarning(
            userGroup.Key.Value, 
            userGroup.Select(g => new ExpiringRequirement {
                CategoryName = g.RecordState.RecordCategory.Name,
                ExpirationDate = g.ExpirationDate,
                DaysUntilExpiration = g.DaysUntilExpiration
            }).ToList()
        );
    }
}
```

### Scenario 8: Cross-Department Requirement Sharing

**Given**: Both ADN and LVN-RN Bridge departments require "CPR Certification"
**When**: A student transfers from ADN to LVN-RN Bridge
**Then**: 
- System identifies equivalent requirements between departments
- Existing compliant record state is preserved/mapped to new department's requirement
- Student doesn't need to re-upload the same document
- Compliance status is maintained across the transfer

**Implementation**:
```csharp
// In DepartmentTransferService
public async Task TransferStudentBetweenDepartments(long userId, Guid fromDeptId, Guid toDeptId) {
    // 1. Get requirements from both departments
    var fromRequirements = await GetDepartmentRequirements(fromDeptId);
    var toRequirements = await GetDepartmentRequirements(toDeptId);
    
    // 2. Find equivalent requirements by name/description matching
    var equivalentRequirements = from fromReq in fromRequirements
                               join toReq in toRequirements 
                               on fromReq.Name.ToLower() equals toReq.Name.ToLower()
                               select new { FromReq = fromReq, ToReq = toReq };
    
    // 3. Map existing record states to new requirements
    foreach (var equiv in equivalentRequirements) {
        var existingRecordStates = await _recordStateRepository.GetAllListAsync(rs => 
            rs.UserId == userId && 
            rs.RecordCategory.RecordRequirementId == equiv.FromReq.Id);
        
        foreach (var recordState in existingRecordStates) {
            // Find equivalent category in target department
            var targetCategory = await _recordCategoryRepository.FirstOrDefaultAsync(rc => 
                rc.RecordRequirementId == equiv.ToReq.Id && 
                rc.Name.ToLower() == recordState.RecordCategory.Name.ToLower());
            
            if (targetCategory != null) {
                recordState.RecordCategoryId = targetCategory.Id;
                await _recordStateRepository.UpdateAsync(recordState);
            }
        }
    }
    
    // 4. Create record states for new requirements not in source department
    var newRequirements = toRequirements.Where(tr => 
        !equivalentRequirements.Any(er => er.ToReq.Id == tr.Id));
    
    foreach (var newReq in newRequirements) {
        await CreateRecordStatesForRequirement(userId, newReq);
    }
}
```

### Scenario 9: Surpath Service Ordering and Processing

**Given**: A new student needs to order drug test and background check services
**When**: The student completes registration and payment for required services
**Then**: 
- User purchases are created for each service
- Record states are updated from "Not Ordered" to "Ordered"
- External service providers are notified via API
- Student receives confirmation and tracking information

**Implementation**:
```csharp
// In PaymentController.cs
public async Task<IActionResult> ProcessServiceOrder(ServiceOrderDto orderDto) {
    var user = await GetCurrentUser();
    var services = await GetTenantSurpathServices(orderDto.ServiceIds);
    
    // 1. Calculate total cost with any applicable discounts
    var totalCost = services.Sum(s => CalculateServicePrice(s));
    
    // 2. Process payment
    var paymentResult = await _paymentProcessor.ProcessPayment(totalCost, orderDto.PaymentInfo);
    
    if (paymentResult.IsSuccessful) {
        // 3. Create purchase records
        foreach (var service in services) {
            var purchase = new UserPurchase {
                UserId = user.Id,
                TenantSurpathServiceId = service.Id,
                OriginalPrice = service.Price,
                AmountPaid = service.Price,
                Status = PurchaseStatus.Paid,
                PurchaseDate = DateTime.Now
            };
            await _userPurchaseRepository.InsertAsync(purchase);
        }
        
        // 4. Update record states to "Ordered"
        await UpdateServiceRecordStates(user.Id, services, "Ordered");
        
        // 5. Submit orders to external providers
        foreach (var service in services) {
            await SubmitExternalServiceOrder(user, service, paymentResult.TransactionId);
        }
        
        // 6. Send confirmation notifications
        await _notificationService.SendServiceOrderConfirmation(user.Id, services);
    }
    
    return Json(new { success = paymentResult.IsSuccessful, transactionId = paymentResult.TransactionId });
}
```

### Scenario 10: External Service Result Processing

**Given**: A drug test has been completed by the external provider
**When**: The provider sends results via webhook
**Then**: 
- Record state status is automatically updated based on test results
- User compliance is recalculated
- Student and administrators are notified of results
- Any required follow-up actions are triggered

**Implementation**:
```csharp
// Webhook endpoint for external service results
[HttpPost]
[Route("api/services/results")]
public async Task<IActionResult> ProcessServiceResults([FromBody] ExternalServiceResultDto result) {
    try {
        // 1. Validate webhook signature
        if (!await _webhookValidator.ValidateSignature(Request, result)) {
            return Unauthorized("Invalid webhook signature");
        }
        
        // 2. Find the associated record state
        var recordState = await _recordStateRepository.FirstOrDefaultAsync(rs => 
            rs.ExternalOrderId == result.OrderId && rs.IsDeleted == false);
        
        if (recordState == null) {
            return NotFound($"Record state not found for order {result.OrderId}");
        }
        
        // 3. Determine new status based on result
        var newStatusName = DetermineStatusFromResult(result);
        var newStatus = await _recordStatusRepository.FirstOrDefaultAsync(rs => 
            rs.StatusName == newStatusName && rs.TenantId == recordState.TenantId);
        
        // 4. Update record state
        recordState.RecordStatusId = newStatus.Id;
        recordState.Notes = $"Result received: {result.ResultSummary}";
        recordState.LastModificationTime = DateTime.Now;
        await _recordStateRepository.UpdateAsync(recordState);
        
        // 5. Recalculate user compliance
        var compliance = await _complianceEvaluator.GetDetailedComplianceValuesForUser(recordState.UserId.Value);
        
        // 6. Send notifications
        await _notificationService.NotifyServiceResultReceived(recordState.UserId.Value, result, compliance);
        
        // 7. Trigger any required follow-up actions
        if (result.RequiresFollowUp) {
            await TriggerFollowUpActions(recordState, result);
        }
        
        return Ok(new { message = "Result processed successfully" });
    }
    catch (Exception ex) {
        Logger.Error($"Error processing service result: {ex.Message}", ex);
        return StatusCode(500, "Internal server error");
    }
}

private string DetermineStatusFromResult(ExternalServiceResultDto result) {
    return result.ServiceType.ToLower() switch {
        "drug_test" => result.IsPassed ? "Compliant" : "Non-Compliant",
        "background_check" => result.IsPassed ? "Compliant" : "Requires Review",
        _ => "Pending Review"
    };
}
```

### Scenario 11: Service Scoping and Priority Resolution

**Given**: A student belongs to multiple organizational levels (tenant, department, cohort)
**When**: The system determines which services apply to the student
**Then**: 
- Services are resolved based on hierarchical priority (user > cohort > department > tenant)
- Most specific service configuration takes precedence
- Pricing reflects the most applicable service level
- Student sees only the services that apply to their specific context

**Implementation**:
```csharp
// Service resolution with priority logic
public async Task<List<TenantSurpathService>> GetApplicableServicesForUser(long userId) {
    // 1. Get user's organizational memberships
    var userMemberships = await GetUserMemberships(userId);
    var userDepartments = userMemberships.Select(m => m.TenantDepartmentId).Where(id => id.HasValue);
    var userCohorts = userMemberships.Select(m => m.CohortId).Where(id => id.HasValue);
    
    // 2. Get all potential services for this tenant
    var allTenantServices = await _tenantSurpathServiceRepository.GetAllListAsync(ts => 
        ts.TenantId == AbpSession.TenantId && ts.IsEnabled && !ts.IsDeleted);
    
    // 3. Group services by SurpathServiceId and apply priority resolution
    var applicableServices = new List<TenantSurpathService>();
    var serviceGroups = allTenantServices.GroupBy(s => s.SurpathServiceId);
    
    foreach (var serviceGroup in serviceGroups) {
        var prioritizedService = ResolvePriorityService(serviceGroup.ToList(), userId, userCohorts, userDepartments);
        if (prioritizedService != null) {
            applicableServices.Add(prioritizedService);
        }
    }
    
    return applicableServices;
}

private TenantSurpathService ResolvePriorityService(
    List<TenantSurpathService> services, 
    long userId, 
    IEnumerable<Guid?> userCohorts, 
    IEnumerable<Guid?> userDepartments) {
    
    // Priority 1: User-specific service
    var userSpecific = services.FirstOrDefault(s => s.UserId == userId);
    if (userSpecific != null) return userSpecific;
    
    // Priority 2: Cohort-specific service
    var cohortSpecific = services.FirstOrDefault(s => 
        s.CohortId.HasValue && userCohorts.Contains(s.CohortId));
    if (cohortSpecific != null) return cohortSpecific;
    
    // Priority 3: Department-specific service
    var departmentSpecific = services.FirstOrDefault(s => 
        s.TenantDepartmentId.HasValue && userDepartments.Contains(s.TenantDepartmentId));
    if (departmentSpecific != null) return departmentSpecific;
    
    // Priority 4: Tenant-wide service
    return services.FirstOrDefault(s => 
        !s.UserId.HasValue && !s.CohortId.HasValue && !s.TenantDepartmentId.HasValue);
}
```

### Scenario 12: Service Migration and Compliance Preservation

**Given**: A tenant wants to migrate immunization requirements to Surpath services
**When**: The migration process is initiated
**Then**: 
- Existing compliance statuses are preserved during migration
- New service-based requirements are created
- Users maintain their current compliance levels
- Document-based fallback remains available during transition

**Implementation**:
```csharp
// Immunization service migration process
public async Task MigrateImmunizationToSurpathService(int tenantId, Guid targetDepartmentId) {
    using (var uow = _unitOfWorkManager.Begin()) {
        try {
            // 1. Create tenant-specific immunization service
            var systemImmunizationService = await GetSystemImmunizationService();
            var tenantService = new TenantSurpathService {
                TenantId = tenantId,
                SurpathServiceId = systemImmunizationService.Id,
                TenantDepartmentId = targetDepartmentId,
                Name = "Immunization Verification",
                Description = "Automated immunization record verification",
                Price = 5.00,
                IsEnabled = true
            };
            await _tenantSurpathServiceRepository.InsertAsync(tenantService);
            
            // 2. Get existing immunization requirements
            var existingRequirements = await _recordRequirementRepository.GetAllListAsync(rr => 
                rr.TenantId == tenantId && 
                rr.TenantDepartmentId == targetDepartmentId &&
                !rr.IsSurpathOnly &&
                rr.Name.ToLower().Contains("immunization"));
            
            // 3. Migrate each requirement while preserving compliance
            foreach (var requirement in existingRequirements) {
                await MigrateRequirementToService(requirement, tenantService);
            }
            
            // 4. Update existing record states to maintain compliance
            await PreserveExistingCompliance(existingRequirements, tenantService);
            
            await uow.CompleteAsync();
            
            // 5. Notify administrators of successful migration
            await _notificationService.NotifyServiceMigrationComplete(tenantId, targetDepartmentId);
        }
        catch (Exception ex) {
            await uow.RollbackAsync();
            Logger.Error($"Immunization service migration failed: {ex.Message}", ex);
            throw;
        }
    }
}

private async Task MigrateRequirementToService(RecordRequirement requirement, TenantSurpathService service) {
    // 1. Update requirement to be Surpath-only
    requirement.IsSurpathOnly = true;
    requirement.TenantSurpathServiceId = service.Id;
    requirement.SurpathServiceId = service.SurpathServiceId;
    await _recordRequirementRepository.UpdateAsync(requirement);
    
    // 2. Update associated categories to link to service rules
    var categories = await _recordCategoryRepository.GetAllListAsync(rc => 
        rc.RecordRequirementId == requirement.Id);
    
    foreach (var category in categories) {
        // Link to service-specific compliance rules
        category.RecordCategoryRuleId = service.RecordCategoryRuleId;
        await _recordCategoryRepository.UpdateAsync(category);
    }
}

private async Task PreserveExistingCompliance(List<RecordRequirement> requirements, TenantSurpathService service) {
    foreach (var requirement in requirements) {
        var categories = await _recordCategoryRepository.GetAllListAsync(rc => 
            rc.RecordRequirementId == requirement.Id);
        
        foreach (var category in categories) {
            var existingStates = await _recordStateRepository.GetAllListAsync(rs => 
                rs.RecordCategoryId == category.Id && !rs.IsDeleted);
            
            foreach (var state in existingStates) {
                // Preserve existing compliance status
                // If user was compliant, they remain compliant in the new service model
                if (await IsCurrentlyCompliant(state)) {
                    var compliantStatus = await GetCompliantStatusForService(service);
                    state.RecordStatusId = compliantStatus.Id;
                    state.Notes += $"\n[Migration] Compliance preserved from document-based system";
                    await _recordStateRepository.UpdateAsync(state);
                }
            }
        }
    }
}
```

This comprehensive documentation provides a complete understanding of the Surpath compliance system's architecture, relationships, and operational logic, with special emphasis on the Surpath services integration. The Given-When-Then scenarios demonstrate real-world usage patterns and implementation approaches for common compliance management tasks, including the sophisticated service management capabilities that enable both system-wide standardization and tenant-specific customization. 