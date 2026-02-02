set @tenantid = 11;

select 
rr.*, rc.* 
from recordcategories rc
left outer join recordrequirements rr on rr.id = rc.RecordRequirementId
where rc.tenantid = @tenantid and rc.isdeleted = 0;

select * from recordcategories where id in (
'08db07f2-a0d2-4892-8395-ae6f7654fbf0',
'08db07f2-a0d2-497d-8594-0a1e4b77b644',
-- '08db07f2-a0d2-49b2-8213-5dfc768a0a72',
-- '08db07f2-a0d2-49dd-8973-71759f7ff307',
'08db0906-5e1f-491f-8d9c-5ccd4fb73536',
'08db0906-5e1f-4cf5-8561-5669660b1c0e'
);

-- update recordcategories set isdeleted=1 where id in (
-- '08db07f2-a0d2-4892-8395-ae6f7654fbf0',
-- '08db07f2-a0d2-497d-8594-0a1e4b77b644',
-- -- '08db07f2-a0d2-49b2-8213-5dfc768a0a72',
-- -- '08db07f2-a0d2-49dd-8973-71759f7ff307',
-- '08db0906-5e1f-491f-8d9c-5ccd4fb73536',
-- '08db0906-5e1f-4cf5-8561-5669660b1c0e'
-- );
