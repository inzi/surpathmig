-- Consolidation query for duplicate Drug Screening record requirements in Tenant 24
--
-- ISSUE: Tenant 24 has multiple record requirements for the same SurPath Service ID (08dae8d6-d5fc-4a9c-8ee9-a3518c27e4b4)
-- There should only be ONE requirement per SurPath Service per tenant
--
-- DUPLICATE REQUIREMENTS FOUND:
--   1. PRIMARY (Target): 08dd2a6b-bc92-4f70-862d-fca210fbae8d - "Drug Screening" - 175 record states
--   2. DUPLICATE 1:      08db71c7-b5f1-4666-8d8a-cbfc04c9a2a6 - "Drug Screening (navarro)" - 0 record states
--   3. DUPLICATE 2:      08db94bf-958e-48bb-8696-2f95d30f5466 - "Drug Screening (navarro)" - 242 record states
--
-- GOAL: Consolidate all records to the PRIMARY requirement (08dd2a6b-bc92-4f70-862d-fca210fbae8d)
--       This will consolidate 417 total record states (175 + 0 + 242) under a single requirement
--
-- IMPORTANT: Run these queries in a transaction and backup your data first!

-- Start transaction
START TRANSACTION;

-- ============================================
-- STEP 0: VERIFY CURRENT STATE (Optional - for safety)
-- ============================================
-- Check what will be affected before making changes
SELECT
    'Records to be moved:' as Description,
    COUNT(*) as Count
FROM recordstates rs
INNER JOIN recordcategories rc ON rs.RecordCategoryId = rc.Id
WHERE rs.TenantId = 24
    AND rc.RecordRequirementId IN (
        '08db71c7-b5f1-4666-8d8a-cbfc04c9a2a6',  -- Duplicate 1
        '08db94bf-958e-48bb-8696-2f95d30f5466'   -- Duplicate 2
    );

-- ============================================
-- STEP 1: UPDATE RECORDS TABLE
-- ============================================
-- Update the TenantDocumentCategoryId in records table to point to the target category
-- Target category ID: 08dd2a6b-bc93-4a43-8069-552e2bb0aa2a (belongs to target requirement)

UPDATE records r
SET
    r.TenantDocumentCategoryId = '08dd2a6b-bc93-4a43-8069-552e2bb0aa2a',
    r.LastModificationTime = NOW(),
    r.LastModifierUserId = 1  -- Update with appropriate admin user ID
WHERE r.TenantId = 24
    AND r.TenantDocumentCategoryId IN (
        '08db71c7-b5f1-499c-8548-22b48d54c2f4',  -- Category for Duplicate 1
        '08db94bf-958e-48d9-84c9-7b3e079c8e27'   -- Category for Duplicate 2
    );

-- Verify the update
SELECT 'Records updated:', ROW_COUNT();

-- ============================================
-- STEP 2: UPDATE RECORD STATES
-- ============================================
-- Move all record states from duplicate requirements to the target category
-- Target category ID: 08dd2a6b-bc93-4a43-8069-552e2bb0aa2a (belongs to target requirement)

UPDATE recordstates rs
INNER JOIN recordcategories rc ON rs.RecordCategoryId = rc.Id
SET
    rs.RecordCategoryId = '08dd2a6b-bc93-4a43-8069-552e2bb0aa2a',
    rs.LastModificationTime = NOW(),
    rs.LastModifierUserId = 1  -- Update with appropriate admin user ID
WHERE rs.TenantId = 24
    AND rc.RecordRequirementId IN (
        '08db71c7-b5f1-4666-8d8a-cbfc04c9a2a6',  -- Duplicate 1
        '08db94bf-958e-48bb-8696-2f95d30f5466'   -- Duplicate 2
    );

-- Verify the update
SELECT 'Record states updated:', ROW_COUNT();

-- ============================================
-- STEP 3: DELETE ORPHANED CATEGORIES
-- ============================================
-- Remove the now-unused categories from duplicate requirements

DELETE FROM recordcategories
WHERE TenantId = 24
    AND RecordRequirementId IN (
        '08db71c7-b5f1-4666-8d8a-cbfc04c9a2a6',  -- Duplicate 1
        '08db94bf-958e-48bb-8696-2f95d30f5466'   -- Duplicate 2
    );

-- Verify the deletion
SELECT 'Categories deleted:', ROW_COUNT();

-- ============================================
-- STEP 4: DELETE DUPLICATE REQUIREMENTS
-- ============================================
-- Remove the duplicate requirement records

DELETE FROM recordrequirements
WHERE TenantId = 24
    AND Id IN (
        '08db71c7-b5f1-4666-8d8a-cbfc04c9a2a6',  -- Duplicate 1
        '08db94bf-958e-48bb-8696-2f95d30f5466'   -- Duplicate 2
    );

-- Verify the deletion
SELECT 'Requirements deleted:', ROW_COUNT();

-- ============================================
-- STEP 5: VERIFY CONSOLIDATION
-- ============================================
-- Check that all records are now under the single requirement

SELECT
    rr.Id as RecordRequirementId,
    rr.Name as RequirementName,
    rc.Id as RecordCategoryId,
    rc.Name as CategoryName,
    COUNT(DISTINCT rs.Id) as TotalRecordStates,
    COUNT(DISTINCT rs.UserId) as TotalUsers
FROM recordrequirements rr
LEFT JOIN recordcategories rc ON rc.RecordRequirementId = rr.Id
LEFT JOIN recordstates rs ON rs.RecordCategoryId = rc.Id
WHERE rr.TenantId = 24
    AND rr.SurPathServiceId = '08dae8d6-d5fc-4a9c-8ee9-a3518c27e4b4'
    AND rs.IsDeleted = 0
GROUP BY rr.Id, rr.Name, rc.Id, rc.Name;

-- ============================================
-- FINALIZE
-- ============================================
-- If everything looks correct, commit the transaction
-- COMMIT;

-- If there are issues, rollback
-- ROLLBACK;