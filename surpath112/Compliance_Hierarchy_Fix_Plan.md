# Compliance Hierarchy Fix Plan

## Understanding the Current State

### TenantSurpathService Hierarchy
TenantSurpathService can be assigned at multiple levels:
1. **Tenant-wide**: No TenantDepartmentId, CohortId, or UserId
2. **Department level**: Has TenantDepartmentId
3. **Cohort level**: Has CohortId (may also have TenantDepartmentId)
4. **User level**: Has UserId or CohortUserId

### RecordRequirement Assignment
RecordRequirements can be assigned at:
1. **Tenant-wide**: No TenantDepartmentId or CohortId
2. **Department level**: Has TenantDepartmentId
3. **Cohort level**: Has CohortId (may have TenantDepartmentId)

### Current Issue
When a RecordRequirement is linked to a disabled TenantSurpathService, it's excluded from compliance even if there's an enabled service at a higher level.

## Proposed Solution

### Step 1: Create Helper Method to Check Hierarchical Service Status
Create a method that checks if a service is effectively enabled through the hierarchy:

```csharp
private async Task<bool> IsServiceEffectivelyEnabled(
    Guid surpathServiceId, 
    int tenantId,
    Guid? departmentId = null,
    Guid? cohortId = null,
    long? userId = null)
{
    // Check from most specific to least specific level
    // 1. User level
    // 2. Cohort level  
    // 3. Department level
    // 4. Tenant level
    
    // If any enabled service exists at any applicable level, return true
}
```

### Step 2: Modify SetTenantRequirements Query
Instead of just checking the linked TenantSurpathService:
```csharp
&& (tss == null || (tss.IsDeleted == false && tss.IsEnabled == true && tss != null))
```

We need to:
1. Get the SurpathServiceId from the linked TenantSurpathService
2. Check if the service is effectively enabled at any level that applies to the requirement

### Step 3: Consider Requirement Assignment Level
- If requirement is at cohort level: check cohort → department → tenant
- If requirement is at department level: check department → tenant
- If requirement is at tenant level: check tenant only

## Implementation Approach

### Option 1: Modify the LINQ Query (Complex)
Modify the SetTenantRequirements query to include hierarchical checks. This would be complex and potentially impact performance.

### Option 2: Post-Process Filter (Recommended)
1. Load all requirements initially (including those with disabled services)
2. Post-process to check hierarchical enablement
3. Filter out only those that have no enabled service at any applicable level

### Option 3: Separate Method for Hierarchical Requirements
Create a new method that properly handles hierarchical service enablement, keeping the existing method for backward compatibility.

## Risk Mitigation

1. **Performance**: The hierarchical check could impact performance. Consider:
   - Caching service enablement status
   - Batch loading all TenantSurpathServices once
   - Using efficient queries

2. **Testing**: Need comprehensive tests for:
   - Service enabled at tenant, disabled at department
   - Service disabled at all levels
   - Multiple services at different levels
   - Requirements at different assignment levels

3. **Backward Compatibility**: Ensure existing compliance calculations continue to work

## Implementation Steps

1. Write unit tests for expected behavior
2. Create helper method for hierarchical service check
3. Modify SetTenantRequirements to use new logic
4. Test with various scenarios
5. Update documentation