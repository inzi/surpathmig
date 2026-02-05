set @tenantid = 7;

select t.name,r.id, c.id, rcr.id, st.id, c.name, u.name, u.Surname, st.*, s.* -- c.* -- r.*, c.*
from recordrequirements r 
left outer join abptenants t on t.id = r.TenantId
left outer join recordcategories c on c.RecordRequirementId = r.id
left outer join recordcategoryrules rcr on rcr.id = c.RecordCategoryRuleId
left outer join recordstates s on t.id = s.TenantId and s.RecordCategoryId = c.id
left outer join recordstatuses st on s.RecordStatusId = st.id
left outer join cohortusers cu on s.UserId = cu.UserId
left outer join abpusers u on cu.UserId = u.id
where r.IsSurpathOnly = 1
and t.id = @tenantid
-- and u.Surname like '%Cobb%'-- 
and c.name like '%background%';

select r.id, c.id, rcr.*
from recordcategoryrules rcr 
left outer join recordcategories c on c.RecordCategoryRuleId = rcr.id
left outer join recordrequirements r on r.id = c.RecordRequirementId
where r.TenantId = @tenantid 
-- and c.IsDeleted = 0
and r.IsDeleted = 0
and c.IsDeleted = 0
and rcr.IsDeleted =0;

select * from recordcategories where TenantId = @tenantid 
order by name; 

select * from recordrequirements where TenantId = @tenantid 
order by name; 
