-- =====================================================
-- Update Script: Fix NULL IsArchived values in RecordStates
-- =====================================================
-- Issue: After restoring production backup, all existing RecordState records 
-- have NULL values for IsArchived column, causing compliance calculations to fail
-- because queries filter by !IsArchived (and NULL is not equal to false).
--
-- Solution: Set IsArchived = 0 (false) for all existing records where IsArchived IS NULL
-- This makes all existing records "current" by default, which is the correct behavior
-- for a production restore where we want to preserve the existing compliance state.
-- =====================================================

-- First, let's check how many records are affected
SELECT COUNT(*) AS affected_records_count
FROM RecordStates 
WHERE IsArchived IS NULL;

-- Update all NULL IsArchived values to false (0)
-- This is idempotent - running multiple times is safe
UPDATE RecordStates 
SET IsArchived = 0
WHERE IsArchived IS NULL;

-- Verify the update
SELECT 
    COUNT(*) AS total_records,
    SUM(CASE WHEN IsArchived = 0 THEN 1 ELSE 0 END) AS not_archived_count,
    SUM(CASE WHEN IsArchived = 1 THEN 1 ELSE 0 END) AS archived_count,
    SUM(CASE WHEN IsArchived IS NULL THEN 1 ELSE 0 END) AS null_count
FROM RecordStates;

-- Optional: Add default constraint to prevent future NULLs
-- Note: This may already exist from the migration, but MySQL allows redundant defaults
-- ALTER TABLE RecordStates MODIFY COLUMN IsArchived TINYINT(1) NOT NULL DEFAULT 0;