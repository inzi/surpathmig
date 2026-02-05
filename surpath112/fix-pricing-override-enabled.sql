-- Fix for $0.00 Pricing Bug
-- This script identifies records that may have been affected by the pricing bug
-- where IsPricingOverrideEnabled was not set when prices were updated.
--
-- IMPORTANT: DO NOT run this automatically. Review the results first!
-- Some records may have been intentionally disabled by administrators.
--
-- Background: When a price was changed, the IsPricingOverrideEnabled flag was not being set to true,
-- causing the hierarchical pricing manager to exclude these services from pricing calculations,
-- resulting in students being charged $0.00 instead of the correct price.

-- Step 1: Find potentially affected records
-- These are records with prices but IsPricingOverrideEnabled = 0
SELECT
    Id,
    TenantId,
    Name,
    Price,
    IsPricingOverrideEnabled,
    IsInvoiced,
    SurpathServiceId,
    TenantDepartmentId,
    CohortId,
    UserId,
    LastModificationTime,
    CreationTime
FROM TenantSurpathServices
WHERE IsDeleted = 0
  AND Price > 0
  AND IsPricingOverrideEnabled = 0
ORDER BY LastModificationTime DESC;

-- Step 2: Review the results above and determine which records should be fixed
-- Records that were recently modified (after the bug was introduced) likely need fixing
-- Records that are old may have been intentionally disabled

-- Step 3: ONLY IF you've verified the records should be enabled, run this update:
-- UNCOMMENT THE FOLLOWING LINES AFTER REVIEW:

/*
UPDATE TenantSurpathServices
SET IsPricingOverrideEnabled = 1,
    LastModificationTime = UTC_TIMESTAMP(),
    LastModifierUserId = NULL  -- Or set to your admin user ID
WHERE IsDeleted = 0
  AND Price > 0
  AND IsPricingOverrideEnabled = 0
  AND LastModificationTime > '2025-09-01';  -- Adjust this date to when the bug was introduced
*/

-- Step 4: Verify the fix (run after update)
/*
SELECT
    COUNT(*) as RecordsWithPricingEnabled,
    SUM(Price) as TotalPricing
FROM TenantSurpathServices
WHERE IsDeleted = 0
  AND Price > 0
  AND IsPricingOverrideEnabled = 1;
*/
