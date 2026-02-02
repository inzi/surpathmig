set @tenantid = 6;

select * from abptenants where id = @tenantid;

set @drugss = (select id from surpathservices where name like '%drug%');
set @backgroundss = (select id from surpathservices where name like '%background%');

select @drugss, @backgroundss;
-- # @drugss, @backgroundss
-- '08dae8d6-d5fc-4a9c-8ee9-a3518c27e4b4', '08dae8d6-e700-438d-8aba-d9fcd45002e1'

set @bcid = (select id from recordrequirements where TenantId = @tenantid and IsSurpathOnly =1
and name like '%background%' and IsDeleted =0 and name not like '%OCCC%' limit 1);

set @dtid = (select id from recordrequirements where TenantId = @tenantid and IsSurpathOnly =1
and name like '%drug%' and IsDeleted =0 and name not like '%OCCC%' limit 1);

select @bcid, @dtid;

set @tenantdrugcatid = (select c.id from recordcategories c 
left outer join recordrequirements r on c.RecordRequirementId = r.id
where c.TenantId=@tenantid and c.IsDeleted = 0 and r.Id = @dtid and c.name like '%drug%' and c.name not like '%OCCC%' limit 1);

set @tenantbackgroundcatid = (select c.id from recordcategories c 
left outer join recordrequirements r on c.RecordRequirementId = r.id
where c.TenantId=@tenantid and c.IsDeleted = 0 and r.id = @bcid and c.name like '%background%' and c.name not like '%OCCC%' limit 1);

select @tenantdrugcatid, @tenantbackgroundcatid;

set @tenantssbackgroundid = (select t.id from tenantsurpathservices t
where t.TenantId = @tenantid and t.name like '%background%' and t.name not like '%OCCC%' and t.IsDeleted = 0);

set @tenantssdrugid = (select t.id from tenantsurpathservices t
where t.TenantId = @tenantid and t.name like '%drug%' and t.name not like '%OCCC%' and t.IsDeleted = 0);

select @tenantssdrugid, @tenantssbackgroundid;


-- set the correct SPS id and tenant id for drug
-----
update ledgerentrydetails l set l.TenantSurpathServiceId = @tenantssdrugid where l.SurpathServiceId = @drugss;
update ledgerentrydetails l set l.TenantSurpathServiceId = @tenantssbackgroundid where l.SurpathServiceId = @backgroundss;

-- update the record requirements to point to proper tenant SPS
-----
update recordrequirements r set r.TenantSurpathServiceId = @tenantssdrugid 
where r.TenantId = @tenantid and r.SurpathServiceId = @drugss;
update recordrequirements r set r.TenantSurpathServiceId = @tenantssbackgroundid 
where r.TenantId = @tenantid and r.SurpathServiceId = @backgroundss;

-- select c.* from recordcategories c
-- left outer join recordrequirements r on r.id = c.RecordRequirementId
-- where c.TenantId = @tenantid and r.IsSurpathOnly = 1;

-- set the correct recordcategory requirement id for services
-----
update recordcategories c
set c.RecordRequirementId = @dtid
where c.TenantId = @tenantid and c.Name like '%Drug%';
update recordcategories c
set c.RecordRequirementId = @bcid
where c.TenantId = @tenantid and c.Name like '%Background%';

update recordcategories c 
set isdeleted = 1
where c.RecordRequirementId = @dtid and c.Id != @tenantdrugcatid and tenantid = @tenantid;
update recordcategories c 
set isdeleted = 1
where c.RecordRequirementId = @bcid and c.Id != @tenantbackgroundcatid and tenantid = @tenantid;

-- set the correct category for record states
-- we want to set category to the tenants categoryids

update recordstates rs
set RecordCategoryId = @tenantdrugcatid
where rs.TenantId = @tenantid and rs.RecordCategoryId in 
(
select c.id from recordcategories c 
left outer join recordrequirements r on c.RecordRequirementId = r.id
where c.TenantId=@tenantid and r.IsSurpathOnly = 1 and c.name like '%drug%' -- and c.name like '%OCCC%'
);
update recordstates rs
set RecordCategoryId = @tenantbackgroundcatid
where rs.TenantId = @tenantid and rs.RecordCategoryId in 
(
select c.id from recordcategories c 
left outer join recordrequirements r on c.RecordRequirementId = r.id
where c.TenantId=@tenantid and r.IsSurpathOnly = 1 and c.name like '%background%' -- and c.name like '%OCCC%'
);

update recordcategories c
left outer join recordrequirements r on c.RecordRequirementId = r.id
set c.isDeleted = 1
where c.TenantId = @tenantid and r.IsSurpathOnly = 1 and c.name like '%drug%' and r.id != @dtid;

update recordcategories c
left outer join recordrequirements r on c.RecordRequirementId = r.id
set c.isDeleted = 1
where c.TenantId = @tenantid and r.IsSurpathOnly = 1 and c.name like '%background%' and r.id != @bcid;

select c.* from recordcategories c
left outer join recordrequirements r on c.RecordRequirementId = r.id
where c.TenantId = @tenantid and r.IsSurpathOnly = 1;

update tenantsurpathservices t
set t.IsDeleted = 1, t.isEnabled = 0
where t.TenantId = @tenantId and t.name like '%OCCC%';

select t.* from tenantsurpathservices t
where t.TenantId = @tenantId;

update recordrequirements r 
set r.IsDeleted = 1
where r.TenantId=@tenantid and r.IsSurpathOnly = 1 and r.name like '%drug%' and r.id != @dtid;
update recordrequirements r 
set r.IsDeleted = 1
where r.TenantId=@tenantid and r.IsSurpathOnly = 1 and r.name like '%background%' and r.id != @bcid;

select * from recordrequirements r
where r.TenantId=@tenantid;

-- select rs.* from recordstates rs 
-- left outer join recordcategories c on rs.RecordCategoryId = c.id
-- where rs.TenantId = @tenantid;

-- select * from recordstates rs
-- where rs.TenantId = @tenantid and rs.RecordCategoryId in 
-- (
-- select c.id from recordcategories c 
-- left outer join recordrequirements r on c.RecordRequirementId = r.id
-- where c.TenantId=@tenantid and r.IsSurpathOnly = 1 and c.name like '%drug%' and c.name like '%OCCC%'
-- );
-- select * from recordstates rs
-- where rs.TenantId = @tenantid and rs.RecordCategoryId in 
-- (
-- select c.id from recordcategories c 
-- left outer join recordrequirements r on c.RecordRequirementId = r.id
-- where c.TenantId=@tenantid and r.IsSurpathOnly = 1 and c.name like '%background%' and c.name like '%OCCC%'
-- );
