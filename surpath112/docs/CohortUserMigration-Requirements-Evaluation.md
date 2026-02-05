# Cohort User Migration - Requirements Evaluation Documentation

## Overview
When migrating users between cohorts, the system must carefully evaluate which RecordRequirements and RecordCategories need to be considered. This document explains how the system should determine what requirements apply to users in their current cohort versus their target cohort.

## Key Principle
During cohort user migration, the system should **only evaluate the differential** between:
1. Requirements that apply to the user in their **current cohort**
2. Requirements that would apply to the user in the **target cohort**

## Requirement Hierarchy

The system uses a hierarchical approach to determine which requirements apply to a user:

### 1. Tenant-Level Requirements
- Apply to **all users** in the tenant
- No department or cohort restrictions
- These requirements remain the same regardless of cohort transfer

### 2. Department-Level Requirements
- Apply to all users in a specific department
- Inherited by cohorts within that department
- May change if source and target cohorts are in different departments

### 3. Cohort-Level Requirements
- Apply only to users in a specific cohort
- Do not have department restrictions
- Will definitely change during cohort transfer

### 4. Cohort+Department Requirements
- Apply to users in a specific cohort AND department combination
- Most specific level of requirement
- Will change if either cohort or department changes

## Migration Logic

### Current Implementation (from UserTransferAppService)

```csharp
// Get all requirements for the target
var targetCategories = await _surpathComplianceEvaluator.GetHierarchicalRequirementCategories(
    departmentId: targetDepartment.Id,
    cohortId: targetCohort.Id,
    includeInherited: true
);

// Separate categories that exist at both source and target
noTransferRequiredCategories = requirementCategories
    .Where(rc => targetRequirementIds.Contains(rc.RequirementId))
    .ToList();

// Categories that need explicit mapping
categoriesToMap = requirementCategories
    .Where(rc => !targetRequirementIds.Contains(rc.RequirementId))
    .ToList();
```

### Recommended Approach for Differential Evaluation

#### Step 1: Identify Source Requirements
Get all requirements that apply to the user in their current cohort:
- Tenant-level requirements
- Department-level requirements (if cohort has department)
- Cohort-specific requirements
- Cohort+Department combination requirements

#### Step 2: Identify Target Requirements
Get all requirements that would apply to the user in the target cohort:
- Tenant-level requirements (same as source)
- Department-level requirements (may differ if different department)
- Target cohort-specific requirements
- Target cohort+Department combination requirements

#### Step 3: Calculate Differential

**Requirements that don't need migration:**
- Requirements that exist in **both** source and target (same RequirementId)
- User's existing records for these requirements remain valid

**Requirements that need handling:**
1. **Lost Requirements**: Exist in source but not in target
   - User records become orphaned
   - May need archiving or cleanup
   
2. **New Requirements**: Exist in target but not in source
   - User needs to fulfill these new requirements
   - No existing records to migrate

3. **Mapped Requirements**: Similar requirements with different IDs
   - Requires category mapping during migration
   - User's existing records need to be reassigned

## Implementation Recommendations

### 1. Enhance Analysis Phase
```csharp
public async Task<UserTransferAnalysisDto> AnalyzeSelectiveUserTransfer(AnalyzeUserTransferInput input)
{
    // Get source requirements
    var sourceRequirements = await GetRequirementsForCohort(
        sourceCohort.Id, 
        sourceCohort.TenantDepartmentId
    );
    
    // Get target requirements
    var targetRequirements = await GetRequirementsForCohort(
        targetCohort.Id, 
        targetCohort.TenantDepartmentId
    );
    
    // Calculate differential
    var analysis = new RequirementsDifferential
    {
        CommonRequirements = sourceRequirements.Intersect(targetRequirements),
        LostRequirements = sourceRequirements.Except(targetRequirements),
        NewRequirements = targetRequirements.Except(sourceRequirements)
    };
}
```

### 2. Category Mapping Strategy
When departments differ, the system should:
1. Automatically match categories with identical names/types
2. Allow manual mapping for similar but non-identical categories
3. Flag categories that have no equivalent in the target

### 3. Record State Handling
```csharp
// Only update record states for categories that need mapping
if (requiresCategoryMapping && input.CategoryMappings != null)
{
    foreach (var recordState in userRecordStates)
    {
        // Only process if this category needs mapping
        if (input.CategoryMappings.ContainsKey(recordState.RecordCategoryId.Value))
        {
            recordState.RecordCategoryId = input.CategoryMappings[recordState.RecordCategoryId.Value];
        }
        // Records for common requirements remain unchanged
    }
}
```

### 4. Compliance Recalculation
After migration:
1. Recalculate compliance based on new requirement set
2. Consider grace period for new requirements
3. Maintain historical compliance data for audit

## Special Considerations

### 1. Same Department Transfers
When source and target cohorts are in the same department:
- Department-level requirements remain unchanged
- Only cohort-specific requirements change
- Simpler migration process

### 2. Cross-Department Transfers
When moving between departments:
- Department-level requirements change
- May require extensive category mapping
- Higher complexity and risk

### 3. Surpath-Only Requirements
Requirements marked as `IsSurpathOnly`:
- May have special handling rules
- Could be excluded from certain migrations
- Need explicit business rule validation

## Benefits of Differential Approach

1. **Efficiency**: Only processes requirements that actually change
2. **Data Integrity**: Preserves valid records for unchanged requirements
3. **User Experience**: Minimizes disruption to compliant users
4. **Audit Trail**: Clear tracking of what changed during migration
5. **Flexibility**: Allows for intelligent mapping of similar requirements

## Example Scenario

**User Migration: Nursing Cohort (Hospital A) → Nursing Cohort (Hospital B)**

Source Requirements:
- Background Check (Tenant-level) ✓
- Medical License (Department-level: Healthcare) ✓
- Hospital A Orientation (Cohort-specific) ✓
- Hospital A Parking Permit (Cohort+Department) ✓

Target Requirements:
- Background Check (Tenant-level) ✓
- Medical License (Department-level: Healthcare) ✓
- Hospital B Orientation (Cohort-specific) ✗
- Hospital B Parking Permit (Cohort+Department) ✗

Migration Result:
- Background Check: No change needed (same requirement)
- Medical License: No change needed (same department)
- Hospital A Orientation: Map to Hospital B Orientation
- Hospital A Parking: Archive (no equivalent)
- Hospital B Parking: New requirement (user must complete)

## Conclusion

By focusing on the differential between source and target requirements, the cohort user migration process becomes more efficient, maintains data integrity, and provides a better user experience. The system should clearly identify which requirements remain valid, which need mapping, and which are new obligations for the migrated users.