# Fix: Cohort-Only Requirements Not Showing for Cohort Users

## Issue Summary
When a requirement/category is assigned directly to a cohort without specifying a department, users in that cohort don't see the requirement when calling `GetRecordStateComplianceForUserId`.

## Root Cause
The issue was in the `GetUserDeptAndCohortMembership` method in `SurpathComplianceEvaluator.cs`. The original query was joining through departments first, which meant it couldn't find cohorts that don't have a department assigned.

## The Problem Query
The original query structure was:
```csharp
from td in _TenantDepartmentRepository...
join tdu in _TenantDepartmentUserRepository...
join co in _CohortRepository on td.Id equals co.TenantDepartmentId...
join cu in _CohortUserRepository...
```

This approach only found cohorts that were associated with departments, missing standalone cohorts.

## Solution Applied
Changed `GetUserDeptAndCohortMembership` to use two separate queries:

1. **Department Memberships**: Get all departments the user belongs to directly
2. **Cohort Memberships**: Get all cohorts the user belongs to (with or without departments)

```csharp
// Get department memberships
var deptMemberships = from td in _TenantDepartmentRepository...
                      join tdu in _TenantDepartmentUserRepository...
                      
// Get cohort memberships (including cohorts without departments)
var cohortMemberships = from cu in _CohortUserRepository.Where(_cu => _cu.UserId == _userId)
                        join co in _CohortRepository...
                        join td in _TenantDepartmentRepository... into tdj
                        from td in tdj.DefaultIfEmpty()  // LEFT JOIN to include cohorts without departments

// Union the results
var _q = deptMemberships.Union(cohortMemberships);
```

## How the System Works

### SetTenantRequirements Method
- Retrieves ALL requirements for a tenant and caches them
- Includes cohort-only requirements without filtering by user

### GetComplianceInfo Method
- Calls `GetUserDeptAndCohortMembership` to find user's memberships
- Filters the cached requirements based on user's actual cohort/department memberships

### The Fix
By fixing `GetUserDeptAndCohortMembership` to properly find cohorts without departments, the rest of the system now works correctly to show cohort-only requirements.

## Testing
With the debug logging added, you should see:
1. The user's cohort membership being detected
2. Cohort-only requirements being matched to the user's cohort
3. These requirements appearing in the compliance results

## How Requirements Work Now

The system supports four types of requirement assignments:

1. **Tenant-level**: Requirements with no cohort and no department assigned - apply to ALL users in the tenant
2. **Department-level**: Requirements assigned to a department but no cohort - apply to all users in that department
3. **Cohort-level**: Requirements assigned to a cohort but no department - apply to all users in that specific cohort
4. **Cohort+Department**: Requirements assigned to both a cohort AND department - apply only to users in that specific cohort when it's in that department

## Testing Recommendations

To verify the fix works correctly:

1. Create a requirement/category assigned only to a cohort (no department)
2. Add a user to that cohort
3. Call `GetRecordStateComplianceForUserId` for that user
4. Verify the cohort-specific requirement appears in the results

## Files Modified
- `/src/inzibackend.Application/Surpath/ComplianceManager/SurpathComplianceEvaluator.cs`
  - Line 900: Fixed cohort join condition
  - Lines 909-918: Fixed WHERE clause logic for requirement filtering