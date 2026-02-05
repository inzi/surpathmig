-- ========================================
  -- USER TENANT MIGRATION SCRIPT
  -- ========================================
  -- WARNING: This is a destructive operation. Back up your database first!
  -- This script moves a user and all their data from one tenant to another.

  -- SET YOUR VARIABLES HERE:
  SET @source_user_id = 123;           -- The UserId to migrate
  SET @source_tenant_id = 1;           -- Current tenant ID
  SET @target_tenant_id = 2;           -- New tenant ID

  -- Start transaction for safety
  START TRANSACTION;

  -- ========================================
  -- STEP 1: DELETE TENANT-SPECIFIC ASSOCIATIONS
  -- These cannot be migrated because they reference tenant-specific entities
  -- ========================================

  -- Delete cohort associations (cohorts belong to specific tenants)
  DELETE FROM CohortUsers
  WHERE UserId = @source_user_id
    AND TenantId = @source_tenant_id;

  -- Delete department associations (departments belong to specific tenants)
  DELETE FROM TenantDepartmentUsers
  WHERE UserId = @source_user_id
    AND TenantId = @source_tenant_id;

  DELETE FROM DepartmentUsers
  WHERE UserId = @source_user_id
    AND TenantId = @source_tenant_id;


  -- ========================================
  -- STEP 2: UPDATE THE USER'S TENANT
  -- ========================================

  UPDATE AbpUsers
  SET TenantId = @target_tenant_id,
      LastModificationTime = NOW()
  WHERE Id = @source_user_id
    AND TenantId = @source_tenant_id;


  -- ========================================
  -- STEP 3: MIGRATE USER DATA
  -- Update all user-related records to new tenant
  -- ========================================

  -- Migrate compliance records (uploaded documents)
  -- Note: User indicated uploads won't be associated with anything
  -- so these will exist but may not be linked to new tenant requirements
  UPDATE Records
  SET TenantId = @target_tenant_id,
      LastModificationTime = NOW()
  WHERE CreatorUserId = @source_user_id
    AND TenantId = @source_tenant_id;

  -- Migrate record states (compliance status)
  UPDATE RecordStates
  SET TenantId = @target_tenant_id,
      LastModificationTime = NOW()
  WHERE UserId = @source_user_id
    AND TenantId = @source_tenant_id;

  -- Migrate record notes
  UPDATE RecordNotes
  SET TenantId = @target_tenant_id,
      LastModificationTime = NOW()
  WHERE UserId = @source_user_id
    AND TenantId = @source_tenant_id;

  -- Migrate personal identifiers (SSN, employee ID, etc.)
  UPDATE UserPids
  SET TenantId = @target_tenant_id,
      LastModificationTime = NOW()
  WHERE UserId = @source_user_id
    AND TenantId = @source_tenant_id;

  -- Migrate user purchases
  UPDATE UserPurchases
  SET TenantId = @target_tenant_id,
      LastModificationTime = NOW()
  WHERE UserId = @source_user_id
    AND TenantId = @source_tenant_id;

  -- Migrate ledger entries (financial transactions)
  UPDATE LedgerEntries
  SET TenantId = @target_tenant_id,
      LastModificationTime = NOW()
  WHERE UserId = @source_user_id
    AND TenantId = @source_tenant_id;

  -- Migrate ledger entry details
  UPDATE LedgerEntryDetails
  SET TenantId = @target_tenant_id,
      LastModificationTime = NOW()
  WHERE Id IN (
      SELECT led.Id
      FROM LedgerEntryDetails led
      INNER JOIN LedgerEntries le ON led.LedgerEntryId = le.Id
      WHERE le.UserId = @source_user_id
  );


  -- ========================================
  -- STEP 4: VERIFICATION QUERIES
  -- Run these to verify the migration
  -- ========================================

  -- Check user was updated
  SELECT Id, UserName, EmailAddress, TenantId
  FROM AbpUsers
  WHERE Id = @source_user_id;

  -- Check record counts
  SELECT
      'Records' as TableName, COUNT(*) as Count
  FROM Records
  WHERE CreatorUserId = @source_user_id AND TenantId = @target_tenant_id
  UNION ALL
  SELECT
      'RecordStates', COUNT(*)
  FROM RecordStates
  WHERE UserId = @source_user_id AND TenantId = @target_tenant_id
  UNION ALL
  SELECT
      'UserPids', COUNT(*)
  FROM UserPids
  WHERE UserId = @source_user_id AND TenantId = @target_tenant_id
  UNION ALL
  SELECT
      'UserPurchases', COUNT(*)
  FROM UserPurchases
  WHERE UserId = @source_user_id AND TenantId = @target_tenant_id
  UNION ALL
  SELECT
      'LedgerEntries', COUNT(*)
  FROM LedgerEntries
  WHERE UserId = @source_user_id AND TenantId = @target_tenant_id
  UNION ALL
  SELECT
      'CohortUsers', COUNT(*)
  FROM CohortUsers
  WHERE UserId = @source_user_id AND TenantId = @target_tenant_id;

  -- If everything looks good, commit:
  COMMIT;

  -- If something went wrong, rollback:
  -- ROLLBACK;

  Important Notes:

  1. DELETED Associations:
    - CohortUsers - Cohorts are tenant-specific
    - TenantDepartmentUsers & DepartmentUsers - Departments are tenant-specific
    - The user will need to be manually added to cohorts/departments in the new tenant
  2. Migrated Data:
    - Records (uploaded files - but won't match new tenant requirements)
    - RecordStates (compliance status records)
    - UserPids (personal identifiers)
    - UserPurchases & LedgerEntries (financial history)
    - RecordNotes
  3. NOT Migrated:
    - File uploads themselves (as you specified)
    - ABP framework tables (permissions, settings, etc.)
  4. Post-Migration Steps:
    - Add user to appropriate cohorts in new tenant
    - Add user to appropriate departments in new tenant
    - User will need to re-upload documents to meet new tenant requirements
    - Verify compliance status