set @tenantid = 11;

select distinct
rr.*
, rc.*
-- rr.id, rr.name, rr.Description, rr.cohortid, rr.TenantDepartmentId
-- ,td.id as 'Deptid' ,td.Name as 'DeptName'
-- ,co.id as 'Cohortid' ,co.Name as 'CohortName', co.TenantDepartmentId as 'cohortdeptid'
-- ,cu.id as 'CohortUserid', @userid as 'UserId'
-- ,rc.id as 'CatId', rc.Name as 'CatName'
,rcr.*
from
recordrequirements rr
left outer join tenantdepartments td on td.id = rr.TenantDepartmentId
left outer join tenantdepartmentusers tdu on tdu.TenantDepartmentId = td.id and tdu.UserId = @userid
-- left outer join cohorts co2 on co2.TenantDepartmentId = td.id
-- left outer join cohortusers cu2 on cu2.CohortId = co2.id and cu2.UserId = @userid
left outer join cohorts co on (co.id = rr.cohortid or co.TenantDepartmentId = rr.TenantDepartmentId)
-- left outer join cohortusers cu on cu.CohortId = co.id and cu.UserId = @userid
left outer join recordcategories rc on rc.RecordRequirementId = rr.id
left outer join recordcategoryrules rcr on rcr.id = rc.RecordCategoryRuleId
left outer join recordstates rs on rs.recordcategoryid = rc.id and rs.userId = @userid
-- left outer join recordstatuses rst on rst.id = rs.recordstatusid
where rr.isdeleted = 0 and 
rc.isdeleted = 0 and
-- rr.IsSurpathOnly = 0 and
-- cu.id = @cohortuserid and
rr.TenantId = @tenantid  
-- ( cu.id is not null or (rr.CohortId is null and rr.TenantDepartmentId is null))
and (
    (rr.cohortid is null and rr.TenantDepartmentId is null) -- a requirement that applies to no cohort and no dept should match all cohorts
	or (rr.CohortId is not null and rr.TenantDepartmentId is not null and co.TenantDepartmentId=rr.TenantDepartmentId) -- a requirement that applies to a dept and a cohort must match both
    or (rr.CohortId is not null and rr.TenantDepartmentId is null) -- a requirement that applies to a cohort should match that cohort regardless of dept
    or (rr.cohortid is null and rr.TenantDepartmentId is not null and co.TenantDepartmentId=rr.TenantDepartmentId) -- a requirement that applies to a dept should match and any cohort with that dept id
);