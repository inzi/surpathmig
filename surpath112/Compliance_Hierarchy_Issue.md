# Compliance Hierarchy Issue with Disabled TenantSurpathServices

## Current Behavior
The compliance system checks if a RecordRequirement's linked TenantSurpathService is enabled, but doesn't consider hierarchical inheritance.

## Issue
When a RecordRequirement is linked to a specific TenantSurpathService at a particular level (e.g., department), the compliance check only looks at that specific service's IsEnabled status. It doesn't check if there's an enabled service at a higher level (e.g., tenant-wide) that should apply.

### Example Scenario
1. Drug Screening service enabled at Tenant level (IsEnabled = true)
2. Drug Screening service disabled at Department level (IsEnabled = false)
3. RecordRequirement linked to the Department-level TenantSurpathService
4. Current behavior: Requirement excluded from compliance (incorrect)
5. Expected behavior: Requirement included because tenant-level service is enabled

## Current Implementation
In `SurpathComplianceEvaluator.SetTenantRequirements`:
```csharp
&& (tss == null || (tss.IsDeleted == false && tss.IsEnabled == true && tss != null))
```

This only checks the specific linked TenantSurpathService, not the hierarchy.

## Recommended Solution
The compliance logic needs to check the effective enabled status through the hierarchy:
1. If the linked TenantSurpathService is enabled, include the requirement
2. If the linked TenantSurpathService is disabled, check higher levels:
   - For department-level: check tenant-level
   - For cohort-level: check department-level and tenant-level
   - For user-level: check cohort-level, department-level, and tenant-level
3. Include the requirement if ANY level in the hierarchy has an enabled service

## Impact
This ensures compliance requirements are properly evaluated based on the hierarchical nature of service enablement, matching the behavior of pricing calculations.