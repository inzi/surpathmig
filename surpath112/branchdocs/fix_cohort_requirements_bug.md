# Fix: Cohort Requirements Not Appearing in User Views and Compliance Calculations

## Issue Description
Cohort-assigned requirements were not appearing when viewing users and were not being counted in compliance calculations.

## Root Cause Analysis
Two critical bugs in `SurpathComplianceEvaluator.cs` in the `GetComplianceInfo` method:

### Bug 1: Impossible Logic Condition
**File:** `src/inzibackend.Application/Surpath/ComplianceManager/SurpathComplianceEvaluator.cs`  
**Line:** 661  
**Issue:** Logic checking for `CohortId != null && CohortId == null` which is always false

```csharp
// BROKEN CODE:
var _requirementsForCohort = TenantRequirements.Where(_r => 
    _r.RecordRequirement.CohortId != null && 
    _r.RecordRequirement.CohortId == null).ToList();
```

### Bug 2: Missing AddRange Call
**File:** `src/inzibackend.Application/Surpath/ComplianceManager/SurpathComplianceEvaluator.cs`  
**Line:** 653  
**Issue:** Cohort+Department requirements calculated but never added to user's requirements list

## The Fix

### Fixed Logic for Cohort-Specific Requirements
```csharp
// FIXED CODE:
var _requirementsForCohort = TenantRequirements.Where(_r => 
    _r.RecordRequirement.CohortId != null && 
    _r.RecordRequirement.TenantDepartmentId == null && 
    _userCohort != null && 
    _userCohort.CohortId == _r.RecordRequirement.CohortId).ToList();
```

### Added Missing AddRange for Cohort+Department Requirements
```csharp
var _requirementsForCohortAndDept = TenantRequirements
    .Where(_r => _r.RecordRequirement != null
                 && _r.RecordRequirement.CohortId != null
                 && _r.RecordRequirement.TenantDepartmentId != null
                 && _userDeptList.Contains(_r.RecordRequirement.TenantDepartmentId)
                 && _userCohort != null
                 && _userCohort.CohortId == _r.RecordRequirement.CohortId)
    .ToList();
_requirementsForUser.AddRange(_requirementsForCohortAndDept); // ADDED THIS LINE
```

## Impact
- ✅ Cohort-assigned requirements now appear when viewing users
- ✅ Cohort requirements are now properly counted in compliance calculations
- ✅ Both cohort-only and cohort+department requirements work correctly

## Requirements Logic Summary
The system now correctly handles all requirement assignment scenarios:

1. **Tenant-wide requirements:** Apply to all users (`CohortId == null && TenantDepartmentId == null`)
2. **Department requirements:** Apply to all users in a department (`CohortId == null && TenantDepartmentId != null`)
3. **Cohort requirements:** Apply to users in a specific cohort (`CohortId != null && TenantDepartmentId == null`)
4. **Cohort+Department requirements:** Apply to users in a specific cohort within a specific department (`CohortId != null && TenantDepartmentId != null`)

## Testing Recommendations
1. Assign a requirement to a cohort
2. Verify it appears in user views for cohort members
3. Verify compliance calculations include the cohort requirement
4. Test all four requirement assignment scenarios

## Modified Files
- `src/inzibackend.Application/Surpath/ComplianceManager/SurpathComplianceEvaluator.cs`
- `branchdocs/fix_cohort_requirements_bug.md` 