-- select * from donors d
-- inner join donor_test_info dti on d.donor_id = dti.donor_id
-- order by d.donor_id 
-- desc limit 50;


select d.donor_id, ip.backend_integration_partners_pidtype, cd.lab_code from donors d
inner join donor_test_info dti on d.donor_id = dti.donor_id
inner join backend_integration_partner_client_map cm on cm.client_department_id = dti.client_department_id
inner join backend_integration_partners ip on cm.backend_integration_partner_id = ip.backend_integration_partner_id
inner join client_departments cd on cd.client_department_id = dti.client_department_id
where
cm.backend_integration_partner_client_map_id is not null
and 
dti.test_status = 4;


update donors d
inner join donor_test_info dti on d.donor_id = dti.donor_id
inner join backend_integration_partner_client_map cm on cm.client_department_id = dti.client_department_id
inner join backend_integration_partners ip on cm.backend_integration_partner_id = ip.backend_integration_partner_id
inner join client_departments cd on cd.client_department_id = dti.client_department_id
set dti.test_status = 6
where
cm.backend_integration_partner_client_map_id is not null
and dti.test_status = 4;


select 
d.donor_id, dti.*
-- , ip.backend_integration_partners_pidtype
-- , cd.lab_code 
from donors d
inner join donor_test_info dti on d.donor_id = dti.donor_id
-- inner join backend_integration_partner_client_map cm on cm.client_department_id = dti.client_department_id
-- inner join backend_integration_partners ip on cm.backend_integration_partner_id = ip.backend_integration_partner_id
-- inner join client_departments cd on cd.client_department_id = dti.client_department_id
where
-- cm.backend_integration_partner_client_map_id is not null
-- and 
-- dti.test_status = 4
-- and 
dti.donor_id = 95461;

update donor_test_info set test_status = 4 where donor_id = 95461;
delete from report_info where donor_test_info_id = 113977;
delete from backend_integration_partner_release where donor_test_info_id = 113977;
delete from donor_documents where donor_id = 95461;

select * from backend_integration_partners;

-- delete from donor_test_info where donor_test_info_id = 113980;

select * from individual_pids where donor_id = 95461;

   SELECT p.*
   FROM backend_integration_partners p
   inner join backend_integration_partner_client_map bipm on p.backend_integration_partner_id = bipm.backend_integration_partner_id
   WHERE bipm.partner_client_code = 'pcclientcode1';

select *
from backend_integration_partner_client_map bipm
Where
 bipm.partner_client_code = 'pcclientcode1';
 
 
 select * from donor_test_info_test_categories where donor_test_info_id = 113977;
 
 
 
 select * from donor_test_info_test_categories order by donor_test_info_id desc, test_category_id limit 500;