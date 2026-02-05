-- Comprehensive query to show all records with their details for a specific user or tenant
-- This query displays all records, their state, status, category, and rule information
--
-- Usage:
--   - Change the TenantId value in the WHERE clause to filter by a specific tenant
--   - Uncomment the UserId line in WHERE clause to filter by a specific user
--   - Adjust the LIMIT clause as needed (or remove for all results)

SELECT
    u.Id as UserId,
    u.Name as UserName,
    u.Surname as UserSurname,
    u.EmailAddress,
    t.Id as TenantId,
    t.TenancyName,
    rs.Id as RecordStateId,
    rs.State,
    rs.CreationTime as StateCreationTime,
    rs.LastModificationTime as StateLastModified,
    rs.Notes as StateNotes,
    rs.IsArchived,
    r.Id as RecordId,
    r.filename as RecordFilename,
    r.DateUploaded,
    r.EffectiveDate,
    r.ExpirationDate,
    rst.Id as StatusId,
    rst.StatusName,
    rst.HtmlColor as StatusColor,
    rst.ComplianceImpact,
    rc.Id as CategoryId,
    rc.Name as CategoryName,
    rc.Instructions as CategoryInstructions,
    rcr.Id as RuleId,
    rcr.Name as RuleName,
    rcr.Description as RuleDescription,
    rcr.Expires,
    rcr.ExpireInDays,
    rcr.Required as RuleRequired,
    rcr.WarnDaysBeforeFirst,
    rcr.WarnDaysBeforeSecond,
    rcr.WarnDaysBeforeFinal,
    rr.Id as RequirementId,
    rr.Name as RequirementName,
    rr.Description as RequirementDescription,
    rr.SurPathServiceId,
    ss.Name as SurpathServiceName,
    tss.IsPricingOverrideEnabled as ServicePricingEnabled
FROM recordstates rs
INNER JOIN abpusers u ON rs.UserId = u.Id
INNER JOIN abptenants t ON rs.TenantId = t.Id
LEFT JOIN records r ON rs.RecordId = r.Id
LEFT JOIN recordstatuses rst ON rs.RecordStatusId = rst.Id
LEFT JOIN recordcategories rc ON rs.RecordCategoryId = rc.Id
LEFT JOIN recordcategoryrules rcr ON rc.RecordCategoryRuleId = rcr.Id
LEFT JOIN recordrequirements rr ON rc.RecordRequirementId = rr.Id
LEFT JOIN surpathservices ss ON rr.SurpathServiceId = ss.Id
LEFT JOIN tenantsurpathservices tss ON tss.SurpathServiceId = ss.Id AND tss.TenantId = t.Id
WHERE rs.TenantId = 24  -- Change this to the desired tenant ID
    -- AND rs.UserId = 123  -- Uncomment and change this to filter by specific user
    AND rs.IsDeleted = 0
ORDER BY u.EmailAddress, ss.Name, rr.Name, rc.Name, rs.CreationTime DESC
-- LIMIT 100  -- Uncomment to limit results
;