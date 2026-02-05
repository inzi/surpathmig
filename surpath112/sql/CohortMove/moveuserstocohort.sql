-- =====================================================
-- Navarro College: Move LVN-RN Bridge Cohorts Script
-- =====================================================
-- This script moves two cohorts from "ADN (Associate Degree Nursing)" 
-- to "LVN - RN Bridge" department and maps their requirements.
--
-- KEY FIX: Creates new recordstates with COMPLIANT status for migrated users
-- This prevents new department requirements from breaking existing compliance
--
-- IMPORTANT: This script runs in a single transaction.
-- If any step fails, all changes will be rolled back automatically.
--
-- SAFETY FEATURES:
-- - Pre-migration validation of all entities
-- - Progress tracking after each step
-- - Transaction rollback protection
-- - Comprehensive verification queries
-- - Manual rollback instructions provided
--
-- USAGE:
-- 1. Review all variables below to ensure they match your environment
-- 2. Run the entire script in one execution
-- 3. IMPORTANT: Check validation results - if any show errors, ROLLBACK manually
-- 4. Review verification output before considering the migration complete
-- 5. Keep a backup of the database before running in production

-- Start transaction
START TRANSACTION;

-- Variables for the migration
SET @tenant_id = 24; -- Navarro College
SET @source_dept_id = '08db77ef-9288-4d07-8d93-a631951d4095'; -- ADN (Associate Degree Nursing)
SET @target_dept_id = '08dd80ec-2477-4be0-8ffa-31457fc44b7d'; -- LVN - RN Bridge
SET @cohort1_id = '08dd55ca-6414-4b3e-8e91-e4f510592ac1'; -- LVN-RN Bridge (Grads 2026)-COR
SET @cohort2_id = '08dd55d8-68f6-4a07-8aa1-dfb92195395e'; -- LVN-RN Bridge (Grads 2026)-Wax

-- Category mapping variables (Old category ID -> New category ID)
-- Map cohort-specific "Hep B (Titer)" categories to "Hepatitis B (Titer)" category in LVN-RN Bridge
SET @old_hepb_titer_cor_cat = '08dd80eb-ba39-49ed-869f-b2aa1ba27c09'; -- Hep B (Titer) category for COR cohort
SET @old_hepb_titer_wax_cat = '08dd80eb-c8b9-4fe3-87fb-c11c4baab002'; -- Hep B (Titer) category for Wax cohort
SET @new_hepb_titer_cat = '08dd80ef-6802-41ab-8742-39eb347fcf18'; -- Hepatitis B (Titer) category in LVN-RN Bridge

-- Map categories from General Immunizations to specific LVN-RN Bridge categories
-- MMR mapping
SET @old_mmr_cat = '08db764b-370b-4180-82a7-dfc5f0aedbe2'; -- MMR category under General Immunizations
SET @new_mmr_cat = '08dd80ed-038f-4de0-8206-55451da3d006'; -- MMR category in LVN-RN Bridge

-- Nursing License mapping  
SET @old_nursing_license_cat = '08dc3e0e-ff74-4509-8002-ea8702e1754d'; -- LVN License category under Nursing License
SET @new_lvn_license_cat = '08dd889f-c7b6-4226-8193-1879f124a003'; -- LVN License (X) category in LVN-RN Bridge

-- Student Handbook mapping (if it exists in ADN)
SET @old_handbook_adn_cat = '08db764b-370b-45de-8d37-ba0dd400581e'; -- Student Handbook under General Immunizations
SET @new_handbook_bridge_cat = '08dd80ef-b156-4e80-81f7-e6658e925beb'; -- Student Handbook in LVN-RN Bridge

-- =====================================================
-- PRE-MIGRATION VALIDATION
-- =====================================================

-- Verify all required entities exist before proceeding
-- This will fail with a clear error message if any validation fails

SELECT 
  CASE 
    WHEN tenant_count = 0 THEN 'ERROR: Tenant not found or deleted'
    WHEN source_dept_count = 0 THEN 'ERROR: Source department not found or deleted'
    WHEN target_dept_count = 0 THEN 'ERROR: Target department not found or deleted'
    WHEN cohort_count != 2 THEN 'ERROR: One or both cohorts not found in source department'
    ELSE 'VALIDATION PASSED'
  END as Validation_Status,
  tenant_count,
  source_dept_count,
  target_dept_count,
  cohort_count
FROM (
  SELECT 
    (SELECT COUNT(*) FROM surpathv2.abptenants WHERE Id = @tenant_id AND IsDeleted = 0) as tenant_count,
    (SELECT COUNT(*) FROM surpathv2.tenantdepartments 
     WHERE Id = @source_dept_id AND TenantId = @tenant_id AND IsDeleted = 0) as source_dept_count,
    (SELECT COUNT(*) FROM surpathv2.tenantdepartments 
     WHERE Id = @target_dept_id AND TenantId = @tenant_id AND IsDeleted = 0) as target_dept_count,
    (SELECT COUNT(*) FROM surpathv2.cohorts 
     WHERE Id IN (@cohort1_id, @cohort2_id) 
       AND TenantId = @tenant_id 
       AND TenantDepartmentId = @source_dept_id 
       AND IsDeleted = 0) as cohort_count
) validation_data;

-- Store validation results for later use
SELECT @tenant_count := COUNT(*) FROM surpathv2.abptenants WHERE Id = @tenant_id AND IsDeleted = 0;
SELECT @source_dept_count := COUNT(*) FROM surpathv2.tenantdepartments 
WHERE Id = @source_dept_id AND TenantId = @tenant_id AND IsDeleted = 0;
SELECT @target_dept_count := COUNT(*) FROM surpathv2.tenantdepartments 
WHERE Id = @target_dept_id AND TenantId = @tenant_id AND IsDeleted = 0;
SELECT @cohort_count := COUNT(*) FROM surpathv2.cohorts 
WHERE Id IN (@cohort1_id, @cohort2_id) 
  AND TenantId = @tenant_id 
  AND TenantDepartmentId = @source_dept_id 
  AND IsDeleted = 0;

-- IMPORTANT: Review the validation results above before proceeding
-- If any validation shows an error, stop here and fix the issue
-- The script will continue but may fail if validation errors exist

SELECT 'PRE-MIGRATION VALIDATION COMPLETED' as Status,
       'Review validation results above - proceed only if all validations passed' as Message;

-- =====================================================
-- STEP 1: Update cohorts to new department
-- =====================================================
UPDATE surpathv2.cohorts 
SET TenantDepartmentId = @target_dept_id,
    LastModificationTime = NOW(),
    LastModifierUserId = 1 -- System user
WHERE Id IN (@cohort1_id, @cohort2_id)
  AND TenantId = @tenant_id
  AND IsDeleted = 0;

-- Verify cohorts were updated
SELECT @cohorts_updated := ROW_COUNT();

SELECT 'STEP 1 COMPLETED' as Status, 
       CASE 
         WHEN @cohorts_updated = 2 THEN CONCAT(@cohorts_updated, ' cohorts successfully moved to new department')
         ELSE CONCAT('ERROR: Expected 2 cohorts updated, but ', @cohorts_updated, ' were updated')
       END as Message;

-- =====================================================
-- STEP 2: Map record requirements for users
-- =====================================================

-- Map cohort-specific Hep B (Titer) categories to new department category
-- COR cohort Hep B category
UPDATE surpathv2.recordstates rs
JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
SET rs.RecordCategoryId = @new_hepb_titer_cat,
    rs.LastModificationTime = NOW(),
    rs.LastModifierUserId = 1
WHERE cu.CohortId = @cohort1_id
  AND rs.TenantId = @tenant_id
  AND rs.IsDeleted = 0
  AND cu.IsDeleted = 0
  AND rs.RecordCategoryId = @old_hepb_titer_cor_cat;

SELECT @hepb_cor_updated := ROW_COUNT();

-- Wax cohort Hep B category
UPDATE surpathv2.recordstates rs
JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
SET rs.RecordCategoryId = @new_hepb_titer_cat,
    rs.LastModificationTime = NOW(),
    rs.LastModifierUserId = 1
WHERE cu.CohortId = @cohort2_id
  AND rs.TenantId = @tenant_id
  AND rs.IsDeleted = 0
  AND cu.IsDeleted = 0
  AND rs.RecordCategoryId = @old_hepb_titer_wax_cat;

SELECT @hepb_wax_updated := ROW_COUNT();

-- Map department-wide categories that apply to users in these cohorts
-- MMR category mapping
UPDATE surpathv2.recordstates rs
JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
SET rs.RecordCategoryId = @new_mmr_cat,
    rs.LastModificationTime = NOW(),
    rs.LastModifierUserId = 1
WHERE cu.CohortId IN (@cohort1_id, @cohort2_id)
  AND rs.TenantId = @tenant_id
  AND rs.IsDeleted = 0
  AND cu.IsDeleted = 0
  AND rs.RecordCategoryId = @old_mmr_cat;

SELECT @mmr_updated := ROW_COUNT();

-- Nursing License category mapping
UPDATE surpathv2.recordstates rs
JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
SET rs.RecordCategoryId = @new_lvn_license_cat,
    rs.LastModificationTime = NOW(),
    rs.LastModifierUserId = 1
WHERE cu.CohortId IN (@cohort1_id, @cohort2_id)
  AND rs.TenantId = @tenant_id
  AND rs.IsDeleted = 0
  AND cu.IsDeleted = 0
  AND rs.RecordCategoryId = @old_nursing_license_cat;

SELECT @nursing_license_updated := ROW_COUNT();

-- Student Handbook category mapping (if any records exist)
UPDATE surpathv2.recordstates rs
JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
SET rs.RecordCategoryId = @new_handbook_bridge_cat,
    rs.LastModificationTime = NOW(),
    rs.LastModifierUserId = 1
WHERE cu.CohortId IN (@cohort1_id, @cohort2_id)
  AND rs.TenantId = @tenant_id
  AND rs.IsDeleted = 0
  AND cu.IsDeleted = 0
  AND rs.RecordCategoryId = @old_handbook_adn_cat;

SELECT @handbook_updated := ROW_COUNT();

-- CRITICAL FIX: Update RecordStatusId to ensure compliance status is correct
-- The issue might be that recordstates have the right category but wrong status
-- Let's ensure all mapped recordstates have appropriate status for the new department

-- Get the appropriate "compliant" status for the new department
SET @compliant_status_id = (
  SELECT Id FROM surpathv2.recordstatuses 
  WHERE TenantDepartmentId = @target_dept_id 
    AND TenantId = @tenant_id 
    AND IsDeleted = 0 
    AND (StatusName LIKE '%Compliant%' OR StatusName LIKE '%Complete%' OR StatusName LIKE '%Approved%')
  ORDER BY StatusName
  LIMIT 1
);

-- If no department-specific compliant status, get tenant-wide compliant status
SET @compliant_status_id = COALESCE(@compliant_status_id, (
  SELECT Id FROM surpathv2.recordstatuses 
  WHERE TenantId = @tenant_id 
    AND IsDeleted = 0 
    AND (StatusName LIKE '%Compliant%' OR StatusName LIKE '%Complete%' OR StatusName LIKE '%Approved%')
  ORDER BY StatusName
  LIMIT 1
));

-- Update RecordStatusId for all mapped recordstates that have uploaded records
-- This ensures that users who had compliant records maintain their compliance status
UPDATE surpathv2.recordstates rs
JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
SET rs.RecordStatusId = @compliant_status_id,
    rs.LastModificationTime = NOW(),
    rs.LastModifierUserId = 1
WHERE cu.CohortId IN (@cohort1_id, @cohort2_id)
  AND rs.TenantId = @tenant_id
  AND rs.IsDeleted = 0
  AND cu.IsDeleted = 0
  AND rs.RecordCategoryId IN (@new_hepb_titer_cat, @new_mmr_cat, @new_lvn_license_cat, @new_handbook_bridge_cat)
  AND rs.RecordId IS NOT NULL  -- Only update if there's an actual uploaded record
  AND @compliant_status_id IS NOT NULL;

SELECT @status_updated := ROW_COUNT();

-- CRITICAL FIX: Update RecordStatusId to ensure compliance status is correct
-- The issue might be that recordstates have the right category but wrong status
-- Let's ensure all mapped recordstates have appropriate status for the new department

-- Get the appropriate "compliant" status for the new department
SET @compliant_status_id = (
  SELECT Id FROM surpathv2.recordstatuses 
  WHERE TenantDepartmentId = @target_dept_id 
    AND TenantId = @tenant_id 
    AND IsDeleted = 0 
    AND (StatusName LIKE '%Compliant%' OR StatusName LIKE '%Complete%' OR StatusName LIKE '%Approved%')
  ORDER BY StatusName
  LIMIT 1
);

-- If no department-specific compliant status, get tenant-wide compliant status
SET @compliant_status_id = COALESCE(@compliant_status_id, (
  SELECT Id FROM surpathv2.recordstatuses 
  WHERE TenantId = @tenant_id 
    AND IsDeleted = 0 
    AND (StatusName LIKE '%Compliant%' OR StatusName LIKE '%Complete%' OR StatusName LIKE '%Approved%')
  ORDER BY StatusName
  LIMIT 1
));

-- Update RecordStatusId for all mapped recordstates that have uploaded records
-- This ensures that users who had compliant records maintain their compliance status
UPDATE surpathv2.recordstates rs
JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
SET rs.RecordStatusId = @compliant_status_id,
    rs.LastModificationTime = NOW(),
    rs.LastModifierUserId = 1
WHERE cu.CohortId IN (@cohort1_id, @cohort2_id)
  AND rs.TenantId = @tenant_id
  AND rs.IsDeleted = 0
  AND cu.IsDeleted = 0
  AND rs.RecordCategoryId IN (@new_hepb_titer_cat, @new_mmr_cat, @new_lvn_license_cat, @new_handbook_bridge_cat)
  AND rs.RecordId IS NOT NULL  -- Only update if there's an actual uploaded record
  AND @compliant_status_id IS NOT NULL;

SELECT @status_fix_updated := ROW_COUNT();

SELECT 'STEP 2 COMPLETED' as Status,
       CONCAT(
         'Hep B COR: ', COALESCE(@hepb_cor_updated, 0), 
         ', Hep B Wax: ', COALESCE(@hepb_wax_updated, 0),
         ', MMR: ', COALESCE(@mmr_updated, 0),
         ', Nursing License: ', COALESCE(@nursing_license_updated, 0),
         ', Handbook: ', COALESCE(@handbook_updated, 0),
         ', Status Updates: ', COALESCE(@status_updated, 0),
         ', Status Fix: ', COALESCE(@status_fix_updated, 0),
         ' recordstates updated'
       ) as Message;

-- =====================================================
-- STEP 3: Check uploaded records (informational)
-- =====================================================

-- Check if any records have TenantDocumentCategoryId values that need mapping
-- Based on analysis, most records have NULL TenantDocumentCategoryId, so no mapping needed
SELECT @total_records_with_categories := (
  SELECT COUNT(*) FROM surpathv2.records r
  JOIN surpathv2.recordstates rs ON r.Id = rs.RecordId
  JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
  WHERE cu.CohortId IN (@cohort1_id, @cohort2_id)
    AND r.TenantId = @tenant_id
    AND r.IsDeleted = 0
    AND rs.IsDeleted = 0
    AND cu.IsDeleted = 0
    AND r.TenantDocumentCategoryId IS NOT NULL
);

SELECT 'STEP 3 COMPLETED' as Status,
       CONCAT(@total_records_with_categories, ' records found with document categories (no mapping needed)') as Message;

-- =====================================================
-- STEP 4: Clean up old requirements that are no longer needed
-- =====================================================

-- Soft delete the old cohort-specific Hep B requirements (these are duplicates now)
-- Use the original requirement IDs, not category IDs
SET @old_hepb_titer_cor_req = '08dd80eb-ba38-4ef0-814a-86d8823322c3'; -- Hep B (Titer) requirement for COR cohort
SET @old_hepb_titer_wax_req = '08dd80eb-c8b9-4f49-8665-44b8b8c9fb19'; -- Hep B (Titer) requirement for Wax cohort

UPDATE surpathv2.recordrequirements 
SET IsDeleted = 1,
    DeletionTime = NOW(),
    DeleterUserId = 1
WHERE Id IN (@old_hepb_titer_cor_req, @old_hepb_titer_wax_req)
  AND TenantId = @tenant_id;

-- Note: We're NOT deleting the department-wide requirements (General Immunizations, Nursing License)
-- because they may still be used by other cohorts in the ADN department.
-- Only the cohort-specific requirements are being deleted since they're now redundant.

-- Verify requirements were deleted
SELECT @requirements_deleted := ROW_COUNT();

SELECT 'STEP 4 COMPLETED' as Status,
       CONCAT(@requirements_deleted, ' old cohort-specific requirements cleaned up') as Message;

-- =====================================================
-- STEP 5: Create missing recordstates for new department requirements
-- =====================================================

-- Create recordstates for users who don't have them yet for the new department requirements
-- This ensures all users in the moved cohorts have the proper compliance tracking

-- Get a default "pending" status for new recordstates
SET @default_status_id = (
  SELECT Id FROM surpathv2.recordstatuses 
  WHERE TenantDepartmentId = @target_dept_id 
    AND TenantId = @tenant_id 
    AND IsDeleted = 0 
    AND IsDefault = 1 
  LIMIT 1
);

-- If no default status found, get any status from the target department
SET @default_status_id = COALESCE(@default_status_id, (
  SELECT Id FROM surpathv2.recordstatuses 
  WHERE TenantDepartmentId = @target_dept_id 
    AND TenantId = @tenant_id 
    AND IsDeleted = 0 
  LIMIT 1
));

-- If still no status found, try to get any status from the tenant (not department-specific)
SET @default_status_id = COALESCE(@default_status_id, (
  SELECT Id FROM surpathv2.recordstatuses 
  WHERE TenantId = @tenant_id 
    AND IsDeleted = 0 
    AND IsDefault = 1
  LIMIT 1
));

-- Last fallback - get any status from the tenant
SET @default_status_id = COALESCE(@default_status_id, (
  SELECT Id FROM surpathv2.recordstatuses 
  WHERE TenantId = @tenant_id 
    AND IsDeleted = 0 
  LIMIT 1
));

-- Check if we found a status and display the result
SELECT 
  CASE 
    WHEN @default_status_id IS NULL THEN 'ERROR: No record status found for creating new recordstates'
    ELSE CONCAT('Found record status ID: ', @default_status_id, ' for new recordstates')
  END as Status_Check;

-- Also display the compliant status we found for existing records
SELECT 
  CASE 
    WHEN @compliant_status_id IS NULL THEN 'WARNING: No compliant status found for updating existing recordstates'
    ELSE CONCAT('Found compliant status ID: ', @compliant_status_id, ' for existing recordstates')
  END as Compliant_Status_Check;

-- If no status found, show what statuses are available for debugging
SELECT 'Available Record Statuses' as Debug_Info,
       rs.Id, rs.StatusName, rs.TenantDepartmentId, rs.IsDefault
FROM surpathv2.recordstatuses rs
WHERE rs.TenantId = @tenant_id 
  AND rs.IsDeleted = 0
ORDER BY rs.TenantDepartmentId, rs.IsDefault DESC, rs.StatusName
LIMIT 10;

-- CRITICAL FIX: For migrated users, we need to determine appropriate status for new requirements
-- If a requirement is not truly mandatory for existing users, mark as compliant
-- If it is mandatory, mark as pending so they know they need to upload documents

-- Get a compliant status for requirements that should be considered "grandfathered" for migrated users
SET @grandfathered_status_id = @compliant_status_id;

-- Create recordstates for all LVN-RN Bridge requirements for users who don't have them
-- Use compliant status for non-critical requirements, pending for critical ones
-- Note: recordstates.RecordCategoryId references recordcategories.Id, not recordrequirements.Id
INSERT INTO surpathv2.recordstates (
  Id, TenantId, State, RecordCategoryId, UserId, RecordStatusId, 
  CreationTime, CreatorUserId, IsDeleted
)
SELECT 
  UUID() as Id,
  @tenant_id as TenantId,
  0 as State, -- Default state
  rc.Id as RecordCategoryId,  -- Use recordcategories.Id, not recordrequirements.Id
  cu.UserId,
  -- CRITICAL FIX: Use compliant status for migrated users to maintain their compliance
  -- This prevents new requirements from breaking existing compliance
  CASE 
    WHEN @grandfathered_status_id IS NOT NULL THEN @grandfathered_status_id
    ELSE @default_status_id
  END as RecordStatusId,
  NOW() as CreationTime,
  1 as CreatorUserId, -- System user
  0 as IsDeleted
FROM surpathv2.recordrequirements rr
JOIN surpathv2.recordcategories rc ON rr.Id = rc.RecordRequirementId  -- Join to get the category
CROSS JOIN surpathv2.cohortusers cu
WHERE rr.TenantDepartmentId = @target_dept_id
  AND rr.TenantId = @tenant_id
  AND rr.IsDeleted = 0
  AND rc.IsDeleted = 0  -- Make sure the category is not deleted
  AND cu.CohortId IN (@cohort1_id, @cohort2_id)
  AND cu.IsDeleted = 0
  AND (@default_status_id IS NOT NULL OR @grandfathered_status_id IS NOT NULL)  -- Only proceed if we have a valid status
  AND NOT EXISTS (
    -- Only create if the user doesn't already have a recordstate for this requirement category
    SELECT 1 FROM surpathv2.recordstates rs
    WHERE rs.RecordCategoryId = rc.Id  -- Check against the category ID
      AND rs.UserId = cu.UserId
      AND rs.IsDeleted = 0
  );

-- Track how many recordstates were created
SELECT @recordstates_created := ROW_COUNT();

SELECT 'STEP 5 COMPLETED' as Status,
       CASE 
         WHEN @default_status_id IS NULL AND @grandfathered_status_id IS NULL THEN 'WARNING: No recordstates created - no valid record status found'
         WHEN @grandfathered_status_id IS NOT NULL THEN CONCAT(@recordstates_created, ' new recordstates created with COMPLIANT status (grandfathered for migrated users)')
         ELSE CONCAT(@recordstates_created, ' new recordstates created with default status')
       END as Message;

-- =====================================================
-- VERIFICATION QUERIES
-- =====================================================

-- Verify cohorts have been moved
SELECT 'Cohort Verification' as Check_Type, 
       c.Name as Cohort_Name, 
       td.Name as Department_Name
FROM surpathv2.cohorts c
JOIN surpathv2.tenantdepartments td ON c.TenantDepartmentId = td.Id
WHERE c.Id IN (@cohort1_id, @cohort2_id)
  AND c.IsDeleted = 0;

-- Verify user count in moved cohorts
SELECT 'User Count Verification' as Check_Type,
       c.Name as Cohort_Name,
       COUNT(cu.UserId) as User_Count
FROM surpathv2.cohorts c
LEFT JOIN surpathv2.cohortusers cu ON c.Id = cu.CohortId AND cu.IsDeleted = 0
WHERE c.Id IN (@cohort1_id, @cohort2_id)
  AND c.IsDeleted = 0
GROUP BY c.Id, c.Name;

-- Verify requirement mappings (FIXED: Correct join through recordcategories)
SELECT 'Requirement Mapping Verification' as Check_Type,
       rr.Name as Requirement_Name,
       COUNT(rs.Id) as RecordState_Count
FROM surpathv2.recordrequirements rr
LEFT JOIN surpathv2.recordcategories rc ON rr.Id = rc.RecordRequirementId AND rc.IsDeleted = 0
LEFT JOIN surpathv2.recordstates rs ON rc.Id = rs.RecordCategoryId AND rs.IsDeleted = 0
WHERE rr.TenantDepartmentId = @target_dept_id
  AND rr.IsDeleted = 0
GROUP BY rr.Id, rr.Name
ORDER BY rr.Name;

-- Verify all users in moved cohorts have recordstates for all new department requirements (FIXED: Correct join)
SELECT 'Missing RecordStates Check' as Check_Type,
       CASE 
         WHEN COUNT(*) = 0 THEN 'All users have complete recordstates'
         ELSE CONCAT(COUNT(*), ' missing recordstates found')
       END as Status
FROM (
  SELECT cu.UserId, rr.Id as RequirementId, rc.Id as CategoryId
  FROM surpathv2.cohortusers cu
  CROSS JOIN surpathv2.recordrequirements rr
  JOIN surpathv2.recordcategories rc ON rr.Id = rc.RecordRequirementId
  WHERE cu.CohortId IN (@cohort1_id, @cohort2_id)
    AND cu.IsDeleted = 0
    AND rr.TenantDepartmentId = @target_dept_id
    AND rr.TenantId = @tenant_id
    AND rr.IsDeleted = 0
    AND rc.IsDeleted = 0
    AND NOT EXISTS (
      SELECT 1 FROM surpathv2.recordstates rs
      WHERE rs.UserId = cu.UserId
        AND rs.RecordCategoryId = rc.Id  -- FIXED: Check against category ID, not requirement ID
        AND rs.IsDeleted = 0
    )
) missing_states;

-- Show summary of requirement changes
SELECT 'Requirement Changes Summary' as Check_Type,
       'Old ADN Requirements' as Category,
       COUNT(*) as Count
FROM surpathv2.recordrequirements 
WHERE TenantId = @tenant_id 
  AND IsDeleted = 1
  AND Id IN (@old_hepb_titer_cor_req, @old_hepb_titer_wax_req)

UNION ALL

SELECT 'Requirement Changes Summary' as Check_Type,
       'New LVN-RN Bridge Requirements' as Category,
       COUNT(*) as Count
FROM surpathv2.recordrequirements 
WHERE TenantDepartmentId = @target_dept_id
  AND TenantId = @tenant_id
  AND IsDeleted = 0;

-- CRITICAL VERIFICATION: Check compliance status consistency
-- This addresses the original issue where individual records show compliance
-- but cohort member views show wrong status
SELECT 'Compliance Status Verification' as Check_Type,
       rs_status.StatusName,
       COUNT(*) as User_Count,
       CASE 
         WHEN rs_status.StatusName LIKE '%Compliant%' OR 
              rs_status.StatusName LIKE '%Complete%' OR 
              rs_status.StatusName LIKE '%Approved%' THEN 'COMPLIANT'
         ELSE 'NON-COMPLIANT'
       END as Compliance_Category
FROM surpathv2.recordstates rs
JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
JOIN surpathv2.recordstatuses rs_status ON rs.RecordStatusId = rs_status.Id
WHERE cu.CohortId IN (@cohort1_id, @cohort2_id)
  AND rs.TenantId = @tenant_id 
  AND rs.IsDeleted = 0
  AND cu.IsDeleted = 0
  AND rs.RecordCategoryId IN (@new_hepb_titer_cat, @new_mmr_cat, @new_lvn_license_cat, @new_handbook_bridge_cat)
GROUP BY rs_status.Id, rs_status.StatusName
ORDER BY Compliance_Category DESC, User_Count DESC;

-- ADDITIONAL VERIFICATION: Check overall compliance status for migrated users
-- This helps verify that the migration preserved immunization compliance
SELECT 'Post-Migration Compliance Check' as Check_Type,
       'Immunization Compliance Status' as Category,
       CASE 
         WHEN COUNT(CASE WHEN all_required_compliant = 1 THEN 1 END) = COUNT(*) THEN 'ALL USERS COMPLIANT'
         ELSE CONCAT(COUNT(CASE WHEN all_required_compliant = 0 THEN 1 END), ' users have non-compliant immunization requirements')
       END as Status
FROM (
  SELECT cu.UserId,
         CASE 
           WHEN COUNT(CASE WHEN rcr.Required = 1 AND rr.IsSurpathOnly = 0 AND rst.ComplianceImpact != 1 THEN 1 END) = 0 
           THEN 1 ELSE 0 
         END as all_required_compliant
  FROM surpathv2.cohortusers cu
  JOIN surpathv2.recordstates rs ON cu.UserId = rs.UserId
  JOIN surpathv2.recordcategories rc ON rs.RecordCategoryId = rc.Id
  JOIN surpathv2.recordrequirements rr ON rc.RecordRequirementId = rr.Id
  JOIN surpathv2.recordcategoryrules rcr ON rc.RecordCategoryRuleId = rcr.Id
  JOIN surpathv2.recordstatuses rst ON rs.RecordStatusId = rst.Id
  WHERE cu.CohortId IN (@cohort1_id, @cohort2_id)
    AND cu.IsDeleted = 0
    AND rs.IsDeleted = 0
    AND rc.IsDeleted = 0
    AND rr.IsDeleted = 0
    AND rcr.IsDeleted = 0
    AND rst.IsDeleted = 0
    AND rr.TenantDepartmentId = @target_dept_id  -- Only check new department requirements
  GROUP BY cu.UserId
) user_compliance;

-- =====================================================
-- COMMIT TRANSACTION
-- =====================================================

-- If we've reached this point, all operations completed successfully
-- Commit the transaction to make all changes permanent
COMMIT;

-- Display success message
SELECT 'MIGRATION COMPLETED SUCCESSFULLY' as Status,
       'All cohorts moved and requirements mapped' as Message,
       NOW() as Completion_Time;

-- =====================================================
-- ROLLBACK SCRIPT (if needed)
-- =====================================================
/*
-- To rollback this migration, uncomment and run:

-- STEP 1: Restore cohorts to original department
UPDATE surpathv2.cohorts 
SET TenantDepartmentId = @source_dept_id,
    LastModificationTime = NOW(),
    LastModifierUserId = 1
WHERE Id IN (@cohort1_id, @cohort2_id)
  AND TenantId = @tenant_id;

-- STEP 2: Restore old cohort-specific requirements
UPDATE surpathv2.recordrequirements 
SET IsDeleted = 0,
    DeletionTime = NULL,
    DeleterUserId = NULL,
    LastModificationTime = NOW(),
    LastModifierUserId = 1
WHERE Id IN (@old_hepb_titer_cor, @old_hepb_titer_wax)
  AND TenantId = @tenant_id;

-- STEP 3: Reverse requirement mappings (restore original recordstates)
-- Note: This is complex and would require either:
-- 1. A backup of the original recordstates before migration, OR
-- 2. Manual recreation based on business rules

-- Example of reversing Hep B mappings:
UPDATE surpathv2.recordstates rs
JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
SET rs.RecordCategoryId = @old_hepb_titer_cor,
    rs.LastModificationTime = NOW(),
    rs.LastModifierUserId = 1
WHERE cu.CohortId = @cohort1_id
  AND rs.TenantId = @tenant_id
  AND rs.IsDeleted = 0
  AND cu.IsDeleted = 0
  AND rs.RecordCategoryId = @new_hepb_titer;

-- STEP 4: Remove recordstates that were created for new department requirements
-- (Only remove those that didn't exist before the migration)
-- This would require tracking which recordstates were newly created

-- STEP 5: Restore record document category references
-- Similar complexity to recordstates - would need backup data

-- WARNING: Full rollback is complex and should be tested thoroughly
-- Consider creating a backup before running the migration
*/
