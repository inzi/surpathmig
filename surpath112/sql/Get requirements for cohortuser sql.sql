-- get all appplicable requirements

-- An example issue is when a user is viewing their home page, and they’re a member of a cohort in the therapy department – they should ONLY see the generic immunizations requirement.
-- If they’re in a cohort in the surgical dept – they should see immunizations and the surgical requirements.
-- If they’re in HCC PM Class, they’ll see the generic immunizations requirement AND the Immunizations with Tdap and MMR.

-- jack barber - phys therapy - cohortuserid 08db9123-d59b-4d01-851e-8d51c1ecc6f0
-- Elizabeth Adams - surgical - 08db6c5f-cd43-42f8-823d-3a20d9ba9aec
-- Lucas Babcock - HCC - 08db8bf2-97e9-4c7f-85df-3779dbf1aafe
-- Megan Grissam - PM Select 1 - 08db563c-a058-4503-8e8d-509e9deabb54


-- a requirement that applies to a dept and a cohort must match both
-- a requirement that applies to a dept should match and any cohort with that dept id
-- a requirement that applies to a cohort should match that cohort regardless of dept
-- a requirement that applies to no cohort and no dept should match all cohorts


-- select * from abptenants; -- mntc, 11
set @tenantid = 11;
-- set @cohortuserid = '08db9123-d59b-4d01-851e-8d51c1ecc6f0'; -- jack 
-- set @cohortuserid = '08db6c5f-cd43-42f8-823d-3a20d9ba9aec'; -- Elizabeth
-- set @cohortuserid = '08db8bf2-97e9-4c7f-85df-3779dbf1aafe'; -- Lucas
set @cohortuserid = '08db563c-a058-4503-8e8d-509e9deabb54'; -- Megan (PN Select) 
set @userid = (select userid from cohortusers where id = @cohortuserid);

select 
rr.id, rr.name, rr.Description, rr.cohortid, rr.TenantDepartmentId
,td.id as 'Deptid' ,td.Name as 'DeptName'
,co.id as 'Cohortid' ,co.Name as 'CohortName', co.TenantDepartmentId as 'cohortdeptid'
,cu.id as 'CohortUserid', @userid as 'UserId'
,rc.id as 'CatId', rc.Name as 'CatName'
,rcr.*
from
recordrequirements rr
left outer join tenantdepartments td on td.id = rr.TenantDepartmentId
left outer join tenantdepartmentusers tdu on tdu.TenantDepartmentId = td.id and tdu.UserId = @userid
-- left outer join cohorts co2 on co2.TenantDepartmentId = td.id
-- left outer join cohortusers cu2 on cu2.CohortId = co2.id and cu2.UserId = @userid
left outer join cohorts co on (co.id = rr.cohortid or co.TenantDepartmentId = rr.TenantDepartmentId)
left outer join cohortusers cu on cu.CohortId = co.id and cu.UserId = @userid
left outer join recordcategories rc on rc.RecordRequirementId = rr.id
left outer join recordcategoryrules rcr on rcr.id = rc.RecordCategoryRuleId
where
rr.isdeleted = 0 and rr.IsSurpathOnly = 0 and
-- cu.id = @cohortuserid and
rr.TenantId = @tenantid  
-- ( cu.id is not null or (rr.CohortId is null and rr.TenantDepartmentId is null))
and (
    (rr.cohortid is null and rr.TenantDepartmentId is null) -- a requirement that applies to no cohort and no dept should match all cohorts
	or (rr.CohortId is not null and rr.TenantDepartmentId is not null and co.TenantDepartmentId=rr.TenantDepartmentId and cu.id is not null) -- a requirement that applies to a dept and a cohort must match both
    or (rr.CohortId is not null and rr.TenantDepartmentId is null and cu.id is not null) -- a requirement that applies to a cohort should match that cohort regardless of dept
    or (rr.cohortid is null and rr.TenantDepartmentId is not null and co.TenantDepartmentId=rr.TenantDepartmentId and cu.id is not null) -- a requirement that applies to a dept should match and any cohort with that dept id
    -- or 
    
)
-- (
-- (rr.CohortId is not null and cu.id is not null and rr.CohortId = co.id and rr.TenantDepartmentId = co.TenantDepartmentId) or -- a requirement that applies to a dept and a cohort must match both
-- -- a requirement that applies to a dept should match and any cohort with that dept id
-- -- a requirement that applies to a cohort should match that cohort regardless of dept
-- (rr.CohortId is null and rr.TenantDepartmentId is null) -- a requirement that applies to no cohort and no dept should match all cohorts
-- )
;