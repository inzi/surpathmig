-- =====================================================
-- Navarro College: Move LVN-RN Bridge Cohorts Script
-- =====================================================
-- This script moves two cohorts from "ADN (Associate Degree Nursing)" 
-- to "LVN - RN Bridge" department and maps their requirements.

-- Variables for the migration
SET @tenant_id = 24; -- Navarro College
SET @source_dept_id = '08db77ef-9288-4d07-8d93-a631951d4095'; -- ADN (Associate Degree Nursing)
SET @target_dept_id = '08dd80ec-2477-4be0-8ffa-31457fc44b7d'; -- LVN - RN Bridge
SET @cohort1_id = '08dd55ca-6414-4b3e-8e91-e4f510592ac1'; -- LVN-RN Bridge (Grads 2026)-COR
SET @cohort2_id = '08dd55d8-68f6-4a07-8aa1-dfb92195395e'; -- LVN-RN Bridge (Grads 2026)-Wax

-- Requirement mapping variables (Old requirement ID -> New requirement ID)
-- Map "Hep B (Titer)" from ADN cohorts to "Hepatitis B (Titer)" in LVN-RN Bridge
SET @old_hepb_titer_cor = '08dd80eb-ba38-4ef0-814a-86d8823322c3'; -- Hep B (Titer) for COR cohort
SET @old_hepb_titer_wax = '08dd80eb-c8b9-4f49-8665-44b8b8c9fb19'; -- Hep B (Titer) for Wax cohort
SET @new_hepb_titer = '08dd80ef-6802-4151-88c0-3b22bb4d9b65'; -- Hepatitis B (Titer) in LVN-RN Bridge

-- Map "Student Handbook Signature Form" (exists in both departments)
SET @old_handbook_adn = '08dd889d-aaef-445d-85b3-5018b924d137'; -- Student Handbook in ADN
SET @new_handbook_bridge = '08dd80ef-b156-4e19-8612-bfe69e3503f0'; -- Student Handbook in LVN-RN Bridge

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

-- =====================================================
-- STEP 2: Map record requirements for users
-- =====================================================

-- Update recordstates that reference the old Hep B (Titer) requirements
-- Map COR cohort Hep B requirement to new department requirement
UPDATE surpathv2.recordstates rs
JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
SET rs.RecordCategoryId = @new_hepb_titer,
    rs.LastModificationTime = NOW(),
    rs.LastModifierUserId = 1
WHERE cu.CohortId = @cohort1_id
  AND rs.TenantId = @tenant_id
  AND rs.IsDeleted = 0
  AND cu.IsDeleted = 0
  AND EXISTS (
    SELECT 1 FROM surpathv2.recordrequirements rr 
    WHERE rr.Id = @old_hepb_titer_cor 
    AND rs.RecordCategoryId = rr.Id
  );

-- Map Wax cohort Hep B requirement to new department requirement
UPDATE surpathv2.recordstates rs
JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
SET rs.RecordCategoryId = @new_hepb_titer,
    rs.LastModificationTime = NOW(),
    rs.LastModifierUserId = 1
WHERE cu.CohortId = @cohort2_id
  AND rs.TenantId = @tenant_id
  AND rs.IsDeleted = 0
  AND cu.IsDeleted = 0
  AND EXISTS (
    SELECT 1 FROM surpathv2.recordrequirements rr 
    WHERE rr.Id = @old_hepb_titer_wax 
    AND rs.RecordCategoryId = rr.Id
  );

-- Map Student Handbook Signature Form requirement
UPDATE surpathv2.recordstates rs
JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
SET rs.RecordCategoryId = @new_handbook_bridge,
    rs.LastModificationTime = NOW(),
    rs.LastModifierUserId = 1
WHERE cu.CohortId IN (@cohort1_id, @cohort2_id)
  AND rs.TenantId = @tenant_id
  AND rs.IsDeleted = 0
  AND cu.IsDeleted = 0
  AND EXISTS (
    SELECT 1 FROM surpathv2.recordrequirements rr 
    WHERE rr.Id = @old_handbook_adn 
    AND rs.RecordCategoryId = rr.Id
  );

-- =====================================================
-- STEP 3: Update any records that reference old requirements
-- =====================================================

-- Update records that were uploaded for the old Hep B requirements
UPDATE surpathv2.records r
JOIN surpathv2.recordstates rs ON r.Id = rs.RecordId
JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
SET r.TenantDocumentCategoryId = @new_hepb_titer,
    r.LastModificationTime = NOW(),
    r.LastModifierUserId = 1
WHERE cu.CohortId IN (@cohort1_id, @cohort2_id)
  AND r.TenantId = @tenant_id
  AND r.IsDeleted = 0
  AND rs.IsDeleted = 0
  AND cu.IsDeleted = 0
  AND r.TenantDocumentCategoryId IN (@old_hepb_titer_cor, @old_hepb_titer_wax);

-- Update records for Student Handbook
UPDATE surpathv2.records r
JOIN surpathv2.recordstates rs ON r.Id = rs.RecordId
JOIN surpathv2.cohortusers cu ON rs.UserId = cu.UserId
SET r.TenantDocumentCategoryId = @new_handbook_bridge,
    r.LastModificationTime = NOW(),
    r.LastModifierUserId = 1
WHERE cu.CohortId IN (@cohort1_id, @cohort2_id)
  AND r.TenantId = @tenant_id
  AND r.IsDeleted = 0
  AND rs.IsDeleted = 0
  AND cu.IsDeleted = 0
  AND r.TenantDocumentCategoryId = @old_handbook_adn;

-- =====================================================
-- STEP 4: Clean up old cohort-specific requirements
-- =====================================================

-- Soft delete the old cohort-specific Hep B requirements
UPDATE surpathv2.recordrequirements 
SET IsDeleted = 1,
    DeletionTime = NOW(),
    DeleterUserId = 1
WHERE Id IN (@old_hepb_titer_cor, @old_hepb_titer_wax)
  AND TenantId = @tenant_id;

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

-- Verify requirement mappings
SELECT 'Requirement Mapping Verification' as Check_Type,
       rr.Name as Requirement_Name,
       COUNT(rs.Id) as RecordState_Count
FROM surpathv2.recordrequirements rr
LEFT JOIN surpathv2.recordstates rs ON rr.Id = rs.RecordCategoryId AND rs.IsDeleted = 0
WHERE rr.TenantDepartmentId = @target_dept_id
  AND rr.IsDeleted = 0
GROUP BY rr.Id, rr.Name
ORDER BY rr.Name;

-- =====================================================
-- ROLLBACK SCRIPT (if needed)
-- =====================================================
/*
-- To rollback this migration, uncomment and run:

-- Restore cohorts to original department
UPDATE surpathv2.cohorts 
SET TenantDepartmentId = @source_dept_id,
    LastModificationTime = NOW(),
    LastModifierUserId = 1
WHERE Id IN (@cohort1_id, @cohort2_id)
  AND TenantId = @tenant_id;

-- Restore old requirements
UPDATE surpathv2.recordrequirements 
SET IsDeleted = 0,
    DeletionTime = NULL,
    DeleterUserId = NULL,
    LastModificationTime = NOW(),
    LastModifierUserId = 1
WHERE Id IN (@old_hepb_titer_cor, @old_hepb_titer_wax)
  AND TenantId = @tenant_id;

-- Note: Requirement mappings would need manual restoration
-- based on backup data or specific business rules
*/
