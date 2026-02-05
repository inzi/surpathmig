-- select * from backend_integration_partner_client_map;

select d.donor_id, ip.backend_integration_partners_pidtype, cd.lab_code from donors d
inner join donor_test_info dti on d.donor_id = dti.donor_id
inner join backend_integration_partner_client_map cm on cm.client_department_id = dti.client_department_id
inner join backend_integration_partners ip on cm.backend_integration_partner_id = ip.backend_integration_partner_id
inner join client_departments cd on cd.client_department_id = dti.client_department_id
where
cm.backend_integration_partner_client_map_id is not null
and dti.test_status = 4
-- dti.client_id = 110
-- and
-- dti.client_department_id = 389
-- dti.test_status = 2;