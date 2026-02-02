ok - 

SurpathServices and TenantSurpathServices are basically identical except a tenant version points to root

a surpath service is tied to a feature
a tenantsurpath service is a copy of a root service where the user can edit pricing, etc.

The TSS (Tenant Surpath Services) is what is associated with the requirement, etc.
Services are always required

A service of the same type should not be allowed

---
sql fix

 ALTER TABLE `surpathv2`.`recordrequirements` 
DROP INDEX `IX_RecordRequirements_TenantSurpathServiceId`;

ALTER TABLE `surpathv2`.`recordrequirements` 
DROP COLUMN `TenantSurpathServiceId`;

update tenantsurpathservices tss
left outer join surpathservices ss on ss.id = tss.surpathserviceid
set tss.name = ss.name, tss.description = ss.description;

-- change the name of the tenant ss to match ss

-- update tss to point to tss as well for drug

update
recordrequirements r
set r.TenantSurpathServiceId = (select id from tenantsurpathservices where tenantid = r.TenantId and name like '%drug%'), 
SurpathServiceId = (select SurpathServiceId from tenantsurpathservices where tenantid = r.TenantId and name like '%drug%')
where r.IsSurpathOnly = 1 and r.name like '%drug%';
-- update tss to point to tss as well for background
update
recordrequirements r
set r.TenantSurpathServiceId = (select id from tenantsurpathservices where tenantid = r.TenantId and name like '%background%'),
r.SurpathServiceId = (select SurpathServiceId from tenantsurpathservices where tenantid = r.TenantId and name like '%background%')
where r.IsSurpathOnly = 1 and r.name like '%background%';

