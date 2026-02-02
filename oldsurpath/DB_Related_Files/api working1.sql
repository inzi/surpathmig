-- select * from client_departments where lab_code like '%Surscan';

-- delete from client_departments where client_department_id = 391;

-- select d.donor_id, ip.backend_integration_partners_pidtype, cd.lab_code
-- from donors d
-- inner join donor_test_info dti on d.donor_id = dti.donor_id
-- inner join backend_integration_partner_client_map cm on cm.client_department_id = dti.client_department_id
-- inner join backend_integration_partners ip on cm.backend_integration_partner_id = ip.backend_integration_partner_id
-- inner join client_departments cd on cd.client_department_id = dti.client_department_id
-- where
-- cm.backend_integration_partner_client_map_id is not null
-- and dti.test_status = 4
-- 
-- 
select d.donor_id, ri.*, dti.test_overall_result
from donors d
inner join donor_test_info dti on d.donor_id = dti.donor_id
inner join report_info ri on ri.donor_test_info_id = dti.donor_test_info_id
-- inner join backend_integration_partner_client_map cm on cm.client_department_id = dti.client_department_id
-- inner join backend_integration_partners ip on cm.backend_integration_partner_id = ip.backend_integration_partner_id
inner join client_departments cd on cd.client_department_id = dti.client_department_id
where
dti.test_status = 7
-- and cm.backend_integration_partner_client_map_id is not null
and dti.test_overall_result = 1
and ri.report_type = 2 -- MRO
and ri.is_archived = 0 
limit 50;